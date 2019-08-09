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
    public enum TextBoxType
    {
        normal = 0,
        dialogue = 1
    }

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
        public Vector2 TextBoxLocation { get; set; }
        public bool UseTextBox { get; set; } = false;
        public bool FreezeStage { get; set; } = false;
        public float Scale { get; set; }
        public Color Color { get; set; }
        public int NumberOfClicks { get; set; }
        public TextBoxType TextBoxType { get; set; }
        public int WrapIndex { get; set; }

        public string[] Words { get; set; }
        public int CurrentWordIndex { get; set; } = 0;

        public float LineWidth { get; set; } = 0;

        public TextBuilder(string stringToWrite, float writeSpeed, float stringDisplayTimer)
        {
            this.StringToWrite = stringToWrite;
            this.Words = stringToWrite.Split(' ');
            this.WriteSpeed = writeSpeed;
            this.StringDisplayTimer = stringDisplayTimer;
            this.StringDisplayAnchor = this.StringDisplayTimer;
            this.SpeedAnchor = WriteSpeed;
            PositionToWriteTo = Game1.Utility.DialogueTextLocation;
            this.Scale = 1f;
            this.Color = Color.Black;
            this.NumberOfClicks = 0;
            this.WrapIndex = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                this.Words = StringToWrite.Split(' ');
                if (NumberOfClicks == 1)
                {
                    this.outputString = StringToWrite;
                    StringDisplayTimer = 10f;
                    currentTextIndex = StringToWrite.Length;
                    
                }
                if(NumberOfClicks == 2)
                {
                    StringDisplayTimer = 0;
                    NumberOfClicks = 0;
                    Game1.freeze = false;
                }
                if (Game1.myMouseManager.IsClicked)
                {
                    NumberOfClicks++;
                }
                if(FreezeStage)
                {
                    Game1.freeze = true;
                }

                WriteSpeed -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (currentTextIndex == StringToWrite.Length)
                {
                    this.StringDisplayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (StringDisplayTimer < 0)
                {
                    Reset();
                    this.IsActive = false;
                    this.UseTextBox = false;
                    currentTextIndex = 0;
                    Game1.freeze = false;
                    this.StringDisplayTimer = this.StringDisplayAnchor;
                }

                if (WriteSpeed < 0 && currentTextIndex < Words[CurrentWordIndex].Length && CurrentWordIndex < Words.Length - 1)
                {
                    outputString += Words[CurrentWordIndex][currentTextIndex];
                    float spaceWidth = Game1.AllTextures.MenuText.MeasureString(Words[CurrentWordIndex][currentTextIndex].ToString()).X;
                    currentTextIndex++;
                    WrapIndex++;
                    WriteSpeed = SpeedAnchor; ///////////////

                }
                if(WriteSpeed < 0 && currentTextIndex >= Words[CurrentWordIndex].Length)
                {
                    outputString  += " ";
                    currentTextIndex = 0;
                    CurrentWordIndex++;
                    WrapIndex++;
                    WriteSpeed = SpeedAnchor;
                }


            }
        }

        private String ParseText()
        {
            String line = String.Empty;
            String returnString = String.Empty;
            foreach(String word in Words)
            {
                if(Game1.AllTextures.MenuText.MeasureString(line + word).Length() > 500)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                    
                }
                line = line + word + ' ';
            }
            return returnString + line;
        }


        public void Reset()
        {
            this.outputString = "";
            this.StringToWrite = "";
            this.currentTextIndex = 0;
            this.CurrentWordIndex = 0;
            this.IsActive = false;
            WrapIndex = 0;
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (IsActive)
            {


                    //spriteBatch.DrawString(Game1.AllTextures.MenuText, outputString, this.PositionToWriteTo, this.Color, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, layerDepth);

                spriteBatch.DrawString(Game1.AllTextures.MenuText, outputString, this.PositionToWriteTo, this.Color, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, layerDepth);
                if(UseTextBox)
                {
                    TextBox speechBox;
                    this.TextBoxLocation = new Vector2(PositionToWriteTo.X - 50, PositionToWriteTo.Y - 50);
                    switch (TextBoxType)
                    {
                        
                        case TextBoxType.normal:
                            speechBox = new TextBox(TextBoxLocation, 0);
                            speechBox.position = new Vector2(PositionToWriteTo.X, PositionToWriteTo.Y);
                            speechBox.DrawWithoutString(spriteBatch);
                            break;
                        case TextBoxType.dialogue:
                            speechBox = new TextBox(TextBoxLocation, 1);
                            speechBox.position = new Vector2(PositionToWriteTo.X, PositionToWriteTo.Y);
                            speechBox.DrawWithoutString(spriteBatch);
                            break;

                    }

                    
                }
                
            }
        }
    }
}
