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
        public Vector2 BaseVelocity { get; set; }
        public Vector2 Velocity { get; set; }
        public Dir DirectionToBounce { get; set; }
        public float VerticalFloor { get; set; }
        public float VerticalSpeed { get; set; } = 2f;
        public float HorizontalSpeed { get; set; } = .75f;

        public Vector2 AirResistance { get; set; } = new Vector2(2f, 12f);
        public bool IsActive { get; set; }
 

        public Bouncer(Vector2 bounceObjectStartPosition, Dir directionToBounce)
        {
            this.BounceObjectPosition = bounceObjectStartPosition;
            this.DirectionToBounce = directionToBounce;

            this.VerticalFloor = this.VerticalSpeed * Game1.Utility.RFloat(.75f, 1.5f);
            switch(DirectionToBounce)
            {
                case Dir.Down:
                    Velocity = new Vector2(HorizontalSpeed * Game1.Utility.RNumber(-1, 2), -this.VerticalSpeed);
                    break;
                case Dir.Left:
                    Velocity = new Vector2(HorizontalSpeed, -this.VerticalSpeed);
                    break;
                case Dir.Right:
                    Velocity = new Vector2(-HorizontalSpeed, -this.VerticalSpeed);
                    break;
                case Dir.Up:
                    Velocity = new Vector2(HorizontalSpeed * Game1.Utility.RNumber(-1, 2), -this.VerticalSpeed);
                    break;
                default:
                    Velocity = new Vector2(HorizontalSpeed, - this.VerticalSpeed);
                    break;
            }
            this.BaseVelocity = Velocity;
            this.VerticalFloor = bounceObjectStartPosition.Y + Game1.Utility.RNumber(3,8);
            this.IsActive = true;

        }

        public Vector2 Update(GameTime gameTime)
        {

            Velocity += AirResistance * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.BounceObjectPosition += Velocity;
            if (this.BounceObjectPosition.Y >= this.VerticalFloor)
            {
                this.Velocity = this.BaseVelocity *.8f;
                this.BaseVelocity = this.Velocity;

                
            }

            if (Math.Abs(Velocity.X) <= .0004 || Math.Abs(Velocity.Y) <= .005)
            {
                this.IsActive = false;
            }
            return this.BounceObjectPosition;
        }


    }
}
