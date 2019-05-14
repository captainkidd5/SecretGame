using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using SecretProject.Class.ItemStuff.Items;

namespace SecretProject.Class.ItemStuff
{
    //xml bin data is saved to bin as xnb!
    
    public class ItemBank
    {
        public Dictionary<string, InventoryItem> Items;

        public ItemBank()
        {
             Items = new Dictionary<string, InventoryItem>();

        }
    }
    
}
