using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.UI.ButtonStuff;
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

        public bool CanCraft { get; set; }

        public List<ExternalToolTip> ToolTips { get; set; }

        public RecipeContainer(CraftingWindow craftingWindow, ItemRecipe itemRecipe, Vector2 position)
        {
            this.CraftingWindow = craftingWindow;
            this.ItemRecipe = itemRecipe;
            this.Unlocked = itemRecipe.Unlocked;
            this.Item = Game1.ItemVault.GenerateNewItem(this.ItemRecipe.ItemToCraftID, null);
            this.ItemButton = new Button(Game1.AllTextures.ItemSpriteSheet, Item.SourceTextureRectangle, craftingWindow.Graphics,
                position, Controls.CursorType.Normal, craftingWindow.Scale, this.Item);
            this.ToolTips = new List<ExternalToolTip>();
            Vector2 tooltipsPosition = new Vector2(craftingWindow.ExternalCraftingWindow.Position.X, craftingWindow.ExternalCraftingWindow.Position.Y + 96);
            for (int i = 0; i < this.ItemRecipe.AllItemsRequired.Count; i++)
            {
                this.ToolTips.Add(new ExternalToolTip(craftingWindow, this.ItemRecipe.AllItemsRequired[i].ItemID, new Vector2(tooltipsPosition.X + 32 + 24 * craftingWindow.Scale * i, tooltipsPosition.Y), this.ItemRecipe.AllItemsRequired[i].Count));
            }
        }

        public void Update(GameTime gameTime)
        {
            this.ItemButton.Update(Game1.MouseManager);
            this.CanCraft = CheckIfCanCraft();
            if (this.ItemButton.isClicked)
            {
                CraftingWindow.ExternalCraftingWindow.IsActive = true;
                CraftingWindow.ExternalCraftingWindow.CurrentRecipe = this;
                CraftingWindow.ExternalCraftingWindow.Item = this.Item;
                CraftingWindow.ExternalCraftingWindow.ItemToCraftButton.Item = this.Item;
                CraftingWindow.ExternalCraftingWindow.ItemToCraftButton.ChangeItemTexture(this.Item);
            }
        }

        public void UpdateToolTips(GameTime gameTime)
        {
            for (int i = 0; i < this.ToolTips.Count; i++)
            {
                ToolTips[i].Update(gameTime);
            }
        }
        public void DrawToolTips(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.ToolTips.Count; i++)
            {
                ToolTips[i].Draw(spriteBatch);
            }
        }

        public bool CheckIfCanCraft()
        {
            this.CanCraft = true;
            for (int i = 0; i < this.ToolTips.Count; i++)
            {
                if (!this.ToolTips[i].UpdateItemCount())
                {
                    this.CanCraft = false;

                }

            }
            return this.CanCraft;


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.ItemButton.Draw(spriteBatch);
        }
    }
}
