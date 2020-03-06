﻿using Microsoft.Xna.Framework;
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

        public List<RecipeContainer> RecipeContainers { get; set; }

        public CraftingCategoryTab(CraftingWindow craftingWindow,CraftingCategory craftingCategory,  Vector2 position)
        {
            this.CraftingWindow = craftingWindow;
            this.Graphics = craftingWindow.Graphics;
            this.Scale = craftingWindow.Scale;
            this.TabButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(272, 384 * (int)this.Category, 32, 32), this.Graphics,
                position, Controls.CursorType.Normal, this.Scale);

            this.Category = craftingCategory;

            CraftingPage craftingPage = craftingWindow.CraftingGuide.CraftingPages.Find(x => x.CategoryName == this.Category);

            this.RecipeContainers = new List<RecipeContainer>();


            int row = 0;
            int column = 0;
            for (int i =0; i < craftingPage.CraftingRecipes.Count; i++)
            {
                if(column % 5 == 0)
                {
                    row++;
                    column = 0;
                }
                this.RecipeContainers.Add(new RecipeContainer(craftingWindow, craftingPage.CraftingRecipes[i],
                    new Vector2(CraftingWindow.Position.X + column * 32 * Scale, CraftingWindow.Position.Y + row * 32 * Scale)));
            }
           
        }
        public void Update(GameTime gameTime)
        {
            for(int i =0; i < this.RecipeContainers.Count; i++)
            {
                this.RecipeContainers[i].Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.RecipeContainers.Count; i++)
            {
                this.RecipeContainers[i].Draw(spriteBatch);
            }
        }
    }
}
