using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Universal
{
    public class PerlinNoiseGenerator
    {
        public int OctaveCount { get; set; }
        public float Persistence { get; set; }


        public PerlinNoiseGenerator(int octaveCount, float persistence)
        {
            this.OctaveCount = octaveCount;
            this.Persistence = persistence;
        }

        public float[,] GenerateWhiteNoise(int width, int height)
        {
            float[,] noiseFieldToReturn = new float[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    noiseFieldToReturn[i, j] = (float)Game1.Utility.RGenerator.NextDouble() % 1;
                }
            }
            return noiseFieldToReturn;
        }

        public float[,] SmoothNoiseField(float[,] whiteNoise, int octave)
        {
            int width = whiteNoise.GetLength(0);
            int height = whiteNoise.GetLength(1);
            float[,] smoothField = new float[width, height];

            int samplePeriod = 1 << octave;

            float sampleFrequency = 1.0f / samplePeriod;

            for(int i =0; i < width; i++)
            {
                int samplei0 = (i / samplePeriod) * samplePeriod;
                int samplei1 = (samplei0 + samplePeriod) % width;

                float horizontalBlend = (i - samplei0) * sampleFrequency;
                for(int j =0; j < height; j++)
                {
                    int samplej0 = (j/samplePeriod) * samplePeriod;
                    int samplej1 = (samplej0 + samplePeriod) % height;
                    float verticalBlend = (j - samplej0) * sampleFrequency;

                    float top = LinearInterpolate(whiteNoise[samplei0, samplej0],
                        whiteNoise[samplei1, samplej0], horizontalBlend);

                    float bottom = LinearInterpolate(whiteNoise[samplei0, samplej1],
                        whiteNoise[samplei1, samplej1], horizontalBlend);

                    smoothField[i, j] = LinearInterpolate(top, bottom, verticalBlend);
                }
            }

            return smoothField;
        }

        public float[,] GeneratePerlinNoise(float[,] baseNoise, int octaveCount)
        {
            int width = baseNoise.GetLength(0);
            int height = baseNoise.GetLength(1);

            float[][,] smoothNoise = new float[octaveCount][,];

            float persistance = .5f;

            for(int i =0; i < octaveCount;i++)
            {
                smoothNoise[i] = SmoothNoiseField(baseNoise, i);
            }
            float[,] perlinNoise = new float[width, height];
            float amplitude = 1f;
            float totalAmplitude = 0.0f;

            for(int octave = octaveCount - 1; octave > 0; octave-- )
            {
                amplitude *= persistance;
                totalAmplitude += amplitude;

                for(int i =0; i < width;i++)
                {
                    for(int j =0; j < height; j++)
                    {
                        perlinNoise[i, j] += smoothNoise[octave][i, j] * amplitude;
                    }
                }
            }

            for(int i =0; i < width; i++)
            {
                for(int j =0; j < height; j++)
                {
                    perlinNoise[i, j] /= totalAmplitude;
                }
            }
            return perlinNoise;
        }

        public float LinearInterpolate(float a, float b, float alpha)
        {
            return a * (1 - alpha) + alpha * b;
        }
    }
}
