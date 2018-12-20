using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;

namespace SecretProject.Class.UI
{
    public class UserInterface
    {
        ToolBar bottomBar;
        EscMenu esc;
        Game1 game;
        GraphicsDevice graphicsDevice;
        ContentManager content;
        MouseManager mouse;

        static bool isEscMenu;

        public static bool IsEscMenu { get { return isEscMenu; } set { isEscMenu = value; } }

        //keyboard


        public UserInterface(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, MouseManager mouse)
        {
            this.game = game;
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            this.mouse = mouse;
            isEscMenu = false;
            
            bottomBar = new ToolBar(game, graphicsDevice, content, mouse);
            esc = new EscMenu(game, graphicsDevice, content, mouse);
            
        }


        public void Update(GameTime gameTime, KeyboardState kState, KeyboardState oldKeyState)
        {
            bottomBar.Update(gameTime);

            if ((oldKeyState.IsKeyDown(Keys.Escape)) && (kState.IsKeyUp(Keys.Escape)))
            {
                isEscMenu = !isEscMenu;

            }

            if(isEscMenu)
            {
                esc.Update(gameTime);
                Game1.freeze = true;
            }
            if(!isEscMenu)
            {
                Game1.freeze = false;
            }
                
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            bottomBar.Draw(spriteBatch);
            if(isEscMenu)
            {
                esc.Draw(spriteBatch);
            }
            spriteBatch.End();
        }


    }
}
