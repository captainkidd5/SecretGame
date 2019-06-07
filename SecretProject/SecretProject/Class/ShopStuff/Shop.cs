using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.UI;

namespace SecretProject.Class.ShopStuff
{
    public class Shop : IShop
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ShopMenu ShopMenu { get; set; }
        public bool IsActive { get; set; } = false;

        public Shop(GraphicsDevice graphics, int id, string name, ShopMenu shopMenu)
        {
            this.ID = id;
            this.Name = name;
            this.ShopMenu = shopMenu;
            
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            ShopMenu.Update(gameTime, mouse);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ShopMenu.Draw(spriteBatch);
        }
    }
}
