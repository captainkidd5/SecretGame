using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.SanctuaryStuff
{
    //public enum SanctuaryReward
    //{
    //    Gold = 1,
    //    GrindStone = 1202,
    //    SawTable = 1201,
    //    Furnace = 1202,

    //}
    public class SanctuaryReward
    {
        public string Description { get; set; }
        [ContentSerializer(Optional = true)]
        public int GoldAmount { get; set; }

        [ContentSerializer(Optional = true)]
        public int GIDUnlock { get; set; }

        [ContentSerializer(Optional = true)]
        public int ItemUnlock { get; set; }

        [ContentSerializer(Optional = true)]
        public int LootIndexUnlock { get; set; }






    }
}
