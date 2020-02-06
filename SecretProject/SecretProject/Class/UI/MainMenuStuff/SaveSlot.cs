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
        public GraphicsDevice Graphics { get; set; }
        public int ID { get; set; }
        public bool Occupied { get; set; }
        public string String { get; set; }
        public Button Button { get; set; }

        public SaveSlot(GraphicsDevice graphics, int id, Button button)
        {
            this.Graphics = graphics;
            this.ID = id;
            this.String = "Empty";
            this.Button = button;
            if(Game1.SaveLoadManager.CheckIfSaveEmpty(this.ID))
            {
                this.Occupied = true;
            }
            this.String = "Occupied";
            
        }

        public void Update(GameTime gameTime)
        {
            this.Button.Update(Game1.myMouseManager);
            if(this.Button.isClicked)
            {
                if(this.Occupied)
                {

                    LoadSave();
                }
                else
                {
                   StartNewSave();
                   
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

        public void LoadSave()
        {
            Game1.SaveLoadManager.Load(this.Graphics,Game1.SaveLoadManager.GetSaveFileFromID(this.ID));
        }

        public void StartNewSave()
        {
            Game1.SaveLoadManager.CurrentSave = this.ID;
            Game1.SaveLoadManager.Save();
        }
    }
}
