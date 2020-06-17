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
        public Rectangle InteractionRectangle { get;  set; }

        public DungeonNode DestinationRoom { get; set; }

        public int SafteyX { get; private set; }
        public int SafteyY { get; private set; }

        public Dir DirectionToSpawn { get; set; }


        public DungeonPortal(DungeonNode destinationNode,  int safteyX, int safteyY, Dir directionToSpawn) 
        {
            
            this.DestinationRoom = destinationNode;

            this.SafteyX = safteyX;
            this.SafteyY = safteyY;
            this.DirectionToSpawn = directionToSpawn;
        }

      
    }
}
