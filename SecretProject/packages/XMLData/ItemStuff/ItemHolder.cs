using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.ItemStuff
{
    public class ItemHolder
    {
        public List<ItemData> AllItems { get; set; }

        public ItemData GetItemFromID(int iD)
        {
            ItemData newItem = new ItemData();
            int debugID = iD;
            ItemData oldItem = AllItems.Find(x => x.ID == iD);
            newItem.Name = oldItem.Name;
            newItem.ID = oldItem.ID;
            newItem.InvMaximum = oldItem.InvMaximum;
            newItem.TextureColumn = oldItem.TextureColumn;
            newItem.TextureRow = oldItem.TextureRow;
            newItem.Price = oldItem.Price;
            newItem.Plantable = oldItem.Plantable;
            newItem.SmeltedItem = oldItem.SmeltedItem;
            newItem.Durability = oldItem.Durability;
            newItem.PlaceID = oldItem.PlaceID;


            return newItem;
        }
    }
}
