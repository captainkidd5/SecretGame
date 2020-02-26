using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.TileStuff;
using System.Collections.Generic;

namespace SecretProject.Class.ItemStuff
{
    public class Chest : IStorableItemBuilding
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
        public Tile Tile { get; set; }



        public ItemStorageSlot CurrentHoveredSlot { get; set; }

        private Button redEsc;
        public Chest(string iD, int size, Vector2 location, GraphicsDevice graphics, bool isRandomlyGenerated)
        {
            this.StorableItemType = StorableItemType.Chest;
            this.ID = iD;
            this.Size = size;
            this.Inventory = new Inventory(this.Size);
            this.Location = location;
            this.GraphicsDevice = graphics;
            this.BackDropSourceRectangle = new Rectangle(224, 128, 48, 96);
            this.BackDropPosition = new Vector2(Game1.ScreenWidth / 6, Game1.ScreenHeight / 6);
            this.BackDropScale = 3f;

            redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), this.GraphicsDevice,
                new Vector2(this.BackDropPosition.X + 6 * 32 * this.BackDropScale, this.BackDropPosition.Y * this.BackDropScale), CursorType.Normal);

            this.ItemSlots = new List<ItemStorageSlot>();
            for (int i = 0; i < 6; i++)
            {
                this.ItemSlots.Add(new ItemStorageSlot(graphics, this.Inventory, i, new Vector2(this.BackDropPosition.X + i * 32 * this.BackDropScale, this.BackDropPosition.Y * this.BackDropScale), new Rectangle(208, 80, 32, 32), this.BackDropScale, true));

            }

            if (isRandomlyGenerated)
            {
                FillWithLoot(size);
            }
        }

        public void Activate(Tile tile)
        {
            this.IsUpdating = true;
            this.Tile = tile;
            this.Tile.SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(1759, 100);

        }
        public void Deactivate()
        {
            this.IsUpdating = false;
            this.Tile.SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(1755, 100);
        }

        public bool IsItemAllowedToBeStored(Item item)
        {
            return true;
        }
        public void Update(GameTime gameTime)
        {
            redEsc.Update(Game1.MouseManager);


            if (redEsc.isClicked)
            {

                Deactivate();
            }
            this.IsInventoryHovered = false;
            if (!Game1.Player.ClickRangeRectangle.Intersects(new Rectangle((int)this.Location.X, (int)this.Location.Y, 16, 16)))
            {
                Deactivate();
            }


            for (int i = 0; i < this.ItemSlots.Count; i++)
            {
                this.ItemSlots[i].Update(gameTime);
                if (this.ItemSlots[i].Button.IsHovered)
                {

                    this.IsInventoryHovered = true;
                    this.CurrentHoveredSlot = this.ItemSlots[i];

                }
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            redEsc.Draw(spriteBatch);
            for (int i = 0; i < this.ItemSlots.Count; i++)
            {
                this.ItemSlots[i].Draw(spriteBatch);
            }


        }

        public void FillWithLoot(int size)
        {
            int slotsToFill = Game1.Utility.RGenerator.Next(1, size + 1);
            for (int i = 0; i < slotsToFill; i++)
            {
                int selection = Game1.Utility.RGenerator.Next(0, Game1.AllItems.AllItems.Count);
                this.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem(Game1.AllItems.AllItems[selection].ID, null));
            }

        }


    }
}



