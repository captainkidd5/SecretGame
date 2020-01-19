using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.ItemStuff.LootStuff
{
    public class LootPiece
    {
        public bool Unlocked { get; set; }
        public int ItemToSpawnID { get; set; }
        public int NumberOfItemsToSpawn { get; set; }
        public int ProbabilityOfSpawn { get; set; }
    }
}
