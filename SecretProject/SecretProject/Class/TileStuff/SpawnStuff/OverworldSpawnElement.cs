using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.SpawnStuff
{
    public class OverworldSpawnElement
    {
        public int GID { get; private set; }
        public MapLayer MapLayerToPlace { get; private set; }
        public MapLayer MapLayerToCheckIfEmpty { get; private set; }
        public GenerationType GenerationType { get; private set; }
        public int Frequency { get; set; }

        public bool ZeroLayerOnly { get; private set; }
        public bool AssertLeftAndRight { get; set; }
        public int Limit { get; set; }


        public bool Unlocked { get; set; }


        public OverworldSpawnElement(int gid, MapLayer mapLayerToPlace, MapLayer mapLayerToCheckIfEmpty, GenerationType generationType,int frequency, bool zeroLayerOnly = false, bool assertLeftAndRight = false, int limit = 0)
        {
            this.GID = gid;
            this.MapLayerToPlace = mapLayerToPlace;
            this.MapLayerToCheckIfEmpty = mapLayerToCheckIfEmpty;
            this.GenerationType = generationType;
            this.Frequency = frequency;
            this.ZeroLayerOnly = zeroLayerOnly;
            this.AssertLeftAndRight = assertLeftAndRight;
            this.Limit = limit;
        }
    }
}
