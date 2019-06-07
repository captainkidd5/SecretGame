using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ShopStuff
{
    public interface IShop
    {
        int ID { get; set; }
        string Name { get; set; }
        ShopMenu ShopMenu { get; set; }
        bool IsActive { get; set; }

        void Update(GameTime gameTime, MouseManager mouse);

        void Draw(SpriteBatch spriteBatch);

    }
}
