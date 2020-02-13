using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.SpawnStuff
{
    public class NoiseConverter
    {
        public List<NoiseInterval> OverWorldBackgroundNoise { get; set; }
        public List<NoiseInterval> OverWorldBuildingsNoise { get; set; }
        public List<NoiseInterval> OverworldForegroundNoise { get; set; }

        public NoiseConverter(List<NoiseInterval> overWorldBackgroundNoise, List<NoiseInterval> overWorldBuildingsNoise, List<NoiseInterval> overworldForegroundNoise)
        {
            this.OverWorldBackgroundNoise = overWorldBackgroundNoise;
            this.OverWorldBuildingsNoise = overWorldBuildingsNoise;
            this.OverworldForegroundNoise = overworldForegroundNoise;

        }
        public int ConvertNoiseToGID(float noiseValue, int desiredLayer)
        {

        }
    }
}
