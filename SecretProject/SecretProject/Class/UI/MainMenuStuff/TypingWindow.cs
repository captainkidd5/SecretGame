﻿using Microsoft.Xna.Framework;
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
        public Rectangle BackGroundSourceRectangle { get; set; }

        public string EnteredString { get; set; }

        public TypingWindow(GraphicsDevice graphics,  Vector2 position)
        {
            this.Scale = 3f;
            this.Graphics = graphics;
            this.Position = position;
            this.BackGroundSourceRectangle = new Rectangle(832, 624, 192, 32);
            this.Button = new Button(Game1.AllTextures.UserInterfaceTileSet, this.BackGroundSourceRectangle, graphics, this.Position, Controls.CursorType.Normal, Scale, null);
            this.EnteredString = string.Empty;
        }

        public void Update(GameTime gameTime)
        {
            this.Button.Update(Game1.myMouseManager);
            Keys[] pressedKeys = Game1.OldKeyBoardState.GetPressedKeys();
            foreach (Keys key in pressedKeys)
            {
                if ((Game1.OldKeyBoardState.IsKeyDown(key)) && (Game1.NewKeyBoardState.IsKeyUp(key)))
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
                        this.EnteredString = String.Empty;
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
        public void Draw(SpriteBatch spriteBatch)
        {
            this.Button.Draw(spriteBatch, Game1.AllTextures.MenuText, this.EnteredString, Button.FontLocation, Button.Color, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth, 3f);
        }
    }
}
