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
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;

namespace SecretProject.Class.UI
{
    public class UserInterface
    {
        ContentManager content;

        static bool isEscMenu;

        public static bool IsEscMenu { get { return isEscMenu; } set { isEscMenu = value; } }

        public GraphicsDevice GraphicsDevice { get; set; }
        public Game1 Game { get; set; }
        public EscMenu Esc { get; set; }
        internal ToolBar BottomBar { get; set; }

        //keyboard


        public UserInterface(Game1 game, GraphicsDevice graphicsDevice, ContentManager content )
        {
            this.Game = game;
            this.GraphicsDevice = graphicsDevice;
            this.content = content;
            isEscMenu = false;
            
            BottomBar = new ToolBar(game, graphicsDevice, content);
            Esc = new EscMenu(graphicsDevice, content);
            
        }


        public void Update(GameTime gameTime, KeyboardState kState, KeyboardState oldKeyState, Inventory inventory, MouseManager mouse, Game1 game)
        {
            BottomBar.Update(gameTime, inventory, mouse);

            if ((oldKeyState.IsKeyDown(Keys.Escape)) && (kState.IsKeyUp(Keys.Escape)))
            {
                isEscMenu = !isEscMenu;

                if (isEscMenu == false)
                {
                    Esc.isTextChanged = false;
                }
                    

            }

            if(isEscMenu)
            {
                Esc.Update(gameTime, mouse, game );
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
            BottomBar.Draw(spriteBatch);
            if(isEscMenu)
            {
                Esc.Draw(spriteBatch);
            }
            spriteBatch.End();
        }


    }
}
