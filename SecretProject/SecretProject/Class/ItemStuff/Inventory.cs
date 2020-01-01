using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

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
            ItemCount = 0;
            this.Capacity = capacity;
            currentInventory = new List<InventorySlot>(Capacity - 1);
            for (int i = 0; i < Capacity; i++)
            {
                if(slotCapacity == 0)
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
            bool slotsAvailable = false;
            foreach (InventorySlot s in currentInventory)
            {
                if (s.SlotItems.Any(x => x.ID == item.ID) && s.SlotItems.Count(x => x.ID == item.ID) < item.InvMaximum)
                {
                    if (s.SlotItems.Count < s.Capacity)
                    {
                        s.AddItemToSlot(item);
                        return true;
                    }
                }
                if (s.SlotItems.Count == 0)
                {
                    slotsAvailable = true;
                }
            }
            if (slotsAvailable)
            {

                if (AddToFirstEmptySlotOnly(item))
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

                if (s.SlotItems.Count == 0)
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
                if ((s.SlotItems.Any(x => x.ID == item.ID) && s.SlotItems.Count(x => x.ID == item.ID) < item.InvMaximum) || s.SlotItems.Count == 0)
                {
                    return true;

                }
            }
            return false;

        }

        public void RemoveItem(Item item)
        {
            bool removed = false;
            foreach (InventorySlot s in currentInventory)
            {
                if (removed == false)
                {
                    if (s.SlotItems.Contains(item))
                    {
                        s.RemoveItemFromSlot();
                        removed = true;
                    }
                }
            }
        }

        public void RemoveItem(int id)
        {
            bool removed = false;
            foreach (InventorySlot s in currentInventory)
            {
                if (removed == false)
                {
                    if (s.SlotItems.Any(x => x.ID == id))
                    {
                        s.RemoveItemFromSlot();
                        removed = true;
                    }
                }
            }
        }

        public int FindNumberOfItemInInventory(int id)
        {
            int counter = 0;
            foreach (InventorySlot s in currentInventory)
            {
                counter += s.SlotItems.Count(x => x.ID == id);

            }
            return counter;
        }


        public void GetNextAvailableSlot()
        {

        }


    }

    public class InventorySlot
    {
        public int Capacity { get; set; }
        public List<Item> SlotItems { get; set; }


        public Item Item { get; set; }

        public bool IsCurrentSelection = false;

        public InventorySlot(Item item)
        {
            SlotItems = new List<Item>(1);
            SlotItems.Add(item);
            this.Item = item;
        }



        public InventorySlot()
        {
            SlotItems = new List<Item>(1);
            Capacity = 999;
        }

        public InventorySlot(int capacity)
        {
            SlotItems = new List<Item>(1);
            this.Capacity = capacity;
        }

        public Item GetItem()
        {
            return SlotItems[0];
        }

        public void RemoveItemFromSlot()
        {
            SlotItems.RemoveAt(SlotItems.Count - 1);
        }

        public void AddItemToSlot(Item item)
        {
            SlotItems.Add(item);
        }

    }
}
