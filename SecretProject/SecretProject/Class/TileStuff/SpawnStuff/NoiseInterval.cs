using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.SpawnStuff
{
    public class NoiseInterval
    {
        public TilingTileManager TilingTileManager { get; set; }
        public float LowerBound { get; set; }
        public float UpperBound { get; set; }

        public NoiseInterval( TilingTileManager tilingTileManager,float lowerBound, float upperBound)
        {
            this.TilingTileManager = tilingTileManager;
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
        }
    }
}
