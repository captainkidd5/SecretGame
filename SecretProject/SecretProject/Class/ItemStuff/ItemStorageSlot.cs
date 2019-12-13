using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff
{
    public class ItemStorageSlot
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public Inventory Inventory { get; set; }
        public int Index { get; set; }
        public int Count { get; set; }
        public Vector2 Position { get; set; }
        public float Scale { get; set; }
        public Button Button { get; set; }


        public ItemStorageSlot(GraphicsDevice graphics, Inventory inventory, int index, Vector2 position, float scale)
        {
            this.GraphicsDevice = graphics;
            this.Inventory = inventory;
            this.Index = index;
            this.Count = 0;
            this.Position = position;
            this.Scale = scale;
            this.Button = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(320, 32, 32, 32), this.GraphicsDevice, this.Position, CursorType.Normal, this.Scale);
        }

        public void Update(GameTime gameTime)
        {
            if (this.Button.isClicked)
            {
                if (this.Inventory.currentInventory[this.Index].SlotItems.Count > 0 && this.Inventory.currentInventory[this.Index].SlotItems[0] != null)
                {
                    if (Game1.Player.Inventory.TryAddItem(Inventory.currentInventory[this.Index].SlotItems[0]))
                    {
                        this.Inventory.currentInventory[this.Index].RemoveItemFromSlot();
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Button.Draw(spriteBatch, Button.ItemSourceRectangleToDraw, Button.BackGroundSourceRectangle, Game1.AllTextures.MenuText, this.Count.ToString(),
                Button.Position, Color.White, this.Scale, this.Scale);
        }

    }
}
