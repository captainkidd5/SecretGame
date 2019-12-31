using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.SanctuaryStuff
{
    public enum SanctuaryReward
    {
        GrindStone = 1202,
        SawTable = 1201,
        Furnace = 1202,

    }
    public class SanctuaryHolder
    {
        public int ID { get; set; }
        public List<SanctuaryRequirement> AllRequirements{ get; set; }
    }
}
