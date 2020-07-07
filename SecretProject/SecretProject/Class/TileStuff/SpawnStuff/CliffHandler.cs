using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.SpawnStuff
{
    public class CliffHandler
    {
        public List<int> TopEdges { get; private set; }
        public int CenterGID { get; private set; }
        public int BottomGID { get; private set; }

        public int CliffSize { get; private set; }

        public CliffHandler(List<int> topEdges, int centerGID, int bottomGID)
        {
            this.TopEdges = topEdges;
            this.CenterGID = centerGID + 1;
            this.BottomGID = bottomGID + 1;
            this.CliffSize = ((this.BottomGID - this.CenterGID) / 100) - 1;
        }

        public void ExtendCliffs(TileManager TileManager)
        {
            for (int i = 0; i < TileUtility.ChunkWidth; i++)
            {
                for (int j = 0; j < TileUtility.ChunkHeight; j++)
                {
                    if (TopEdges.Contains(TileManager.AllTiles[3][i, j].GID))
                    {

                        int counter = 1;

                        for (int c = j; c < j + CliffSize; c++)
                        {
                            if (j + counter < 16)
                            {


                                TileManager.AllTiles[2][i, j + counter].GID = TileManager.AllTiles[3][i, j].GID + 1 + 100 * counter;

                                TileManager.AllTiles[3][i, j + counter].GID = 0;
                                if (c < j + CliffSize - 1)
                                {
                                    TileManager.AllTiles[0][i, j + counter].GID = 0;
                                }

                                counter++;
                            }
                        }

                    }


                }
            }

        }

        public void HandleCliffEdgeCases(TileManager TileManager, List<int[,,]> allAdjacentChunkNoise)
        {

            int gidToTest = this.CenterGID;
            int gidBottomToTest = this.BottomGID;

            int jDifference = this.BottomGID - this.CenterGID;
            //if (TileManager.TileManager.Stage == Game1.OverWorld)
            //{
            //    gidToTest = 4124;
            //    gidBottomToTest = 4724;
            //}
            //else
            //{
            //    gidToTest = 4829;
            //    gidBottomToTest = 5429;
            //}
            //these gids are all +1
            for (int i = 0; i < 16; i++)
            {
                for (int j = 15; j > 15 - this.CliffSize; j--)
                {
                    if (allAdjacentChunkNoise[0][3, i, j] == gidToTest)
                    {
                        int newJIndex = j;

                        int newCliffGID = gidToTest + 100;
                        for (int remainAbove = newJIndex; remainAbove < 15; remainAbove++)
                        {
                            newCliffGID += 100;
                        }

                        for (int newY = 0; newCliffGID != gidBottomToTest; newY++)
                        {

                            newCliffGID += 100;
                            TileManager.AllTiles[2][i, newY].GID = newCliffGID;
                            if (newCliffGID != gidBottomToTest)
                            {
                                TileManager.AllTiles[0][i, newY].GID = 0;

                                TileManager.AllTiles[1][i, newY].GID = 0;

                            }


                        }
                        break;


                    }
                }
            }
        }

        //    CliffBottomTiles = new List<int>()
        //            {
        //                4222, 4223, 4224,4021,4025,4026,4027,4028
        //            };

        public static List<int> GetTopCliffEdges(TilingTileManager tilingTileManager)
        {
            int centralID = tilingTileManager.TilingDictionary[15];
            List<int> edgeList = new List<int>()
            {
                centralID - 102, centralID + 99, centralID + 100, centralID + 101, centralID - 98,
                centralID - 97, centralID - 96, centralID - 95,
            };
            return edgeList;
        }
    }
}
