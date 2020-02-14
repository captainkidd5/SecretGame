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
        public List<NoiseInterval> OverWorldMidgroudNoise { get; set; }
        public List<NoiseInterval> OverWorldBuildingsNoise{ get; set; }
        public List<NoiseInterval> OverworldForegroundNoise { get; set; }

        public List<List<NoiseInterval>> AllOverWorldIntervals{ get; set; }

        public NoiseConverter(List<NoiseInterval> overWorldBackgroundNoise,
            List<NoiseInterval> overWorldMidgroundNoise, List<NoiseInterval> overWorldBuildsingsNoise,
            List<NoiseInterval> overworldForegroundNoise)
        {

            this.OverWorldBackgroundNoise = overWorldBackgroundNoise;


            this.OverWorldMidgroudNoise = overWorldMidgroundNoise;
            this.OverWorldBuildingsNoise = overWorldBuildsingsNoise;

            this.OverworldForegroundNoise = overworldForegroundNoise;

            this.AllOverWorldIntervals = new List<List<NoiseInterval>>()
            {
                OverWorldBackgroundNoise,
                OverWorldMidgroudNoise,
                OverWorldBuildingsNoise,
                OverworldForegroundNoise
            };

        }
        //not complete
        public int ConvertNoiseToGID(ChunkType chunkType, float noiseValue, int desiredLayer)
        {

            List<NoiseInterval> noiseList = this.AllOverWorldIntervals[desiredLayer];
            for(int i =0; i < noiseList.Count; i++)
            {
                if(noiseValue > noiseList[i].LowerBound && noiseValue <= noiseList[i].UpperBound)
                {
                    return (int)noiseList[i].TilingContainer.GenerationType + 1;
                }
            }
            return 0;

        }
    }


}
