using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    public enum GenerationType
    {
        Grass = 1014,
        Dirt = 1114,
        Sand = 1321,
        SandRuin = 1621,
        Water = 426,
        Stone = 929,
        DirtCliff = 2934,
        FenceTiling = 456,
        OakFloorTiling = 632,

    };
    public class Procedural
    {
        public FastNoise FastNoise;

        public List<int> DirtGeneratableTiles;
        public List<int> SandGeneratableTiles;
        public List<int> SandRuinGeneratableTiles;
        public List<int> GrassGeneratableTiles;
        public List<int> WaterGeneratableTiles;
        public List<int> StoneGeneratableTiles;
        public List<int> StandardGeneratableDirtTiles;
        public List<int> StandardGeneratableGrassTiles;
        public List<int> FenceGeneratableTiles;

        public List<int> DirtCliffGeneratableTiles;

        //MIDGROUND TILES
        public List<int> OakFloorGeneratableTiles;

        public Dictionary<int, int> FenceTiling;



        public Dictionary<int, int> DirtTiling;

        public Dictionary<int, int> GrassTiling;

        public Dictionary<int, int> SandTiling;


        public Dictionary<int, int> WaterTiling = new Dictionary<int, int>()
        {
            {0, 226},{1,329}, {2, 428 },  {3, 527}, {4, 328}, {5, 525},{6,228},{7, 526}, {8, 429}, {9, 227}, {10, 327}, {11, 427}, {12,325}, {13,425}, {14,326}, {15, 427}
        };

        public Dictionary<int, int> StoneTiling;

        public Dictionary<int, int> SandRuinTiling;

        public Dictionary<int, int> DirtCliffTiling;

        //MIDGROUNDDICTIONARIES
        public Dictionary<int, int> OakFloorTiling;

        public Dictionary<int, int> FillTilingDictionary(int centralGID)
        {
            return new Dictionary<int, int>()
            {
                {0, centralGID -98},{1, centralGID + 3}, {2,  centralGID + 102 },  {3, centralGID + 101}, {4, centralGID + 2}, {5, centralGID + 99},{6,centralGID - 96},
                { 7, centralGID + 100}, {8, centralGID + 103}, {9, centralGID - 97}, {10, centralGID - 99}, {11, centralGID + 1},
                { 12,centralGID - 101}, {13,centralGID - 1}, {14,centralGID - 100}, {15, centralGID}
            };
        }



        public Procedural()
        {
            //FASTNOISE
            FastNoise = new FastNoise(45);
            FastNoise.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
            FastNoise.SetFractalOctaves(5);
            FastNoise.SetFractalLacunarity(3f);

            //Smaller the smooth the biomes
            FastNoise.SetFractalGain(.5f);

            //larger the smaller the biomes
            FastNoise.SetFrequency(.001f);

            //Lists
            DirtGeneratableTiles = new List<int>();
            SandGeneratableTiles = new List<int>();
            SandRuinGeneratableTiles = new List<int>();
            GrassGeneratableTiles = new List<int>();
            WaterGeneratableTiles = new List<int>();
            StoneGeneratableTiles = new List<int>();
            StandardGeneratableDirtTiles = new List<int>();
            StandardGeneratableGrassTiles = new List<int>();
            FenceGeneratableTiles = new List<int>();
            OakFloorGeneratableTiles = new List<int>();
            DirtCliffGeneratableTiles = new List<int>();

            FenceTiling = new Dictionary<int, int>()
        {
            {0, 456},{1,256}, {2, 455 },  {3, 255}, {4, 453}, {5, 253},{6,454},{7, 254}, {8, 456}, {9, 256}, {10, 455}, {11, 255}, {12,453}, {13,253}, {14,454}, {15, 254}
        };
            DirtTiling = FillTilingDictionary(1005);
            GrassTiling = FillTilingDictionary(1014);
            SandTiling = FillTilingDictionary(1321);
            StoneTiling = FillTilingDictionary(929);
            SandRuinTiling = FillTilingDictionary(1621);
            DirtCliffTiling = FillTilingDictionary(2934);

            //MIDGROUDNTILING
            OakFloorTiling = FillTilingDictionary(632);
        }

        public List<int> GetGeneratableTilesFromGenerationType(GenerationType type)
        {
            switch (type)
            {

                case GenerationType.Grass:
                    return GrassGeneratableTiles;

                case GenerationType.Dirt:
                    return Game1.Procedural.DirtGeneratableTiles;

                case GenerationType.Sand:
                    return Game1.Procedural.SandGeneratableTiles;


                case GenerationType.SandRuin:
                    return Game1.Procedural.SandRuinGeneratableTiles;


                case GenerationType.Water:
                    return Game1.Procedural.WaterGeneratableTiles;



                case GenerationType.Stone:
                    return Game1.Procedural.StoneGeneratableTiles;


                case GenerationType.DirtCliff:
                    return Game1.Procedural.DirtCliffGeneratableTiles;

                case GenerationType.FenceTiling:
                    return Game1.Procedural.FenceGeneratableTiles;

                case GenerationType.OakFloorTiling:
                    return Game1.Procedural.OakFloorGeneratableTiles;

                default:
                    return DirtGeneratableTiles;
            }

        }

        public Dictionary<int,int> GetTilingDictionaryFromGenerationType(GenerationType type)
        {
            switch (type)
            {

                case GenerationType.Grass:
                    return GrassTiling;

                case GenerationType.Dirt:
                    return DirtTiling;

                case GenerationType.Sand:
                    return SandTiling;


                case GenerationType.SandRuin:
                    return SandRuinTiling;


                case GenerationType.Water:
                    return WaterTiling;



                case GenerationType.Stone:
                    return StoneTiling;


                case GenerationType.DirtCliff:
                    return DirtCliffTiling;

                case GenerationType.FenceTiling:
                    return FenceTiling;

                case GenerationType.OakFloorTiling:
                    return OakFloorTiling;

                default:
                    return DirtTiling;
            }

        }

        public void GeneratePerlinTiles(int layerToPlace, int x, int y, int gid, List<int> acceptableGenerationTiles, int layerToCheckIfEmpty, IInformationContainer container, int comparisonLayer, int chance = 100)
        {
            if (chance == 100)
            {
                if (!TileUtility.CheckIfTileAlreadyExists(x, y, layerToPlace, container) && TileUtility.CheckIfTileMatchesGID(x, y, layerToPlace,
               acceptableGenerationTiles, container, comparisonLayer))
                {
                    container.AllTiles[layerToPlace][x, y] = new Tile(x, y, gid);
                }
            }

            else
            {
                if (Game1.Utility.RGenerator.Next(0, 101) < chance)
                {
                    if (!TileUtility.CheckIfTileAlreadyExists(x, y, layerToPlace, container) && TileUtility.CheckIfTileMatchesGID(x, y, layerToPlace,
               acceptableGenerationTiles, container, comparisonLayer))
                    {
                        container.AllTiles[layerToPlace][x, y] = new Tile(x, y, gid);
                    }
                }

            }

        }

        public int GetTileFromNoise(float perlinValue, float layer)
        {
            int newGID = 0;
            if (layer == 0)
            {


                if (perlinValue >= .2f && perlinValue <= 1f)
                {
                    newGID = 1006;
                   // newGID = Game1.Procedural.StandardGeneratableDirtTiles[Game1.Utility.RGenerator.Next(0, Game1.Procedural.StandardGeneratableDirtTiles.Count)] + 1;
                }
                else if (perlinValue >= .12f && perlinValue <= .2f)
                {
                   // newGID = Game1.Procedural.StandardGeneratableDirtTiles[Game1.Utility.RGenerator.Next(0, Game1.Procedural.StandardGeneratableDirtTiles.Count)] + 1;
                    newGID = 1006;
                }
                else if (perlinValue >= .1f && perlinValue <= .12f)
                {
                    newGID = 930;//Stone
                }
                else if (perlinValue >= .07f && perlinValue <= .1f)
                {

                    newGID = 2935; //dirt cliff

                }
                else if (perlinValue >= .02f && perlinValue <= .07f)
                {

                    //   newGID = Game1.Procedural.StandardGeneratableGrassTiles[Game1.Utility.RGenerator.Next(0, Game1.Procedural.StandardGeneratableDirtTiles.Count)] + 1;
                    newGID = 1006;
                }

                else if (perlinValue >= -.09f && perlinValue < .02f)
                {
                    // newGID = Game1.Procedural.StandardGeneratableGrassTiles[Game1.Utility.RGenerator.Next(0, Game1.Procedural.StandardGeneratableDirtTiles.Count)] + 1;
                    newGID = 1006;
                    //  int randomGrass = Game1.Utility.RGenerator.Next(0, Game1.Utility.GrassGeneratableTiles.Count);
                    // newGID = Game1.Utility.GrassGeneratableTiles[randomGrass];
                }

                //newGID = 930; //STONE

                else if (perlinValue >= -.15f && perlinValue < -.09f)
                {
                    newGID = 1322;//SAND
                }
                else if (perlinValue >= -1f && perlinValue < -.15f)
                {
                    newGID = 1622;//SANDRUIN
                }
                //else if (perlinValue >= -1f && perlinValue < -.1f)
                //{
                //    newGID = 427;//WATER
                //}

            }
            else if (layer == 1)
            {
                if (perlinValue >= .02f && perlinValue <= .07f)
                { 
                    newGID = 1015;
                }
            }
            return newGID;
        }

        public void GenerationReassignForTiling(int mainGid, List<int> generatableTiles, Dictionary<int, int> tilingDictionary, int layer,
           int x, int y, int worldWidth, int worldHeight, IInformationContainer container, List<int[,]> adjacentChunkInfo = null)
        {
            List<int> secondaryTiles = new List<int>();
            //if (generatableTiles == Game1.Procedural.DirtGeneratableTiles)
            //{
            //    secondaryTiles = Game1.Procedural.StandardGeneratableDirtTiles;
            //}
            //else if (generatableTiles == Game1.Procedural.GrassGeneratableTiles)
            //{
            //    secondaryTiles = Game1.Procedural.StandardGeneratableGrassTiles;
            //}

            //else
            //{
            //    secondaryTiles = new List<int>();
            //}


                if (!generatableTiles.Contains(container.AllTiles[layer][x, y].GID) && !secondaryTiles.Contains(container.AllTiles[layer][x, y].GID))
                {

                    return;
                }
            
            int keyToCheck = 0;
            if (y > 0)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x, y - 1].GID) || secondaryTiles.Contains(container.AllTiles[layer][x, y - 1].GID))
                {
                    keyToCheck += 1;
                }
            }
            //if top tile is 0 we look at the chunk above it

            else if (adjacentChunkInfo != null && (generatableTiles.Contains(adjacentChunkInfo[0][layer,x]) || secondaryTiles.Contains(adjacentChunkInfo[0][layer,x])))
            {
                keyToCheck += 1;
            }



            if (y < worldHeight - 1)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x, y + 1].GID) || secondaryTiles.Contains(container.AllTiles[layer][x, y + 1].GID))
                {
                    keyToCheck += 8;
                }
            }

            else if (adjacentChunkInfo != null && (generatableTiles.Contains(adjacentChunkInfo[1][layer,x]) || secondaryTiles.Contains(adjacentChunkInfo[1][layer,x])))
            {
                keyToCheck += 8;
            }

            //looking at rightmost tile
            if (x < worldWidth - 1)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x + 1, y].GID) || secondaryTiles.Contains(container.AllTiles[layer][x + 1, y].GID))
                {
                    keyToCheck += 4;
                }
            }


            else if (adjacentChunkInfo != null && (generatableTiles.Contains(adjacentChunkInfo[3][layer,y]) || secondaryTiles.Contains(adjacentChunkInfo[3][layer,y])))
            {
                keyToCheck += 4;
            }


            if (x > 0)
            {
                if (generatableTiles.Contains(container.AllTiles[layer][x - 1, y].GID) || secondaryTiles.Contains(container.AllTiles[layer][x - 1, y].GID))
                {
                    keyToCheck += 2;
                }
            }

            else if (adjacentChunkInfo != null && (generatableTiles.Contains(adjacentChunkInfo[2][layer,y]) || secondaryTiles.Contains(adjacentChunkInfo[2][layer,y])))
            {
                keyToCheck += 2;
            }

            if (keyToCheck >= 15)
            {

            }
            else
            {
                TileUtility.ReplaceTile(layer, x, y, tilingDictionary[keyToCheck] + 1, container);
                if (tilingDictionary == DirtCliffTiling)
                {
                    TileUtility.ReplaceTile(3, x, y, tilingDictionary[keyToCheck] + 1, container);
                }
            }


        }

    }
}
