using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.Universal;

namespace SecretProject.Class.MenuStuff
{
    class EscMenu : IGeneral
    {

        Texture2D menuButtonTexture;
        Texture2D settingsButtonTexture;
        Texture2D returnButtonTexture;

        Button menuButton;
        Button settingsButton;
        Button returnButton;

        public EscMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse)
        {
            menuButtonTexture = content.Load<Texture2D>("basicButton");
            settingsButtonTexture = content.Load<Texture2D>("basicButton");
            returnButtonTexture = content.Load<Texture2D>("basicButton");

            menuButton = new Button(menuButtonTexture, graphicsDevice, mouse, Utility.centerScreen);
            settingsButton = new Button(settingsButtonTexture, graphicsDevice, mouse, Utility.centerScreen);
            returnButton = new Button(returnButtonTexture, graphicsDevice, mouse, Utility.centerScreen);


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
