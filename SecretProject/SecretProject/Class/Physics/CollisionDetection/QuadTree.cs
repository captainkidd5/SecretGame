//Massive credit to https://gamedevelopment.tutsplus.com/tutorials/quick-tip-use-quadtrees-to-detect-likely-collisions-in-2d-space--gamedev-374 
//Steven Lambert

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace SecretProject.Class.CollisionDetection
{
    public class QuadTree
    {
        public int TotalObjects { get; set; }
        private int MAX_OBJECTS = 8;
        private int MAX_LEVELS = 10;

        private int level;
        private List<ICollidable> Objects;
        private Rectangle bounds;
        private QuadTree[] nodes;


        public QuadTree(int pLevel, Rectangle pBounds)
        {
            this.TotalObjects = 0;
            level = pLevel;
            Objects = new List<ICollidable>();
            bounds = pBounds;
            nodes = new QuadTree[4];
        }

        public void Clear()
        {
            Objects.Clear();

            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null)
                {
                    nodes[i].Clear();
                    nodes[i] = null;
                }
            }
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
            float verticalMidpoint = bounds.X + (bounds.Width / 2);
            float horizontalMidpoint = bounds.Y + (bounds.Height / 2);

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
            if (objectBody.Rectangle.Intersects(Game1.cam.CameraScreenRectangle))
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
        }


        //public List<ICollidable> Retrieve(List<ICollidable> returnObjects, ICollidable objectBody)
        //{
        //    int index = GetIndex(objectBody);
        //    if (index != -1 && nodes[0] != null)
        //    {
        //        nodes[index].Retrieve(returnObjects, objectBody);
        //    }

        //    returnObjects.AddRange(Objects);

        //    return returnObjects;
        //}

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

        //private List<SquareOne> Retrieve(List<SquareOne> fSpriteList, Rect pRect)
        //{
        //    List<int> indexes = GetIndexes(pRect);
        //    for (int ii = 0; ii < indexes.Count; ii++)
        //    {
        //        int index = indexes[ii];
        //        if (index != -1 && nodes[0] != null)
        //        {
        //            nodes[index].Retrieve(fSpriteList, pRect);
        //        }

        //        fSpriteList.AddRange(objects);
        //    }

        //    return fSpriteList;
        //}
    }
}