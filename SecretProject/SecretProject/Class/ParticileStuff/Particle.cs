using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ParticileStuff
{
    public class Particle
    {
        public Texture2D ParticleTexture { get; set; }
        public Vector2 Position { get; set; }
        public float BaseY { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public Color Color { get; set; }
        public float ColorMultiplier { get; set; }
        public float Size { get; set; }
        public int TTL { get; set; }
        public float VelocityReductionTimer { get; set; }

        public float LayerDepth { get; set; }


        public Particle(Texture2D particleTexture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl, float layerDepth = 1f)
        {
            this.ParticleTexture = particleTexture;
            this.Position = position;
            this.Velocity = velocity;
            this.Angle = angle;
            this.AngularVelocity = angularVelocity;
            this.Color = color;
            this.ColorMultiplier = 1f;
            this.TTL = ttl;
            this.LayerDepth = layerDepth;

            this.BaseY = Game1.Utility.RFloat(position.Y - 5, position.Y + 20);
            this.Size = Game1.Utility.RGenerator.Next(1, 3);
            this.VelocityReductionTimer = Game1.Utility.RFloat(.25f, .5f);

        }


        public virtual void Update(GameTime gameTime)
        {
            VelocityReductionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(VelocityReductionTimer < 0)
            {
                
                Velocity = new Vector2(Velocity.X, Velocity.Y + .1f);
            }
            TTL--;
           
            Position += Velocity;
            if (Position.Y > BaseY + 10)
            {
                Position = new Vector2(Position.X, Position.Y - Velocity.Y);
            }
                //Position = new Vector2(Position.X, Position.Y )
                Angle += AngularVelocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, this.ParticleTexture.Width, this.ParticleTexture.Height);
            Vector2 origin = new Vector2(this.ParticleTexture.Width / 2, this.ParticleTexture.Height / 2);

            spriteBatch.Draw(this.ParticleTexture, Position, sourceRectangle, Color * ColorMultiplier,
                Angle, origin, Size, SpriteEffects.None, LayerDepth);
        }
    }
}
