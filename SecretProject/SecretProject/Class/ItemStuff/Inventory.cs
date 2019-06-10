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
    [Serializable,]
    public class Inventory : ISerializable

    {

        [XmlArray("currentInventory")]
        public List<InventorySlot> currentInventory;


        public int ID { get; set; }
        public string Name { get; set; }
        public int ItemCount { get; set; }
        [XmlIgnore]
        public Sprite ItemSprite { get; set; }
        public int Capacity { get; set; }

        public int Money { get; set; } = 0;


        private Inventory()
        {

        }

        public Inventory(int capacity)
        {
            ItemCount = 0;
            this.Capacity = capacity;
            currentInventory = new List<InventorySlot>(Capacity - 1);
            for(int i = 0; i < Capacity; i++)
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
                    if (s.SlotItems.Any(x => x.ID == item.ID) && s.SlotItems.Count(x => x.ID == item.ID) < item.InvMaximum || s.SlotItems.Count == 0)
                {
                        s.AddItemToSlot(item);
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


        public void GetNextAvailableSlot()
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("currentInventory", typeof(List<InventorySlot>));
        }

        public Inventory(SerializationInfo info, StreamingContext context)
        {
            currentInventory = (List<InventorySlot>)info.GetValue("currentInventory", typeof(List<InventorySlot>));
        }

    }

    public class InventorySlot
    {
        [XmlArray("SlotItems")]
        public List<Item> SlotItems { get; set; }

        [XmlIgnore]
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
