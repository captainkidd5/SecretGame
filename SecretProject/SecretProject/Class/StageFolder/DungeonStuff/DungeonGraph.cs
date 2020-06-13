
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
        public Dungeon Dungeon { get; private set; }
        public int Dimensions { get; private set; }
        public DungeonNode[,] Nodes { get; set; }
        public DungeonGraph(Dungeon dungeon, int dimensions)
        {
            //if (Math.Log(dimensions, 2) != (int)Math.Log(dimensions, 2))
            //{
            //    throw new Exception("Invalid dungeon size, size in X and Y must be power of 2");
            //}
            //else
            //{
            //    this.Dimensions = dimensions;
            //}
            this.Dimensions = dimensions;
            this.Nodes = new DungeonNode[this.Dimensions,this.Dimensions];
            this.Dungeon = dungeon;

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
            for (int i = 0; i < Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < Nodes.GetLength(1); j++)
                {
                    
                    int key = WangRooms(Nodes[i, j].GridStatus, i, j, this.Dimensions, this.Dimensions);
                    Dungeon.Rooms[i, j].DungeonPortals = GetPortalsFromWang(Nodes[i, j], key);
                }
            }
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


            switch(wangKey)
            {
                case 0: //isolated, no portals
                    break;
                case 1: //one above
                    AddPortal(portals, currentNode, Dir.Up);
                    break;
                case 2://one left
                    AddPortal(portals, currentNode, Dir.Left);
                    break;
                case 3: //one left, one above

                    AddPortal(portals, currentNode, Dir.Up);
                    AddPortal(portals, currentNode, Dir.Left);
                    break;
                case 4: //one right

                    AddPortal(portals, currentNode, Dir.Right);
                    break;
                case 5: //one up, one right


                    AddPortal(portals, currentNode, Dir.Up);
                    AddPortal(portals, currentNode, Dir.Right);

                    break;
                case 6: //one left, one right
                    AddPortal(portals, currentNode, Dir.Left);
                    AddPortal(portals, currentNode, Dir.Right);

                    break;
                case 7: //one up, one left, one right
    

                    AddPortal(portals, currentNode, Dir.Up);
                    AddPortal(portals, currentNode, Dir.Left);
                    AddPortal(portals, currentNode, Dir.Right);

                    break;
                case 8://one down

                    AddPortal(portals, currentNode, Dir.Down);
                    break;
                case 9: //one down, one up

                    AddPortal(portals, currentNode, Dir.Down);
                    AddPortal(portals, currentNode, Dir.Up);

                    break;
                case 10: //one left, one down

                    AddPortal(portals, currentNode, Dir.Left);
                    AddPortal(portals, currentNode, Dir.Down);

                    break;
                case 11: //one down, one left, one up

                    AddPortal(portals, currentNode, Dir.Down);
                    AddPortal(portals, currentNode, Dir.Left);
                    AddPortal(portals, currentNode, Dir.Up);


                    break;
                case 12://one down, one right

                    AddPortal(portals, currentNode, Dir.Down);
                    AddPortal(portals, currentNode, Dir.Right);

                    break;
                case 13://one down, one right, one up

                    AddPortal(portals, currentNode, Dir.Down);
                    AddPortal(portals, currentNode, Dir.Right);
                    AddPortal(portals, currentNode, Dir.Up);

                    break;
                case 14: //one left, one down, one right
                    AddPortal(portals, currentNode, Dir.Left);
                    AddPortal(portals, currentNode, Dir.Down);
                    AddPortal(portals, currentNode, Dir.Right);


                    break;
                case 15: // all

                    AddPortal(portals, currentNode, Dir.Down);
                    AddPortal(portals, currentNode, Dir.Up);
                    AddPortal(portals, currentNode, Dir.Left);
                    AddPortal(portals, currentNode, Dir.Right);
                    break;

                default:
                    throw new Exception("Portal key was not between 0 and 15!");
            }
            return portals;
        }

        private void AddPortal(List<DungeonPortal> portals,DungeonNode currentNode, Dir direction)
        {
            int plusX = 0;
            int plusY = 0;

            switch(direction)
            {
                case Dir.Down:
                    plusY = 1;
                    break;
                case Dir.Up:
                    plusY = -1;
                    break;
                case Dir.Left:
                    plusX = -1;
                    break;
                case Dir.Right:
                    plusX = 1;
                    break;
            }

            DungeonPortal newPortal = new DungeonPortal(this.Nodes[currentNode.X + plusX, currentNode.Y + plusY], GetRectangleFromDirection(direction), GetSafteyXFromDirection(direction), GetSafteyYFromDirection(direction), direction);
            portals.Add(newPortal);
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
                    return 64;

                case Dir.Right:
                    return -64;

                default:
                    throw new Exception("direction " + directionOfNewRoom.ToString() + " is invalid!");
            }
        }
        public static int GetSafteyYFromDirection(Dir directionOfNewRoom)
        {
            switch (directionOfNewRoom)
            {
                case Dir.Down:
                    return -64;
                case Dir.Up:
                    return 64;

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

                case Dir.Right:
                    return new Rectangle(1020, 496, 16, 32);

                case Dir.Left:
                    return new Rectangle(0, 496, 16, 32);

                default:
                    throw new Exception("direction " + directionOfNewRoom.ToString() + " is invalid!");
            }
        }
    }
}
