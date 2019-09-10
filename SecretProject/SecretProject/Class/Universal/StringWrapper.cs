using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public StringWrapper(string message, Vector2 position, float endAtX, float endAtY, float rate,float duration)
        {
            this.Message = message;
            this.Position = position;
            this.EndAtX = endAtX;
            this.EndAtY = endAtY;
            this.Rate = rate;
            this.Duration = duration;
            this.ColorOpacity = 1f;

        }

        public void Update(GameTime gameTime, List<StringWrapper> strings)
        {
            Duration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            ColorOpacity -= .01f;
            if(Duration<= 0)
            {
                strings.Remove(this);
            }
            if(this.Position.X < EndAtX)
            {
                this.Position.X += (float)(gameTime.ElapsedGameTime.TotalSeconds * Rate);
            }
            else if(this.Position.X > EndAtX)
            {
                this.Position.X -= (float)(gameTime.ElapsedGameTime.TotalSeconds * Rate);
            }

            if (this.Position.Y < EndAtY)
            {
                this.Position.Y += (float)(gameTime.ElapsedGameTime.TotalSeconds * Rate);
            }
            else if (this.Position.Y > EndAtY)
            {
                this.Position.Y -= (float)(gameTime.ElapsedGameTime.TotalSeconds * Rate);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, Message, this.Position, Color.White * this.ColorOpacity, 0f, Game1.Utility.Origin, .25f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .0001f);
        }
    }
}
