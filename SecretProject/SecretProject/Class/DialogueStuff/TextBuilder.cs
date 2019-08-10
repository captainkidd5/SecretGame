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
            typedText = "";
            parsedText = parseText(stringToWrite);
            isDoneDrawing = false;
        }

        public void ChangedParsedText()
        {
            this.parsedText = parseText(this.StringToWrite);
        }

        public void Activate(bool useTextBox, TextBoxType textBoxType, bool freezeStage, string stringToWrite)
        {
            this.IsActive = true;
            this.UseTextBox = useTextBox;
            this.TextBoxType = textBoxType;
            this.FreezeStage = freezeStage;
            this.StringToWrite = stringToWrite;
            ChangedParsedText();
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
                    StringDisplayTimer = 10f;
                    this.WriteSpeed = .1f;

                }
                if (NumberOfClicks == 2)
                {
                    Reset();
                }
                if (Game1.myMouseManager.IsClicked)
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
                if (Game1.AllTextures.MenuText.MeasureString(line + word).Length() * Scale > SpeechBox.DestinationRectangle.Width - 50)
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
            this.StringDisplayTimer = 10f;
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
        public TextBox SpeechBox { get; set; }
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
                            SpeechBox.position = new Vector2(PositionToWriteTo.X, PositionToWriteTo.Y);
                            SpeechBox.DrawWithoutString(spriteBatch);
                            break;
                        case TextBoxType.dialogue:
                            SpeechBox = new TextBox(TextBoxLocation, 1);
                            SpeechBox.position = new Vector2(PositionToWriteTo.X, PositionToWriteTo.Y);
                            SpeechBox.DrawWithoutString(spriteBatch);
                            break;

                    }

                    

                }
                spriteBatch.DrawString(Game1.AllTextures.MenuText, typedText, new Vector2(SpeechBox.DestinationRectangle.X + 50, SpeechBox.DestinationRectangle.Y + 50), this.Color, 0f, Game1.Utility.Origin, Scale, SpriteEffects.None, 1f);

            }
        }
    }
}
