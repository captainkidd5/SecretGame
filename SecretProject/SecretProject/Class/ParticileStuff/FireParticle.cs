using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ParticileStuff
{
    public class FireParticle : Particle
    {
        public float GroundLevel { get; set; }
        public float BaseLayerDepth { get; set; }

        public FireParticle(Texture2D particleTexture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl, float layerDepth = 1f) : base(particleTexture, position, velocity, angle, angularVelocity, color, size, ttl, layerDepth = 1f)
        {
            GroundLevel = Game1.Utility.RFloat(.25f, .5f);

            if(Game1.Utility.RGenerator.Next(0, 2) == 0)
            {
                LayerDepth = layerDepth + -.5f;
            }
            else
            {
                LayerDepth = layerDepth;
            }
            this.BaseY = position.Y;
        }

        public override void Update(GameTime gameTime)
        {

            TTL--;

            Position -= Velocity;

            



            // Position.X += Math.Sin(SinMultiplier);
            if (Position.Y < BaseY - GroundLevel)
            {
                Position = new Vector2(Position.X, Position.Y + Velocity.Y);
                TTL = 20;
            }

        }
    }
}
