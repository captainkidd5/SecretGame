using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Physics
{
    public class Bouncer
    {
        public Vector2 BounceObjectPosition { get; set; }
        public Vector2 BaseVelocity;
        public Vector2 Velocity;
        public Dir DirectionToBounce { get; set; }
        public float VerticalFloor { get; set; }
        public float VerticalSpeed { get; set; } = 2f;
        public float HorizontalSpeed { get; set; } = .75f;

        public Vector2 AirResistance { get; set; } = new Vector2(2f, 12f);
        public bool IsActive { get; set; }

        public int NumBounces { get; set; }

        public Bouncer(Vector2 bounceObjectStartPosition, Dir directionToBounce)
        {
            this.BounceObjectPosition = bounceObjectStartPosition;
            this.DirectionToBounce = directionToBounce;

            this.VerticalFloor = this.VerticalSpeed * Game1.Utility.RFloat(.75f, 3f);

            int horizontalMultiplier = 1;
            if(Game1.Utility.RNumber(0,3) == 1)
            {
                horizontalMultiplier = -1;
            }
            switch(DirectionToBounce)
            {
                case Dir.Down:
                    Velocity = new Vector2(HorizontalSpeed * horizontalMultiplier, this.VerticalSpeed);
                    break;
                case Dir.Left:
                    Velocity = new Vector2(-HorizontalSpeed, -this.VerticalSpeed);
                    break;
                case Dir.Right:
                    Velocity = new Vector2(HorizontalSpeed, -this.VerticalSpeed);
                    break;
                case Dir.Up:
                    Velocity = new Vector2(HorizontalSpeed * horizontalMultiplier, -this.VerticalSpeed);
                    break;
                default:
                    Velocity = new Vector2(HorizontalSpeed, - this.VerticalSpeed);
                    break;
            }
            this.BaseVelocity = Velocity;
            this.VerticalFloor = bounceObjectStartPosition.Y + Game1.Utility.RNumber(-3,8);
            this.IsActive = true;
            this.NumBounces = 0;
        }

        public Vector2 Update(GameTime gameTime, ref Vector2 primaryVelocity)
        {
            //this.Velocity = primaryVelocity;
            Velocity += AirResistance * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.BounceObjectPosition += Velocity;
            if (this.BounceObjectPosition.Y >= this.VerticalFloor)
            {
                
                float newVelocityY = this.BaseVelocity.Y * .9f;
                float newVelocityX = this.BaseVelocity.X * .6f;
                this.Velocity = new Vector2(newVelocityX, newVelocityY);
                this.BaseVelocity = this.Velocity;

                NumBounces++;


            }

            if (NumBounces > 6||Math.Abs(Velocity.X) <= .01 || Math.Abs(Velocity.Y) <= .000005)
            {
                this.IsActive = false;
            }
            return this.BounceObjectPosition;
        }


    }
}
