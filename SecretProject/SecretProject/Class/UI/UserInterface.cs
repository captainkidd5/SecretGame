using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;

namespace SecretProject.Class.UI
{
    public class UserInterface : IGeneral
    {
        ToolBar bottomBar;
        Game1 game;
        GraphicsDevice graphicsDevice;
        ContentManager content;
        MouseManager mouse;
        

        public UserInterface(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse)
        {
            this.game = game;
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            this.mouse = mouse;
            
            bottomBar = new ToolBar(game, graphicsDevice, content, mouse);
        }


        public void Update(GameTime gameTime)
        {
            bottomBar.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            bottomBar.Draw(spriteBatch);
        }


    }
}
