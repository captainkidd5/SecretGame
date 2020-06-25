using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.UI;
using SecretProject.Class.UI.ButtonStuff;
using System;
using System.Collections.Generic;
using XMLData.DialogueStuff;

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

        List<SelectableOption> SelectableOptions;
        public float LineLimit { get; set; }

        public TextBox SpeechBox { get; set; }
        public float startDisplay { get; set; }


        public bool IsPaused { get; set; }
        public bool HaveOptionsBeenChecked { get; set; }
        public bool AreSelectableOptionsActivated { get; set; }
        public bool MoveToSelectableOptions { get; set; }

        public DialogueSkeleton Skeleton { get; set; }

        public Character CharacterTalking { get; set; }
        public string SpeakerName { get; set; }
        public int SpeakerID { get; set; }


        public Texture2D SpeakerTexture { get; set; }
        public Rectangle SpeakerPortraitSourceRectangle { get; set; }
        public TextBuilder(string stringToWrite, float writeSpeed, float stringDisplayTimer)
        {
            this.StringToWrite = stringToWrite;
            this.WriteSpeed = writeSpeed;
            this.SpeedAnchor = writeSpeed;
            this.PositionToWriteTo = Game1.Utility.DialogueTextLocation;
            this.Scale = 2f;
            this.Color = Color.Black;
            this.NumberOfClicks = 0;


            this.TextBoxLocation = new Vector2(this.PositionToWriteTo.X, this.PositionToWriteTo.Y);
            this.SpeechBox = new TextBox(this.TextBoxLocation, 1);
            this.SpeechBox.position = new Vector2(this.PositionToWriteTo.X, this.PositionToWriteTo.Y);
            this.LineLimit = this.SpeechBox.DestinationRectangle.Width - 100;
            typedText = "";
            parsedText = ParseText(stringToWrite, this.LineLimit, this.Scale);
            isDoneDrawing = false;
            this.IsPaused = false;
            SelectableOptions = new List<SelectableOption>();
            this.MoveToSelectableOptions = false;
            this.HaveOptionsBeenChecked = false;
            this.AreSelectableOptionsActivated = false;

        }

        public void ChangedParsedText()
        {
            parsedText = ParseText(this.StringToWrite, this.LineLimit, this.Scale);
        }

        public void Activate(bool useTextBox, TextBoxType textBoxType, bool freezeStage, string stringToWrite, float scale, Vector2? positionToWriteTo, float? lineLimit)
        {

            this.IsActive = true;
            this.UseTextBox = useTextBox;
            this.TextBoxType = textBoxType;
            this.FreezeStage = freezeStage;
            this.StringToWrite = stringToWrite;
            this.Scale = scale;
            this.SpeedAnchor = .1f;


            ChangedParsedText();

            if (this.UseTextBox)
            {
                this.PositionToWriteTo = new Vector2(this.SpeechBox.DestinationRectangle.X, this.SpeechBox.DestinationRectangle.Y);
                this.LineLimit = this.SpeechBox.DestinationRectangle.Width - 100;
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

        public void ActivateCharacter(Character character,TextBoxType textBoxType, bool freezeStage, string stringToWrite, float scale)
        {
            this.CharacterTalking = character;
            this.SpeakerTexture = character.CharacterPortraitTexture;
            this.SpeakerPortraitSourceRectangle = character.CharacterPortraitSourceRectangle;
            this.SpeakerID = character.SpeakerID;
            this.SpeakerName = character.Name;
            this.IsActive = true;
            this.TextBoxType = textBoxType;
            this.FreezeStage = freezeStage;
            this.StringToWrite = stringToWrite;
            this.Scale = scale;
            this.SpeedAnchor = .1f;
            this.UseTextBox = true;


            ChangedParsedText();

            if (this.UseTextBox)
            {
                this.PositionToWriteTo = new Vector2(this.SpeechBox.DestinationRectangle.X, this.SpeechBox.DestinationRectangle.Y);
                this.LineLimit = this.SpeechBox.DestinationRectangle.Width - 100;
            }


        }

        public void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {
                if (Game1.KeyboardManager.WasKeyPressed(Keys.Escape))
                {
                    Reset();
                    this.IsActive = false;
                }
                if (this.UseTextBox)
                {
                    Game1.Player.UserInterface.BottomBar.IsActive = false;

                }
                if (this.AreSelectableOptionsActivated)
                {
                    Game1.freeze = true;
                    ClearWindowForResponse();
                    foreach (SelectableOption option in SelectableOptions)
                    {
                        option.Update(gameTime, this.CharacterTalking);
                    }
                }

                else
                {

                    if (this.IsPaused && this.NumberOfClicks == 0 && Game1.MouseManager.IsHovering(this.SpeechBox.DestinationRectangle))
                    {
                        Game1.MouseManager.ChangeMouseTexture(CursorType.NextChatWindow);
                    }


                    if (this.NumberOfClicks >= 1)
                    {
                        this.SpeedAnchor = .1f;
                        if (this.IsPaused)
                        {
                            if (this.MoveToSelectableOptions && this.Skeleton != null)
                            {
                                if (this.Skeleton.SelectableOptions != null)
                                {
                                    if (!this.HaveOptionsBeenChecked)
                                    {
                                        CheckSelectableOptions(this.Skeleton);
                                        this.HaveOptionsBeenChecked = true;

                                    }

                                }

                            }
                            else
                            {
                                MoveTextToNewWindow();
                                this.IsPaused = false;
                                this.HaveOptionsBeenChecked = false;
                            }


                        }
                    }
                    if (this.NumberOfClicks == 2)
                    {
                        if (this.Skeleton != null)
                        {

                        }
                        else
                        {
                            Reset();
                        }

                    }
                    if (Game1.MouseManager.IsClicked)
                    {
                        this.NumberOfClicks++;
                    }
                    if (this.FreezeStage && this.NumberOfClicks < 2)
                    {
                        Game1.freeze = true;

                    }
                    else
                    {
                        Reset();
                    }


                    if (!isDoneDrawing && !this.IsPaused)
                    {
                        if (this.WriteSpeed == 0)
                        {
                            typedText = parsedText;
                            isDoneDrawing = true;
                        }
                        else if (typedTextLength < parsedText.Length - 1)
                        {
                            HandleSpecialCase();


                            this.SpeedAnchor += (float)(gameTime.ElapsedGameTime.TotalMilliseconds / this.WriteSpeed);
                            if (this.SpeedAnchor > 2f)
                            {
                                typedTextLength++;
                                PlayTextNoise();
                                this.SpeedAnchor = 0f;
                            }

                            typedText = parsedText.Substring(0, (int)typedTextLength);
                        }
                    }

                }
            }
        }

        public void HandleSpecialCase()
        {
            if (parsedText[(int)typedTextLength + 1] == '#')
            {
                parsedText.Remove((int)typedTextLength, 1);
                //typedTextLength--;
                PauseUntilInput();

            }
            if (parsedText[(int)typedTextLength + 1] == '`')
            {
                parsedText.Remove((int)typedTextLength, 1);
                //typedTextLength--;
                PauseUntilInput();

                this.MoveToSelectableOptions = true;

            }

            if (parsedText[(int)typedTextLength + 1] == '%')
            {
                parsedText.Remove((int)typedTextLength, 1);
                //typedTextLength--;
                string speed = parsedText[(int)typedTextLength + 2].ToString() + parsedText[(int)typedTextLength + 3].ToString() + parsedText[(int)typedTextLength + 4].ToString();
                this.WriteSpeed = int.Parse(speed);
                parsedText = parsedText.Remove((int)typedTextLength + 1, 4);

            }

            if (parsedText[(int)typedTextLength + 1] == '$')
            {
                parsedText = parsedText.Remove((int)typedTextLength + 1, 1);
                //typedTextLength--;

                parsedText = parsedText.Insert((int)typedTextLength + 1, Game1.Player.Name);

            }
        }

        public static String ParseText(String text, float lineLimit, float scale)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (Game1.AllTextures.MenuText.MeasureString(line + word).Length() * scale > lineLimit)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }

        /// <summary>
        /// Returns the width of the longest text line, separated at the new line.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textScale"></param>
        /// <returns></returns>
        public static float GetTextLength(string text, float textScale)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] lineArray = text.Split('\n');
            float lengthToReturn = 0f;
            for(int i =0; i < lineArray.Length; i++)
            {
                float length = Game1.AllTextures.MenuText.MeasureString(lineArray[i]).X * textScale;
                if(length > lengthToReturn)
                {
                    lengthToReturn = length;
                }
            }

            return lengthToReturn;



        }

        public static float GetTextHeight(string text, float textScale)
        {
 
            String returnString = String.Empty;
            String[] lineArray = text.Split('\n');

            float totalHeight = 0;

            foreach(string line in lineArray)
            {
                totalHeight += Game1.AllTextures.MenuText.MeasureString(line).Y * textScale;
            }

            return totalHeight;
        }

        public void Reset(bool unfreeze = true)
        {
            this.StringToWrite = "";
            this.IsActive = false;
            this.UseTextBox = false;
            typedText = "";
            if (unfreeze)
            {
                this.FreezeStage = false;

            }
            else
            {
                this.FreezeStage = true;
            }

            this.NumberOfClicks = 0;
            this.WriteSpeed = .5f;
            isDoneDrawing = false;
            parsedText = "";
            //Game1.Player.UserInterface.BottomBar.IsActive = true;
            typedTextLength = 0;
            this.IsPaused = false;
            this.Skeleton = null;
            SelectableOptions = new List<SelectableOption>();
            this.HaveOptionsBeenChecked = false;
            this.AreSelectableOptionsActivated = false;
            this.MoveToSelectableOptions = false;
            if(this.CharacterTalking != null)
            {
                this.CharacterTalking.IsBeingSpokenTo = false;
            }
            
            this.SpeakerName = null;
            this.SpeakerID = -1;


        }


        public void MoveTextToNewWindow()
        {
            typedText = "";
            this.NumberOfClicks = 0;
            typedTextLength = 0;
            this.StringToWrite = this.StringToWrite.Split(new[] { '#' }, 2)[1];
            ChangedParsedText();

        }

        public void ClearWindowForResponse()
        {
            typedText = "";
            this.NumberOfClicks = 0;
            typedTextLength = 0;
        }

        public void PauseUntilInput()
        {
            this.NumberOfClicks = 0;
            this.IsPaused = true;


        }

        public void CheckSelectableOptions(DialogueSkeleton skeleton)
        {
            string[] options = skeleton.SelectableOptions.Split(',');
            for (int s = 0; s < options.Length; s++)
            {
                string response = options[s].Split('~')[0];
                string action = options[s].Split('~')[1];
                SelectableOptions.Add(new SelectableOption(response, action, new Vector2(this.PositionToWriteTo.X, this.PositionToWriteTo.Y + 64 * s)));
            }
            if (options.Length > 0)
            {
                this.AreSelectableOptionsActivated = true;
            }

            //Reset();


        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (this.IsActive)
            {
                if (this.UseTextBox)
                {
                    switch (this.TextBoxType)
                    {

                        case TextBoxType.normal:
                            this.SpeechBox = new TextBox(this.TextBoxLocation, 0);
                            this.SpeechBox.position = new Vector2(this.PositionToWriteTo.X, this.PositionToWriteTo.Y);
                            this.SpeechBox.Draw(spriteBatch, false);
                            break;
                        case TextBoxType.dialogue:
                            this.SpeechBox = new TextBox(this.TextBoxLocation, 1);
                            this.SpeechBox.position = new Vector2(this.PositionToWriteTo.X - 50, this.PositionToWriteTo.Y - 50);
                            this.SpeechBox.Draw(spriteBatch, false);
                            if (this.SpeakerTexture != null)
                            {
                                spriteBatch.Draw(this.SpeakerTexture, new Vector2(this.SpeechBox.position.X, this.SpeechBox.position.Y - 276), this.SpeakerPortraitSourceRectangle, Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, 1f);
                            }
                            break;

                    }
                }
                spriteBatch.DrawString(Game1.AllTextures.MenuText, typedText, this.PositionToWriteTo, this.Color, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, 1f);

                if (this.AreSelectableOptionsActivated)
                {
                    foreach (SelectableOption option in SelectableOptions)
                    {
                        option.Draw(spriteBatch);
                    }
                }

            }
        }

        public void PlayTextNoise()
        {
            float noiseRand = Game1.Utility.RFloat(0, 1);

            Game1.SoundManager.TextNoise.Play(.1f, noiseRand, 0f);

        }

    }

    public class SelectableOption
    {
        public Button Button { get; set; }
        public string Response { get; set; }
        public string Action { get; set; }

        public SelectableOption(string response, string action, Vector2 position)
        {
            this.Response = response;
            this.Action = action;
            this.Button = new Button(new Rectangle((int)position.X, (int)position.Y, (int)Game1.AllTextures.MenuText.MeasureString(response).X,
                (int)Game1.AllTextures.MenuText.MeasureString(response).Y), CursorType.Normal, 2f);
 
        }

        public void Update(GameTime gameTime, Character characterTalking)
        {
            this.Button.UpdateSelectableText(Game1.MouseManager);
            if (this.Button.isClicked)
            {
                Game1.Utility.PerformSpeechAction(this.Action, characterTalking);

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Button.DrawSelectableTextBoxOption(spriteBatch, this.Response);
        }
    }
}
