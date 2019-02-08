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
    public class EscMenu
    {
        public bool isTextChanged = false;
        List<Button> allButtons;




        //  SaveLoadManager saveManager;
        SaveFile mySave;

        private Texture2D MenuButtonTexture { get; set; }
        private Texture2D SettingsButtonTexture { get; set; }
        private Texture2D ReturnButtonTexture { get; set; }
        private Texture2D ToggleFullScreenButtonTexture { get; set; }
        private Texture2D BackDrop { get; set; }
        internal Button MenuButton { get; set; }
        internal Button SettingsButton { get; set; }
        internal Button ReturnButton { get; set; }
        internal Button ToggleFullScreenButton { get; set; }
        private string ReturnText { get; set; }
        private string SettingsText { get; set; }
        private string MenuText { get; set; }
        private string ToggleFullScreenButtonText;
        private SpriteFont Font { get; set; }

        public EscMenu(GraphicsDevice graphicsDevice, ContentManager content)
        {
            mySave = new SaveFile();
            MenuButtonTexture = content.Load<Texture2D>("Button/basicButton");
            SettingsButtonTexture = content.Load<Texture2D>("Button/basicButton");
            ReturnButtonTexture = content.Load<Texture2D>("Button/basicButton");
            ToggleFullScreenButtonTexture = content.Load<Texture2D>("Button/basicButton");

            BackDrop = content.Load<Texture2D>("Button/escBackDrop");

            Font = content.Load<SpriteFont>("SpriteFont/MenuText");

            ReturnButton = new Button(ReturnButtonTexture, graphicsDevice, new Vector2(Utility.centerScreen.X, Utility.centerScreenY - 100));
            
            SettingsButton = new Button(SettingsButtonTexture, graphicsDevice, Utility.centerScreen);

            MenuButton = new Button(MenuButtonTexture, graphicsDevice, new Vector2(Utility.centerScreen.X, Utility.centerScreenY + 100));
            ToggleFullScreenButton = new Button(ToggleFullScreenButtonTexture, graphicsDevice, new Vector2(Utility.centerScreenX, Utility.centerScreenY + 200));

            MenuText = "Exit Game";
            SettingsText = "Save Game";
            ReturnText = "Return";
            ToggleFullScreenButtonText = "FullScreen Mode";



            allButtons = new List<Button>() { MenuButton, SettingsButton, ReturnButton, ToggleFullScreenButton };

        }

        public void Update(GameTime gameTime, MouseManager mouse, Game1 game)
        {
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
                game.gameStages = Stages.MainMenu;
            }

            if(ReturnButton.isClicked)
            {
                UserInterface.IsEscMenu = false;
            }

            if(SettingsButton.isClicked)
            {
                mySave.Save();
                isTextChanged = true;
            }

            if(ToggleFullScreenButton.isClicked)
            {
                Game1.ToggleFullScreen = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackDrop, new Vector2(435, 170), Color.White);
            MenuButton.Draw(spriteBatch, Font, MenuText, MenuButton.FontLocation, Color.BlueViolet);
            ReturnButton.Draw(spriteBatch, Font, ReturnText, ReturnButton.FontLocation, Color.BlueViolet);
            SettingsButton.Draw(spriteBatch, Font, SettingsText, SettingsButton.FontLocation, Color.BlueViolet);
            ToggleFullScreenButton.Draw(spriteBatch, Font, ToggleFullScreenButtonText, ToggleFullScreenButton.FontLocation, Color.BlueViolet);


            
        }

       
    }
}
