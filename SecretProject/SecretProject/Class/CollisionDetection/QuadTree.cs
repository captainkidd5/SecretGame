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

        private int MAX_OBJECTS = 10;
        private int MAX_LEVELS = 5;

        private int level;
        private List<Collider> Objects;
        private Rectangle bounds;
        private QuadTree[] nodes;

        /*
         * Constructor
         */
        public QuadTree(int pLevel, Rectangle pBounds)
        {
            level = pLevel;
            Objects = new List<Collider>();
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
        private int GetIndex(Collider objectBody)
        {
            int index = -1;
            double verticalMidpoint = bounds.X + (bounds.Width / 2);
            double horizontalMidpoint = bounds.Y + (bounds.Height / 2);

            // Object can completely fit within the top quadrants
            bool topQuadrant = (objectBody.Rectangle.Y < horizontalMidpoint && objectBody.Rectangle.Y + objectBody.Rectangle.Height < horizontalMidpoint);
            // Object can completely fit within the bottom quadrants
            bool bottomQuadrant = (objectBody.Rectangle.Y > horizontalMidpoint);

            // Object can completely fit within the left quadrants
            if (objectBody.Rectangle.X < verticalMidpoint && objectBody.Rectangle.X + objectBody.Rectangle.Width < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 1;
                }
                else if (bottomQuadrant)
                {
                    index = 2;
                }
            }
            // Object can completely fit within the right quadrants
            else if (objectBody.Rectangle.X > verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 0;
                }
                else if (bottomQuadrant)
                {
                    index = 3;
                }
            }

            return index;
        }

        public void Insert(Collider objectBody)
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

        public List<Collider> Retrieve(List<Collider> returnObjects, Collider objectBody)
        {
            int index = GetIndex(objectBody);
            if (index != -1 && nodes[0] != null)
            {
                nodes[index].Retrieve(returnObjects, objectBody);
            }

            returnObjects.AddRange(Objects);

            return returnObjects;
        }
    }
}
