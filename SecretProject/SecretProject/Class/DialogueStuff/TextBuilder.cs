using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public string SpeakerName { get; set; }
        public int SpeakerID { get; set; }
        

        public Texture2D SpeakerTexture { get; set; }
        public Rectangle SpeakerPortraitSourceRectangle { get; set; }
        public TextBuilder(string stringToWrite, float writeSpeed, float stringDisplayTimer)
        {
            this.StringToWrite = stringToWrite;
            this.WriteSpeed = writeSpeed;
            this.SpeedAnchor = writeSpeed;
            PositionToWriteTo = Game1.Utility.DialogueTextLocation;
            this.Scale = 2f;
            this.Color = Color.Black;
            this.NumberOfClicks = 0;


            this.TextBoxLocation = new Vector2(PositionToWriteTo.X, PositionToWriteTo.Y);
            SpeechBox = new TextBox(TextBoxLocation, 1);
            SpeechBox.position = new Vector2(PositionToWriteTo.X, PositionToWriteTo.Y);
            this.LineLimit = SpeechBox.DestinationRectangle.Width - 100;
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
            this.parsedText = ParseText(this.StringToWrite, this.LineLimit, this.Scale);
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
                if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)))
                {
                    Reset();
                    this.IsActive = false;
                }
                if (UseTextBox)
                {
                    Game1.Player.UserInterface.BottomBar.IsActive = false;

                }
                if (AreSelectableOptionsActivated)
                {
                    Game1.freeze = true;
                    ClearWindowForResponse();
                    foreach (SelectableOption option in SelectableOptions)
                    {
                        option.Update(gameTime, this.SpeakerName, this.SpeakerID);
                    }
                }

                else
                {

                    if (this.IsPaused && this.NumberOfClicks == 0 && Game1.myMouseManager.IsHovering(this.SpeechBox.DestinationRectangle))
                    {
                        Game1.myMouseManager.ChangeMouseTexture(CursorType.NextChatWindow);
                    }


                    if (NumberOfClicks >= 1)
                    {
                        this.SpeedAnchor = .1f;
                        if (this.IsPaused)
                        {
                            if (this.MoveToSelectableOptions && Skeleton != null)
                            {
                                if (Skeleton.SelectableOptions != null)
                                {
                                    if (!this.HaveOptionsBeenChecked)
                                    {
                                        CheckSelectableOptions(Skeleton);
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
                    if (NumberOfClicks == 2)
                    {
                        if (this.Skeleton != null)
                        {

                        }
                        else
                        {
                            Reset();
                        }

                    }
                    if (Game1.myMouseManager.IsClicked)
                    {
                        NumberOfClicks++;
                    }
                    if (FreezeStage && NumberOfClicks < 2)
                    {
                        Game1.freeze = true;

                    }
                    else
                    {
                        Reset();
                    }


                    if (!isDoneDrawing && !IsPaused)
                    {
                        if (WriteSpeed == 0)
                        {
                            typedText = parsedText;
                            isDoneDrawing = true;
                        }
                        else if (typedTextLength < parsedText.Length - 1)
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


                            SpeedAnchor += (float)(gameTime.ElapsedGameTime.TotalMilliseconds / WriteSpeed);
                            if (SpeedAnchor > 2f)
                            {
                                typedTextLength++;
                                PlayTextNoise();
                                SpeedAnchor = 0f;
                            }

                            typedText = parsedText.Substring(0, (int)typedTextLength);
                        }
                    }

                }
            }
        }

        public String ParseText(String text, float lineLimit, float scale)
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

        public void Reset(bool unfreeze = true)
        {
            this.StringToWrite = "";
            this.IsActive = false;
            this.UseTextBox = false;
            this.typedText = "";
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
            this.isDoneDrawing = false;
            this.parsedText = "";
            Game1.Player.UserInterface.BottomBar.IsActive = true;
            this.typedTextLength = 0;
            this.IsPaused = false;
            this.Skeleton = null;
            SelectableOptions = new List<SelectableOption>();
            this.HaveOptionsBeenChecked = false;
            this.AreSelectableOptionsActivated = false;
            this.MoveToSelectableOptions = false;
            this.SpeakerName = null;
            this.SpeakerID = -1;


        }


        public void MoveTextToNewWindow()
        {
            this.typedText = "";
            this.NumberOfClicks = 0;
            this.typedTextLength = 0;
            StringToWrite = StringToWrite.Split(new[] { '#' }, 2)[1];
            ChangedParsedText();

        }

        public void ClearWindowForResponse()
        {
            this.typedText = "";
            this.NumberOfClicks = 0;
            this.typedTextLength = 0;
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
                SelectableOptions.Add(new SelectableOption(response, action, new Vector2(PositionToWriteTo.X, PositionToWriteTo.Y + 64 * s)));
            }
            if (options.Length > 0)
            {
                this.AreSelectableOptionsActivated = true;
            }

            //Reset();


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
                            SpeechBox.position = new Vector2(PositionToWriteTo.X, PositionToWriteTo.Y);
                            SpeechBox.Draw(spriteBatch,false);
                            break;
                        case TextBoxType.dialogue:
                            SpeechBox = new TextBox(TextBoxLocation, 1);
                            SpeechBox.position = new Vector2(PositionToWriteTo.X - 50, PositionToWriteTo.Y - 50);
                            SpeechBox.Draw(spriteBatch,false);
                            if(this.SpeakerTexture != null)
                            {
                                spriteBatch.Draw(SpeakerTexture, new Vector2(SpeechBox.position.X, SpeechBox.position.Y - 255), this.SpeakerPortraitSourceRectangle, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, 1f);
                            }
                            break;

                    }
                }
                spriteBatch.DrawString(Game1.AllTextures.MenuText, typedText, this.PositionToWriteTo, this.Color, 0f, Game1.Utility.Origin, Scale, SpriteEffects.None, 1f);

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
            int noiseRand = Game1.Utility.RGenerator.Next(1, 3);
            if (noiseRand == 1)
            {

                Game1.SoundManager.TextNoise.Play(.1f, 0f, 0f);
            }
            else
            {
                Game1.SoundManager.TextNoise2.Play(.1f, 0f, 0f);
            }
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
            Button = new Button(new Rectangle((int)position.X, (int)position.Y, (int)Game1.AllTextures.MenuText.MeasureString(response).X,
                (int)Game1.AllTextures.MenuText.MeasureString(response).Y), CursorType.Normal);
        }

        public void Update(GameTime gameTime, string speakerName, int speakerID)
        {
            Button.UpdateSelectableText(Game1.myMouseManager);
            if (Button.isClicked)
            {
                Game1.Utility.PerformSpeechAction(this.Action, speakerID, speakerName);

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Button.DrawSelectableTextBoxOption(spriteBatch, Response);
        }
    }
}
