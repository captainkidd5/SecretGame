using SecretProject.Class.Universal;
using System.Collections.Generic;

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
        StoneWallTiling = 452,
        OakFloorTiling = 632,
        DirtCliffBottom = 3534

    };

    public enum GenerationIndex
    {
        Grass = 0,
        Dirt = 1,
        Sand = 2,
        SandRuin = 3,
        Water = 4,
        Stone = 5,
        DirtCliff = 6,
        FenceTiling = 7,
        OakFloorTiling = 8,
        StoneWallTiling = 9,
        DirtCliffBottom = 10

    }
    public class Procedural
    {
        public FastNoise FastNoise;



        public Dictionary<int, int> FillTilingDictionary(int centralGID)
        {
            return new Dictionary<int, int>()
            {
                {0, centralGID -98},{1, centralGID + 3}, {2,  centralGID + 102 },  {3, centralGID + 101}, {4, centralGID + 2}, {5, centralGID + 99},{6,centralGID - 96},
                { 7, centralGID + 100}, {8, centralGID + 103}, {9, centralGID - 97}, {10, centralGID - 99}, {11, centralGID + 1},
                { 12,centralGID - 101}, {13,centralGID - 1}, {14,centralGID - 100}, {15, centralGID}
            };
        }

        public Dictionary<int, int> FillFenceTilingDictionary(int centralGID)
        {
            return new Dictionary<int, int>()
            {
                {0, centralGID},{1, centralGID - 200}, {2,  centralGID -1 },  {3, centralGID -201}, {4, centralGID -3}, {5, centralGID -203},{6,centralGID - 2},
                { 7, centralGID -202}, {8, centralGID }, {9, centralGID - 200}, {10, centralGID - 1}, {11, centralGID -201},
                { 12,centralGID - 3}, {13,centralGID - 203}, {14,centralGID - 2}, {15, centralGID -202}
            };
        }

        public List<List<int>> AllGeneratableTiles;

        public TilingContainer Grass;
        public TilingContainer Dirt;
        public TilingContainer Sand;
        public TilingContainer SandRuin;
        public TilingContainer Water;
        public TilingContainer Stone;
        public TilingContainer DirtCliff;
        public TilingContainer OakFence;
        public TilingContainer StoneWall;
        public TilingContainer OakFloor;
        public TilingContainer DirtCliffBottom;

        public List<TilingContainer> AllTilingContainers;



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




            //   Grass = new TilingContainer(GenerationType.Grass, FillTilingDictionary(1014), new List<int>());
            //Dirt = new TilingContainer(GenerationType.Dirt, FillTilingDictionary(1005), new List<int>());
            //   Sand = new TilingContainer(GenerationType.Sand, FillTilingDictionary(1321), new List<int>());
            //   SandRuin = new TilingContainer(GenerationType.SandRuin, FillTilingDictionary(1621), new List<int>());
            //   Water = new TilingContainer(GenerationType.Water, FillTilingDictionary(426), new List<int>());
            //   Stone = new TilingContainer(GenerationType.Stone, FillTilingDictionary(929), new List<int>());
            //   DirtCliff = new TilingContainer(GenerationType.DirtCliff, FillTilingDictionary(2934), new List<int>());
            //   OakFence = new TilingContainer(GenerationType.OakFloorTiling, FillFenceTilingDictionary(632), new List<int>());
            //   StoneWall = new TilingContainer(GenerationType.StoneWallTiling, FillFenceTilingDictionary(452), new List<int>());
            //   OakFloor = new TilingContainer(GenerationType.DirtCliff, FillTilingDictionary(632), new List<int>());

            //MUST ADD IN THE SAME ORDER AS ENUM
            AllTilingContainers = new List<TilingContainer>()
            {
                new TilingContainer(GenerationType.Grass, FillTilingDictionary(1014), new List<int>()),
                new TilingContainer(GenerationType.Dirt, FillTilingDictionary(1005), new List<int>()),
                new TilingContainer(GenerationType.Sand, FillTilingDictionary(1321), new List<int>()),
                new TilingContainer(GenerationType.SandRuin, FillTilingDictionary(1621), new List<int>()),
                new TilingContainer(GenerationType.Water, FillTilingDictionary(426), new List<int>()),
                new TilingContainer(GenerationType.Stone, FillTilingDictionary(929), new List<int>()),
                new TilingContainer(GenerationType.DirtCliff, FillTilingDictionary(2934), new List<int>()),
                new TilingContainer(GenerationType.FenceTiling, FillFenceTilingDictionary(456), new List<int>()),
                new TilingContainer(GenerationType.OakFloorTiling, FillTilingDictionary(632), new List<int>()),
                new TilingContainer(GenerationType.StoneWallTiling, FillFenceTilingDictionary(452), new List<int>()),
                new TilingContainer(GenerationType.DirtCliff, FillTilingDictionary(632), new List<int>()),
                new TilingContainer(GenerationType.DirtCliffBottom, null, new List<int>()
                {
                    3534
                }),
            };
        }

        public TilingContainer GetTilingContainerFromGenerationType(GenerationType generationType)
        {
            for (int i = 0; i < AllTilingContainers.Count; i++)
            {
                if (AllTilingContainers[i].GenerationType == generationType)
                {

                    return AllTilingContainers[i];
                }
            }
            return null;
        }

        public TilingContainer GetTilingContainerFromGID(int gid)
        {
            for (int i = 0; i < AllTilingContainers.Count; i++)
            {
                for (int j = 0; j < AllTilingContainers[i].GeneratableTiles.Count; j++)
                {
                    if (gid == AllTilingContainers[i].GeneratableTiles[j])
                    {
                        return AllTilingContainers[i];
                    }
                }
            }
            return null;
        }
        //public GenerationType GetGenerationTypeFromGID(int gid)
        //{
        //    for(int i =0; i < AllGeneratableTiles.Count; i++)
        //    {
        //        if(AllGeneratableTiles[i].Contains(gid))
        //    }
        //}



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
                    //newGID = 1006;
                    newGID = Game1.Procedural.GetTilingContainerFromGenerationType(GenerationType.Dirt).GeneratableTiles[Game1.Utility.RGenerator.Next(0, Game1.Procedural.GetTilingContainerFromGenerationType(GenerationType.Dirt).GeneratableTiles.Count)] + 1;
                }
                else if (perlinValue >= .12f && perlinValue <= .2f)
                {
                    // newGID = Game1.Procedural.StandardGeneratableDirtTiles[Game1.Utility.RGenerator.Next(0, Game1.Procedural.StandardGeneratableDirtTiles.Count)] + 1;
                    newGID = Game1.Procedural.GetTilingContainerFromGenerationType(GenerationType.Dirt).GeneratableTiles[Game1.Utility.RGenerator.Next(0, Game1.Procedural.GetTilingContainerFromGenerationType(GenerationType.Dirt).GeneratableTiles.Count)] + 1;
                }
                else if (perlinValue >= .1f && perlinValue <= .12f)
                {
                    newGID = Game1.Procedural.GetTilingContainerFromGenerationType(GenerationType.Dirt).GeneratableTiles[Game1.Utility.RGenerator.Next(0, Game1.Procedural.GetTilingContainerFromGenerationType(GenerationType.Dirt).GeneratableTiles.Count)] + 1;
                }
                else if (perlinValue >= .07f && perlinValue <= .1f)
                {

                    newGID = Game1.Procedural.GetTilingContainerFromGenerationType(GenerationType.Dirt).GeneratableTiles[Game1.Utility.RGenerator.Next(0, Game1.Procedural.GetTilingContainerFromGenerationType(GenerationType.Dirt).GeneratableTiles.Count)] + 1;

                }
                else if (perlinValue >= .02f && perlinValue <= .07f)
                {

                    //   newGID = Game1.Procedural.StandardGeneratableGrassTiles[Game1.Utility.RGenerator.Next(0, Game1.Procedural.StandardGeneratableDirtTiles.Count)] + 1;
                    newGID = Game1.Procedural.GetTilingContainerFromGenerationType(GenerationType.Dirt).GeneratableTiles[Game1.Utility.RGenerator.Next(0, Game1.Procedural.GetTilingContainerFromGenerationType(GenerationType.Dirt).GeneratableTiles.Count)] + 1;
                }

                else if (perlinValue >= -.09f && perlinValue < .02f)
                {
                    // newGID = Game1.Procedural.StandardGeneratableGrassTiles[Game1.Utility.RGenerator.Next(0, Game1.Procedural.StandardGeneratableDirtTiles.Count)] + 1;
                    newGID = Game1.Procedural.GetTilingContainerFromGenerationType(GenerationType.Dirt).GeneratableTiles[Game1.Utility.RGenerator.Next(0, Game1.Procedural.GetTilingContainerFromGenerationType(GenerationType.Dirt).GeneratableTiles.Count)] + 1;
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
                    newGID = 1322;//SAND
                }
                //else if (perlinValue >= -1f && perlinValue < -.1f)
                //{
                //    newGID = 427;//WATER
                //}

            }
            else if (layer == 1)
            {
                if (perlinValue >= -.1f && perlinValue <= .0f)
                {
                    newGID = 1015;//GRASS
                }
                else if (perlinValue >= .1f && perlinValue <= .12f)
                {
                    newGID = 930;//Stone
                }
                else if (perlinValue >= .07f && perlinValue <= .1f)
                {

                    newGID = 930;//Stone

                }
                else if (perlinValue >= .15f && perlinValue <= .3f)
                {
                    newGID = 1015; //GRASS
                }

                else if (perlinValue >= -1f && perlinValue < -.15f)
                {
                    newGID = 1622;//SANDRUIN
                }
            }
            else if (layer == 3)
            {
                if (perlinValue >= .07f && perlinValue <= .1f)
                {

                    newGID = 2935; //dirt cliff

                }
            }
            return newGID;
        }

        public enum RelativeChunkPosition
        {
            ChunkAbove = 0,
            ChunkBelow = 1,
            ChunkLeft = 2,
            ChunkRight = 3

        }


        public void GenerationReassignForTiling(int mainGid, List<int> generatableTiles, Dictionary<int, int> tilingDictionary, int layer,
           int x, int y, int worldWidth, int worldHeight, IInformationContainer container, List<int[,,]> adjacentChunkInfo = null)
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

            else if (adjacentChunkInfo != null && (generatableTiles.Contains(adjacentChunkInfo[(int)RelativeChunkPosition.ChunkAbove][layer, x, 15]) || secondaryTiles.Contains(adjacentChunkInfo[(int)RelativeChunkPosition.ChunkAbove][layer, x, 15])))
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

            else if (adjacentChunkInfo != null && (generatableTiles.Contains(adjacentChunkInfo[(int)RelativeChunkPosition.ChunkBelow][layer, x, 0]) || secondaryTiles.Contains(adjacentChunkInfo[(int)RelativeChunkPosition.ChunkBelow][layer, x, 0])))
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


            else if (adjacentChunkInfo != null && (generatableTiles.Contains(adjacentChunkInfo[(int)RelativeChunkPosition.ChunkRight][layer, 0, y]) || secondaryTiles.Contains(adjacentChunkInfo[(int)RelativeChunkPosition.ChunkRight][layer, 0, y])))
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

            else if (adjacentChunkInfo != null && (generatableTiles.Contains(adjacentChunkInfo[(int)RelativeChunkPosition.ChunkLeft][layer, 15, y]) || secondaryTiles.Contains(adjacentChunkInfo[(int)RelativeChunkPosition.ChunkLeft][layer, 15, y])))
            {
                keyToCheck += 2;
            }

            if (keyToCheck >= 15)
            {

            }
            else
            {
                container.AllTiles[layer][x, y].GID = tilingDictionary[keyToCheck] + 1;
            }
        }


        List<int> CliffBottomTiles = new List<int>()
        {
            3033, 3034, 3035
        };

    }

    public class TilingContainer
    {
        public GenerationType GenerationType { get; set; }
        public Dictionary<int, int> TilingDictionary { get; set; }
        public List<int> GeneratableTiles { get; set; }

        public TilingContainer(GenerationType generationType, Dictionary<int, int> tilingDictionary, List<int> generatableTiles)
        {
            this.GenerationType = generationType;
            this.TilingDictionary = tilingDictionary;
            this.GeneratableTiles = generatableTiles;
        }
    }
}
