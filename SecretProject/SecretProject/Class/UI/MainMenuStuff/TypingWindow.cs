using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.MainMenuStuff
{
    public class TypingWindow
    {
        public bool IsActive { get; set; }
        public float Scale { get; set; }
        public Button Button { get; set; }
        public GraphicsDevice Graphics { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 InsertionIconPosition { get; set; }

        public Rectangle IconSourceRectangle { get; set; }
        public Rectangle BackGroundSourceRectangle { get; set; }

        public string EnteredString { get; set; }

        public SimpleTimer IconFlashTimer { get; set; }
        public bool IsIconDrawn { get; set; }

        public TypingWindow(GraphicsDevice graphics,  Vector2 position)
        {
            this.Scale = 3f;
            this.Graphics = graphics;
            this.Position = position;
            this.IconSourceRectangle = new Rectangle(832, 696, 2, 18);
            this.BackGroundSourceRectangle = new Rectangle(848, 688, 160, 32);
            this.Button = new Button(Game1.AllTextures.UserInterfaceTileSet, this.BackGroundSourceRectangle, graphics, this.Position, Controls.CursorType.Normal, Scale, null);
            this.EnteredString = string.Empty;
            this.IconFlashTimer = new SimpleTimer(1f);
        }

        public void Update(GameTime gameTime)
        {
            this.Button.Update(Game1.MouseManager);
            if (this.IsActive && Game1.MouseManager.IsClicked && !this.Button.isClicked)
            {
                this.IsActive = false;
                this.IsIconDrawn = false;
            }
            else if (this.Button.isClicked && !this.IsActive)
            {
                this.IsActive = true;
            }


            if (this.IsActive)
            {


                Keys[] pressedKeys = Game1.KeyboardManager.OldKeyBoardState.GetPressedKeys();
                foreach (Keys key in pressedKeys)
                {
                    if (Game1.KeyboardManager.WasKeyPressed(key))
                    {
                        string keyValue = string.Empty;
                        if (key == Keys.Space)
                        {
                            keyValue = " ";
                            this.EnteredString += keyValue;
                        }
                        else if (key == Keys.Back)
                        {
                            if (this.EnteredString.Length > 0)
                            {
                                this.EnteredString = this.EnteredString.TrimEnd(EnteredString[EnteredString.Length - 1]);
                            }
                        }
                        else if (key == Keys.Enter)
                        {
                            // ProcessString();
                            this.IsActive = false;
                            this.IsIconDrawn = false;
                            return;
                        }
                        else if ((int)key > 64 && (int)key < 91)
                        {
                            keyValue = key.ToString();
                            this.EnteredString += keyValue;
                        }
                        else if ((int)key > 47 && (int)key < 58)
                        {
                            keyValue = key.ToString();
                            keyValue = keyValue.TrimStart('D');

                            this.EnteredString += keyValue;
                        }
                        else if (key == Keys.Subtract)
                        {
                            keyValue = "-";
                            this.EnteredString += keyValue;
                        }


                    }


                }
            }
            this.InsertionIconPosition = new Vector2(this.Position.X + Game1.AllTextures.MenuText.MeasureString(this.EnteredString).X * this.Scale + 4, this.Position.Y + 4 * Scale);
            if (this.IsActive && this.IconFlashTimer.Run(gameTime))
            {
                this.IsIconDrawn = !IsIconDrawn;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            this.Button.Draw(spriteBatch, Game1.AllTextures.MenuText, this.EnteredString, Button.FontLocation, Button.Color, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth, 3f);
            if(this.IsIconDrawn)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.InsertionIconPosition, this.IconSourceRectangle, Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, .9f);
            }
        }
    }
}
