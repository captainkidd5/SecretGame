using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.ItemStuff
{
    public enum ItemType
    {
        Axe = 21,
        Hammer = 22,
        Shovel = 23,
        Sword = 25,
        Tree = 40

    }
    public class ItemHolder
    {
        public List<ItemData> AllItems { get; set; }

        public ItemData GetItemFromID(int iD)
        {
            ItemData newItem = new ItemData();
            int debugID = iD;
            ItemData oldItem = AllItems.Find(x => x.ID == iD);
            if(oldItem == null)
            {
                return null;
            }
            newItem.Name = oldItem.Name;
            newItem.Description = oldItem.Description;
            newItem.ID = oldItem.ID;
            newItem.InvMaximum = oldItem.InvMaximum;
            newItem.Price = oldItem.Price;
            newItem.Plantable = oldItem.Plantable;
            newItem.SmeltedItem = oldItem.SmeltedItem;
            newItem.FuelValue = oldItem.FuelValue;
            newItem.Durability = oldItem.Durability;
            newItem.PlaceID = oldItem.PlaceID;
            newItem.TilingSet = oldItem.TilingSet;
            newItem.TilingLayer = oldItem.TilingLayer;

                newItem.StaminaRestoreAmount = oldItem.StaminaRestoreAmount;
            newItem.Type = oldItem.Type;
            newItem.AnimationColumn = oldItem.AnimationColumn;
            newItem.CrateType = oldItem.CrateType;
            newItem.Food = oldItem.Food;
            newItem.StaminaRestoreAmount = oldItem.StaminaRestoreAmount;
            newItem.MeatValue = oldItem.MeatValue;
            newItem.VegetableValue = oldItem.VegetableValue;
            newItem.FruitValue = oldItem.FruitValue;


            return newItem;
        }
    }
}
