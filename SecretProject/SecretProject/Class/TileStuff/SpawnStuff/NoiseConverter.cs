﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.SpawnStuff
{
    public class NoiseConverter
    {


        public List<List<NoiseInterval>> AllOverWorldIntervals{ get; set; }
        public List<List<NoiseInterval>> AllUnderWorldIntervals{ get; set; }

        public NoiseConverter(List<List<NoiseInterval>> allOverworldNoise, List<List<NoiseInterval>> allUnderWorldNoise)
        {
            this.AllOverWorldIntervals = allOverworldNoise;
            this.AllUnderWorldIntervals = allUnderWorldNoise;
        }
        //not complete
        public int ConvertNoiseToGID(ChunkType chunkType, float noiseValue, int desiredLayer)
        {
            List<NoiseInterval> noiseList;
            if (chunkType == ChunkType.Rai)
            {
                noiseList = this.AllOverWorldIntervals[desiredLayer];
            }
            else
            {
                noiseList = this.AllUnderWorldIntervals[desiredLayer];
            }
           
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
