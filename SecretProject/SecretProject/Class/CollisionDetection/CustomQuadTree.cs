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
            this.Nodes = new List<Node>(SplitLimit);
        }

        public void Insert()
        {

        }
    }

    public class Node
    {
        // 0 thru 3, clockwise
        public int Quadrant { get; set; }
        public Rectangle Rectangle { get; set; }

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
            this.SubNodes = new List<Node>(CustomQuadTree.SplitLimit);
        }
    }
}
