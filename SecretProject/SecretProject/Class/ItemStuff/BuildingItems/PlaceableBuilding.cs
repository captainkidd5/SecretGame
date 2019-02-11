using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff.BuildingItems
{
    public class PlaceableBuilding
    {
        public string Name { get; set; }

        public int[] ForeGroundID { get; set; }
        public int[] BuildingID { get; set; }
        public int[,] TotalTiles { get; set; }
        public int CurrentCoordinate { get; set; }

        public bool HasInteraction { get; set; }

        public PlaceableBuilding(string name)
        {
            this.Name = name;

            switch(name)
            {
                //should be GID + 1 for all cases
                case "barrel":
                    ForeGroundID = new int[2] { 3437, 3438 };
                    BuildingID = new int[2] { 3537, 3538 };

                    TotalTiles = new int[2, 2] {
                        {3437, 3438 },
                        {3537, 3538 }
                    };
                    break;
            }
        }
    }
}
