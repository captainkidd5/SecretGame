using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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


        public Particle(Texture2D particleTexture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl, float layerDepth = 1f, float sizeMin = .25f, float sizeMax = 1f)
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

            this.Size = Game1.Utility.RFloat(sizeMin, sizeMax);


            this.VelocityReductionTimer = Game1.Utility.RFloat(.25f, .5f);

        }


        public virtual void Update(GameTime gameTime)
        {
            this.VelocityReductionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (this.VelocityReductionTimer < 0)
            {

                this.Velocity = new Vector2(this.Velocity.X, this.Velocity.Y + .1f);
            }
            this.TTL--;

            this.Position += this.Velocity;
            if (this.Position.Y > this.BaseY + 10)
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y - this.Velocity.Y);
            }
            //Position = new Vector2(Position.X, Position.Y )
            this.Angle += this.AngularVelocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, this.ParticleTexture.Width, this.ParticleTexture.Height);
            Vector2 origin = new Vector2(this.ParticleTexture.Width / 2, this.ParticleTexture.Height / 2);


            spriteBatch.Draw(this.ParticleTexture, this.Position, sourceRectangle, this.Color * this.ColorMultiplier,
                            this.Angle, origin, this.Size, SpriteEffects.None, this.LayerDepth);


        }
    }
}
