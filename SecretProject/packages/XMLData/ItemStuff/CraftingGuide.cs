using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.ItemStuff
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
        public List<ItemRecipe> CraftingRecipes { get; set; }
    }
}
