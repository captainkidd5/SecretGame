using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    public static class WangManager
    {

        public static void GroupReassignForTiling(int mouseWorldX, int mouseWorldY,int mainGid, List<int> generatableTiles, Dictionary<int, int> tilingDictionary, int layer,  ITileManager tileManager)
        {
            for (int t = -1; t < 2; t++)
            {
                for (int q = -1; q < 2; q++)
                {

                    ReassignTileForTiling(mainGid, generatableTiles,
                                        tilingDictionary, layer,
                                        TileUtility.GetLocalChunkCoord(mouseWorldX + t * 16), TileUtility.GetLocalChunkCoord(mouseWorldY + q * 16),
                                        TileUtility.ChunkWidth, TileUtility.ChunkHeight, TileUtility.GetChunk(TileUtility.GetSquareTileCoord(mouseWorldX + t * 16), TileUtility.GetSquareTileCoord(mouseWorldY + q * 16), tileManager.ActiveChunks));
                }
            }
        }

        public static void ReassignTileForTiling(int mainGid, List<int> generatableTiles, Dictionary<int, int> tilingDictionary, int layer,
    int x, int y, int worldWidth, int worldHeight, IInformationContainer container)
        {

            if (!generatableTiles.Contains(container.AllTiles[layer][x, y].GID))
            {
                return;
            }
            int keyToCheck = 0;
            if (y > 0)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x, y - 1].GID))
                {
                    keyToCheck += 1;
                }
            }
            //if top tile is 0 we look at the chunk above it


            else if (generatableTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI, container.ArrayJ - 1].AllTiles[layer][x, 15].GID))
            {
                keyToCheck += 1;
            }


            if (y < worldHeight - 1)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x, y + 1].GID))
                {
                    keyToCheck += 8;
                }
            }
            else if (generatableTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI, container.ArrayJ + 1].AllTiles[layer][x, 0].GID))
            {
                keyToCheck += 8;
            }


            //looking at rightmost tile
            if (x < worldWidth - 1)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x + 1, y].GID))
                {
                    keyToCheck += 4;
                }
            }
            else if (generatableTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI + 1, container.ArrayJ].AllTiles[layer][0, y].GID))
            {
                keyToCheck += 4;
            }




            if (x > 0)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x - 1, y].GID))
                {
                    keyToCheck += 2;
                }
            }
            else if (generatableTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI - 1, container.ArrayJ].AllTiles[layer][15, y].GID))
            {
                keyToCheck += 2;
            }


            TileUtility.ReplaceTile(layer, x, y, tilingDictionary[keyToCheck] + 1, container);



        }


    }
}
