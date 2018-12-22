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
    public class EscMenu : IGeneral
    {

        Texture2D menuButtonTexture;
        Texture2D settingsButtonTexture;
        Texture2D returnButtonTexture;

        Texture2D backDrop;

        Button menuButton;
        Button settingsButton;
        Button returnButton;

       public bool isTextChanged = false;

        string menuText;
       public string settingsText;
        string returnText;

        SpriteFont font;

        List<Button> allButtons;

        Game1 game;

      //  SaveLoadManager saveManager;
        SaveFile mySave;

        public EscMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse)
        {
            mySave = new SaveFile();
            this.game = game;
            menuButtonTexture = content.Load<Texture2D>("Button/basicButton");
            settingsButtonTexture = content.Load<Texture2D>("Button/basicButton");
            returnButtonTexture = content.Load<Texture2D>("Button/basicButton");

            backDrop = content.Load<Texture2D>("Button/escBackDrop");

            font = content.Load<SpriteFont>("SpriteFont/MenuText");

            returnButton = new Button(returnButtonTexture, graphicsDevice, mouse, new Vector2(Utility.centerScreen.X, Utility.centerScreenY - 100));
            
            settingsButton = new Button(settingsButtonTexture, graphicsDevice, mouse, Utility.centerScreen);

            menuButton = new Button(menuButtonTexture, graphicsDevice, mouse, new Vector2(Utility.centerScreen.X, Utility.centerScreenY + 100));

            menuText = "Exit Game";
            settingsText = "Save Game";
            returnText = "Return";



            allButtons = new List<Button>() { menuButton, settingsButton, returnButton };

        }

        public void Update(GameTime gameTime)
        {
            if (isTextChanged == true)
            {
                settingsText = "Game Saved!";
            }
            if(isTextChanged == false)
            {
                settingsText = "Save Game";
            }

            foreach (Button button in allButtons)
            {
                button.Update();
            }

            if(menuButton.isClicked)
            {
                game.gameStages = Stages.MainMenu;
            }

            if(returnButton.isClicked)
            {
                UserInterface.IsEscMenu = false;
            }

            if(settingsButton.isClicked)
            {
                mySave.Save();
                isTextChanged = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backDrop, new Vector2(435, 170), Color.White);
            menuButton.Draw(spriteBatch, font, menuText, menuButton.FontLocation, Color.BlueViolet);
            returnButton.Draw(spriteBatch, font, returnText, returnButton.FontLocation, Color.BlueViolet);
            settingsButton.Draw(spriteBatch, font, settingsText, settingsButton.FontLocation, Color.BlueViolet);


            
        }

       
    }
}
