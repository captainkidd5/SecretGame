using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SecretProject.Class.UI
{
    public class TextBox
    {
       public bool IsActivated { get; set; } = false;
        protected SpriteFont textFont;
        protected Vector2 position;
        public string TextToWrite { get; set; }
        protected Texture2D Texture;

        public KeyboardState oldKeys = Keyboard.GetState();

        public TextBox(SpriteFont textFont, Vector2 position, string textToWrite, Texture2D texture)
        {
            this.textFont = textFont;
            this.position = position;
            this.TextToWrite = textToWrite;
            this.Texture = texture;
        }

        public virtual void Update(GameTime gameTime)
        {
            KeyboardState currentKeys = Keyboard.GetState();

            if(oldKeys.IsKeyDown(Keys.F1) && !currentKeys.IsKeyDown(Keys.F1))
            {
                IsActivated = !IsActivated;
            }


            oldKeys = currentKeys;
        }

        public virtual void Update(GameTime gameTime, bool stayActivated)
        {
            IsActivated = stayActivated;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if(IsActivated)
            {
               // spriteBatch.Draw(Texture, position, Color.White);
                spriteBatch.Draw(this.Texture, new Rectangle((int)this.position.X, (int)this.position.Y, 128, 64), new Rectangle(48, 176, 128, 64), Color.White, 0f,
                    Game1.Utility.Origin, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                spriteBatch.DrawString(textFont, TextToWrite, position, Color.Red, 0f, Game1.Utility.Origin,1f, SpriteEffects.None, Game1.Utility.StandardTextDepth);
            }
            spriteBatch.End();

        }
    }
}
