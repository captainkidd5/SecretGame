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
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FreezesGame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GraphicsDevice GraphicsDevice { get; set; }
        public Rectangle BackDropSourceRectangle { get; set; }
        public Vector2 BackDropPosition { get; set; }

        public float BackDropScale { get; set; }

        public int ActiveTab { get; set; }
        public Tab[] Tabs { get; set; }
       

        public CraftingGuide CraftingGuide { get; set; }

        public Button FowardButton { get; set; }
        public Button BackButton { get; set; }
        private Button redEsc;


        public CraftableRecipeBar CraftableRecipeBar { get; set; }

        public CraftingMenu(ContentManager content, GraphicsDevice graphics)
        {
            this.GraphicsDevice = graphics;
            BackDropSourceRectangle = new Rectangle(304, 352, 224, 224);
            BackDropPosition = new Vector2(Game1.ScreenWidth /4, (int)(Game1.ScreenHeight / 16));
            BackDropScale = 3f;
            CraftingGuide = content.Load<CraftingGuide>("Item/Crafting/CraftingGuide");
            Tabs = new Tab[6];
            for(int i = 0; i < Tabs.Length; i++)
            {
                Tabs[i] = new Tab(this,GraphicsDevice, new Vector2(BackDropPosition.X - 32*BackDropScale, BackDropPosition.Y + 64 + i * 32 * BackDropScale), new Rectangle(272,384 + 32*i, 32,32), BackDropScale);
            }
            //tabs 1 should be tools
            Tabs[0].AddNewCraftableItem(0, 5, new Vector2(BackDropPosition.X, BackDropPosition.Y), graphics, this);
            Tabs[0].AddNewCraftableItem(1, 5, new Vector2(BackDropPosition.X, BackDropPosition.Y + 48), graphics, this);
            Tabs[0].AddNewCraftableItem(2, 5, new Vector2(BackDropPosition.X, BackDropPosition.Y + 96), graphics, this);
            Tabs[0].AddNewCraftableItem(3, 5, new Vector2(BackDropPosition.X, BackDropPosition.Y + 144), graphics, this);
            Tabs[0].AddNewCraftableItem(20, 5, new Vector2(BackDropPosition.X, BackDropPosition.Y + 192), graphics, this);
            Tabs[1].AddNewCraftableItem(124, 5, new Vector2(BackDropPosition.X, BackDropPosition.Y), graphics, this);
            Tabs[1].AddNewCraftableItem(121, 5, new Vector2(BackDropPosition.X, BackDropPosition.Y + 48), graphics, this);
            Tabs[1].AddNewCraftableItem(211, 5, new Vector2(BackDropPosition.X, BackDropPosition.Y + 96), graphics, this);
            Tabs[1].AddNewCraftableItem(212, 5, new Vector2(BackDropPosition.X, BackDropPosition.Y + 144), graphics, this);
            Tabs[1].AddNewCraftableItem(232, 5, new Vector2(BackDropPosition.X, BackDropPosition.Y + 192), graphics, this);
            Tabs[1].AddNewCraftableItem(233, 5, new Vector2(BackDropPosition.X, BackDropPosition.Y + 48), graphics, this);
            Tabs[1].AddNewCraftableItem(145, 5, new Vector2(BackDropPosition.X, BackDropPosition.Y + 96), graphics, this);

            ActiveTab = 0;

            FowardButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1008, 176, 16, 64), this.GraphicsDevice,
                new Vector2(BackDropPosition.X + BackDropSourceRectangle.Width * BackDropScale -50 , BackDropSourceRectangle.Y), CursorType.Normal, this.BackDropScale);
            BackButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(848, 176, 16, 64),
                this.GraphicsDevice, new Vector2(BackDropPosition.X, BackDropSourceRectangle.Y), CursorType.Normal, this.BackDropScale);
            this.redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), GraphicsDevice,
                new Vector2(BackDropPosition.X + BackDropSourceRectangle.Width * BackDropScale - 50, BackDropPosition.Y), CursorType.Normal);


            this.CraftableRecipeBar = new CraftableRecipeBar(this.CraftingGuide,graphics, this.BackDropSourceRectangle, this.BackDropPosition,this.BackDropScale);
        }

        public void Update(GameTime gameTime)
        {
            for(int i =0; i < Tabs.Length;i++)
            {
               
                Tabs[i].Button.Update(Game1.myMouseManager);
                if(Tabs[i].Button.isClicked)
                {
                    ActiveTab = i;
                    
                }
                if (ActiveTab == i)
                {
                    Tabs[i].IsActive = true;
                    Tabs[i].ButtonColorMultiplier = .5f;
                }
                else
                {
                    Tabs[i].IsActive = false;
                    Tabs[i].ButtonColorMultiplier = 1f;
                }
                Tabs[ActiveTab].Update(gameTime);

                
                
            }
            this.FowardButton.Update(Game1.myMouseManager);

            if (this.FowardButton.isClicked)
            {
                if (Tabs[ActiveTab].ActivePage < Tabs[ActiveTab].Pages.Count - 1)
                {
                    Tabs[ActiveTab].ActivePage++;
                }

            }


            this.BackButton.Update(Game1.myMouseManager);

            if (BackButton.isClicked)
            {
                if (Tabs[ActiveTab].ActivePage > 0)
                {
                    Tabs[ActiveTab].ActivePage--;
                }
            }
            CraftableRecipeBar.Update(gameTime);
            redEsc.Update(Game1.myMouseManager);


            if (redEsc.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                Game1.Player.UserInterface.CurrentOpenShop = 0;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, BackDropPosition, BackDropSourceRectangle,
                Color.White, 0f, Game1.Utility.Origin, BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);

            for (int i = 0; i < Tabs.Length; i++)
            {
                Tabs[i].Draw(spriteBatch);
            }

            if (Tabs[ActiveTab].ActivePage >= Tabs[ActiveTab].Pages.Count - 1)
            {
                this.FowardButton.DrawNormal(spriteBatch, FowardButton.Position, FowardButton.BackGroundSourceRectangle, Color.White * .5f,
                0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            }
            else
            {
                this.FowardButton.DrawNormal(spriteBatch, FowardButton.Position, FowardButton.BackGroundSourceRectangle, Color.White,
                0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            }
            if (Tabs[ActiveTab].ActivePage <= 0)
            {
                this.BackButton.DrawNormal(spriteBatch, BackButton.Position, BackButton.BackGroundSourceRectangle, Color.White * .5f,
               0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);

            }
            else
            {
                this.BackButton.DrawNormal(spriteBatch, BackButton.Position, BackButton.BackGroundSourceRectangle, Color.White,
               0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);

            }

            CraftableRecipeBar.Draw(spriteBatch);
            redEsc.Draw(spriteBatch);



        }
    }


    public class CraftableRecipeBar
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public int ActiveRecipe { get; set; }
        public Item ActiveItemToCraft { get; set; }
        public Rectangle ItemToCraftSourceRectangle { get; set; }
        public Button ItemToCraftButton { get; set; }
        public float BackDropScale { get; set; }

        public ItemRecipe Recipe { get; set; }

        public List<CraftableRecipeIngredient> Ingredients{ get; set; }

        public CraftingGuide CraftingGuide { get; set; }
        public Vector2 BackDropPosition { get; set; }

        public float ColorMultiplier { get; set; }
        public bool Locked { get; set; }
        public Color Color { get; set; }
        public CraftableRecipeBar(CraftingGuide craftingGuide,GraphicsDevice graphics, Rectangle backDropSourceRectangle,Vector2 backDropPosition, float backDropScale)
        {
            this.GraphicsDevice = graphics;
            ItemToCraftSourceRectangle = new Rectangle(0, 0, 1, 1);
            this.BackDropScale = backDropScale;
            ItemToCraftButton = new Button(Game1.AllTextures.ItemSpriteSheet, new Rectangle(528, 352, 48, 48),
                graphics, new Vector2(backDropPosition.X + backDropSourceRectangle.Width * backDropScale - 220, backDropPosition.Y + 10),
                CursorType.Normal, backDropScale);
            ItemToCraftButton.HitBoxRectangle = new Rectangle((int)ItemToCraftButton.Position.X, (int)ItemToCraftButton.Position.Y, 48, 48);

            Ingredients = new List<CraftableRecipeIngredient>();
            this.CraftingGuide = craftingGuide;
            this.BackDropPosition = backDropPosition;

            this.ColorMultiplier = 1f;
            this.Locked = false;
            this.Color = Color.Black;
        }

        public void UpdateRecipe(int craftableItemID)
        {
            Ingredients = new List<CraftableRecipeIngredient>();
            ItemRecipe recipe = CraftingGuide.CraftingRecipes.Find(x => x.ItemToCraftID == craftableItemID);
            for(int i =0; i < recipe.AllItemsRequired.Count; i++)
            {
                Item itemToReference = Game1.ItemVault.GenerateNewItem(recipe.AllItemsRequired[i].ItemID, null);
                Ingredients.Add(new CraftableRecipeIngredient(GraphicsDevice, new Button(Game1.AllTextures.ItemSpriteSheet, itemToReference.SourceTextureRectangle, GraphicsDevice,
                    new Vector2(BackDropPosition.X + 60 + i * 112, ItemToCraftButton.Position.Y + 60), CursorType.Normal, 5f),itemToReference, recipe.AllItemsRequired[i].Count));
            }

        }

        public void Update(GameTime gameTime)
        {
            if(!this.Locked)
            {
                this.Color = Color.White;
                bool craftable = true;
                for (int i = 0; i < Ingredients.Count; i++)
                {
                    Ingredients[i].Update(gameTime);
                    if (!Ingredients[i].Satisfied)
                    {
                        craftable = false;
                    }
                }

                if (craftable)
                {
                    this.ColorMultiplier = 1f;
                }
                else
                {
                    this.ColorMultiplier = .5f;

                }

                ItemToCraftButton.Update(Game1.myMouseManager);
                if (ItemToCraftButton.IsHovered)
                {
                    Item item = Game1.ItemVault.GenerateNewItem(this.ActiveRecipe, null);
                    Game1.Player.UserInterface.InfoBox.IsActive = true;
                    Game1.Player.UserInterface.InfoBox.FitText(item.Name + ": " + item.Description, 1f);
                    Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(ItemToCraftButton.Position.X + 200, ItemToCraftButton.Position.Y + 50);
                    if (ItemToCraftButton.isClicked && craftable)
                    {
                        Game1.Player.Inventory.TryAddItem(item);
                        for (int i = 0; i < Ingredients.Count; i++)
                        {
                            for (int j = 0; j < Ingredients[i].CountRequired; j++)
                            {
                                Game1.Player.Inventory.RemoveItem(Ingredients[i].Item.ID);
                            }
                        }
                        Game1.SoundManager.CraftMetal.Play();

                    }
                }
            }
            else
            {
                this.Color = Color.Black;
            }
            
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ItemToCraftButton.Draw(spriteBatch, ItemToCraftSourceRectangle, ItemToCraftButton.BackGroundSourceRectangle,
               Game1.AllTextures.MenuText, "", ItemToCraftButton.Position, this.Color * ColorMultiplier, this.BackDropScale, this.BackDropScale + 2, Game1.Utility.StandardButtonDepth + .01f);
            for(int i =0; i < Ingredients.Count; i++)
            {
                Ingredients[i].Draw(spriteBatch,this.Color);
            }
        }
    }

    public class CraftableRecipeIngredient
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public Button Button { get; set; }
        public bool Satisfied { get; set; }
        public Item Item { get; set; }
        public int CurrentCount { get; set; }
        public int CountRequired { get; set; }
        public float ColorMultiplier { get; set; }


        public CraftableRecipeIngredient(GraphicsDevice graphics, Button button, Item item, int countRequired)
        {
            this.GraphicsDevice = graphics;
            this.Button = button;
            this.Satisfied = false;
            this.Item = item;
            this.CurrentCount = 0;
            this.CountRequired = countRequired;
            this.ColorMultiplier = 1f;

        }

        public void Update(GameTime gameTime)
        {
            Button.Update(Game1.myMouseManager);

                this.CurrentCount = Game1.Player.Inventory.FindNumberOfItemInInventory(Item.ID);
                if (this.CurrentCount >= CountRequired)
                {
                    this.Satisfied = true;
                    ColorMultiplier = 1f;
                }
                else
                {
                    this.Satisfied = false;
                    ColorMultiplier = .5f;
                }
            

            
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            Button.DrawNormal(spriteBatch, Button.Position, Button.BackGroundSourceRectangle, color * ColorMultiplier, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, .95f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.CurrentCount.ToString() + "/" + this.CountRequired.ToString(),
                new Vector2(Button.Position.X, Button.Position.Y + 50), Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, .95f);
        }
    }


    public class Tab
    {
        public CraftingMenu CraftingMenu { get; set; }
        public int Index { get; set; }
        public bool IsActive { get; set; }
        public int ActivePage { get; set; }
        public List<ToolTipPage> Pages { get; set; }

        public Button Button { get; set; }

        public Vector2 PositionToDraw { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public float ButtonColorMultiplier { get; set; }
        public Tab(CraftingMenu craftingMenu,GraphicsDevice graphics, Vector2 positionToDraw, Rectangle sourceRectangle, float scale)
        {
            this.CraftingMenu = craftingMenu;
            this.PositionToDraw = positionToDraw;
            this.SourceRectangle = sourceRectangle;
            this.Button = new Button(Game1.AllTextures.UserInterfaceTileSet, SourceRectangle, graphics, PositionToDraw, CursorType.Normal, scale);
            this.ButtonColorMultiplier = 1f;
            this.ActivePage = 0;
            this.Pages = new List<ToolTipPage>()
            {
                new ToolTipPage(craftingMenu),
                new ToolTipPage(craftingMenu)
            };
        }

        public void Update(GameTime gameTime)
        {

            for(int i =0; i < Pages[ActivePage].ToolTips.Count; i++)
            {
                Pages[ActivePage].ToolTips[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Button.DrawNormal(spriteBatch, Button.Position, Button.BackGroundSourceRectangle, Color.White * ButtonColorMultiplier, 0f, Game1.Utility.Origin, Button.HitBoxScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            //Button.Draw(spriteBatch, Color.White * ButtonColorMultiplier, Game1.Utility.StandardButtonDepth);
            if (this.IsActive)
            {
                spriteBatch.DrawString(Game1.AllTextures.MenuText, ActivePage.ToString(), CraftingMenu.BackDropPosition, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);
                this.Pages[ActivePage].Draw(spriteBatch);
            }
            // Button.Draw()
        }//

        public void AddNewCraftableItem(int id, int maxRecipesPerPage, Vector2 backDropPosition, GraphicsDevice graphics, CraftingMenu craftingMenu)
        {

            for(int i =0; i < Pages.Count - 2; i++)
            {
                for(int j = 0; j< Pages[i].ToolTips.Count;i++)
                {
                    if(Pages[i].ToolTips[j].Item.ID == id)
                    {
                        throw (new Exception("Item recipe has already been added ya dingus"));
                    }
                }
            }
            
            for (int i = 0; i < Pages.Count; i++)
            {
                if (Pages[i].ToolTips.Count < maxRecipesPerPage)
                {
                    Pages[i].ToolTips.Add(new ToolTip(id, new Vector2(backDropPosition.X + 50, backDropPosition.Y + 200 + i * 50), graphics, craftingMenu));
                    return;
                }
            }

            Pages.Add(new ToolTipPage(craftingMenu));
            Pages[Pages.Count].ToolTips.Add(new ToolTip(id, new Vector2(backDropPosition.X + 50, backDropPosition.Y + 200), graphics, craftingMenu));
        }
    }


    public class ToolTipPage
    {
        public CraftingMenu CraftingMenu { get; set; }
        public bool IsActive { get; set; }
        public int Index { get; set; }
        public List<ToolTip> ToolTips { get; set; }
        public ToolTipPage(CraftingMenu craftingMenu)
        {
            this.CraftingMenu = craftingMenu;
            ToolTips = new List<ToolTip>();
        }
        public void Update(GameTime gameTime)
        {
            for(int i =0; i < ToolTips.Count;i++)
            {
                ToolTips[i].Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            for (int i = 0; i < ToolTips.Count; i++)
            {
                ToolTips[i].Draw(spriteBatch);
            }
        }
    }

    public class ToolTip
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public Rectangle BackGroundSourceRectangle { get; set; }
        public Vector2 Position { get; set; }
        public Button Button { get; set; }
        public Item Item { get; set; }
        public CraftingMenu CraftingMenu { get; set; }

        public bool Locked { get; set; }
        public Color Color { get; set; }

        public ToolTip(int itemToCraftID, Vector2 position,GraphicsDevice graphics, CraftingMenu craftingMenu)
        {
            this.Position = position;
            this.GraphicsDevice = graphics;
            Item = Game1.ItemVault.GenerateNewItem(itemToCraftID, null);
            Button = new Button(Game1.AllTextures.ItemSpriteSheet, Item.SourceTextureRectangle, GraphicsDevice, this.Position, CursorType.Normal, 3f);
            CraftingMenu = craftingMenu;
            this.Locked = false;
            this.Color = Color.Black;
           
        }

        public void Update(GameTime gameTime)
        {
            if(this.Locked)
            {
                Button.Color = Color.Black;
            }
            else
            {
                Button.Color = Color.White;
            }
            Button.Update(Game1.myMouseManager);
            if(this.Button.isClicked)
            {
                CraftingMenu.CraftableRecipeBar.ActiveRecipe = this.Item.ID;
                CraftingMenu.CraftableRecipeBar.ItemToCraftSourceRectangle = Item.SourceTextureRectangle;
                CraftingMenu.CraftableRecipeBar.UpdateRecipe(this.Item.ID);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
           // Button.Draw(spriteBatch, .9f);
            Button.DrawNormal(spriteBatch, this.Position, Item.SourceTextureRectangle, Button.Color, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, .95f);
        }
    }




}
