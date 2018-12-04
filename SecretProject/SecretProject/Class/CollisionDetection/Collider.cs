using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SecretProject.Class.ObjectFolder;

namespace SecretProject.Class.CollisionDetection
{
    public class Collider
    {
        private List<ObjectBody> ObjectBody;

        public Collider(List<ObjectBody> objectBody)
        {
            this.ObjectBody = objectBody;
        }



        public void DidCollide(Rectangle rectangle, Vector2 velocity)
        {
            foreach (var body in ObjectBody)
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
    }
}
