using Microsoft.Xna.Framework;
using SecretProject.Class.SpriteFolder;
using System.Collections.Generic;
using System.Linq;

namespace SecretProject.Class.ItemStuff
{

    public class Inventory

    {

        public List<InventorySlot> currentInventory;


        public int ID { get; set; }
        public string Name { get; set; }
        public int ItemCount { get; set; }
        public Sprite ItemSprite { get; set; }
        public int Capacity { get; set; }

        public int Money { get; set; } = 0;


        private Inventory()
        {

        }

        public Inventory(int capacity, int slotCapacity = 0)
        {
            this.ItemCount = 0;
            this.Capacity = capacity;
            currentInventory = new List<InventorySlot>(this.Capacity - 1);
            for (int i = 0; i < this.Capacity; i++)
            {
                if (slotCapacity == 0)
                {
                    currentInventory.Add(new InventorySlot());
                }
                else
                {
                    currentInventory.Add(new InventorySlot(slotCapacity));
                }

            }

        }

        public void Update(GameTime gameTime)
        {

        }

        public bool TryAddItem(Item item)
        {
            foreach (InventorySlot s in currentInventory)
            {
                if (s.AddItemToSlot(item))
                {
                    return true;
                }
            }
            return false;

        }

        public bool AddToFirstEmptySlotOnly(Item item)
        {
            foreach (InventorySlot s in currentInventory)
            {

                if (s.ItemCount == 0)
                {
                    s.AddItemToSlot(item);
                    return true;
                }

            }
            return false;
        }

        public bool IsPossibleToAddItem(Item item)
        {
            foreach (InventorySlot s in currentInventory)
            {
                if (s.IsPossibleToAddItem(item))
                {
                    return true;

                }
            }
            return false;

        }


        public bool RemoveItem(int id)
        {

            foreach (InventorySlot s in currentInventory)
            {

                if (s.Item.ID == id)
                {
                    s.RemoveItemFromSlot();
                    return true;
                }


            }
            return false;
        }
        public bool RemoveItem(Item item)
        {
            foreach (InventorySlot s in currentInventory)
            {

                if (s.Item == item)
                {
                    s.RemoveItemFromSlot();
                    return true;
                }


            }
            return false;
        }

        public int FindNumberOfItemInInventory(int id)
        {
            int counter = 0;
            foreach (InventorySlot s in currentInventory)
            {
                if (s.Item != null)
                {
                    if (s.Item.ID == id)
                    {
                        counter += s.ItemCount;
                    }
                }


            }
            return counter;
        }


        public void GetNextAvailableSlot()
        {

        }


    }

    public class InventorySlot
    {
        public Item Item { get; set; }
        public int Capacity { get; set; }
        public int ItemCount { get; set; }

        public bool IsCurrentSelection = false;

        public InventorySlot(Item item)
        {
            this.Item = item;
            this.ItemCount = 1;
        }



        public InventorySlot()
        {

            this.Capacity = 999;
            this.ItemCount = 0;
        }

        public InventorySlot(int capacity)
        {
            this.ItemCount = 0;
            this.Capacity = capacity;
        }

        public Item GetItem()
        {
            return this.Item;
        }

        public bool RemoveItemFromSlot()
        {
            if (this.ItemCount <= 0)
            {
                return false;
            }
            this.ItemCount--;
            if (this.ItemCount <= 0)
            {
                this.Item = null;
                this.ItemCount = 0;
            }
            return true;
        }

        public bool IsPossibleToAddItem(Item item)
        {
            if (this.Item == null)
            {

                return true;
            }
            else if (this.ItemCount <= Game1.ItemVault.GetItem(item.ID).InvMaximum)
            {

                return true;
            }
            return false;
        }

        public bool AddItemToSlot(Item item)
        {

            if (this.Item == null)
            {
                this.Item = item;
                this.ItemCount = 1;
                return true;
            }
            else if (item.ID == this.Item.ID && this.ItemCount < Game1.ItemVault.GetItem(item.ID).InvMaximum)
            {
                this.ItemCount++;
                return true;
            }
            return false;
        }

    }
}
