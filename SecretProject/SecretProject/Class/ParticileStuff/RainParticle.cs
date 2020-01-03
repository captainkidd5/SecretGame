using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SecretProject.Class.ParticileStuff
{
    public class RainParticle : Particle
    {
        public float GroundLevel { get; set; }
        public RainParticle(Texture2D particleTexture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl, float layerDepth = 1f) : base(particleTexture, position, velocity, angle, angularVelocity, color, size, ttl, layerDepth = 1f)
        {
            this.GroundLevel = Game1.Utility.RGenerator.Next(100, 400);
        }

        public override void Update(GameTime gameTime)
        {

            this.TTL--;

            this.Position += this.Velocity;
            if (this.Position.Y > this.BaseY + this.GroundLevel)
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y - this.Velocity.Y);
                this.TTL = 40;
            }

        }
    }
}
