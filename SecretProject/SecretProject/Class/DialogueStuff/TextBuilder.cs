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
        public bool UseTextBox { get; set; } = false;
        public bool FreezeStage { get; set; } = false;
        public float Scale { get; set; }
        public Color Color { get; set; }
        public int NumberOfClicks { get; set; }
        public TextBoxType TextBoxType { get; set; }

        public TextBuilder(string stringToWrite, float writeSpeed, float stringDisplayTimer)
        {
            this.StringToWrite = stringToWrite;
            this.WriteSpeed = writeSpeed;
            this.StringDisplayTimer = stringDisplayTimer;
            this.StringDisplayAnchor = this.StringDisplayTimer;
            this.SpeedAnchor = WriteSpeed;
            PositionToWriteTo = Game1.Utility.DialogueTextLocation;
            this.Scale = 1f;
            this.Color = Color.Black;
            this.NumberOfClicks = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                if(NumberOfClicks == 1)
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

                if (this.IsActive && WriteSpeed < 0 && currentTextIndex < StringToWrite.Length)
                {
                    outputString += StringToWrite[currentTextIndex];
                    currentTextIndex++;
                    WriteSpeed = SpeedAnchor; ///////////////

                }


            }
        }

        public void ApplyWrap(Vector2 wrapEdge, float yWrapTo)
        {
            if(this.PositionToWriteTo.X + currentTextIndex >= wrapEdge.X)
            {
                this.PositionToWriteTo = new Vector2(this.PositionToWriteTo.X, this.PositionToWriteTo.Y + yWrapTo);
            }
        }

        public void Reset()
        {
            this.outputString = "";
            this.StringToWrite = "";
            this.currentTextIndex = 0;
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (IsActive)
            {

                
                spriteBatch.DrawString(Game1.AllTextures.MenuText, outputString, this.PositionToWriteTo, this.Color, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, layerDepth);
                if(UseTextBox)
                {
                    TextBox speechBox;
                    switch (TextBoxType)
                    {
                        
                        case TextBoxType.normal:
                            //ApplyWrap(PositionToWriteTo, 200);
                            ApplyWrap(new Vector2(PositionToWriteTo.X + 100, PositionToWriteTo.Y), 0);
                            speechBox = new TextBox(PositionToWriteTo, 0);
                            speechBox.position = new Vector2(PositionToWriteTo.X, PositionToWriteTo.Y);
                            speechBox.DrawWithoutString(spriteBatch);
                            break;
                        case TextBoxType.dialogue:
                            ApplyWrap(new Vector2(PositionToWriteTo.X + 100, PositionToWriteTo.Y), 0);
                            speechBox = new TextBox(PositionToWriteTo, 1);
                            speechBox.position = new Vector2(PositionToWriteTo.X - 50, PositionToWriteTo.Y - 50);
                            speechBox.DrawWithoutString(spriteBatch);
                            break;

                    }

                    
                }
                
            }
        }
    }
}
