﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.UI.ButtonStuff;
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

        public List<RecipeTileManager> RecipeTileManagers { get; set; }

        public CraftingCategoryTab(CraftingWindow craftingWindow,CraftingCategory craftingCategory,  Vector2 position)
        {
            this.CraftingWindow = craftingWindow;
            this.Graphics = craftingWindow.Graphics;
            this.Scale = craftingWindow.Scale;
            this.Category = craftingCategory;
            this.TabButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(272, 384 + 32 * (int)this.Category, 32, 32), this.Graphics,
                position, Controls.CursorType.Normal, this.Scale);

            this.Category = craftingCategory;

            CraftingPage craftingPage = craftingWindow.CraftingGuide.CraftingPages.Find(x => x.CategoryName == this.Category);

            this.RecipeTileManagers = new List<RecipeTileManager>();


            int row = 0;
            int column = 0;
            for (int i =0; i < craftingPage.CraftingRecipes.Count; i++)
            {
                column++;
                if(column % 5 == 0)
                {
                    row++;
                    column = 1;
                }
                this.RecipeTileManagers.Add(new RecipeTileManager(craftingWindow, craftingPage.CraftingRecipes[i],
                    new Vector2(CraftingWindow.Position.X + column * 16 * Scale, CraftingWindow.Position.Y + 32+ row * 16 * Scale)));
            }
           
        }
        public void Update(GameTime gameTime)
        {
            for(int i =0; i < this.RecipeTileManagers.Count; i++)
            {
                this.RecipeTileManagers[i].Update(gameTime);
            }
            //if(Game1.Player.Inventory.HasChangedSinceLastFrame)
            //{
                UpdateToolTips();
           // }
        }
        public void UpdateToolTips()
        {
            for (int i = 0; i < this.RecipeTileManagers.Count; i++)
            {
                this.RecipeTileManagers[i].CheckIfCanCraft();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.RecipeTileManagers.Count; i++)
            {
                this.RecipeTileManagers[i].Draw(spriteBatch);
            }
        }
    }
}
