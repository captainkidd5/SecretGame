using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.ObjectFolder;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
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
        public static int ChunkX = 16;
        public static int ChunkY = 16;
        #region TILING

        public static int GrassSpawnRate = 15;
        static Dictionary<int, int> DirtTiling = new Dictionary<int, int>()
        {
            {0, 705},{1,1210}, {2, 1309 },  {3, 1413}, {4, 1209}, {5, 1408},{6,707},{7, 1411}, {8, 1310}, {9, 706}, {10, 913}, {11, 1113}, {12,908}, {13,1308}, {14,911}, {15, 1006}
        };

        public static void ReassignTileForTiling(List<Tile[,]> tiles, int x, int y, int worldWidth, int worldHeight)
        {
            if (!Game1.Utility.DirtGeneratableTiles.Contains(tiles[0][x, y].GID))
            {
                return;
            }
            int keyToCheck = 0;
            if (y > 0)
            {
                if (Game1.Utility.DirtGeneratableTiles.Contains(tiles[0][x, y - 1].GID))
                {
                    keyToCheck += 1;
                }
            }

            if (y < worldHeight - 1)
            {
                if (Game1.Utility.DirtGeneratableTiles.Contains(tiles[0][x, y + 1].GID))
                {
                    keyToCheck += 8;
                }
            }

            if (x < worldWidth - 1)
            {
                if (Game1.Utility.DirtGeneratableTiles.Contains(tiles[0][x + 1, y].GID))
                {
                    keyToCheck += 4;
                }
            }

            if (x > 0)
            {
                if (Game1.Utility.DirtGeneratableTiles.Contains(tiles[0][x - 1, y].GID))
                {
                    keyToCheck += 2;
                }
            }
            if (keyToCheck == 15)
            {
                tiles[0][x, y].GID = Game1.Utility.StandardGeneratableDirtTiles[Game1.Utility.RGenerator.Next(0, Game1.Utility.StandardGeneratableDirtTiles.Count - 1)] + 1;
            }
            else
            {
                tiles[0][x, y].GID = DirtTiling[keyToCheck] + 1;
            }


        }
        #endregion
        public static Tile[,] DoSimulation(Tile[,] tiles, IInformationContainer container, int chunkX = 0, int chunkY = 0, int chunkOffSet = 0)
        {
            Tile[,] newTiles = new Tile[container.MapWidth, container.MapHeight];
            for (int i = 0; i < newTiles.GetLength(0); i++)
            {
                for (int j = 0; j < newTiles.GetLength(1); j++)
                {
                    if (chunkOffSet != 0)
                    {
                        newTiles[i, j] = new Tile(chunkX * chunkOffSet + i, chunkY * chunkOffSet + j, 1106);
                    }
                    else
                    {
                        newTiles[i, j] = new Tile(i, j, 1106);
                    }

                }
            }

            for (int i = 0; i < newTiles.GetLength(0); i++)
            {
                for (int j = 0; j < newTiles.GetLength(1); j++)
                {
                    int nbs = CountAliveNeighbors(tiles, 0, i, j);
                    if (tiles[i, j].GID != 1115)
                    {
                        if (nbs < 3)
                        {
                            newTiles[i, j].GID = newTiles[i, j].GID = Game1.Utility.DirtGeneratableTiles[Game1.Utility.RGenerator.Next(0, Game1.Utility.DirtGeneratableTiles.Count - 1)] + 1;

                        }
                        else
                        {
                            newTiles[i, j].GID = Game1.Utility.GrassGeneratableTiles[Game1.Utility.RGenerator.Next(0, Game1.Utility.GrassGeneratableTiles.Count - 1)] + 1;


                        }
                    }
                    else
                    {
                        if (nbs > 4)
                        {
                            newTiles[i, j].GID = Game1.Utility.GrassGeneratableTiles[Game1.Utility.RGenerator.Next(0, Game1.Utility.GrassGeneratableTiles.Count - 1)] + 1;

                        }
                        else
                        {
                            newTiles[i, j].GID = newTiles[i, j].GID = Game1.Utility.DirtGeneratableTiles[Game1.Utility.RGenerator.Next(0, Game1.Utility.DirtGeneratableTiles.Count - 1)] + 1;
                        }
                    }
                }
            }
            return newTiles;

        }

        public static int CountAliveNeighbors(Tile[,] tiles, int layer, int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int neighborX = x + i;
                    int neighborY = y + j;

                    if (i == 0 && j == 0)
                    {

                    }
                    else if (neighborX < 0 || neighborY < 0 || neighborX >= tiles.GetLength(0) || neighborY >= tiles.GetLength(1))
                    {
                        count++;

                    }
                    else if ((Game1.Utility.DirtGeneratableTiles.Contains(tiles[neighborX, neighborY].GID)))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public static void PlaceChests(IInformationContainer container, GraphicsDevice graphics, int chunkX = 0, int chunkY = 0)
        {
            int hiddenTreasureLimit = 5;
            for (int i = 1; i < container.AllTiles[0].GetLength(0) - 1; i++)
            {
                for (int j = 1; j < container.AllTiles[0].GetLength(1) - 1; j++)
                {
                    if (container.AllTiles[0][i, j].GID == 1115)
                    {
                        int nbs = CountAliveNeighbors(container.AllTiles[0], 1, i, j);
                        if (nbs >= hiddenTreasureLimit)
                        {

                            container.AllTiles[3][i, j - 1].GID = 1753;
                            container.AllTiles[1][i, j].GID = 1853;
                        }
                    }
                }
            }
        }


        //public static void SpawnBaseCamp(List<Tile[,]> tiles)
        //{
        //    //top and bottom fences
        //    for (int i = worldWidth /2; i < worldWidth/2 + 50; i++)
        //    {
        //        tiles[3][i, worldWidth / 2].GID = 1251;
        //        tiles[1][i, worldWidth / 2 + 1].GID = 1350;
        //        tiles[3][i, worldWidth / 2 + 50].GID = 1251;
        //        tiles[1][i, worldWidth / 2 + 51].GID = 1350;
        //    }

        //    //left and right fences
        //    for (int i = worldWidth / 2; i < worldWidth / 2 + 50; i++)
        //    {
        //        tiles[1][worldWidth / 2, i].GID = 1055;
        //        tiles[1][worldWidth / 2 + 50, i].GID = 1055;
        //        //tiles[1][i, 20].GID = 1350;
        //    }
        //    //spawn gondola platform
        //    int iCounter = 0;
        //    int jCounter = 0;
        //    for (int i = worldWidth / 2 + 5; i < worldWidth / 2 + 14; i++)
        //    {
        //        for (int j = worldWidth / 2 + 10; j < worldWidth / 2 + 17; j++)
        //        {
        //            tiles[1][i, j].GID = 3963 + jCounter + iCounter;
        //            jCounter += 100;
        //        }
        //        jCounter = 0;
        //        if(iCounter < 8)
        //        {
        //            iCounter++;
        //        }

        //    }
        //}

        public static void ReplaceTile(int layer, int tileToReplaceX, int tileToReplaceY, int newTileGID, IInformationContainer container)
        {
            Tile ReplaceMenttile = new Tile(container.AllTiles[layer][tileToReplaceX, tileToReplaceY].X, container.AllTiles[layer][tileToReplaceX, tileToReplaceY].Y, newTileGID);
            container.AllTiles[layer][tileToReplaceX, tileToReplaceY] = ReplaceMenttile;
        }

        public static bool CheckIfChunkExistsInMemory(int idX, int idY)
        {
            if (File.Exists(@"Content/SaveFiles/Chunks/Chunk" + idX + idY + ".dat"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Rectangle GetDestinationRectangle(Tile tile, int chunkX = 0, int chunkY = 0)
        {

            float X = (tile.X * 16);
            float Y = (tile.Y * 16);
            if (chunkX == 0 && chunkY == 0)
            {
                return new Rectangle((int)X, (int)Y, 16, 16);
            }
            else
            {
                return new Rectangle((int)(X + chunkX * ChunkX * ChunkX), (int)(Y + chunkY * ChunkY * ChunkY), 16, 16);
            }

        }
        public static Rectangle GetSourceRectangle(Tile tile, int tilesetTilesWide)
        {
            int Column = tile.GID % tilesetTilesWide;
            int Row = (int)Math.Floor((double)tile.GID / (double)tilesetTilesWide);

            return new Rectangle(16 * Column, 16 * Row, 16, 16);
        }

        public static void AssignProperties(Tile tileToAssign, int layer, int oldX, int oldY, IInformationContainer container)
        {
            if (container.MapName.Tilesets[container.TileSetNumber].Tiles.ContainsKey(tileToAssign.GID))
            {
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].AnimationFrames.Count > 0 && !container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("idleStart"))
                {
                    List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                    for (int i = 0; i < container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].AnimationFrames.Count; i++)
                    {
                        frames.Add(new EditableAnimationFrame(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].AnimationFrames[i]));
                    }
                    EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, oldX, oldY, layer, tileToAssign.GID);
                    container.AnimationFrames.Add(tileToAssign.GetTileKey(layer), frameHolder);
                }
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("lightSource"))
                {
                    int lightType = int.Parse(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["lightSource"]);
                    container.Lights.Add(new LightSource(lightType, new Vector2(GetDestinationRectangle(tileToAssign).X, GetDestinationRectangle(tileToAssign).Y)));
                }


                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("destructable"))
                {
                    container.TileHitPoints[tileToAssign.GetTileKey(layer)] = Game1.Utility.GetTileHitpoints(container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].Properties["destructable"]);

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

                        if (!container.Chests.ContainsKey(tileToAssign.GetTileKey(layer)))
                        {
                            container.Chests.Add(tileToAssign.GetTileKey(layer), new Chest(tileToAssign.GetTileKey(layer), 3,
                                    new Vector2(GetDestinationRectangle(tileToAssign).X, GetDestinationRectangle(tileToAssign).Y), container.GraphicsDevice, true));
                        }

                    }
                }
                if (layer == 3)
                {
                    int randomInt = Game1.Utility.RGenerator.Next(1, 1000);
                    float randomFloat = (float)(randomInt * .0000001);
                    tileToAssign.LayerToDrawAtZOffSet = (GetDestinationRectangle(tileToAssign).Top + GetDestinationRectangle(tileToAssign).Height) * .00001f + randomFloat;
                }

                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].ObjectGroups.Count > 0)
                {


                    for (int k = 0; k < container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].ObjectGroups[0].Objects.Count; k++)
                    {
                        TmxObject tempObj = container.MapName.Tilesets[container.TileSetNumber].Tiles[tileToAssign.GID].ObjectGroups[0].Objects[k];


                        ObjectBody tempObjectBody = new ObjectBody(container.GraphicsDevice,
                            new Rectangle(GetDestinationRectangle(tileToAssign).X + (int)Math.Ceiling(tempObj.X),
                            GetDestinationRectangle(tileToAssign).Y + (int)Math.Ceiling(tempObj.Y) - 5, (int)Math.Ceiling(tempObj.Width),
                            (int)Math.Ceiling(tempObj.Height) + 5), tileToAssign.GID);

                        string key = tileToAssign.GetTileKey(layer);

                        container.Objects.Add(key, tempObjectBody);


                    }
                }
            }
        }
        public static void ActionHelper(int z, int i, int j, string action, MouseManager mouse, IInformationContainer container)
        {
            //new Gid should be one larger, per usual
            string[] information = Game1.Utility.GetActionHelperInfo(action);
            Game1.Player.UserInterface.TileSelectorX = TileUtility.GetDestinationRectangle(container.AllTiles[z][i, j]).X;
            Game1.Player.UserInterface.TileSelectorY = TileUtility.GetDestinationRectangle(container.AllTiles[z][i, j]).Y;
            switch (information[0])
            {
                //including animation frame id to replace!

                case "diggable":
                    if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() == 3)
                    {
                        Game1.isMyMouseVisible = false;
                        Game1.Player.UserInterface.DrawTileSelector = true;
                        Game1.myMouseManager.ToggleGeneralInteraction = true;
                        mouse.ChangeMouseTexture(3);

                        if (mouse.IsClicked)
                        {

                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                            TileUtility.ReplaceTile(z, i, j, 86, container);


                        }
                    }
                    break;

                case "plantable":
                    Game1.isMyMouseVisible = false;
                    Game1.Player.UserInterface.DrawTileSelector = true;
                    Game1.myMouseManager.ToggleGeneralInteraction = true;
                    mouse.ChangeMouseTexture(2);

                    if (mouse.IsClicked)
                    {
                        if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem() != null)
                        {
                            Item testItem = Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem();
                            if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().IsPlantable)
                            {
                                if (!container.Crops.ContainsKey(container.AllTiles[1][i, j].GetTileKey(1)))
                                {

                                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                                    Crop tempCrop = Game1.AllCrops.GetCropFromID(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().ID);
                                    tempCrop.TileID = container.AllTiles[1][i, j].GetTileKey(1);
                                    tempCrop.GID++;
                                    TileUtility.ReplaceTile(1, i, j, tempCrop.GID, container);
                                    container.Crops[container.AllTiles[1][i, j].GetTileKey(1)] = tempCrop;
                                    Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().ID);


                                }
                            }
                        }

                    }
                    break;
                case "sanctuaryAdd":
                    if (mouse.IsClicked)
                    {
                        if (Game1.Player.Inventory.FindNumberOfItemInInventory(int.Parse(information[1])) > 0)
                        {
                            int newGID;
                            int relationX;
                            int relationY;
                            int layer;
                            int tileToReplaceGID;

                            if (Game1.SanctuaryCheckList.TryFillRequirement(container.AllTiles[z][i, j].GID))
                            {
                                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[z][i, j].GID].Properties.ContainsKey("spawnWith"))
                                {
                                    newGID = int.Parse(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[z][i, j].GID].Properties["spawnWith"]);
                                    relationX = int.Parse(container.MapName.Tilesets[container.TileSetNumber].Tiles[newGID].Properties["relationX"]);
                                    relationY = int.Parse(container.MapName.Tilesets[container.TileSetNumber].Tiles[newGID].Properties["relationY"]);
                                    layer = int.Parse(container.MapName.Tilesets[container.TileSetNumber].Tiles[newGID].Properties["layer"]);
                                    tileToReplaceGID = container.MapName.Tilesets[container.TileSetNumber].Tiles[newGID].AnimationFrames[0].Id + 1;
                                    TileUtility.ReplaceTile(layer, i + relationX, j + relationY, tileToReplaceGID, container);
                                }
                                Game1.GetCurrentStage().AddTextToAllStrings(Game1.SanctuaryCheckList.AllRequirements.Find(x => x.GID == container.AllTiles[z][i, j].GID).Name,
                                    new Vector2(TileUtility.GetDestinationRectangle(container.AllTiles[z][i, j]).X, TileUtility.GetDestinationRectangle(container.AllTiles[z][i, j]).Y - 10),
                                    TileUtility.GetDestinationRectangle(container.AllTiles[z][i, j]).X, TileUtility.GetDestinationRectangle(container.AllTiles[z][i, j]).Y - 100, 2f, 3f);


                                TileUtility.ReplaceTile(z, i, j, container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[z][i, j].GID].AnimationFrames[0].Id + 1, container);

                                Game1.Player.Inventory.RemoveItem(int.Parse(information[1]));
                                Game1.GetCurrentStage().ParticleEngine.Color = Color.LightGoldenrodYellow;
                                Game1.GetCurrentStage().ParticleEngine.ActivationTime = 1f;
                                Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(TileUtility.GetDestinationRectangle(container.AllTiles[z][i, j]).X + 10,
                                    TileUtility.GetDestinationRectangle(container.AllTiles[z][i, j]).Y - 10);

                                Game1.SoundManager.SanctuaryAdd.Play();
                            }
                        }
                    }
                    break;
                case "chestLoot":
                    if (mouse.IsClicked)
                    {
                        container.Chests[container.AllTiles[z][i, j].GetTileKey(z)].IsUpdating = true;
                    }
                    break;

                case "smelt":
                    if (mouse.IsClicked)
                    {
                        if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() != -50)
                        {


                            Item tempItem = Game1.ItemVault.GenerateNewItem(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool(), null);
                            if (tempItem.SmeltedItem != 0)
                            {
                                Game1.Player.Inventory.RemoveItem(tempItem.ID);
                                Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem(tempItem.SmeltedItem, null));
                            }
                        }
                    }

                    break;
                case "readSanctuary":
                    if (mouse.IsClicked)
                    {
                        Game1.Player.UserInterface.CurrentOpenInterfaceItem = UI.ExclusiveInterfaceItem.SanctuaryCheckList;
                    }
                    break;
                case "triggerLift":
                    if (mouse.IsClicked)
                    {
                        if (Game1.GetCurrentStage().AllSprites.Any(x => x.ID == 232) && Game1.GetCurrentStage().AllSprites.Any(x => x.ID == 233))
                        {
                            Game1.GetCurrentStage().AllSprites.Find(x => x.ID == 232).IsSpinning = true;
                            Game1.GetCurrentStage().AllSprites.Find(x => x.ID == 233).IsSpinning = true;
                            Game1.SoundManager.GearSpin.Play();
                        }
                    }
                    break;
                case "replaceLargeCog":
                    if (mouse.IsClicked)
                    {
                        if (!Game1.GetCurrentStage().AllSprites.Any(x => x.ID == 232))
                        {
                            if (Game1.Player.Inventory.FindNumberOfItemInInventory(232) > 0)
                            {
                                TileUtility.ReplaceTile(3, i, j, -1, container);
                                Game1.GetCurrentStage().AllSprites.Add(new Sprite(container.GraphicsDevice, Game1.AllTextures.Gears, new Rectangle(48, 0, 16, 16),
                                    new Vector2(TileUtility.GetDestinationRectangle(container.AllTiles[z][i, j]).X + 8,
                                    TileUtility.GetDestinationRectangle(container.AllTiles[z][i, j]).Y + 8), 16, 16)
                                { ID = 232, SpinAmount = 10f, SpinSpeed = 2f, Origin = new Vector2(8, 8) });
                                Game1.SoundManager.CraftMetal.Play();
                                Game1.Player.Inventory.RemoveItem(232);

                            }

                        }
                    }

                    break;
                case "replaceSmallCog":
                    if (mouse.IsClicked)
                    {
                        if (!Game1.GetCurrentStage().AllSprites.Any(x => x.ID == 233))
                        {
                            if (Game1.Player.Inventory.FindNumberOfItemInInventory(233) > 0)
                            {
                                TileUtility.ReplaceTile(3, i, j, -1, container);

                                Game1.GetCurrentStage().AllSprites.Add(new Sprite(container.GraphicsDevice, Game1.AllTextures.Gears, new Rectangle(16, 0, 16, 16),
                                    new Vector2(TileUtility.GetDestinationRectangle(container.AllTiles[z][i, j]).X + 8, TileUtility.GetDestinationRectangle(container.AllTiles[z][i, j]).Y + 5), 16, 16)
                                { ID = 233, SpinAmount = -10f, SpinSpeed = 2f, Origin = new Vector2(8, 8) });
                                Game1.SoundManager.CraftMetal.Play();
                                Game1.Player.Inventory.RemoveItem(233);

                            }
                        }
                    }
                    break;


            }
        }

        public static void ToolInteraction(Tile tile, GameTime gameTime, int layer, int x, int y, int setSoundInt, Color particleColor, ILocation world, Rectangle destinationRectangle, IInformationContainer container, bool hasSpawnTiles = false)
        {
            if (container.TileHitPoints.ContainsKey(container.AllTiles[layer][x, y].GetTileKey(layer)))
            {

                if (container.TileHitPoints[tile.GetTileKey(layer)] > 0)
                {
                    Game1.SoundManager.PlaySoundEffectFromInt(false, 1, setSoundInt, 1f);
                    Game1.GetCurrentStage().ParticleEngine.Color = particleColor;
                    Game1.GetCurrentStage().ParticleEngine.ActivationTime = .25f;
                    Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(destinationRectangle.X + 5, destinationRectangle.Y - 20);
                    return;
                }

                if (container.TileHitPoints[tile.GetTileKey(layer)] < 1)
                {
                    Game1.SoundManager.PlaySoundEffectFromInt(false, 1, setSoundInt, 1f);
                    container.TileHitPoints.Remove(tile.GetTileKey(layer));
                    if (hasSpawnTiles)
                    {
                        DestroySpawnWithTiles(tile, x, y, world, container);
                    }
                    if (container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties.ContainsKey("AssociatedTiles"))
                    {
                        int[] associatedTiles = Game1.Utility.ParseSpawnsWithKey(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties["AssociatedTiles"]);

                        for (int i = 0; i < associatedTiles.Length; i++)
                        {
                            if (container.MapName.Tilesets[container.TileSetNumber].Tiles.ContainsKey(associatedTiles[i]))
                            {
                                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[associatedTiles[i]].AnimationFrames.Count > 0)
                                {


                                    List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                                    for (int j = 0; j < container.MapName.Tilesets[container.TileSetNumber].Tiles[associatedTiles[i]].AnimationFrames.Count; j++)
                                    {
                                        frames.Add(new EditableAnimationFrame(container.MapName.Tilesets[container.TileSetNumber].Tiles[associatedTiles[i]].AnimationFrames[j]));
                                    }
                                    EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, x, y, layer, associatedTiles[i]);
                                    container.AnimationFrames.Add(container.AllTiles[layer][x, y - 1].GetTileKey(layer), frameHolder);
                                }
                            }
                        }
                    }
                }
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].AnimationFrames.Count > 0)
                {
                    List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                    for (int i = 0; i < container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].AnimationFrames.Count; i++)
                    {
                        frames.Add(new EditableAnimationFrame(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].AnimationFrames[i]));
                    }
                    EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, x, y, layer, container.AllTiles[layer][x, y].GID);
                    container.AnimationFrames.Add(container.AllTiles[layer][x, y].GetTileKey(layer), frameHolder);
                }
                else
                {
                    FinalizeTile(layer, gameTime, x, y, destinationRectangle, world, container);
                }

                Game1.GetCurrentStage().ParticleEngine.Color = particleColor;
                Game1.GetCurrentStage().ParticleEngine.ActivationTime = 1f;
                Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(destinationRectangle.X, destinationRectangle.Y);
            }
        }

        public static void DestroySpawnWithTiles(Tile baseTile, int xCoord, int yCoord, ILocation world, IInformationContainer container)
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

                    if (container.Objects.ContainsKey(container.AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileKey(intTilePropertyLayer)))
                    {
                        container.Objects.Remove(container.AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileKey(intTilePropertyLayer));
                    }

                    container.AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY] = new Tile(xCoord + intGidX, yCoord + intGidY, 0);
                }
            }
        }
        public static void DoPlayerAnimation(int layer, GameTime gameTime, int oldX, int oldY, int down, int right, int left, int up, Rectangle destinationRectangle, float delayTimer = 0f)
        {
            if (Game1.Player.Position.Y < destinationRectangle.Y - 30)
            {
                Game1.Player.controls.Direction = Dir.Down;
                Game1.Player.PlayAnimation(gameTime, down);
            }

            else if (Game1.Player.Position.Y > destinationRectangle.Y)
            {
                Game1.Player.controls.Direction = Dir.Up;
                Game1.Player.PlayAnimation(gameTime, up);
            }

            else if (Game1.Player.Position.X < destinationRectangle.X)
            {
                Game1.Player.controls.Direction = Dir.Right;
                Game1.Player.PlayAnimation(gameTime, right);
            }
            else if (Game1.Player.Position.X > destinationRectangle.X)
            {
                Game1.Player.controls.Direction = Dir.Left;
                Game1.Player.PlayAnimation(gameTime, left);
            }

        }
        //for destructable keyword
        public static void InteractWithBuilding(int layer, GameTime gameTime, int oldX, int oldY, Rectangle destinationRectangle, ILocation world, IInformationContainer container)
        {

            if (!container.AnimationFrames.ContainsKey(container.AllTiles[layer][oldX, oldY].GetTileKey(layer)) && !Game1.Player.CurrentAction[0, 0].IsAnimated)
            {
                int tool = Game1.Utility.GetRequiredTileTool(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].Properties["destructable"]);
                if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() == tool)
                {
                    switch (tool)
                    {
                        //bare hands
                        case -50:
                            FinalizeTile(layer, gameTime, oldX, oldY, destinationRectangle, world, container, delayTimer: .25f);
                            if (container.TileHitPoints.ContainsKey(container.AllTiles[layer][oldX, oldY].GetTileKey(layer)))
                            {
                                container.TileHitPoints[container.AllTiles[layer][oldX, oldY].GetTileKey(layer)]--;
                            }
                            break;
                        case 0:
                            DoPlayerAnimation(layer, gameTime, oldX, oldY, 9, 10, 11, 12, destinationRectangle, .25f);
                            ToolInteraction(container.AllTiles[layer][oldX, oldY], gameTime, layer, oldX, oldY, Game1.Utility.GetTileDestructionSound(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].Properties["destructable"]),
                                Game1.Utility.GetTileEffectColor(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].Properties["destructable"]),
                                world, destinationRectangle, container, container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"));
                            if (container.TileHitPoints.ContainsKey(container.AllTiles[layer][oldX, oldY].GetTileKey(layer)))
                            {
                                container.TileHitPoints[container.AllTiles[layer][oldX, oldY].GetTileKey(layer)]--;

                            }

                            break;

                        case 1:

                            DoPlayerAnimation(layer, gameTime, oldX, oldY, 5, 6, 7, 8, destinationRectangle, .25f);
                            ToolInteraction(container.AllTiles[layer][oldX, oldY], gameTime, layer, oldX, oldY, Game1.Utility.GetTileDestructionSound(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].Properties["destructable"]),
                                Game1.Utility.GetTileEffectColor(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), world, destinationRectangle, container,
                                container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"));
                            if (container.TileHitPoints.ContainsKey(container.AllTiles[layer][oldX, oldY].GetTileKey(layer)))
                            {
                                container.TileHitPoints[container.AllTiles[layer][oldX, oldY].GetTileKey(layer)]--;
                            }
                            break;
                        case 2:
                            DoPlayerAnimation(layer, gameTime, oldX, oldY, 1, 2, 3, 4, destinationRectangle, .25f);
                            ToolInteraction(container.AllTiles[layer][oldX, oldY], gameTime, layer, oldX, oldY, Game1.Utility.GetTileDestructionSound(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].Properties["destructable"]),
                                Game1.Utility.GetTileEffectColor(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), world,
                                destinationRectangle, container, container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"));
                            if (container.TileHitPoints.ContainsKey(container.AllTiles[layer][oldX, oldY].GetTileKey(layer)))
                            {
                                container.TileHitPoints[container.AllTiles[layer][oldX, oldY].GetTileKey(layer)]--;
                            }
                            break;
                    }

                }

            }


        }

        public static void GetDrop(int layer, int x, int y, Rectangle destinationRectangle, IInformationContainer container)
        {
            int gid = container.AllTiles[layer][x, y].GID;
            List<Loot> tempLoot = Game1.Utility.Parselootkey(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][x, y].GID].Properties["loot"]);

            if (tempLoot != null)
            {


                for (int l = 0; l < tempLoot.Count; l++)
                {
                    int lootCount = Game1.Utility.DetermineLootDrop(tempLoot[l]);
                    for (int d = 0; d < lootCount; d++)
                    {
                        Item item = Game1.ItemVault.GenerateNewItem(tempLoot[l].ID, new Vector2(destinationRectangle.X, destinationRectangle.Y), true);
                        Game1.GetCurrentStage().AllItems.Add(item);
                    }
                }
            }

        }

        public static void FinalizeTile(int layer, GameTime gameTime, int oldX, int oldY, Rectangle destinationRectangle, ILocation world, IInformationContainer container, float delayTimer = 0f)
        {
            if (!container.AnimationFrames.ContainsKey(container.AllTiles[layer][oldX, oldY].GetTileKey(layer)))
            {


                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].AnimationFrames.Count > 0)
                {
                    List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                    for (int i = 0; i < container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].AnimationFrames.Count; i++)
                    {
                        frames.Add(new EditableAnimationFrame(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].AnimationFrames[i]));
                    }
                    EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, oldX, oldY, layer, container.AllTiles[layer][oldX, oldY].GID);
                    container.AnimationFrames.Add(container.AllTiles[layer][oldX, oldY].GetTileKey(layer), frameHolder);
                }
            }

            if (container.Objects.ContainsKey(container.AllTiles[layer][oldX, oldY].GetTileKey(layer)))
            {
                container.Objects.Remove(container.AllTiles[layer][oldX, oldY].GetTileKey(layer));
            }
            if (container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"))
            {

                DestroySpawnWithTiles(container.AllTiles[layer][oldX, oldY], oldX, oldY, world, container);
            }
            //mostly for crops

            GetDrop(layer, oldX, oldY, destinationRectangle, container);
            if (container.Crops.ContainsKey(container.AllTiles[1][oldX, oldY].GetTileKey(layer)))
            {
                container.Crops.Remove(container.AllTiles[1][oldX, oldY].GetTileKey(layer));
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("AssociatedTiles"))
                {
                    TileUtility.ReplaceTilePermanent(3, oldX, oldY - 1, 0, world, container);
                }
            }
            Game1.SoundManager.PlaySoundEffectFromInt(false, 1, Game1.Utility.GetTileDestructionSound(container.MapName.Tilesets[container.TileSetNumber].Tiles[container.AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), 1f);

            TileUtility.ReplaceTilePermanent(layer, oldX, oldY, 0, world, container);


        }
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

        public static Rectangle GetSourceRectangleWithoutTile(int gid, int tilesetTilesWide)
        {
            int Column = gid % tilesetTilesWide;
            int Row = (int)Math.Floor((double)gid / (double)tilesetTilesWide);

            return new Rectangle(16 * Column, 16 * Row, 16, 16);
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
        public static void GenerateTiles(int layerToPlace, int gid, string placementKey, int frequency, int layerToCheckIfEmpty, IInformationContainer container)
        {
            List<int> acceptableGenerationTiles;
            switch (placementKey)
            {
                case "dirt":
                    acceptableGenerationTiles = Game1.Utility.DirtGeneratableTiles;

                    break;
                case "sand":
                    acceptableGenerationTiles = Game1.Utility.SandGeneratableTiles;

                    break;
                default:
                    acceptableGenerationTiles = Game1.Utility.DirtGeneratableTiles;

                    break;
            }

            for (int g = 0; g < frequency; g++)
            {
                GenerateRandomTiles(layerToPlace, gid, acceptableGenerationTiles, container, layerToCheckIfEmpty);
            }
        }

        public static void GenerateRandomTiles(int layer, int id, List<int> acceptableTiles, IInformationContainer container,
            int comparisonLayer = 0)
        {
            int newTileX = Game1.Utility.RNumber(1, container.AllTiles[0].GetLength(0) - 1);
            int newTileY = Game1.Utility.RNumber(1, container.AllTiles[0].GetLength(0) - 1);
            if (!TileUtility.CheckIfTileAlreadyExists(newTileX, newTileY, layer, container) && TileUtility.CheckIfTileMatchesGID(newTileX, newTileY, layer,
                acceptableTiles, container, comparisonLayer))
            {
                Tile sampleTile = new Tile(newTileX, newTileY, id);
                if (!container.MapName.Tilesets[container.TileSetNumber].Tiles[sampleTile.GID].Properties.ContainsKey("spawnWith"))
                {
                    container.AllTiles[layer][newTileX, newTileY] = new Tile(newTileX, newTileY, id);
                    return;
                }
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[sampleTile.GID].Properties.ContainsKey("spawnWith"))
                {
                    string value = "";
                    container.MapName.Tilesets[container.TileSetNumber].Tiles[sampleTile.GID].Properties.TryGetValue("spawnWith", out value);

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
                        if (!TileUtility.CheckIfTileAlreadyExists(newTileX + intGidX, newTileY + intGidY, layer, container))
                        {
                            intermediateNewTiles.Add(new Tile(newTileX + intGidX, newTileY + intGidY, totalGID + 1) { LayerToDrawAt = intTilePropertyLayer });
                        }
                        else
                        {
                            return;
                        }
                    }

                    for (int tileSwapCounter = 0; tileSwapCounter < intermediateNewTiles.Count; tileSwapCounter++)
                    {
                        TileUtility.AssignProperties(intermediateNewTiles[tileSwapCounter], layer, (int)intermediateNewTiles[tileSwapCounter].X, (int)intermediateNewTiles[tileSwapCounter].Y, container);
                        container.AllTiles[(int)intermediateNewTiles[tileSwapCounter].LayerToDrawAt][(int)intermediateNewTiles[tileSwapCounter].X,
                            (int)intermediateNewTiles[tileSwapCounter].Y] = intermediateNewTiles[tileSwapCounter];
                    }
                    container.AllTiles[layer][newTileX, newTileY] = new Tile(newTileX, newTileY, id);
                }
            }
        }
        #endregion

        public static void UpdateCropTile(Crop crop, ILocation stage, IInformationContainer container)
        {
            string tileID = crop.TileID; ;
            int x = int.Parse(tileID.Substring(1, 4));
            int y = int.Parse(tileID.Substring(5, 4));
            TileUtility.ReplaceTilePermanent(1, x, y, crop.GID, stage, container);
            if (container.MapName.Tilesets[container.TileSetNumber].Tiles.ContainsKey(crop.GID - 1))
            {
                if (container.MapName.Tilesets[container.TileSetNumber].Tiles[crop.GID - 1].Properties.ContainsKey("AssociatedTiles"))
                {
                    TileUtility.ReplaceTilePermanent(3, x, y - 1, int.Parse(container.MapName.Tilesets[container.TileSetNumber].Tiles[crop.GID - 1].Properties["AssociatedTiles"]), stage, container);
                }
            }
        }

        #region GRIDITEMS
        public static void DrawGridItem(SpriteBatch spriteBatch, ITileManager tileManager, List<Tile[,]> tiles, IInformationContainer container)
        {
            if (tileManager.AbleToDrawTileSelector)
            {
                if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem() != null)
                {


                    if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().PlaceID != 0)
                    {
                        int[] associatedTiles = new int[0];
                        int placeID = Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().PlaceID;
                        Rectangle sourceRectangle = TileUtility.GetSourceRectangleWithoutTile(placeID, container.TileSetDimension);
                        if (tiles[1][Game1.Player.UserInterface.TileSelectorX / 16, Game1.Player.UserInterface.TileSelectorY / 16].GID != -1)
                        {

                            spriteBatch.Draw(tileManager.TileSet, new Vector2(Game1.Player.UserInterface.TileSelectorX, Game1.Player.UserInterface.TileSelectorY), sourceRectangle, Color.Red * .5f,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[1]);
                            if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles.ContainsKey(placeID))
                            {
                                if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[placeID].Properties.ContainsKey("AssociatedTiles"))
                                {

                                    associatedTiles = Game1.Utility.ParseSpawnsWithKey(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[placeID].Properties["AssociatedTiles"]);
                                }
                            }
                        }

                        else
                        {
                            spriteBatch.Draw(tileManager.TileSet, new Vector2(Game1.Player.UserInterface.TileSelectorX, Game1.Player.UserInterface.TileSelectorY), sourceRectangle, Color.Green * .5f,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[1]);
                            if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles.ContainsKey(placeID))
                            {


                                if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[placeID].Properties.ContainsKey("AssociatedTiles"))
                                {

                                    associatedTiles = Game1.Utility.ParseSpawnsWithKey(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[placeID].Properties["AssociatedTiles"]);
                                    for (int a = 0; a < associatedTiles.Length; a++)
                                    {
                                        spriteBatch.Draw(tileManager.TileSet, new Vector2(Game1.Player.UserInterface.TileSelectorX + int.Parse(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[associatedTiles[a]].Properties["relationX"]) * 16,
                                            Game1.Player.UserInterface.TileSelectorY + int.Parse(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[associatedTiles[a]].Properties["relationY"]) * 16), TileUtility.GetSourceRectangleWithoutTile(associatedTiles[a], container.TileSetDimension), Color.Green * .5f,
                                            0f, Game1.Utility.Origin, 1f, SpriteEffects.None, tileManager.AllDepths[int.Parse(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[associatedTiles[a]].Properties["layer"])]);
                                    }
                                }
                            }
                            if (Game1.myMouseManager.IsClicked)
                            {
                                if (Game1.Player.UserInterface.CurrentOpenInterfaceItem != UI.ExclusiveInterfaceItem.ShopMenu)
                                {


                                    if (associatedTiles.Length > 0)
                                    {
                                        for (int a = 0; a < associatedTiles.Length; a++)
                                        {
                                            ReplaceTilePermanent(int.Parse(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[associatedTiles[a]].Properties["layer"]), Game1.Player.UserInterface.TileSelectorX / 16 + int.Parse(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[associatedTiles[a]].Properties["relationX"]),
                                                Game1.Player.UserInterface.TileSelectorY / 16 + int.Parse(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[associatedTiles[a]].Properties["relationY"]), associatedTiles[a] + 1, Game1.GetCurrentStage(), container);
                                        }
                                    }
                                    int soundRandom = Game1.Utility.RGenerator.Next(0, 2);
                                    switch (soundRandom)
                                    {
                                        case 0:
                                            Game1.SoundManager.PlaceItem1.Play();
                                            break;
                                        case 1:
                                            Game1.SoundManager.PlaceItem2.Play();
                                            break;
                                    }
                                    ReplaceTilePermanent(1, Game1.Player.UserInterface.TileSelectorX / 16, Game1.Player.UserInterface.TileSelectorY / 16, placeID + 1, Game1.GetCurrentStage(), container);
                                    Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool());
                                }
                            }
                        }


                    }

                }

            }



        }
        #endregion
        public static void ReplaceTilePermanent(int layer, int oldX, int oldY, int gid, ILocation stage, IInformationContainer container)
        {
            Tile ReplaceMenttile = new Tile(container.AllTiles[layer][oldX, oldY].X, container.AllTiles[layer][oldX, oldY].Y, gid);
            container.AllTiles[layer][oldX, oldY] = ReplaceMenttile;
            TileUtility.AssignProperties(container.AllTiles[layer][oldX, oldY], layer,
                oldX, oldY, container);
        }
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

        public EditableAnimationFrameHolder(List<EditableAnimationFrame> frames, int oldX, int oldY, int layer, int originalTileID)
        {
            this.Frames = frames;
            this.Counter = 0;
            this.Timer = frames[Counter].AnchorDuration;
            this.OldX = oldX;
            this.OldY = oldY;
            this.Layer = layer;
            this.OriginalTileID = originalTileID;
        }
    }
}
