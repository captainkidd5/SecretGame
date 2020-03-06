using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.ItemStuff.CraftingStuff;

namespace SecretProject.Class.UI.CraftingStuff
{
    public class RecipeContainer
    {
        public CraftingWindow CraftingWindow { get; set; }
        public Item Item { get; set; }
        public ItemRecipe ItemRecipe { get; set; }
        public Button ItemButton { get; set; }

        public bool Unlocked { get; set; }

        public RecipeContainer(CraftingWindow craftingWindow, ItemRecipe itemRecipe, Vector2 position)
        {
            this.CraftingWindow = craftingWindow;
            this.ItemRecipe = itemRecipe;
            this.Unlocked = itemRecipe.Unlocked;
            this.Item = Game1.ItemVault.GenerateNewItem(this.ItemRecipe.ItemToCraftID, null);
            this.ItemButton = new Button(Game1.AllTextures.ItemSpriteSheet, Item.SourceTextureRectangle, craftingWindow.Graphics,
                position, Controls.CursorType.Normal, craftingWindow.Scale, this.Item);
        }

        public void Update(GameTime gameTime)
        {
            this.ItemButton.Update(Game1.MouseManager);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.ItemButton.Draw(spriteBatch);
        }
    }
}
