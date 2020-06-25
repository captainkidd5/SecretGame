using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.TileStuff;
using SecretProject.Class.UI;
using SecretProject.Class.UI.ButtonStuff;
using System.Collections.Generic;
using XMLData.ItemStuff;

namespace SecretProject.Class.ItemStuff
{
    public class Chest : IStorableItemBuilding
    {
        public StorageManager StorageManager { get; private set; }
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

        public IInformationContainer Container { get; set; }
        public Tile Tile { get; set; }
        public int Layer { get; set; }
        public int X { get; set; }
        public int Y { get; set; }



        public ItemStorageSlot CurrentHoveredSlot { get; set; }

        private Button redEsc;

        public bool IsAnimationOpen { get; set; }


        public DragSlot DragSlot { get; private set; }
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
                this.ItemSlots.Add(new ItemStorageSlot(graphics, this.Inventory.currentInventory[i],  new Vector2(this.BackDropPosition.X + i * 32 * this.BackDropScale, this.BackDropPosition.Y * this.BackDropScale), new Rectangle(208, 80, 32, 32), this.BackDropScale, true));

            }

            if (isRandomlyGenerated)
            {
                FillWithLoot(size);
            }

            this.DragSlot = new DragSlot();
            this.StorageManager = new StorageManager(this.Inventory, Game1.Player.Inventory, this.ItemSlots);
        }

        public void Activate(IInformationContainer container, int layer, int x, int y)
        {
            if (!this.IsAnimationOpen)
            {
                if(Game1.Player.GetCurrentEquippedToolData() != null)
                {
                    if(Game1.Player.GetCurrentEquippedToolData().Type != ItemType.Hammer)
                    {
                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.ChestOpen, true, 1f, .6f);
                        this.IsAnimationOpen = true;

                        this.Container = container;
                        this.Layer = layer;
                        this.X = x;
                        this.Y = y;
                        this.IsUpdating = true;
                        this.Tile = container.AllTiles[layer][x, y];
                        // this.Tile.SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(1759, 100);
                        container.AnimationFrames.Remove(Tile.TileKey);
                        TileUtility.Animate(Dir.Right, layer, x, y, container, false);
                    }
                }
                else
                {
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.ChestOpen, true, 1f, .6f);
                    this.IsAnimationOpen = true;

                    this.Container = container;
                    this.Layer = layer;
                    this.X = x;
                    this.Y = y;
                    this.IsUpdating = true;
                    this.Tile = container.AllTiles[layer][x, y];
                    // this.Tile.SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(1759, 100);
                    container.AnimationFrames.Remove(Tile.TileKey);
                    TileUtility.Animate(Dir.Right, layer, x, y, container, false);
                }
               
            }

        }
        public void Deactivate()
        {
            if (this.IsAnimationOpen)
            {
                this.IsAnimationOpen = false;

                this.IsUpdating = false;
                TileUtility.Animate(Dir.Left, this.Layer, this.X, this.Y, this.Container, false);
            }
        }

        public bool DepositItem(Item item)
        {

                    if (this.Inventory.TryAddItem(item))
                    {
                        return true;
                    }

            

            return false;
        }

        /// <summary>
        /// For chests, any item can be stored. May not be true for other storage items.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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
            this.StorageManager.Update(gameTime, Game1.Player.UserInterface.BackPack.DraggedSlot);

            //for (int i = 0; i < this.ItemSlots.Count; i++)
            //{
            //    this.ItemSlots[i].Update(gameTime, this.DragSlot);
            //    if (this.ItemSlots[i].Button.IsHovered)
            //    {

            //        this.IsInventoryHovered = true;
            //        this.CurrentHoveredSlot = this.ItemSlots[i];

            //    }
            //}


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            redEsc.Draw(spriteBatch);
            this.StorageManager.Draw(spriteBatch);
            //for (int i = 0; i < this.ItemSlots.Count; i++)
            //{
            //    this.ItemSlots[i].Draw(spriteBatch);
            //}


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



