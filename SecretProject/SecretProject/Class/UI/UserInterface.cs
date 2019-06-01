using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;

namespace SecretProject.Class.UI
{
    public class UserInterface
    {
        ContentManager content;

        static bool isEscMenu;

        public bool IsEscMenu { get { return isEscMenu; } set { isEscMenu = value; } }
        public bool IsShopMenu { get; set; }

        public bool DrawTileSelector { get; set; } = true;

        public GraphicsDevice GraphicsDevice { get; set; }
        public Game1 Game { get; set; }
        public EscMenu Esc { get; set; }
        public ShopMenu ShopMenu { get; set; }
        internal ToolBar BottomBar { get; set; }

        public Camera2D cam;
        public GraphicsDevice graphics { get; set; }

        public int TileSelectorX { get; set; } = 0;
        public int TileSelectorY { get; set; } = 0;

        public Vector2 Origin { get; set; } = new Vector2(0, 0);

        //keyboard



        private UserInterface()
        {

        }
        public UserInterface(GraphicsDevice graphicsDevice, ContentManager content, Camera2D cam )
        {
            this.GraphicsDevice = graphicsDevice;
            this.content = content;
            isEscMenu = false;
            
            BottomBar = new ToolBar( graphicsDevice, content);
            Esc = new EscMenu(graphicsDevice, content);
            this.ShopMenu = new ShopMenu("ToolShop", graphicsDevice);
            ShopMenu.TryAddStock(3, 1);
            ShopMenu.TryAddStock(0, 1);
            ShopMenu.TryAddStock(1, 1);
            ShopMenu.TryAddStock(147, 1);
            ShopMenu.TryAddStock(2, 1);
            ShopMenu.TryAddStock(124, 1);
            ShopMenu.TryAddStock(125, 1);
            ShopMenu.TryAddStock(127, 1);
            //ShopMenu.TryAddStock(3, 1);
            //ShopMenu.TryAddStock(2, 1);
            //ShopMenu.TryAddStock(9, 5);
            //ShopMenu.TryAddStock(6, 1);
            //ShopMenu.TryAddStock(12, 1);
            this.cam = cam;
            
        }


        public void Update(GameTime gameTime, KeyboardState kState, KeyboardState oldKeyState, Inventory inventory, MouseManager mouse)
        {
            BottomBar.Update(gameTime, inventory, mouse);

            if ((oldKeyState.IsKeyDown(Keys.Escape)) && (kState.IsKeyUp(Keys.Escape)) && !IsShopMenu)
            {
                isEscMenu = !isEscMenu;

                if (isEscMenu == false)
                {
                    Esc.isTextChanged = false;
                }
                    

            }


            if ((oldKeyState.IsKeyDown(Keys.P)) && (kState.IsKeyUp(Keys.P)) && !isEscMenu)
            {
                IsShopMenu = !IsShopMenu;
            }

            if((oldKeyState.IsKeyDown(Keys.Escape)) && (kState.IsKeyUp(Keys.Escape)))
            {
                IsShopMenu = false;
               
                //isEscMenu = false;
            }

            if(IsShopMenu)
            {
                ShopMenu.Update(gameTime,mouse);
                Game1.isMyMouseVisible = true;
                Game1.freeze = true;
            }


            if (isEscMenu)
            {
                Esc.Update(gameTime, mouse);
                Game1.freeze = true;
            }
            if(!isEscMenu && !IsShopMenu)
            {
                Game1.freeze = false;

            }
                
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Begin(SpriteSortMode.FrontToBack);


            
            

            BottomBar.Draw(spriteBatch);
            if(isEscMenu)
            {
                Esc.Draw(spriteBatch);
            }

            if(IsShopMenu)
            {
                ShopMenu.Draw(spriteBatch);
            }
            spriteBatch.DrawString(Game1.AllTextures.MenuText, Game1.Player.Inventory.Money.ToString(), new Vector2(340, 645), Color.Red, 0f, Origin, 1f, SpriteEffects.None, layerDepth: .71f);

            spriteBatch.End();

        }
    }
}
