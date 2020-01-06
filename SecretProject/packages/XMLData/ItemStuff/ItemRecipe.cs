using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.ItemStuff
{
    public class ItemRecipe
    {
        public int ItemToCraftID { get; set; }
        public List<ItemsRequired> AllItemsRequired { get; set; }
        [ContentSerializer(Optional = true)]
        public bool Locked { get; set; }

    }

    

}
