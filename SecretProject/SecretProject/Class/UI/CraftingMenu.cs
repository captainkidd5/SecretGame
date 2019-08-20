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
                new CraftableRecipeBar(CraftingGuide,124,new Vector2(BackDropPosition.X + 32, BackDropPosition.Y + 64), graphics)
            };
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            if (this.IsActive)
            {
                foreach (CraftableRecipeBar bar in CraftingRecipeBars)
                {
                    bar.Update(gameTime, mouse);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsActive)
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
        public Vector2 drawPosition;
        public int countOfItemsRequired;
        public bool satisfied = false;
        public float colorMultiplier;
        public CraftingSlot(GraphicsDevice graphics, int countOfItemsRequired, int itemID, Vector2 drawPosition)
        {
            Item item = Game1.ItemVault.GenerateNewItem(itemID, null);
            this.ItemID = itemID;
            Button = new Button(item.ItemSprite.AtlasTexture, item.SourceTextureRectangle, graphics, drawPosition);
            this.countOfItemsRequired = countOfItemsRequired;
            this.drawPosition = drawPosition;
            this.colorMultiplier = .25f;
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            Button.Update(mouse);
            this.satisfied = CheckIfPlayerHasRequiredItems();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Button.DrawCraftingSlot(spriteBatch, Button.BackGroundSourceRectangle, new Rectangle(1167, 752, 64, 64), Game1.AllTextures.MenuText,
                Game1.Player.Inventory.FindNumberOfItemInInventory(ItemID).ToString() + "/" + countOfItemsRequired.ToString(), drawPosition, Color.White * colorMultiplier, 1f, 5f);
        }

        public bool CheckIfPlayerHasRequiredItems()
        {
            if (Game1.Player.Inventory.FindNumberOfItemInInventory(ItemID) >= countOfItemsRequired)
            {
                this.colorMultiplier = 1f;
                return true;
            }
            else
            {
                this.colorMultiplier = .5f;
                return false;
            }
        }
    }

    public class CraftableRecipeBar
    {
        public List<CraftingSlot> CraftingSlots { get; set; }
        CraftingGuide guide;
        Button retrievableButton;
        int tier;
        int itemID;
        bool canCraft = false;
        public CraftableRecipeBar(CraftingGuide guide, int itemID, Vector2 drawPosition, GraphicsDevice graphics)
        {
            this.guide = guide;
            this.itemID = itemID;
            CraftingSlots = new List<CraftingSlot>();
            //number of separate items required to craft said item.
            this.tier = guide.CraftingRecipes.Find(x => x.ItemToCraftID == Game1.ItemVault.GenerateNewItem(itemID, null, false).ID).AllItemsRequired.Count;
            for (int i = 0; i < tier; i++)
            {
                CraftingSlots.Add(new CraftingSlot(graphics, guide.CraftingRecipes.Find(x => x.ItemToCraftID == Game1.ItemVault.GenerateNewItem(itemID, null, false).ID).AllItemsRequired[i].Count,
                    guide.CraftingRecipes.Find(x => x.ItemToCraftID == Game1.ItemVault.GenerateNewItem(itemID, null, false).ID).AllItemsRequired[i].ItemID, new Vector2(drawPosition.X + (i * 100), drawPosition.Y)));
            }

            retrievableButton = new Button(Game1.ItemVault.GenerateNewItem(itemID, null).ItemSprite.AtlasTexture,
                Game1.ItemVault.GenerateNewItem(itemID, null).SourceTextureRectangle, graphics, new Vector2(CraftingSlots[tier - 1].drawPosition.X + 264, CraftingSlots[tier - 1].drawPosition.Y));
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            canCraft = true;
            foreach (CraftingSlot slot in CraftingSlots)
            {
                slot.Update(gameTime, mouse);
                if(!slot.satisfied)
                {
                    canCraft = false;
                }
            }

            retrievableButton.Update(mouse);


            if(retrievableButton.isClicked && canCraft)
            {
                Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem(itemID, null));

                foreach (CraftingSlot slot in CraftingSlots)
                {
                    for(int i =0; i < slot.countOfItemsRequired; i++)
                    {
                        Game1.Player.Inventory.RemoveItem(slot.ItemID);
                    }
                }
                

            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            

            for(int i = 0; i < CraftingSlots.Count; i++)
            {
                CraftingSlots[i].Draw(spriteBatch);
                if(i == CraftingSlots.Count - 1)
                {
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(CraftingSlots[i].drawPosition.X + 64, CraftingSlots[i].drawPosition.Y + 16),
                new Rectangle(80, 304, 32, 32), Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                }
                else
                {
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(CraftingSlots[i].drawPosition.X + 64, CraftingSlots[i].drawPosition.Y + 16),
                new Rectangle(128, 304, 32, 32), Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                }
                
            }
            //retrievableButton.Draw(spriteBatch);
            retrievableButton.DrawCraftingSlotRetrievable(spriteBatch, retrievableButton.ItemSourceRectangleToDraw, retrievableButton.BackGroundSourceRectangle, Color.White, 1f, 5f, Game1.Utility.StandardButtonDepth);
            //spriteBatch.Draw(Game1.ItemVault.GenerateNewItem(itemID, null).ItemSprite.AtlasTexture, new Vector2(CraftingSlots[tier - 1].drawPosition.X + 264, CraftingSlots[tier - 1].drawPosition.Y), Game1.ItemVault.GenerateNewItem(itemID, null).SourceTextureRectangle, Color.White);




        }
    }


}
