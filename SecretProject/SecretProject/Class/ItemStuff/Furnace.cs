using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Universal;

namespace SecretProject.Class.ItemStuff
{
    public class Furnace : IStorableItemBuilding
    {
        public string ID { get; set; }
        public StorableItemType StorableItemType { get; set; }
        public Inventory Inventory { get; set; }
        public List<ItemStorageSlot> ItemSlots { get; set; }
        public bool IsInventoryHovered { get; set; }
        public bool IsUpdating { get; set; }
        public bool FreezesGame { get; set; }
        public int Size { get; set; }
        public Vector2 Location { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public Rectangle BackDropSourceRectangle { get; set; }
        public Vector2 BackDropPosition { get; set; }
        public float BackDropScale { get; set; }

        public SimpleTimer SimpleTimer { get; set; }
        public ItemStorageSlot SmeltSlot{ get; set; }

        public ItemStorageSlot CurrentHoveredSlot { get; set; }
        public Furnace(string iD, int size, Vector2 location, GraphicsDevice graphics)
        {
            this.StorableItemType = StorableItemType.Cauldron;
            this.ID = iD;
            this.Size = size;
            this.Inventory = new Inventory(Size);
            this.Location = location;
            this.GraphicsDevice = graphics;
            this.BackDropSourceRectangle = new Rectangle(224, 128, 48, 96);
            this.BackDropPosition = new Vector2(Game1.ScreenWidth / 6, Game1.ScreenHeight / 6);
            this.BackDropScale = 3f;

            //this.redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), GraphicsDevice,
            //    new Vector2(BackDropPosition.X + BackDropSourceRectangle.Width * BackDropScale - 50, BackDropPosition.Y), CursorType.Normal);

            ItemSlots = new List<ItemStorageSlot>();
            for (int i = 0; i < 1; i++)
            {
                ItemSlots.Add(new ItemStorageSlot(graphics, this.Inventory, i, new Vector2(this.BackDropPosition.X + i * 32 * BackDropScale, this.BackDropPosition.Y * BackDropScale), new Rectangle(208,80, 32,32), BackDropScale, true));

            }
            SimpleTimer = new SimpleTimer(5f);
            SmeltSlot = new ItemStorageSlot(graphics, new Inventory(1), 0, new Vector2(this.BackDropPosition.X + 32 * BackDropScale, this.BackDropPosition.Y * BackDropScale), new Rectangle(208, 80, 32, 32), BackDropScale, false);
        }

        public void Update(GameTime gameTime)
        {
            IsInventoryHovered = false;
            if (!Game1.Player.ClickRangeRectangle.Intersects(new Rectangle((int)this.Location.X, (int)this.Location.Y, 16, 16)))
            {
                this.IsUpdating = false;
            }


            for (int i = 0; i < ItemSlots.Count; i++)
            {
                ItemSlots[i].Update(gameTime);
                if(ItemSlots[i].Button.IsHovered)
                {

                        IsInventoryHovered = true;
                    CurrentHoveredSlot = ItemSlots[i];
                    
                }
            }

            SmeltSlot.Update(gameTime);
            if (SmeltSlot.Button.IsHovered)
            {
                CurrentHoveredSlot = SmeltSlot;
                IsInventoryHovered = true;

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.BackDropPosition, this.BackDropSourceRectangle,
                Color.White, 0f, Game1.Utility.Origin, BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth - .02f);

            for (int i = 0; i < ItemSlots.Count; i++)
            {
                ItemSlots[i].Draw(spriteBatch);
            }

            SmeltSlot.Draw(spriteBatch);
        }

        
    }
}
