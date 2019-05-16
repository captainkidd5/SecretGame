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
        public Vector2 ShopMenuPosition;

        public ShopMenu(string name, GraphicsDevice graphicsDevice)
        {
            //this.shopMenuItemButton = new Button(Game1.AllTextures.ShopMenuItemButton, graphicsDevice, new Vector2(Utility.centerScreenX, Utility.centerScreenY));
            this.Name = name;
            this.redEsc = new Button(Game1.AllTextures.RedEsc, graphicsDevice, new Vector2(Utility.centerScreenX, Utility.centerScreenY));
            this.mainFont = Game1.AllTextures.MenuText;
            ShopMenuPosition = new Vector2(Utility.centerScreenX - 432, Utility.centerScreenY - 270);

            allShopMenuItemButtons = new List<Button>();

            int menuItemOffsetX = 0;
            int menuItemOffsetY = 0;

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
            for(int i = 0; i < allShopMenuItemButtons.Count; i++)
            {
                allShopMenuItemButtons[i].Update(mouse);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.ShopMenu,ShopMenuPosition , Color.White);
            for (int i = 0; i < allShopMenuItemButtons.Count; i++)
            {
                allShopMenuItemButtons[i].Draw(spriteBatch);
            }
        }


    }
}
