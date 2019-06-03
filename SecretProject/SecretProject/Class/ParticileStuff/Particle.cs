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
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public Color Color { get; set; }
        public float Size { get; set; }
        public int TTL { get; set; }


        public Particle(Texture2D particleTexture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl)
        {
            this.ParticleTexture = particleTexture;
            this.Position = position;
            this.Velocity = velocity;
            this.Angle = angle;
            this.AngularVelocity = angularVelocity;
            this.Color = color;
            this.Size = size;
            this.TTL = ttl;
        }

        public void Update()
        {
            TTL--;
            Position += Velocity;
            Angle += AngularVelocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, this.ParticleTexture.Width, this.ParticleTexture.Height);
            Vector2 origin = new Vector2(this.ParticleTexture.Width / 2, this.ParticleTexture.Height / 2);

            spriteBatch.Draw(this.ParticleTexture, Position, sourceRectangle, Color,
                Angle, origin, Size, SpriteEffects.None, 0f);
        }
    }
}
