using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.ShopStuff
{
    public class ShopMenuSlot
    {
        public GraphicsDevice Graphics { get; set; }
        public int ItemID { get; set; }
        public Button Button { get; set; }
        public Vector2 drawPosition;
        public int Stock;
        public float colorMultiplier;
        public Rectangle BackgroundSourceRectangle { get; set; }
        public Item Item { get; set; }
        public ShopMenuSlot(GraphicsDevice graphics, int stock, int itemID, Vector2 drawPosition, float buttonScale)
        {
            this.Graphics = graphics;
            Item item = Game1.ItemVault.GenerateNewItem(itemID, null);
            this.Item = item;
            this.ItemID = itemID;
            this.BackgroundSourceRectangle = new Rectangle(1168, 96, 48, 48);
            this.Button = new Button(Game1.AllTextures.ItemSpriteSheet, BackgroundSourceRectangle, graphics, drawPosition, CursorType.Currency, buttonScale, this.Item);
            Stock = stock;
            this.drawPosition = drawPosition;
            colorMultiplier = .25f;
            
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            this.Button.Update(mouse);
            if (Stock > 0 && this.Button.IsHovered)
            {
                InfoPopUp infoBox = new InfoPopUp(this.Graphics, Game1.ItemVault.GetItem(this.ItemID), new Vector2(mouse.UIPosition.X + 64, mouse.UIPosition.Y + 64));


                Game1.Player.UserInterface.InfoBox = infoBox;
                Game1.Player.UserInterface.InfoBox.IsActive = true;
                colorMultiplier = .5f;
                if (this.Button.isClicked)
                {
                    Item item = Game1.ItemVault.GenerateNewItem(this.ItemID, null);
                    if (Game1.Player.Inventory.Money >= Game1.ItemVault.GetItem(item.ID).Price)
                    {
                        if (Game1.Player.Inventory.TryAddItem(item))
                        {
                            Stock--;
                            Game1.Player.Inventory.Money -= Game1.ItemVault.GetItem(item.ID).Price;
                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.Sell1);
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
            this.Button.Draw(spriteBatch, this.Button.ItemSourceRectangleToDraw, Button.BackGroundSourceRectangle, Game1.AllTextures.MenuText,
               Stock.ToString() + "\n \n                   " + Game1.ItemVault.GetItem(Item.ID).Name + "\n \n                   Price: " + Game1.ItemVault.GetItem(Item.ID).Price, drawPosition,
               Color.White * colorMultiplier, backDropScale, this.Button.HitBoxScale, layerDepthCustom: Game1.Utility.StandardButtonDepth);
        }

    }
}
