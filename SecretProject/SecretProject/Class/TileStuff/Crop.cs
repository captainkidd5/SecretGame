using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    public class Crop
    {
        public int GID { get; set; }

        public Crop(int itemID)
        {
            this.GID = itemID;
        }
    }
}
