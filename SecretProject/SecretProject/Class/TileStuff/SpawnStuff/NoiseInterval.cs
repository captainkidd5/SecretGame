using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.SpawnStuff
{
    public class NoiseInterval
    {
        public TilingContainer TilingContainer { get; set; }
        public float LowerBound { get; set; }
        public float UpperBound { get; set; }

        public NoiseInterval( TilingContainer tilingContainer,float lowerBound, float upperBound)
        {
            this.TilingContainer = tilingContainer;
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
        }
    }
}
