using Microsoft.Xna.Framework;
using SecretProject.Class.PathFinding;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff.SpawnStuff.CampStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.SpawnStuff
{
    public enum Rarity
    {
        None = 0,
        ExtremelyRare = 1,
        VeryRare = 5,
        Rare = 15,
        Uncommon = 30,
        Common = 50,
        VeryCommon = 70,
        Abundant = 90


    }
    public class SpawnHolder
    {
        public List<SpawnElement> OverWorldSpawnElements { get; set; }
        public List<SpawnElement> UnderWorldSpawnElements { get; set; }
        public List<Camp> AllCamps { get; set; }

        public static int CampSpawnRate { get; set; } = 20;


        public SpawnHolder()
        {
            this.OverWorldSpawnElements = new List<SpawnElement>()
            {
                new SpawnElement(2963,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Abundant, 2, false, 25, 30){Unlocked = true }, //Pine Tree 
                 new SpawnElement(3663,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Abundant, 2, false, 20, 30){Unlocked = true }, //Cedar Tree
                  new SpawnElement(4263,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Abundant, 2, false, 20, 30){Unlocked = true }, //Fir Tree 
                new SpawnElement(978,SpawnMethod.Poisson, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Abundant,1, false, 30, 15){Unlocked = true }, //Stone
                new SpawnElement(1078,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon,1, false, 30, 15){Unlocked = true }, //Grass Tuft
                new SpawnElement(978,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Abundant,1, false, 30, 15){Unlocked = true }, //Stone
                new SpawnElement(1078,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon,1, false, 30, 15){Unlocked = true }, //Grass Tuft
                new SpawnElement(1580,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon,1, false, 30, 15){Unlocked = true }, //Stick
                new SpawnElement(1579,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon,1, false, 30, 15){Unlocked = true }, //rock
                new SpawnElement(1077,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Common,Rarity.VeryCommon,7, false, 1, 15){Unlocked = true }, //Steel

                 new SpawnElement(1076,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.MidGround, GenerationType.Stone, Rarity.Uncommon,Rarity.VeryCommon,7, false, 1, 15){Unlocked = true }, //copper
                  new SpawnElement(1075,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.MidGround, GenerationType.Stone, Rarity.Uncommon,Rarity.VeryCommon,7, false, 1, 15){Unlocked = true }, //monzanite
                   new SpawnElement(1074,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.MidGround, GenerationType.Stone, Rarity.Uncommon,Rarity.VeryCommon,7, false, 1, 15){Unlocked = true }, //blunate
                    new SpawnElement(1073,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.MidGround, GenerationType.Stone, Rarity.Uncommon,Rarity.VeryCommon,7, false, 1, 15){Unlocked = true }, //santalite

                    new SpawnElement(1076,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.Rare,Rarity.VeryCommon,7, false, 1, 15){Unlocked = true }, //copper
                  new SpawnElement(1075,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Rare,Rarity.VeryCommon,7, false, 1, 15){Unlocked = true }, //monzanite
                   new SpawnElement(1074,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.VeryRare,Rarity.VeryCommon,7, false, 1, 15){Unlocked = true }, //blunate
                    new SpawnElement(1073,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.ExtremelyRare,Rarity.VeryCommon,7, false, 1, 15){Unlocked = true }, //santalite

                new SpawnElement(1681,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.VeryCommon,Rarity.VeryCommon,1, false, 30, 15){Unlocked = true }, //carrotte
                 new SpawnElement(1380,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.VeryCommon,Rarity.VeryCommon,1, false, 30, 15){Unlocked = true }, //pumpkin
                 new SpawnElement(1581,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.VeryCommon,Rarity.VeryCommon,1, false, 30, 15){Unlocked = true }, //Red Mushroom\
                 new SpawnElement(1582,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.VeryCommon,Rarity.VeryCommon,1, false, 30, 15){Unlocked = true }, //Blue Mushroom
            //    new SpawnElement(1586, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 5){Unlocked = true }, //Clue Fruit
            //    new SpawnElement(3161, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 5, false, true){Unlocked = true }, //big stump
            //    new SpawnElement(1664, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 5, true){Unlocked = true }, //Oak Tree
            //    new SpawnElement(1381, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 15, true){Unlocked = true }, //Pumpkin
            //    new SpawnElement(1381, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 15, true){Unlocked = true }, //Pumpkin
                

            //    new SpawnElement(1581, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 50, true){Unlocked = true }, //rock
            //    new SpawnElement(3439, MapLayer.ForeGround, MapLayer.ForeGround, GenerationType.DirtCliff, 50){Unlocked = true }, //mineshaft

            //    ////CROPS - goes by GID, not by item id
            //    ///

                new SpawnElement(486,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Uncommon, 1, false, 25, 30){Unlocked = true, IsCrop = true  }, //Bloom Berry
                 new SpawnElement(492,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Uncommon, 1, false, 20, 30){Unlocked = true, IsCrop = true  }, //Yallon
                  new SpawnElement(692,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Uncommon, 1, false, 20, 30){Unlocked = true, IsCrop = true }, //Desmodus

                  new SpawnElement(1670,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Rare, 1, false, 25, 30){Unlocked = true, IsCrop = true  }, //Oak Seed
                 new SpawnElement(2970,SpawnMethod.Poisson, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Rare, 1, false, 20, 30){Unlocked = true, IsCrop = true  }, //Pine Seed
                  new SpawnElement(3670,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Rare, 1, false, 20, 30){Unlocked = true, IsCrop = true }, //Cedar Seed
                  new SpawnElement(4269,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Rare, 1, false, 20, 30){Unlocked = true, IsCrop = true }, //Fir Seed
            //    new SpawnElement(487, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 15){Unlocked = true }, //bloomberry

            //    new SpawnElement(287, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 15){Unlocked = true }, //bloodcorn

            //     new SpawnElement(687, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 5){Unlocked = true }, //Brine Bulb


            //      new SpawnElement(493, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 2){Unlocked = true }, //Yallon

            //       new SpawnElement(2272, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 50){Unlocked = true }, //ThunderBirch seed

            //       new SpawnElement(1671, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 50){Unlocked = true }, //Oak seed

            //    //DESERT
            new SpawnElement(663,  SpawnMethod.Poisson,MapLayer.ForeGround, MapLayer.BackGround,MapLayer.MidGround, GenerationType.Sand, Rarity.Abundant,Rarity.Abundant,3, false, 15, 20){Unlocked = true }, //palm tree
            new SpawnElement(1682, SpawnMethod.RandomScatter,MapLayer.ForeGround,MapLayer.BackGround, MapLayer.MidGround, GenerationType.Sand, Rarity.Abundant,Rarity.Abundant,2, false, 10, 2){Unlocked = true }, //Red Shell
            new SpawnElement(670,SpawnMethod.Poisson, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.MidGround, GenerationType.Sand, Rarity.Abundant,Rarity.Abundant, 2, false, 25, 30){Unlocked = true }, //Palm Tree Seed 
            new SpawnElement(1285, SpawnMethod.RandomScatter,MapLayer.ForeGround,MapLayer.BackGround, MapLayer.MidGround, GenerationType.Sand, Rarity.Abundant,Rarity.Abundant,2, false, 10, 2){Unlocked = true }, //Thorn Bush
            new SpawnElement(1573, SpawnMethod.RandomScatter,MapLayer.ForeGround,MapLayer.BackGround, MapLayer.MidGround, GenerationType.Sand, Rarity.Abundant,Rarity.Abundant,2, false, 10, 2){Unlocked = true }, //Big Cactus
            new SpawnElement(1574, SpawnMethod.RandomScatter,MapLayer.ForeGround, MapLayer.BackGround,MapLayer.MidGround, GenerationType.Sand, Rarity.Abundant,Rarity.Abundant,2, false, 10, 2){Unlocked = true }, //Small Cactus
            //    new SpawnElement(664, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Sand, 5, true){Unlocked = true }, //palm tree
            //    new SpawnElement(1286, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Sand, 5, true){Unlocked = true }, //thorn bush
            //    new SpawnElement(976, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Sand, 50, true){Unlocked = true }, //desert stone
            //    new SpawnElement(1683, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Sand, 25, true){Unlocked = true }, //Red Shell

            //    new SpawnElement(1573, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Sand, 5, true){Unlocked = true }, //Reeds

            //    //Wooden Boards
            //    new SpawnElement(2052, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.OakFloorTiling, 50, false){Unlocked = true }, //Wood Barrel

            //    //SWAMP
            //    new SpawnElement(4764, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.LandSwamp, 5, true){Unlocked = true }, //Swamp tree
            //    new SpawnElement(1681, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.LandSwamp, 5, true){Unlocked = true }, //Swamp vine


            //    //TileUtility.GenerateRandomlyDistributedTiles(2, 1286, GenerationType.Sand, 10, 0, this, true); //THORN
            ////TileUtility.GenerateRandomlyDistributedTiles(2, 664, GenerationType.Sand, 10, 0, this, true);
            };

            this.AllCamps = new List<Camp>()
            {
                new CalciteCamp(80),
                new ArcaneCamp(70)

            };




            this.UnderWorldSpawnElements = new List<SpawnElement>()
            {
                //    new SpawnElement(976, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.CaveDirt, 50, true){Unlocked = true }, //desert stone
                //new SpawnElement(1278, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.CaveDirt, 10){Unlocked = true }, //Steel Vein
                //new SpawnElement(1277, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.CaveDirt, 5){Unlocked = true }, //copper
                //new SpawnElement(1276, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.CaveDirt, 3){Unlocked = true }, //monzanite
                //new SpawnElement(1275, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.CaveDirt, 2){Unlocked = true }, //monzanite
                //new SpawnElement(1274, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.CaveDirt, 1){Unlocked = true }, //monzanite
                //new SpawnElement(1581, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.CaveDirt, 50, true){Unlocked = true }, //rock
                //    new SpawnElement(1580, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.CaveDirt, 50, true){Unlocked = true }, //Stick
                //     new SpawnElement(1682, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.CaveDirt, 25, true){Unlocked = true }, //Carrotte
            };

        }

        public bool CheckIfCampSpawns()
        {
            int num = Game1.Utility.RNumber(0, 100);
            if (num < CampSpawnRate)
            {
                return true;
            }
            return false;

        }

        public Camp GetCamp()
        {
            List<IWeightable> list = AllCamps.ToList<IWeightable>();
            return (Camp)Game1.Utility.WheelSelection.GetSelection(list);
        }

        public void UnlockSpawnElement(int gid)
        {
            for (int i = 0; i < this.OverWorldSpawnElements.Count; i++)
            {
                if (gid == this.OverWorldSpawnElements[i].GID - 1)
                {
                    this.OverWorldSpawnElements[i].Unlocked = true;
                }
            }
        }

        public static void SpawnOverWorld(SpawnHolder holder, IInformationContainer container, Random random)
        {
            foreach (SpawnElement element in holder.OverWorldSpawnElements)
            {
                if (element.Unlocked)
                {
                    if (element.SpawnMethod == SpawnMethod.Poisson)
                    {
                        PoissonSpawn(element, container, random);
                    }
                    else if (element.SpawnMethod == SpawnMethod.RandomScatter)
                    {
                        ScatterSpawn(element, container, random);
                    }

                }

            }

        }

        /// <summary>
        /// Good for things which don't necessarily clump, like sticks, or random rocks. Basically throwing darts for points.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="container"></param>
        /// <param name="random"></param>
        public static void ScatterSpawn(SpawnElement element, IInformationContainer container, Random random)
        {
            if (random.Next(0, 101) < (int)element.Rarity) //odds of first one even spawning at all
            {
                for (int i = 0; i < element.Limit; i++)
                {

                    int xSpawn = random.Next(0, container.AllTiles[(int)element.TileLayer].GetLength(0));
                    int ySpawn = random.Next(0, container.AllTiles[(int)element.TileLayer].GetLength(1));

                    if (container.PathGrid.Weight[xSpawn, ySpawn] == (int)GridStatus.Clear &&
                        container.AllTiles[(int)element.LayerToPlaceOn][xSpawn, ySpawn].GenerationType == element.GenerationType)
                    {

                        if((int)element.MapLayerToCheckIfEmpty != 0)
                        {
                            if(container.AllTiles[(int)element.MapLayerToCheckIfEmpty][xSpawn, ySpawn].GID == -1)
                            {
                                TileUtility.ReplaceTile((int)element.TileLayer, xSpawn, ySpawn, element.GID, container);
                                if (element.IsCrop)
                                {
                                    TileUtility.AddCropToTile(container.AllTiles[3][xSpawn, ySpawn], xSpawn, ySpawn, 3, container, true);
                                }
                            }
                        }
                        else
                        {
                            TileUtility.ReplaceTile((int)element.TileLayer, xSpawn, ySpawn, element.GID, container);
                            if (element.IsCrop)
                            {
                                TileUtility.AddCropToTile(container.AllTiles[3][xSpawn, ySpawn], xSpawn, ySpawn, 3, container, true);
                            }
                        }
                        
                    }


                    if (random.Next(0, 101) > (int)element.OddsOfAdditionalSpawn)
                    {
                        return;
                    }

                }
            }
        }

        /// <summary>
        /// A clumpy type of spawning. Good for objects like trees or rock formations
        /// </summary>
        /// <param name="element"></param>
        /// <param name="container"></param>
        /// <param name="random"></param>
        public static void PoissonSpawn(SpawnElement element, IInformationContainer container, Random random)
        {
            if (random.Next(0, 101) < (int)element.Rarity) //odds of first one even spawning at all
            {
                PoissonSampler sampler = new PoissonSampler(element.DistanceBetweenNeighbors, 6, container.PathGrid, element.Tries, element.OddsOfAdditionalSpawn);
                sampler.Generate(element.GID, container.AllTiles[(int)element.TileLayer],  (int)element.TileLayer, (int)element.LayerToPlaceOn, (int)element.MapLayerToCheckIfEmpty,container, element.GenerationType, random, element.IsCrop);
            }
        }

        public static void AddGrassTufts(IInformationContainer container, Tile tile, Tile zeroTile)
        {
            if (tile.GID == -1)
            {

                if (zeroTile.GenerationType == GenerationType.Grass)
                {
                    if (container.Random.Next(0, 10) < 2)
                    {
                        if ((container.Tufts.ContainsKey(tile.TileKey)))
                        {
                        }
                        else
                        {

                            int numberOfGrassTuftsToSpawn = container.Random.Next(1, 4);
                            List<GrassTuft> tuftList = new List<GrassTuft>();
                            for (int g = 0; g < numberOfGrassTuftsToSpawn; g++)
                            {
                                int grassType = container.Random.Next(1, 5);
                                GrassTuft grassTuft = new GrassTuft(container.GraphicsDevice, grassType, new Vector2(TileUtility.GetDestinationRectangle(tile).X
                                    + container.Random.Next(-8, 8), TileUtility.GetDestinationRectangle(tile).Y + container.Random.Next(-8, 8)));
                                grassTuft.TuftsIsPartOf = tuftList;
                                tuftList.Add(grassTuft);


                            }
                            container.Tufts.Add(tile.TileKey, tuftList);

                        }
                    }
                }
            }
        }
    }
}
