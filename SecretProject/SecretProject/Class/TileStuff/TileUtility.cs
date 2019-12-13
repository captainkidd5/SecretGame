using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   
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
        /// <param name="tileY">The tile's SQUARE (meaning worldPos /16 ) X Coord!</param>
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

            int localX = (int)Math.Floor((float)(tileX  - chunkX * 16));
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
        /// <param name="oldY"></param>
        /// <param name="container"></param>
        public static void AssignProperties(Tile tileToAssign, int layer, int x, int oldY, IInformationContainer container)
        {

            tileToAssign.DestinationRectangle = GetDestinationRectangle(tileToAssign);
            tileToAssign.SourceRectangle = GetSourceRectangle(tileToAssign, container.TileSetDimension);

            if (container.MapName.Tilesets[container.TileSetNumber].Tiles.ContainsKey(tileToAssign.GID))
            {

                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("spawnWith"))
                {
                    string value = "";
                    container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.TryGetValue("spawnWith", out value);

                    List<Tile> intermediateNewTiles = new List<Tile>();
                    int[] spawnsWith = Game1.Utility.ParseSpawnsWithKey(value);
                    for (int index = 0; index < spawnsWith.Length; index++)
                    {
                        string gidX = "";
                        container.MapName.Tilesets[container.TileSetNumber].Tiles[spawnsWith[index]].Properties.TryGetValue("relationX", out gidX);
                        string gidY = "";
                        container.MapName.Tilesets[container.TileSetNumber].Tiles[spawnsWith[index]].Properties.TryGetValue("relationY", out gidY);
                        string tilePropertyLayer = "";
                        container.MapName.Tilesets[container.TileSetNumber].Tiles[spawnsWith[index]].Properties.TryGetValue("layer", out tilePropertyLayer);
                        int intGidX = int.Parse(gidX);
                        int intGidY = int.Parse(gidY);
                        int intTilePropertyLayer = int.Parse(tilePropertyLayer);
                        int totalGID = container.MapName.Tilesets[container.TileSetNumber].Tiles[spawnsWith[index]].Id;
                        //basically, if any tile in the associated tiles already contains a tile in the same layer we'll just stop

                        intermediateNewTiles.Add(new Tile(x + intGidX, oldY + intGidY, totalGID + 1) { LayerToDrawAt = intTilePropertyLayer });
                    }

                    if (x != 79)
                    {
                        for (int tileSwapCounter = 0; tileSwapCounter < intermediateNewTiles.Count; tileSwapCounter++)
                        {
                            TileUtility.AssignProperties(intermediateNewTiles[tileSwapCounter], layer, (int)intermediateNewTiles[tileSwapCounter].X, (int)intermediateNewTiles[tileSwapCounter].Y, container);
                            container.AllTiles[(int)intermediateNewTiles[tileSwapCounter].LayerToDrawAt][(int)intermediateNewTiles[tileSwapCounter].X,
                                (int)intermediateNewTiles[tileSwapCounter].Y] = intermediateNewTiles[tileSwapCounter];
                        }
                    }

                }
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].AnimationFrames.Count > 0 && !container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("idleStart"))
                {

                    List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                   // frames.Add(new EditableAnimationFrame(new AnimationFrameHolder();
                    for (int i = 0; i < container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].AnimationFrames.Count; i++)
                    {
                        frames.Add(new EditableAnimationFrame(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].AnimationFrames[i]));
                    }
                    EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, x, oldY, layer, tileToAssign.GID);
                    container.AnimationFrames.Add(tileToAssign.GetTileKeyStringNew(layer, container), frameHolder);
                }
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("lightSource"))
                {
                    int lightType = LightSource.ParseLightType(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["lightSource"]);
                    Vector2 lightOffSet = LightSource.ParseLightData(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["lightSource"]);
                    container.Lights.Add(new LightSource(lightType, new Vector2(GetDestinationRectangle(tileToAssign).X + lightOffSet.X, GetDestinationRectangle(tileToAssign).Y + lightOffSet.Y)));
                }


                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("destructable"))
                {
                    container.TileHitPoints[tileToAssign.GetTileKeyStringNew(layer, container)] = Game1.Utility.GetTileHitpoints(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["destructable"]);

                }

                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("layer"))
                {
                    tileToAssign.LayerToDrawAt = int.Parse(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["layer"]);
                    //grass = 1, stone = 2, wood = 3, sand = 4
                }

                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("action"))
                {
                    if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["action"] == "chestLoot")
                    {

                        if (!container.StoreableItems.ContainsKey(tileToAssign.GetTileKeyStringNew(layer, container)))
                        {
                            container.StoreableItems.Add(tileToAssign.GetTileKeyStringNew(layer, container), new Chest(tileToAssign.GetTileKeyStringNew(layer, container), 6,
                                    new Vector2(GetDestinationRectangle(tileToAssign).X, GetDestinationRectangle(tileToAssign).Y), container.GraphicsDevice, false));
                        }

                    }
                    else if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["action"] == "cook")
                    {

                        if (!container.StoreableItems.ContainsKey(tileToAssign.GetTileKeyStringNew(layer, container)))
                        {
                            container.StoreableItems.Add(tileToAssign.GetTileKeyStringNew(layer, container), new Cauldron(tileToAssign.GetTileKeyStringNew(layer, container), 3,
                                    new Vector2(GetDestinationRectangle(tileToAssign).X, GetDestinationRectangle(tileToAssign).Y), container.GraphicsDevice));
                        }

                    }
                    else if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["action"] == "smelt")
                    {

                        if (!container.StoreableItems.ContainsKey(tileToAssign.GetTileKeyStringNew(layer, container)))
                        {
                            container.StoreableItems.Add(tileToAssign.GetTileKeyStringNew(layer, container), new Furnace(tileToAssign.GetTileKeyStringNew(layer, container), 3,
                                    new Vector2(GetDestinationRectangle(tileToAssign).X, GetDestinationRectangle(tileToAssign).Y), container.GraphicsDevice));
                        }

                    }
                }
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("newSource"))
                {
                    int[] rectangleCoords = GetNewTileSourceRectangle(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["newSource"]);


                    tileToAssign.SourceRectangle = new Rectangle(tileToAssign.SourceRectangle.X + rectangleCoords[0], tileToAssign.SourceRectangle.Y + rectangleCoords[1],
                        rectangleCoords[2], rectangleCoords[3]);


                    tileToAssign.DestinationRectangle = new Rectangle(tileToAssign.DestinationRectangle.X + rectangleCoords[0], tileToAssign.DestinationRectangle.Y + rectangleCoords[1],
                       rectangleCoords[2], rectangleCoords[3]);
                }
                if (layer == 3)
                {
                    float randomOffSet = Game1.Utility.RFloat(Game1.Utility.ForeGroundMultiplier, .0000001f);
                    float offSetDrawValue = (GetDestinationRectangle(tileToAssign).Y + 16) * Game1.Utility.ForeGroundMultiplier + randomOffSet;

                    tileToAssign.LayerToDrawAtZOffSet = offSetDrawValue;
                }


                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("newHitBox"))
                {
                    int[] rectangleCoords = GetNewTileSourceRectangle(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["newHitBox"]);
                    string key = tileToAssign.GetTileKeyStringNew(layer, container);

                    Collider tempObjectBody = new Collider(container.GraphicsDevice, new Vector2(0, 0),
                            new Rectangle(GetDestinationRectangle(tileToAssign).X + rectangleCoords[0],
                            GetDestinationRectangle(tileToAssign).Y + rectangleCoords[1], rectangleCoords[2],
                            rectangleCoords[3]), null, ColliderType.inert)
                    { LocationKey = key };

                    if (container.Objects.ContainsKey(key))
                    {

                    }
                    else
                    {
                        container.Objects.Add(key, new List<ICollidable>());
                    }
                    container.Objects[key].Add(tempObjectBody);


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
                                container.PathGrid.UpdateGrid(x + i, oldY + j, 0);
                            }
                        }
                    }
                }

                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].ObjectGroups.Count > 0)
                {

                    container.PathGrid.UpdateGrid(x, oldY, 0);

                    string key = tileToAssign.GetTileKeyStringNew(layer, container);
                    if (container.Objects.ContainsKey(key))
                    {

                    }
                    else
                    {
                        container.Objects.Add(key, new List<ICollidable>());
                    }

                    for (int k = 0; k < container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].ObjectGroups[0].Objects.Count; k++)
                    {
                        TmxObject tempObj = container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].ObjectGroups[0].Objects[k];


                        Collider tempObjectBody = new Collider(container.GraphicsDevice, new Vector2(0, 0),
                            new Rectangle(GetDestinationRectangle(tileToAssign).X + (int)Math.Ceiling(tempObj.X),
                            GetDestinationRectangle(tileToAssign).Y + (int)Math.Ceiling(tempObj.Y) - 5, (int)Math.Ceiling(tempObj.Width),
                            (int)Math.Ceiling(tempObj.Height) + 5), null, ColliderType.inert)
                        { LocationKey = key };



                        container.Objects[key].Add(tempObjectBody);

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
                        if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem() != null)
                        {
                            if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().Type == 23)
                            {
                                Game1.Player.UserInterface.DrawTileSelector = true;

                                mouse.ChangeMouseTexture(CursorType.Digging);

                                if (mouse.IsClicked)
                                {
                                    switch (container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[z][i, j].GID].Properties["generate"])
                                    {
                                        case "dirt":
                                            Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.DigDirt);
                                            TileUtility.ReplaceTile(z, i, j, 86, container);
                                            break;
                                        case "dirtBasic":
                                            Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.DigDirt);
                                            TileUtility.ReplaceTile(z, i, j, 86, container);
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
                        }
                    }
                    break;

                case "plantable":
                    Game1.Player.UserInterface.DrawTileSelector = true;
                  

                    if (mouse.IsClicked)
                    {
                        if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem() != null)
                        {
                            Item testItem = Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem();
                            if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().IsPlantable)
                            {
                                mouse.ChangeMouseTexture(CursorType.Planting);
                                if (!container.Crops.ContainsKey(container.AllTiles[1][i, j].GetTileKeyStringNew(1, container)))
                                {

                                    Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.DigDirt);
                                    Crop tempCrop = Game1.AllCrops.GetCropFromID(Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ID);
                                    tempCrop.DayPlanted = Game1.GlobalClock.TotalDays;
                                    tempCrop.GID++;
                                    tempCrop.X = i;
                                    tempCrop.Y = j;
                                    TileUtility.ReplaceTile(1, i, j, tempCrop.GID, container);
                                    container.Crops[container.AllTiles[1][i, j].GetTileKeyStringNew(1, container)] = tempCrop;
                                    Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ID);
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
                        Game1.GlobalClock.IncrementDay();
                        Game1.GlobalClock.TotalHours = 6;
                    }
                    break;
                
                case "chestLoot":
                    if (mouse.IsClicked)
                    {
                        Game1.Player.UserInterface.CurrentAccessedStorableItem = container.StoreableItems[container.AllTiles[z][i, j].GetTileKeyStringNew(z, container)];
                        Game1.Player.UserInterface.CurrentAccessedStorableItem.IsUpdating = true;
                    }
                    break;

                //case "smelt":
                //    if (mouse.IsClicked)
                //    {
                //        if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool() != -50)
                //        {


                //            Item tempItem = Game1.ItemVault.GenerateNewItem(Game1.Player.UserInterface.BackPack.GetCurrentEquippedTool(), null);
                //            if (tempItem.SmeltedItem != 0)
                //            {
                //                Game1.Player.Inventory.RemoveItem(tempItem.ID);
                //                Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem(tempItem.SmeltedItem, null));
                //                Game1.SoundManager.FurnaceLight.Play();
                //            }
                //        }
                //    }

                //    break;

                case "triggerLift":
                    if (mouse.IsClicked)
                    {
                        if (Game1.GetCurrentStageInt() == Stages.World)
                        {
                            Game1.Player.UserInterface.WarpGate.To = Stages.Town;
                        }
                        else if (Game1.GetCurrentStageInt() == Stages.Town)
                        {
                            Game1.Player.UserInterface.WarpGate.To = Stages.World;
                        }

                        Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.WarpGate;

                    }
                    break;

                case "openProgressBook":
                    mouse.ChangeMouseTexture(CursorType.Normal);
                    if (mouse.IsClicked)
                    {
                        switch (Game1.GetCurrentStageInt())
                        {
                            case Stages.JulianHouse:
                                Game1.Player.UserInterface.ActivateProgressBook(UI.CurrentOpenProgressBook.Julian);
                                break;
                            case Stages.ElixirHouse:
                                Game1.Player.UserInterface.ActivateProgressBook(UI.CurrentOpenProgressBook.Elixir);
                                break;
                        }

                    }

                    break;

                case "cook":
                    mouse.ChangeMouseTexture(CursorType.Normal);
                    if (mouse.IsClicked)
                    {
                        Game1.Player.UserInterface.CurrentAccessedStorableItem = container.StoreableItems[container.AllTiles[z][i, j].GetTileKeyStringNew(z, container)];
                        Game1.Player.UserInterface.CurrentAccessedStorableItem.IsUpdating = true;
                       

                    }
                    break;
                case "smelt":
                    mouse.ChangeMouseTexture(CursorType.Normal);
                    if (mouse.IsClicked)
                    {
                        Game1.Player.UserInterface.CurrentAccessedStorableItem = container.StoreableItems[container.AllTiles[z][i, j].GetTileKeyStringNew(z, container)];
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

            }
        }

       

        #region TILEREPLACEMENT AND INTERACTIONS
        public static void ReplaceTile(int layer, int tileToReplaceX, int tileToReplaceY, int newTileGID, IInformationContainer container)
        {
            Tile ReplaceMenttile = new Tile(container.AllTiles[layer][tileToReplaceX, tileToReplaceY].X, container.AllTiles[layer][tileToReplaceX, tileToReplaceY].Y, newTileGID);
            AssignProperties(ReplaceMenttile, layer, tileToReplaceX, tileToReplaceY, container);
           
            container.AllTiles[layer][tileToReplaceX, tileToReplaceY] = ReplaceMenttile;
        }

        public static void ToolInteraction(Tile tile, GameTime gameTime, int layer, int x, int y, int setSoundInt, Color particleColor, Rectangle destinationRectangle, IInformationContainer container, bool hasSpawnTiles = false)
        {
            string keyString = container.AllTiles[layer][x, y].GetTileKeyStringNew(layer, container);

            if (container.TileHitPoints.ContainsKey(keyString))
            {

                if (container.TileHitPoints[keyString] > 0)
                {
                    Game1.SoundManager.PlaySoundEffectFromInt(1, setSoundInt);
                    Game1.GetCurrentStage().ParticleEngine.Activate(.25f, new Vector2(destinationRectangle.X + 5, destinationRectangle.Y - 20), particleColor, tile.LayerToDrawAt + tile.LayerToDrawAtZOffSet);
                    return;
                }

                if (container.TileHitPoints[keyString] < 1)
                {
                    Game1.SoundManager.PlaySoundEffectFromInt(1, setSoundInt);
                    container.TileHitPoints.Remove(tile.GetTileKeyStringNew(layer, container));
                    if (hasSpawnTiles)
                    {
                        DestroySpawnWithTiles(tile, x, y, container);
                    }
                }
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].AnimationFrames.Count > 0)
                {
                    List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                    for (int i = 0; i < container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].AnimationFrames.Count; i++)
                    {
                        frames.Add(new EditableAnimationFrame(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].AnimationFrames[i]));
                    }
                    EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, x, y, layer, container.AllTiles[layer][x, y].GID, true);
                    container.AnimationFrames.Add(keyString, frameHolder);
                }
                else
                {
                    FinalizeTile(layer, gameTime, x, y, container);
                }

                Game1.GetCurrentStage().ParticleEngine.Activate(1f, new Vector2(destinationRectangle.X + 5, destinationRectangle.Y - 20), particleColor, tile.LayerToDrawAt + tile.LayerToDrawAtZOffSet);
            }
        }

        public static void DestroySpawnWithTiles(Tile baseTile, int xCoord, int yCoord,IInformationContainer container)
        {
            List<Tile> tilesToReturn = new List<Tile>();
            string value = "";
            container.MapName.Tilesets[container.TileSetNumber].Tiles[baseTile.GID].Properties.TryGetValue("spawnWith", out value);

            int[] spawnsWith = Game1.Utility.ParseSpawnsWithKey(value);
            if (spawnsWith != null)
            {
                for (int i = 0; i < spawnsWith.Length; i++)
                {
                    string gidX = "";
                    container.MapName.Tilesets[container.TileSetNumber].Tiles[spawnsWith[i]].Properties.TryGetValue("relationX", out gidX);
                    string gidY = "";
                    container.MapName.Tilesets[container.TileSetNumber].Tiles[spawnsWith[i]].Properties.TryGetValue("relationY", out gidY);
                    string tilePropertyLayer = "";
                    container.MapName.Tilesets[container.TileSetNumber].Tiles[spawnsWith[i]].Properties.TryGetValue("layer", out tilePropertyLayer);
                    int intGidX = int.Parse(gidX);
                    int intGidY = int.Parse(gidY);
                    int intTilePropertyLayer = int.Parse(tilePropertyLayer);

                    int totalGID = container.MapName.Tilesets[container.TileSetNumber].Tiles[spawnsWith[i]].Id;
                    if(container.Objects.ContainsKey(container.AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileKeyStringNew(intTilePropertyLayer, container)))
                    {
                        container.Objects.Remove(container.AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileKeyStringNew(intTilePropertyLayer, container));
                    }

                    container.AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY] = new Tile(xCoord + intGidX, yCoord + intGidY, 0);
                }
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

            if (!container.AnimationFrames.ContainsKey(container.AllTiles[layer][x, y].GetTileKeyStringNew(layer, container)) && !Game1.Player.IsPerformingAction)
            {
                AnimationType actionType = Game1.Utility.GetRequiredTileTool(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties["destructable"]);
               
                if (actionType == AnimationType.HandsPicking)  //this is out here because any equipped item should be able to pick it up no matter what
                {
                    FinalizeTile(layer, gameTime, x, y, container, delayTimer: .25f);
                    if (container.TileHitPoints.ContainsKey(container.AllTiles[layer][x, y].GetTileKeyStringNew(layer, container)))
                    {
                        container.TileHitPoints[container.AllTiles[layer][x, y].GetTileKeyStringNew(layer, container)]--;
                    }

                }
                else if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem() != null)
                {
                    if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().Type == (int)actionType)
                    {


                        Game1.Player.DoPlayerAnimation(gameTime, actionType, .25f, Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem());
                        ToolInteraction(container.AllTiles[layer][x, y], gameTime, layer, x, y, Game1.Utility.GetTileDestructionSound(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties["destructable"]),
                            Game1.Utility.GetTileEffectColor(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties["destructable"]), destinationRectangle, container,
                            container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties.ContainsKey("spawnWith"));
                        Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().AlterDurability(1);
                        if (container.TileHitPoints.ContainsKey(container.AllTiles[layer][x, y].GetTileKeyStringNew(layer, container)))
                        {
                            container.TileHitPoints[container.AllTiles[layer][x, y].GetTileKeyStringNew(layer, container)]--;


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
           

            if (container.Objects.ContainsKey(container.AllTiles[layer][x, y].GetTileKeyStringNew(layer, container)))
            {
                container.Objects.Remove(container.AllTiles[layer][x, y].GetTileKeyStringNew(layer, container));
            }

            if (container.TileHitPoints.ContainsKey(container.AllTiles[layer][x, y].GetTileKeyStringNew(layer, container)))
            {
                container.TileHitPoints.Remove(container.AllTiles[layer][x, y].GetTileKeyStringNew(layer, container));
            }
            if (container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties.ContainsKey("spawnWith"))
            {

                DestroySpawnWithTiles(container.AllTiles[layer][x, y], x, y, container);
            }
            Item itemToCheckForReassasignTiling = null;
            if (container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties.ContainsKey("loot"))
            {
                List<Loot> tempLoot = Loot.Parselootkey(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties["loot"]);
                itemToCheckForReassasignTiling = Loot.GetDrop(tempLoot, container.AllTiles[layer][x, y].DestinationRectangle);
            }
            
            if (container.Crops.ContainsKey(container.AllTiles[1][x, y].GetTileKeyStringNew(layer, container)))
            {
                container.Crops.Remove(container.AllTiles[1][x, y].GetTileKeyStringNew(layer, container));

                Game1.SoundManager.PlaySoundEffectFromInt(1, Game1.Utility.GetTileDestructionSound(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties["destructable"]));
            }


            TileUtility.ReplaceTile(layer, x, y, 0, container);

           // this is used to see if that tile should tell other tiles around it to check their tiling, as this one may affect it.
            if(itemToCheckForReassasignTiling != null)
            {
                if(itemToCheckForReassasignTiling.GenerationType != 0)
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
        public static void GenerateRandomlyDistributedTiles(int layerToPlace, int gid, GenerationType type, int frequency, int layerToCheckIfEmpty, IInformationContainer container)
        {
            int cap = Game1.Utility.RGenerator.Next(0, frequency);

            for (int g = 0; g < cap; g++)
            {
                RetrieveRandomlyDistributedTile(layerToPlace, gid, Game1.Procedural.GetTilingContainerFromGenerationType(type).GeneratableTiles, container, layerToCheckIfEmpty);
            }
        }
        public static void RetrieveRandomlyDistributedTile(int layer, int id, List<int> acceptableTiles, IInformationContainer container,
            int comparisonLayer = 0)
        {
            int newTileX = Game1.Utility.RNumber(1, container.AllTiles[0].GetLength(0) - 1);
            int newTileY = Game1.Utility.RNumber(1, container.AllTiles[0].GetLength(0) - 1);
            if (!TileUtility.CheckIfTileAlreadyExists(newTileX, newTileY, layer, container) && TileUtility.CheckIfTileMatchesGID(newTileX, newTileY, layer,
                acceptableTiles, container, comparisonLayer))
            {

                container.AllTiles[layer][newTileX, newTileY] = new Tile(newTileX, newTileY, id);

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

        public EditableAnimationFrameHolder(List<EditableAnimationFrame> frames, int oldX, int oldY, int layer, int originalTileID, bool selfDestruct = false)
        {
            this.Frames = frames;
            this.Counter = 0;
            this.Timer = frames[Counter].AnchorDuration;
            this.OldX = oldX;
            this.OldY = oldY;
            this.Layer = layer;
            this.OriginalTileID = originalTileID;
            this.SelfDestruct = selfDestruct;
        }
    }
}
