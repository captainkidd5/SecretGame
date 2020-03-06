using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.ItemStuff.CraftingStuff
{
    public class CraftingPage
    {
        public CraftingCategory CategoryName { get; set; }
        public List<ItemRecipe> CraftingRecipes { get; set; }
    }
}
