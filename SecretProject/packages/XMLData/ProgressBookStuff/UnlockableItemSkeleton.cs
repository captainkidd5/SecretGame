using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.ProgressBookStuff
{
    public class UnlockableItemSkeleton
    {
        public int RewardItemID { get; set; }
        public List<ItemRequirementSkeleton> ItemRequirements { get; set; }
    }
}
