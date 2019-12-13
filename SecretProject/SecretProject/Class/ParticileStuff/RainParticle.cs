using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ParticileStuff
{
    public class RainParticle : Particle
    {
        public RainParticle(Texture2D particleTexture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl, float layerDepth = 1f): base( particleTexture,  position,  velocity,  angle,  angularVelocity,  color,  size,  ttl,  layerDepth = 1f)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            //VelocityReductionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            //if (VelocityReductionTimer < 0)
            //{

            //    Velocity = new Vector2(Velocity.X, Velocity.Y + .1f);
            //}
            TTL--;

            Position += Velocity;
            if (Position.Y > BaseY + Game1.Utility.RGenerator.Next(100, 400))
            {
                //Position = new Vector2(Position.X, Position.Y - Velocity.Y);
            }
            //Position = new Vector2(Position.X, Position.Y )
           // Angle += AngularVelocity;
        }
    }
}
