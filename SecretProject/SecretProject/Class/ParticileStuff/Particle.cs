﻿using Microsoft.Xna.Framework;
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
        public float VelocityReductionTimer { get; set; }


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
            this.VelocityReductionTimer = .01f;
        }

        public void Update(GameTime gameTime)
        {
            VelocityReductionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(VelocityReductionTimer < 0)
            {
                Position = new Vector2(Position.X, Position.Y + .05f);
            }
            TTL--;
            Position += Velocity;
            //Position = new Vector2(Position.X, Position.Y )
            Angle += AngularVelocity;
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, this.ParticleTexture.Width, this.ParticleTexture.Height);
            Vector2 origin = new Vector2(this.ParticleTexture.Width / 2, this.ParticleTexture.Height / 2);

            spriteBatch.Draw(this.ParticleTexture, Position, sourceRectangle, Color,
                Angle, origin, Size, SpriteEffects.None, layerDepth);
        }
    }
}
