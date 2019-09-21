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
        public static Tile[,] DoSimulation(Tile[,] tiles, int tileSetWide, int tileSetHigh, int worldWidth, int worldHeight, int chunkX = 0, int chunkY = 0, int chunkOffSet = 0)
        {
            Tile[,] newTiles = new Tile[worldWidth, worldHeight];
            for (int i = 0; i < newTiles.GetLength(0); i++)
            {
                for (int j = 0; j < newTiles.GetLength(1); j++)
                {
                    if(chunkOffSet != 0)
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

        public static void PlaceChests(List<Tile[,]> tiles,ILocation location, int tileSetWide, int tileSetHigh, int worldWidth, int worldHeight, GraphicsDevice graphics)
        {
            int hiddenTreasureLimit = 4;
            for (int i = 10; i < tiles[0].GetLength(0) - 10; i++)
            {
                for (int j = 10; j < tiles[0].GetLength(1) - 10; j++)
                {
                    if (tiles[0][i, j].GID == 1115)
                    {
                        int nbs = CountAliveNeighbors(tiles[0], 1, i, j);
                        if (nbs >= hiddenTreasureLimit)
                        {
                            
                                tiles[3][i, j - 1].GID = 1753;
                                tiles[1][i, j].GID = 1853;
                            if (!location.AllChests.ContainsKey(tiles[1][i, j].GetTileKey(1)))
                            {
                                location.AllChests.Add(tiles[1][i, j].GetTileKey(1), new Chest(tiles[1][i, j].GetTileKey(1), 3,
                                    new Vector2(tiles[1][i, j].X % worldWidth * 16,
                                tiles[1][i, j].Y % worldHeight * 16), graphics, true));
                            }
                            else
                            {
                                tiles[3][i, j - 1].GID = 0;
                                tiles[1][i, j].GID = 0;
                            }
                                


                        }
                    }
                }
            }
        }


        public static void SpawnBaseCamp(List<Tile[,]> tiles, int tileSetWide, int tileSetHigh, int worldWidth, int worldHeight)
        {
            //top and bottom fences
            for (int i = worldWidth /2; i < worldWidth/2 + 50; i++)
            {
                tiles[3][i, worldWidth / 2].GID = 1251;
                tiles[1][i, worldWidth / 2 + 1].GID = 1350;
                tiles[3][i, worldWidth / 2 + 50].GID = 1251;
                tiles[1][i, worldWidth / 2 + 51].GID = 1350;
            }

            //left and right fences
            for (int i = worldWidth / 2; i < worldWidth / 2 + 50; i++)
            {
                tiles[1][worldWidth / 2, i].GID = 1055;
                tiles[1][worldWidth / 2 + 50, i].GID = 1055;
                //tiles[1][i, 20].GID = 1350;
            }
            //spawn gondola platform
            int iCounter = 0;
            int jCounter = 0;
            for (int i = worldWidth / 2 + 5; i < worldWidth / 2 + 14; i++)
            {
                for (int j = worldWidth / 2 + 10; j < worldWidth / 2 + 17; j++)
                {
                    tiles[1][i, j].GID = 3963 + jCounter + iCounter;
                    jCounter += 100;
                }
                jCounter = 0;
                if(iCounter < 8)
                {
                    iCounter++;
                }
                
            }
        }

        public static void ReplaceTile(int layer, int tileToReplaceX, int tileToReplaceY, int newTileGID, ITileManager tileManager)
        {
            Tile ReplaceMenttile = new Tile(tileManager.AllTiles[layer][tileToReplaceX, tileToReplaceY].X, tileManager.AllTiles[layer][tileToReplaceX, tileToReplaceY].Y, newTileGID);
            tileManager.AllTiles[layer][tileToReplaceX, tileToReplaceY] = ReplaceMenttile;
        }

        public static bool CheckIfChunkExistsInMemory(int idX, int idY)
        {
            if (File.Exists(@"Content/SaveFiles/Chunks/Chunk" + idX +idY + ".dat"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Rectangle GetDestinationRectangle(Tile tile)
        {

            float X = (tile.X * 16);
            float Y = (tile.Y * 16);
            return new Rectangle((int)X, (int)Y, 16, 16);
        }
        public static Rectangle GetSourceRectangle(Tile tile, int tilesetTilesWide)
        {
            int Column = tile.GID % tilesetTilesWide;
            int Row = (int)Math.Floor((double)tile.GID / (double)tilesetTilesWide);

            return new Rectangle(16 * Column, 16 * Row, 16, 16);
        }

        public static void AssignProperties(Tile tileToAssign, GraphicsDevice graphics,TmxMap MapName,int mapWidth, int mapHeight, ITileManager manager, int tileSetNumber, int layer, int oldX, int oldY, ILocation stage)
        {
            if (MapName.Tilesets[tileSetNumber].Tiles.ContainsKey(tileToAssign.GID))
            {
                if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].AnimationFrames.Count > 0 && !MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("idleStart"))
                {
                    List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                    for (int i = 0; i < MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].AnimationFrames.Count; i++)
                    {
                        frames.Add(new EditableAnimationFrame(MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].AnimationFrames[i]));
                    }
                    EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, oldX, oldY, layer, tileToAssign.GID);
                    manager.AnimationFrames.Add(tileToAssign.GetTileKey(layer), frameHolder);
                }
                if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("lightSource"))
                {
                    int lightType = int.Parse(MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["lightSource"]);
                    stage.AllLights.Add(new LightSource(lightType, new Vector2(GetDestinationRectangle(tileToAssign).X, GetDestinationRectangle(tileToAssign).Y)));
                }


                if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("destructable"))
                {
                    manager.TileHitPoints[tileToAssign.GetTileKey(layer)] = Game1.Utility.GetTileHitpoints(MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["destructable"]);

                }

                if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("layer"))
                {
                    tileToAssign.LayerToDrawAt = int.Parse(MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["layer"]);
                    //grass = 1, stone = 2, wood = 3, sand = 4
                }

                if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("action"))
                {
                    if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["action"] == "chestLoot")
                    {
                        if (!stage.AllChests.ContainsKey(tileToAssign.GetTileKey(layer)))
                        {
                            stage.AllChests.Add(tileToAssign.GetTileKey(layer), new Chest(tileToAssign.GetTileKey(layer), 3,
                                    new Vector2(tileToAssign.X % mapWidth * 16,
                               tileToAssign.Y % mapHeight * 16), graphics, true));
                        }

                    }
                }
                if (layer == 3)
                {
                    int randomInt = Game1.Utility.RGenerator.Next(1, 1000);
                    float randomFloat = (float)(randomInt * .0000001);
                    tileToAssign.LayerToDrawAtZOffSet = (GetDestinationRectangle(tileToAssign).Top + GetDestinationRectangle(tileToAssign).Height) * .00001f + randomFloat;
                }

                if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].ObjectGroups.Count > 0)
                {


                    for (int k = 0; k < MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].ObjectGroups[0].Objects.Count; k++)
                    {
                        TmxObject tempObj = MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].ObjectGroups[0].Objects[k];


                        ObjectBody tempObjectBody = new ObjectBody(graphics,
                            new Rectangle(GetDestinationRectangle(tileToAssign).X + (int)Math.Ceiling(tempObj.X),
                            GetDestinationRectangle(tileToAssign).Y + (int)Math.Ceiling(tempObj.Y) - 5, (int)Math.Ceiling(tempObj.Width),
                            (int)Math.Ceiling(tempObj.Height) + 5), tileToAssign.GID);

                        string key = tileToAssign.GetTileKey(layer);

                        stage.AllObjects.Add(key, tempObjectBody); // not gonna work for saving, gotta figure out.

                    }
                }
            }
        }
        public static void ActionHelper(int z, int i, int j, string action, MouseManager mouse, ITileManager tileManager,List<Tile[,]> tiles)
        {
            //new Gid should be one larger, per usual
            string[] information = Game1.Utility.GetActionHelperInfo(action);
            Game1.Player.UserInterface.TileSelectorX = TileUtility.GetDestinationRectangle(tiles[z][i, j]).X;
            Game1.Player.UserInterface.TileSelectorY = TileUtility.GetDestinationRectangle(tiles[z][i, j]).Y;
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
                            TileUtility.ReplaceTile(z, i, j, 86, tileManager);


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
                                if (!Game1.GetCurrentStage().AllCrops.ContainsKey(tiles[1][i, j].GetTileKey(1)))
                                {

                                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                                    Crop tempCrop = Game1.AllCrops.GetCropFromID(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().ID);
                                    tempCrop.TileID = tiles[1][i, j].GetTileKey(1);
                                    tempCrop.GID++;
                                    TileUtility.ReplaceTile(1, i, j, tempCrop.GID, tileManager);
                                    Game1.GetCurrentStage().AllCrops[tiles[1][i, j].GetTileKey(1)] = tempCrop;
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

                            if (Game1.SanctuaryCheckList.TryFillRequirement(tiles[z][i, j].GID))
                            {
                                if (tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[tiles[z][i, j].GID].Properties.ContainsKey("spawnWith"))
                                {
                                    newGID = int.Parse(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[tiles[z][i, j].GID].Properties["spawnWith"]);
                                    relationX = int.Parse(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[newGID].Properties["relationX"]);
                                    relationY = int.Parse(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[newGID].Properties["relationY"]);
                                    layer = int.Parse(tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[newGID].Properties["layer"]);
                                    tileToReplaceGID = tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[newGID].AnimationFrames[0].Id + 1;
                                    TileUtility.ReplaceTile(layer, i + relationX, j + relationY, tileToReplaceGID, tileManager);
                                }
                                Game1.GetCurrentStage().AddTextToAllStrings(Game1.SanctuaryCheckList.AllRequirements.Find(x => x.GID == tiles[z][i, j].GID).Name, new Vector2(TileUtility.GetDestinationRectangle(tiles[z][i, j]).X, TileUtility.GetDestinationRectangle(tiles[z][i, j]).Y - 10),
                                    TileUtility.GetDestinationRectangle(tiles[z][i, j]).X, TileUtility.GetDestinationRectangle(tiles[z][i, j]).Y - 100, 2f, 3f);


                                TileUtility.ReplaceTile(z, i, j, tileManager.MapName.Tilesets[tileManager.TileSetNumber].Tiles[tiles[z][i, j].GID].AnimationFrames[0].Id + 1, tileManager);

                                Game1.Player.Inventory.RemoveItem(int.Parse(information[1]));
                                Game1.GetCurrentStage().ParticleEngine.Color = Color.LightGoldenrodYellow;
                                Game1.GetCurrentStage().ParticleEngine.ActivationTime = 1f;
                                Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(TileUtility.GetDestinationRectangle(tiles[z][i, j]).X + 10, TileUtility.GetDestinationRectangle(tiles[z][i, j]).Y - 10);

                                Game1.SoundManager.SanctuaryAdd.Play();
                            }
                        }
                    }
                    break;
                case "chestLoot":
                    if (mouse.IsClicked)
                    {
                        Game1.GetCurrentStage().AllChests[tiles[z][i, j].GetTileKey(z)].IsUpdating = true;
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
                                TileUtility.ReplaceTile(3, i, j, -1, tileManager);
                                Game1.GetCurrentStage().AllSprites.Add(new Sprite(tileManager.GraphicsDevice, Game1.AllTextures.Gears, new Rectangle(48, 0, 16, 16), new Vector2(TileUtility.GetDestinationRectangle(tiles[z][i, j]).X + 8,
                                    TileUtility.GetDestinationRectangle(tiles[z][i, j]).Y + 8), 16, 16)
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
                                TileUtility.ReplaceTile(3, i, j, -1, tileManager);

                                Game1.GetCurrentStage().AllSprites.Add(new Sprite(tileManager.GraphicsDevice, Game1.AllTextures.Gears, new Rectangle(16, 0, 16, 16),
                                    new Vector2(TileUtility.GetDestinationRectangle(tiles[z][i, j]).X + 8, TileUtility.GetDestinationRectangle(tiles[z][i, j]).Y + 5), 16, 16)
                                { ID = 233, SpinAmount = -10f, SpinSpeed = 2f, Origin = new Vector2(8, 8) });
                                Game1.SoundManager.CraftMetal.Play();
                                Game1.Player.Inventory.RemoveItem(233);

                            }
                        }
                    }
                    break;


            }
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
