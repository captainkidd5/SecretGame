//Massive credit to https://gamedevelopment.tutsplus.com/tutorials/quick-tip-use-quadtrees-to-detect-likely-collisions-in-2d-space--gamedev-374 
//Steven Lambert

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.CollisionDetection
{
    public class QuadTree
    {

        private int MAX_OBJECTS = 20;
        private int MAX_LEVELS =15;

        private int level;
        private List<ICollidable> Objects;
        private Rectangle bounds;
        private QuadTree[] nodes;
        public int Count;

        public QuadTree(int pLevel, Rectangle pBounds)
        {
            level = pLevel;
            Objects = new List<ICollidable>();
            bounds = pBounds;
            nodes = new QuadTree[4];
        }



        private void Split()
        {
            int subWidth = (int)(bounds.Width / 2);
            int subHeight = (int)(bounds.Height / 2);
            int x = (int)bounds.X;
            int y = (int)bounds.Y;

            nodes[0] = new QuadTree(level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
            nodes[1] = new QuadTree(level + 1, new Rectangle(x, y, subWidth, subHeight));
            nodes[2] = new QuadTree(level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
            nodes[3] = new QuadTree(level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        //will return number between 0 and 3 which represent which node of the tree contains the object
        //if didn't fit completely inside of any quadrant return -1
        private int GetIndex(ICollidable collider)
        {
            int index = -1;
            double verticalMidpoint = bounds.X + (bounds.Width / 2);
            double horizontalMidpoint = bounds.Y + (bounds.Height / 2);

            // Object can completely fit within the top quadrants
            bool topQuadrant = (collider.Rectangle.Y + collider.Rectangle.Height < horizontalMidpoint);
            // Object can completely fit within the bottom quadrants
            bool bottomQuadrant = (collider.Rectangle.Y > horizontalMidpoint);

            // Object can completely fit within the left quadrants
            if (collider.Rectangle.X + collider.Rectangle.Width < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    //TOPLEFT
                    index = 1;
                }
                else if (bottomQuadrant)
                {
                    //BOTTOM LEFT
                    index = 2;
                }
            }
            // Object can completely fit within the right quadrants
            else if (collider.Rectangle.X > verticalMidpoint)
            {
                if (topQuadrant)
                {
                    //TOP RIGHT
                    index = 0;
                }
                else if (bottomQuadrant)
                {
                    //BOTTOM RIGHT
                    index = 3;
                }
            }

            
            return index;
        }

        public void Insert(ICollidable objectBody)
        {

            if (nodes[0] != null)
            {
                int index = GetIndex(objectBody);

                if (index != -1)
                {
                    nodes[index].Insert(objectBody);

                    return;
                }
            }

            Objects.Add(objectBody);

            if (Objects.Count > MAX_OBJECTS && level < MAX_LEVELS)
            {
                if (nodes[0] == null)
                {
                    Split();
                }

                int i = 0;
                while (i < Objects.Count)
                {
                    int index = GetIndex(Objects.ElementAt(i));
                    if (index != -1)
                    {
                        nodes[index].Insert(Objects.ElementAt(i));
                        Objects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }
       

        public void Retrieve(List<ICollidable> returnedObjs, ICollidable obj)
        {

            if (nodes[0] != null)
            {
                var index = GetIndex(obj);
                if (index != -1)
                {
                    nodes[index].Retrieve(returnedObjs, obj);
                }
                else
                {
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        nodes[i].Retrieve(returnedObjs, obj);
                    }
                }
            }
            
            returnedObjs.AddRange(Objects);
        }


    }
}
