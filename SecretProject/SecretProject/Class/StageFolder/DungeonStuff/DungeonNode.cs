using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.StageFolder.DungeonStuff
{
    public class DungeonNode
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public DungeonNode(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
