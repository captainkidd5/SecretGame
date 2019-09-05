using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    public static class ProceduralUtility
    {
        public static Tile[,] DoSimulation(Tile[,] tiles, int tileSetWide, int tileSetHigh, int worldWidth, int worldHeight)
        {
            Tile[,] newTiles = new Tile[worldWidth, worldHeight];
            for(int i =0; i < newTiles.GetLength(0); i++)
            {
                for(int j =0; j < newTiles.GetLength(1); j++)
                {
                    newTiles[i, j] = new Tile(i, j, 1106, tileSetWide, tileSetHigh, worldWidth, worldHeight);
                }
            }

            for(int i =0; i < newTiles.GetLength(0); i++)
            {
                for (int j = 0; j < newTiles.GetLength(1); j++)
                {
                    int nbs = CountAliveNeighbors(tiles, 0, i, j);
                    if(tiles[i,j].GID != 1115)
                    {
                        if(nbs < 3)
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
                        if(nbs > 4)
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

        public static int CountAliveNeighbors(Tile[,] tiles,int layer, int x, int y)
        {
            int count = 0;
            for(int i =-1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++)
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
                    else if((Game1.Utility.DirtGeneratableTiles.Contains(tiles[neighborX, neighborY].GID)))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public static void PlaceChests(List<Tile[,]> tiles, int tileSetWide, int tileSetHigh, int worldWidth, int worldHeight)
        {
            int hiddenTreasureLimit = 5;
            for (int i = 0; i < tiles[0].GetLength(0); i++)
            {
                for (int j = 0; j < tiles[0].GetLength(1); j++)
                {
                    if (tiles[0][i, j].GID == 1115)
                    {
                        int nbs = CountAliveNeighbors(tiles[0], 1, i, j);
                        if (nbs >= hiddenTreasureLimit)
                        {
                            tiles[1][i, j].GID = 1852;
                        }
                    }
                }
            }
        }
    }
}
