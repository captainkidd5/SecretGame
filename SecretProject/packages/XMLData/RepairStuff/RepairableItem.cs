using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.ItemStuff.CraftingStuff;

namespace XMLData.RepairStuff
{
    public class RepairableItem
    {
        public int GID { get; set; }
        public string RequirementDescription { get; set; }
        public string RewardDescription { get; set; }
        public List<ItemsRequired> ItemsRequired{ get; set; }

    }
}
