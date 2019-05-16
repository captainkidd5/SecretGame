using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
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

        public ShopMenu(string name, GraphicsDevice graphicsDevice)
        {
            //this.shopMenuItemButton = new Button(Game1.AllTextures.ShopMenuItemButton, graphicsDevice, new Vector2(Utility.centerScreenX, Utility.centerScreenY));
            this.Name = name;
            this.redEsc = new Button(Game1.AllTextures.RedEsc, graphicsDevice, new Vector2(Utility.centerScreenX, Utility.centerScreenY));
            this.mainFont = Game1.AllTextures.MenuText;

            allShopMenuItemButtons = new List<Button>();

            for(int i = 0; i < 16; i++)
            {

                allShopMenuItemButtons.Add(new Button(Game1.AllTextures.ShopMenuItemButton, graphicsDevice, new Vector2(Utility.centerScreenX + i * 64, Utility.centerScreenY)));
                if(i >= 4 && i < 8)
                {
                    allShopMenuItemButtons[i].Rectangle.X = Utility.centerScreenY + (i * 8 - i) + 46;
                    allShopMenuItemButtons[i].Rectangle.Y = Utility.centerScreenY + 46;
                }
                if (i >= 8 && i < 12)
                {
                    allShopMenuItemButtons[i].Rectangle.Y = Utility.centerScreenY + 46 * 2;
                }

            }
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            for(int i = 0; i < allShopMenuItemButtons.Count; i++)
            {
                allShopMenuItemButtons[i].Update(mouse);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.ShopMenu, new Vector2(Utility.centerScreenX - 432, Utility.centerScreenY - 270), Color.White);
            for (int i = 0; i < allShopMenuItemButtons.Count; i++)
            {
                allShopMenuItemButtons[i].Draw(spriteBatch);
            }
        }


    }
}
