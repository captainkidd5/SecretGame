using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff
{
    public class StorageManager
    {
        public Inventory Inventory { get; private set; }
        public Inventory DestinationInventory { get; private set; }
        public List<ItemStorageSlot> StorageSlots { get; private set; }
        public DragSlot DragSlot { get; private set; }

        public ItemStorageSlot StorageSlotHovered { get; private set; }
        public ItemStorageSlot StorageSlotHoveredLastFrame { get; private set; }

        public InventorySlot MyProperty { get; set; }
        public StorageManager(Inventory inventory, Inventory destinationInventory, List<ItemStorageSlot> storageSlots)
        {
            this.Inventory = inventory;
            this.DestinationInventory = destinationInventory;
            this.StorageSlots = storageSlots;
        }

        public void Update(GameTime gameTime, DragSlot dragSlot)
        {
            for(int i =0; i < this.StorageSlots.Count; i++)
            {
                this.StorageSlots[i].Update(gameTime,this, this.DestinationInventory, this.StorageSlotHoveredLastFrame, dragSlot);
            }
            if(this.StorageSlotHovered != this.StorageSlotHoveredLastFrame)
            {
                Game1.Player.UserInterface.BackPack.ItemInfoInteraction(StorageSlotHovered.Button, StorageSlotHovered.Slot, StorageSlotHovered.Slot.GetItemData());
            }
            StorageSlotHoveredLastFrame = this.StorageSlotHovered;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.StorageSlots.Count; i++)
            {
                this.StorageSlots[i].Draw(spriteBatch);
            }
        }
    }
}
