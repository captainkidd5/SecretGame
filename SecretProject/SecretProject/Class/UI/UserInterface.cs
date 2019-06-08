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
using SecretProject.Class.Playable;

namespace SecretProject.Class.UI
{
    public enum OpenShop
    {
        None = 0,
        ToolShop = 1
        //LodgeInteior = 1,
        //Iliad = 2,
        //Exit = 3,
        //Sea = 4,
        //RoyalDock = 5
    }
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
        //public ShopMenu ShopMenu { get; set; }
        internal ToolBar BottomBar { get; set; }

        public Camera2D cam;
        public GraphicsDevice graphics { get; set; }

        public int TileSelectorX { get; set; } = 0;
        public int TileSelectorY { get; set; } = 0;

        public Vector2 Origin { get; set; } = new Vector2(0, 0);

        public TextBuilder TextBuilder { get; set; }


        public Player Player { get; set; }

        public OpenShop CurrentOpenShop { get; set; } = OpenShop.None;

        //keyboard



        private UserInterface()
        {

        }
        public UserInterface(Player player, GraphicsDevice graphicsDevice, ContentManager content, Camera2D cam )
        {
            this.GraphicsDevice = graphicsDevice;
            this.content = content;
            isEscMenu = false;
            
            BottomBar = new ToolBar( graphicsDevice, content);
            Esc = new EscMenu(graphicsDevice, content);
            //this.ShopMenu = new ShopMenu("ToolShop", graphicsDevice);
            //ShopMenu.TryAddStock(3, 1);
            //ShopMenu.TryAddStock(0, 1);
            //ShopMenu.TryAddStock(1, 1);
            //ShopMenu.TryAddStock(147, 1);
            //ShopMenu.TryAddStock(2, 1);
            //ShopMenu.TryAddStock(122, 1);
            //ShopMenu.TryAddStock(124, 1);
            //ShopMenu.TryAddStock(125, 1);
            //ShopMenu.TryAddStock(127, 1);
            //ShopMenu.TryAddStock(128, 10);
            //ShopMenu.TryAddStock(141, 1);
            //ShopMenu.TryAddStock(143, 3);
            //ShopMenu.TryAddStock(161, 5);
            //ShopMenu.TryAddStock(145, 1);
            //ShopMenu.TryAddStock(165, 1);
            //ShopMenu.TryAddStock(167, 1);
           // ShopMenu.TryAddStock(165, 1);
           // ShopMenu.TryAddStock(165, 1);
            this.cam = cam;
            TextBuilder = new TextBuilder("", .1f, 5f);
            this.Player = player;
        }

        //public void CloseOtherMenus(bool exemption)
        //{
        //    IsShopMenu = false;
        //    IsEscMenu = false;

        //    exemption = true;
        //}

        public void Update(GameTime gameTime, KeyboardState oldKeyState, KeyboardState newKeyState, Inventory inventory, MouseManager mouse)
        {
            BottomBar.Update(gameTime, inventory, mouse);
            //if (!IsShopMenu)
            //{
                
            //}

            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Escape)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Escape)) && !IsShopMenu)
            {
                isEscMenu = !isEscMenu;

                if (isEscMenu == false)
                {
                    Esc.isTextChanged = false;
                }
                    

            }


            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.P)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.P)) && !isEscMenu)
            {
                IsShopMenu = !IsShopMenu;
               Game1.AllShops.Find(x => x.ID == 0).IsActive = IsShopMenu;
                if(!IsShopMenu)
                {
                    Player.UserInterface.CurrentOpenShop = OpenShop.None;
                }
                //Player.UserInterface.CurrentOpenShop = OpenShop.ToolShop;

                
            }

            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.T)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.T)) && !isEscMenu)
            {
                TextBuilder.IsActive = !TextBuilder.IsActive;
            }


                TextBuilder.Update(gameTime);


            //if ((oldKeyState.IsKeyDown(Keys.Escape)) && (kState.IsKeyUp(Keys.Escape)))
            //{
            //    IsShopMenu = false;
               
            //    //isEscMenu = false;
            //}
            

            //if(IsShopMenu)
            //{
            //    ShopMenu.Update(gameTime,mouse);
            //    Game1.isMyMouseVisible = true;
            //    Game1.freeze = true;
            //}

            if(IsShopMenu)
            {
                for (int i = 0; i < Game1.AllShops.Count; i++)
                {
                    if (Game1.AllShops[i].IsActive)
                    {
                        Game1.isMyMouseVisible = true;
                        Game1.freeze = true;
                        Game1.AllShops[i].Update(gameTime, mouse);
                    }
                }
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
                for (int i = 0; i < Game1.AllShops.Count; i++)
                {
                    if (Game1.AllShops[i].IsActive)
                    {
                        Game1.AllShops[i].Draw(spriteBatch);
                    }
                }
            }
            


                TextBuilder.Draw(spriteBatch, .71f);

            spriteBatch.DrawString(Game1.AllTextures.MenuText, Game1.Player.Inventory.Money.ToString(), new Vector2(340, 645), Color.Red, 0f, Origin, 1f, SpriteEffects.None, layerDepth: .71f);

            spriteBatch.End();

        }
    }
}
