using SecretProject.Class.Universal;
using System.Collections.Generic;

namespace SecretProject.Class.TileStuff.SpawnStuff
{
    public enum GenerationType
    {
        None = 0,
        Grass = 108,
        Dirt = 401,
        Sand = 1401,
        SandRuin = 1407,
        Water = 2601,
        Stone = 701,
        DirtCliff = 720,
        FenceTiling = 456,
        StoneWallTiling = 452,
        OakFloorTiling = 462,
        DirtCliffBottom = 720,
        LandSwamp = 3201,
        WaterSwamp = 3206,
        CaveCliff = 3218,
        CaveDirt = 2616,
        CaveWater = 3721,
        VillageFloor = 1054,
        VillageFence = 856,
        ArcaneFloor = 1048,
        ArcaneFence = 852,
        ForestWall = 710,
        SandStoneWall = 2103,
        DesertStone = 1701,
        GrassLake = 118,
        DeepForest = 3031,
        DarkGrass = 3608,
        BedRock = 1348


    };

    public class Procedural
    {
        public FastNoise OverworldBackNoise;

        public FastNoise UnderWorldNoise;



        private Dictionary<int, int> FillTilingDictionary(int centralGID)
        {
            return new Dictionary<int, int>()
            {
                {0, centralGID -98},{1, centralGID + 3}, {2,  centralGID + 102 },  {3, centralGID + 101}, {4, centralGID + 2}, {5, centralGID + 99},{6,centralGID - 96},
                { 7, centralGID + 100}, {8, centralGID + 103}, {9, centralGID - 97}, {10, centralGID - 99}, {11, centralGID + 1},
                { 12,centralGID - 101}, {13,centralGID - 1}, {14,centralGID - 100}, {15, centralGID}
            };
        }

        private Dictionary<int, int> FillFenceTilingDictionary(int centralGID)
        {
            return new Dictionary<int, int>()
            {
                {0, centralGID},{1, centralGID - 200}, {2,  centralGID -1 },  {3, centralGID -201}, {4, centralGID -3}, {5, centralGID -203},{6,centralGID - 2},
                { 7, centralGID -202}, {8, centralGID }, {9, centralGID - 200}, {10, centralGID - 1}, {11, centralGID -201},
                { 12,centralGID - 3}, {13,centralGID - 203}, {14,centralGID - 2}, {15, centralGID -202}
            };
        }

        private Dictionary<int, int> FillCliffTilingDictionary(int centralGID)
        {
            return new Dictionary<int, int>()
            {
                {0, centralGID -96},{1, centralGID - 97}, {2,  centralGID -98 },  {3, centralGID + 101}, {4, centralGID -102}, {5, centralGID + 99},{6,centralGID - 95},
                { 7, centralGID + 100}, {8, centralGID - 103}, {9, centralGID - 94}, {10, centralGID - 99}, {11, centralGID + 1},
                { 12,centralGID - 101}, {13,centralGID - 1}, {14,centralGID - 100}, {15, centralGID}
            };                                                                                                                              
        }
        private Dictionary<int, int> TallFillCliffTilingDictionary(int centralGID)
        {
            return new Dictionary<int, int>()
            {
                {0, centralGID + 304},{1, centralGID + 303}, {2,  centralGID +302 },  {3, centralGID + 501}, {4, centralGID +298}, {5, centralGID + 499},{6,centralGID +305},
                { 7, centralGID + 500}, {8, centralGID + 297}, {9, centralGID + 306}, {10, centralGID - 196}, {11, centralGID + 1},
                { 12,centralGID - 198}, {13,centralGID - 1}, {14,centralGID - 197}, {15, centralGID}
            };
        }

        private Dictionary<int, int> ShortFillCliffTilingDictionary(int centralGID)
        {
            return new Dictionary<int, int>()
            {
                {0, centralGID + 104},{1, centralGID + 103}, {2,  centralGID +102 },  {3, centralGID + 301}, {4, centralGID +198}, {5, centralGID + 299},{6,centralGID +105},
                { 7, centralGID + 300}, {8, centralGID + 97}, {9, centralGID + 106}, {10, centralGID - 196}, {11, centralGID + 1},
                { 12,centralGID - 198}, {13,centralGID - 1}, {14,centralGID - 197}, {15, centralGID}
            };
        }

        private Dictionary<int,int> BigTileFillDictionary(int centralGID)
        {
            return new Dictionary<int, int>()
            {
                {0, centralGID -96},{1, centralGID + 6}, {2,  centralGID + 204 },  {3, centralGID + 202}, {4, centralGID + 5}, {5, centralGID + 199},{6,centralGID - 192}, //?
                { 7, centralGID + 400}, {8, centralGID + 306}, {9, centralGID - 94}, {10, centralGID - 98}, {11, centralGID + 2},
                { 12,centralGID - 101}, {13,centralGID - 1}, {14,centralGID - 100}, {15, centralGID}
            };
        }

        public List<NoiseInterval> OverWorldBedRockNoise { get; set; }
        public List<NoiseInterval> OverWorldBackgroundNoise { get; set; }
        public List<NoiseInterval> OverWorldMidgroundNoise { get; set; }
        public List<NoiseInterval> OverWorldBuildingsNoise { get; set; }
        public List<NoiseInterval> OverworldForegroundNoise { get; set; }
        public List<NoiseInterval> OverWorldFrontNoise { get; set; }

        public List<NoiseInterval> UnderWorldBackgroundNoise { get; set; }
        public List<NoiseInterval> UnderWorldMidgroundNoise { get; set; }
        public List<NoiseInterval> UnderWorldBuildingsNoise { get; set; }
        public List<NoiseInterval> UnderworldForegroundNoise { get; set; }


        public NoiseConverter NoiseConverter { get; set; }
        public List<List<int>> AllGeneratableTiles;


        public Dictionary<GenerationType, TilingContainer> AllTilingContainers;

        public List<CliffHandler> TopCliffs { get; set; }
        public List<CliffHandler> BottomCliffs { get; set; }

        //OverworldFrontNoise = new FastNoise(500);
        //OverworldFrontNoise.SetNoiseType(FastNoise.NoiseType.SimplexFractal); // Simplex
        //    OverworldFrontNoise.SetFractalOctaves(6); // This many noise levels are added together
        //    OverworldFrontNoise.SetFractalLacunarity(2.5f); // Each level/octave of noise gets this much times higher in frequency each time

        //    //Smaller the smooth the biomes
        //    OverworldFrontNoise.SetFractalGain(.35f); // each successive noise octave/level is this times as tall/influential compared to the previous

        //    //larger the smaller the biomes
        //    OverworldFrontNoise.SetFrequency(.008f);

        public Procedural()
        {
            //FASTNOISE
            OverworldBackNoise = new FastNoise(2);
            OverworldBackNoise.SetNoiseType(FastNoise.NoiseType.SimplexFractal);
            OverworldBackNoise.SetFractalOctaves(7);
            OverworldBackNoise.SetFractalLacunarity(2.5f);

            //Smaller the smooth the biomes
            OverworldBackNoise.SetFractalGain(.45f);

            //larger the smaller the biomes
            OverworldBackNoise.SetFrequency(.001f);


            //UNDERWORLDNOISE
            UnderWorldNoise = new FastNoise(500);
            UnderWorldNoise.SetNoiseType(FastNoise.NoiseType.Cellular);
            UnderWorldNoise.SetFractalOctaves(5);
            UnderWorldNoise.SetFractalLacunarity(1.5f);

            //Smaller the smooth the biomes
            UnderWorldNoise.SetFractalGain(.045f);

            //larger the smaller the biomes
            UnderWorldNoise.SetFrequency(.09f);



            AllTilingContainers = new Dictionary<GenerationType, TilingContainer>()
            {
                 { GenerationType.None, null},
                { GenerationType.Grass, new TilingContainer(GenerationType.Grass, FillTilingDictionary((int)GenerationType.Grass), new List<int>()) },
                { GenerationType.Dirt, new TilingContainer(GenerationType.Dirt, FillTilingDictionary((int)GenerationType.Dirt), new List<int>()) },
                { GenerationType.Sand, new TilingContainer(GenerationType.Sand, FillTilingDictionary((int)GenerationType.Sand), new List<int>()) },
                { GenerationType.SandRuin, new TilingContainer(GenerationType.SandRuin, FillTilingDictionary((int)GenerationType.SandRuin), new List<int>()) },
                 { GenerationType.Water, new TilingContainer(GenerationType.Water, FillTilingDictionary((int)GenerationType.Water), new List<int>()) },
                 { GenerationType.Stone, new TilingContainer(GenerationType.Stone, FillTilingDictionary((int)GenerationType.Stone), new List<int>()) },
                 { GenerationType.DirtCliff, new TilingContainer(GenerationType.DirtCliff, TallFillCliffTilingDictionary((int)GenerationType.DirtCliff), new List<int>()) },
                 { GenerationType.FenceTiling, new TilingContainer(GenerationType.FenceTiling, FillFenceTilingDictionary((int)GenerationType.FenceTiling), new List<int>()) },
                 { GenerationType.OakFloorTiling, new TilingContainer(GenerationType.OakFloorTiling, FillTilingDictionary((int)GenerationType.OakFloorTiling), new List<int>()) },
                 { GenerationType.StoneWallTiling, new TilingContainer(GenerationType.StoneWallTiling, FillFenceTilingDictionary((int)GenerationType.StoneWallTiling), new List<int>()) },
                 { GenerationType.LandSwamp, new TilingContainer(GenerationType.LandSwamp, FillTilingDictionary((int)GenerationType.LandSwamp), new List<int>()) },
                 { GenerationType.WaterSwamp, new TilingContainer(GenerationType.WaterSwamp, FillTilingDictionary((int)GenerationType.WaterSwamp), new List<int>()) },
                 { GenerationType.CaveCliff, new TilingContainer(GenerationType.CaveCliff, FillCliffTilingDictionary((int)GenerationType.CaveCliff), new List<int>()) },
                 { GenerationType.CaveDirt, new TilingContainer(GenerationType.CaveDirt, FillTilingDictionary((int)GenerationType.CaveDirt), new List<int>()) },
                 { GenerationType.CaveWater, new TilingContainer(GenerationType.CaveWater, FillTilingDictionary((int)GenerationType.CaveWater), new List<int>()) },
                 { GenerationType.VillageFloor, new TilingContainer(GenerationType.VillageFloor, FillTilingDictionary((int)GenerationType.VillageFloor), new List<int>()) },
                 { GenerationType.VillageFence, new TilingContainer(GenerationType.VillageFence, FillFenceTilingDictionary((int)GenerationType.VillageFence), new List<int>()) },
                 { GenerationType.ArcaneFloor, new TilingContainer(GenerationType.ArcaneFloor, FillTilingDictionary((int)GenerationType.ArcaneFloor), new List<int>()) },
                 { GenerationType.ArcaneFence, new TilingContainer(GenerationType.ArcaneFence, FillFenceTilingDictionary((int)GenerationType.ArcaneFence), new List<int>()) },
                 { GenerationType.ForestWall, new TilingContainer(GenerationType.ForestWall, TallFillCliffTilingDictionary((int)GenerationType.ForestWall), new List<int>()) },
                 { GenerationType.SandStoneWall, new TilingContainer(GenerationType.SandStoneWall, ShortFillCliffTilingDictionary((int)GenerationType.SandStoneWall), new List<int>()) },
                 { GenerationType.DesertStone, new TilingContainer(GenerationType.DesertStone, FillTilingDictionary((int)GenerationType.DesertStone), new List<int>()) },
                 { GenerationType.GrassLake, new TilingContainer(GenerationType.GrassLake, FillTilingDictionary((int)GenerationType.GrassLake), new List<int>()) },
                 { GenerationType.DeepForest, new TilingContainer(GenerationType.DeepForest, BigTileFillDictionary((int)GenerationType.DeepForest), new List<int>()) },
                   { GenerationType.DarkGrass, new TilingContainer(GenerationType.DarkGrass, FillTilingDictionary((int)GenerationType.DarkGrass), new List<int>()) },
                   { GenerationType.BedRock, new TilingContainer(GenerationType.BedRock, FillTilingDictionary((int)GenerationType.BedRock), new List<int>()) },
            };

            this.OverWorldBedRockNoise = new List<NoiseInterval>();
            //NOISE INTERVALS, must be sorted by interval
            this.OverWorldBackgroundNoise = new List<NoiseInterval>()
            {
                new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.GrassLake),-1f, -.4f ),
                new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.Dirt), -.4f, -.08f),
                new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.Dirt), -.08f, .04f),
                 new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.GrassLake), .04f, .07f),
                 new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.Dirt), .07f, .36f),
                new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.Dirt),.36f, .37f),
                new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.Water),.37f, .372f),
                new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.Dirt),.372f, .39f),
                new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.Water),.39f, .43f),
                new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.Dirt),.43f, .46f),
                 new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.Water),.46f, 1f),


            };
            this.OverWorldMidgroundNoise = new List<NoiseInterval>()
                {
                    new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DarkGrass),-.14f, -.12f ),
                    new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DarkGrass),-.05f, -.047f),
                    new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DarkGrass),-.07f, .03f),
                    new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DarkGrass),.071f, .12f),
                    new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DarkGrass),.12f,.123f),
                    new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DarkGrass),.13f, .18f),
                    new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DarkGrass),.24f,.27f),
                    new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DarkGrass),.27f, .32f),
                    new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DarkGrass),.37f, .372f),
                    new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DarkGrass),.39f, .43f),
                    new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DarkGrass),.46f, 1f),

                };
            this.OverWorldBuildingsNoise = new List<NoiseInterval>()
            {

            };

            this.OverworldForegroundNoise = new List<NoiseInterval>()
                {
                    //new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DirtCliff), .3f,.33f),
                    //new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DeepForest), .2f,.25f),
                    //new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DeepForest), .14f,.145f),
                    //new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DeepForest), .1f,.119f),

                    //new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.ForestWall), .02f,.024f),
                    //new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.SandStoneWall), -.3f,-.29f),
                    //new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.SandStoneWall), -.24f,-.2f),

                    //new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.SandStoneWall), -.115f,-.1f),


                };

            this.OverWorldFrontNoise = new List<NoiseInterval>()
            {
                new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DeepForest), .2f,.25f),
                    new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DeepForest), .14f,.145f),
                    new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.DeepForest), .1f,.119f),
            };

            List<List<NoiseInterval>> allOverworldNoise = new List<List<NoiseInterval>>()
            {
                OverWorldBedRockNoise,
                OverWorldBackgroundNoise,
                OverWorldMidgroundNoise,
                OverWorldBuildingsNoise,
                OverworldForegroundNoise,
                OverWorldFrontNoise,

            };

            this.UnderWorldBackgroundNoise = new List<NoiseInterval>()
            {
                new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.CaveDirt),-1f, 1f ),
            };
            this.UnderWorldMidgroundNoise = new List<NoiseInterval>()
            {
                new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.CaveWater),-.2f, 0f ),
            };
            this.UnderWorldBuildingsNoise = new List<NoiseInterval>()
            {
            };
            this.UnderworldForegroundNoise = new List<NoiseInterval>()
            {
                  new NoiseInterval(GetTilingContainerFromGenerationType(GenerationType.CaveCliff),.2f, 1f ),
            };
            List<List<NoiseInterval>> allUnderWorldNoise = new List<List<NoiseInterval>>()
            {
                UnderWorldBackgroundNoise,
                UnderWorldMidgroundNoise,
                UnderWorldBuildingsNoise,
                UnderworldForegroundNoise
            };

            this.NoiseConverter = new NoiseConverter(allOverworldNoise, allUnderWorldNoise);

            TilingContainer sandStoneWallContainer = GetTilingContainerFromGenerationType(GenerationType.SandStoneWall);
            this.TopCliffs = new List<CliffHandler>()
            {
               // new CliffHandler(CliffHandler.GetTopCliffEdges(dirtCliffContainer), dirtCliffContainer.TilingDictionary[15], 4723),
                //new CliffHandler(CliffHandler.GetTopCliffEdges(forestWallContainer), forestWallContainer.TilingDictionary[15], 4613),
              //  new CliffHandler(CliffHandler.GetTopCliffEdges(sandStoneWallContainer), sandStoneWallContainer.TilingDictionary[15], 5113),

            };
            TilingContainer unraiCliffs = GetTilingContainerFromGenerationType(GenerationType.CaveCliff);
            this.BottomCliffs = new List<CliffHandler>()
            {
                new CliffHandler(CliffHandler.GetTopCliffEdges(unraiCliffs), unraiCliffs.TilingDictionary[15], 5428),


            };
        }

        public void ExtendCliffs(IInformationContainer container)
        {
           // if(Game1.CurrentStage == Game1.OverWorld)
          //  {
                foreach(CliffHandler handler in this.TopCliffs)
                {
                    handler.ExtendCliffs(container);
                }
            //}
            //else if (Game1.CurrentStage == Game1.UnderWorld)
            //{
            //    foreach (CliffHandler handler in this.BottomCliffs)
            //    {
            //        handler.ExtendCliffs(container);
            //    }
            //}
        }

        public void HandleCliffEdgeCases(IInformationContainer container, List<int[,,]> allAdjacentChunkNoise)
        {
          //  this.TopCliffs[1].HandleCliffEdgeCases(container, allAdjacentChunkNoise);
            foreach (CliffHandler handler in this.TopCliffs)
            {
                handler.HandleCliffEdgeCases(container, allAdjacentChunkNoise);
            }
        }

        public TilingContainer GetTilingContainerFromGenerationType(GenerationType generationType)
        {
            return AllTilingContainers[generationType];
        }

        public TilingContainer GetTilingContainerFromGID(GenerationType tileGeneratyionType)
        {
            return AllTilingContainers[tileGeneratyionType];


        }

        //public void GeneratePerlinTiles(int layerToPlace, int x, int y, int gid, List<int> acceptableGenerationTiles, int layerToCheckIfEmpty, IInformationContainer container, int comparisonLayer, int chance = 100)
        //{
        //    if (chance == 100)
        //    {
        //        if (!TileUtility.CheckIfTileAlreadyExists(x, y, layerToPlace, container) && TileUtility.CheckIfTileMatchesGID(x, y, layerToPlace,
        //       acceptableGenerationTiles, container, comparisonLayer))
        //        {
        //            container.AllTiles[layerToPlace][x, y] = new Tile(x, y, gid);
        //        }
        //    }

        //    else
        //    {
        //        if (Game1.Utility.RGenerator.Next(0, 101) < chance)
        //        {
        //            if (!TileUtility.CheckIfTileAlreadyExists(x, y, layerToPlace, container) && TileUtility.CheckIfTileMatchesGID(x, y, layerToPlace,
        //       acceptableGenerationTiles, container, comparisonLayer))
        //            {
        //                container.AllTiles[layerToPlace][x, y] = new Tile(x, y, gid);
        //            }
        //        }

        //    }

        //}



       

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
