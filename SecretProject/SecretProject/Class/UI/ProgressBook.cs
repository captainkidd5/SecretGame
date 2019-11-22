using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
using XMLData.ProgressBookStuff;

namespace SecretProject.Class.UI
{

    public class ProgressBook : IExclusiveInterfaceComponent
    {
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FreezesGame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int ID { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public Rectangle BackDropSourceRectangle { get; set; }
        public Vector2 BackDropPosition { get; set; }
        public float BackDropScale { get; set; }

        public List<CategoryTab> Tabs { get; set; }
        public int ActiveTab { get; set; }

        public ProgressBookHolder ProgressBookHolder { get; set; }

        public Button FowardButton { get; set; }
        public Button BackButton { get; set; }
        private Button redEsc;

        public ProgressBook(ContentManager content, GraphicsDevice graphics, int iD)
        {
            this.ID = iD;
            this.GraphicsDevice = graphics;
            this.BackDropSourceRectangle = new Rectangle(448, 112, 288, 160);
            this.BackDropPosition = new Vector2(Game1.ScreenWidth / 6, Game1.ScreenHeight / 6);
            this.BackDropScale = 3f;
            LoadContent(content, iD);
            Tabs = new List<CategoryTab>();
            for (int i = 0; i < ProgressBookHolder.Tabs.Count; i++)
            {
                Tabs.Add(new CategoryTab(this, ProgressBookHolder.Tabs[i].TabName, GraphicsDevice, new Vector2(this.BackDropPosition.X + 96 + i * 144, this.BackDropPosition.Y - 96),
                    new Rectangle(464, 80, 48, 32), this.BackDropScale));

                for (int j = 0; j < ProgressBookHolder.Tabs[i].Pages.Count; j++)
                {
                    UnlockableItemPage page = new UnlockableItemPage();


                    for (int z = 0; z < ProgressBookHolder.Tabs[i].Pages[j].UnlockableItems.Count; z++)
                    {

                        UnlockableItem unlockableItem = new UnlockableItem();
                        for (int x = 0; x < ProgressBookHolder.Tabs[i].Pages[j].UnlockableItems[z].ItemRequirements.Count; x++)
                        {
                            UnlockableItemRequirement requirement = new UnlockableItemRequirement(GraphicsDevice, Game1.ItemVault.GenerateNewItem(ProgressBookHolder.Tabs[i].Pages[j].UnlockableItems[z].ItemRequirements[x].ItemIDRequired, null),
                           new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width * 2 - 100 + x * 96, this.BackDropPosition.Y + this.BackDropSourceRectangle.Height * 2 - 64), ProgressBookHolder.Tabs[i].Pages[j].UnlockableItems[z].ItemRequirements[x].CountRequired, z, this.BackDropScale);
                            unlockableItem.Requirements.Add(requirement);
                        }

                        Reward reward = new Reward(this.GraphicsDevice, new Vector2(this.BackDropPosition.X + BackDropSourceRectangle.Width * 2, this.BackDropPosition.Y + this.BackDropSourceRectangle.Height * 2), this.BackDropScale);
                        reward.Item = Game1.ItemVault.GenerateNewItem(ProgressBookHolder.Tabs[i].Pages[j].UnlockableItems[z].RewardItemID, null);
                        reward.RewardName = reward.Item.Name;
                        unlockableItem.Reward = reward;
                        unlockableItem.ToolTipSourceRectangle = reward.Item.SourceTextureRectangle;
                        unlockableItem.ToolTipPosition = new Vector2(this.BackDropPosition.X + 32 + z * 64, this.BackDropPosition.Y + 96);
                        unlockableItem.ToolTip = new Button(Game1.AllTextures.ItemSpriteSheet, unlockableItem.ToolTipSourceRectangle, this.GraphicsDevice, unlockableItem.ToolTipPosition, CursorType.Normal, this.BackDropScale);



                        page.UnlockableItems.Add(unlockableItem);

                    }

                    Tabs[i].Pages.Add(page);




                }

            }

            FowardButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1008, 176, 16, 64), this.GraphicsDevice,
                new Vector2(BackDropPosition.X + BackDropSourceRectangle.Width * BackDropScale - 50, BackDropSourceRectangle.Y), CursorType.Normal, this.BackDropScale);
            BackButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(848, 176, 16, 64),
                this.GraphicsDevice, new Vector2(BackDropPosition.X, BackDropSourceRectangle.Y), CursorType.Normal, this.BackDropScale);
            this.redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), GraphicsDevice,
                new Vector2(BackDropPosition.X + BackDropSourceRectangle.Width * BackDropScale - 50, BackDropPosition.Y), CursorType.Normal);

        }

        public void LoadContent(ContentManager content, int iD)
        {
            switch (iD)
            {
                case 1:
                    ProgressBookHolder = content.Load<ProgressBookHolder>("ProgressBook/JulianProgressBook");
                    break;

                case 2:
                    ProgressBookHolder = content.Load<ProgressBookHolder>("ProgressBook/ElixirProgressBook");
                    break;

            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Tabs.Count; i++)
            {
                Tabs[i].Button.Update(Game1.myMouseManager);
                if (Tabs[i].Button.isClicked)
                {
                    this.ActiveTab = i;

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
            }

            Tabs[ActiveTab].Update(gameTime);

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
            redEsc.Update(Game1.myMouseManager);


            if (redEsc.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                Game1.Player.UserInterface.CurrentOpenShop = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.BackDropPosition, this.BackDropSourceRectangle,
                Color.White, 0f, Game1.Utility.Origin, BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            for (int i = 0; i < Tabs.Count; i++)
            {
                Tabs[i].Button.DrawNormal(spriteBatch, Tabs[i].Button.Position, Tabs[i].Button.BackGroundSourceRectangle, Color.White * Tabs[i].ButtonColorMultiplier, 0f, Game1.Utility.Origin, Tabs[i].Button.HitBoxScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            }

            Tabs[ActiveTab].Draw(spriteBatch);

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

            redEsc.Draw(spriteBatch);
        }
    }

    public class CategoryTab
    {
        public ProgressBook ProgressBook { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public bool IsActive { get; set; }
        public int ActivePage { get; set; }
        public List<UnlockableItemPage> Pages { get; set; }

        public Button Button { get; set; }

        public Vector2 PositionToDraw { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public float ButtonColorMultiplier { get; set; }
        public CategoryTab(ProgressBook book, string name, GraphicsDevice graphics, Vector2 positionToDraw, Rectangle sourceRectangle, float scale)
        {
            this.Name = name;
            this.ProgressBook = book;
            this.PositionToDraw = positionToDraw;
            this.SourceRectangle = sourceRectangle;
            this.Button = new Button(Game1.AllTextures.UserInterfaceTileSet, SourceRectangle, graphics, PositionToDraw, CursorType.Normal, scale);
            this.ButtonColorMultiplier = 1f;
            this.ActivePage = 0;
            this.Pages = new List<UnlockableItemPage>();
        }

        public void Update(GameTime gameTime)
        {

            Pages[ActivePage].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.DrawString(Game1.AllTextures.MenuText, ActivePage.ToString(), ProgressBook.BackDropPosition, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Name, new Vector2(ProgressBook.BackDropPosition.X + ProgressBook.BackDropSourceRectangle.Width /8 * ProgressBook.BackDropScale, ProgressBook.BackDropPosition.Y + 32), Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);
            this.Pages[ActivePage].Draw(spriteBatch);


        }


    }

    public class UnlockableItemPage
    {
        public int ActiveItem { get; set; }
        public List<UnlockableItem> UnlockableItems { get; set; }
        public UnlockableItemPage()
        {
            this.ActiveItem = 0;
            UnlockableItems = new List<UnlockableItem>();

        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < UnlockableItems.Count; i++)
            {
                if (UnlockableItems[i].ToolTip.IsHovered)
                {
                    UnlockableItems[i].ToolTipColorMultiplier = .5f;
                }
                else
                {
                    UnlockableItems[i].ToolTipColorMultiplier = 1f;
                }
                UnlockableItems[i].ToolTip.Update(Game1.myMouseManager);
                if (UnlockableItems[i].ToolTip.isClicked)
                {
                    this.ActiveItem = i;
                }
            }

            UnlockableItems[ActiveItem].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < UnlockableItems.Count; i++)
            {

                UnlockableItems[i].ToolTip.DrawNormal(spriteBatch, UnlockableItems[i].ToolTip.Position, UnlockableItems[i].ToolTip.BackGroundSourceRectangle,
                    Color.White * UnlockableItems[i].ToolTipColorMultiplier, 0f, Game1.Utility.Origin, UnlockableItems[i].ToolTip.HitBoxScale,
                    SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);
                //UnlockableItems[i].Draw(spriteBatch);

                if (UnlockableItems[i].Reward.Claimed)
                {

                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, UnlockableItems[i].ToolTip.Position, new Rectangle(208, 256, 32, 32),
                        Color.White, 0f, Game1.Utility.Origin, UnlockableItems[i].ToolTip.HitBoxScale - 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .02f);

                }
            }
            UnlockableItems[ActiveItem].Draw(spriteBatch);
        }
    }

    public class UnlockableItem
    {
        public Button ToolTip { get; set; }
        public float ToolTipColorMultiplier { get; set; }
        public Rectangle ToolTipSourceRectangle { get; set; }
        public Vector2 ToolTipPosition { get; set; }
        public List<UnlockableItemRequirement> Requirements { get; set; }
        public Reward Reward { get; set; }
        public bool RewardSatisfied { get; set; }
        public UnlockableItem()
        {
            Requirements = new List<UnlockableItemRequirement>();
            this.ToolTipColorMultiplier = 1f;
            this.RewardSatisfied = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!Reward.Claimed)
            {
                this.RewardSatisfied = true;
                // ToolTip.Update(Game1.myMouseManager);
                this.ToolTipColorMultiplier = .5f;
                for (int i = 0; i < this.Requirements.Count; i++)
                {
                    Requirements[i].Update(gameTime);
                    if (!Requirements[i].Satisfied)
                    {
                        this.RewardSatisfied = false;
                    }
                }

                Reward.Button.Update(Game1.myMouseManager);
                if (this.RewardSatisfied)
                {
                    Reward.Button.BackGroundSourceRectangle = Reward.UnopenedChestSourceRectangle;
                }
                if (Reward.Button.isClicked && Reward.Button.BackGroundSourceRectangle == Reward.UnopenedChestSourceRectangle)
                {
                    Reward.Button.BackGroundSourceRectangle = Reward.OpenedChestSourceRectangle;
                    Reward.Claimed = true;
                    SoundEffectInstance unlockItemInstance = Game1.SoundManager.UnlockItem.CreateInstance();
                    unlockItemInstance.Volume = Game1.SoundManager.GameVolume/2;
                    unlockItemInstance.Play();
                    unlockItemInstance.Dispose();
                    //Game1.SoundManager.UnlockItem.Play();
                    for (int i = 0; i < Game1.Player.UserInterface.CraftingMenu.Tabs.Length; i++)
                    {
                        for (int j = 0; j < Game1.Player.UserInterface.CraftingMenu.Tabs[i].Pages.Count; j++)
                        {
                            for (int z = 0; z < Game1.Player.UserInterface.CraftingMenu.Tabs[i].Pages[j].ToolTips.Count; z++)
                            {
                                if (Game1.Player.UserInterface.CraftingMenu.Tabs[i].Pages[j].ToolTips[z].Item.ID == Reward.Item.ID)
                                {
                                    Game1.Player.UserInterface.CraftingMenu.Tabs[i].Pages[j].ToolTips[z].Locked = false;
                                }
                            }
                        }
                    }

                }
            }




        }


        public void Draw(SpriteBatch spriteBatch)
        {



            for (int i = 0; i < this.Requirements.Count; i++)
            {
                Requirements[i].Draw(spriteBatch);
            }

            Reward.Draw(spriteBatch);
            ToolTip.DrawNormal(spriteBatch, new Vector2(Reward.PositionToDraw.X, Reward.PositionToDraw.Y - 200), ToolTip.BackGroundSourceRectangle,
                    Color.White, 0f, Game1.Utility.Origin, 6f,
                    SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);
            if (Reward.Claimed)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(Reward.PositionToDraw.X, Reward.PositionToDraw.Y - 200), new Rectangle(208, 256, 32, 32),
                    Color.White, 0f, Game1.Utility.Origin, 6f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .02f);
            }

        }
    }

    public class UnlockableItemRequirement
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public Button Button { get; set; }
        public Button ItemDropButton { get; set; }
        public Rectangle ItemDropButtonSourceRectangle { get; set; }
        public bool Satisfied { get; set; }
        public Item Item { get; set; }
        public Vector2 ButtonPosition { get; set; }
        public int CurrentCount { get; set; }
        public int CountRequired { get; set; }
        public float ColorMultiplier { get; set; }
        public int Index { get; set; }
        public float Scale { get; set; }

        public UnlockableItemRequirement(GraphicsDevice graphics, Item item, Vector2 buttonPosition, int countRequired, int index, float scale)
        {
            this.GraphicsDevice = graphics;
            this.Satisfied = false;
            this.Item = item;
            this.ButtonPosition = buttonPosition;
            this.CurrentCount = 0;
            this.CountRequired = countRequired;
            this.ColorMultiplier = 1f;
            this.Index = index;
            this.Scale = scale;
            this.Button = new Button(Game1.AllTextures.ItemSpriteSheet, Item.SourceTextureRectangle, this.GraphicsDevice, this.ButtonPosition, CursorType.Normal, this.Scale);
            this.ItemDropButtonSourceRectangle = new Rectangle(464, 320, 32, 32);
            this.ItemDropButton = new Button(Game1.AllTextures.UserInterfaceTileSet, this.ItemDropButtonSourceRectangle, this.GraphicsDevice, this.ButtonPosition, CursorType.Normal, this.Scale);
        }

        public void Update(GameTime gameTime)
        {
            Button.Update(Game1.myMouseManager);
            this.ItemDropButton.Update(Game1.myMouseManager);

            if (!this.Satisfied)
            {
                if (ItemDropButton.IsHovered)
                {
                    if (Game1.Player.UserInterface.BottomBar.WasSlotJustReleased)
                    {
                        if (Game1.Player.UserInterface.BottomBar.ItemJustReleased.ID == this.Item.ID)
                        {

                            this.CurrentCount++;
                            Game1.Player.Inventory.RemoveItem(this.Item.ID);
                        }
                        // Console.WriteLine(Game1.Player.UserInterface.BottomBar.ItemJustReleased.Name + "was just released");
                    }
                }
            }

            if (this.CurrentCount >= CountRequired)
            {
                this.Satisfied = true;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Button.DrawNormal(spriteBatch, Button.Position, Button.BackGroundSourceRectangle, Color.White * ColorMultiplier, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, .95f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.CurrentCount.ToString() + "/" + this.CountRequired.ToString(),
                new Vector2(Button.Position.X, Button.Position.Y + 50), Color.Black, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, .95f);

            this.ItemDropButton.DrawNormal(spriteBatch, ItemDropButton.Position, ItemDropButton.BackGroundSourceRectangle, Color.White * .5f, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, .95f);
        }
    }

    public class Reward
    {
        public string RewardName { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public Button Button { get; set; }
        public bool AvailableToClaim { get; set; }
        public bool Claimed { get; set; }

        public Vector2 PositionToDraw { get; set; }
        public float Scale { get; set; }
        public Rectangle InvisibleChestSourceRectangle { get; set; }
        public Rectangle UnopenedChestSourceRectangle { get; set; }
        public Rectangle OpenedChestSourceRectangle { get; set; }

        public Item Item { get; set; }

        public Reward(GraphicsDevice graphicsDevice, Vector2 positionToDraw, float scale)
        {
            this.GraphicsDevice = graphicsDevice;
            this.AvailableToClaim = false;
            this.Claimed = false;

            this.PositionToDraw = positionToDraw;
            this.Scale = scale;
            InvisibleChestSourceRectangle = new Rectangle(496, 320, 32, 32);
            UnopenedChestSourceRectangle = new Rectangle(528, 320, 32, 32);
            OpenedChestSourceRectangle = new Rectangle(560, 320, 32, 32);

            Button = new Button(Game1.AllTextures.UserInterfaceTileSet, InvisibleChestSourceRectangle, this.GraphicsDevice, this.PositionToDraw, CursorType.Normal, this.Scale);


        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Button.DrawNormal(spriteBatch, Button.Position, Button.BackGroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, .95f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.RewardName,
                new Vector2(Button.Position.X, Button.Position.Y - 300), Color.Black, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, .95f);
        }
    }
}
