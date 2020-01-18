using Microsoft.Xna.Framework;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;

using SecretProject.Class.UI;
using System;
using System.Collections.Generic;
using TiledSharp;
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
        public static int[] GetNewTileSourceRectangle(string info)
        {
            int[] numsToReturn = new int[4];
            numsToReturn[0] = int.Parse(info.Split(',')[0]);
            numsToReturn[1] = int.Parse(info.Split(',')[1]);
            numsToReturn[2] = int.Parse(info.Split(',')[2]);
            numsToReturn[3] = int.Parse(info.Split(',')[3]);

            return numsToReturn;
        }

        public static Rectangle GetTileRectangleFromProperty(Tile tile, bool adjustDestinationRectangle, IInformationContainer container = null, int gid = 0)
        {
            int newGID = gid;
            TmxTileset tileSet = null;
            if (container == null)
            {
                tileSet = Game1.Town.AllTiles.MapName.Tilesets[Game1.Town.AllTiles.TileSetNumber];
            }
            else
            {

                tileSet = container.MapName.Tilesets[container.TileSetNumber];
            }
            int[] rectangleCoords = GetNewTileSourceRectangle(tileSet.Tiles[newGID].Properties["newSource"]);

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
            return worldPos / 16;
        }
        /// <summary>
        /// Finds and indexes into a chunk, returning the proper tile.
        /// </summary>
        /// <param name="tileX">The tile's SQUARE (meaning worldPos /16 ) X Coord!</param>
        /// <param name="tileY">The tile's SQUARE (meaning worldPos /16 ) Y Coord!</param>
        /// <param name="layer"></param>
        /// <param name="ActiveChunks"></param>
        /// <returns></returns>
        public static Tile GetChunkTile(int tileX, int tileY, int layer, Chunk[,] ActiveChunks)
        {
            int chunkX = (int)Math.Floor((float)tileX / 16.0f);

            int chunkY = (int)Math.Floor((float)tileY / 16.0f);

            Chunk chunk = GetChunk(tileX, tileY, ActiveChunks);
            if (chunk == null)
            {
                return null;

            }

            int localX = (int)Math.Floor((float)(tileX - chunkX * 16));
            int localY = (int)Math.Floor((float)(tileY - chunkY * 16));

            if (localX > 15)
            {
                localX = 15;
            }
            else if (localX < 0)
            {
                localX = 0;
            }

            if (localY > 15)
            {
                localY = 15;
            }
            else if (localY < 0)
            {
                localY = 0;
            }
            // = tile;
            return (chunk.AllTiles[layer][localX, localY]);

        }
        /// <summary>
        /// Given a world position, returns the index of the x or y coordinate within a found chunk.
        /// </summary>
        /// <param name="globalCoord"></param>
        /// <returns></returns>
        public static int GetLocalChunkCoord(int globalCoord)
        {
            int chunkCoord = (int)Math.Floor((float)globalCoord / 16.0f / 16.0f);
            int localCoord = (int)Math.Ceiling((float)(globalCoord / 16 - chunkCoord * 16));
            if (chunkCoord < 0)
            {
                localCoord--;
            }
            return localCoord;
        }


        public static Chunk GetChunk(int tileX, int tileY, Chunk[,] ActiveChunks)
        {
            int chunkX = (int)Math.Floor((float)tileX / 16.0f);

            int chunkY = (int)Math.Floor((float)tileY / 16.0f);
            for (int i = 0; i < ActiveChunks.GetUpperBound(0); i++)
            {
                for (int j = 0; j < ActiveChunks.GetUpperBound(0); j++)
                {
                    if (ActiveChunks[i, j].X == chunkX && ActiveChunks[i, j].Y == chunkY)
                    {
                        return ActiveChunks[i, j];
                    }


                }
            }
            return null;
        }
        #endregion

        /// <summary>
        /// All new tiles should be "wrung through" this method. Makes sure all dictionaries etc will be aware of this new tile. 
        /// </summary>
        /// <param name="tileToAssign"></param>
        /// <param name="layer"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="container"></param>
        public static void AssignProperties(Tile tileToAssign, int layer, int x, int y, IInformationContainer container)
        {

            tileToAssign.DestinationRectangle = GetDestinationRectangle(tileToAssign);
            tileToAssign.SourceRectangle = GetSourceRectangle(tileToAssign, container.TileSetDimension);
            tileToAssign.TileKey = tileToAssign.GetTileKeyStringNew(layer, container);

            if (container.MapName.Tilesets[container.TileSetNumber].Tiles.ContainsKey(tileToAssign.GID))
            {
                //replaces tiles with wheat grass
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("replace"))
                {
                    GrassTuft grassTuft = new GrassTuft(container.GraphicsDevice, 4, new Vector2(tileToAssign.DestinationRectangle.X + 8
                                , tileToAssign.DestinationRectangle.Y + 8));
                    List<GrassTuft> tufts = new List<GrassTuft>()
                    {
                        grassTuft,
                    };
                    container.Tufts.Add(tileToAssign.TileKey, tufts);
                    ReplaceTile(layer, x, y, 0, container);
                    return;
                }
                
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].AnimationFrames.Count > 0 && !container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("idleStart"))
                {

                    if (!container.AnimationFrames.ContainsKey(tileToAssign.TileKey))
                    {


                        List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                        


                            frames.Add(new EditableAnimationFrame(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].AnimationFrames[0].Duration, container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].AnimationFrames[0].Duration, tileToAssign.GID));
                        for (int i = 0; i < container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].AnimationFrames.Count; i++)
                        {
                            frames.Add(new EditableAnimationFrame(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].AnimationFrames[i]));
                        }
                        bool hasNewSource = false;
                        EditableAnimationFrameHolder frameHolder;
                        if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("newSource"))
                        {
                            hasNewSource = true;
                            int[] nums = GetNewTileSourceRectangle(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["newSource"]);

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
                        
                        container.AnimationFrames.Add(tileToAssign.TileKey, frameHolder);
                    }

                }
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("crop"))
                {
                    int cropID = int.Parse(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["crop"]);
                    Crop tempCrop = Game1.AllCrops.GetCropFromID(cropID);
                    tempCrop.DayPlanted = Game1.GlobalClock.TotalDays;
                    tempCrop.GID++;
                    tempCrop.X = x;
                    tempCrop.Y = y;
                    container.Crops[tileToAssign.GetTileKeyStringNew(layer, container)] = tempCrop;
                }
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("lightSource"))
                {
                    Vector2 lightOffSet = LightSource.ParseLightData(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["lightSource"]);
                    LightSource newLight = new LightSource(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["lightSource"], new Vector2(GetDestinationRectangle(tileToAssign).X + lightOffSet.X, GetDestinationRectangle(tileToAssign).Y + lightOffSet.Y));

                    if(newLight.LightType == LightType.NightTime)
                    {
                        if(!container.NightTimeLights.Contains(newLight))
                        {
                            container.NightTimeLights.Add(newLight);
                        }
                        
                    }
                    else
                    {
                        if(!container.DayTimeLights.Contains(newLight))
                        {
                            container.DayTimeLights.Add(newLight);
                        }
                      
                    }
                }


                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("destructable"))
                {
                    container.TileHitPoints[tileToAssign.TileKey] = Game1.Utility.GetTileHitpoints(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["destructable"]);

                }

                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("layer"))
                {
                    tileToAssign.LayerToDrawAt = int.Parse(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["layer"]);
                    //grass = 1, stone = 2, wood = 3, sand = 4
                }

                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("generate"))
                {
                    tileToAssign.GenerationType = (GenerationType)Enum.Parse(typeof(GenerationType),container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["generate"]);
                    //grass = 1, stone = 2, wood = 3, sand = 4
                }

                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("action"))
                {
                    if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["action"] == "chestLoot")
                    {

                        if (!container.StoreableItems.ContainsKey(tileToAssign.TileKey))
                        {
                            container.StoreableItems.Add(tileToAssign.TileKey, new Chest(tileToAssign.TileKey, 6,
                                    new Vector2(GetDestinationRectangle(tileToAssign).X, GetDestinationRectangle(tileToAssign).Y), container.GraphicsDevice, false));
                        }

                    }
                    else if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["action"] == "cook")
                    {

                        if (!container.StoreableItems.ContainsKey(tileToAssign.TileKey))
                        {
                            container.StoreableItems.Add(tileToAssign.TileKey, new Cauldron(tileToAssign.TileKey, 3,
                                    new Vector2(GetDestinationRectangle(tileToAssign).X, GetDestinationRectangle(tileToAssign).Y), container.GraphicsDevice));
                        }

                    }
                    else if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["action"] == "smelt")
                    {

                        if (!container.StoreableItems.ContainsKey(tileToAssign.TileKey))
                        {
                            container.StoreableItems.Add(tileToAssign.TileKey, new Furnace(tileToAssign.TileKey, 3,
                                    new Vector2(GetDestinationRectangle(tileToAssign).X, GetDestinationRectangle(tileToAssign).Y), container.GraphicsDevice));
                        }

                    }
                }
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("newSource"))
                {
                    tileToAssign.SourceRectangle = GetTileRectangleFromProperty(tileToAssign, true, container, tileToAssign.GID);
                }
                if (layer == 3)
                {
                    float randomOffSet = Game1.Utility.RFloat(Game1.Utility.ForeGroundMultiplier, Game1.Utility.ForeGroundMultiplier * 10);
                    float offSetDrawValue = (GetDestinationRectangle(tileToAssign).Y) * Game1.Utility.ForeGroundMultiplier + randomOffSet;
                    if (x > 0 && container.AllTiles[layer][x - 1, y].LayerToDrawAtZOffSet == offSetDrawValue)
                    {
                        offSetDrawValue += Game1.Utility.ForeGroundMultiplier;
                    }
                    tileToAssign.LayerToDrawAtZOffSet = offSetDrawValue;

                    
                    
                }


                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("newHitBox"))
                {
                    int[] rectangleCoords = GetNewTileSourceRectangle(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["newHitBox"]);

                    Collider tempObjectBody = new Collider(container.GraphicsDevice,
                            new Rectangle(GetDestinationRectangle(tileToAssign).X + rectangleCoords[0],
                            GetDestinationRectangle(tileToAssign).Y + rectangleCoords[1], rectangleCoords[2],
                            rectangleCoords[3]), null, ColliderType.inert)
                    { LocationKey = tileToAssign.TileKey };

                    if (container.Objects.ContainsKey(tileToAssign.TileKey))
                    {

                    }
                    else
                    {
                        container.Objects.Add(tileToAssign.TileKey, new List<ICollidable>());
                    }
                    container.Objects[tileToAssign.TileKey].Add(tempObjectBody);


                    // reassignGrid = false;
                    if (container.Type == 0)
                    {
                        int startI = rectangleCoords[0] / 16;
                        int endI = rectangleCoords[2] / 16;
                        endI = endI + startI;

                        int startJ = rectangleCoords[1] / 16;
                        int endJ = rectangleCoords[3] / 16;
                        endJ = startJ + endJ;
                        for (int i = startI; i < endI; i++)
                        {
                            for (int j = startJ; j < endJ; j++)
                            {
                                container.PathGrid.UpdateGrid(x + i, y + j, 0);
                            }
                        }
                    }
                }
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].ObjectGroups.Count > 0)
                {

                    container.PathGrid.UpdateGrid(x, y, 0);


                    if (container.Objects.ContainsKey(tileToAssign.TileKey))
                    {

                    }
                    else
                    {
                        container.Objects.Add(tileToAssign.TileKey, new List<ICollidable>());
                    }

                    for (int k = 0; k < container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].ObjectGroups[0].Objects.Count; k++)
                    {
                        TmxObject tempObj = container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].ObjectGroups[0].Objects[k];


                        Collider tempObjectBody = new Collider(container.GraphicsDevice, new Rectangle(GetDestinationRectangle(tileToAssign).X + (int)Math.Ceiling(tempObj.X),
                            GetDestinationRectangle(tileToAssign).Y + (int)Math.Ceiling(tempObj.Y) - 5, (int)Math.Ceiling(tempObj.Width),
                            (int)Math.Ceiling(tempObj.Height) + 5), null, ColliderType.inert)
                        { LocationKey = tileToAssign.TileKey };



                        container.Objects[tileToAssign.TileKey].Add(tempObjectBody);

                    }
                }


            }

        }

        /// <summary>
        /// For use with the "action" tile property. Does a number of various things depending on what the property is. May change the cursor texture.
        /// </summary>
        /// <param name="z"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="action"></param>
        /// <param name="mouse"></param>
        /// <param name="container"></param>
        public static void ActionHelper(int z, int i, int j, string action, MouseManager mouse, IInformationContainer container)
        {
            //new Gid should be one larger, per usual
            string[] information = Game1.Utility.GetActionHelperInfo(action);
            switch (information[0])
            {
                //including animation frame id to replace!
                case "diggable":

                    if (container.AllTiles[1][i, j].GID == -1)
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
                                    switch (container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[z][i, j].GID].Properties["generate"])
                                    {
                                        case "Dirt":
                                            Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.DigDirt);
                                            TileUtility.ReplaceTile(z, i, j, 86, container);
                                            Game1.GetCurrentStage().AllItems.Add(Game1.ItemVault.GenerateNewItem(1006, container.AllTiles[z][i, j].GetPosition(container), true));
                                            break;
                                        case "dirtBasic":
                                            Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.DigDirt);
                                            TileUtility.ReplaceTile(z, i, j, 86, container);
                                            Game1.GetCurrentStage().AllItems.Add(Game1.ItemVault.GenerateNewItem(1006, container.AllTiles[z][i, j].GetPosition(container), true));
                                            break;
                                        case "grassBasic":
                                            Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.DigDirt);
                                            TileUtility.ReplaceTile(z, i, j, 1006, container);
                                            int cx = (int)Game1.myMouseManager.WorldMousePosition.X;
                                            int cy = (int)Game1.myMouseManager.WorldMousePosition.Y;


                                            WangManager.GroupReassignForTiling(cx, cy, 1006, Game1.Procedural.AllTilingContainers[1].GeneratableTiles,
                                        Game1.Procedural.AllTilingContainers[1].TilingDictionary, 0, Game1.GetCurrentStage().AllTiles);

                                            break;
                                    }
                                }
                            }
                            else if (item.ItemType == ItemType.Tree)
                            {
                                mouse.ChangeMouseTexture(CursorType.Planting);
                                if (mouse.IsClicked)
                                {
                                    if (!container.Crops.ContainsKey(container.AllTiles[3][i, j].GetTileKeyStringNew(3, container)))
                                    {

                                        Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.DigDirt);
                                        Crop tempCrop = Game1.AllCrops.GetCropFromID(Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ID);

                                        TileUtility.ReplaceTile(3, i, j, tempCrop.GID + 1, container);

                                        Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ID);
                                    }
                                }
                            }
                        }
                    }
                    break;

                case "plantable":
                    if (Game1.GetCurrentStage().StageType == StageType.Procedural)
                    {


                        Game1.Player.UserInterface.DrawTileSelector = true;


                        if (mouse.IsClicked)
                        {
                            if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem() != null)
                            {
                                Item testItem = Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem();
                                if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().IsPlantable)
                                {
                                    mouse.ChangeMouseTexture(CursorType.Planting);
                                    if (!container.Crops.ContainsKey(container.AllTiles[3][i, j].GetTileKeyStringNew(2, container)))
                                    {

                                        Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.DigDirt);
                                        Crop tempCrop = Game1.AllCrops.GetCropFromID(Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ID);

                                        TileUtility.ReplaceTile(3, i, j, tempCrop.GID + 1, container);
  
                                        Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ID);
                                    }
                                }
                            }

                        }
                    }
                    else if (Game1.GetCurrentStage().StageType == StageType.Sanctuary)
                    {
                        if (mouse.IsClicked)
                        {
                            for(int sp = 0; sp < Game1.Forest.AllTiles.AllPlots.Count; sp++)
                            {
                                if(mouse.WorldMouseRectangle.Intersects(Game1.Forest.AllTiles.AllPlots[sp].Bounds))
                                {
                                    if (Game1.Forest.AllTiles.AllPlots[sp].ItemIDAllowed == Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool())
                                    {
                                        SanctuaryTracker tracker = Game1.GetSanctuaryTrackFromStage(Game1.GetCurrentStageInt());
                                        Item testItem = Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem();
                                        if (testItem != null)
                                        {
                                            if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().IsPlantable)
                                            {

                                                mouse.ChangeMouseTexture(CursorType.Planting);
                                                if (!container.Crops.ContainsKey(container.AllTiles[3][i, j].GetTileKeyStringNew(3, container)))
                                                {
                                                    if (tracker.UpdateCompletionGuide(Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool()))
                                                    {
                                                        Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.DigDirt);
                                                        Crop tempCrop = Game1.AllCrops.GetCropFromID(Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ID);

                                                        TileUtility.ReplaceTile(3, i, j, tempCrop.GID + 1, container);

                                                        Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ID);
                                                    }
                                                }

                                            }
                                        }
                                        return;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                
                            }
                            

                        }

                    }
                    //}
                    break;

                case "sleep":
                    mouse.ChangeMouseTexture(CursorType.Normal);
                    if (mouse.IsClicked)
                    {
                        Game1.Player.UserInterface.TransitionSpeed = .005f;
                        Game1.Player.UserInterface.TransitionTimer.TargetTime = 4f;
                        Game1.Player.UserInterface.IsTransitioning = true;
                        Game1.GlobalClock.IncrementDay();
                        Game1.GlobalClock.TotalHours = 6;
                    }
                    break;

                case "chestLoot":
                    if (mouse.IsClicked)
                    {
                        //Game1.Player.UserInterface.CurrentAccessedStorableItem = container.StoreableItems[container.AllTiles[z][i, j].GetTileKeyStringNew(z, container)];
                        Game1.Player.UserInterface.SwitchCurrentAccessedStorageItem(container.StoreableItems[container.AllTiles[z][i, j].TileKey]);
                        Game1.Player.UserInterface.CurrentAccessedStorableItem.Activate(container.AllTiles[z][i, j]);
                    }
                    break;

                case "triggerLift":
                    if (mouse.IsClicked)
                    {
                        if (Game1.GetCurrentStageInt() == Stages.OverWorld)
                        {
                            foreach (Chunk chunk in Game1.OverWorld.AllTiles.ActiveChunks)
                            {
                                if (!chunk.AreReadersAndWritersDone)
                                {
                                    return;
                                }
                            }
                            Game1.Player.UserInterface.WarpGate.To = Stages.Town;
                        }
                        else if (Game1.GetCurrentStageInt() == Stages.Town)
                        {
                            Game1.Player.UserInterface.WarpGate.To = Stages.OverWorld;
                        }

                        Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.WarpGate;

                    }
                    break;



                case "cook":
                    mouse.ChangeMouseTexture(CursorType.Normal);
                    if (mouse.IsClicked)
                    {
                        Game1.Player.UserInterface.SwitchCurrentAccessedStorageItem(container.StoreableItems[container.AllTiles[z][i, j].TileKey]);
                        Game1.Player.UserInterface.CurrentAccessedStorableItem.Activate(container.AllTiles[z][i, j]);


                    }
                    break;
                case "smelt":
                    mouse.ChangeMouseTexture(CursorType.Normal);
                    if (mouse.IsClicked)
                    {
                        Game1.Player.UserInterface.CurrentAccessedStorableItem = container.StoreableItems[container.AllTiles[z][i, j].TileKey];
                        Game1.Player.UserInterface.CurrentAccessedStorableItem.IsUpdating = true;


                    }
                    break;
                case "enterPlayerHouse":
                    mouse.ChangeMouseTexture(CursorType.Door);
                    {
                        if (mouse.IsClicked)
                        {
                            Portal portal = Game1.GetCurrentStage().AllPortals.Find(x => x.To == 5);
                            Game1.SwitchStage(Game1.GetCurrentStageInt(), Stages.PlayerHouse, portal);
                        }
                    }
                    break;
                case "enterPortal":
                    if (mouse.IsClicked)
                    {
                        if (Game1.GetCurrentStageInt() == Stages.OverWorld)
                        {
                            foreach (Chunk chunk in Game1.OverWorld.AllTiles.ActiveChunks)
                            {
                                if (!chunk.AreReadersAndWritersDone)
                                {
                                    return;
                                }
                            }
                            Game1.Player.UserInterface.WarpGate.To = Stages.Town;
                        }
                        else if (Game1.GetCurrentStageInt() == Stages.Town)
                        {
                            Game1.Player.UserInterface.WarpGate.To = Stages.OverWorld;
                        }

                        Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.WarpGate;
                        Game1.Player.UserInterface.WarpGate.CenterOfPortal = container.AllTiles[z][i, j].GetPosition(container);
                    }
                    break;

            }
        }



        #region TILEREPLACEMENT AND INTERACTIONS
        public static void ReplaceTile(int layer, int tileToReplaceX, int tileToReplaceY, int newTileGID, IInformationContainer container)
        {
            Tile ReplaceMenttile = new Tile(container.AllTiles[layer][tileToReplaceX, tileToReplaceY].X, container.AllTiles[layer][tileToReplaceX, tileToReplaceY].Y, newTileGID);
            AssignProperties(ReplaceMenttile, layer, tileToReplaceX, tileToReplaceY, container);

            container.AllTiles[layer][tileToReplaceX, tileToReplaceY] = ReplaceMenttile;
        }

        public static void ToolInteraction(Tile tile, GameTime gameTime, int layer, int x, int y, int setSoundInt, Color particleColor, Rectangle destinationRectangle, IInformationContainer container)
        {


            if (container.TileHitPoints.ContainsKey(tile.TileKey))
            {

                if (container.TileHitPoints[tile.TileKey] > 0)
                {
                    Game1.SoundManager.PlaySoundEffectFromInt(1, setSoundInt);
                    Game1.GetCurrentStage().ParticleEngine.Activate(.25f, new Vector2(destinationRectangle.X + 5, destinationRectangle.Y - 20), particleColor, tile.LayerToDrawAt + tile.LayerToDrawAtZOffSet);
                    return;
                }

                if (container.TileHitPoints[tile.TileKey] < 1)
                {
                    Game1.SoundManager.PlaySoundEffectFromInt(1, setSoundInt);
                    container.TileHitPoints.Remove(tile.TileKey);

                }
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].AnimationFrames.Count > 0)
                {
                    List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                    for (int i = 0; i < container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].AnimationFrames.Count; i++)
                    {
                        frames.Add(new EditableAnimationFrame(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].AnimationFrames[i]));
                    }
                    EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, x, y, layer, container.AllTiles[layer][x, y].GID, true);
                    container.AnimationFrames.Add(tile.TileKey, frameHolder);
                }
                else
                {
                    FinalizeTile(layer, gameTime, x, y, container);
                }

                Game1.GetCurrentStage().ParticleEngine.Activate(1f, new Vector2(destinationRectangle.X + 5, destinationRectangle.Y - 20), particleColor, tile.LayerToDrawAt + tile.LayerToDrawAtZOffSet);
            }
        }

        

        /// <summary>
        /// For use with the destructable tile property. May trigger player animation and/or reduce tile hitpoints.
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="gameTime"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="destinationRectangle"></param>
        /// <param name="container"></param>
        public static void InteractWithDestructableTile(int layer, GameTime gameTime, int x, int y, Rectangle destinationRectangle, IInformationContainer container)
        {

            if (!container.AnimationFrames.ContainsKey(container.AllTiles[layer][x, y].TileKey) && !Game1.Player.IsPerformingAction)
            {
                AnimationType actionType = Game1.Utility.GetRequiredTileTool(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties["destructable"]);

                if (actionType == AnimationType.HandsPicking)  //this is out here because any equipped item should be able to pick it up no matter what
                {
                    Game1.Player.DoPlayerAnimation(AnimationType.HandsPicking, .25f);
                    FinalizeTile(layer, gameTime, x, y, container, delayTimer: .25f);
                    if (container.TileHitPoints.ContainsKey(container.AllTiles[layer][x, y].TileKey))
                    {
                        container.TileHitPoints[container.AllTiles[layer][x, y].TileKey]--;
                    }

                }
                else if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem() != null)
                {
                    if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ItemType == (ItemType)actionType)
                    {


                        Game1.Player.DoPlayerAnimation(actionType, .25f, Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem());
                        ToolInteraction(container.AllTiles[layer][x, y], gameTime, layer, x, y, Game1.Utility.GetTileDestructionSound(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties["destructable"]),
                            Game1.Utility.GetTileEffectColor(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties["destructable"]), destinationRectangle, container);
                        Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().AlterDurability(1);
                        if (container.TileHitPoints.ContainsKey(container.AllTiles[layer][x, y].TileKey))
                        {
                            container.TileHitPoints[container.AllTiles[layer][x, y].TileKey]--;


                        }

                    }
                }

            }
        }
        /// <summary>
        /// Replaces tile with default and possibly removes associated: objects, hitpoints, spawnwith, crops, as well as reassigning adjacent tiles
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="gameTime"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="container"></param>
        /// <param name="delayTimer"></param>
        public static void FinalizeTile(int layer, GameTime gameTime, int x, int y, IInformationContainer container, float delayTimer = 0f)
        {

            //if(container.AllTiles[layer][x, y].TileKey == null)
            //{
            //    container.AllTiles[layer][x, y].TileKey = container.AllTiles[layer][x, y].GetTileKeyStringNew(layer, container);
            //}
            if (container.Objects.ContainsKey(container.AllTiles[layer][x, y].TileKey))
            {
                container.Objects.Remove(container.AllTiles[layer][x, y].TileKey);
                bool atLeastOneObjectExists = false;
                for (int i = 0; i < 4; i++)
                {
                    if (container.Objects.ContainsKey(container.AllTiles[i][x, y].GetTileKeyStringNew(i, container)))
                    {
                        atLeastOneObjectExists = true;
                    }
                }
                if (!atLeastOneObjectExists)
                {
                    container.PathGrid.UpdateGrid(x, y, 1);
                }
            }

            if (container.TileHitPoints.ContainsKey(container.AllTiles[layer][x, y].TileKey))
            {
                container.TileHitPoints.Remove(container.AllTiles[layer][x, y].TileKey);
            }

            Item itemToCheckForReassasignTiling = null;
            if (container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties.ContainsKey("loot"))
            {
                List<Loot> tempLoot = Loot.Parselootkey(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties["loot"]);
                itemToCheckForReassasignTiling = Loot.GetDrop(tempLoot, container.AllTiles[layer][x, y].DestinationRectangle);
            }

            if (container.Crops.ContainsKey(container.AllTiles[layer][x, y].GetTileKeyStringNew(layer, container)))
            {
                container.Crops.Remove(container.AllTiles[layer][x, y].GetTileKeyStringNew(layer, container));

                Game1.SoundManager.PlaySoundEffectFromInt(1, Game1.Utility.GetTileDestructionSound(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties["destructable"]));
                TileUtility.ReplaceTile(0, x, y, 86, container);
                TileUtility.ReplaceTile(layer, x, y, 0, container);
            }


            TileUtility.ReplaceTile(layer, x, y, 0, container);

            // this is used to see if that tile should tell other tiles around it to check their tiling, as this one may affect it.
            if (itemToCheckForReassasignTiling != null)
            {
                if (itemToCheckForReassasignTiling.GenerationType != 0)
                {
                    TilingContainer tilingContainer = Game1.Procedural.GetTilingContainerFromGenerationType(itemToCheckForReassasignTiling.GenerationType);
                    WangManager.GroupReassignForTiling((int)Game1.myMouseManager.WorldMousePosition.X, (int)Game1.myMouseManager.WorldMousePosition.Y, -1, tilingContainer.GeneratableTiles,
                        tilingContainer.TilingDictionary,
                   itemToCheckForReassasignTiling.TilingLayer, Game1.GetCurrentStage().AllTiles);
                }

            }
        }
        #endregion


        public static bool CheckIfTileAlreadyExists(int tileX, int tileY, int layer, IInformationContainer container)
        {
            if (container.AllTiles[layer][tileX, tileY].GID != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckIfTileMatchesGID(int tileX, int tileY, int layer, List<int> acceptablTiles, IInformationContainer container, int comparisonLayer = 0)
        {
            for (int i = 0; i < acceptablTiles.Count; i++)
            {
                if (container.AllTiles[comparisonLayer][tileX, tileY].GID == acceptablTiles[i])
                {
                    return true;
                }
            }
            return false;
        }

        #region GENERATION
        /// <summary>
        /// Generates desired tiles at a certain frequency, and can be restricted to certain tiles and layers
        /// </summary>
        /// <param name="layerToPlace">Layer which tile will try to generate</param>
        /// <param name="gid"></param>
        /// <param name="type">tileset to check which you want to spawn on</param>
        /// <param name="frequency">will try at most this many times to spawn</param>
        /// <param name="layerToCheckIfEmpty">layer which the tileset youre looking at is empty</param>
        /// <param name="container"></param>
        /// <param name="onlyLayerZero">set to true if you want to disallow spawning if path layer is occupied</param>
        /// <param name="assertLeftAndRight">if true, the tiles to the left and right of the found tile must also be in the allowable tiles</param>
        /// <param name="limit">Maximum number of this type of tile we can spawn in a chunk</param>
        public static void GenerateRandomlyDistributedTiles(int layerToPlace, int gid, GenerationType type, int frequency, int layerToCheckIfEmpty, IInformationContainer container, bool onlyLayerZero = false, bool assertLeftAndRight = false, int limit = 0)
        {
            int cap = container.Random.Next(0, frequency);

            int limitCounter = 0;

            for (int g = 0; g < cap; g++)
            {
                if (RetrieveRandomlyDistributedTile(layerToPlace, gid, Game1.Procedural.GetTilingContainerFromGenerationType(type).GeneratableTiles, container, layerToCheckIfEmpty, onlyLayerZero, assertLeftAndRight))
                {
                    limitCounter++;
                }
                if (limit > 0 && limitCounter >= limit)
                {
                    return;
                }
            }
        }
        /// <summary>
        /// Randomly chooses a location within the bounds of the chunk. If it meets the criteria then it will reassign that tile. 
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="id"></param>
        /// <param name="acceptableTiles">Tiles which may be considered to spawn on</param>
        /// <param name="container"></param>
        /// <param name="comparisonLayer"></param>
        /// <param name="zeroLayerOnly">Will only spawn on layer zero when path layer is not occupied</param>
        /// <param name="assertLeftAndRight">will only be considered if tiles to the right and left are also in the acceptable tiles list</param>
        public static bool RetrieveRandomlyDistributedTile(int layer, int id, List<int> acceptableTiles, IInformationContainer container,
            int comparisonLayer = 0, bool zeroLayerOnly = false, bool assertLeftAndRight = false)
        {
            int newTileX = container.Random.Next(0, container.AllTiles[0].GetLength(0));
            int newTileY = container.Random.Next(1, container.AllTiles[0].GetLength(0));
            if (!TileUtility.CheckIfTileAlreadyExists(newTileX, newTileY, layer, container) && TileUtility.CheckIfTileMatchesGID(newTileX, newTileY, layer,
                acceptableTiles, container, comparisonLayer))
            {
                if (zeroLayerOnly)
                {
                    if (TileUtility.CheckIfTileAlreadyExists(newTileX, newTileY, 1, container))
                    {
                        return false;
                    }
                }
                if (assertLeftAndRight)
                {
                    if (newTileX < 15 && newTileY < 15 && newTileX > 0)
                    {
                        if (!acceptableTiles.Contains(container.AllTiles[comparisonLayer][newTileX + 1, newTileY].GID) || !acceptableTiles.Contains(container.AllTiles[comparisonLayer][newTileX - 1, newTileY].GID))
                        {
                            return false;
                        }
                    }

                }

                container.AllTiles[layer][newTileX, newTileY] = new Tile(newTileX, newTileY, id);
                return true;

            }
            else

            {
                return false;
            }
        }

        #endregion


    }

    public class EditableAnimationFrame
    {
        public float CurrentDuration { get; set; }
        public float AnchorDuration { get; set; }
        public int ID { get; set; }

        public EditableAnimationFrame(AnimationFrameHolder frame)
        {
            this.CurrentDuration = frame.Duration;
            this.AnchorDuration = frame.Duration;
            this.ID = frame.Id;

        }

        public EditableAnimationFrame(float duration, float anchorDuration, int GID)
        {
            this.CurrentDuration = duration;
            this.AnchorDuration = anchorDuration;
            this.ID = GID;
        }
    }

    public class EditableAnimationFrameHolder
    {
        public List<EditableAnimationFrame> Frames { get; set; }
        public float Timer { get; set; }
        public int Counter { get; set; }
        public int OldX { get; }
        public int OldY { get; }
        public int Layer { get; set; }
        public int OriginalTileID { get; set; }
        public bool Repeats { get; set; }
        public bool SelfDestruct { get; set; }

        public bool HasNewSource { get; set; }
        public int OriginalXOffSet { get; set; }
        public int OriginalYOffSet { get; set; }
        public int OriginalWidth { get; set; }
        public int OriginalHeight { get; set; }

        public EditableAnimationFrameHolder(List<EditableAnimationFrame> frames, int oldX, int oldY, int layer, int originalTileID, bool selfDestruct = false, bool hasNewSource = false)
        {
            this.Frames = frames;
            this.Counter = 0;
            this.Timer = frames[this.Counter].AnchorDuration;
            this.OldX = oldX;
            this.OldY = oldY;
            this.Layer = layer;
            this.OriginalTileID = originalTileID;
            this.SelfDestruct = selfDestruct;
            this.HasNewSource = hasNewSource;
        }
    }
}
