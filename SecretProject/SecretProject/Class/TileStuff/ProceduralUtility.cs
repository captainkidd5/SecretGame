using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    public static class ProceduralUtility
    {
        public static void DoSimulation(List<Tile[,]> tiles)
        {
            List<Tile[,]> newTiles = new List<Tile[,]>();

        }

        public static void CountAliveNeighbors(List<Tile[,]> tiles,int layer, int x, int y)
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
                    else if (neighborX < 0 || neighborY < 0 || neighborX >= tiles[layer].GetLength(0) || neighborY >= tiles[layer].GetLength(1))
                    {
                        count++;

                    }
                    else if(tiles[layer][neighborX][neighborY])
                }
            }
        }
    }
}
