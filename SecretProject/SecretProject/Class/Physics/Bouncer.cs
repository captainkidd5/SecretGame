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

        public Vector2 AirResistance { get; set; } = new Vector2(.25f, .9f);
        public bool IsActive { get; set; }
        public int TimesBounced { get; set; }

        public Bouncer(Vector2 bounceObjectStartPosition, Dir directionToBounce)
        {
            this.BounceObjectPosition = bounceObjectStartPosition;
            this.DirectionToBounce = directionToBounce;

            switch(DirectionToBounce)
            {
                case Dir.Down:
                    Velocity = new Vector2(0, .1f);
                    break;
                case Dir.Left:
                    Velocity = new Vector2(-.1f, -.1f);
                    break;
                case Dir.Right:
                    Velocity = new Vector2(.1f, -.1f);
                    break;
                case Dir.Up:
                    Velocity = new Vector2(0, -.1f);
                    break;
            }
            this.BaseVelocity = Velocity;
            this.VerticalFloor = bounceObjectStartPosition.Y + 20;
            this.IsActive = true;
            this.TimesBounced = 0;
        }

        public Vector2 Update(GameTime gameTime)
        {
            float newY = this.BounceObjectPosition.Y + (float)gameTime.ElapsedGameTime.TotalMilliseconds * Velocity.Y;
            float newX = this.BounceObjectPosition.X + (float)gameTime.ElapsedGameTime.TotalMilliseconds * Velocity.X;
            Velocity = new Vector2(Velocity.X * AirResistance.X, Velocity.Y * AirResistance.Y);
            if(Math.Abs(Velocity.Y) <= .00001)
            {
                Velocity = new Vector2(Velocity.X, Math.Abs(BaseVelocity.Y) * -1);
            }
            if(this.BounceObjectPosition.Y >= this.VerticalFloor)
            {
                Velocity = this.BaseVelocity / 2;
                TimesBounced++;
            }
            this.BounceObjectPosition = new Vector2(newX, newY);
           
            if(TimesBounced >= 4)
            {
                this.IsActive = false;
            }
            return this.BounceObjectPosition;
        }


    }
}
