﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public class ShopMenu
    {
        private List<Button> allShopMenuItemButtons;
        //private Button shopMenuItemButton;
        private Button redEsc;
        private SpriteFont mainFont;
        public string Name;
        public Vector2 ShopMenuPosition;

        public SpriteFont Font;

        public Inventory ShopInventory;

        public ShopMenu(string name, GraphicsDevice graphicsDevice)
        {
            //this.shopMenuItemButton = new Button(Game1.AllTextures.ShopMenuItemButton, graphicsDevice, new Vector2(Utility.centerScreenX, Utility.centerScreenY));
            this.Name = name;
            this.redEsc = new Button(Game1.AllTextures.RedEsc, graphicsDevice, new Vector2(Utility.centerScreenX, Utility.centerScreenY));
            this.mainFont = Game1.AllTextures.MenuText;
            ShopMenuPosition = new Vector2(Utility.centerScreenX - 432, Utility.centerScreenY - 270);

            Font = Game1.AllTextures.MenuText;

            allShopMenuItemButtons = new List<Button>();

            int menuItemOffsetX = 0;
            int menuItemOffsetY = 0;

            ShopInventory = new Inventory(25);

            for (int i = 1; i <= 25; i++)
            {                
                allShopMenuItemButtons.Add(new Button(Game1.AllTextures.ShopMenuItemButton, graphicsDevice, new Vector2(ShopMenuPosition.X + 32 + menuItemOffsetX * 194,
                    ShopMenuPosition.Y + 32 + 100 * menuItemOffsetY)));
                if(i%5 == 0)
                {
                    menuItemOffsetX = -1;
                    menuItemOffsetY++;
                }
                    menuItemOffsetX++;             
            }
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            for (int i = 0; i < allShopMenuItemButtons.Count; i++)
            {
                if (ShopInventory.currentInventory.ElementAt(i) == null)
                {
                    allShopMenuItemButtons[i].ItemCounter = 0;
                }
                else
                {
                    allShopMenuItemButtons[i].ItemCounter = ShopInventory.currentInventory.ElementAt(i).SlotItems.Count;
                }

                if (allShopMenuItemButtons[i].ItemCounter != 0)
                {
                    allShopMenuItemButtons[i].Texture = ShopInventory.currentInventory.ElementAt(i).SlotItems[0].Texture;
                }
                else
                {
                    allShopMenuItemButtons[i].Texture = Game1.AllTextures.ShopMenuItemButton;
                }

                allShopMenuItemButtons[i].Update(mouse);

                //Make a transaction
                if(allShopMenuItemButtons[i].isClicked && allShopMenuItemButtons[i].ItemCounter != 0)
                {
                    if(Game1.Player.Inventory.Money >= ShopInventory.currentInventory.ElementAt(i).SlotItems[0].Price)
                    {
                        Game1.Player.Inventory.TryAddItem(ShopInventory.currentInventory.ElementAt(i).SlotItems[0]);
                        Game1.Player.Inventory.Money -= ShopInventory.currentInventory.ElementAt(i).SlotItems[0].Price; //reduce players money if transaction goes through!
                        ShopInventory.currentInventory.ElementAt(i).RemoveItemFromSlot();
                        allShopMenuItemButtons[i].ItemCounter--;
                    }
                    //ShopInventory.currentInventory
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.ShopMenu,ShopMenuPosition , Color.White);
            for (int i = 0; i < allShopMenuItemButtons.Count; i++)
            {
                if(allShopMenuItemButtons[i].ItemCounter != 0)
                {
                   allShopMenuItemButtons[i].Draw(spriteBatch, Font, allShopMenuItemButtons[i].ItemCounter.ToString(), allShopMenuItemButtons[i].Position, Color.White,
                    new Vector2(allShopMenuItemButtons[i].Position.X +45, allShopMenuItemButtons[i].Position.Y + 80), ShopInventory.currentInventory.ElementAt(i).SlotItems[0].Price);
                }
                else
                {
                    allShopMenuItemButtons[i].Draw(spriteBatch, Font, allShopMenuItemButtons[i].ItemCounter.ToString(), allShopMenuItemButtons[i].Position, Color.White);
                }
                
            }
        }

        public void TryAddStock(int id, int amountToAdd)
        {
            for(int i = 0; i < amountToAdd; i++)
            {
                this.ShopInventory.TryAddItem(Game1.ItemVault.GenerateNewItem(id, null, false));
            }
            
        }


    }
}