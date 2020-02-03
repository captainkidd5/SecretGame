using Microsoft.Xna.Framework.Content;
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
        Bow = 26,
        Ammunition = 27,
        Tree = 40

    }
    public class ItemHolder
    {
        public List<ItemData> AllItems { get; set; }



    }
}
