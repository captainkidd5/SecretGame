using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    public static class ChunkUtility
    {

        public const int ChunkWidth = 16;
        public const int ChunkHeight = 16;
        public static Tile GetChunkTile(int tileX, int tileY, int layer, Chunk[,] ActiveChunks)
        {
            int chunkX = GetChunkX(tileX);

            int chunkY = GetChunkY(tileY);

            Chunk chunk = GetChunk(chunkX, chunkY, ActiveChunks);
            if (chunk == null)
            {
                return null;

            }

            int localX = CheckArrayLimits((int)Math.Floor((float)(tileX - chunkX * ChunkWidth)));
            int localY = CheckArrayLimits((int)Math.Floor((float)(tileY - chunkY * ChunkHeight)));


            return (chunk.AllTiles[layer][localX, localY]);

        }


        public static int GetChunkX(int tileX)
        {
           return (int)Math.Floor((float)tileX / ChunkWidth);
        }

        public static int GetChunkY(int tileY)
        {
            return (int)Math.Floor((float)tileY / ChunkHeight);
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
            int chunkX = GetChunkX(tileX);

            int chunkY = GetChunkY(tileY);

            Chunk chunk = GetChunk(chunkX, chunkY, ActiveChunks);
            if (chunk == null)
            {
                Console.WriteLine("chunk was null!");

            }

            int localX = CheckArrayLimits((int)Math.Floor((float)(tileX - chunkX * ChunkWidth)));
            int localY = CheckArrayLimits((int)Math.Floor((float)(tileY - chunkY * ChunkHeight)));


            if (returnX)
            {
                return localX;
            }
            else
            {
                return localY;
            }
        }

        public static int CheckArrayLimits(int index)
        {

            if (index < 0)
            {
                index = 0;
            }
            else if (index > 15)
            {
                index = 15;
            }
            return index;
        }
        /// <summary>
        /// Given a world position, returns the index of the x or y coordinate within a found chunk.
        /// </summary>
        /// <param name="globalCoord"></param>
        /// <returns></returns>
        public static int GetLocalChunkCoord(int globalCoord)
        {
            int chunkCoord = (int)Math.Floor((float)globalCoord / ChunkWidth / ChunkWidth);
            int localCoord = (int)Math.Floor((float)(globalCoord / ChunkHeight - chunkCoord * ChunkHeight));

            if (globalCoord < 0)
            {
                localCoord--;
            }
            if (localCoord > 15)
            {
                localCoord = 15;
            }
            else if(localCoord < 0)
            {
                localCoord = 0;
            }
            
            return localCoord;
        }


        public static Chunk GetChunk(int chunkX, int chunkY, Chunk[,] ActiveChunks)
        {
            for (int i = 0; i < ActiveChunks.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < ActiveChunks.GetUpperBound(1) + 1; j++)
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
