using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public class CommandConsole : IExclusiveInterfaceComponent
    {
        public Vector2 Position { get; set; }
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FreezesGame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string EnteredString { get; set; } = string.Empty;

        public CommandConsole(Vector2 position)
        {
            this.Position = position;
        }

        public void Update(GameTime gameTime)
        {
            Game1.freeze = true;
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
                    else if(key == Keys.Back)
                    {
                        if(this.EnteredString.Length > 0)
                        {
                          this.EnteredString = this.EnteredString.TrimEnd(EnteredString[EnteredString.Length - 1]);
                        }
                    }
                    else if(key == Keys.Enter)
                    {
                        ProcessString();
                        this.EnteredString = String.Empty;
                        return;
                    }
                    else if((int)key > 64 && (int)key <  91)
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


                }
            }

        }

        public void ProcessString()
        {
            string[] separatedString = this.EnteredString.Split(' ');

            switch (separatedString[0].ToLower())
            {
                case "spawn":
                    Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem(int.Parse(separatedString[1]), null));
                    break;
                case "getnine":
                    Game1.Player.UserInterface.AddAlert(AlertSize.Large, Game1.Utility.centerScreen, "this is a thicc test");
                    break;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.EnteredString, this.Position, Color.Red, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, 1f);
        }
    }
}
