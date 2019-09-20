using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.StageFolder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    public static class TileUtility
    {
        public static int ChunkX = 32;
        public static int ChunkY = 32;
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
        public static Tile[,] DoSimulation(Tile[,] tiles, int tileSetWide, int tileSetHigh, int worldWidth, int worldHeight)
        {
            Tile[,] newTiles = new Tile[worldWidth, worldHeight];
            for (int i = 0; i < newTiles.GetLength(0); i++)
            {
                for (int j = 0; j < newTiles.GetLength(1); j++)
                {
                    newTiles[i, j] = new Tile(i, j, 1106);
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
                            if (!location.AllChests.ContainsKey(tiles[1][i, j].GetTileKey(1, worldWidth, worldHeight)))
                            {
                                location.AllChests.Add(tiles[1][i, j].GetTileKey(1, worldWidth, worldHeight), new Chest(tiles[1][i, j].GetTileKey(1,worldWidth, worldHeight), 3,
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

    }
}
