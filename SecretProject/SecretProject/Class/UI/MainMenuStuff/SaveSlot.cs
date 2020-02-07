using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.IO;

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
           
            this.Button = button;
            if(Game1.SaveLoadManager.CheckIfSaveEmpty(this.ID))
            {
                this.Occupied = false;
                this.String = "Empty";
            }
            else
            {
                this.Occupied = true;
               
                this.String = "Occupied";
            }
            
            
        }

        public void Update(GameTime gameTime)
        {
            this.Button.Update(Game1.myMouseManager);
            if(this.Button.isClicked)
            {
                if(this.Occupied)
                {

                    LoadSave();
                    Game1.mainMenu.StartNewGame();
                    return;
                }
                else
                {
                   StartNewSave();
                    this.String = Game1.GlobalClock.Calendar.CurrentMonth.ToString() + ", " + Game1.GlobalClock.Calendar.CurrentDay + DateTime.Now.ToString("h:mm:ss tt");
                    Game1.SaveLoadManager.Save( Game1.SaveLoadManager.MainMenuData, false);
                    
                    Game1.mainMenu.StartNewGame();
                    return;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Button.Draw( spriteBatch,new Rectangle(80,288, 32,32), Button.BackGroundSourceRectangle,
                Game1.AllTextures.MenuText, this.String, Button.Position, Button.Color, 2f, 2f, .8f, true);
        
            }

        public void LoadSave()
        {
            Game1.SaveLoadManager.Load(this.Graphics,Game1.SaveLoadManager.GetSaveFileFromID(this.ID));
        }

        public void StartNewSave()
        {
            Game1.SaveLoadManager.CurrentSave = this.ID;
            Game1.SaveLoadManager.Save(Game1.SaveLoadManager.GetSaveFileFromID(Game1.SaveLoadManager.CurrentSave));
        }

        public void SaveString(BinaryWriter writer)
        {
            writer.Write(this.String);
        }

        public void LoadString(BinaryReader reader)
        {
            this.String = reader.ReadString();
        }
    }
}
