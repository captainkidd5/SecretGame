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

        public Crop GetCropFromID(int gid)
        {
            Crop newCrop = new Crop();
            Crop oldCrop = AllCrops.Find(x => x.ItemID == gid);
            newCrop.ItemID = oldCrop.ItemID;
            newCrop.Name = oldCrop.Name;
            newCrop.GID = oldCrop.GID;
            newCrop.DaysToGrow = oldCrop.DaysToGrow;
            newCrop.CurrentGrowth = oldCrop.CurrentGrowth;
            newCrop.Harvestable = oldCrop.Harvestable;
            newCrop.BaseGID = oldCrop.GID;

            return newCrop; 
        }

        public Crop GetCropFromGID(int gid)
        {
            Crop newCrop = new Crop();
            Crop oldCrop = AllCrops.Find(x => x.GID == gid);
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
