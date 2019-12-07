using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    public static class WangManager
    {
        public static void ReassignGroupOfTiles(int z, int i, int j, int mainGID, List<int> generatableTiles, Dictionary<int, int> tilingDictionary, IInformationContainer container)
        {
            for (int t = -1; t < 2; t++)
            {
                for (int q = -1; q < 2; q++)
                {
                    //tile isnt touching any borders
                    if (i > 0 && j > 0 && i < TileUtility.ChunkWidth - 1 && j < TileUtility.ChunkHeight - 1)
                    {
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, i + t, j + q, container.MapWidth, container.MapHeight, container);
                    }
                    //tile is touching top
                    else if (i > 0 && j <= 0 && i < TileUtility.ChunkWidth - 1 && j < TileUtility.ChunkHeight - 1)
                    {
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, i + t, j, container.MapWidth, container.MapHeight, container);
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, i + t, 15, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI, container.ArrayJ - 1]);
                    }
                    //tile is touching left
                    else if (i <= 0 && j > 0 && i < TileUtility.ChunkWidth - 1 && j < TileUtility.ChunkHeight - 1)
                    {
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, i, j + q, container.MapWidth, container.MapHeight, container);
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, 15, j + q, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI - 1, container.ArrayJ]);
                    }
                    //tile is touching right
                    else if (i >= TileUtility.ChunkWidth - 1 && j < TileUtility.ChunkHeight - 1 && j > 0)
                    {
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, i, j + q, container.MapWidth, container.MapHeight, container);
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, 0, j + q, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI + 1, container.ArrayJ]);
                    }
                    //tile is touching bottom
                    else if (i < TileUtility.ChunkWidth - 1 && i > 0 && j >= TileUtility.ChunkHeight - 1)
                    {
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, i + t, j, container.MapWidth, container.MapHeight, container);
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, i + t, 0, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI, container.ArrayJ + 1]);
                    }
                    //bottom right corner
                    else if (i == TileUtility.ChunkWidth - 1 && j == TileUtility.ChunkHeight - 1)
                    {

                        //immediate right
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, 0, j, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI + 1, container.ArrayJ]);
                        //right one, down one
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, 0, 0, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI + 1, container.ArrayJ + 1]);
                        //down one
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, i, 0, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI, container.ArrayJ + 1]);
                    }

                    //bottom left corner
                    else if (i == 0 && j == TileUtility.ChunkHeight - 1)
                    {
                        //immediate left
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, 15, j, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI - 1, container.ArrayJ]);
                        //left one down one
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, 15, 0, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI - 1, container.ArrayJ - 1]);
                        //down one
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, 0, 0, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI, container.ArrayJ + 1]);
                    }
                    //top right corner
                    else if (i == TileUtility.ChunkWidth - 1 && j == 0)
                    {

                        //immediate right
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, 0, j, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI + 1, container.ArrayJ]);
                        //right one, up one
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, 0, 0, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI + 1, container.ArrayJ + 1]);
                        //up one
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, i, 15, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI, container.ArrayJ - 1]);
                    }
                    //top left corner
                    else if (i == 0 && j == 0)
                    {

                        //immediate left
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, 15, j, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI - 1, container.ArrayJ]);
                        //left one, up one
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, 15, 15, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI - 1, container.ArrayJ - 1]);
                        //up one
                        PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, i, 15, container.MapWidth, container.MapHeight, container.TileManager.ActiveChunks[container.ArrayI, container.ArrayJ - 1]);
                    }

                }
            }
        }

        public static void PlayerInvokedReassignForTiling(int mainGid, List<int> generatableTiles, Dictionary<int, int> tilingDictionary, int layer,
            int x, int y, int worldWidth, int worldHeight, IInformationContainer container)
        {
            List<int> secondaryTiles;
            if (generatableTiles == Game1.Procedural.DirtGeneratableTiles)
            {
                secondaryTiles = Game1.Procedural.StandardGeneratableDirtTiles;
            }
            else if (generatableTiles == Game1.Procedural.GrassGeneratableTiles)
            {
                secondaryTiles = Game1.Procedural.StandardGeneratableGrassTiles;
            }

            else
            {
                secondaryTiles = new List<int>();
            }


            if (!generatableTiles.Contains(container.AllTiles[layer][x, y].GID) && !secondaryTiles.Contains(container.AllTiles[layer][x, y].GID))
            {
                return;
            }
            int keyToCheck = 0;
            if (y > 0)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x, y - 1].GID) || secondaryTiles.Contains(container.AllTiles[layer][x, y - 1].GID))
                {
                    keyToCheck += 1;
                }
            }
            //if top tile is 0 we look at the chunk above it
            else if (generatableTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI, container.ArrayJ - 1].AllTiles[layer][x, 15].GID) ||
                secondaryTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI, container.ArrayJ - 1].AllTiles[layer][x, 15].GID))
            {
                keyToCheck += 1;
            }

            //now look at chunk below 
            if (y < worldHeight - 1)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x, y + 1].GID) || secondaryTiles.Contains(container.AllTiles[layer][x, y + 1].GID))
                {
                    keyToCheck += 8;
                }
            }

            else if (generatableTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI, container.ArrayJ + 1].AllTiles[layer][x, 0].GID) ||
                secondaryTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI, container.ArrayJ + 1].AllTiles[layer][x, 0].GID))
            {
                keyToCheck += 8;
            }

            //looking at rightmost tile
            if (x < worldWidth - 1)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x + 1, y].GID) || secondaryTiles.Contains(container.AllTiles[layer][x + 1, y].GID))
                {
                    keyToCheck += 4;
                }
            }

            else if (generatableTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI + 1, container.ArrayJ].AllTiles[layer][0, y].GID) ||
                secondaryTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI + 1, container.ArrayJ].AllTiles[layer][0, y].GID))
            {
                keyToCheck += 4;
            }


            //left
            if (x > 0)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x - 1, y].GID) || secondaryTiles.Contains(container.AllTiles[layer][x - 1, y].GID))
                {
                    keyToCheck += 2;
                }
            }
            else if (generatableTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI - 1, container.ArrayJ].AllTiles[layer][15, y].GID) ||
                secondaryTiles.Contains(container.TileManager.ActiveChunks[container.ArrayI - 1, container.ArrayJ].AllTiles[layer][15, y].GID))
            {
                keyToCheck += 2;
            }

            if (keyToCheck >= 15)
            {

                TileUtility.ReplaceTile(layer, x, y, mainGid, container);


            }
            else
            {
                TileUtility.ReplaceTile(layer, x, y, tilingDictionary[keyToCheck] + 1, container);
            }


        }
    }
}
