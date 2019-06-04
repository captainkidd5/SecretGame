using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.DialogueStuff
{
    public class TextBuilder
    {
        public string StringToWrite { get; set; }
        public bool IsActive { get; set; } = false;
        public float WriteSpeed { get; set; }
        private string outputString = "";
        int currentTextIndex = 0;

        public TextBuilder(string stringToWrite, float writeSpeed)
        {
            this.StringToWrite = stringToWrite;
            this.WriteSpeed = writeSpeed;
        }

        public void Update(GameTime gameTime)
        {
            WriteSpeed -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(WriteSpeed < 0)
            {
                outputString += StringToWrite[currentTextIndex];
                currentTextIndex++;
                if(currentTextIndex == StringToWrite.Length)
                {
                    this.IsActive = false;
                    currentTextIndex = 0;
                    outputString = "";
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, outputString, Game1.Utility.centerScreen, Color.White);
        }
    }
}
