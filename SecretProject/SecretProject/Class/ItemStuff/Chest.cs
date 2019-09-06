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
        public float ID { get; set; }
        public int Size { get; set; }
        public Vector2 Location { get; set; }
        public Inventory Inventory { get; set; }

        public Chest(float iD,int size, Vector2 location)
        {
            this.ID = ID;
            this.Size = size;
            this.Inventory = new Inventory(size);
            this.Location = location;
        }
    }
}
