using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.Universal;
using System.Collections.Generic;

namespace SecretProject.Class.UI
{
    public class ShopMenu : IExclusiveInterfaceComponent
    {
        public GraphicsDevice Graphics { get; set; }
        public List<Button> allShopMenuItemButtons;
        public List<List<ShopMenuSlot>> Pages { get; set; }
        public int MaxMenuSlotsPerPage { get; set; }
        public int CurrentPage { get; set; }
        //private Button shopMenuItemButton;
        private Button redEsc;
        private SpriteFont mainFont;
        public string Name;
        public Vector2 ShopMenuPosition;
        public Rectangle ShopBackDropSourceRectangle { get; set; }
        public float BackDropScale { get; set; }

        public SpriteFont Font;


        public TextBox ShopTextBox { get; set; }
        public bool IsActive { get; set; }
        public bool FreezesGame { get; set; }

        public Button FowardButton { get; set; }
        public Button BackButton { get; set; }

        public ShopMenu(string name, GraphicsDevice graphicsDevice, int inventorySlotCount)
        {
            this.Graphics = graphicsDevice;
            //this.shopMenuItemButton = new Button(Game1.AllTextures.ShopMenuItemButton, graphicsDevice, new Vector2(Utility.centerScreenX, Utility.centerScreenY));
            ShopMenuPosition = new Vector2(Game1.PresentationParameters.BackBufferWidth / 3, 0);
            Name = name;
            redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), graphicsDevice,
                new Vector2(ShopMenuPosition.X + this.ShopBackDropSourceRectangle.Width * this.BackDropScale + 400, this.ShopBackDropSourceRectangle.Y + 100), CursorType.Normal);
            mainFont = Game1.AllTextures.MenuText;

            this.ShopBackDropSourceRectangle = new Rectangle(864, 80, 144, 240);
            this.BackDropScale = 3f;
            Font = Game1.AllTextures.MenuText;

            //ShopTextBox = new TextBox(Game1.AllTextures.MenuText, )

            allShopMenuItemButtons = new List<Button>();
            this.MaxMenuSlotsPerPage = 5;
            this.Pages = new List<List<ShopMenuSlot>>() { new List<ShopMenuSlot>() };
            this.FowardButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(1008, 176, 16, 64), this.Graphics,
                new Vector2(ShopMenuPosition.X + this.ShopBackDropSourceRectangle.Width * this.BackDropScale, this.ShopBackDropSourceRectangle.Y), CursorType.Normal, this.BackDropScale);
            this.BackButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(848, 176, 16, 64),
                this.Graphics, new Vector2(ShopMenuPosition.X - 48, this.ShopBackDropSourceRectangle.Y), CursorType.Normal, this.BackDropScale);
            this.FreezesGame = true;
            this.IsActive = false;
        }

        public void TryAddStock(int id, int count)
        {
            bool added = false;
            for (int i = 0; i < this.Pages.Count; i++)
            {
                for (int p = 0; p < this.Pages[i].Count; p++)
                {
                    if (this.Pages[i][p].ItemID == id)
                    {
                        this.Pages[i][p].Stock += count;
                        added = true;
                        return;
                    }
                }
            }
            if (!added)
            {


                for (int i = 0; i < this.Pages.Count; i++)
                {
                    if (this.Pages[i].Count < this.MaxMenuSlotsPerPage)
                    {
                        this.Pages[i].Add(new ShopMenuSlot(this.Graphics, count, id, new Vector2(ShopMenuPosition.X, ShopMenuPosition.Y + 128 + (96 * this.Pages[i].Count)), this.BackDropScale + 1f));
                        return;
                    }
                }
                this.Pages.Add(new List<ShopMenuSlot>());
                TryAddStock(id, count);
            }
        }


        public void Update(GameTime gameTime, MouseManager mouse)
        {
            for (int i = 0; i < this.Pages[this.CurrentPage].Count; i++)
            {
                this.Pages[this.CurrentPage][i].Update(gameTime, mouse);

            }
            this.FowardButton.Update(mouse);

            if (this.FowardButton.isClicked)
            {
                if (this.CurrentPage < this.Pages.Count - 1)
                {
                    this.CurrentPage++;
                }

            }


            this.BackButton.Update(mouse);

            if (this.BackButton.isClicked)
            {
                if (this.CurrentPage > 0)
                {
                    this.CurrentPage--;
                }
            }




            redEsc.Update(mouse);


            if (redEsc.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
                Game1.Player.UserInterface.CurrentOpenShop = 0;
            }


        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, ShopMenuPosition, this.ShopBackDropSourceRectangle, Color.White,
                0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None,
                Utility.StandardButtonDepth - .04f);
            for (int i = 0; i < this.Pages[this.CurrentPage].Count; i++)
            {
                this.Pages[this.CurrentPage][i].Draw(spriteBatch, this.BackDropScale);
            }
            if (this.CurrentPage >= this.Pages.Count - 1)
            {
                this.FowardButton.DrawNormal(spriteBatch, this.FowardButton.Position, this.FowardButton.BackGroundSourceRectangle, Color.White * .5f,
                0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Utility.StandardButtonDepth);
            }
            else
            {
                this.FowardButton.DrawNormal(spriteBatch, this.FowardButton.Position, this.FowardButton.BackGroundSourceRectangle, Color.White,
                0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Utility.StandardButtonDepth);
            }
            if (this.CurrentPage <= 0)
            {
                this.BackButton.DrawNormal(spriteBatch, this.BackButton.Position, this.BackButton.BackGroundSourceRectangle, Color.White * .5f,
               0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Utility.StandardButtonDepth);

            }
            else
            {
                this.BackButton.DrawNormal(spriteBatch, this.BackButton.Position, this.BackButton.BackGroundSourceRectangle, Color.White,
               0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Utility.StandardButtonDepth);

            }
            redEsc.Draw(spriteBatch);

        }



        public void TrySellToShop(Item item, int amountToSell)
        {
            TryAddStock(item.ID, amountToSell);
            for (int i = 0; i < amountToSell; i++)
            {
                Game1.Player.Inventory.Money += item.Price;
            }

            Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.Sell1);

        }


    }

    public class ShopMenuSlot
    {

        public int ItemID { get; set; }
        public Button Button { get; set; }
        public Vector2 drawPosition;
        public int Stock;
        public float colorMultiplier;
        public Rectangle BackgroundSourceRectangle { get; set; }
        public Item Item { get; set; }
        public ShopMenuSlot(GraphicsDevice graphics, int stock, int itemID, Vector2 drawPosition, float buttonScale)
        {
            Item item = Game1.ItemVault.GenerateNewItem(itemID, null);
            this.Item = item;
            this.ItemID = itemID;
            this.Button = new Button(item.ItemSprite.AtlasTexture, item.SourceTextureRectangle, graphics, drawPosition, CursorType.Currency, buttonScale);
            Stock = stock;
            this.drawPosition = drawPosition;
            colorMultiplier = .25f;
            this.BackgroundSourceRectangle = new Rectangle(864, 48, 113, 32);
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            this.Button.Update(mouse);
            if (Stock > 0 && this.Button.IsHovered)
            {
                Game1.Player.UserInterface.InfoBox.IsActive = true;
                Game1.Player.UserInterface.InfoBox.FitText(this.Item.Description, 2f);
                // Game1.Player.UserInterface.InfoBox.StringToWrite = Item.Name + "\n " + Item.Description;
                Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(drawPosition.X - 232, drawPosition.Y);
                colorMultiplier = .5f;
                if (this.Button.isClicked)
                {
                    Item item = Game1.ItemVault.GenerateNewItem(this.ItemID, null);
                    if (Game1.Player.Inventory.Money >= item.Price)
                    {
                        if (Game1.Player.Inventory.TryAddItem(item))
                        {
                            Stock--;
                            Game1.Player.Inventory.Money -= item.Price;
                            Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.Sell1);
                        }

                        //Game1.SoundManager.Sell1.Play();
                    }
                }
            }
            else
            {
                colorMultiplier = 1f;
            }
        }

        public void Draw(SpriteBatch spriteBatch, float backDropScale)
        {
            this.Button.Draw(spriteBatch, this.Button.BackGroundSourceRectangle, this.BackgroundSourceRectangle, Game1.AllTextures.MenuText,
               Stock.ToString() + "\n \n                   " + this.Item.Name + "\n \n                   Price: " + this.Item.Price, drawPosition,
               Color.White * colorMultiplier, backDropScale, this.Button.HitBoxScale, layerDepthCustom: Utility.StandardButtonDepth);
        }

    }
}
