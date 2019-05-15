using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using SecretProject.Class.ItemStuff.Items;

namespace SecretProject.Class.ItemStuff
{
    //xml bin data is saved to bin as xnb!
    
    public class ItemBank
    {
        public Dictionary<string, Item> RawItems;

        public Dictionary<int, Item> Items;

        public ItemBank()
        {
             RawItems = new Dictionary<string, Item>();
             Items = new Dictionary<int, Item>();

            

        }

        public void LoadItems(GraphicsDevice graphics, ContentManager content)
        {
            for (int i = 0; i < RawItems.Count; i++)
            {
                if (RawItems.ContainsKey(i.ToString()))
                {
                    Items.Add(i, new Item(i, graphics, content));
                }

            }
        }

        //creates copy of item in dictionary.
        public Item GenerateNewItem(int id, Vector2? location, bool isWorldItem = false)
        {
            Item newItem = this.Items[id];
            if(!(location == null))
            {
                newItem.WorldPosition = (Vector2)location;
            }
            if(isWorldItem)
            {
                newItem.IsWorldItem = true;
                newItem.IsDropped = true;
                //newItem.IsMagnetizable = true;
            }
            newItem.Load();
            return newItem;
        }
    }
    
}
