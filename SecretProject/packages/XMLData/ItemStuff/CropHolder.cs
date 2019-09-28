using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.ItemStuff
{
    public class CropHolder
    {
        public List<Crop> AllCrops { get; set; }

        public Crop GetCropFromID(int ID)
        {
            Crop newCrop = new Crop();
            Crop oldCrop = AllCrops.Find(x => x.ItemID == ID);
            newCrop.ItemID = oldCrop.ItemID;
            newCrop.Name = oldCrop.Name;
            newCrop.GID = oldCrop.GID;
            newCrop.DaysToGrow = oldCrop.DaysToGrow;
            newCrop.CurrentGrowth = oldCrop.CurrentGrowth;
            newCrop.Harvestable = oldCrop.Harvestable;
            newCrop.BaseGID = oldCrop.GID;

            return newCrop; 
        }
    }
}
