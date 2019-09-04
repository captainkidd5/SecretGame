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
                            newTiles[i, j].GID = 1106;
                        }
                        else
                        {
                            newTiles[i, j].GID = 1116;
                        }
                    }
                    else
                    {
                        if(nbs > 4)
                        {
                            newTiles[i, j].GID = 1116;

                        }
                        else
                        {
                            newTiles[i, j].GID = 1106;
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
                    else if(tiles[neighborX,neighborY].GID == 1105 )
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
