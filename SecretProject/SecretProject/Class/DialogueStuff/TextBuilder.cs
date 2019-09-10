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
        dialogue = 1,
        none = 2
    }

    public class TextBuilder
    {
        public string StringToWrite { get; set; }
        public bool IsActive { get; set; } = false;
        public float WriteSpeed { get; set; }
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
        String parsedText;
        String typedText;
        double typedTextLength;
        bool isDoneDrawing;

        public float LineLimit { get; set; }

        public TextBox SpeechBox { get; set; }
        public float startDisplay { get; set; }

        public TextBuilder(string stringToWrite, float writeSpeed, float stringDisplayTimer)
        {
            this.StringToWrite = stringToWrite;
            this.WriteSpeed = writeSpeed;
            this.SpeedAnchor = writeSpeed;
            this.StringDisplayTimer = stringDisplayTimer;
            this.StringDisplayAnchor = this.StringDisplayTimer;
            PositionToWriteTo = Game1.Utility.DialogueTextLocation;
            this.Scale = 2f;
            this.Color = Color.Black;
            this.NumberOfClicks = 0;


            this.TextBoxLocation = new Vector2(PositionToWriteTo.X, PositionToWriteTo.Y);
            SpeechBox = new TextBox(TextBoxLocation, 1);
            SpeechBox.position = new Vector2(PositionToWriteTo.X, PositionToWriteTo.Y);
            this.LineLimit = SpeechBox.DestinationRectangle.Width - 100;
            typedText = "";
            parsedText = parseText(stringToWrite);
            isDoneDrawing = false;
            
        }

        public void ChangedParsedText()
        {
            this.parsedText = parseText(this.StringToWrite);
        }

        public void Activate(bool useTextBox, TextBoxType textBoxType, bool freezeStage, string stringToWrite, float scale, Vector2? positionToWriteTo, float? lineLimit)
        {
            this.IsActive = true;
            this.UseTextBox = useTextBox;
            this.TextBoxType = textBoxType;
            this.FreezeStage = freezeStage;
            this.StringToWrite = stringToWrite;
            this.Scale = scale;
            
            
            ChangedParsedText();
           
            if(this.UseTextBox)
            {
                this.PositionToWriteTo = new Vector2(SpeechBox.DestinationRectangle.X, SpeechBox.DestinationRectangle.Y);
                this.LineLimit = SpeechBox.DestinationRectangle.Width - 100;
            }
            else
            {
                this.PositionToWriteTo = (Vector2)positionToWriteTo;
                if (lineLimit != null)
                {
                    this.LineLimit = (float)lineLimit;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                if(UseTextBox)
                {
                    Game1.Player.UserInterface.BottomBar.IsActive = false;
                    
                }
                
                if (NumberOfClicks == 1)
                {
                    //StringDisplayTimer = 10f;
                    this.WriteSpeed = .1f;

                }
                if (NumberOfClicks == 2)
                {
                    Reset();
                }
                if (Game1.myMouseManager.IsClicked && StringDisplayTimer <= StringDisplayAnchor -1 && !Game1.myMouseManager.MouseRectangle.Intersects(this.SpeechBox.DestinationRectangle))
                {
                    NumberOfClicks++;
                }
                if (FreezeStage)
                {
                    Game1.freeze = true;
                }


                if (StringDisplayTimer <= 0)
                {
                    Reset();

                }
                if (!isDoneDrawing)
                {
                    if (WriteSpeed == 0)
                    {
                        typedText = parsedText;
                        isDoneDrawing = true;
                    }
                    else if (typedTextLength < parsedText.Length)
                    {
                        typedTextLength = typedTextLength + gameTime.ElapsedGameTime.TotalMilliseconds / WriteSpeed;
                        int noiseRand = Game1.Utility.RGenerator.Next(1, 3);
                        if(noiseRand == 1)
                        {
                            
                            Game1.SoundManager.TextNoise.Play(.1f, 0f, 0f);
                        }
                        else
                        {
                            Game1.SoundManager.TextNoise2.Play(.1f, 0f, 0f);
                        }
                        
                        if (typedTextLength >= parsedText.Length)
                        {
                            typedTextLength = parsedText.Length;
                            isDoneDrawing = true;
                        }

                        typedText = parsedText.Substring(0, (int)typedTextLength);
                    }
                }

                StringDisplayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            }
        }

        private String parseText(String text)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (Game1.AllTextures.MenuText.MeasureString(line + word).Length() * Scale > LineLimit)
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
            this.StringDisplayTimer = StringDisplayAnchor;
            this.StringToWrite = "";
            this.IsActive = false;
            this.UseTextBox = false;
            this.typedText = "";
            Game1.freeze = false;
            this.StringDisplayTimer = this.StringDisplayAnchor;
            this.NumberOfClicks = 0;
            this.WriteSpeed = SpeedAnchor;
            this.isDoneDrawing = false;
            this.parsedText = "";
            Game1.Player.UserInterface.BottomBar.IsActive = true;
            this.typedTextLength = 0;
        }
        
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (IsActive)
            {
                if (UseTextBox)
                {
                    switch (TextBoxType)
                    {

                        case TextBoxType.normal:
                            SpeechBox = new TextBox(TextBoxLocation, 0);
                            SpeechBox.position = new Vector2(PositionToWriteTo.X , PositionToWriteTo.Y);
                            SpeechBox.DrawWithoutString(spriteBatch);
                            break;
                        case TextBoxType.dialogue:
                            SpeechBox = new TextBox(TextBoxLocation, 1);
                            SpeechBox.position = new Vector2(PositionToWriteTo.X- 50, PositionToWriteTo.Y- 50);
                            SpeechBox.DrawWithoutString(spriteBatch);
                            break;

                    }
                }
                spriteBatch.DrawString(Game1.AllTextures.MenuText, typedText, this.PositionToWriteTo, this.Color, 0f, Game1.Utility.Origin, Scale, SpriteEffects.None, 1f);

            }
        }


        
    }
}
