using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.ItemStuff.CraftingStuff;

namespace XMLData.ItemStuff.CraftingStuff
{
    public enum CraftingCategory
    {
        Tools = 0,
        Parts = 1,
        Utility = 2,
        Outdoor = 3,
        Indoor = 4
    }
    public class CraftingGuide
    {
        public List<CraftingPage> CraftingPages { get; set; }
    }
}
