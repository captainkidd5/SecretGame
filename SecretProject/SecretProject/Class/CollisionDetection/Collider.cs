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



        public bool DidCollide(List<ObjectBody> objectBody)
        {
            
            foreach (var body in objectBody)
            {
                if (body.Rectangle.Left < Game1.cam.Pos.X + (Game1.ScreenWidth / 2) - 400 && body.Rectangle.Left > Game1.cam.Pos.X - (Game1.ScreenWidth / 2) - 400
                             && body.Rectangle.Y < Game1.cam.Pos.Y + (Game1.ScreenHeight / 2) + 300 && body.Rectangle.Y > Game1.cam.Pos.Y - (Game1.ScreenHeight / 2) + 300)
                {
                    if (velocity.X > 0 && IsTouchingLeft(rectangle, body, velocity))
                    {
                        velocity.X -= velocity.X; //+ (float).25;
                        return true;
                    }
                        


                    if (velocity.X < 0 && IsTouchingRight(rectangle, body, velocity))
                    {
                        velocity.X -= velocity.X; //- (float).25;
                        return true;
                    }
                        

                    if (velocity.Y > 0 && IsTouchingTop(rectangle, body, velocity))
                    {
                        velocity.Y -= velocity.Y; //+ (float).25;
                        return true;
                    }
                        
                    if (velocity.Y < 0 && IsTouchingBottom(rectangle, body, velocity))
                    {
                        velocity.Y -= velocity.Y;// - (float).25;
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
                if (spr.TextureDestinationRectangle.Left < Game1.cam.Pos.X + (Game1.ScreenWidth / 2) - 400 && spr.TextureDestinationRectangle.Left > Game1.cam.Pos.X - (Game1.ScreenWidth / 2) - 400
                             && spr.TextureDestinationRectangle.Y < Game1.cam.Pos.Y + (Game1.ScreenHeight / 2) + 300 && spr.TextureDestinationRectangle.Y > Game1.cam.Pos.Y - (Game1.ScreenHeight / 2) + 300)
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
            return rectangle.Right + velocity.X > sprite.TextureDestinationRectangle.Left &&
                rectangle.Left < sprite.TextureDestinationRectangle.Left &&
                rectangle.Bottom > sprite.TextureDestinationRectangle.Top &&
                rectangle.Top < sprite.TextureDestinationRectangle.Bottom;
        }
        public bool IsTouchingRight(Rectangle rectangle, Sprite sprite, Vector2 velocity)
        {
            return rectangle.Left + velocity.X < sprite.TextureDestinationRectangle.Right &&
                rectangle.Right > sprite.TextureDestinationRectangle.Right &&
                rectangle.Bottom > sprite.TextureDestinationRectangle.Top &&
                rectangle.Top < sprite.TextureDestinationRectangle.Bottom;
        }
        public bool IsTouchingTop(Rectangle rectangle, Sprite sprite, Vector2 velocity)
        {
            return rectangle.Bottom + velocity.Y > sprite.Rectangle.Top &&
                rectangle.Top < sprite.TextureDestinationRectangle.Top &&
                rectangle.Right > sprite.TextureDestinationRectangle.Left &&
                rectangle.Left < sprite.TextureDestinationRectangle.Right;
        }
        public bool IsTouchingBottom(Rectangle rectangle, Sprite sprite, Vector2 velocity)
        {
            return rectangle.Top + velocity.Y < sprite.TextureDestinationRectangle.Bottom &&
                rectangle.Bottom > sprite.TextureDestinationRectangle.Bottom &&
                rectangle.Right > sprite.TextureDestinationRectangle.Left &&
                rectangle.Left < sprite.TextureDestinationRectangle.Right;
        }

       
    }
}
