using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.CollisionDetection
{
   public class CustomQuadTree
    {
        public List<Node> Nodes { get; set; }
        public static int SplitLimit = 4;

        public Rectangle Rectangle { get; set; }

        public CustomQuadTree(Rectangle rectangle)
        {
            this.Rectangle = rectangle;
            this.Nodes = new List<Node>(SplitLimit)
            {
                new Node(0, this.Rectangle, new List<ICollidable>()),
                new Node(1, this.Rectangle, new List<ICollidable>()),
                new Node(2, this.Rectangle, new List<ICollidable>()),
                new Node(3, this.Rectangle, new List<ICollidable>()),
            };
        }

        public void Insert()
        {

        }
    }

    public class Node
    {
        // 0 thru 3, clockwise
        public int Quadrant { get; private set; }
        public Rectangle Rectangle { get; private set; }

        public List<ICollidable> Objects { get; private set; }

        public List<Node> SubNodes { get; set; }
        public Node(int quadrant, Rectangle rectangle, List<ICollidable> objects)
        {
            this.Quadrant = quadrant;
            switch (this.Quadrant)
            {
                //top left
                case 0:
                    this.Rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width / 2, rectangle.Height / 2);
                    break;
                    //top right
                case 1:
                    this.Rectangle = new Rectangle(rectangle.X + rectangle.Width/2, rectangle.Y, rectangle.Width / 2, rectangle.Height / 2);
                    break;
                    //bottom left
                case 2:
                    this.Rectangle = new Rectangle(rectangle.X, rectangle.Y + rectangle.Height/2, rectangle.Width / 2, rectangle.Height / 2);
                    break;
                    //bottom right
                case 3:
                    this.Rectangle = new Rectangle(rectangle.X + rectangle.Width/2, rectangle.Y + rectangle.Height/2, rectangle.Width / 2, rectangle.Height / 2);
                    break;
            }
            this.Objects = objects;
            
        }

        /// <summary>
        /// //Initializes four subnodes and, 
        //if the objects in the current subnodes fit completely into any of them, add it to the subnode and remove from this nodes object list
        /// </summary>
        public void Split()
        {
            this.SubNodes = new List<Node>(CustomQuadTree.SplitLimit);

            List<List<ICollidable>> collideLists = new List<List<ICollidable>>();
            for(int col = 0; col < this.Objects.Count; col++)
            {
                int colliderIndex = GetIndex(this.Objects[col]);
                if(colliderIndex == -1) //didn't fit into any subnodes completely
                {
                    continue;
                }
                else
                {
                    collideLists[colliderIndex].Add(this.Objects[col]); //which list the object is inserted into is determined with the getindex() method
                    this.Objects.Remove(this.Objects[col]); //if we insert it into a subnode, we want to remove it from the parent node
                }
                
            }

            for (int i = 0; i < this.SubNodes.Count; i++)
            {
                this.SubNodes.Add(new Node(i, this.Rectangle,collideLists[i]));
            }
        }
        /// <summary>
        /// Simply returns whether or not the rectangle fits into the specified quadrant. 
        /// </summary>
        /// <param name="quadrant">Top left, top right, bottom left, bottom right</param>
        /// <param name="rectangle">rectangle to test</param>
        /// <returns></returns>
        public bool DoesFitIntoQuadrant(int quadrant, Rectangle rectangle)
        {
            switch(quadrant)
            {
                //top left
                case 0:
                    if(rectangle.X < this.Rectangle.Width /2 && rectangle.Y < this.Rectangle.Height / 2)
                    {
                        return true;
                    }
                    break;
                    //top right
                case 1:
                    if (rectangle.X > this.Rectangle.Width / 2 && rectangle.Y < this.Rectangle.Height / 2)
                    {
                        return true;
                    }
                    break;
                    //bottom left
                case 2:
                    if (rectangle.X < this.Rectangle.Width / 2 && rectangle.Y > this.Rectangle.Height / 2)
                    {
                        return true;
                    }
                    break;
                    //bottom right
                case 3:
                    if (rectangle.X > this.Rectangle.Width / 2 && rectangle.Y > this.Rectangle.Height / 2)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// Runs through all 4 quadrants to see if the rectangle fits entirely into their bounds. If so, stop short and return that quadrants index, if not,
        /// return -1
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        public int GetIndex(ICollidable collider)
        {
            for(int i =0; i < 4; i++)
            {
                if(DoesFitIntoQuadrant(i, collider.Rectangle))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(ICollidable collider)
        {

        }
    }
}
