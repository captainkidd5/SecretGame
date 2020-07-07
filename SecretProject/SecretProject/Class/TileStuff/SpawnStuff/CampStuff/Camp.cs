using Microsoft.Xna.Framework;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.StageFolder;
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
        public List<SpawnElement> SpawnElements { get; set; }
        public List<Enemy> PossibleEnemies { get; set; }
        public int Chance { get; set; }

        public int FloorTileID { get; set; }

        public Camp(int probability)
        {
            this.Chance = probability;
        }

        public virtual void Spawn(TileManager TileManager, TmxStageBase location)
        {
            //for(int i =0; i < this.SpawnElements.Count; i++)
            //{
            //    SpawnElement element = SpawnElements[i];
            //    TileUtility.GenerateRandomlyDistributedTiles((int)element.MapLayerToPlace, element.GID, element.GenerationType, element.Frequency,
            //                    (int)element.MapLayerToCheckIfEmpty, TileManager, element.ZeroLayerOnly, element.AssertLeftAndRight, element.Limit);
            //}
            
            
            
            
        }
    }
}
