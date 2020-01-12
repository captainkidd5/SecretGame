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

        public bool ZeroLayerOnly { get; private set; }


        public bool Unlocked { get; set; }


        public OverworldSpawnElement(int gid, MapLayer mapLayerToPlace, MapLayer mapLayerToCheckIfEmpty, GenerationType generationType, bool zeroLayerOnly = false)
        {
            this.GID = gid;
            this.MapLayerToPlace = mapLayerToPlace;
            this.MapLayerToCheckIfEmpty = mapLayerToCheckIfEmpty;
            this.GenerationType = generationType;
            this.ZeroLayerOnly = zeroLayerOnly;
        }
    }
}
