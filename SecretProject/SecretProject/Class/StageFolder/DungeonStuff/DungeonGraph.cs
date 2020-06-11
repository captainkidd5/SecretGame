using Microsoft.Xna.Framework;
using SecretProject.Class.PathFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
                    Nodes[i, j] = new DungeonNode(i, j, GridStatus.Clear);
                }
            }
        }

        public void GeneratePortalConnections()
        {
            //use wang manager for this!
        }

        public int WangRooms(GridStatus gridStatus, 
   int x, int y, int worldWidth, int worldHeight)
        {
            int keyToCheck = 0;
            if (gridStatus == GridStatus.Obstructed)
            {
                return 0;
            }
            
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

            return keyToCheck;




        }

        public List<DungeonPortal> GetPortalsFromWang(DungeonNode currentNode, int wangKey)
        {
            List<DungeonPortal> portals = new List<DungeonPortal>();

            DungeonPortal downPortal ;
            DungeonPortal upPortal;
            DungeonPortal leftPortal;
            DungeonPortal rightPortal;

            switch(wangKey)
            {
                case 0: //isolated, no portals
                    break;
                case 1: //one above
                    upPortal = new DungeonPortal(this.Nodes[currentNode.X, currentNode.Y - 1], GetRectangleFromDirection(Dir.Up), GetSafteyXFromDirection(Dir.Up), GetSafteyYFromDirection(Dir.Up));
                    portals.Add(upPortal);
                    break;
                case 2://one left
                    leftPortal = new DungeonPortal(this.Nodes[currentNode.X - 1, currentNode.Y], GetRectangleFromDirection(Dir.Left), GetSafteyXFromDirection(Dir.Left), GetSafteyYFromDirection(Dir.Left));
                    portals.Add(leftPortal);
                    break;
                case 3: //one left, one above
                    upPortal = new DungeonPortal(this.Nodes[currentNode.X, currentNode.Y - 1], GetRectangleFromDirection(Dir.Up), GetSafteyXFromDirection(Dir.Up), GetSafteyYFromDirection(Dir.Up));
                    leftPortal = new DungeonPortal(this.Nodes[currentNode.X - 1, currentNode.Y], GetRectangleFromDirection(Dir.Left), GetSafteyXFromDirection(Dir.Left), GetSafteyYFromDirection(Dir.Left));
                    portals.Add(upPortal);
                    portals.Add(leftPortal);
                    break;
                case 4: //one right
                    rightPortal = new DungeonPortal(this.Nodes[currentNode.X + 1, currentNode.Y], GetRectangleFromDirection(Dir.Right), GetSafteyXFromDirection(Dir.Right), GetSafteyYFromDirection(Dir.Right));
                    portals.Add(rightPortal);
                    break;
                case 5: //one up, one right
                    break;
                case 6: //one left, one right
                    break;
                case 7: //one up, one left, one right
                    break;
                case 8://one down
                    break;
                case 9: //one down, one up
                    break;
                case 10: //one left, one down
                    break;
                case 11: //one down, one left, one up
                    break;
                case 12://one down, one right
                    break;
                case 13://one down, one right, one up
                    break;
                case 14: //one left, one down, one right
                    break;
                case 15: // all
                    break;
            }

        }

        public static int GetSafteyXFromDirection(Dir directionOfNewRoom)
        {
            switch (directionOfNewRoom)
            {
                case Dir.Down:
                    return 0;
                case Dir.Up:
                    return 0;

                case Dir.Left:
                    return 16;

                case Dir.Right:
                    return -16;

                default:
                    throw new Exception("direction " + directionOfNewRoom.ToString() + " is invalid!");
            }
        }
        public static int GetSafteyYFromDirection(Dir directionOfNewRoom)
        {
            switch (directionOfNewRoom)
            {
                case Dir.Down:
                    return -32;
                case Dir.Up:
                    return 32;

                case Dir.Left:
                    return 0;

                case Dir.Right:
                    return 0;

                default:
                    throw new Exception("direction " + directionOfNewRoom.ToString() + " is invalid!");
            }
        }
        /// <summary>
        /// Gets rectangle in one of four directions, depending on where the other room is located compared to the current one.
        /// </summary>
        /// <param name="currentRoom"></param>
        /// <param name="destinationRoom"></param>
        /// <returns></returns>
        public static Rectangle GetRectangleFromDirection(Dir directionOfNewRoom)
        {
          
            switch (directionOfNewRoom)
            {
                case Dir.Down:
                    return new Rectangle(496, 1008, 32, 16);
                case Dir.Up:
                    return new Rectangle(496, 0, 32, 16);

                case Dir.Left:
                    return new Rectangle(0, 496, 16, 32);

                case Dir.Right:
                    return new Rectangle(1008, 496, 16, 32);

                default:
                    throw new Exception("direction " + directionOfNewRoom.ToString() + " is invalid!");
            }
        }
    }
}
