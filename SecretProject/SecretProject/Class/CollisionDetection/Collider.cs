using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SecretProject.Class.ObjectFolder;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.ItemStuff;

namespace SecretProject.Class.CollisionDetection
{
    public struct RectangleCollider
    {
        private Vector2 topLeft;
        private Vector2 bottomRight;
        public Vector2 TopLeft { get { return topLeft; } set { topLeft = value; } }
        public Vector2 BottomRight { get { return bottomRight; } set { bottomRight = value; } }

        public Vector2 TopRight { get { return new Vector2(bottomRight.X, topLeft.Y); } }
        public Vector2 BottomLeft { get { return new Vector2(topLeft.X, bottomRight.Y); } }

        public float Top { get { return topLeft.Y; } }
        public float Right { get { return bottomRight.X; } }
        public float Left { get { return topLeft.X; } }
        public float Bottom { get { return bottomRight.Y; } }

        public RectangleCollider(Vector2 topLeft, Vector2 bottomRight)
        {
            this.topLeft = topLeft;
            this.bottomRight = bottomRight;

       
        }

        public bool Contains(Vector2 Point)
        {
            return (topLeft.X <= Point.X && bottomRight.X >= Point.X &&
                    topLeft.Y <= Point.Y && bottomRight.Y >= Point.Y);
        }

        public bool Intersects(RectangleCollider Rect)
        {
            return (!(Bottom < Rect.Top ||
                       Top > Rect.Bottom ||
                       Right < Rect.Left ||
                       Left > Rect.Right));
        }


    }

    public class Collider
    {
        private Vector2 velocity;
        private Rectangle rectangle;

        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }

        public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }

        private Collider()
        {

        }

        public Collider(Vector2 velocity, Rectangle rectangle)
        {
            this.velocity = velocity;
            this.rectangle = rectangle;
        }



        public bool DidCollide(Dictionary<string,ObjectBody> objectBody, Vector2 position)
        {
            
            foreach (var body in objectBody.Values)
            {
                if (body.Rectangle.Left < Game1.cam.Pos.X + (Game1.ScreenWidth / 2 / Game1.cam.Zoom) && body.Rectangle.Left > Game1.cam.Pos.X - (Game1.ScreenWidth / 2 / Game1.cam.Zoom + 16)
                             && body.Rectangle.Y < Game1.cam.Pos.Y + (Game1.ScreenHeight / 2 / Game1.cam.Zoom + 16) && body.Rectangle.Y > Game1.cam.Pos.Y - (Game1.ScreenHeight / 2 / Game1.cam.Zoom + 16))
                {
                    
                    if (velocity.X > 0 && IsTouchingLeft(rectangle, body.Rectangle, velocity))
                    {
                        velocity.X -= velocity.X; //+ (float).25;
                        position.X = body.Rectangle.Left - rectangle.Right;
                        return true;
                    }
                        


                    if (velocity.X < 0 && IsTouchingRight(rectangle, body.Rectangle, velocity))
                    {
                        velocity.X -= velocity.X; //- (float).25;
                        position.X = body.Rectangle.Right - rectangle.Left;
                        return true;
                    }
                        

                    if (velocity.Y > 0 && IsTouchingTop(rectangle, body.Rectangle, velocity))
                    {
                        velocity.Y -= velocity.Y; //+ (float).25;
                        position.Y = body.Rectangle.Top - Rectangle.Top;
                        return true;
                    }
                        
                    if (velocity.Y < 0 && IsTouchingBottom(rectangle, body.Rectangle, velocity))
                    {
                        velocity.Y -= velocity.Y;// - (float).25;
                        position.Y = body.Rectangle.Bottom - Rectangle.Bottom;
                        return true;
                    }

                 
                        
                }
            }
            return false;

        }

        public void DidCollide(List<Sprite> sprite)
        {

            foreach (var spr in sprite)
            {
                if (spr.DestinationRectangle.Left < Game1.cam.Pos.X + (Game1.ScreenWidth / 2) - 400 && spr.DestinationRectangle.Left > Game1.cam.Pos.X - (Game1.ScreenWidth / 2) - 400
                             && spr.DestinationRectangle.Y < Game1.cam.Pos.Y + (Game1.ScreenHeight / 2) + 300 && spr.DestinationRectangle.Y > Game1.cam.Pos.Y - (Game1.ScreenHeight / 2) + 300)
                {

                    if (velocity.X > 0 && IsTouchingLeft(rectangle, spr.DestinationRectangle, velocity))
                        velocity.X -= velocity.X; //+ (float).25;

                    if (velocity.X < 0 && IsTouchingRight(rectangle, spr.DestinationRectangle, velocity))
                        velocity.X -= velocity.X; //- (float).25;

                    if (velocity.Y > 0 && IsTouchingTop(rectangle, spr.DestinationRectangle, velocity))
                        velocity.Y -= velocity.Y; //+ (float).25;
                    if (velocity.Y < 0 && IsTouchingBottom(rectangle, spr.DestinationRectangle, velocity))
                        velocity.Y -= velocity.Y;// - (float).25;
                }
            }

        }

        

        public void DidCollideMagnet(List<Item> item)
        {
            for(int i = 0; i < item.Count; i ++)
            {
                if (item[i].Ignored == false)
                {
                    if (item[i].IsMagnetized)
                    {
                        item[i].Magnetize(velocity);
                        //it.IsMagnetized = false;
                    }

                    else if (velocity.X > 0 && IsTouchingLeft(rectangle, item[i].ItemSprite.DestinationRectangle, velocity))
                    {
                        item[i].IsMagnetizable = true;
                    }


                    else if (velocity.X < 0 && IsTouchingRight(rectangle, item[i].ItemSprite.DestinationRectangle, velocity))
                    {
                        item[i].IsMagnetizable = true;
                    }


                    else if (velocity.Y > 0 && IsTouchingTop(rectangle, item[i].ItemSprite.DestinationRectangle, velocity))
                    {
                        item[i].IsMagnetizable = true;
                    }

                    else if (velocity.Y < 0 && IsTouchingBottom(rectangle, item[i].ItemSprite.DestinationRectangle, velocity))
                    {
                        item[i].IsMagnetizable = true;
                    }

                    else
                    {
                        item[i].IsMagnetizable = false;
                    }
                }
            }

            
                
                
            
        }

        public bool IsTouchingLeft(Rectangle rectangle, Rectangle objectRectangle, Vector2 velocity)
        {
            return rectangle.Right + velocity.X > objectRectangle.Left &&
                rectangle.Left < objectRectangle.Left &&
                rectangle.Bottom > objectRectangle.Top &&
                rectangle.Top < objectRectangle.Bottom;
        }
        public bool IsTouchingRight(Rectangle rectangle, Rectangle objectRectangle, Vector2 velocity)
        {
            return rectangle.Left + velocity.X < objectRectangle.Right &&
                rectangle.Right > objectRectangle.Right &&
                rectangle.Bottom > objectRectangle.Top &&
                rectangle.Top < objectRectangle.Bottom;
        }
        public bool IsTouchingTop(Rectangle rectangle, Rectangle objectRectangle, Vector2 velocity)
        {
            return rectangle.Bottom + velocity.Y > objectRectangle.Top &&
                rectangle.Top < objectRectangle.Top &&
                rectangle.Right > objectRectangle.Left &&
                rectangle.Left < objectRectangle.Right;
        }
        public bool IsTouchingBottom(Rectangle rectangle, Rectangle objectRectangle, Vector2 velocity)
        {
            return rectangle.Top + velocity.Y < objectRectangle.Bottom &&
                rectangle.Bottom > objectRectangle.Bottom &&
                rectangle.Right > objectRectangle.Left &&
                rectangle.Left < objectRectangle.Right;
        }

        
       
    }
}
