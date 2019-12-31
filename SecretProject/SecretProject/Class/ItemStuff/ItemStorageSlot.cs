using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            Button.Update(Game1.myMouseManager);
            if (Inventory.currentInventory.Count > 0)
            {
                this.Count = Inventory.currentInventory[Index].SlotItems.Count;
                if (Inventory.currentInventory[Index].SlotItems.Count > 0)
                {
                    Button.ItemSourceRectangleToDraw = Inventory.currentInventory[Index].SlotItems[0].SourceTextureRectangle;
                    if (this.Button.isClicked)
                    {
                        if(Retrievable)
                        {

                            if (Game1.OldKeyBoardState.IsKeyDown(Keys.LeftShift))
                            {
                                Item item = Inventory.currentInventory[Index].GetItem();
                                if (item != null)
                                {


                                    for (int shiftItem = Inventory.currentInventory[Index].SlotItems.Count - 1; shiftItem >= 0; shiftItem--)
                                    {
                                        if (Game1.Player.Inventory.TryAddItem(item))
                                        {
                                            Inventory.currentInventory[Index].SlotItems.RemoveAt(shiftItem);
                                            Count--;
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

                if (Count != 0)
                {
                    Button.Texture = Inventory.currentInventory[Index].SlotItems[0].ItemSprite.AtlasTexture;
                    Button.ItemSourceRectangleToDraw = Inventory.currentInventory[Index].SlotItems[0].SourceTextureRectangle;
                }
                else
                {
                    Button.Texture = Game1.AllTextures.UserInterfaceTileSet;
                    Button.ItemSourceRectangleToDraw = new Rectangle(0, 0, 1, 1);
                }

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Button.Draw(spriteBatch, Button.ItemSourceRectangleToDraw, Button.BackGroundSourceRectangle, Game1.AllTextures.MenuText, this.Count.ToString(),
                Button.Position, Color.White, this.Scale, this.Scale);

            //Button.Draw(spriteBatch, ItemSourceRectangleToDraw, BackGroundSourceRectangle, Game1.AllTextures.MenuText, ItemCounter.ToString(),
            //      Position, Color.White, Scale, Scale);
        }

    }
}
