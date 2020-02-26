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
        public int MinNumberToSpawn { get; set; }
        public int ProbabilityAdditionalSpawn { get; set; }
        public int MaxNumberToSpawn { get; set; }
    }
}
