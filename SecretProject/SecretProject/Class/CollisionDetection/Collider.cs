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



        public bool DidCollide(Dictionary<int,ObjectBody> objectBody, Vector2 position)
        {
            
            foreach (var body in objectBody.Values)
            {
                if (body.Rectangle.Left < Game1.cam.Pos.X + (Game1.ScreenWidth / 2 / Game1.cam.Zoom) && body.Rectangle.Left > Game1.cam.Pos.X - (Game1.ScreenWidth / 2 / Game1.cam.Zoom + 16)
                             && body.Rectangle.Y < Game1.cam.Pos.Y + (Game1.ScreenHeight / 2 / Game1.cam.Zoom + 16) && body.Rectangle.Y > Game1.cam.Pos.Y - (Game1.ScreenHeight / 2 / Game1.cam.Zoom + 16))
                {
                    
                    if (velocity.X > 0 && IsTouchingLeft(rectangle, body, velocity))
                    {
                        velocity.X -= velocity.X; //+ (float).25;
                        position.X = body.Rectangle.Left - rectangle.Right;
                        return true;
                    }
                        


                    if (velocity.X < 0 && IsTouchingRight(rectangle, body, velocity))
                    {
                        velocity.X -= velocity.X; //- (float).25;
                        position.X = body.Rectangle.Right - rectangle.Left;
                        return true;
                    }
                        

                    if (velocity.Y > 0 && IsTouchingTop(rectangle, body, velocity))
                    {
                        velocity.Y -= velocity.Y; //+ (float).25;
                        position.Y = body.Rectangle.Top - Rectangle.Top;
                        return true;
                    }
                        
                    if (velocity.Y < 0 && IsTouchingBottom(rectangle, body, velocity))
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

                    if (velocity.X > 0 && IsTouchingLeft(rectangle, spr, velocity))
                        velocity.X -= velocity.X; //+ (float).25;

                    if (velocity.X < 0 && IsTouchingRight(rectangle, spr, velocity))
                        velocity.X -= velocity.X; //- (float).25;

                    if (velocity.Y > 0 && IsTouchingTop(rectangle, spr, velocity))
                        velocity.Y -= velocity.Y; //+ (float).25;
                    if (velocity.Y < 0 && IsTouchingBottom(rectangle, spr, velocity))
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

                    else if (velocity.X > 0 && IsTouchingLeft(rectangle, item[i].ItemSprite, velocity))
                    {
                        item[i].IsMagnetizable = true;
                    }


                    else if (velocity.X < 0 && IsTouchingRight(rectangle, item[i].ItemSprite, velocity))
                    {
                        item[i].IsMagnetizable = true;
                    }


                    else if (velocity.Y > 0 && IsTouchingTop(rectangle, item[i].ItemSprite, velocity))
                    {
                        item[i].IsMagnetizable = true;
                    }

                    else if (velocity.Y < 0 && IsTouchingBottom(rectangle, item[i].ItemSprite, velocity))
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

        public bool IsTouchingLeft(Rectangle rectangle, ObjectBody obj, Vector2 velocity)
        {
            return rectangle.Right + velocity.X > obj.Rectangle.Left &&
                rectangle.Left < obj.Rectangle.Left &&
                rectangle.Bottom > obj.Rectangle.Top &&
                rectangle.Top < obj.Rectangle.Bottom;
        }
        public bool IsTouchingRight(Rectangle rectangle, ObjectBody obj, Vector2 velocity)
        {
            return rectangle.Left + velocity.X < obj.Rectangle.Right &&
                rectangle.Right > obj.Rectangle.Right &&
                rectangle.Bottom > obj.Rectangle.Top &&
                rectangle.Top < obj.Rectangle.Bottom;
        }
        public bool IsTouchingTop(Rectangle rectangle, ObjectBody obj, Vector2 velocity)
        {
            return rectangle.Bottom + velocity.Y > obj.Rectangle.Top &&
                rectangle.Top < obj.Rectangle.Top &&
                rectangle.Right > obj.Rectangle.Left &&
                rectangle.Left < obj.Rectangle.Right;
        }
        public bool IsTouchingBottom(Rectangle rectangle, ObjectBody obj, Vector2 velocity)
        {
            return rectangle.Top + velocity.Y < obj.Rectangle.Bottom &&
                rectangle.Bottom > obj.Rectangle.Bottom &&
                rectangle.Right > obj.Rectangle.Left &&
                rectangle.Left < obj.Rectangle.Right;
        }

        //Sprite part

        public bool IsTouchingLeft(Rectangle rectangle, Sprite sprite, Vector2 velocity)
        {
            return rectangle.Right + velocity.X > sprite.DestinationRectangle.Left &&
                rectangle.Left < sprite.DestinationRectangle.Left &&
                rectangle.Bottom > sprite.DestinationRectangle.Top &&
                rectangle.Top < sprite.DestinationRectangle.Bottom;
        }
        public bool IsTouchingRight(Rectangle rectangle, Sprite sprite, Vector2 velocity)
        {
            return rectangle.Left + velocity.X < sprite.DestinationRectangle.Right &&
                rectangle.Right > sprite.DestinationRectangle.Right &&
                rectangle.Bottom > sprite.DestinationRectangle.Top &&
                rectangle.Top < sprite.DestinationRectangle.Bottom;
        }
        public bool IsTouchingTop(Rectangle rectangle, Sprite sprite, Vector2 velocity)
        {
            return rectangle.Bottom + velocity.Y > sprite.DestinationRectangle.Top &&
                rectangle.Top < sprite.DestinationRectangle.Top &&
                rectangle.Right > sprite.DestinationRectangle.Left &&
                rectangle.Left < sprite.DestinationRectangle.Right;
        }
        public bool IsTouchingBottom(Rectangle rectangle, Sprite sprite, Vector2 velocity)
        {
            return rectangle.Top + velocity.Y < sprite.DestinationRectangle.Bottom &&
                rectangle.Bottom > sprite.DestinationRectangle.Bottom &&
                rectangle.Right > sprite.DestinationRectangle.Left &&
                rectangle.Left < sprite.DestinationRectangle.Right;
        }

       
    }
}
