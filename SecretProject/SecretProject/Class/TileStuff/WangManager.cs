using System.Collections.Generic;

namespace SecretProject.Class.TileStuff
{
    public static class WangManager
    {

        public static void ChunkGroupReassignForTiling(int mouseWorldX, int mouseWorldY, int mainGid, List<int> generatableTiles, Dictionary<int, int> tilingDictionary, int layer, ITileManager tileManager)
        {
            for (int t = -1; t < 2; t++)
            {
                for (int q = -1; q < 2; q++)
                {
                    Chunk chunk = ChunkUtility.GetChunk(ChunkUtility.GetChunkX(TileUtility.GetSquareTileCoord(mouseWorldX + t * 16)), ChunkUtility.GetChunkY(TileUtility.GetSquareTileCoord(mouseWorldY + q * 16)), tileManager.ActiveChunks);
                    if (chunk != null)
                    {
                        int indexX = ChunkUtility.GetLocalChunkCoord(mouseWorldX + t * 16);
                        int indexY = ChunkUtility.GetLocalChunkCoord(mouseWorldY + q * 16);
                        Tile tile = chunk.AllTiles[layer][indexX, indexY];
                        if (tile != null)
                        {
                            ChunkReassignForTiling(mainGid, generatableTiles,
                                            tilingDictionary, layer,
                                           indexX, indexY,
                                            TileUtility.ChunkWidth, TileUtility.ChunkHeight, chunk);
                        }
                    }


                }
            }
        }

        public static void ChunkReassignForTiling(int mainGid, List<int> generatableTiles, Dictionary<int, int> tilingDictionary, int layer,
    int x, int y, int worldWidth, int worldHeight, IInformationContainer container)
        {
            bool chunkWithinBounds = IsContainerWithinArrayBounds(container);
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


            else if (chunkWithinBounds && generatableTiles.Contains(container.ITileManager.ActiveChunks[container.ArrayI, container.ArrayJ - 1].AllTiles[layer][x, 15].GID))
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
            else if (chunkWithinBounds && generatableTiles.Contains(container.ITileManager.ActiveChunks[container.ArrayI, container.ArrayJ + 1].AllTiles[layer][x, 0].GID))
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
            else if (chunkWithinBounds && generatableTiles.Contains(container.ITileManager.ActiveChunks[container.ArrayI + 1, container.ArrayJ].AllTiles[layer][0, y].GID))
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
            else if (chunkWithinBounds && generatableTiles.Contains(container.ITileManager.ActiveChunks[container.ArrayI - 1, container.ArrayJ].AllTiles[layer][15, y].GID))
            {
                keyToCheck += 2;
            }


            TileUtility.ReplaceTile(layer, x, y, tilingDictionary[keyToCheck], container);



        }

        public static void GroupReassignForTiling(int mainGid, List<int> generatableTiles, Dictionary<int, int> tilingDictionary, int layer,
   int x, int y, int worldWidth, int worldHeight, IInformationContainer container)
        {
            for (int t = -1; t < 2; t++)
            {
                for (int q = -1; q < 2; q++)
                {
                    int newX = x + t;
                    int newY = y + q;
                    if (CheckBounds(newX, worldWidth) && CheckBounds(newY, worldWidth))
                    {


                        Tile tile = container.AllTiles[layer][newX, newY];
                        if (tile != null)
                        {
                            ReassignForTiling(mainGid, generatableTiles, tilingDictionary, layer, newX, newY, worldWidth, worldHeight, container);
                        }

                    }

                }
            }
        }

        public static bool CheckBounds(int index, int maxBounds)
        {
            if (index >= 0 && index < maxBounds)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
            public static void ReassignForTiling(int mainGid, List<int> generatableTiles, Dictionary<int, int> tilingDictionary, int layer,
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


                if (y < worldHeight - 1)
                {
                    if (generatableTiles.Contains(container.AllTiles[layer][x, y + 1].GID))
                    {
                        keyToCheck += 8;
                    }
                }


                //looking at rightmost tile
                if (x < worldWidth - 1)
                {
                    if (generatableTiles.Contains(container.AllTiles[layer][x + 1, y].GID))
                    {
                        keyToCheck += 4;
                    }
                }


                if (x > 0)
                {
                    if (generatableTiles.Contains(container.AllTiles[layer][x - 1, y].GID))
                    {
                        keyToCheck += 2;
                    }
                }


                TileUtility.ReplaceTile(layer, x, y, tilingDictionary[keyToCheck] , container);



            }

            public static bool IsContainerWithinArrayBounds(IInformationContainer container)
            {
                if (container.ArrayI > 0 && container.ArrayJ > 0 && container.ArrayI < 8 && container.ArrayJ < 8)
                {
                    return true;
                }
                return false;
            }

        }
    }
