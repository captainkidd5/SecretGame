using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SecretProject.Class.ParticileStuff
{
    public class FireParticle : Particle
    {
        public float GroundLevel { get; set; }
        public float BaseLayerDepth { get; set; }

        public FireParticle(Texture2D particleTexture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl, float layerDepth = 1f) : base(particleTexture, position, velocity, angle, angularVelocity, color, size, ttl, layerDepth = 1f)
        {
            this.GroundLevel = Game1.Utility.RFloat(.25f, .5f);

            if (Game1.Utility.RGenerator.Next(0, 2) == 0)
            {
                this.LayerDepth = layerDepth + -.5f;
            }
            else
            {
                this.LayerDepth = layerDepth;
            }
            this.BaseY = position.Y;
        }

        public override void Update(GameTime gameTime)
        {

            this.TTL--;

            this.Position -= this.Velocity;





            // Position.X += Math.Sin(SinMultiplier);
            if (this.Position.Y < this.BaseY - this.GroundLevel)
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y + this.Velocity.Y);
                this.TTL = 20;
            }

        }
    }
}
