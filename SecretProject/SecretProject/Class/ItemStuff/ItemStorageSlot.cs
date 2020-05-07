using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.UI;

namespace SecretProject.Class.ItemStuff
{
    public class ItemStorageSlot
    {
        public bool Retrievable { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public InventorySlot Slot { get; set; }
        public int Index { get; set; }
        public Vector2 Position { get; set; }
        public float Scale { get; set; }
        public Button Button { get; set; }
        public Rectangle BackGroundSourceRectangle { get; set; }
        public Rectangle ItemSourceRectangleToDraw { get; set; }
        public int ItemCounter { get; set; }


        public ItemStorageSlot(GraphicsDevice graphics, InventorySlot slot, Vector2 position, Rectangle backGroundSourceRectangle, float scale, bool retrievable)
        {
            this.GraphicsDevice = graphics;
            this.Slot = slot;
            this.Position = position;
            this.Scale = scale;
            this.BackGroundSourceRectangle = backGroundSourceRectangle;
            this.ItemSourceRectangleToDraw = new Rectangle(0, 0, 1, 1);
            this.Button = new Button(Game1.AllTextures.UserInterfaceTileSet, backGroundSourceRectangle, this.GraphicsDevice, this.Position, CursorType.Normal, this.Scale);
            this.Retrievable = retrievable;
        }

        /// <summary>
        /// Emptys contents of inventory slot to the specified inventory.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        private bool ShiftClickInteraction(InventorySlot slot, Inventory destinationInventory)
        {
            if (Game1.KeyboardManager.OldKeyBoardState.IsKeyDown(Keys.LeftShift))
            {
                Item item = slot.GetItem();
                if (item != null)
                {
                    for (int shiftItem = slot.ItemCount - 1; shiftItem >= 0; shiftItem--)
                    {
                        if (destinationInventory.TryAddItem(item))
                        {
                            slot.RemoveItemFromSlot();
                        }
                        else
                        {
                            break;
                        }

                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Update(GameTime gameTime, StorageManager storageManager, Inventory destinationInventory, ItemStorageSlot storageSlotHovered, DragSlot dragSlot)
        {
            this.Button.Update(Game1.MouseManager);
            if (this.Button.IsHovered)
            {
                if(dragSlot != null)
                {

                }
                if (this.Slot.ItemCount > 0)
                {
                    if (Slot.ItemCount > 0)
                    {
                        storageSlotHovered = this;

                        if (this.Button.isClicked)
                        {
                            if (this.Retrievable)
                            {

                                if (!ShiftClickInteraction(Slot, destinationInventory)) //if slot was not shift clicked we'll grab the tooltip of the item.
                                {
                                    dragSlot.Index = this.Index;
                                    dragSlot.InventorySlot = Slot;
                                }

                            }

                        }

                    }

                }

            }


            if (Slot.ItemCount != 0)
            {
                this.Button.Texture = Slot.Item.ItemSprite.AtlasTexture;
                this.Button.ItemSourceRectangleToDraw = Slot.Item.SourceTextureRectangle;
            }
            else
            {
                this.Button.Texture = Game1.AllTextures.UserInterfaceTileSet;
                this.Button.ItemSourceRectangleToDraw = new Rectangle(32, 0, 1, 1);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Button.Draw(spriteBatch, this.Button.ItemSourceRectangleToDraw, this.Button.BackGroundSourceRectangle, Game1.AllTextures.MenuText, Slot.ItemCount.ToString(),
                this.Button.Position, Color.White, this.Scale, this.Scale);

        }

    }
}
