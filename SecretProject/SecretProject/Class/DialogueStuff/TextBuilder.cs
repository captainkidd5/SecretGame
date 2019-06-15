using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.UI;
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
        public float StringDisplayTimer { get; set; }
        public float StringDisplayAnchor { get; set; }
        public float SpeedAnchor { get; set; }
        public Vector2 PositionToWriteTo { get; set; }
        public bool UseTextBox { get; set; } = false;

        public TextBuilder(string stringToWrite, float writeSpeed, float stringDisplayTimer)
        {
            this.StringToWrite = stringToWrite;
            this.WriteSpeed = writeSpeed;
            this.StringDisplayTimer = stringDisplayTimer;
            this.StringDisplayAnchor = this.StringDisplayTimer;
            this.SpeedAnchor = WriteSpeed;
            PositionToWriteTo = Game1.Utility.centerScreen;
        }

        public void Update(GameTime gameTime)
        {
            if (IsActive)
            {


                WriteSpeed -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (currentTextIndex == StringToWrite.Length)
                {
                    this.StringDisplayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (StringDisplayTimer < 0)
                {
                    outputString = "";
                    this.IsActive = false;
                    this.UseTextBox = false;
                    currentTextIndex = 0;
                    this.StringDisplayTimer = this.StringDisplayAnchor;
                }

                if (WriteSpeed < 0 && currentTextIndex < StringToWrite.Length)
                {
                    outputString += StringToWrite[currentTextIndex];
                    currentTextIndex++;
                    WriteSpeed = SpeedAnchor; ///////////////

                }


            }
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (IsActive)
            {


                spriteBatch.DrawString(Game1.AllTextures.MenuText, outputString, this.PositionToWriteTo, Color.White, 0f, Game1.Utility.Origin, .5f, SpriteEffects.None, layerDepth);
                if(UseTextBox)
                {
                    TextBox speechBox = new TextBox(PositionToWriteTo, 1);
                    speechBox.position = new Vector2(PositionToWriteTo.X - speechBox.SourceRectangle.Width / 2, PositionToWriteTo.Y - speechBox.SourceRectangle.Width / 2);
                    speechBox.DrawWithoutString(spriteBatch);
                }
                
            }
        }
    }
}
