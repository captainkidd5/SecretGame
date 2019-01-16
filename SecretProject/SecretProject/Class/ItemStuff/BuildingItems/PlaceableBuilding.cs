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

        public bool HasInteraction { get; set; }

        public PlaceableBuilding(string name)
        {
            this.Name = name;

            switch(name)
            {
                case "barrel":
                    ForeGroundID = new int[2] { 3433, 3434 };
                    BuildingID = new int[2] { 3533, 3534 };

                    TotalTiles = new int[2, 2] {
                        {3433, 3434 },
                        {3533, 3534 }
                    };
                    break;
            }
        }
    }
}
