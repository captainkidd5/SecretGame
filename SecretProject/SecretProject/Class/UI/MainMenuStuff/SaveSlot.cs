using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System.Collections.Generic;

namespace SecretProject.Class.UI.MainMenuStuff
{
    public class SaveSlot
    {
        public int ID { get; set; }
        public bool Occupied { get; set; }
        public string String { get; set; }
        public Button Button { get; set; }

        public SaveSlot(Button button)
        {
            this.String = "Empty";
            this.Button = button;
            
        }

        public void Update(GameTime gameTime)
        {
            if(this.Button.isClicked)
            {
                if(this.Occupied)
                {
                    Game1.mainMenu.LoadSave(this.ID);
                }
                else
                {
                    Game1.mainMenu.StartNewSave(this.ID);
                   
                    this.String = "Occupied";
                    Game1.mainMenu.StartNewGame();
                    return;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Button.Draw(spriteBatch, Game1.AllTextures.MenuText, this.String, Button.Position, Color.Black, Game1.Utility.StandardTextDepth + .01f, Game1.Utility.StandardTextDepth + .02f, 1f);
        }
    }
}
