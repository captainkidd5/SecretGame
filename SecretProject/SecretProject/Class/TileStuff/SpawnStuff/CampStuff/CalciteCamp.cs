using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretProject.Class.NPCStuff.Enemies;

namespace SecretProject.Class.TileStuff.SpawnStuff.CampStuff
{
    public class CalciteCamp : Camp
    {


        public CalciteCamp(int probability) : base(probability)
        {
            this.CampType = CampType.Calcite;

        }
    }
}
