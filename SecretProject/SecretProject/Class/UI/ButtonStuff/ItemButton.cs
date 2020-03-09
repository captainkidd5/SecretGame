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

namespace SecretProject.Class.UI.ButtonStuff
{
    public class ItemButton : Button
    {
        public int CurrentCount { get; set; }
        public int CountNeeded { get; set; }

        public Vector2 TextPosition { get; set; }
        public string StringToWrite { get; set; }

        public ItemButton(GraphicsDevice graphicsDevice, Vector2 position, int countNeeded, float scale, Item item)
        {
            this.Texture = Game1.AllTextures.ItemSpriteSheet;
            Position = position;

            size = new Vector2((graphicsDevice.Viewport.Width / 10), (graphicsDevice.Viewport.Height / 11));


                this.HitBoxScale = 1f;

                this.HitBoxScale = scale;

            UpdateHitBoxRectanlge(this.BackGroundSourceRectangle);
  
            this.CursorType = CursorType.Normal;
 

                this.Item = item;
                this.ItemSourceRectangleToDraw = this.Item.SourceTextureRectangle;
            this.StringToWrite = string.Empty;
            this.CountNeeded = countNeeded;
        }
        public override void Update(MouseManager mouseManager)
        {
            base.Update(mouseManager);
            this.TextPosition = new Vector2(this.Position.X, this.Position.Y + 32);
     
            UpdateString();
        }

        public void UpdateString()
        {
            this.CurrentCount = Game1.Player.Inventory.FindNumberOfItemInInventory(this.Item.ID);
            this.StringToWrite = this.CurrentCount.ToString() + "/" + this.CountNeeded.ToString();
        }

        public void DrawItemButton(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, this.ItemSourceRectangleToDraw,
                this.Color, 0f, Game1.Utility.Origin, this.HitBoxScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .02f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.StringToWrite, this.TextPosition, Color.White, 0f, Game1.Utility.Origin, this.HitBoxScale, SpriteEffects.None,
                Game1.Utility.StandardTextDepth + .02f);
        }
    }
}
