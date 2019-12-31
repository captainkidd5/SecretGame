using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.TileStuff;
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

        public Vector2 TimerStringLocation { get; set; }
        public Tile Tile { get; set; }

        private Button redEsc;
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
                ItemSlots.Add(new ItemStorageSlot(graphics, this.Inventory, i, new Vector2(this.BackDropPosition.X + BackDropSourceRectangle.Width / 2, this.BackDropPosition.Y + BackDropSourceRectangle.Height + 32 * BackDropScale), new Rectangle(208,80, 32,32), BackDropScale, true));

            }
            SimpleTimer = new SimpleTimer(5f);
            SmeltSlot = new ItemStorageSlot(graphics, new Inventory(1), 0, new Vector2(this.BackDropPosition.X + BackDropSourceRectangle.Width / 2, this.BackDropPosition.Y ), new Rectangle(208, 80, 32, 32), BackDropScale, true);
            TimerStringLocation = new Vector2(this.BackDropPosition.X + BackDropSourceRectangle.Width, this.BackDropPosition.Y + BackDropSourceRectangle.Height);

            this.redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), graphics,
               new Vector2(this.BackDropPosition.X + BackDropSourceRectangle.Width * BackDropScale, this.BackDropPosition.Y), CursorType.Normal);
        }
        public void Activate(Tile tile)
        {
            IsUpdating = true;
            Tile = tile;
            //Tile.SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(1752, 100);

        }

        public void Deactivate()
        {

        }

        public bool IsItemAllowedToBeStored(Item item)
        {
            return true;
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
            if(SmeltSlot.Inventory.currentInventory[0].SlotItems.Count > 0 && ItemSlots[0].Inventory.currentInventory[0].SlotItems.Count > 0)
            {
                if (SmeltSlot.Inventory.currentInventory[0].SlotItems[0].SmeltedItem != 0 && ItemSlots[0].Inventory.currentInventory[0].SlotItems[0].FuelValue > 0)
                {
                    if (SimpleTimer.Run(gameTime))
                    {
                        SmeltSlot.Inventory.currentInventory[0].SlotItems[0] = Game1.ItemVault.GenerateNewItem(SmeltSlot.Inventory.currentInventory[0].SlotItems[0].SmeltedItem, null);
                        ItemSlots[0].Inventory.RemoveItem(ItemSlots[0].Inventory.currentInventory[0].SlotItems[0]);
                    }
                }
            }
            else

            {
                SimpleTimer.Time = 0;
            }

            redEsc.Update(Game1.myMouseManager);


            if (redEsc.isClicked)
            {
                this.IsUpdating = false;
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

            spriteBatch.DrawString(Game1.AllTextures.MenuText, SimpleTimer.Time.ToString(), TimerStringLocation, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);

            redEsc.Draw(spriteBatch);
        }

    }
}
