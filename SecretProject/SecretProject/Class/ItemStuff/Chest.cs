using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff
{
    public class Chest
    {
        public int ID { get; set; }
        public int Size { get; set; }
        public Vector2 MyProperty { get; set; }
        public Inventory Inventory { get; set; }

        public Chest()
        {

        }
    }
}
