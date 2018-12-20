using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;

namespace SecretProject.Class.MenuStuff
{
    class EscMenu : IGeneral
    {

        Texture2D menuButtonTexture;
        Texture2D settingsButtonTexture;
        Texture2D returnButtonTexture;

        Texture2D backDrop;

        Button menuButton;
        Button settingsButton;
        Button returnButton;

        SpriteFont font;

        List<Button> allButtons;

        Game1 game;

        public EscMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse)
        {
            this.game = game;
            menuButtonTexture = content.Load<Texture2D>("Button/basicButton");
            settingsButtonTexture = content.Load<Texture2D>("Button/basicButton");
            returnButtonTexture = content.Load<Texture2D>("Button/basicButton");

            backDrop = content.Load<Texture2D>("Button/escBackDrop");

            font = content.Load<SpriteFont>("SpriteFont/MenuText");

            returnButton = new Button(returnButtonTexture, graphicsDevice, mouse, new Vector2(Utility.centerScreen.X, Utility.centerScreenY - 100));
            
            settingsButton = new Button(settingsButtonTexture, graphicsDevice, mouse, Utility.centerScreen);

            menuButton = new Button(menuButtonTexture, graphicsDevice, mouse, new Vector2(Utility.centerScreen.X, Utility.centerScreenY + 100));




            allButtons = new List<Button>() { menuButton, settingsButton, returnButton };

        }

        public void Update(GameTime gameTime)
        {
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backDrop, new Vector2(435, 170), Color.White);
            menuButton.Draw(spriteBatch, font, "Menu", menuButton.FontLocation, Color.BlueViolet);
            returnButton.Draw(spriteBatch, font, "Return", returnButton.FontLocation, Color.BlueViolet);
            settingsButton.Draw(spriteBatch, font, "Settings", settingsButton.FontLocation, Color.BlueViolet);
        }

       
    }
}
