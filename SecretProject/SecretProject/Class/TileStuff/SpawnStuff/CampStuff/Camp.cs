using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.SpawnStuff.CampStuff
{
    public enum CampType
    {
        Calcite = 1,
        Arcane = 2
    }
    public class Camp : IWeightable
    {
        public CampType CampType { get; set; }
        public float SpawnChance { get; set; }
        public List<SpawnElement> SpawnElements { get; set; }
        public List<Enemy> PossibleEnemies { get; set; }
        public int Chance { get; set; }

        public Camp(int probability)
        {
            this.Chance = probability;
        }
    }
}
