using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        public string TileID { get; set; }
        public int DaysToGrow { get; set; }
        public int CurrentGrowth { get; set; } 
        public bool Harvestable { get; set; } = false;
        [ContentSerializer(Optional = true)]
        public int DayPlanted { get; set; }
        [ContentSerializer(Optional = true)]
        public int BaseGID { get; set; }


        public void UpdateGrowthCycle()
        {
            if (CurrentGrowth < DaysToGrow)
            {

                this.GID = this.BaseGID + CurrentGrowth + 1;
                

            }
            if (CurrentGrowth == DaysToGrow)
            {
                this.Harvestable = true;

            }
            
           
            
        }
    }
}
