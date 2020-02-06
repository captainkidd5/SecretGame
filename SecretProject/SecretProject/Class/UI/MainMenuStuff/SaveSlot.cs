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

        public SaveSlot(int id, Button button)
        {
            this.ID = id;
            this.String = "Empty";
            this.Button = button;
            
        }

        public void Update(GameTime gameTime)
        {
            this.Button.Update(Game1.myMouseManager);
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
            this.Button.Draw( spriteBatch,new Rectangle(80,288, 32,32), Button.BackGroundSourceRectangle,
                Game1.AllTextures.MenuText, this.String, Button.Position, Color.White, 3f, 3f, .8f, false);
        
            }
    }
}
