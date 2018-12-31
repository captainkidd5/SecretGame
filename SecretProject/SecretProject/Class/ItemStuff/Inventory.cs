using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff.Items;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.Stage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff
{
   public class Inventory

    {

        MouseManager mouse;

        public List<InventorySlot> currentInventory;


        public int ID { get; set; }
        public string Name { get; set; }
        public int ItemCount { get; set; }
        public Sprite ItemSprite { get; set; }
        public int Capacity { get; set; }

        GraphicsDevice graphics;
        ContentManager content;

        public Inventory(GraphicsDevice graphics, ContentManager content, MouseManager mouse)
        {
            currentInventory = new List<InventorySlot>(3);
            currentInventory.Add(new InventorySlot());
            currentInventory.Add(new InventorySlot());
            currentInventory.Add(new InventorySlot());
            currentInventory.Add(new InventorySlot());

            ItemCount = 0;
            this.mouse = mouse;
            this.graphics = graphics;
            this.content = content;
            this.Capacity = 4;
        }

        public void Update(GameTime gameTime)
        {

        }

        public bool TryAddItem(InventoryItem item)
        {
            bool itemFilled = false;
            bool slotsAvailable = false;
            foreach(InventorySlot s in currentInventory)
            {
                if(s.SlotItems.Any(x => x.Name == item.Name) && s.SlotItems.Count(x => x.Name == item.Name) < item.InvMaximum)
                {
                    s.AddItemToSlot(item);
                    itemFilled = true;
                    break;
                }  
                else if(s.SlotItems.Count == 0)
                {
                    slotsAvailable = true;
                }
                else
                {

                }
            }
            if(itemFilled == false && slotsAvailable == true)
            {
                foreach(InventorySlot s in currentInventory)
                {
                    if(itemFilled == true)
                    {
                        break;
                    }
                    if(s.SlotItems.Count == 0)
                    {
                        s.AddItemToSlot(item);
                        itemFilled = true;
                    }
                }
            }
            if(itemFilled)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveItem(InventoryItem item)
        {
            foreach (InventorySlot s in currentInventory)
            {
                if (s.SlotItems.Contains(item))
                {
                    s.RemoveItemFromSlot(item);
                }
            }
        }

   

        public void GetNextAvailableSlot()
        {

        }

       
    }

    public class InventorySlot
    {
        public List<InventoryItem> SlotItems { get; set; }
       // public int InventoryQueuePosition { get; set; }

        public InventorySlot(InventoryItem item)
        {
            SlotItems = new List<InventoryItem>(1);
            SlotItems.Add(item);
        }

        public InventorySlot()
        {
            SlotItems = new List<InventoryItem>(1);
        }

        public void RemoveItemFromSlot(InventoryItem item)
        {
            if (SlotItems.Count(x => x.Name == item.Name) > 0)
            {

                SlotItems.Remove(item);
                
            }

        }

        public void AddItemToSlot(InventoryItem item)
        {
            
          SlotItems.Add(item);
   
        }

    }
}
