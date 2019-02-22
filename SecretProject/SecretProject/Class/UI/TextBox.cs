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
        SpriteFont textFont;
        Vector2 textBoxLocation;
        string TextToWrite { get; set; }
        Texture2D backDrop;

        public TextBox(SpriteFont textFont, Vector2 textBoxLocation, string textToWrite, Texture2D backDrop)
        {
            this.textFont = textFont;
            this.textBoxLocation = textBoxLocation;
            this.TextToWrite = textToWrite;
            this.backDrop = backDrop;
        }

        public void Draw(SpriteBatch spriteBatch)
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
