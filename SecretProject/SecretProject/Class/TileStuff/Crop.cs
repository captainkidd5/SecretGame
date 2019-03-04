using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    class Crop : IPlant
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int GrowthLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Crop(string CropType)
        {
            this.Name = CropType;

        }
    }
}
