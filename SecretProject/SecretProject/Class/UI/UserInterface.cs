﻿using System;
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
using SecretProject.Class.DialogueStuff;
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

        public TextBuilder TextBuilder { get; set; }
        public bool IsTextBuilderActive { get; set; } = false;

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
            ShopMenu.TryAddStock(122, 1);
            ShopMenu.TryAddStock(124, 1);
            ShopMenu.TryAddStock(125, 1);
            ShopMenu.TryAddStock(127, 1);
            ShopMenu.TryAddStock(128, 10);
            ShopMenu.TryAddStock(141, 1);
            ShopMenu.TryAddStock(143, 3);
            ShopMenu.TryAddStock(161, 5);
            ShopMenu.TryAddStock(145, 1);
            ShopMenu.TryAddStock(165, 1);
            ShopMenu.TryAddStock(167, 1);
           // ShopMenu.TryAddStock(165, 1);
           // ShopMenu.TryAddStock(165, 1);
            this.cam = cam;
            TextBuilder = new TextBuilder("", .1f);
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

            if ((oldKeyState.IsKeyDown(Keys.T)) && (kState.IsKeyUp(Keys.T)) && !isEscMenu)
            {
                TextBuilder.IsActive = !TextBuilder.IsActive;
            }

            if(TextBuilder.IsActive)
            {
                TextBuilder.Update(gameTime);
            }

            if ((oldKeyState.IsKeyDown(Keys.Escape)) && (kState.IsKeyUp(Keys.Escape)))
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
            
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);


            
            

            BottomBar.Draw(spriteBatch);
            if(isEscMenu)
            {
                Esc.Draw(spriteBatch);
            }

            if(IsShopMenu)
            {
                ShopMenu.Draw(spriteBatch);
            }
            if(TextBuilder.IsActive)
            {
                TextBuilder.Draw(spriteBatch);
            }
            spriteBatch.DrawString(Game1.AllTextures.MenuText, Game1.Player.Inventory.Money.ToString(), new Vector2(340, 645), Color.Red, 0f, Origin, 1f, SpriteEffects.None, layerDepth: .71f);

            spriteBatch.End();

        }
    }
}
