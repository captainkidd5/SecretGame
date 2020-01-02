using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ParticileStuff
{
    public class SmokeParticle : Particle
    {
        public float GroundLevel { get; set; }
        public float SinMultiplier { get; set; }
        public int One { get; set; }

        public int Direction { get; set; }

        public SmokeParticle(Texture2D particleTexture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl, float layerDepth = 1f) : base(particleTexture, position, velocity, angle, angularVelocity, color, size, ttl, layerDepth = 1f)
        {
            GroundLevel = Game1.Utility.RGenerator.Next(45, 65);
            SinMultiplier = 0;
            One = 1;



            Direction = Game1.Utility.RGenerator.Next(0, 2);

        }

        public override void Update(GameTime gameTime)
        {

            TTL--;

            Position -= Velocity;
            if(Direction == 0)
            {
                SinMultiplier += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                SinMultiplier -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            

            if(SinMultiplier >= .5f)
            {
                One = One * -1;
            }
            if (SinMultiplier <= -.5f)
            {
                One = One * -1;
            }

            SinMultiplier = SinMultiplier * One;


            Position = new Vector2(Position.X + (float)Math.Sin(SinMultiplier), Position.Y);
           // Position.X += Math.Sin(SinMultiplier);
            if (Position.Y < BaseY - GroundLevel)
            {
                Position = new Vector2(Position.X, Position.Y + Velocity.Y);
                TTL = 40;
            }

        }
    }
}
