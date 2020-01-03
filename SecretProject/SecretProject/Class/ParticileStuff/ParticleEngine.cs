using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SecretProject.Class.ParticileStuff
{
    public class ParticleEngine
    {
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<Texture2D> textures;
        public float ActivationTime { get; set; } = 0f;
        public Color Color { get; set; } = Color.White;
        public float AddNewParticleTimer { get; set; }
        public float LayerDepth { get; set; }

        public ParticleEngine(List<Texture2D> textures, Vector2 location)
        {
            this.EmitterLocation = location;
            this.textures = textures;
            particles = new List<Particle>();
            this.LayerDepth = 1f;
            this.AddNewParticleTimer = .01f;

        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[Game1.Utility.RGenerator.Next(textures.Count)];
            Vector2 position = this.EmitterLocation;
            Vector2 velocity = new Vector2(
                1.25f * Game1.Utility.RFloat(-1, 1),
                -1);
            float angle = -1f;
            float angularVelocity = .25f;
            Color color = this.Color;
            float size = 1f;
            int ttl = 100 + Game1.Utility.RGenerator.Next(40);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl, this.LayerDepth);
        }

        private Particle GenerateNewWeatherParticle()
        {
            Texture2D texture = textures[Game1.Utility.RGenerator.Next(textures.Count)];
            Vector2 position = new Vector2(this.EmitterLocation.X + Game1.Utility.RGenerator.Next(-(int)(Game1.ScreenWidth / 2 / Game1.cam.Zoom), (int)(Game1.ScreenWidth * 1.5f / Game1.cam.Zoom)), this.EmitterLocation.Y);
            Vector2 velocity = new Vector2(
                0f,
                3f);
            float angle = 0f;
            float angularVelocity = 0f;
            Color color = this.Color;
            float size = 1f;
            int ttl = 200 + Game1.Utility.RGenerator.Next(40);

            return new RainParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl, this.LayerDepth);
        }

        private Particle GenerateNewSmokeParticle()
        {
            Texture2D texture = textures[Game1.Utility.RGenerator.Next(textures.Count)];
            Vector2 position = new Vector2(this.EmitterLocation.X + Game1.Utility.RGenerator.Next(-5, 5), this.EmitterLocation.Y);
            Vector2 velocity = new Vector2(
                0f,
                .25f);
            float angle = 0f;
            float angularVelocity = 0f;
            Color color = this.Color;
            float size = .5f;
            int ttl = 200 + Game1.Utility.RGenerator.Next(40);

            return new SmokeParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl, this.LayerDepth);
        }

        private Particle GenerateNewFireParticle()
        {
            Texture2D texture = textures[Game1.Utility.RGenerator.Next(textures.Count)];
            Vector2 position = new Vector2(this.EmitterLocation.X + Game1.Utility.RGenerator.Next(-2, 2), this.EmitterLocation.Y);
            Vector2 velocity = new Vector2(
                0f,
                .25f);
            float angle = 0f;
            float angularVelocity = 0f;
            Color color = this.Color;
            float size = .5f;
            int ttl = 200 + Game1.Utility.RGenerator.Next(40);

            return new FireParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl, this.LayerDepth);
        }

        public void Activate(float activationTime, Vector2 emitterLocation, Color color, float layerDepth)
        {
            this.ActivationTime = activationTime;
            this.EmitterLocation = emitterLocation;
            this.Color = color;
            this.LayerDepth = layerDepth;
        }

        public void ClearParticles()
        {
            particles.Clear();
        }


        public void UpdateWeather(GameTime gameTime)
        {

            this.EmitterLocation = new Vector2(Game1.cam.CameraScreenRectangle.X, Game1.cam.CameraScreenRectangle.Y);
            int total = 1;
            this.AddNewParticleTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.AddNewParticleTimer <= 0)
            {
                for (int i = 0; i < total; i++)
                {
                    particles.Add(GenerateNewWeatherParticle());
                }
                this.AddNewParticleTimer = Game1.Utility.RFloat(.005f, .01f);
            }


            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update(gameTime);
                if (particles[particle].TTL <= 40)
                {
                    particles[particle].ColorMultiplier -= .1f;
                }
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void UpdateFire(GameTime gameTime)
        {
            int total = 1;
            this.AddNewParticleTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.AddNewParticleTimer <= 0)
            {
                for (int i = 0; i < total; i++)
                {
                    particles.Add(GenerateNewFireParticle());
                }
                this.AddNewParticleTimer = Game1.Utility.RFloat(.05f, .1f);
            }


            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update(gameTime);
                if (particles[particle].TTL <= 100)
                {
                    particles[particle].ColorMultiplier -= .025f;
                }
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void UpdateSmoke(GameTime gameTime)
        {
            int total = 1;
            this.AddNewParticleTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.AddNewParticleTimer <= 0)
            {
                for (int i = 0; i < total; i++)
                {
                    particles.Add(GenerateNewSmokeParticle());
                }
                this.AddNewParticleTimer = Game1.Utility.RFloat(.1f, .5f);
            }


            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update(gameTime);
                if (particles[particle].TTL <= 100)
                {
                    particles[particle].ColorMultiplier -= .025f;
                }
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (this.ActivationTime > 0)
            {

                int total = 1;
                this.AddNewParticleTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (this.AddNewParticleTimer <= 0)
                {
                    for (int i = 0; i < total; i++)
                    {
                        particles.Add(GenerateNewParticle());
                    }
                    this.AddNewParticleTimer = Game1.Utility.RFloat(.01f, .2f);
                }
                this.ActivationTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update(gameTime);
                if (particles[particle].TTL <= 40)
                {
                    particles[particle].ColorMultiplier -= .1f;
                }
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            if (particles.Count > 0)
            {
                for (int index = 0; index < particles.Count; index++)
                {
                    particles[index].Draw(spriteBatch);
                }
            }

        }
    }
}
