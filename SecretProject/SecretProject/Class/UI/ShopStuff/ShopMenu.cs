using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.UI.ButtonStuff;
using SecretProject.Class.UI.ShopStuff;
using SecretProject.Class.Universal;
using System.Collections.Generic;

namespace SecretProject.Class.UI.ShopStuff
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
            this.ShopBackDropSourceRectangle = new Rectangle(1232, 80, 272, 224);
            this.BackDropScale = 3f;
            ShopMenuPosition = Game1.Utility.CenterRectangleOnScreen(this.ShopBackDropSourceRectangle, this.BackDropScale);
            Name = name;
            redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), graphicsDevice,
                new Vector2(ShopMenuPosition.X + this.ShopBackDropSourceRectangle.Width * this.BackDropScale - 48, this.ShopMenuPosition.Y + 16), CursorType.Normal);
            mainFont = Game1.AllTextures.MenuText;

            
            Font = Game1.AllTextures.MenuText;

            //ShopTextBox = new TextBox(Game1.AllTextures.MenuText, )

            allShopMenuItemButtons = new List<Button>();
            this.MaxMenuSlotsPerPage = 8;
            this.Pages = new List<List<ShopMenuSlot>>() { new List<ShopMenuSlot>() };
            this.FowardButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(384, 528, 32, 16), this.Graphics,
                new Vector2(ShopMenuPosition.X -128 + this.ShopBackDropSourceRectangle.Width * this.BackDropScale, this.ShopMenuPosition.Y + this.ShopBackDropSourceRectangle.Height* this.BackDropScale - 80), CursorType.Normal, this.BackDropScale);
            this.BackButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(304, 528, 32, 16),
                this.Graphics, new Vector2(ShopMenuPosition.X - 224 + this.ShopBackDropSourceRectangle.Width * this.BackDropScale, this.ShopMenuPosition.Y + this.ShopBackDropSourceRectangle.Height * this.BackDropScale - 80), CursorType.Normal, this.BackDropScale);
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

                int yMultiplier = 0;
                int xCount = 0;
                for (int i = 0; i < this.Pages.Count; i++)
                {
                    if (this.Pages[i].Count < this.MaxMenuSlotsPerPage)
                    {
                        xCount = this.Pages[i].Count;
                        if(this.Pages[i].Count > 3)
                        {
                            yMultiplier = 96;
                            xCount = 7 - this.Pages[i].Count;
                        }
                        this.Pages[i].Add(new ShopMenuSlot(this.Graphics, count, id, new Vector2(ShopMenuPosition.X + 48 + 64 * xCount * this.BackDropScale, ShopMenuPosition.Y + 48 + yMultiplier * this.BackDropScale), this.BackDropScale ));
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
                Game1.Utility.StandardButtonDepth - .04f);
            for (int i = 0; i < this.Pages[this.CurrentPage].Count; i++)
            {
                this.Pages[this.CurrentPage][i].Draw(spriteBatch, this.BackDropScale);
            }
            if (this.CurrentPage >= this.Pages.Count - 1)
            {
                this.FowardButton.DrawNormal(spriteBatch, this.FowardButton.Position, this.FowardButton.BackGroundSourceRectangle, Color.White * .5f,
                0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            }
            else
            {
                this.FowardButton.DrawNormal(spriteBatch, this.FowardButton.Position, this.FowardButton.BackGroundSourceRectangle, Color.White,
                0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            }
            if (this.CurrentPage <= 0)
            {
                this.BackButton.DrawNormal(spriteBatch, this.BackButton.Position, this.BackButton.BackGroundSourceRectangle, Color.White * .5f,
               0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);

            }
            else
            {
                this.BackButton.DrawNormal(spriteBatch, this.BackButton.Position, this.BackButton.BackGroundSourceRectangle, Color.White,
               0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);

            }
            redEsc.Draw(spriteBatch);

        }



        public void TrySellToShop(Item item, int amountToSell)
        {
            TryAddStock(item.ID, amountToSell);
            for (int i = 0; i < amountToSell; i++)
            {
                Game1.Player.Inventory.Money += Game1.ItemVault.GetItem(item.ID).Price;
            }
            Game1.Player.UserInterface.AllUISprites.Add(new UISprite(UISpriteType.Coin, Graphics, Game1.Player.UserInterface.BottomBar.GoldIconPosition,
                new Vector2(Game1.Player.UserInterface.BottomBar.GoldIconPosition.X, Game1.Player.UserInterface.BottomBar.GoldIconPosition.Y - 50),
                Game1.Player.UserInterface.AllUISprites, 1, 3));
            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.Sell1);

        }


    }

    
}
