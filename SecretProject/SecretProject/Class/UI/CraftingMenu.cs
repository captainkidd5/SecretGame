using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
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

        public ToolTip ActiveToolTip { get; set; }

        public CraftableRecipeBar CraftableRecipeBar { get; set; }

        public CraftingMenu(ContentManager content, GraphicsDevice graphics)
        {
            this.GraphicsDevice = graphics;
            this.BackDropSourceRectangle = new Rectangle(304, 365, 112, 163);
            this.BackDropPosition = new Vector2(Game1.ScreenWidth / 8, (int)(Game1.ScreenHeight / 16));
            this.BackDropScale = 3f;
            this.CraftingGuide = content.Load<CraftingGuide>("Item/Crafting/CraftingGuide");
            this.Tabs = new Tab[6];
            for (int i = 0; i < this.Tabs.Length; i++)
            {
                this.Tabs[i] = new Tab(this, this.GraphicsDevice, new Vector2(this.BackDropPosition.X - 32 * this.BackDropScale, this.BackDropPosition.Y + 64 + i * 32 * this.BackDropScale), new Rectangle(272, 384 + 32 * i, 32, 32), this.BackDropScale);
            }

            //TOOLS TAB 0
            //stone axe
            this.Tabs[0].AddNewCraftableItem(0, 0,0, this.BackDropPosition, graphics, this);
            //steel axe
            this.Tabs[0].AddNewCraftableItem(1,1,0, this.BackDropPosition, graphics, this);

            //turquoise axe
            this.Tabs[0].AddNewCraftableItem(3, 3, 0, this.BackDropPosition, graphics, this);
            //stone hammer
            this.Tabs[0].AddNewCraftableItem(40,0,1, this.BackDropPosition, graphics, this);
            //steel hammer
            this.Tabs[0].AddNewCraftableItem(41,1,1, this.BackDropPosition, graphics, this);
            //dark hammer
            this.Tabs[0].AddNewCraftableItem(42, 2, 1, this.BackDropPosition, graphics, this);
            //stone shovel
            this.Tabs[0].AddNewCraftableItem(120,0,2, this.BackDropPosition, graphics, this);
            //stone sword
            this.Tabs[0].AddNewCraftableItem(160, 0,3, this.BackDropPosition, graphics, this);

            //Bow
            this.Tabs[0].AddNewCraftableItem(240, 0, 4, this.BackDropPosition, graphics, this);
            this.Tabs[0].AddNewCraftableItem(280, 1, 4, this.BackDropPosition, graphics, this);


            //GIZMOS TAB 1
            //small cog
            this.Tabs[1].AddNewCraftableItem(760, 0, 0, this.BackDropPosition, graphics, this);

            //Large cog
            this.Tabs[1].AddNewCraftableItem(800, 1, 0, this.BackDropPosition, graphics, this);

            //Steel Spring
            this.Tabs[1].AddNewCraftableItem(720, 2, 0, this.BackDropPosition, graphics, this);

            //Rope
            this.Tabs[1].AddNewCraftableItem(1041, 0, 1, this.BackDropPosition, graphics, this);

            //Canvas
            this.Tabs[1].AddNewCraftableItem(1040, 1, 1, this.BackDropPosition, graphics, this);



            //FOOD --> needs to be changed to stations
            //cauldron
            this.Tabs[2].AddNewCraftableItem(1055, 0,0, this.BackDropPosition, graphics, this);
            //saw table
            this.Tabs[2].AddNewCraftableItem(1201, 0,1, this.BackDropPosition, graphics, this);
            //furnace
            this.Tabs[2].AddNewCraftableItem(1202, 0,2, this.BackDropPosition, graphics, this);

            this.Tabs[2].AddNewCraftableItem(1162, 0,3, this.BackDropPosition, graphics, this);

            //small chest
            this.Tabs[2].AddNewCraftableItem(1240, 1, 0, this.BackDropPosition, graphics, this);

            //capture crate
            this.Tabs[2].AddNewCraftableItem(333, 2, 0, this.BackDropPosition, graphics, this);

            //DECORATIONS TAB 3

            //Oak Floor
            this.Tabs[3].AddNewCraftableItem(480, 0, 0, this.BackDropPosition, graphics, this);
            //Stone Floor
            this.Tabs[3].AddNewCraftableItem(482, 1, 0, this.BackDropPosition, graphics, this);
            //Barrel
            this.Tabs[3].AddNewCraftableItem(1160, 0, 1, this.BackDropPosition, graphics, this);
            //Oak Dresser
            this.Tabs[3].AddNewCraftableItem(1440, 0, 2, this.BackDropPosition, graphics, this);


            this.ActiveTab = 0;

            this.FowardButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(384, 528, 32, 16), this.GraphicsDevice,
                new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width * this.BackDropScale - 96, this.BackDropSourceRectangle.Y + this.BackDropSourceRectangle.Height), CursorType.Normal, this.BackDropScale);
            this.BackButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(304, 528, 32, 16),
                this.GraphicsDevice, new Vector2(this.BackDropPosition.X, this.BackDropSourceRectangle.Y + this.BackDropSourceRectangle.Height), CursorType.Normal, this.BackDropScale);
            redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), this.GraphicsDevice,
                new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width * this.BackDropScale - 32, this.BackDropPosition.Y + 32), CursorType.Normal);


            this.CraftableRecipeBar = new CraftableRecipeBar(this, graphics, this.BackDropPosition, this.BackDropScale);
        }

        public bool UnlockRecipe(int id)
        {
            for(int i =0; i < Tabs.Length; i++)
            {
                for(int j =0; j < Tabs[i].Pages.Count; j++)
                {
                    ToolTip toolTip = Tabs[i].Pages[j].ToolTips.Find(x => x.Item.ID == id);
                    if(toolTip != null)
                    {
                        toolTip.Locked = false;
                        Game1.Player.UserInterface.AddAlert(AlertType.Normal, AlertSize.Medium, Game1.Utility.centerScreen, Game1.ItemVault.GetItem(id).Name + " has been unlocked!");
                        return true;
                    }
                    
                }
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.Tabs.Length; i++)
            {

                this.Tabs[i].Button.Update(Game1.MouseManager);
                if (this.Tabs[i].Button.isClicked)
                {
                    this.ActiveTab = i;

                }
                if (this.ActiveTab == i)
                {
                    this.Tabs[i].IsActive = true;
                    this.Tabs[i].ButtonColorMultiplier = .5f;
                }
                else
                {
                    this.Tabs[i].IsActive = false;
                    this.Tabs[i].ButtonColorMultiplier = 1f;
                }
                this.Tabs[this.ActiveTab].Update(gameTime);



            }
            this.FowardButton.Update(Game1.MouseManager);

            if (this.FowardButton.isClicked)
            {
                if (this.Tabs[this.ActiveTab].ActivePage < this.Tabs[this.ActiveTab].Pages.Count - 1)
                {
                    this.Tabs[this.ActiveTab].ActivePage++;
                }

            }


            this.BackButton.Update(Game1.MouseManager);

            if (this.BackButton.isClicked)
            {
                if (this.Tabs[this.ActiveTab].ActivePage > 0)
                {
                    this.Tabs[this.ActiveTab].ActivePage--;
                }
            }
            this.CraftableRecipeBar.Update(gameTime);
            redEsc.Update(Game1.MouseManager);


            if (redEsc.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                Game1.Player.UserInterface.CurrentOpenShop = 0;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.BackDropPosition, this.BackDropSourceRectangle,
                Color.White, 0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Utility.StandardButtonDepth - .01f);

            for (int i = 0; i < this.Tabs.Length; i++)
            {
                this.Tabs[i].Draw(spriteBatch);
            }

            if (this.Tabs[this.ActiveTab].ActivePage >= this.Tabs[this.ActiveTab].Pages.Count - 1)
            {
                this.FowardButton.DrawNormal(spriteBatch, this.FowardButton.Position, this.FowardButton.BackGroundSourceRectangle, Color.White * .5f,
                0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Utility.StandardButtonDepth);
            }
            else
            {
                this.FowardButton.DrawNormal(spriteBatch, this.FowardButton.Position, this.FowardButton.BackGroundSourceRectangle, Color.White,
                0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Utility.StandardButtonDepth);
            }
            if (this.Tabs[this.ActiveTab].ActivePage <= 0)
            {
                this.BackButton.DrawNormal(spriteBatch, this.BackButton.Position, this.BackButton.BackGroundSourceRectangle, Color.White * .5f,
               0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Utility.StandardButtonDepth);

            }
            else
            {
                this.BackButton.DrawNormal(spriteBatch, this.BackButton.Position, this.BackButton.BackGroundSourceRectangle, Color.White,
               0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Utility.StandardButtonDepth);

            }

            this.CraftableRecipeBar.Draw(spriteBatch);
            redEsc.Draw(spriteBatch);



        }
    }


    public class CraftableRecipeBar
    {
        public CraftingMenu CraftingMenu { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public int ActiveRecipe { get; set; }
        public Item ActiveItemToCraft { get; set; }
        public Rectangle ItemToCraftSourceRectangle { get; set; }
        public Rectangle BackGroundSourceRectangle { get; set; }
        public Button ItemToCraftButton { get; set; }
        public float BackDropScale { get; set; }

        public ItemRecipe Recipe { get; set; }

        public List<CraftableRecipeIngredient> Ingredients { get; set; }

        public CraftingGuide CraftingGuide { get; set; }
        public Vector2 BackDropPosition { get; set; }

        public float ColorMultiplier { get; set; }
        public bool Locked { get; set; }
        public Color Color { get; set; }

        public bool IsDrawn { get; set; }

        public Button CraftButton { get; set; }
        public Color CraftButtonColor { get; set; }



        public CraftableRecipeBar(CraftingMenu craftingMenu, GraphicsDevice graphics, Vector2 backDropPosition, float backDropScale)
        {
            this.CraftingMenu = craftingMenu;
            this.GraphicsDevice = graphics;
            this.ItemToCraftSourceRectangle = new Rectangle(0, 0, 1, 1);
            this.BackDropScale = backDropScale;
            this.BackGroundSourceRectangle = new Rectangle(432, 400, 80, 96);
            this.BackDropPosition = new Vector2(craftingMenu.BackDropPosition.X + craftingMenu.BackDropSourceRectangle.Width * craftingMenu.BackDropScale,
                craftingMenu.BackDropSourceRectangle.Height * craftingMenu.BackDropScale / 4);
            this.ItemToCraftButton = new Button(Game1.AllTextures.ItemSpriteSheet, new Rectangle(528, 352, 16, 16),
                graphics, new Vector2(this.BackDropPosition.X + this.BackGroundSourceRectangle.Width / 2 + 32, this.BackDropPosition.Y + 16),
                CursorType.Normal, backDropScale);
            this.ItemToCraftButton.HitBoxRectangle = new Rectangle((int)this.ItemToCraftButton.Position.X, (int)this.ItemToCraftButton.Position.Y, 48, 48);

            this.Ingredients = new List<CraftableRecipeIngredient>();
            this.CraftingGuide = craftingMenu.CraftingGuide;


            this.ColorMultiplier = 1f;
            this.Locked = false;
            this.Color = Color.Black;
            this.CraftButtonColor = Color.White;

            this.CraftButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(441, 496, 62, 22), graphics,
                new Vector2(this.BackDropPosition.X + this.BackGroundSourceRectangle.Width / 3, this.BackDropPosition.Y + this.BackGroundSourceRectangle.Height * backDropScale - 80), CursorType.Normal, 3f);
        }

        public void UpdateRecipe(int craftableItemID)
        {
            this.Ingredients = new List<CraftableRecipeIngredient>();
            ItemRecipe recipe = this.CraftingGuide.CraftingRecipes.Find(x => x.ItemToCraftID == craftableItemID);
            this.Locked = recipe.Locked;
            for (int i = 0; i < recipe.AllItemsRequired.Count; i++)
            {
                Item itemToReference = Game1.ItemVault.GenerateNewItem(recipe.AllItemsRequired[i].ItemID, null);
                this.Ingredients.Add(new CraftableRecipeIngredient(this.GraphicsDevice, new Button(Game1.AllTextures.ItemSpriteSheet, itemToReference.SourceTextureRectangle, this.GraphicsDevice,
                    new Vector2(this.BackDropPosition.X + 60 + i * 112, this.ItemToCraftButton.Position.Y + 60), CursorType.Normal, 5f), itemToReference, recipe.AllItemsRequired[i].Count));
            }

        }

        public void Update(GameTime gameTime)
        {
            if (this.CraftingMenu.ActiveToolTip != null)
            {
                this.Locked = this.CraftingMenu.ActiveToolTip.Locked;
                this.IsDrawn = true;
            }
            else
            {
                this.ItemToCraftSourceRectangle = new Rectangle(0, 0, 1, 1);
                this.IsDrawn = false;
                return;
            }

            if (!this.Locked)
            {
                this.Color = Color.White;
                bool craftable = true;
                for (int i = 0; i < this.Ingredients.Count; i++)
                {
                    this.Ingredients[i].Update(gameTime);
                    if (!this.Ingredients[i].Satisfied)
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

                this.ItemToCraftButton.Update(Game1.MouseManager);
                this.CraftButton.Update(Game1.MouseManager);
                if (this.ItemToCraftButton.IsHovered)
                {
                    Item item = Game1.ItemVault.GenerateNewItem(this.ActiveRecipe, null);
                    Game1.Player.UserInterface.InfoBox.IsActive = true;
                    Game1.Player.UserInterface.InfoBox.FitText(Game1.ItemVault.GetItem(item.ID).Name + ": " + Game1.ItemVault.GetItem(item.ID).Description, 1f);
                    Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(Game1.MouseManager.Position.X, Game1.MouseManager.Position.Y + 64);
                }
                if (this.CraftButton.isClicked && craftable)
                {
                    Item item = Game1.ItemVault.GenerateNewItem(this.ActiveRecipe, null);
                    Game1.Player.Inventory.TryAddItem(item);
                    for (int i = 0; i < this.Ingredients.Count; i++)
                    {
                        for (int j = 0; j < this.Ingredients[i].CountRequired; j++)
                        {
                            Game1.Player.Inventory.RemoveItem(this.Ingredients[i].Item.ID);
                        }
                    }
                    Game1.SoundManager.CraftMetal.Play();

                }

            }
            else
            {
                this.Color = Color.Black;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            if (!this.Locked && this.CraftingMenu.ActiveToolTip != null)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.BackDropPosition,
                this.BackGroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Utility.StandardButtonDepth);
                this.ItemToCraftButton.Draw(spriteBatch, this.ItemToCraftSourceRectangle, this.ItemToCraftButton.BackGroundSourceRectangle,
               Game1.AllTextures.MenuText, "", this.ItemToCraftButton.Position, this.Color * this.ColorMultiplier, this.BackDropScale, this.BackDropScale, Utility.StandardButtonDepth + .01f);
                for (int i = 0; i < this.Ingredients.Count; i++)
                {
                    this.Ingredients[i].Draw(spriteBatch, this.Color, new Vector2(this.BackDropPosition.X + 32 + i * 64, this.BackDropPosition.Y + this.BackGroundSourceRectangle.Height), 1f);
                }

                this.CraftButton.Draw(spriteBatch, Game1.AllTextures.MenuText, "Craft", new Vector2(this.CraftButton.Position.X + this.CraftButton.BackGroundSourceRectangle.Width / 2 + 16, this.CraftButton.Position.Y + 16), this.CraftButtonColor * this.ColorMultiplier, Utility.StandardButtonDepth + .01f, Utility.StandardButtonDepth + .1f, 2f);
            }
            else if (this.CraftingMenu.ActiveToolTip == null)
            {
                this.ItemToCraftButton.Draw(spriteBatch, this.ItemToCraftSourceRectangle, this.ItemToCraftButton.BackGroundSourceRectangle,
               Game1.AllTextures.MenuText, "", new Vector2(this.ItemToCraftButton.Position.X - 400, this.ItemToCraftButton.Position.Y + 200), this.Color * this.ColorMultiplier, this.BackDropScale, this.BackDropScale + 2, Utility.StandardButtonDepth + .01f);
            }
            else if (this.Locked)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.BackDropPosition,
                this.BackGroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Utility.StandardButtonDepth);
                this.ItemToCraftButton.Draw(spriteBatch, this.ItemToCraftSourceRectangle, this.ItemToCraftButton.BackGroundSourceRectangle,
               Game1.AllTextures.MenuText, TextBuilder.ParseText("This item needs to be prototyped before it can be crafted.", this.BackGroundSourceRectangle.Width * this.BackDropScale - 32, 1f), new Vector2(this.BackDropPosition.X + 28, this.ItemToCraftButton.Position.Y + 96), this.Color * this.ColorMultiplier, this.BackDropScale, this.BackDropScale, Utility.StandardButtonDepth + .01f);
                //spriteBatch.DrawString(Game1.AllTextures.MenuText, "LOCKED", ItemToCraftButton.Position, Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, Utility.StandardButtonDepth + .01f);
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
            this.Button.Update(Game1.MouseManager);

            this.CurrentCount = Game1.Player.Inventory.FindNumberOfItemInInventory(this.Item.ID);
            if (this.CurrentCount >= this.CountRequired)
            {
                this.Satisfied = true;
                this.ColorMultiplier = 1f;
            }
            else
            {
                this.Satisfied = false;
                this.ColorMultiplier = .5f;
            }



        }

        public void Draw(SpriteBatch spriteBatch, Color color, Vector2 position, float stringScale)
        {
            this.Button.DrawNormal(spriteBatch, position, this.Button.BackGroundSourceRectangle, color * this.ColorMultiplier, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, Utility.StandardButtonDepth + .05f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.CurrentCount.ToString() + "/" + this.CountRequired.ToString(),
                new Vector2(position.X, position.Y + 25), Color.White, 0f, Game1.Utility.Origin, stringScale, SpriteEffects.None, Utility.StandardButtonDepth + .06f);
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
        public Tab(CraftingMenu craftingMenu, GraphicsDevice graphics, Vector2 positionToDraw, Rectangle sourceRectangle, float scale)
        {
            this.CraftingMenu = craftingMenu;
            this.PositionToDraw = positionToDraw;
            this.SourceRectangle = sourceRectangle;
            this.Button = new Button(Game1.AllTextures.UserInterfaceTileSet, this.SourceRectangle, graphics, this.PositionToDraw, CursorType.Normal, scale);
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

            for (int i = 0; i < this.Pages[this.ActivePage].ToolTips.Count; i++)
            {
                this.Pages[this.ActivePage].ToolTips[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Button.DrawNormal(spriteBatch, this.Button.Position, this.Button.BackGroundSourceRectangle, Color.White * this.ButtonColorMultiplier, 0f, Game1.Utility.Origin, this.Button.HitBoxScale, SpriteEffects.None, Utility.StandardButtonDepth);
            if (this.IsActive)
            {
                spriteBatch.DrawString(Game1.AllTextures.MenuText, this.ActivePage.ToString(), this.CraftingMenu.BackDropPosition, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, Utility.StandardButtonDepth + .01f);
                this.Pages[this.ActivePage].Draw(spriteBatch);
            }

        }

        public void AddNewCraftableItem(int id,  int rowIndex, int columnIndex,Vector2 backDropPosition, GraphicsDevice graphics, CraftingMenu craftingMenu)
        {

            for (int i = 0; i < this.Pages.Count - 2; i++)
            {
                for (int j = 0; j < this.Pages[i].ToolTips.Count; i++)
                {
                    if (this.Pages[i].ToolTips[j].Item.ID == id)
                    {
                        throw (new Exception("Item recipe has already been added ya dingus"));
                    }
                }
            }

            for (int i = 0; i < this.Pages.Count; i++)
            {
                if (this.Pages[i].ToolTips.Count < 20)
                {
                    this.Pages[i].ToolTips.Add(new ToolTip(id, new Vector2(backDropPosition.X + 50 + rowIndex * 48, backDropPosition.Y + 64 + columnIndex * 50), graphics, craftingMenu));
                    return;
                }
            }

            this.Pages.Add(new ToolTipPage(craftingMenu));
            this.Pages[this.Pages.Count].ToolTips.Add(new ToolTip(id, new Vector2(backDropPosition.X + 50, backDropPosition.Y + 200), graphics, craftingMenu));
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
            this.ToolTips = new List<ToolTip>();
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.ToolTips.Count; i++)
            {
                this.ToolTips[i].Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            for (int i = 0; i < this.ToolTips.Count; i++)
            {
                this.ToolTips[i].Draw(spriteBatch);
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


        public ToolTip(int itemToCraftID, Vector2 position, GraphicsDevice graphics, CraftingMenu craftingMenu)
        {
            this.Position = position;
            this.GraphicsDevice = graphics;
            this.Item = Game1.ItemVault.GenerateNewItem(itemToCraftID, null);
            this.Button = new Button(Game1.AllTextures.ItemSpriteSheet, this.Item.SourceTextureRectangle, this.GraphicsDevice, this.Position, CursorType.Normal, 3f);
            this.CraftingMenu = craftingMenu;
            this.Locked = this.CraftingMenu.CraftingGuide.CraftingRecipes.Find(x => x.ItemToCraftID == itemToCraftID).Locked;
            this.Color = Color.Black;

        }

        public void Update(GameTime gameTime)
        {
            if (this.Locked)
            {
                this.Color = Color.Black;
            }
            else
            {
                this.Color = Color.White;
            }
            this.Button.Update(Game1.MouseManager);
            if (this.Button.isClicked)
            {
                this.CraftingMenu.ActiveToolTip = this;
                // if (!this.Locked)
                // {
                this.CraftingMenu.CraftableRecipeBar.ActiveRecipe = this.Item.ID;

                this.CraftingMenu.CraftableRecipeBar.ItemToCraftSourceRectangle = this.Item.SourceTextureRectangle;
                this.CraftingMenu.CraftableRecipeBar.UpdateRecipe(this.Item.ID);
                // }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Button.DrawNormal(spriteBatch, this.Position, this.Item.SourceTextureRectangle, this.Color, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, .95f);
        }
    }


}
