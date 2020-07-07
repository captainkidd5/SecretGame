using System.Collections.Generic;

namespace SecretProject.Class.TileStuff
{
    public static class WangManager
    {

        

       

        public static void GroupReassignForTiling(int mainGid, List<int> generatableTiles, Dictionary<int, int> tilingDictionary, int layer,
   int x, int y, int worldWidth, int worldHeight, TileManager TileManager)
        {
            for (int t = -1; t < 2; t++)
            {
                for (int q = -1; q < 2; q++)
                {
                    int newX = x + t;
                    int newY = y + q;
                    if (CheckBounds(newX, worldWidth) && CheckBounds(newY, worldWidth))
                    {


                        Tile tile = TileManager.AllTiles[layer][newX, newY];
                        if (tile != null)
                        {
                            ReassignForTiling(mainGid, generatableTiles, tilingDictionary, layer, newX, newY, worldWidth, worldHeight, TileManager);
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
       int x, int y, int worldWidth, int worldHeight, TileManager TileManager)
            {
                if (!generatableTiles.Contains(TileManager.AllTiles[layer][x, y].GID))
                {
                    return;
                }
                int keyToCheck = 0;
                if (y > 0)
                {
                    if (generatableTiles.Contains(TileManager.AllTiles[layer][x, y - 1].GID))
                    {
                        keyToCheck += 1;
                    }
                }


                if (y < worldHeight - 1)
                {
                    if (generatableTiles.Contains(TileManager.AllTiles[layer][x, y + 1].GID))
                    {
                        keyToCheck += 8;
                    }
                }


                //looking at rightmost tile
                if (x < worldWidth - 1)
                {
                    if (generatableTiles.Contains(TileManager.AllTiles[layer][x + 1, y].GID))
                    {
                        keyToCheck += 4;
                    }
                }


                if (x > 0)
                {
                    if (generatableTiles.Contains(TileManager.AllTiles[layer][x - 1, y].GID))
                    {
                        keyToCheck += 2;
                    }
                }


                TileUtility.ReplaceTile(layer, x, y, tilingDictionary[keyToCheck] , TileManager);



            }

            

        }
    }
