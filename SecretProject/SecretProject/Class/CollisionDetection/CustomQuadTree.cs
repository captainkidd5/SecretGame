using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.CollisionDetection
{
    public class CustomQuadTree
    {
        // 0 thru 3, clockwise
        public int Quadrant { get; private set; }
        public int NodeLevel { get; set; }
        public List<CustomQuadTree> SubNodes { get; set; }
        public static int SplitLimit = 30;
        public static int MaximumLevels = 8;

        public Rectangle Rectangle { get; set; }


        public List<ICollidable> Objects { get; private set; }

        public Texture2D ColoredRectangle { get; set; }
        public GraphicsDevice Graphics { get; set; }



        public CustomQuadTree(GraphicsDevice graphics, Rectangle rectangle)
        {
            this.NodeLevel = 0;
            this.Rectangle = rectangle;
            this.Objects = new List<ICollidable>();
            this.Graphics = graphics;

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            for (int i = 0; i < this.Objects.Count; i++)
            {
                spriteBatch.Draw(Game1.AllTextures.redPixel, new Vector2(this.Objects[i].Rectangle.X, this.Objects[i].Rectangle.Y), null, Color.White, 0f, Game1.Utility.Origin, 5f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(Game1.AllTextures.MenuText, this.NodeLevel.ToString(), new Vector2(this.Objects[i].Rectangle.X, this.Objects[i].Rectangle.Y), Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, 1f);
            }


            if (this.SubNodes != null)
            {
                for (int i = 0; i < this.SubNodes.Count; i++)
                {
                    this.SubNodes[i].Draw(spriteBatch);
                }
            }

        }

        private CustomQuadTree(GraphicsDevice graphics, int nodeLevel, int quadrant, Rectangle rectangle, List<ICollidable> objects)
        {
            this.Graphics = graphics;

            this.NodeLevel = nodeLevel;
            this.Quadrant = quadrant;

            switch (this.Quadrant)
            {
                //top left
                case 0:
                    this.Rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width / 2, rectangle.Height / 2);
                    break;
                //top right
                case 1:
                    this.Rectangle = new Rectangle(rectangle.X + rectangle.Width / 2, rectangle.Y, rectangle.Width / 2, rectangle.Height / 2);
                    break;
                //bottom left
                case 2:
                    this.Rectangle = new Rectangle(rectangle.X, rectangle.Y + rectangle.Height / 2, rectangle.Width / 2, rectangle.Height / 2);
                    break;
                //bottom right
                case 3:
                    this.Rectangle = new Rectangle(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2, rectangle.Width / 2, rectangle.Height / 2);
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
            this.SubNodes = new List<CustomQuadTree>(4);
            List<List<ICollidable>> collideLists = new List<List<ICollidable>>
            {
                new List<ICollidable>(),
                new List<ICollidable>(),
                new List<ICollidable>(),
                new List<ICollidable>(),
            };
            for (int col = 0; col < this.Objects.Count; col++)
            {
                int colliderIndex = GetIndex(this.Objects[col]);
                if (colliderIndex == -1) //didn't fit into any subnodes completely, remove it from the parent node and just add it to all four subnodes
                {
     
                    for(int i =0; i < 4; i++)
                    {
                        collideLists[i].Add(this.Objects[col]); //which list the object is inserted into is determined with the getindex() method
                    }
                    this.Objects.Remove(this.Objects[col]); //if we insert it into a subnode, we want to remove it from the parent node
                }
                else //only add it to the subnode it completely fit into
                {
                    collideLists[colliderIndex].Add(this.Objects[col]); //which list the object is inserted into is determined with the getindex() method
                    this.Objects.Remove(this.Objects[col]); //if we insert it into a subnode, we want to remove it from the parent node
                }

            }

            for (int i = 0; i < 4; i++)
            {
                this.SubNodes.Add(new CustomQuadTree(this.Graphics, this.NodeLevel + 1, i, this.Rectangle, collideLists[i]));
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
            switch (quadrant)
            {
                //top left
                case 0:
                    if (rectangle.X + rectangle.Width < this.Rectangle.Width / 2 && rectangle.Y + rectangle.Height < this.Rectangle.Height / 2)
                    {
                        return true;
                    }
                    break;
                //top right
                case 1:
                    if (rectangle.X > this.Rectangle.Width / 2 && rectangle.Y + rectangle.Height < this.Rectangle.Height / 2)
                    {
                        return true;
                    }
                    break;
                //bottom left
                case 2:
                    if (rectangle.X + rectangle.Width < this.Rectangle.Width / 2 && rectangle.Y > this.Rectangle.Height / 2)
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
            for (int i = 0; i < 4; i++)
            {
                if (DoesFitIntoQuadrant(i, collider.Rectangle))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(ICollidable collider)
        {
            if (this.Objects.Count < CustomQuadTree.SplitLimit)
            {

            }
            else //only split if we've exceeded the max object limit
            {
                if (this.SubNodes == null) //split hasn't occurred yet
                {
                    if (this.NodeLevel < CustomQuadTree.MaximumLevels)
                    {
                        Split();
                    }
                    else
                    {
                        this.Objects.Add(collider); //if we get here it means the collider didn't fit completely into any of the subnodes and we just stick it into this (the parent node). 
                        return;
                    }

                }
                for (int i = 0; i < this.SubNodes.Count; i++)
                {
                    if (DoesFitIntoQuadrant(i, collider.Rectangle))
                    {
                        this.SubNodes[i].Insert(collider);
                        return;
                    }
                }
            }

            this.Objects.Add(collider); //if we get here it means the collider didn't fit completely into any of the subnodes and we just stick it into this (the parent node). 
        }

        public void RetrievePotentialCollisions(List<ICollidable> colliderList, Collider colliderToTest)
        {
            int index = GetIndex(colliderToTest);
            if(index != -1)
            {
                if (SubNodes != null)
                {
                    SubNodes[index].RetrievePotentialCollisions(colliderList, colliderToTest);
                }
                else
                {
                    colliderList.AddRange(this.Objects);
                }
            }
            else
            {
               // colliderList.AddRange(this.Objects);
            }
            //else
            //{
            //    colliderList.AddRange(Objects);
            //}
            //if (GetIndex(colliderToTest) == -1)
            //{
            //    colliderList.AddRange(Objects);
            //}
            //else
            //{
            //    if (SubNodes != null)
            //    {
            //        for (int i = 0; i < this.SubNodes.Count; i++)
            //        {
            //            SubNodes[i].RetrievePotentialCollisions(colliderList, colliderToTest);

            //        }
            //    }
            //}

        }

    }


}
