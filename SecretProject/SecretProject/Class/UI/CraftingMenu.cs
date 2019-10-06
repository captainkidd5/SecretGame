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
    public class CraftingMenu : IExclusiveInterfaceComponent
    {

        public List<CraftableRecipeBar> CraftingRecipeBars { get; set; }
        public bool IsActive { get; set; }
        public Texture2D BackDropTexture { get; set; }
        public Vector2 BackDropPosition { get; set; }
        public Rectangle BackDropSourceRectangle { get; set; }
        public Rectangle TabSourceRectangle { get; set; }

        GraphicsDevice graphics;

        public CraftingGuide CraftingGuide { get; set; }
        public List<Tab> Tabs { get; set; }
        public Button RedEsc { get; set; }
        public bool FreezesGame { get; set; }

        int currentPage;

        public CraftingMenu()
        {

            this.IsActive = false;
            this.FreezesGame = false;
            

        }

        public void LoadContent(ContentManager content, GraphicsDevice graphics)
        {
            this.graphics = graphics;
            CraftingGuide = content.Load<CraftingGuide>("Item/Crafting/CraftingGuide");
            this.BackDropTexture = Game1.AllTextures.UserInterfaceTileSet;
            this.BackDropPosition = new Vector2(500, 100);
            this.TabSourceRectangle = new Rectangle(800, 48, 64, 64);
            CraftingRecipeBars = new List<CraftableRecipeBar>()
            {

                new CraftableRecipeBar(0,CraftingGuide,124,new Vector2(BackDropPosition.X + 32, BackDropPosition.Y + 64), graphics,TabSourceRectangle),
                new CraftableRecipeBar(0,CraftingGuide,3,new Vector2(BackDropPosition.X + 32, BackDropPosition.Y + 128), graphics,TabSourceRectangle),
                new CraftableRecipeBar(0,CraftingGuide,121,new Vector2(BackDropPosition.X + 32, BackDropPosition.Y + 192), graphics,TabSourceRectangle),
                new CraftableRecipeBar(1,CraftingGuide,211,new Vector2(BackDropPosition.X + 32, BackDropPosition.Y + 64), graphics,TabSourceRectangle),
                new CraftableRecipeBar(1,CraftingGuide,212,new Vector2(BackDropPosition.X + 32, BackDropPosition.Y + 128), graphics,TabSourceRectangle),
                new CraftableRecipeBar(2,CraftingGuide,232,new Vector2(BackDropPosition.X + 32, BackDropPosition.Y + 64), graphics,TabSourceRectangle),
                new CraftableRecipeBar(2,CraftingGuide,233,new Vector2(BackDropPosition.X + 32, BackDropPosition.Y + 128), graphics,TabSourceRectangle),
                new CraftableRecipeBar(3,CraftingGuide,145,new Vector2(BackDropPosition.X + 32, BackDropPosition.Y + 64), graphics,TabSourceRectangle)
            };

            Tabs = new List<Tab>()
            {
                new Tab(0, Game1.AllTextures.UserInterfaceTileSet, TabSourceRectangle,new Rectangle(832,16, 64,64), graphics, new Vector2(BackDropPosition.X-16, BackDropPosition.Y + 64)),
                new Tab(1, Game1.AllTextures.UserInterfaceTileSet, TabSourceRectangle,new Rectangle(32,288, 16,16), graphics, new Vector2(BackDropPosition.X-16, BackDropPosition.Y + 128)),
                new Tab(2, Game1.AllTextures.UserInterfaceTileSet, TabSourceRectangle,new Rectangle(32,288, 16,16), graphics, new Vector2(BackDropPosition.X-16, BackDropPosition.Y + 192)),
                new Tab(3, Game1.AllTextures.UserInterfaceTileSet, TabSourceRectangle,new Rectangle(32,288, 16,16), graphics, new Vector2(BackDropPosition.X-16, BackDropPosition.Y + 256)),
                new Tab(4, Game1.AllTextures.UserInterfaceTileSet, TabSourceRectangle,new Rectangle(32,288, 16,16), graphics, new Vector2(BackDropPosition.X-16, BackDropPosition.Y + 320))

            };
            RedEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), graphics, new Vector2(BackDropPosition.X + 300, BackDropPosition.Y));
            currentPage = 0;
            this.BackDropSourceRectangle = new Rectangle(864, 48, 288, 336);
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {

                RedEsc.Update(mouse);
                if(RedEsc.isClicked)
                {
                    Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                }
                foreach(Tab tab in Tabs)
                {
                   
                    tab.Update(gameTime, ref this.currentPage, mouse);
                }
                foreach (CraftableRecipeBar bar in CraftingRecipeBars)
                {
                    if(bar.page == this.currentPage)
                    {
                        bar.Update(gameTime, mouse);
                    }
                    
                }

            
        }
        public void Draw(SpriteBatch spriteBatch)
        {

                RedEsc.Draw(spriteBatch);
                spriteBatch.Draw(this.BackDropTexture, BackDropPosition, this.BackDropSourceRectangle, Color.White);
                foreach(Tab tab in Tabs)
                {
                    tab.Draw(spriteBatch, this.currentPage);
                }
                foreach (CraftableRecipeBar bar in CraftingRecipeBars)
                {
                    if (bar.page == this.currentPage)
                    {
                        bar.Draw(spriteBatch);
                    }
                }
            

        }
    }

    public class Tab
    {
        Button button;
        int pageToOpen;
        Rectangle backGroundSourceRectangle;
        Rectangle foreGroundSourceRectangle;
        public Tab(int index,Texture2D texture, Rectangle backGroundSourceRectangle, Rectangle foreGroundSourceRectangle, GraphicsDevice graphics, Vector2 position)
        {
            button = new Button(texture, backGroundSourceRectangle, graphics, position);
            this.backGroundSourceRectangle = backGroundSourceRectangle;
            this.foreGroundSourceRectangle = foreGroundSourceRectangle;
            this.pageToOpen = index;
        }

        public void Update(GameTime gameTime, ref int currentTab,MouseManager mouse)
        {
            button.Update(mouse);
            if(button.isClicked)
            {
                currentTab = pageToOpen;
            }
        }

        public void Draw(SpriteBatch spriteBatch, int currentTab)
        {
            if(pageToOpen == currentTab)
            {
                button.DrawCraftingSlot(spriteBatch, this.foreGroundSourceRectangle, this.backGroundSourceRectangle, Game1.AllTextures.MenuText, "", button.Position, Color.White);
            }
            else
            {
                button.DrawCraftingSlot(spriteBatch, this.foreGroundSourceRectangle, this.backGroundSourceRectangle, Game1.AllTextures.MenuText, "", button.Position, Color.White * .25f);
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
        public Rectangle BackgroundSourceRectangle { get; set; }
        public CraftingSlot(GraphicsDevice graphics, int countOfItemsRequired, int itemID, Vector2 drawPosition)
        {
            Item item = Game1.ItemVault.GenerateNewItem(itemID, null);
            this.ItemID = itemID;
            Button = new Button(item.ItemSprite.AtlasTexture, item.SourceTextureRectangle, graphics, drawPosition);
            this.countOfItemsRequired = countOfItemsRequired;
            this.drawPosition = drawPosition;
            this.colorMultiplier = .25f;
            this.BackgroundSourceRectangle = new Rectangle(800, 48, 64, 64);
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            Button.Update(mouse);
            this.satisfied = CheckIfPlayerHasRequiredItems();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Button.DrawCraftingSlot(spriteBatch, Button.BackGroundSourceRectangle,this.BackgroundSourceRectangle, Game1.AllTextures.MenuText,
                Game1.Player.Inventory.FindNumberOfItemInInventory(ItemID).ToString() + "/" + countOfItemsRequired.ToString(), drawPosition, Color.White * colorMultiplier, 1f, 3f);
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
        public int page;
        CraftingGuide guide;
        Button retrievableButton;
        int tier;
        int itemID;
        bool canCraft = false;
        public Rectangle BackgroundSourceRectangle { get; set; }

        public CraftableRecipeBar(int page, CraftingGuide guide, int itemID, Vector2 drawPosition, GraphicsDevice graphics, Rectangle backGroundSourceRectangle)
        {
            this.page = page;
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
            this.BackgroundSourceRectangle = backGroundSourceRectangle;
            retrievableButton = new Button(Game1.ItemVault.GenerateNewItem(itemID, null).ItemSprite.AtlasTexture,
                Game1.ItemVault.GenerateNewItem(itemID, null).SourceTextureRectangle, graphics, new Vector2(CraftingSlots[tier - 1].drawPosition.X + 128, CraftingSlots[tier - 1].drawPosition.Y));
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            canCraft = true;
            
            

            retrievableButton.Update(mouse);

            if (this.retrievableButton.IsHovered)
            {
                foreach (CraftingSlot slot in CraftingSlots)
                {
                    slot.Update(gameTime, mouse);
                    if (!slot.satisfied)
                    {
                        canCraft = false;
                    }
                }
            }
            if (retrievableButton.isClicked && canCraft)
            {
                switch (page)
                {
                    case (0):
                        Game1.SoundManager.CraftMetal.Play();
                        break;
                    default:
                        Game1.SoundManager.CraftMetal.Play();
                        break;

                }

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
            
            if(retrievableButton.IsHovered)
            {
                for (int i = 0; i < CraftingSlots.Count; i++)
                {
                    CraftingSlots[i].Draw(spriteBatch);
                    if (i == CraftingSlots.Count - 1)
                    {
                        spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(CraftingSlots[i].drawPosition.X + 80, CraftingSlots[i].drawPosition.Y + 16),
                    new Rectangle(112, 288, 32, 32), Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                    }
                    else
                    {
                        spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(CraftingSlots[i].drawPosition.X + 64, CraftingSlots[i].drawPosition.Y + 16),
                    new Rectangle(80, 288, 32, 32), Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                    }

                }
            }
            
            //retrievableButton.Draw(spriteBatch);
            if(this.canCraft)
            {
                retrievableButton.DrawCraftingSlot(spriteBatch, retrievableButton.BackGroundSourceRectangle, this.BackgroundSourceRectangle,
                Game1.AllTextures.MenuText, "", new Vector2(CraftingSlots[tier - 1].drawPosition.X + 64, CraftingSlots[tier - 1].drawPosition.Y), Color.White, 1f, 3f);
            }
            else
            {
                retrievableButton.DrawCraftingSlot(spriteBatch, retrievableButton.BackGroundSourceRectangle, this.BackgroundSourceRectangle,
                Game1.AllTextures.MenuText, "", new Vector2(CraftingSlots[tier - 1].drawPosition.X + 64, CraftingSlots[tier - 1].drawPosition.Y), Color.White  *.25f, 1f, 3f);
            }
            
            //spriteBatch.Draw(Game1.ItemVault.GenerateNewItem(itemID, null).ItemSprite.AtlasTexture, new Vector2(CraftingSlots[tier - 1].drawPosition.X + 264, CraftingSlots[tier - 1].drawPosition.Y), Game1.ItemVault.GenerateNewItem(itemID, null).SourceTextureRectangle, Color.White);




        }
    }


}
