using SecretProject.Class.PathFinding;
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

        public GridStatus GridStatus{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="clear">Whether or not the room will be passable or impassbile</param>
        public DungeonNode(int x, int y, GridStatus gridStatus)
        {
            this.X = x;
            this.Y = y;
            this.GridStatus = gridStatus;
        }
    }
}
