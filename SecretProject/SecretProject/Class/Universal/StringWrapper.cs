using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SecretProject.Class.Universal
{
    public class StringWrapper
    {
        public string Message { get; set; }
        public Vector2 Position;
        public float EndAtX { get; set; }
        public float EndAtY { get; set; }
        public float Rate { get; set; }
        public float Duration { get; set; }
        public float ColorOpacity { get; set; }

        public StringWrapper(string message, Vector2 position, float endAtX, float endAtY, float rate, float duration)
        {
            this.Message = message;
            Position = position;
            this.EndAtX = endAtX;
            this.EndAtY = endAtY;
            this.Rate = rate;
            this.Duration = duration;
            this.ColorOpacity = 1f;

        }

        public void Update(GameTime gameTime, List<StringWrapper> strings)
        {
            this.Duration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.ColorOpacity -= .01f;
            if (this.Duration <= 0)
            {
                strings.Remove(this);
            }
            if (Position.X < this.EndAtX)
            {
                Position.X += (float)(gameTime.ElapsedGameTime.TotalSeconds * this.Rate);
            }
            else if (Position.X > this.EndAtX)
            {
                Position.X -= (float)(gameTime.ElapsedGameTime.TotalSeconds * this.Rate);
            }

            if (Position.Y < this.EndAtY)
            {
                Position.Y += (float)(gameTime.ElapsedGameTime.TotalSeconds * this.Rate);
            }
            else if (Position.Y > this.EndAtY)
            {
                Position.Y -= (float)(gameTime.ElapsedGameTime.TotalSeconds * this.Rate);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Message, Position, Color.White * this.ColorOpacity, 0f, Game1.Utility.Origin, .25f, SpriteEffects.None,Utility.StandardButtonDepth + .0001f);
        }
    }
}
