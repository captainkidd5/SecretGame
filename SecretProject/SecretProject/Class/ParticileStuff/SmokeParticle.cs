using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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
            this.GroundLevel = Game1.Utility.RGenerator.Next(45, 65);
            this.SinMultiplier = 0;
            this.One = 1;



            this.Direction = Game1.Utility.RGenerator.Next(0, 2);

        }

        public override void Update(GameTime gameTime)
        {

            this.TTL--;

            this.Position -= this.Velocity;
            if (this.Direction == 0)
            {
                this.SinMultiplier += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                this.SinMultiplier -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }


            if (this.SinMultiplier >= .5f)
            {
                this.One = this.One * -1;
            }
            if (this.SinMultiplier <= -.5f)
            {
                this.One = this.One * -1;
            }

            this.SinMultiplier = this.SinMultiplier * this.One;


            this.Position = new Vector2(this.Position.X + (float)Math.Sin(this.SinMultiplier), this.Position.Y);
            // Position.X += Math.Sin(SinMultiplier);
            if (this.Position.Y < this.BaseY - this.GroundLevel)
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y + this.Velocity.Y);
                this.TTL = 40;
            }

        }
    }
}
