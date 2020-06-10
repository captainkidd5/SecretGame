using SecretProject.Class.PathFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.StageFolder.DungeonStuff
{
    public class DungeonGraph
    {
        public int Dimensions { get; private set; }
        public DungeonNode[,] Nodes { get; set; }
        public DungeonGraph(int dimensions)
        {
            if (Math.Log(dimensions, 2) != (int)Math.Log(dimensions, 2))
            {
                throw new Exception("Invalid dungeon size, size in X and Y must be power of 2");
            }
            else
            {
                this.Dimensions = dimensions;
            }
            this.Nodes = new DungeonNode[this.Dimensions,this.Dimensions];
        }

        public DungeonNode GetNode(int x, int y)
        {
            return Nodes[x, y];
        }

        public void GenerateLayout()
        {
            for(int i =0; i < Nodes.GetLength(0); i++)
            {
                for(int j = 0; j < Nodes.GetLength(1); j++)
                {
                    Nodes[i, j] = new DungeonNode(i, j, true);
                }
            }
        }

        public void GeneratePortalConnections()
        {
            //use wang manager for this!
        }

        public void WangRooms(GridStatus gridStatus, 
   int x, int y, int worldWidth, int worldHeight)
        {
            if (gridStatus == GridStatus.Obstructed)
            {
                return;
            }
            int keyToCheck = 0;
            if (y > 0)
            {
                if (Nodes[x, y - 1].GridStatus == GridStatus.Clear)
                {
                    keyToCheck += 1;
                }
            }


            if (y < worldHeight - 1)
            {
                if (Nodes[x, y + 1].GridStatus == GridStatus.Clear)
                {
                    keyToCheck += 8;
                }
            }


            //looking at rightmost tile
            if (x < worldWidth - 1)
            {
                if (Nodes[x + 1, y ].GridStatus == GridStatus.Clear)
                {
                    keyToCheck += 4;
                }
            }


            if (x > 0)
            {
                if (Nodes[x - 1, y].GridStatus == GridStatus.Clear)
                {
                    keyToCheck += 2;
                }
            }


            TileUtility.ReplaceTile(layer, x, y, tilingDictionary[keyToCheck] + 1, container);



        }
    }
}
