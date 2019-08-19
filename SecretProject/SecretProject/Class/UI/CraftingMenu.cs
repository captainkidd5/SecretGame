using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.ItemStuff;

namespace SecretProject.Class.UI
{
    public class CraftingMenu
    {
        //LAYER1
       
        public List<CraftableRecipeBar> CraftingRecipeBars { get; set; }
        public bool IsActive { get; set; }
        public Texture2D BackDropTexture { get; set; }
        public Vector2 BackDropPosition { get; set; }

        GraphicsDevice graphics;

        public CraftingGuide CraftingGuide { get; set; }

        public CraftingMenu()
        {
            
            this.IsActive = false;
           

           
        }

        public void LoadContent(ContentManager content, GraphicsDevice graphics)
        {
            this.graphics = graphics;
            CraftingGuide = content.Load<CraftingGuide>("Item/Crafting/CraftingGuide");
            this.BackDropTexture = Game1.AllTextures.UserInterfaceTileSet;
            this.BackDropPosition = new Vector2(500, 100);
            CraftingRecipeBars = new List<CraftableRecipeBar>()
            {
                new CraftableRecipeBar(CraftingGuide,124,new Vector2(BackDropPosition.X + BackDropTexture.Width / 5, BackDropPosition.Y + BackDropTexture.Height/5), graphics)
            };
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
           if(this.IsActive)
            {
                foreach(CraftableRecipeBar bar in CraftingRecipeBars)
                {
                    bar.Update(gameTime, mouse);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
           if(this.IsActive)
            {
                spriteBatch.Draw(this.BackDropTexture, BackDropPosition, new Rectangle(1104, 1120, 288, 336), Color.White);
                foreach (CraftableRecipeBar bar in CraftingRecipeBars)
                {
                    bar.Draw(spriteBatch);
                }
            }
            
        }
    }

    public class CraftingSlot
    {
        public int ItemID { get; set; }
        public Button Button { get; set; }
        Vector2 drawPosition;
        int countOfItemsRequired;
        public CraftingSlot(GraphicsDevice graphics,int countOfItemsRequired, int itemID, Vector2 drawPosition)
        {
            Item item = Game1.ItemVault.GenerateNewItem(itemID, null);
            Button = new Button(item.ItemSprite.AtlasTexture, item.SourceTextureRectangle, graphics, drawPosition);
            this.countOfItemsRequired = countOfItemsRequired;
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            Button.Update(mouse);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Button.Draw(spriteBatch, Button.ItemSourceRectangleToDraw, new Rectangle(1167, 752, 64, 64), Game1.AllTextures.MenuText, countOfItemsRequired.ToString(), drawPosition, Color.White *.5f);
        }
    }

    public class CraftableRecipeBar
    {
        public List<CraftingSlot> CraftingSlots{ get; set; }
        CraftingGuide guide;
        public CraftableRecipeBar(CraftingGuide guide, int itemID, Vector2 drawPosition, GraphicsDevice graphics)
        {
            this.guide = guide;
            CraftingSlots = new List<CraftingSlot>();
            for(int i = 0; i < guide.CraftingRecipes.Find(x => x.ItemToCraftID == Game1.ItemVault.GenerateNewItem(itemID, null, false).ID).AllItemsRequired.Count; i++)
            {
                CraftingSlots.Add(new CraftingSlot(graphics,guide.CraftingRecipes.Find(x => x.ItemToCraftID == Game1.ItemVault.GenerateNewItem(itemID, null, false).ID).AllItemsRequired[i].Count,
                    guide.CraftingRecipes.Find(x => x.ItemToCraftID == Game1.ItemVault.GenerateNewItem(itemID, null, false).ID).AllItemsRequired[i].ItemID, drawPosition));
            }
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            foreach(CraftingSlot slot in CraftingSlots)
            {
                slot.Update(gameTime, mouse);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (CraftingSlot slot in CraftingSlots)
            {
                slot.Draw(spriteBatch);
            }
        }
    }


}
