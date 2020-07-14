using Microsoft.Xna.Framework;
using Penumbra;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Physics;
using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using SecretProject.Class.TileStuff.SpawnStuff;
using SecretProject.Class.TileStuff.TileModifications;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using TiledSharp;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using XMLData.ItemStuff;

namespace SecretProject.Class.TileStuff
{

    public static class TileUtility
    {
        public static int ChunkWidth = 16;
        public static int ChunkHeight = 16;



        #region GETRECTANGLES
        public static Rectangle GetDestinationRectangle(Tile tile)
        {

            float X = (tile.X * 16);
            float Y = (tile.Y * 16);

            return new Rectangle((int)(X), (int)(Y), 16, 16);


        }
        public static Rectangle GetSourceRectangle(Tile tile, int tilesetTilesWide)
        {

            int Column = tile.GID % tilesetTilesWide;
            int Row = (int)Math.Floor((double)tile.GID / (double)tilesetTilesWide);

            return new Rectangle(16 * Column, 16 * Row, 16, 16);
        }
        public static Rectangle GetSourceRectangleWithoutTile(int gid, int tilesetTilesWide)
        {

            int Column = gid % tilesetTilesWide;
            int Row = (int)Math.Floor((double)gid / (double)tilesetTilesWide);

            return new Rectangle(16 * Column, 16 * Row, 16, 16);
        }
        public static int[] GetRectangeFromString(string info)
        {
            int[] numsToReturn = new int[4];
            numsToReturn[0] = int.Parse(info.Split(',')[0]);
            numsToReturn[1] = int.Parse(info.Split(',')[1]);
            numsToReturn[2] = int.Parse(info.Split(',')[2]);
            numsToReturn[3] = int.Parse(info.Split(',')[3]);

            return numsToReturn;
        }

        public static Rectangle GetTileRectangleFromProperty(Tile tile, Dictionary<int, TmxTilesetTile> tileSet, bool adjustDestinationRectangle, TileManager TileManager = null, int gid = 0)
        {
            int newGID = gid;

            int[] rectangleCoords = GetRectangeFromString(tileSet[newGID].Properties["newSource"]);

            Rectangle originalRectangle = GetSourceRectangleWithoutTile(gid, 100);



            if (adjustDestinationRectangle)
            {
                tile.DestinationRectangle = new Rectangle(tile.DestinationRectangle.X + rectangleCoords[0], tile.DestinationRectangle.Y + rectangleCoords[1],
               rectangleCoords[2], rectangleCoords[3]);
            }


            return new Rectangle(originalRectangle.X + rectangleCoords[0], originalRectangle.Y + rectangleCoords[1],
                rectangleCoords[2], rectangleCoords[3]);

        }
        /// <summary>
        /// Gets tiles world pos / 16.
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public static int GetSquareTileCoord(int worldPos)
        {
            int result = (int)Math.Floor((float)((float)worldPos / 16f));
            return result;
        }


        #endregion



        public static bool GetProperty(Dictionary<int, TmxTilesetTile> tileSet, int tileGID, ref string property)
        {

            return tileSet[tileGID].Properties.TryGetValue(property, out property);

        }

        /// <summary>
        /// All new tiles should be "wrung through" this method. Makes sure all dictionaries etc will be aware of this new tile. Also adds storable items to 
        /// user interface list
        /// </summary>
        /// <param name="tileToAssign"></param>
        /// <param name="layer"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="TileManager"></param>
        public static void AssignProperties(Tile tileToAssign, int layer, int x, int y, TileManager TileManager)
        {
            Dictionary<int, TmxTilesetTile> tileSet = TileManager.MapName.Tilesets[TileManager.TileSetNumber].Tiles;

            tileToAssign.DestinationRectangle = GetDestinationRectangle(tileToAssign);
            tileToAssign.SourceRectangle = GetSourceRectangle(tileToAssign, TileManager.TileSetDimension);
            tileToAssign.TileKey = tileToAssign.GetTileKeyString(layer, TileManager);
            string propertyString;
            tileToAssign.Position = new Vector2(tileToAssign.DestinationRectangle.X, tileToAssign.DestinationRectangle.Y);
            if (tileSet.ContainsKey(tileToAssign.GID))
            {
                //replaces tiles with wheat grass

                propertyString = "replace";
                if (GetProperty(tileSet, tileToAssign.GID, ref propertyString))
                {



                    int grasstype = int.Parse(propertyString);
                    GrassTuft grassTuft = new GrassTuft(TileManager.GraphicsDevice, grasstype, new Vector2(tileToAssign.DestinationRectangle.X + 8
                                , tileToAssign.DestinationRectangle.Y + 8), TileManager.Stage);
                    List<GrassTuft> tufts = new List<GrassTuft>()
                    {
                        grassTuft,
                    };
                    if (!TileManager.Tufts.ContainsKey(tileToAssign.TileKey))
                    {
                        TileManager.Tufts.Add(tileToAssign.TileKey, tufts);
                        ReplaceTile(layer, x, y, 0, TileManager);
                    }

                    return;
                }
                propertyString = "portal";
                if (GetProperty(tileSet, tileToAssign.GID, ref propertyString))
                {
                    string[] portalString = propertyString.Split(',');
                    bool clickRequired = bool.Parse(portalString[0]);
                    int from = (int)Enum.Parse(typeof(Stages), portalString[1]);
                    int to = (int)Enum.Parse(typeof(Stages), portalString[2]);
                    Portal portal = new Portal(from, to, 0, -32, clickRequired);
                    if (!Game1.PortalGraph.HasEdge((Stages)portal.From, (Stages)portal.To))
                    {
                        Game1.PortalGraph.AddEdge((Stages)portal.From, (Stages)portal.To);
                    }
                    portal.PortalStart = new Rectangle(tileToAssign.DestinationRectangle.X, tileToAssign.DestinationRectangle.Y + 32, tileToAssign.DestinationRectangle.Width, tileToAssign.DestinationRectangle.Height);
                    TileManager.Stage.AllPortals.Add(portal);
                }


                if (tileSet[tileToAssign.GID].AnimationFrames.Count > 0 && !tileSet[tileToAssign.GID].Properties.ContainsKey("idleStart"))
                {

                    if (!TileManager.AnimationFrames.ContainsKey(tileToAssign.TileKey))
                    {


                        List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();



                        frames.Add(new EditableAnimationFrame(tileSet[tileToAssign.GID].AnimationFrames[0].Duration, tileToAssign.GID));
                        for (int i = 0; i < tileSet[tileToAssign.GID].AnimationFrames.Count; i++)
                        {
                            frames.Add(new EditableAnimationFrame(tileSet[tileToAssign.GID].AnimationFrames[i]));
                        }
                        bool hasNewSource = false;
                        EditableAnimationFrameHolder frameHolder;
                        propertyString = "newSource";
                        if (GetProperty(tileSet, tileToAssign.GID, ref propertyString))
                        {



                            hasNewSource = true;
                            int[] nums = GetRectangeFromString(propertyString);

                            frameHolder = new EditableAnimationFrameHolder(frames, x, y, layer, tileToAssign.GID, hasNewSource: hasNewSource)
                            {
                                OriginalXOffSet = nums[0],
                                OriginalYOffSet = nums[1],
                                OriginalWidth = nums[2],
                                OriginalHeight = nums[3]
                            };

                        }
                        else
                        {
                            frameHolder = new EditableAnimationFrameHolder(frames, x, y, layer, tileToAssign.GID, hasNewSource: hasNewSource);
                        }


                        TileManager.AnimationFrames.Add(tileToAssign.TileKey, frameHolder);
                    }

                }
                propertyString = "lightSource";
                if (GetProperty(tileSet, tileToAssign.GID, ref propertyString))
                {

                    if (!TileManager.Stage.Lights.ContainsKey(tileToAssign.TileKey))
                    {
                        Vector2 lightOffSet = LightSource.ParseLightData(propertyString);
                        Color lightColor = LightSource.GetLightType(propertyString);

                        PointLight pointLight = new PointLight()
                        {
                            Position = new Vector2(GetDestinationRectangle(tileToAssign).X + lightOffSet.X, GetDestinationRectangle(tileToAssign).Y + lightOffSet.Y),
                            Scale = new Vector2(400),
                            ShadowType = ShadowType.Solid,

                            Color = lightColor,


                        };
                        TileManager.Stage.Lights.Add(tileToAssign.TileKey, pointLight);
                        TileManager.Stage.Penumbra.Lights.Add(pointLight);
                    }



                }


                propertyString = "destructable";
                if (GetProperty(tileSet, tileToAssign.GID, ref propertyString))
                {
                    TileManager.TileHitPoints[tileToAssign.TileKey] = Game1.Utility.GetTileHitpoints(propertyString);
                }

                propertyString = "layer";
                if (GetProperty(tileSet, tileToAssign.GID, ref propertyString))
                {
                    tileToAssign.LayerToDrawAt = int.Parse(propertyString);
                }


                //grass = 1, stone = 2, wood = 3, sand = 4

                propertyString = "generate";
                if (GetProperty(tileSet, tileToAssign.GID, ref propertyString))
                {
                    tileToAssign.GenerationType = (GenerationType)Enum.Parse(typeof(GenerationType), propertyString);
                }


                //grass = 1, stone = 2, wood = 3, sand = 4

                propertyString = "action";
                if (GetProperty(tileSet, tileToAssign.GID, ref propertyString))
                {

                    if (propertyString == "chestLoot")
                    {

                        if (!TileManager.StoreableItems.ContainsKey(tileToAssign.TileKey))
                        {
                            TileManager.StoreableItems.Add(tileToAssign.TileKey, new Chest(tileToAssign.TileKey, 6,
                                    new Vector2(GetDestinationRectangle(tileToAssign).X, GetDestinationRectangle(tileToAssign).Y), TileManager.GraphicsDevice, false));
                        }

                    }
                    else if (propertyString == "cook")
                    {

                        if (!TileManager.StoreableItems.ContainsKey(tileToAssign.TileKey))
                        {
                            TileManager.StoreableItems.Add(tileToAssign.TileKey, new Cauldron(tileToAssign.TileKey, 3,
                                    new Vector2(GetDestinationRectangle(tileToAssign).X, GetDestinationRectangle(tileToAssign).Y), TileManager.GraphicsDevice));
                        }

                    }
                    else if (propertyString == "smelt")
                    {

                        if (!TileManager.StoreableItems.ContainsKey(tileToAssign.TileKey))
                        {
                            TileManager.StoreableItems.Add(tileToAssign.TileKey, new Furnace(tileToAssign.TileKey, 3,
                                    new Vector2(GetDestinationRectangle(tileToAssign).X, GetDestinationRectangle(tileToAssign).Y), TileManager.GraphicsDevice));
                        }

                    }
                    else if (propertyString == "saw")
                    {

                        if (!TileManager.StoreableItems.ContainsKey(tileToAssign.TileKey))
                        {
                            TileManager.StoreableItems.Add(tileToAssign.TileKey, new SawTable(tileToAssign.TileKey, 3,
                                    new Vector2(GetDestinationRectangle(tileToAssign).X, GetDestinationRectangle(tileToAssign).Y), TileManager.GraphicsDevice));
                        }

                    }

                }
                propertyString = "newSource";

                if (GetProperty(tileSet, tileToAssign.GID, ref propertyString))
                {
                    tileToAssign.SourceRectangle = GetTileRectangleFromProperty(tileToAssign, tileSet, true, TileManager, tileToAssign.GID);
                }

                if (layer >= 3)
                {
                    float randomOffSet = Game1.Utility.RFloat(Utility.LayerOffSetMultiplier, Utility.LayerOffSetMultiplier * 10);
                    float offSetDrawValue = (GetDestinationRectangle(tileToAssign).Y + GetDestinationRectangle(tileToAssign).Height) * Utility.ForeGroundMultiplier + randomOffSet;
                    if (x > 0 && TileManager.AllTiles[layer][x - 1, y].LayerToDrawAtZOffSet == offSetDrawValue)
                    {
                        offSetDrawValue += randomOffSet;
                    }
                    tileToAssign.LayerToDrawAtZOffSet = offSetDrawValue;



                }

                propertyString = "newHitBox";
                if (GetProperty(tileSet, tileToAssign.GID, ref propertyString))
                {
                    int[] rectangleCoords = GetRectangeFromString(propertyString);
                    Rectangle tileDestinationRectangle = new Rectangle(GetDestinationRectangle(tileToAssign).X + rectangleCoords[0],
                            GetDestinationRectangle(tileToAssign).Y + rectangleCoords[1], rectangleCoords[2],
                           rectangleCoords[3]);

                    Body collisionBody = BodyFactory.CreateRectangle(Game1.VelcroWorld, tileDestinationRectangle.Width, tileDestinationRectangle.Height,
                        .5f, new Vector2(tileDestinationRectangle.X + tileDestinationRectangle.Width / 2, tileDestinationRectangle.Y + tileDestinationRectangle.Height / 2), 0f, BodyType.Static);
                    collisionBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Solid;
                    collisionBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Player | VelcroPhysics.Collision.Filtering.Category.Item;

                    if (TileManager.Objects.ContainsKey(tileToAssign.TileKey))
                    {
                        TileManager.Objects[tileToAssign.TileKey].Add(collisionBody);
                    }
                    else
                    {
                        TileManager.Objects.Add(tileToAssign.TileKey, new List<Body>() { collisionBody });
                    }

                    propertyString = "lightIgnores"; //do not add hull if this object should not obstruct light.
                    if (!GetProperty(tileSet, tileToAssign.GID, ref propertyString))
                    {
                        if (!TileManager.Stage.Hulls.ContainsKey(tileToAssign.TileKey))
                        {
                            Hull hull = Hull.CreateRectangle(new Vector2(tileDestinationRectangle.X + (float)tileDestinationRectangle.Width / 2,
                            tileDestinationRectangle.Y + (float)tileDestinationRectangle.Height / 2),
                            new Vector2((float)tileDestinationRectangle.Width, (float)tileDestinationRectangle.Height));
                            TileManager.Stage.Hulls.Add(tileToAssign.TileKey, hull);
                            TileManager.Stage.Penumbra.Hulls.Add(hull);
                        }

                    }
                    if (TileManager.Type == 0)
                    {
                        int startI = rectangleCoords[0] / 16;
                        int endI = rectangleCoords[2] / 16;
                        endI += startI;

                        int startJ = rectangleCoords[1] / 16;
                        int endJ = rectangleCoords[3] / 16;
                        endJ = startJ + endJ;
                        for (int i = startI; i < endI; i++)
                        {
                            for (int j = startJ; j < endJ; j++)
                            {
                                TileManager.PathGrid.UpdateGrid(x + i, y + j, 0);
                            }
                        }
                    }


                }

                propertyString = "addQuest";
                if (GetProperty(tileSet, tileToAssign.GID, ref propertyString))
                {
                    Game1.Player.UserInterface.WorldQuestMenu.AddSpriteToDictionary(TileManager.QuestIcons, TileManager, tileToAssign);
                }
                if (tileSet[tileToAssign.GID].ObjectGroups.Count > 0)
                {

                    TileManager.PathGrid.UpdateGrid(x, y, 0);

                    for (int k = 0; k < tileSet[tileToAssign.GID].ObjectGroups[0].Objects.Count; k++)
                    {
                        TmxObject tempObj = tileSet[tileToAssign.GID].ObjectGroups[0].Objects[k];
                        Rectangle rectangle = new Rectangle(GetDestinationRectangle(tileToAssign).X + (int)Math.Ceiling(tempObj.X),
                            GetDestinationRectangle(tileToAssign).Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                            (int)Math.Ceiling(tempObj.Height));

                        List<Body> bodies = new List<Body>();
                        if (tempObj.ObjectType == TmxObjectType.Ellipse)
                        {
                            Circle circle = new Circle(new Vector2((float)(rectangle.X + tempObj.Width / 2), (float)(rectangle.Y + tempObj.Height / 2)), (float)(tempObj.Width));



                            Body collisionBody = BodyFactory.CreateCircle(Game1.VelcroWorld, circle.Radius / 2, .5f, circle.Center, BodyType.Static);
                            collisionBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Solid;
                            collisionBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Player | VelcroPhysics.Collision.Filtering.Category.Item;
                            collisionBody.IgnoreGravity = true;
                            TileManager.Stage.DebuggableShapes.Add(new CircleDebugger(collisionBody, TileManager.Stage.DebuggableShapes));
                            bodies.Add(collisionBody);
                            if (!TileManager.Objects.ContainsKey(tileToAssign.TileKey))
                            {
                                TileManager.Objects.Add(tileToAssign.TileKey, bodies);
                            }
                            else
                            {
                                TileManager.Objects[tileToAssign.TileKey].Add(collisionBody);
                            }
 
                        }
                        else
                        {
                            Body collisionBody = BodyFactory.CreateRectangle(Game1.VelcroWorld, rectangle.Width, rectangle.Height,
                                .5f, new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2), 0f, BodyType.Static);
                            collisionBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Solid;
                            collisionBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Player | VelcroPhysics.Collision.Filtering.Category.Item;
                            bodies.Add(collisionBody);

                            if (TileManager.Objects.ContainsKey(tileToAssign.TileKey))
                            {
                                TileManager.Objects[tileToAssign.TileKey].Add(collisionBody);
                            }
                            else
                            {
                                TileManager.Objects.Add(tileToAssign.TileKey, bodies);
                            }
                        }
                        if (layer > (int)MapLayer.MidGround) // only blocks light if over midground layer
                        {

                            propertyString = "lightIgnores"; //do not add hull if this object should not obstruct light.
                            if (!GetProperty(tileSet, tileToAssign.GID, ref propertyString))
                            {
                                if (!TileManager.Stage.Hulls.ContainsKey(tileToAssign.TileKey))
                                {
                                    Hull hull = Hull.CreateRectangle(new Vector2(rectangle.X + (float)tempObj.Width / 2, rectangle.Y + (float)tempObj.Height / 2), new Vector2((float)tempObj.Width, (float)tempObj.Height));
                                    TileManager.Stage.Hulls.Add(tileToAssign.TileKey, hull);
                                    TileManager.Stage.Penumbra.Hulls.Add(hull);
                                }

                            }

                        }

                    }
                }

                propertyString = "transparent";
                if (GetProperty(tileSet, tileToAssign.GID, ref propertyString))
                {


                    int[] nums = GetRectangeFromString(propertyString);
                    Body body = BodyFactory.CreateRectangle(Game1.VelcroWorld, nums[2], nums[3], 1f);
                    body.Position = new Vector2(tileToAssign.Position.X + nums[0] / 2, tileToAssign.Position.Y + nums[1] / 2);
                    body.IsSensor = true;
                    body.IgnoreGravity = true;
                    body.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.TransparencySensor;
                    body.UserData = tileToAssign;

                    if (TileManager.Objects.ContainsKey(tileToAssign.TileKey))
                    {
                        TileManager.Objects[tileToAssign.TileKey].Add(body);
                    }
                    else
                    {
                        TileManager.Objects.Add(tileToAssign.TileKey, new List<Body>()
                        {
                            body
                        });

                    }

                }


                tileToAssign.Position = new Vector2(tileToAssign.DestinationRectangle.X, tileToAssign.DestinationRectangle.Y);
            }

        }


        public static void AddCropToTile(Crop crop, Tile tileToAssign, int x, int y, int layer, TileManager TileManager, bool randomize = false)
        {

            crop.X = x;
            crop.Y = y;
            if (randomize)
            {
                int randomGrowth = Game1.Utility.RNumber(0, 3);
                crop.DayPlanted = Game1.GlobalClock.TotalDays - randomGrowth;
                crop.CurrentGrowth += randomGrowth;
                crop.GID += randomGrowth;
                tileToAssign.GID += randomGrowth + 1;
            }
            else
            {
                crop.DayPlanted = Game1.GlobalClock.TotalDays;
                crop.GID++;
            }


            TileManager.Crops[tileToAssign.TileKey] = crop;




        }

        /// <summary>
        /// For use with the "action" tile property. Does a number of various things depending on what the property is. May change the cursor texture.
        /// </summary>
        /// <param name="z"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="action"></param>
        /// <param name="mouse"></param>
        /// <param name="TileManager"></param>
        public static void ActionHelper(int z, int i, int j, string action, MouseManager mouse, TileManager TileManager)
        {
            //new Gid should be one larger, per usual
            string[] propertyValue = Game1.Utility.GetActionHelperInfo(action);

            switch (propertyValue[0])
            {
                //including animation frame id to replace!
                case "diggable":

                    if (TileManager.AllTiles[1][i, j].GID == -1)
                    {
                        Item item = Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem();
                        if (item != null)
                        {
                            if (item.ItemType == ItemType.Shovel)
                            {
                                Game1.Player.UserInterface.DrawTileSelector = true;

                                mouse.ChangeMouseTexture(CursorType.Digging);

                                if (mouse.IsClicked)
                                {

                                    switch (TileManager.MapName.Tilesets[TileManager.TileSetNumber].Tiles[TileManager.AllTiles[z][i, j].GID].Properties["generate"])
                                    {
                                        case "Dirt":
                                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirt);
                                            TileUtility.ReplaceTile(z, i, j, 87, TileManager);
                                            TileManager.AllItems.Add(Game1.ItemVault.GenerateNewItem(1006, TileManager.AllTiles[z][i, j].GetPosition(TileManager), true, TileManager.AllItems));
                                            break;
                                        case "dirtBasic":
                                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirt);
                                            TileUtility.ReplaceTile(z, i, j, 87, TileManager);
                                            TileManager.AllItems.Add(Game1.ItemVault.GenerateNewItem(1006, TileManager.AllTiles[z][i, j].GetPosition(TileManager), true, TileManager.AllItems));
                                            break;
                                        case "grassBasic":
                                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirt);
                                            TileUtility.ReplaceTile(z, i, j, 1007, TileManager);
                                            int cx = (int)Game1.MouseManager.WorldMousePosition.X;
                                            int cy = (int)Game1.MouseManager.WorldMousePosition.Y;



                                            break;
                                    }
                                    TileManager.WasModifiedDuringInterval = true;
                                }
                            }
                            else if (item.ItemType == ItemType.Tree)
                            {
                                mouse.ChangeMouseTexture(CursorType.Planting);
                                if (mouse.IsClicked)
                                {
                                    if (!TileManager.Crops.ContainsKey(TileManager.AllTiles[3][i, j].GetTileKeyString(3, TileManager)))
                                    {

                                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirt);
                                        Crop tempCrop = Game1.AllCrops.GetCropFromID(Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ID);

                                        TileUtility.ReplaceTile(3, i, j, tempCrop.GID, TileManager);
                                        AddCropToTile(tempCrop, TileManager.AllTiles[3][i, j], i, j, 3, TileManager);

                                        Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ID);
                                        TileManager.WasModifiedDuringInterval = true;
                                    }
                                }
                            }
                        }
                    }
                    break;

                case "plantable":

                    Game1.Player.UserInterface.DrawTileSelector = true;
                    ItemData itemData = Game1.Player.GetCurrentEquippedToolData();
                    if (itemData != null)
                    {
                        if (itemData.Plantable)
                        {
                            mouse.ChangeMouseTexture(CursorType.Planting);
                            if (mouse.IsClicked)
                            {
                                if (!TileManager.Crops.ContainsKey(TileManager.AllTiles[(int)MapLayer.ForeGround][i, j].GetTileKeyString((int)MapLayer.ForeGround, TileManager)))
                                {

                                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirt);
                                    Crop tempCrop = Game1.AllCrops.GetCropFromID(itemData.ID);

                                    TileUtility.ReplaceTile((int)MapLayer.ForeGround, i, j, tempCrop.GID, TileManager);
                                    AddCropToTile(tempCrop, TileManager.AllTiles[(int)MapLayer.ForeGround][i, j], i, j, (int)MapLayer.ForeGround, TileManager);

                                    Game1.Player.Inventory.RemoveItem(itemData.ID);
                                    TileManager.WasModifiedDuringInterval = true;
                                }
                            }
                        }
                    }

                    break;

                case "sleep":
                    mouse.ChangeMouseTexture(CursorType.Normal);
                    if (mouse.IsClicked)
                    {
                        Action newAction = new Action(Game1.GlobalClock.IncrementDay);
                        Game1.Player.UserInterface.AddAlert(AlertType.Confirmation, Game1.Utility.centerScreen, "Go to sleep?", newAction);

                    }
                    break;

                case "triggerQuest":
                    mouse.ChangeMouseTexture(CursorType.Normal);
                    if (mouse.IsClicked)
                    {

                        Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.WorldQuestMenu;
                        Game1.Player.UserInterface.WorldQuestMenu.LoadQuest(Game1.WorldQuestHolder.RetrieveQuest(TileManager.AllTiles[z][i, j].GID), z, i, j, TileManager);

                    }
                    break;

                case "portal":
                    mouse.ChangeMouseTexture(CursorType.Door);
                    if (mouse.IsClicked)
                    {

                        string propertyString = "portal";
                        if (GetProperty(TileManager.TileSetDictionary, TileManager.AllTiles[z][i, j].GID, ref propertyString))
                        {



                            string[] portalString = propertyString.Split(',');
                            int from = (int)Enum.Parse(typeof(Stages), portalString[1]);
                            int to = (int)Enum.Parse(typeof(Stages), portalString[2]);


                            Portal portal = TileManager.Stage.AllPortals.Find(x => x.To == to && x.From == from);

                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DoorOpen);
                            Game1.SwitchStage(Game1.GetStageFromEnum((Stages)to), portal);
                        }
                    }
                    break;

                case "chestLoot":
                    if (mouse.IsClicked)
                    {
                        //Game1.Player.UserInterface.CurrentAccessedStorableItem = TileManager.StoreableItems[TileManager.AllTiles[z][i, j].GetTileKeyStringNew(z, TileManager)];
                        Game1.Player.UserInterface.SwitchCurrentAccessedStorageItem(TileManager.StoreableItems[TileManager.AllTiles[z][i, j].TileKey]);
                        Game1.Player.UserInterface.CurrentAccessedStorableItem.Activate(TileManager, z, i, j);
                        TileManager.WasModifiedDuringInterval = true;
                    }
                    break;

                case "triggerLift":
                    if (mouse.IsClicked)
                    {
                        //if (Game1.GetCurrentStageInt() == Stages.OverWorld)
                        //{
                        //    foreach (Chunk chunk in Game1.OverWorld.AllTiles.ActiveChunks)
                        //    {
                        //        if (!chunk.AreReadersAndWritersDone)
                        //        {
                        //            return;
                        //        }
                        //    }
                        //    Game1.Player.UserInterface.WarpGate.To = Stages.Town;
                        //}
                        //else if (Game1.GetCurrentStageInt() == Stages.Town)
                        //{
                        //    Game1.Player.UserInterface.WarpGate.To = Stages.OverWorld;
                        //}

                        //Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.WarpGate;

                    }
                    break;



                case "cook":
                    mouse.ChangeMouseTexture(CursorType.Normal);
                    if (mouse.IsClicked)
                    {
                        Game1.Player.UserInterface.SwitchCurrentAccessedStorageItem(TileManager.StoreableItems[TileManager.AllTiles[(int)MapLayer.ForeGround][i, j].TileKey]);
                        Game1.Player.UserInterface.CurrentAccessedStorableItem.Activate(TileManager, z, i, j);
                        TileManager.WasModifiedDuringInterval = true;

                    }
                    break;
                case "smelt":
                    mouse.ChangeMouseTexture(CursorType.Normal);
                    if (mouse.IsClicked)
                    {
                        Game1.Player.UserInterface.SwitchCurrentAccessedStorageItem(TileManager.StoreableItems[TileManager.AllTiles[z][i, j].TileKey]);
                        Game1.Player.UserInterface.CurrentAccessedStorableItem.Activate(TileManager, z, i, j);
                        TileManager.WasModifiedDuringInterval = true;

                    }
                    break;
                case "saw":
                    mouse.ChangeMouseTexture(CursorType.Normal);
                    if (mouse.IsClicked)
                    {
                        Game1.Player.UserInterface.CurrentAccessedStorableItem = TileManager.StoreableItems[TileManager.AllTiles[3][i, j].TileKey];
                        Game1.Player.UserInterface.CurrentAccessedStorableItem.IsUpdating = true;
                        TileManager.WasModifiedDuringInterval = true;

                    }
                    break;
                case "enterBurrow":
                    mouse.ChangeMouseTexture(CursorType.Door);
                    {
                        if (mouse.IsClicked)
                        {

                            // Game1.SwitchStage(Game1.GetCurrentStageInt(), Stages.UnderWorld, null);
                            //Game1.UnderWorld.AllTiles.LoadInitialChunks(Game1.Player.position);
                        }
                    }
                    break;
                case "swapHouse":
                    mouse.ChangeMouseTexture(CursorType.Door);
                    if (mouse.IsClicked)
                    {
                        //Portal portal = Game1.CurrentStage.AllPortals.Find(x => x.From == (int)Stages.PlayerHouse);
                        //string announcementString = string.Empty;
                        //if (portal.To == (int)(Stages.Town))
                        //{
                        //    portal.To = (int)Stages.OverWorld;
                        //    announcementString = "Rai!";
                        //}
                        //else if (portal.To == (int)(Stages.OverWorld))
                        //{
                        //    portal.To = (int)Stages.UnderWorld;
                        //    announcementString = "Underworld!";
                        //}
                        //else if (portal.To == (int)(Stages.UnderWorld))
                        //{
                        //    portal.To = (int)Stages.Town;
                        //    announcementString = "Kai!";
                        //}

                        // Game1.Player.UserInterface.AddAlert(AlertType.Normal, Game1.Utility.centerScreen, "Location set to " + announcementString);
                    }
                    break;
                case "enterPortal":
                    if (mouse.IsClicked)
                    {


                    }
                    break;
                case "fillSand": //if current item is an empty bucket, replace it with a bucket of sand.
                    if (Game1.MouseManager.IsClicked)
                    {
                        if (Game1.Player.GetCurrentToolID() == 320)
                        {
                            Game1.Player.ReplaceCurrentItem(322);
                            ReplaceTile(z, i, j, 5521, TileManager);
                        }
                        else
                        {
                            Game1.Player.UserInterface.AddAlert(AlertType.Normal, Game1.Utility.centerScreen, "I need to be holding a bucket in my hand to fill this!");
                        }
                    }

                    break;

                case "fillDirt":
                    if (Game1.MouseManager.IsClicked)
                    {
                        if (Game1.Player.GetCurrentToolID() == 320)
                        {
                            Game1.Player.ReplaceCurrentItem(321);
                            ReplaceTile(z, i, j, 5521, TileManager);
                        }
                        else
                        {
                            Game1.Player.UserInterface.AddAlert(AlertType.Normal, Game1.Utility.centerScreen, "I need to be holding a bucket in my hand to fill this!");
                        }
                    }
                    break;
                case "fillable": //Place bucket of sand or dirt on the bedrock layer to add dirt or sand to it. Replace held item with empty bucket.
                    if (TileManager.AllTiles[(int)MapLayer.BackGround][i, j].GID == -1)
                    {
                        if (Game1.MouseManager.IsClicked)
                        {
                            int newBackGroundGID = 102;
                            if (Game1.Player.GetCurrentToolID() == 321)
                            {
                                newBackGroundGID = 102; //fertile soil 

                            }
                            else if (Game1.Player.GetCurrentToolID() == 322)
                            {
                                newBackGroundGID = 1401; //sand
                            }
                            else
                            {
                                break;
                            }
                            Game1.Player.ReplaceCurrentItem(320);
                            ReplaceTile((int)MapLayer.BackGround, i, j, newBackGroundGID, TileManager); //dirt
                            TilingTileManager tilingTileManager = Game1.Procedural.GetTilingTileManagerFromGID((GenerationType)newBackGroundGID);
                            WangManager.GroupReassignForTiling(newBackGroundGID, tilingTileManager.GeneratableTiles, tilingTileManager.TilingDictionary, (int)MapLayer.BackGround,
                        i, j, Game1.CurrentStage.AllTiles.MapWidth, Game1.CurrentStage.AllTiles.MapWidth,
                        (TileManager)Game1.CurrentStage.AllTiles);

                        }
                    }

                    break;
                case "examine":
                    if (Game1.MouseManager.IsClicked)
                    {
                        Game1.Player.UserInterface.AddAlert(AlertType.Normal, Game1.Utility.centerScreen, propertyValue[1]);
                    }
                    break;

                case "message":
                    if (Game1.MouseManager.IsClicked)
                    {
                        Game1.Player.UserInterface.AddAlert(AlertType.Normal, Game1.Utility.centerScreen, Game1.MessageHolder.GetMessage(i, j));
                    }
                    break;

            }
        }



        #region TILEREPLACEMENT AND INTERACTIONS
        public static void ReplaceTile(int layer, int tileToReplaceX, int tileToReplaceY, int newTileGID, TileManager TileManager)
        {
            newTileGID++;
            Tile ReplaceMenttile = new Tile(TileManager.AllTiles[layer][tileToReplaceX, tileToReplaceY].X, TileManager.AllTiles[layer][tileToReplaceX, tileToReplaceY].Y, newTileGID);
            AssignProperties(ReplaceMenttile, layer, tileToReplaceX, tileToReplaceY, TileManager);

            TileManager.AllTiles[layer][tileToReplaceX, tileToReplaceY] = ReplaceMenttile;
            TileManager.WasModifiedDuringInterval = true;
        }



        public static bool ToolInteraction(Tile tile, GameTime gameTime, int layer, int x, int y, string destructableString, Color particleColor, Rectangle destinationRectangle, TileManager TileManager)
        {


            if (TileManager.TileHitPoints.ContainsKey(tile.TileKey))
            {
                string[] info = destructableString.Split(',');
                if (TileManager.TileHitPoints[tile.TileKey] > 0)
                {
                    if (Game1.Utility.GetTileTierRequired(info))
                    {
                        int soundInt = Game1.Utility.GetTileDestructionSound(info);
                        Game1.SoundManager.PlaySoundEffectFromInt(1, soundInt);
                        Game1.CurrentStage.ParticleEngine.Activate(.25f, new Vector2(destinationRectangle.X + 5, destinationRectangle.Y - 20), particleColor, tile.LayerToDrawAt + tile.LayerToDrawAtZOffSet);
                    }
                    else
                    {
                        return false;
                    }

                    return true;
                }

                if (TileManager.TileHitPoints[tile.TileKey] < 1)
                {
                    if (Game1.Utility.GetTileTierRequired(info))
                    {
                        int soundInt = Game1.Utility.GetTileDestructionSound(info, true); //different sound will play if its the final hit.
                        Game1.SoundManager.PlaySoundEffectFromInt(1, soundInt);
                        TileManager.TileHitPoints.Remove(tile.TileKey);
                    }
                    else
                    {
                        return false;
                    }


                }
                if (TileManager.TileSetDictionary[tile.GID].AnimationFrames.Count > 0 && !TileManager.TileSetDictionary[tile.GID].Properties.ContainsKey("crop"))
                {

                    List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                    for (int i = 0; i < TileManager.MapName.Tilesets[TileManager.TileSetNumber].Tiles[tile.GID].AnimationFrames.Count; i++)
                    {
                        frames.Add(new EditableAnimationFrame(TileManager.MapName.Tilesets[TileManager.TileSetNumber].Tiles[tile.GID].AnimationFrames[i]));
                    }
                    EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, x, y, layer, tile.GID, TileManager.TileSetDictionary[tile.GID].Properties.ContainsKey("newSource")) { Terminates = true };
                    TileManager.AnimationFrames.Add(tile.TileKey, frameHolder);
                }
                else
                {
                    if (TileManager.TileSetDictionary[tile.GID].Properties.ContainsKey("tileType"))
                    {
                        TileManager.AddTileModification(tile, TileModificationHandler.GetTileModificationType(TileManager.TileSetDictionary[tile.GID].Properties["tileType"], TileManager, layer, x, y, tile, Game1.Player.controls.Direction));
                    }
                    else
                    {
                        FinalizeTile(layer, gameTime, x, y, TileManager);
                    }

                }

                Game1.CurrentStage.ParticleEngine.Activate(1f, new Vector2(destinationRectangle.X + 5, destinationRectangle.Y - 20), particleColor, tile.LayerToDrawAt + tile.LayerToDrawAtZOffSet);
                return true;
            }
            return false;
        }



        /// <summary>
        /// For use with the destructable tile property. May trigger player animation and/or reduce tile hitpoints.
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="gameTime"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="destinationRectangle"></param>
        /// <param name="TileManager"></param>
        public static void InteractWithDestructableTile(int layer, GameTime gameTime, int x, int y, Rectangle destinationRectangle, TileManager TileManager)
        {
            Tile tile = TileManager.AllTiles[layer][x, y];

            if (!TileManager.AnimationFrames.ContainsKey(tile.TileKey) && !Game1.Player.IsPerformingAction)
            {
                AnimationType actionType = Game1.Utility.GetRequiredTileTool(TileManager.TileSetDictionary[tile.GID].Properties["destructable"]);

                if (actionType == AnimationType.HandsPicking)  //this is out here because any equipped item should be able to pick it up no matter what
                {
                    Game1.Player.DoPlayerAnimation(AnimationType.HandsPicking, .25f);
                    FinalizeTile(layer, gameTime, x, y, TileManager, delayTimer: .25f);
                    if (TileManager.TileHitPoints.ContainsKey(tile.TileKey))
                    {
                        TileManager.TileHitPoints[tile.TileKey]--;
                    }

                }
                else if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem() != null)
                {
                    if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ItemType == (ItemType)actionType)
                    {



                        if (ToolInteraction(tile, gameTime, layer, x, y, TileManager.TileSetDictionary[tile.GID].Properties["destructable"],
                            Game1.Utility.GetTileEffectColor(TileManager.TileSetDictionary[tile.GID].Properties["destructable"]), destinationRectangle, TileManager))
                        {
                            Game1.Player.DoPlayerAnimation(actionType, .25f, Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem());
                            Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().AlterDurability(1);
                            if (TileManager.TileHitPoints.ContainsKey(tile.TileKey))
                            {
                                TileManager.TileHitPoints[tile.TileKey]--;


                            }
                            Game1.Player.UserInterface.StaminaBar.DecreaseStamina(1);
                        }


                    }
                }

            }
        }

        private static void ProcessBodyRemoval(TileManager tileManager,Tile tile)
        {
            List<Body> bodies = tileManager.Objects[tile.TileKey];
            for (int i = 0; i < bodies.Count; i++)
            {
                Body body = bodies[i];
                Game1.VelcroWorld.RemoveBody(bodies[i]);
                bodies.RemoveAt(i);
            }
            tileManager.Objects.Remove(tile.TileKey);
        }

        private static void ProcessGridRemoval(TileManager tileManager, Tile tile, int x, int y)
        {
            bool atLeastOneObjectExists = false;
            for (int i = 0; i < 4; i++)
            {
                if (tileManager.Objects.ContainsKey(tile.GetTileKeyString(i, tileManager)))
                {
                    atLeastOneObjectExists = true;
                }
            }
            if (!atLeastOneObjectExists)
            {
                tileManager.PathGrid.UpdateGrid(x, y, PathFinding.GridStatus.Clear);
            }
        }
        /// <summary>
        /// Replaces tile with default and possibly removes associated: objects, hitpoints, spawnwith, crops, as well as reassigning adjacent tiles
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="gameTime"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="TileManager"></param>
        /// <param name="delayTimer"></param>
        public static void FinalizeTile(int layer, GameTime gameTime, int x, int y, TileManager TileManager, float delayTimer = 0f)
        {

            Tile tile = TileManager.AllTiles[layer][x, y];

            if (TileManager.Objects.ContainsKey(tile.TileKey))
            {
                ProcessBodyRemoval(TileManager, tile);
                ProcessGridRemoval(TileManager, tile,x,y);
               
            }

            if (TileManager.TileHitPoints.ContainsKey(tile.TileKey))
            {
                TileManager.TileHitPoints.Remove(tile.TileKey);
            }
            if (Game1.CurrentStage.Hulls.ContainsKey(tile.TileKey))
            {

                Hull hull = Game1.CurrentStage.Hulls[tile.TileKey];
                Game1.CurrentStage.Hulls.Remove(tile.TileKey);
                Game1.CurrentStage.Penumbra.Hulls.Remove(hull);
            }
            if (Game1.CurrentStage.Lights.ContainsKey(tile.TileKey))
            {
                Light light = Game1.CurrentStage.Lights[tile.TileKey];
                Game1.CurrentStage.Lights.Remove(tile.TileKey);
                Game1.CurrentStage.Penumbra.Lights.Remove(light);
            }
            //if tileset has loot value, then use that. otherwise check the loot xml data.
            Item itemToCheckForReassasignTiling;
            if (TileManager.TileSetDictionary[tile.GID].Properties.ContainsKey("loot"))
            {
                if (TileManager.TileSetDictionary[tile.GID].Properties["loot"] == string.Empty)
                {
                    itemToCheckForReassasignTiling = Game1.LootBank.GetandSpawnLootFromXML(tile.GID, tile.Position, TileManager);
                }
                else
                {
                    itemToCheckForReassasignTiling = Game1.LootBank.GetLootFromTileset(tile.GID, tile.GetPosition(TileManager), TileManager.TileSetDictionary[tile.GID].Properties["loot"], TileManager);
                }
                //// List<Loot> tempLoot = Loot.Parselootkey(TileManager.MapName.Tilesets[TileManager.TileSetNumber].Tiles[TileManager.AllTiles[layer][x, y].GID].Properties["loot"]);
                // itemToCheckForReassasignTiling = Game1.LootBank.GetLootFromXML(TileManager.AllTiles[layer][x, y].GID, TileManager.AllTiles[layer][x, y].GetPosition(TileManager));
                //   //  Loot.GetDrop(tempLoot, TileManager.AllTiles[layer][x, y].DestinationRectangle);
            }

            if (TileManager.Crops.ContainsKey(tile.GetTileKeyString(layer, TileManager)))
            {
                TileManager.Crops.Remove(tile.GetTileKeyString(layer, TileManager));
                string[] info = TileManager.TileSetDictionary[tile.GID].Properties["destructable"].Split(',');
                Game1.SoundManager.PlaySoundEffectFromInt(1, Game1.Utility.GetTileDestructionSound(info));
                TileUtility.ReplaceTile(0, x, y, 87, TileManager);
                TileUtility.ReplaceTile(layer, x, y, 0, TileManager);
            }


            TileUtility.ReplaceTile(layer, x, y, 0, TileManager);

            // this is used to see if that tile should tell other tiles around it to check their tiling, as this one may affect it.
            if (TileManager.TileSetDictionary[tile.GID].Properties.ContainsKey("generate"))
            {
                GenerationType generationType = (GenerationType)Enum.Parse(typeof(GenerationType), TileManager.TileSetDictionary[tile.GID].Properties["generate"], true);
                if ((int)generationType != 0)
                {
                    TilingTileManager tilingTileManager = Game1.Procedural.GetTilingTileManagerFromGenerationType(generationType);
                    WangManager.GroupReassignForTiling(tile.GID, tilingTileManager.GeneratableTiles, tilingTileManager.TilingDictionary, layer,
                        x, y, Game1.CurrentStage.AllTiles.MapWidth, Game1.CurrentStage.AllTiles.MapWidth,
                        (TileManager)Game1.CurrentStage.AllTiles);
                }
            }

        }
        #endregion


        private static bool CheckIfTileAlreadyExists(int tileX, int tileY, int layer, TileManager TileManager)
        {
            if (TileManager.AllTiles[layer][tileX, tileY].GID != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckIfTileMatchesGID(int tileX, int tileY, int layer, List<int> acceptablTiles, TileManager TileManager, int comparisonLayer = 0)
        {
            for (int i = 0; i < acceptablTiles.Count; i++)
            {
                if (TileManager.AllTiles[comparisonLayer][tileX, tileY].GID == acceptablTiles[i])
                {
                    return true;
                }
            }
            return false;
        }



        public static void Animate(Dir direction, int layer, int x, int y, TileManager TileManager, bool terminates = true)
        {
            Tile tile = TileManager.AllTiles[layer][x, y];
            if (TileManager.AnimationFrames.ContainsKey(tile.TileKey))
            {
                TileManager.AnimationFrames.Remove(tile.TileKey);
            }


            List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
            if (direction == Dir.Right)
            {
                for (int i = 0; i < TileManager.MapName.Tilesets[TileManager.TileSetNumber].Tiles[tile.GID].AnimationFrames.Count; i++)
                {
                    frames.Add(new EditableAnimationFrame(TileManager.MapName.Tilesets[TileManager.TileSetNumber].Tiles[tile.GID].AnimationFrames[i]));
                }
            }
            else
            {
                for (int i = TileManager.MapName.Tilesets[TileManager.TileSetNumber].Tiles[tile.GID].AnimationFrames.Count - 1; i >= 0; i--)
                {
                    frames.Add(new EditableAnimationFrame(TileManager.MapName.Tilesets[TileManager.TileSetNumber].Tiles[tile.GID].AnimationFrames[i]));
                }

                float currentduration = TileManager.MapName.Tilesets[TileManager.TileSetNumber].Tiles[tile.GID].AnimationFrames[0].Duration;
                frames.Add(new EditableAnimationFrame(currentduration, tile.GID));
            }


            bool hasNewSource = false;
            EditableAnimationFrameHolder frameHolder;
            string propertyString = "newSource";
            if (GetProperty(TileManager.MapName.Tilesets[TileManager.TileSetNumber].Tiles, TileManager.AllTiles[layer][x, y].GID, ref propertyString))
            {



                hasNewSource = true;
                int[] nums = GetRectangeFromString(propertyString);

                frameHolder = new EditableAnimationFrameHolder(frames, x, y, layer, TileManager.AllTiles[layer][x, y].GID, hasNewSource: hasNewSource)
                {
                    OriginalXOffSet = nums[0],
                    OriginalYOffSet = nums[1],
                    OriginalWidth = nums[2],
                    OriginalHeight = nums[3],
                    Terminates = terminates
                };

            }
            else
            {
                frameHolder = new EditableAnimationFrameHolder(frames, x, y, layer, tile.GID, hasNewSource: hasNewSource) { Terminates = terminates };
            }
            TileManager.AnimationFrames.Add(tile.TileKey, frameHolder);
        }

    }



    public class EditableAnimationFrame
    {
        public float AnchorDuration { get; private set; }
        public float TargetDuration { get; set; }
        public int ID { get; set; }

        public EditableAnimationFrame(AnimationFrameHolder frame)
        {
            this.AnchorDuration = (float)frame.Duration / 1000;
            this.TargetDuration = this.AnchorDuration + Game1.GlobalClock.SecondsPassedToday;
            this.ID = frame.Id;

        }

        public EditableAnimationFrame(float duration, int GID)
        {

            this.AnchorDuration = (float)duration / 1000;
            this.TargetDuration = this.AnchorDuration + Game1.GlobalClock.SecondsPassedToday;
            this.ID = GID;
        }
    }

    public class EditableAnimationFrameHolder
    {
        public List<EditableAnimationFrame> Frames { get; set; }
        public int Counter { get; set; }
        public int OldX { get; }
        public int OldY { get; }
        public int Layer { get; set; }
        public int OriginalTileID { get; set; }
        public bool Repeats { get; set; }

        public bool Terminates { get; set; }

        public bool HasNewSource { get; set; }
        public int OriginalXOffSet { get; set; }
        public int OriginalYOffSet { get; set; }
        public int OriginalWidth { get; set; }
        public int OriginalHeight { get; set; }

        public EditableAnimationFrameHolder(List<EditableAnimationFrame> frames, int oldX, int oldY, int layer, int originalTileID, bool hasNewSource = false)
        {
            this.Frames = frames;
            this.Counter = 0;
            this.OldX = oldX;
            this.OldY = oldY;
            this.Layer = layer;
            this.OriginalTileID = originalTileID;

            this.HasNewSource = hasNewSource;
            this.Terminates = true;
        }

        public void SetNewTarget(int frame)
        {
            Frames[frame].TargetDuration = Frames[frame].AnchorDuration + Game1.GlobalClock.SecondsPassedToday;
        }
    }
}
