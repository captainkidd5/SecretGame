using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.SpawnStuff
{
    public enum SpawnMethod
    {
        RandomScatter = 1,
        Poisson = 2
    }
    public class SpawnElement
    {
        public int GID { get; private set; }
        public SpawnMethod SpawnMethod { get; set; }
        public MapLayer TileLayer { get; private set; }
        public MapLayer LayerToPlaceOn { get; private set; }
        public MapLayer MapLayerToCheckIfEmpty { get; private set; }
        public GenerationType GenerationType { get; private set; }
        public Rarity Rarity { get; set; }
        public Rarity OddsOfAdditionalSpawn { get; set; }
        public int DistanceBetweenNeighbors { get; set; }
        public bool AssertLeftAndRight { get; set; }

        public bool Unlocked { get; set; }

        public bool IsCrop { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gid">Specify as is.</param>
        /// <param name="mapLayerToPlace">Which layer object should be placed in</param>
        /// <param name="mapLayerToCheckIfEmpty">will only spawn on previous layer, if this specified layer is empty.</param>
        /// <param name="generationType"></param>
        /// <param name="distanceBetweenNeighbors">how far must be away from any other given object.</param>
        /// <param name="assertLeftAndRight"></param>

        public SpawnElement(int gid, SpawnMethod spawnMethod,MapLayer mapLayerToPlace, MapLayer layerToPlaceOn, MapLayer mapLayerToCheckIfEmpty, GenerationType generationType,Rarity rarity, Rarity oddsOfAdditionalSpawn, int distanceBetweenNeighbors, bool assertLeftAndRight)
        {
            this.GID = gid + 1;
            this.SpawnMethod = spawnMethod;
            this.TileLayer = mapLayerToPlace;
            this.MapLayerToCheckIfEmpty = mapLayerToCheckIfEmpty;
            this.LayerToPlaceOn = layerToPlaceOn;
            this.GenerationType = generationType;
            this.Rarity = rarity;
            this.OddsOfAdditionalSpawn = oddsOfAdditionalSpawn;
            this.DistanceBetweenNeighbors = distanceBetweenNeighbors;
            this.AssertLeftAndRight = assertLeftAndRight;

        }
    }
}
