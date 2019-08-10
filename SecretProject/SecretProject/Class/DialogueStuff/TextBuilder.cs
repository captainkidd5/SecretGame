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

        public string[] Words { get; set; }
        public int CurrentWordIndex { get; set; } = 0;

        public float LineWidth { get; set; } = 0;

        String parsedText;
        String typedText;
        double typedTextLength;
        int delayInMilliseconds;
        bool isDoneDrawing;

        

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


            this.TextBoxLocation = new Vector2(PositionToWriteTo.X, PositionToWriteTo.Y);
            SpeechBox = new TextBox(TextBoxLocation, 1);
            SpeechBox.position = new Vector2(PositionToWriteTo.X, PositionToWriteTo.Y);
            typedText = "";
            parsedText = parseText(stringToWrite);
            delayInMilliseconds = 50;
            isDoneDrawing = false;
        }

        public void ChangedParsedText()
        {
            this.parsedText = parseText(this.StringToWrite);
        }

        public void Update(GameTime gameTime)
        {
            if (IsActive)
            {

                if(!isDoneDrawing)
                {
                    if(delayInMilliseconds == 0)
                    {
                        typedText = parsedText;
                        isDoneDrawing = true;
                    }
                    else if(typedTextLength < parsedText.Length)
                    {
                        typedTextLength = typedTextLength + gameTime.ElapsedGameTime.TotalMilliseconds / delayInMilliseconds;
                        if(typedTextLength >= parsedText.Length)
                        {
                            typedTextLength = parsedText.Length;
                            isDoneDrawing = true;
                        }

                        typedText = parsedText.Substring(0, (int)typedTextLength);
                    }
                }
               /* this.Words = StringToWrite.Split(' ');
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
                    WriteSpeed = SpeedAnchor; ///////////////

                }
                if(WriteSpeed < 0 && currentTextIndex >= Words[CurrentWordIndex].Length - 1)
                {
                    outputString  += " ";
                    currentTextIndex = 0;
                    CurrentWordIndex++;
                    WriteSpeed = SpeedAnchor;
                }
                */


            }
        }

        private String parseText(String text)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (Game1.AllTextures.MenuText.MeasureString(line + word).Length() > SpeechBox.DestinationRectangle.Width)
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
        }
        public TextBox SpeechBox { get; set; }
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (IsActive)
            {


                //spriteBatch.DrawString(Game1.AllTextures.MenuText, outputString, this.PositionToWriteTo, this.Color, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, layerDepth);
                //spriteBatch.Draw(Game1.AllTextures.redPixel, SpeechBox.DestinationRectangle, Color.White);
                
                if(UseTextBox)
                {
                    
                    
                    switch (TextBoxType)
                    {
                        
                        case TextBoxType.normal:
                            SpeechBox = new TextBox(TextBoxLocation, 0);
                            SpeechBox.position = new Vector2(PositionToWriteTo.X, PositionToWriteTo.Y);
                           // SpeechBox.DrawWithoutString(spriteBatch);
                            break;
                        case TextBoxType.dialogue:
                            SpeechBox = new TextBox(TextBoxLocation, 1);
                            SpeechBox.position = new Vector2(PositionToWriteTo.X, PositionToWriteTo.Y);
                           // SpeechBox.DrawWithoutString(spriteBatch);
                            break;

                    }

                    spriteBatch.Draw(Game1.AllTextures.redPixel, SpeechBox.DestinationRectangle, Color.White);

                    spriteBatch.DrawString(Game1.AllTextures.MenuText, typedText, new Vector2(SpeechBox.DestinationRectangle.X, SpeechBox.DestinationRectangle.Y), this.Color);

                }
                
            }
        }
    }
}
