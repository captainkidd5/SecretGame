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



    public class TextBuilder
    {
        private enum TextBoxState
        {
            close = 0,
            speaking = 1,
            waitingforresponse = 2

        }
        public string StringToWrite { get; set; }
        public bool IsActive { get; set; }
        private float WriteSpeed { get; set; }
        private float SpeedAnchor { get; set; }
        private Vector2 PositionToWriteTo { get; set; }
        private Vector2 TextBoxLocation { get; set; }
        private bool FreezeStage { get; set; }
        public float Scale { get; set; }
        public Color Color { get; set; }
        private int NumberOfClicks { get; set; }

        String parsedText;
        String typedText;
        double typedTextLength;
        bool isDoneDrawing;

        List<SelectableOption> SelectableOptions;
        private float LineLimit { get; set; }

        private TextBox SpeechBox { get; set; }
        private float startDisplay { get; set; }


        private bool IsPaused { get; set; }
        private bool HaveOptionsBeenChecked { get; set; }

        private bool MoveToSelectableOptions { get; set; }

        public DialogueSkeleton Skeleton { get; set; }

        private Character CharacterTalking { get; set; }
        private string SpeakerName { get; set; }
        private int SpeakerID { get; set; }


        public Texture2D SpeakerTexture { get; set; }
        public Rectangle SpeakerPortraitSourceRectangle { get; set; }

        private TextBoxState textBoxState { get; set; }
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


        }

        private void ChangedParsedText()
        {
            parsedText = ParseText(this.StringToWrite, this.LineLimit, this.Scale);
        }


        public void ActivateCharacter(Character character, bool freezeStage, string stringToWrite, float scale)
        {
            this.CharacterTalking = character;
            this.SpeakerTexture = character.CharacterPortraitTexture;
            this.SpeakerPortraitSourceRectangle = character.CharacterPortraitSourceRectangle;
            this.SpeakerID = character.SpeakerID;
            this.SpeakerName = character.Name;
            this.IsActive = true;
            this.FreezeStage = freezeStage;
            this.StringToWrite = stringToWrite;
            this.Scale = scale;
            this.SpeedAnchor = .1f;



            ChangedParsedText();
            this.textBoxState = TextBoxState.speaking;

            this.PositionToWriteTo = new Vector2(this.SpeechBox.DestinationRectangle.X, this.SpeechBox.DestinationRectangle.Y);
            this.LineLimit = this.SpeechBox.DestinationRectangle.Width - 100;



        }

        public void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {

                Game1.Player.UserInterface.BottomBar.IsActive = false;


                switch (textBoxState)
                {
                    case TextBoxState.speaking:
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
                                HandleSpecialCase(parsedText[(int)typedTextLength + 1]);


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
                        break;

                    case TextBoxState.waitingforresponse:
                        Game1.freeze = true;
                        ClearWindowForResponse();
                        foreach (SelectableOption option in SelectableOptions)
                        {
                            option.Update(gameTime, this.CharacterTalking);
                        }
                        break;
                }



                if (Game1.KeyboardManager.WasKeyPressed(Keys.Escape))
                {
                    Reset();
                    this.IsActive = false;
                }
            }
        }

        /// <summary>
        /// interprets special characters from the dialogue xml.
        /// </summary>
        /// <param name="character"></param>
        private void HandleSpecialCase(Char character)
        {
            switch (character)
            {
                case '#': //pause and wait for input.
                    parsedText.Remove((int)typedTextLength, 1);
                    PauseUntilInput();
                    break;

                case '`': //pause and wait for input, then move to selectable options.
                    parsedText.Remove((int)typedTextLength, 1);
                    PauseUntilInput();
                    this.MoveToSelectableOptions = true;
                    break;

                case '%': //change write speed.
                    parsedText.Remove((int)typedTextLength, 1);
                    string speed = parsedText[(int)typedTextLength + 2].ToString() + parsedText[(int)typedTextLength + 3].ToString() + parsedText[(int)typedTextLength + 4].ToString();
                    this.WriteSpeed = int.Parse(speed);
                    parsedText = parsedText.Remove((int)typedTextLength + 1, 4);
                    break;

                case '$': //Insert Playername.
                    parsedText = parsedText.Remove((int)typedTextLength + 1, 1);
                    parsedText = parsedText.Insert((int)typedTextLength + 1, Game1.Player.Name);
                    break;

                default:
                    break;
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

            String[] lineArray = text.Split('\n');
            float lengthToReturn = 0f;
            for (int i = 0; i < lineArray.Length; i++)
            {
                float length = Game1.AllTextures.MenuText.MeasureString(lineArray[i]).X * textScale;
                if (length > lengthToReturn)
                {
                    lengthToReturn = length;
                }
            }

            return lengthToReturn;



        }

        public static float GetTextHeight(string text, float textScale)
        {

            String[] lineArray = text.Split('\n');

            float totalHeight = 0;

            foreach (string line in lineArray)
            {
                totalHeight += Game1.AllTextures.MenuText.MeasureString(line).Y * textScale;
            }

            return totalHeight;
        }

        public void Reset(bool unfreeze = true)
        {
            this.StringToWrite = "";
            this.IsActive = false;

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

            this.MoveToSelectableOptions = false;
            if (this.CharacterTalking != null)
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

        private void ClearWindowForResponse()
        {
            typedText = "";
            this.NumberOfClicks = 0;
            typedTextLength = 0;
        }

        private void PauseUntilInput()
        {
            this.NumberOfClicks = 0;
            this.IsPaused = true;


        }

        private void CheckSelectableOptions(DialogueSkeleton skeleton)
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
                this.textBoxState = TextBoxState.waitingforresponse;

            }

            //Reset();


        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (this.IsActive)
            {

                this.SpeechBox.position = new Vector2(this.PositionToWriteTo.X - 50, this.PositionToWriteTo.Y - 50);
                this.SpeechBox.Draw(spriteBatch, false);
                if (this.SpeakerTexture != null)
                {
                    spriteBatch.Draw(this.SpeakerTexture, new Vector2(this.SpeechBox.position.X, this.SpeechBox.position.Y - 276), this.SpeakerPortraitSourceRectangle, Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, 1f);
                }




                spriteBatch.DrawString(Game1.AllTextures.MenuText, typedText, this.PositionToWriteTo, this.Color, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, 1f);

                if (textBoxState == TextBoxState.waitingforresponse)
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
                PerformSpeechAction(this.Action, characterTalking);

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Button.DrawSelectableTextBoxOption(spriteBatch, this.Response);
        }

        #region SPEECHUTILITY
        public void PerformSpeechAction(string action, Character character)
        {

            switch (action)
            {
                case "OpenJulianShop":
                    Game1.freeze = true;
                    Game1.Player.UserInterface.ActivateShop(OpenShop.JulianShop);
                    Game1.Player.UserInterface.TextBuilder.Reset();

                    break;
                case "OpenDobbinShop":
                    Game1.Player.UserInterface.ActivateShop(OpenShop.DobbinShop);
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    break;

                case "OpenElixirShop":
                    Game1.Player.UserInterface.ActivateShop(OpenShop.ElixirShop);
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    break;
                case "OpenKayaShop":
                    Game1.Player.UserInterface.ActivateShop(OpenShop.KayaShop);
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    break;
                case "OpenSarahShop":
                    Game1.Player.UserInterface.ActivateShop(OpenShop.SarahShop);
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    break;

                case "OpenBusinessSnailShop":
                    Game1.Player.UserInterface.ActivateShop(OpenShop.BusinessSnailShop);
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    break;
                case "LoadQuest":
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    Game1.Player.UserInterface.TextBuilder.ActivateCharacter(character, true, character.QuestHandler.ActiveQuest.MidQuestSkeleton.TextToWrite, 2f);
                    Game1.Player.UserInterface.TextBuilder.Skeleton = character.QuestHandler.ActiveQuest.MidQuestSkeleton;
                    break;

                case "CheckCurrentProject":

                case "ExitDialogue":
                    Game1.Player.UserInterface.TextBuilder.Reset();
                    break;
                default:
                    Game1.Player.UserInterface.TextBuilder.Reset();

                    Game1.Player.UserInterface.TextBuilder.ActivateCharacter(character, true, character.Name + ": " + action, 2f);

                    //   Game1.freeze = true;
                    return;


            }
        }
        #endregion
    }
}
