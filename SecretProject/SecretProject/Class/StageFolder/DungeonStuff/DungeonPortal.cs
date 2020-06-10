using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.StageFolder.DungeonStuff
{
    public class DungeonPortal
    {
        public Vector2 Position { get; private set; }
        public Rectangle InteractionRectangle { get; private set; }

        public DungeonNode NodeFrom { get; private set; }
        public DungeonNode NodeTo { get; private set; }

        public int SafteyX { get; private set; }
        public int SafteyY { get; private set; }

    

        public DungeonPortal(Vector2 position, DungeonNode from, DungeonNode to, int safteyX, int safteyY) 
        {
            this.Position = position;
            this.InteractionRectangle = new Rectangle((int)position.X, (int)position.Y, 16, 16);
            this.NodeFrom = from;
            this.NodeTo = to;
            this.SafteyX = safteyX;
            this.SafteyY = safteyY;
        }


    }
}
