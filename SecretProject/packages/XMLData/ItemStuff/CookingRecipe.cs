using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.ItemStuff
{
    public class CookingRecipe
    {
        public int ItemID { get; set; }

        public float MeatValueMin { get; set; }
        public float MeatValueMax { get; set; }


        public float VegetableValueMin { get; set; }
        public float VegetableValueMax { get; set; }


        public float FruitValueMin { get; set; }
        public float FruitValueMax { get; set; }


    }
}
