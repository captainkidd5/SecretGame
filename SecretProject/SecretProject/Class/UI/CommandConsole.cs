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
        public string DisplayLog { get; set; } = string.Empty;
        public Vector2 DisplayLogPosition { get; set; } = Game1.Utility.Origin;
        public CommandConsole(Vector2 position)
        {
            this.Position = position;
        }

        public void Update(GameTime gameTime)
        {
            Game1.freeze = true;
            Keys[] pressedKeys = Game1.OldKeyBoardState.GetPressedKeys();
            if(Game1.myMouseManager.HasScrollWheelValueIncreased)
            {
                this.DisplayLogPosition = new Vector2(DisplayLogPosition.X, DisplayLogPosition.Y - 20);
            }
            else if (Game1.myMouseManager.HasScrollWheelValueDecreased)
            {
                this.DisplayLogPosition = new Vector2(DisplayLogPosition.X, DisplayLogPosition.Y + 20);
            }
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
                    int numberToSpawn = int.Parse(separatedString[2]);
                    int itemID = int.Parse(separatedString[1]);
                    for (int i =0; i < numberToSpawn; i++)
                    {
                        Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem(itemID, null));
                    }
                    
                    break;
                case "getnine":
                    Game1.Player.UserInterface.AddAlert(AlertSize.Large, Game1.Utility.centerScreen, "this is a thicc test");
                    break;
                case "show":
                    for (int i = 0; i < Game1.AllItems.AllItems.Count; i++)
                        this.DisplayLog += Game1.AllItems.AllItems[i].Name + " : " + Game1.AllItems.AllItems[i].ID.ToString() + "\n";
                    break;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.EnteredString, this.Position, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.DisplayLog, Game1.Utility.Origin, Color.White, 0f, this.DisplayLogPosition, 2f, SpriteEffects.None, 1f);
        }
    }
}
