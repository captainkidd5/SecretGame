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

        public void GenerateLayout()
        {

        }
    }
}
