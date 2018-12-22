using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff.Items;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff
{
   public class InventoryItem

    {
        string name;
        int iD;
        Sprite itemSprite;

        ContentManager content;

        List<IItem> currentInventory;

        public InventoryItem(ContentManager content)
        {
            this.content = content;
            currentInventory = new List<IItem>();
        }

        public void AddItemToInventory(string name, int iD)
        {
            switch(iD)
            {
                case 1:
                    currentInventory.Add(new Food(name, content));
                    break;


            }
            

        }

        /*
        public void RemoveItemFromInventory(string name)
        {
            currentInventory.Remove()
        }
        */
    }
}
