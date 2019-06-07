using SecretProject.Class.ItemStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ShopStuff
{
    public interface IShop
    {
        int ID { get; set; }
        string Name { get; set; }
        Inventory ShopInventory { get; set; }
    }
}
