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
        Stone = 1648,
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
        BedRock = 1348,
        FertileSoil = 102,
        PollutedSoil = 701,
        PollutedSand = 1001


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


        public Dictionary<GenerationType, TilingTileManager> AllTilingTileManagers;

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



            AllTilingTileManagers = new Dictionary<GenerationType, TilingTileManager>()
            {
                 { GenerationType.None, null},
                { GenerationType.Grass, new TilingTileManager(GenerationType.Grass, FillTilingDictionary((int)GenerationType.Grass), new List<int>()) },
                { GenerationType.Dirt, new TilingTileManager(GenerationType.Dirt, FillTilingDictionary((int)GenerationType.Dirt), new List<int>()) },
                { GenerationType.Sand, new TilingTileManager(GenerationType.Sand, FillTilingDictionary((int)GenerationType.Sand), new List<int>()) },
                { GenerationType.SandRuin, new TilingTileManager(GenerationType.SandRuin, FillTilingDictionary((int)GenerationType.SandRuin), new List<int>()) },
                 { GenerationType.Water, new TilingTileManager(GenerationType.Water, FillTilingDictionary((int)GenerationType.Water), new List<int>()) },
                 { GenerationType.Stone, new TilingTileManager(GenerationType.Stone, FillTilingDictionary((int)GenerationType.Stone), new List<int>()) },
                 { GenerationType.DirtCliff, new TilingTileManager(GenerationType.DirtCliff, TallFillCliffTilingDictionary((int)GenerationType.DirtCliff), new List<int>()) },
                 { GenerationType.FenceTiling, new TilingTileManager(GenerationType.FenceTiling, FillFenceTilingDictionary((int)GenerationType.FenceTiling), new List<int>()) },
                 { GenerationType.OakFloorTiling, new TilingTileManager(GenerationType.OakFloorTiling, FillTilingDictionary((int)GenerationType.OakFloorTiling), new List<int>()) },
                 { GenerationType.StoneWallTiling, new TilingTileManager(GenerationType.StoneWallTiling, FillFenceTilingDictionary((int)GenerationType.StoneWallTiling), new List<int>()) },
                 { GenerationType.LandSwamp, new TilingTileManager(GenerationType.LandSwamp, FillTilingDictionary((int)GenerationType.LandSwamp), new List<int>()) },
                 { GenerationType.WaterSwamp, new TilingTileManager(GenerationType.WaterSwamp, FillTilingDictionary((int)GenerationType.WaterSwamp), new List<int>()) },
                 { GenerationType.CaveCliff, new TilingTileManager(GenerationType.CaveCliff, FillCliffTilingDictionary((int)GenerationType.CaveCliff), new List<int>()) },
                 { GenerationType.CaveDirt, new TilingTileManager(GenerationType.CaveDirt, FillTilingDictionary((int)GenerationType.CaveDirt), new List<int>()) },
                 { GenerationType.CaveWater, new TilingTileManager(GenerationType.CaveWater, FillTilingDictionary((int)GenerationType.CaveWater), new List<int>()) },
                 { GenerationType.VillageFloor, new TilingTileManager(GenerationType.VillageFloor, FillTilingDictionary((int)GenerationType.VillageFloor), new List<int>()) },
                 { GenerationType.VillageFence, new TilingTileManager(GenerationType.VillageFence, FillFenceTilingDictionary((int)GenerationType.VillageFence), new List<int>()) },
                 { GenerationType.ArcaneFloor, new TilingTileManager(GenerationType.ArcaneFloor, FillTilingDictionary((int)GenerationType.ArcaneFloor), new List<int>()) },
                 { GenerationType.ArcaneFence, new TilingTileManager(GenerationType.ArcaneFence, FillFenceTilingDictionary((int)GenerationType.ArcaneFence), new List<int>()) },
                 { GenerationType.ForestWall, new TilingTileManager(GenerationType.ForestWall, TallFillCliffTilingDictionary((int)GenerationType.ForestWall), new List<int>()) },
                 { GenerationType.SandStoneWall, new TilingTileManager(GenerationType.SandStoneWall, ShortFillCliffTilingDictionary((int)GenerationType.SandStoneWall), new List<int>()) },
                 { GenerationType.DesertStone, new TilingTileManager(GenerationType.DesertStone, FillTilingDictionary((int)GenerationType.DesertStone), new List<int>()) },
                 { GenerationType.GrassLake, new TilingTileManager(GenerationType.GrassLake, FillTilingDictionary((int)GenerationType.GrassLake), new List<int>()) },
                 { GenerationType.DeepForest, new TilingTileManager(GenerationType.DeepForest, BigTileFillDictionary((int)GenerationType.DeepForest), new List<int>()) },
                   { GenerationType.DarkGrass, new TilingTileManager(GenerationType.DarkGrass, FillTilingDictionary((int)GenerationType.DarkGrass), new List<int>()) },
                   { GenerationType.BedRock, new TilingTileManager(GenerationType.BedRock, FillTilingDictionary((int)GenerationType.BedRock), new List<int>()) },
                    { GenerationType.FertileSoil, new TilingTileManager(GenerationType.FertileSoil, FillTilingDictionary((int)GenerationType.FertileSoil), new List<int>()) },
                    { GenerationType.PollutedSoil, new TilingTileManager(GenerationType.PollutedSoil, FillTilingDictionary((int)GenerationType.PollutedSoil), new List<int>()) },
                    { GenerationType.PollutedSand, new TilingTileManager(GenerationType.PollutedSand, FillTilingDictionary((int)GenerationType.PollutedSand), new List<int>()) },
            };

            this.OverWorldBedRockNoise = new List<NoiseInterval>()
            {
                new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.BedRock),-1f, 1f ),
            };
            //NOISE INTERVALS, must be sorted by interval
            this.OverWorldBackgroundNoise = new List<NoiseInterval>()
            {
                new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.GrassLake),-1f, -.4f ),
                new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.Dirt), -.4f, -.08f),
                new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.Dirt), -.08f, .04f),
                 new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.GrassLake), .04f, .07f),
                 new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.Dirt), .07f, .36f),
                new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.Dirt),.36f, .37f),
                new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.Water),.37f, .372f),
                new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.Dirt),.372f, .39f),
                new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.Water),.39f, .43f),
                new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.Dirt),.43f, .46f),
                 new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.Water),.46f, 1f),


            };
            this.OverWorldMidgroundNoise = new List<NoiseInterval>()
                {
                    new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DarkGrass),-.14f, -.12f ),
                    new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DarkGrass),-.05f, -.047f),
                    new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DarkGrass),-.07f, .03f),
                    new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DarkGrass),.071f, .12f),
                    new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DarkGrass),.12f,.123f),
                    new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DarkGrass),.13f, .18f),
                    new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DarkGrass),.24f,.27f),
                    new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DarkGrass),.27f, .32f),
                    new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DarkGrass),.37f, .372f),
                    new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DarkGrass),.39f, .43f),
                    new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DarkGrass),.46f, 1f),

                };
            this.OverWorldBuildingsNoise = new List<NoiseInterval>()
            {

            };

            this.OverworldForegroundNoise = new List<NoiseInterval>()
                {
                    //new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DirtCliff), .3f,.33f),
                    //new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DeepForest), .2f,.25f),
                    //new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DeepForest), .14f,.145f),
                    //new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DeepForest), .1f,.119f),

                    //new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.ForestWall), .02f,.024f),
                    //new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.SandStoneWall), -.3f,-.29f),
                    //new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.SandStoneWall), -.24f,-.2f),

                    //new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.SandStoneWall), -.115f,-.1f),


                };

            this.OverWorldFrontNoise = new List<NoiseInterval>()
            {
                new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DeepForest), .2f,.25f),
                    new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DeepForest), .14f,.145f),
                    new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.DeepForest), .1f,.119f),
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
                new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.CaveDirt),-1f, 1f ),
            };
            this.UnderWorldMidgroundNoise = new List<NoiseInterval>()
            {
                new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.CaveWater),-.2f, 0f ),
            };
            this.UnderWorldBuildingsNoise = new List<NoiseInterval>()
            {
            };
            this.UnderworldForegroundNoise = new List<NoiseInterval>()
            {
                  new NoiseInterval(GetTilingTileManagerFromGenerationType(GenerationType.CaveCliff),.2f, 1f ),
            };
            List<List<NoiseInterval>> allUnderWorldNoise = new List<List<NoiseInterval>>()
            {
                UnderWorldBackgroundNoise,
                UnderWorldMidgroundNoise,
                UnderWorldBuildingsNoise,
                UnderworldForegroundNoise
            };

            this.NoiseConverter = new NoiseConverter(allOverworldNoise, allUnderWorldNoise);

            TilingTileManager sandStoneWallTileManager = GetTilingTileManagerFromGenerationType(GenerationType.SandStoneWall);
            this.TopCliffs = new List<CliffHandler>()
            {
               // new CliffHandler(CliffHandler.GetTopCliffEdges(dirtCliffTileManager), dirtCliffTileManager.TilingDictionary[15], 4723),
                //new CliffHandler(CliffHandler.GetTopCliffEdges(forestWallTileManager), forestWallTileManager.TilingDictionary[15], 4613),
              //  new CliffHandler(CliffHandler.GetTopCliffEdges(sandStoneWallTileManager), sandStoneWallTileManager.TilingDictionary[15], 5113),

            };
            TilingTileManager unraiCliffs = GetTilingTileManagerFromGenerationType(GenerationType.CaveCliff);
            this.BottomCliffs = new List<CliffHandler>()
            {
                new CliffHandler(CliffHandler.GetTopCliffEdges(unraiCliffs), unraiCliffs.TilingDictionary[15], 5428),


            };
        }

        public void ExtendCliffs(TileManager TileManager)
        {
           // if(Game1.CurrentStage == Game1.OverWorld)
          //  {
                foreach(CliffHandler handler in this.TopCliffs)
                {
                    handler.ExtendCliffs(TileManager);
                }
            //}
            //else if (Game1.CurrentStage == Game1.UnderWorld)
            //{
            //    foreach (CliffHandler handler in this.BottomCliffs)
            //    {
            //        handler.ExtendCliffs(TileManager);
            //    }
            //}
        }

        public void HandleCliffEdgeCases(TileManager TileManager, List<int[,,]> allAdjacentChunkNoise)
        {
          //  this.TopCliffs[1].HandleCliffEdgeCases(TileManager, allAdjacentChunkNoise);
            foreach (CliffHandler handler in this.TopCliffs)
            {
                handler.HandleCliffEdgeCases(TileManager, allAdjacentChunkNoise);
            }
        }

        public TilingTileManager GetTilingTileManagerFromGenerationType(GenerationType generationType)
        {
            return AllTilingTileManagers[generationType];
        }

        public TilingTileManager GetTilingTileManagerFromGID(GenerationType tileGeneratyionType)
        {
            return AllTilingTileManagers[tileGeneratyionType];


        }

        //public void GeneratePerlinTiles(int layerToPlace, int x, int y, int gid, List<int> acceptableGenerationTiles, int layerToCheckIfEmpty, TileManager TileManager, int comparisonLayer, int chance = 100)
        //{
        //    if (chance == 100)
        //    {
        //        if (!TileUtility.CheckIfTileAlreadyExists(x, y, layerToPlace, TileManager) && TileUtility.CheckIfTileMatchesGID(x, y, layerToPlace,
        //       acceptableGenerationTiles, TileManager, comparisonLayer))
        //        {
        //            TileManager.AllTiles[layerToPlace][x, y] = new Tile(x, y, gid);
        //        }
        //    }

        //    else
        //    {
        //        if (Game1.Utility.RGenerator.Next(0, 101) < chance)
        //        {
        //            if (!TileUtility.CheckIfTileAlreadyExists(x, y, layerToPlace, TileManager) && TileUtility.CheckIfTileMatchesGID(x, y, layerToPlace,
        //       acceptableGenerationTiles, TileManager, comparisonLayer))
        //            {
        //                TileManager.AllTiles[layerToPlace][x, y] = new Tile(x, y, gid);
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
           int x, int y, int worldWidth, int worldHeight, TileManager TileManager, List<int[,,]> adjacentChunkInfo = null)
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


            if (!generatableTiles.Contains(TileManager.AllTiles[layer][x, y].GID) && !secondaryTiles.Contains(TileManager.AllTiles[layer][x, y].GID))
            {

                return;
            }

            int keyToCheck = 0;
            if (y > 0)
            {
                if (generatableTiles.Contains(TileManager.AllTiles[layer][x, y - 1].GID) || secondaryTiles.Contains(TileManager.AllTiles[layer][x, y - 1].GID))
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
                if (generatableTiles.Contains(TileManager.AllTiles[layer][x, y + 1].GID) || secondaryTiles.Contains(TileManager.AllTiles[layer][x, y + 1].GID))
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
                if (generatableTiles.Contains(TileManager.AllTiles[layer][x + 1, y].GID) || secondaryTiles.Contains(TileManager.AllTiles[layer][x + 1, y].GID))
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
                if (generatableTiles.Contains(TileManager.AllTiles[layer][x - 1, y].GID) || secondaryTiles.Contains(TileManager.AllTiles[layer][x - 1, y].GID))
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
                TileManager.AllTiles[layer][x, y].GID = tilingDictionary[keyToCheck] + 1;
            }
        }


        List<int> CliffBottomTiles = new List<int>()
        {
            3033, 3034, 3035
        };

    }

    public class TilingTileManager
    {
        public GenerationType GenerationType { get; set; }
        public Dictionary<int, int> TilingDictionary { get; set; }
        public List<int> GeneratableTiles { get; set; }

        public TilingTileManager(GenerationType generationType, Dictionary<int, int> tilingDictionary, List<int> generatableTiles)
        {
            this.GenerationType = generationType;
            this.TilingDictionary = tilingDictionary;
            this.GeneratableTiles = generatableTiles;
        }
    }


}
