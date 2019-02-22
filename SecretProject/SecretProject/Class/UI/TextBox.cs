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
        protected Vector2 textBoxLocation;
        protected string TextToWrite { get; set; }
        protected Texture2D backDrop;

        public KeyboardState oldKeys = Keyboard.GetState();

        public TextBox(SpriteFont textFont, Vector2 textBoxLocation, string textToWrite, Texture2D backDrop)
        {
            this.textFont = textFont;
            this.textBoxLocation = textBoxLocation;
            this.TextToWrite = textToWrite;
            this.backDrop = backDrop;
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

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if(IsActivated)
            {
                spriteBatch.Draw(backDrop, textBoxLocation, Color.White);
                spriteBatch.DrawString(textFont, TextToWrite, textBoxLocation, Color.Red);
            }
            spriteBatch.End();

        }
    }
}
