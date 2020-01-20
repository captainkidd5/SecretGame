using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    public static class ChunkUtility
    {
        public static Tile GetChunkTile(int tileX, int tileY, int layer, Chunk[,] ActiveChunks)
        {
            int chunkX = (int)Math.Floor((float)tileX / 16.0f);

            int chunkY = (int)Math.Floor((float)tileY / 16.0f);

            Chunk chunk = GetChunk(tileX, tileY, ActiveChunks);
            if (chunk == null)
            {
                return null;

            }

            int localX = (int)Math.Floor((float)(tileX - chunkX * 16));
            int localY = (int)Math.Floor((float)(tileY - chunkY * 16));

            if (localX > 15)
            {
                localX = 15;
            }
            else if (localX < 0)
            {
                localX = 0;
            }

            if (localY > 15)
            {
                localY = 15;
            }
            else if (localY < 0)
            {
                localY = 0;
            }
            // = tile;
            return (chunk.AllTiles[layer][localX, localY]);

        }



        /// <summary>
        /// Returns local X or Y coordinate
        /// </summary>
        /// <param name="tileX"></param>
        /// <param name="tileY"></param>
        /// <param name="ActiveChunks"></param>
        /// <param name="returnX">true if you want the local X, false for local Y</param>
        /// <returns></returns>
        public static int GetTileIndexWithinFoundChunk(int tileX, int tileY, Chunk[,] ActiveChunks, bool returnX)
        {
            int chunkX = (int)Math.Floor((float)tileX / 16.0f);

            int chunkY = (int)Math.Floor((float)tileY / 16.0f);

            Chunk chunk = GetChunk(tileX, tileY, ActiveChunks);
            if (chunk == null)
            {
                Console.WriteLine("chunk was null!");

            }

            int localX = (int)Math.Floor((float)(tileX - chunkX * 16));
            int localY = (int)Math.Floor((float)(tileY - chunkY * 16));

            if (localX > 15)
            {
                localX = 15;
            }
            else if (localX < 0)
            {
                localX = 0;
            }

            if (localY > 15)
            {
                localY = 15;
            }
            else if (localY < 0)
            {
                localY = 0;
            }
            if (returnX)
            {
                return localX;
            }
            else
            {
                return localY;
            }
        }
        /// <summary>
        /// Given a world position, returns the index of the x or y coordinate within a found chunk.
        /// </summary>
        /// <param name="globalCoord"></param>
        /// <returns></returns>
        public static int GetLocalChunkCoord(int globalCoord)
        {
            int chunkCoord = (int)Math.Floor((float)globalCoord / 16.0f / 16.0f);
            int localCoord = (int)Math.Ceiling((float)(globalCoord / 16 - chunkCoord * 16));
            if (chunkCoord < 0)
            {
                localCoord--;
            }
            return localCoord;
        }


        public static Chunk GetChunk(int tileX, int tileY, Chunk[,] ActiveChunks)
        {
            int chunkX = (int)Math.Floor((float)tileX / 16.0f);

            int chunkY = (int)Math.Floor((float)tileY / 16.0f);
            for (int i = 0; i < ActiveChunks.GetUpperBound(0); i++)
            {
                for (int j = 0; j < ActiveChunks.GetUpperBound(0); j++)
                {
                    if (ActiveChunks[i, j].X == chunkX && ActiveChunks[i, j].Y == chunkY)
                    {
                        return ActiveChunks[i, j];
                    }


                }
            }
            return null;
        }
    }
}
