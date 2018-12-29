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

        public Collider(Vector2 velocity, Rectangle rectangle)
        {
            this.velocity = velocity;
            this.rectangle = rectangle;
        }



        public void DidCollide(List<ObjectBody> objectBody)
        {
            
            foreach (var body in objectBody)
            {

                if (velocity.X > 0 && IsTouchingLeft(rectangle, body, velocity))
                    velocity.X -= velocity.X; //+ (float).25;

                if (velocity.X < 0 && IsTouchingRight(rectangle, body, velocity))
                    velocity.X -= velocity.X; //- (float).25;

                if (velocity.Y > 0 && IsTouchingTop(rectangle, body, velocity))
                    velocity.Y -= velocity.Y; //+ (float).25;
                if (velocity.Y < 0 && IsTouchingBottom(rectangle, body, velocity))
                    velocity.Y -= velocity.Y;// - (float).25;
            }

        }

        public void DidCollide(List<Sprite> sprite)
        {

            foreach (var spr in sprite)
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

        

        public void DidCollideMagnet(List<WorldItem> item)
        {

            foreach (var it in item)
            {
                if (it.ItemSprite.IsMagnetized)
                {
                    it.ItemSprite.Magnetize(velocity);
                }

                if (velocity.X > 0 && IsTouchingLeft(rectangle, it.ItemSprite, velocity))
                {
                    it.ItemSprite.IsMagnetized = true;
                }


                if (velocity.X < 0 && IsTouchingRight(rectangle, it.ItemSprite, velocity))
                {
                    it.ItemSprite.IsMagnetized = true;
                }


                if (velocity.Y > 0 && IsTouchingTop(rectangle, it.ItemSprite, velocity))
                {
                    it.ItemSprite.IsMagnetized = true;
                }

                if (velocity.Y < 0 && IsTouchingBottom(rectangle, it.ItemSprite, velocity))
                {
                    it.ItemSprite.IsMagnetized = true;
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
            return rectangle.Right + velocity.X > sprite.Rectangle.Left &&
                rectangle.Left < sprite.Rectangle.Left &&
                rectangle.Bottom > sprite.Rectangle.Top &&
                rectangle.Top < sprite.Rectangle.Bottom;
        }
        public bool IsTouchingRight(Rectangle rectangle, Sprite sprite, Vector2 velocity)
        {
            return rectangle.Left + velocity.X < sprite.Rectangle.Right &&
                rectangle.Right > sprite.Rectangle.Right &&
                rectangle.Bottom > sprite.Rectangle.Top &&
                rectangle.Top < sprite.Rectangle.Bottom;
        }
        public bool IsTouchingTop(Rectangle rectangle, Sprite sprite, Vector2 velocity)
        {
            return rectangle.Bottom + velocity.Y > sprite.Rectangle.Top &&
                rectangle.Top < sprite.Rectangle.Top &&
                rectangle.Right > sprite.Rectangle.Left &&
                rectangle.Left < sprite.Rectangle.Right;
        }
        public bool IsTouchingBottom(Rectangle rectangle, Sprite sprite, Vector2 velocity)
        {
            return rectangle.Top + velocity.Y < sprite.Rectangle.Bottom &&
                rectangle.Bottom > sprite.Rectangle.Bottom &&
                rectangle.Right > sprite.Rectangle.Left &&
                rectangle.Left < sprite.Rectangle.Right;
        }

        /*
        public void DidCollideMagnet(List<Sprite> sprite)
        {

            foreach (var spr in sprite)
            {
                if(spr.IsMagnetized)
                {
                    spr.Magnetize(velocity);
                }

                if (velocity.X > 0 && IsTouchingLeft(rectangle, spr, velocity))
                {
                    spr.IsMagnetized = true;
                }
               

                if (velocity.X < 0 && IsTouchingRight(rectangle, spr, velocity))
                {
                    spr.IsMagnetized = true;
                }
              

                if (velocity.Y > 0 && IsTouchingTop(rectangle, spr, velocity))
                {
                    spr.IsMagnetized = true;
                }
                
                if (velocity.Y < 0 && IsTouchingBottom(rectangle, spr, velocity))
                {
                    spr.IsMagnetized = true;
                }
            }
        }
        */
    }
}
