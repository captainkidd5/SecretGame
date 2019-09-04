using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ParticileStuff
{
    public class ParticleEngine
    {
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<Texture2D> textures;
        public float ActivationTime { get; set; } = 0f;
        public Color Color { get; set; } = Color.White;

        public ParticleEngine(List<Texture2D> textures, Vector2 location)
        {
            this.EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();

        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[Game1.Utility.RGenerator.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                .25f * (float)(Game1.Utility.RGenerator.NextDouble() * 2 -1),
                .5f * (float)(Game1.Utility.RGenerator.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(Game1.Utility.RGenerator.NextDouble() * 2 - 1);
            Color color = Color;
            float size = 1f;
            int ttl = 20 + Game1.Utility.RGenerator.Next(40);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }


        public void Update(GameTime gameTime)
        {
            if (ActivationTime > 0)
            {

                int total = 2;

                for (int i = 0; i < total; i++)
                {
                    particles.Add(GenerateNewParticle());
                }

                for (int particle = 0; particle < particles.Count; particle++)
                {
                    particles[particle].Update(gameTime);
                    if (particles[particle].TTL <= 0)
                    {
                        particles.RemoveAt(particle);
                        particle--;
                    }
                }
                ActivationTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                particles.Clear();
            }
        }


        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if(ActivationTime > 0)
            {
                for (int index = 0; index < particles.Count; index++)
                {
                    particles[index].Draw(spriteBatch, layerDepth);
                }

            }
            
        }
    }
}
