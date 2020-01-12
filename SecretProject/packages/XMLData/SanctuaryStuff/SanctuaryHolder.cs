using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.SanctuaryStuff
{
    public enum SanctuaryReward
    {
        Gold = 1,
        GrindStone = 1202,
        SawTable = 1201,
        Furnace = 1202,

    }

    public enum GIDUnlock
    {
        GrassTuft = 1079,
        Pumpkin = 1381,
    }
    public class SanctuaryHolder
    {
        public int ID { get; set; }
        public List<SanctuaryPageHolder> AllPages{ get; set; }
    }
}
