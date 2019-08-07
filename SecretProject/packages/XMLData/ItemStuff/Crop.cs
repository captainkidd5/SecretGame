using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.ItemStuff
{
    public class Crop
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public int GID { get; set; }
        public int TileID { get; set; }
        public int DaysToGrow { get; set; }
        public int CurrentGrowth { get; set; } = 0;
    }
}
