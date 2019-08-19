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
                new CraftableRecipeBar(CraftingGuide,4,new Vector2(BackDropPosition.X + BackDropTexture.Width / 5, BackDropPosition.Y + BackDropTexture.Height/5), graphics)
            };
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
           if(this.IsActive)
            {
                foreach(CraftableRecipeBar bar in CraftingRecipeBars)
                {
                    
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
           if(this.IsActive)
            {
                spriteBatch.Draw(this.BackDropTexture, BackDropPosition, new Rectangle(1104, 1120, 288, 336), Color.White);
            }
            
        }
    }

    public class CraftingSlot
    {
        public int ItemID { get; set; }
        public Button Button { get; set; }
        CraftingGuide guide;
        public CraftingSlot(CraftingGuide guide, int itemID, Vector2 drawPosition, GraphicsDevice graphics)
        {
            Item item = Game1.ItemVault.GenerateNewItem(itemID, drawPosition);
            Button = new Button(item.ItemSprite.AtlasTexture, item.SourceTextureRectangle, graphics, drawPosition);
            this.guide = guide;
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            Button.Update(mouse);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
           // Button.Draw(spriteBatch, Button.ItemSourceRectangleToDraw, new Rectangle(1167, 752, 64,64 ), Game1.AllTextures.MenuText, guide.CraftingRecipes.Find(x => x.)
        }
    }

    public class CraftableRecipeBar
    {
        public List<CraftingSlot> CraftingSlots{ get; set; }
        CraftingGuide guide;
        public CraftableRecipeBar(CraftingGuide guide, int itemID, Vector2 drawPosition, GraphicsDevice graphics)
        {
            this.guide = guide;
            for(int i = 0; i < guide.CraftingRecipes.Find(x => x.ItemToCraftID == Game1.ItemVault.GenerateNewItem(itemID, null, false).ID).AllItemsRequired.Count; i++)
            {
                CraftingSlots.Add(new CraftingSlot(guide, itemID, ))
            }
        }
    }


}
