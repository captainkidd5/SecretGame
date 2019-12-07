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
            
           PlayerInvokedReassignForTiling(mainGID, generatableTiles, tilingDictionary, z, i , j , container.MapWidth, container.MapHeight, container);
                   


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
