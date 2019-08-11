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

namespace SecretProject.Class.ItemStuff
{
    //xml bin data is saved to bin as xnb!

    public class ItemBank
    {
        

        public ItemBank()
        {
            

        }

        

        //creates copy of item in dictionary.
        public Item GenerateNewItem(int id, Vector2? location, bool isWorldItem = false)
        {
            Item newItem = new Item(Game1.AllItems.GetItemFromID(id));
            //if(newItem.ispl)

            if (!(location == null))
            {
                newItem.WorldPosition = (Vector2)location;
                
            }
            if (isWorldItem)
            {
                newItem.IsWorldItem = true;
                newItem.IsDropped = true;

                //newItem.IsMagnetizable = true;
            }
            else
            {
                newItem.IsWorldItem = false;
            }
            
            newItem.Load();
            return newItem;
        }
    }

}