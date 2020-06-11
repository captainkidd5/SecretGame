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
        public Rectangle InteractionRectangle { get; private set; }

        public DungeonNode DestinationRoom { get; set; }

        public int SafteyX { get; private set; }
        public int SafteyY { get; private set; }

        public Dir DirectionToSpawn { get; set; }


        public DungeonPortal(DungeonNode destinationNode, Rectangle destinationRectangle, int safteyX, int safteyY, Dir directionToSpawn) 
        {
            
            this.DestinationRoom = destinationNode;

            this.InteractionRectangle = destinationRectangle;
            this.SafteyX = safteyX;
            this.SafteyY = safteyY;
            switch(directionToSpawn)
            {
                case Dir.Down:
                    this.DirectionToSpawn = Dir.Up;
                    break;
                case Dir.Up:
                    this.DirectionToSpawn = Dir.Down;
                    break;
                case Dir.Left:
                    this.DirectionToSpawn = Dir.Right;
                    break;
                case Dir.Right:
                    this.DirectionToSpawn = Dir.Left;
                    break;
                    
            }

        }

      
    }
}
