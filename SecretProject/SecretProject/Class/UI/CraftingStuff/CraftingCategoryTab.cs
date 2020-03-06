using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MenuStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.ItemStuff;
using XMLData.ItemStuff.CraftingStuff;

namespace SecretProject.Class.UI.CraftingStuff
{
    public class CraftingCategoryTab
    {
        public CraftingWindow CraftingWindow { get; set; }
        public CraftingCategory Category { get; set; }
        public GraphicsDevice Graphics { get; set; }
        public Button TabButton { get; set; }

        public float Scale { get; set; }

        public CraftingCategoryTab(CraftingWindow craftingWindow,  Vector2 position)
        {
            this.CraftingWindow = craftingWindow;
            this.Graphics = craftingWindow.Graphics;
            this.Scale = craftingWindow.Scale;
            this.TabButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(272, 384 * (int)this.Category, 32, 32), this.Graphics,
                position, Controls.CursorType.Normal, this.Scale);
        }
        public void Update(GameTime gameTime)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
