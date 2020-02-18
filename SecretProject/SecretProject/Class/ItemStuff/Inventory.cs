using Microsoft.Xna.Framework;
using SecretProject.Class.SpriteFolder;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XMLData.ItemStuff;

namespace SecretProject.Class.ItemStuff
{

    public class Inventory

    {

        public List<InventorySlot> currentInventory;


        public int ID { get; set; }
        public int Capacity { get; set; }

        public int Money { get; set; } = 0;


        public Inventory()
        {

        }



        public Inventory(int capacity)
        {
            this.Capacity = capacity;
            currentInventory = new List<InventorySlot>(this.Capacity - 1);
            for (int i = 0; i < this.Capacity; i++)
            {

                    currentInventory.Add(new InventorySlot());
                

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
                if (s.Item != null)
                {
                    if (s.Item.ID == id)
                    {
                        s.RemoveItemFromSlot();
                        return true;
                    }
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

        public bool ContainsAtLeastOne(int id)
        {
            foreach (InventorySlot s in currentInventory)
            {
                if (s.Item != null)
                {
                    if (s.Item.ID == id)
                    {
                        return true;
                    }
                }


            }
            return false;
        }


        public void GetNextAvailableSlot()
        {

        }
        #region FILEIO
        public void Save(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(this.ID);
            binaryWriter.Write(this.Capacity);
            binaryWriter.Write(this.Money);
            binaryWriter.Write(this.currentInventory.Count);

            for (int i = 0; i < this.currentInventory.Count; i++)
            {

                binaryWriter.Write(this.currentInventory[i].ItemCount);
                if(this.currentInventory[i].ItemCount > 0)
                {
                    binaryWriter.Write(this.currentInventory[i].Item.ID);
                }

               
                
            }

        }

        public void Load(BinaryReader reader)
        {
            
            this.currentInventory = new List<InventorySlot>();
            this.ID = reader.ReadInt32();
            this.Capacity = reader.ReadInt32();
            this.Money = reader.ReadInt32();
            int currentInventoryCount = reader.ReadInt32();
            for(int i =0; i < currentInventoryCount; i++)
            {
                InventorySlot slot = new InventorySlot();
                
                int itemCount = reader.ReadInt32();
                if(itemCount > 0)
                {
                    int itemId = reader.ReadInt32();
                    Item item = Game1.ItemVault.GenerateNewItem(itemId, null);
                    slot.AddItemToSlot(item);
                    slot.Item = item;
                    slot.ItemCount = itemCount;
                }
                this.currentInventory.Add(slot);
            }


        }
        #endregion



    }

    public class InventorySlot
    {
        public Item Item { get; set; }
        public int ItemCount { get; set; }

        public bool IsCurrentSelection = false;

        public InventorySlot(Item item)
        {
            this.Item = item;
            this.ItemCount = 1;
        }



        public InventorySlot()
        {
            this.ItemCount = 0;
        }

        public ItemData GetItemData()
        {
            return Game1.ItemVault.GetItem(this.Item.ID);
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
