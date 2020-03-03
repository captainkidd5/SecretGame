using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;

namespace SecretProject.Class.ItemStuff
{
    public class ItemStorageSlot
    {
        public bool Retrievable { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public Inventory Inventory { get; set; }
        public int Index { get; set; }
        public int Count { get; set; }
        public Vector2 Position { get; set; }
        public float Scale { get; set; }
        public Button Button { get; set; }
        public Rectangle BackGroundSourceRectangle { get; set; }
        public Rectangle ItemSourceRectangleToDraw { get; set; }
        public int ItemCounter { get; set; }


        public ItemStorageSlot(GraphicsDevice graphics, Inventory inventory, int index, Vector2 position, Rectangle backGroundSourceRectangle, float scale, bool retrievable)
        {
            this.GraphicsDevice = graphics;
            this.Inventory = inventory;
            this.Index = index;
            this.Count = 0;
            this.Position = position;
            this.Scale = scale;
            this.BackGroundSourceRectangle = backGroundSourceRectangle;
            this.ItemSourceRectangleToDraw = new Rectangle(0, 0, 1, 1);
            this.Button = new Button(Game1.AllTextures.UserInterfaceTileSet, backGroundSourceRectangle, this.GraphicsDevice, this.Position, CursorType.Normal, this.Scale);
            this.Retrievable = retrievable;
        }

        public void Update(GameTime gameTime)
        {
            this.Button.Update(Game1.MouseManager);
            if (this.Inventory.currentInventory.Count > 0)
            {
                InventorySlot slot = this.Inventory.currentInventory[this.Index];
                this.Count = slot.ItemCount;
                if (slot.ItemCount > 0)
                {
                    if (this.Button.isClicked)
                    {
                        if (this.Retrievable)
                        {

                            if (Game1.KeyboardManager.OldKeyBoardState.IsKeyDown(Keys.LeftShift))
                            {
                                Item item = slot.GetItem();
                                if (item != null)
                                {


                                    for (int shiftItem = slot.ItemCount - 1; shiftItem >= 0; shiftItem--)
                                    {
                                        if (Game1.Player.Inventory.TryAddItem(item))
                                        {
                                            slot.RemoveItemFromSlot();
                                            this.Count--;
                                        }
                                        else
                                        {
                                            break;
                                        }

                                    }
                                }
                            }
                        }

                    }
                }

                if (this.Count != 0)
                {
                    this.Button.Texture = slot.Item.ItemSprite.AtlasTexture;
                    this.Button.ItemSourceRectangleToDraw = slot.Item.SourceTextureRectangle;
                }
                else
                {
                    this.Button.Texture = Game1.AllTextures.UserInterfaceTileSet;
                    this.Button.ItemSourceRectangleToDraw = new Rectangle(32, 0, 1, 1);
                }

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Button.Draw(spriteBatch, this.Button.ItemSourceRectangleToDraw, this.Button.BackGroundSourceRectangle, Game1.AllTextures.MenuText, this.Count.ToString(),
                this.Button.Position, Color.White, this.Scale, this.Scale);

        }

    }
}
