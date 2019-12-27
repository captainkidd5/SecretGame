﻿using Microsoft.Xna.Framework;
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
        public int DaysToGrow { get; set; }
        public int CurrentGrowth { get; set; } 
        public bool Harvestable { get; set; } = false;
        [ContentSerializer(Optional = true)]
        public int DayPlanted { get; set; }
        [ContentSerializer(Optional = true)]
        public int BaseGID { get; set; }
        [ContentSerializer(Optional = true)]
        public int X { get; set; }
        [ContentSerializer(Optional = true)]
        public int Y { get; set; }


        public void UpdateGrowthCycle(int gid)
        {
            if (CurrentGrowth < DaysToGrow)
            {

                this.GID = gid;
                

            }
            if (CurrentGrowth == DaysToGrow)
            {
                this.Harvestable = true;

            }
            
           
            
        }
    }
}
