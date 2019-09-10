using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;

namespace SecretProject.Class.MenuStuff
{
    public class EscMenu : IExclusiveInterfaceComponent
    {
        public bool isTextChanged = false;
        List<Button> allButtons;


        //  SaveLoadManager saveManager;
        SaveLoadManager mySave;

        internal Button MenuButton { get; set; }
        internal Button SettingsButton { get; set; }
        internal Button ReturnButton { get; set; }
        internal Button ToggleFullScreenButton { get; set; }
        private string ReturnText { get; set; }
        private string SettingsText { get; set; }
        private string MenuText { get; set; }
        private string ToggleFullScreenButtonText;
        private SpriteFont Font { get; set; }
        public bool IsActive { get; set; }
        public bool FreezesGame { get; set; }

        public EscMenu(GraphicsDevice graphicsDevice, ContentManager content)
        {
            mySave = new SaveLoadManager();
            

           

            Font = content.Load<SpriteFont>("SpriteFont/MenuText");

            ReturnButton = new Button(Game1.AllTextures.UserInterfaceTileSet,new Rectangle(48,176,128,64),   graphicsDevice, new Vector2(Game1.Utility.centerScreen.X , Game1.Utility.CenterScreenY - 150));
            
            SettingsButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(Game1.Utility.centerScreen.X, Game1.Utility.CenterScreenY- 90));

            MenuButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64),graphicsDevice, new Vector2(Game1.Utility.centerScreen.X, Game1.Utility.CenterScreenY));
            ToggleFullScreenButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(Game1.Utility.centerScreen.X, Game1.Utility.CenterScreenY + 90));

            MenuText = "Exit Game";
            SettingsText = "Save Game";
            ReturnText = "Return";
            ToggleFullScreenButtonText = "FullScreen Mode";



            allButtons = new List<Button>() { MenuButton, SettingsButton, ReturnButton, ToggleFullScreenButton };
            this.IsActive = false;
            this.FreezesGame = true;

        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            Game1.isMyMouseVisible = true;
            if (isTextChanged == true)
            {
                SettingsText = "Game Saved!";
            }
            if(isTextChanged == false)
            {
                SettingsText = "Save Game";
            }

            foreach (Button button in allButtons)
            {
                button.Update(mouse);
            }

            if(MenuButton.isClicked)
            {
                Game1.mainMenu.LoadContent();
                Game1.gameStages = Stages.MainMenu;
            }

            if(ReturnButton.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                isTextChanged = false;
            }

            if(SettingsButton.isClicked)
            {
                mySave.Save();
                isTextChanged = true;
            }

            if(ToggleFullScreenButton.isClicked)
            {
                Game1.FullScreenToggle();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(Game1.Utility.CenterScreenX -200, Game1.Utility.CenterScreenY -200 , 224,304),
                new Rectangle(576, 48, 224, 304),Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .69f);
            MenuButton.Draw(spriteBatch, Font, MenuText, MenuButton.FontLocation, Color.BlueViolet, .69f, .75f);
            ReturnButton.Draw(spriteBatch, Font, ReturnText, ReturnButton.FontLocation, Color.BlueViolet, .69f, .75f);
            SettingsButton.Draw(spriteBatch, Font, SettingsText, SettingsButton.FontLocation, Color.BlueViolet, .69f, .75f);
            ToggleFullScreenButton.Draw(spriteBatch, Font, ToggleFullScreenButtonText, ToggleFullScreenButton.FontLocation, Color.BlueViolet, .69f, .75f);


            
        }

       
    }
}
