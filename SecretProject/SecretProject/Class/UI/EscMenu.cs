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

        Game1 game;

      //  SaveLoadManager saveManager;
        SaveFile mySave;

        public Texture2D MenuButtonTexture { get; set; }
        public Texture2D SettingsButtonTexture { get; set; }
        public Texture2D ReturnButtonTexture { get; set; }
        public Texture2D BackDrop { get; set; }
        internal Button MenuButton { get; set; }
        internal Button SettingsButton { get; set; }
        internal Button ReturnButton { get; set; }
        public string ReturnText { get; set; }
        public string SettingsText { get; set; }
        public string MenuText { get; set; }
        public SpriteFont Font { get; set; }

        public EscMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse)
        {
            mySave = new SaveFile();
            this.game = game;
            MenuButtonTexture = content.Load<Texture2D>("Button/basicButton");
            SettingsButtonTexture = content.Load<Texture2D>("Button/basicButton");
            ReturnButtonTexture = content.Load<Texture2D>("Button/basicButton");

            BackDrop = content.Load<Texture2D>("Button/escBackDrop");

            Font = content.Load<SpriteFont>("SpriteFont/MenuText");

            ReturnButton = new Button(ReturnButtonTexture, graphicsDevice, mouse, new Vector2(Utility.centerScreen.X, Utility.centerScreenY - 100));
            
            SettingsButton = new Button(SettingsButtonTexture, graphicsDevice, mouse, Utility.centerScreen);

            MenuButton = new Button(MenuButtonTexture, graphicsDevice, mouse, new Vector2(Utility.centerScreen.X, Utility.centerScreenY + 100));

            MenuText = "Exit Game";
            SettingsText = "Save Game";
            ReturnText = "Return";



            allButtons = new List<Button>() { MenuButton, SettingsButton, ReturnButton };

        }

        public void Update(GameTime gameTime)
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
                button.Update();
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackDrop, new Vector2(435, 170), Color.White);
            MenuButton.Draw(spriteBatch, Font, MenuText, MenuButton.FontLocation, Color.BlueViolet);
            ReturnButton.Draw(spriteBatch, Font, ReturnText, ReturnButton.FontLocation, Color.BlueViolet);
            SettingsButton.Draw(spriteBatch, Font, SettingsText, SettingsButton.FontLocation, Color.BlueViolet);


            
        }

       
    }
}
