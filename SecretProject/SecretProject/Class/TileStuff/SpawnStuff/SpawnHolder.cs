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
using XMLData.ItemStuff;

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
                new SpawnElement(1628,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Abundant, 2, false){Unlocked = true }, //Pine Tree 
                 new SpawnElement(1617,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Abundant, 2, false){Unlocked = true }, //Cedar Tree
                  new SpawnElement(1535,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Abundant, 2, false){Unlocked = true }, //Fir Tree 
                new SpawnElement(140,SpawnMethod.Poisson, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Abundant,1, false){Unlocked = true }, //Stone
                new SpawnElement(34,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon,1, false){Unlocked = true }, //Grass Tuft
                new SpawnElement(3802,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon,1, false){Unlocked = true }, //Grass Tuft skinny
                new SpawnElement(240,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Abundant,1, false){Unlocked = true }, //grassy Stone
                new SpawnElement(34,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon,1, false){Unlocked = true }, //Grass Tuft
                new SpawnElement(3903,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.Abundant,1, false){Unlocked = true }, //Curly fern
                new SpawnElement(3704,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon,1, false){Unlocked = true }, //straight fern
                new SpawnElement(439,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon,1, false){Unlocked = true }, //Stick
                new SpawnElement(440,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon,1, false){Unlocked = true }, //rock


                new SpawnElement(33,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Common,Rarity.VeryCommon,7, false){Unlocked = true }, //Steel
                new SpawnElement(233,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Uncommon,Rarity.Uncommon,7, false){Unlocked = true }, //Large Steel

                new SpawnElement(25,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Common,Rarity.Common,7, false){Unlocked = true }, //Coal
                new SpawnElement(225,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Uncommon,Rarity.Uncommon,7, false){Unlocked = true }, //Large Coal

                 new SpawnElement(32,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.MidGround, GenerationType.Stone, Rarity.Uncommon,Rarity.VeryCommon,7, false){Unlocked = true }, //copper small
                  new SpawnElement(31,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.MidGround, GenerationType.Stone, Rarity.Uncommon,Rarity.VeryCommon,7, false){Unlocked = true }, //monzanite small
                   new SpawnElement(32,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.MidGround, GenerationType.Stone, Rarity.Uncommon,Rarity.VeryCommon,7, false){Unlocked = true }, //blunate small
                    new SpawnElement(33,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.MidGround, GenerationType.Stone, Rarity.Uncommon,Rarity.VeryCommon,7, false){Unlocked = true }, //santalite small

                    new SpawnElement(32,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.Rare,Rarity.VeryCommon,7, false){Unlocked = true }, //copper small 
                  new SpawnElement(33,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Rare,Rarity.VeryCommon,7, false){Unlocked = true }, //monzanite small
                   new SpawnElement(34,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.VeryRare,Rarity.VeryCommon,7, false){Unlocked = true }, //blunate small
                    new SpawnElement(35,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.ExtremelyRare,Rarity.VeryCommon,7, false){Unlocked = true }, //santalite small
                    new SpawnElement(28,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.ExtremelyRare,Rarity.VeryCommon,7, false){Unlocked = true }, //dark small
                    new SpawnElement(27,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.ExtremelyRare,Rarity.VeryCommon,7, false){Unlocked = true }, //gold small

                    new SpawnElement(232,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.Rare,Rarity.VeryCommon,7, false){Unlocked = true }, //copper Large 
                  new SpawnElement(231,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Rare,Rarity.VeryCommon,7, false){Unlocked = true }, //monzanite Large
                   new SpawnElement(230,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.VeryRare,Rarity.VeryCommon,7, false){Unlocked = true }, //blunate Large
                    new SpawnElement(228,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.ExtremelyRare,Rarity.VeryCommon,7, false){Unlocked = true }, //santalite Large
                    new SpawnElement(229,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.ExtremelyRare,Rarity.VeryCommon,7, false){Unlocked = true }, //dark Large
                    new SpawnElement(227,SpawnMethod.RandomScatter, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.ExtremelyRare,Rarity.VeryCommon,7, false){Unlocked = true }, //gold Large

                new SpawnElement(446,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.VeryCommon,Rarity.VeryCommon,1, false){Unlocked = true }, //carrotte
                 new SpawnElement(40,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.VeryCommon,Rarity.VeryCommon,1, false){Unlocked = true }, //pumpkin
                 new SpawnElement(441,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.VeryCommon,Rarity.VeryCommon,1, false){Unlocked = true }, //Red Mushroom\
                 new SpawnElement(442,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.VeryCommon,Rarity.VeryCommon,1, false){Unlocked = true }, //Blue Mushroom

                      new SpawnElement(731,SpawnMethod.RandomScatter, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.VeryCommon,Rarity.VeryCommon,1, false){Unlocked = true }, //big stump


            //    ////CROPS - goes by GID, not by item id
            //    ///

                new SpawnElement(769,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon, 1, false){Unlocked = true, IsCrop = true  }, //Bloom Berry
                 new SpawnElement(775,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon, 1, false){Unlocked = true, IsCrop = true  }, //Yallon
                  new SpawnElement(975,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon, 1, false){Unlocked = true, IsCrop = true }, //Desmodus

                  new SpawnElement(1035,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon, 1, false){Unlocked = true, IsCrop = true  }, //Oak Seed
                 new SpawnElement(1635,SpawnMethod.Poisson, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon, 1, false){Unlocked = true, IsCrop = true  }, //Pine Seed
                  new SpawnElement(1624,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon, 1, false){Unlocked = true, IsCrop = true }, //Cedar Seed
                  new SpawnElement(1541,SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround,MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,Rarity.VeryCommon, 1, false){Unlocked = true, IsCrop = true }, //Fir Seed


            //    //DESERT
            new SpawnElement(2411,  SpawnMethod.Poisson,MapLayer.ForeGround, MapLayer.BackGround,MapLayer.MidGround, GenerationType.Sand, Rarity.Abundant,Rarity.Abundant,3, false){Unlocked = true }, //palm tree
            new SpawnElement(1910, SpawnMethod.RandomScatter,MapLayer.ForeGround,MapLayer.BackGround, MapLayer.MidGround, GenerationType.Sand, Rarity.Abundant,Rarity.Abundant,2, false){Unlocked = true }, //Red Shell
            new SpawnElement(2418,SpawnMethod.Poisson, MapLayer.ForeGround,MapLayer.BackGround, MapLayer.MidGround, GenerationType.Sand, Rarity.Abundant,Rarity.Abundant, 2, false){Unlocked = true }, //Palm Tree Seed 
            new SpawnElement(1813, SpawnMethod.RandomScatter,MapLayer.ForeGround,MapLayer.BackGround, MapLayer.MidGround, GenerationType.Sand, Rarity.Abundant,Rarity.Abundant,2, false){Unlocked = true }, //Thorn Bush
            new SpawnElement(1811, SpawnMethod.RandomScatter,MapLayer.ForeGround,MapLayer.BackGround, MapLayer.MidGround, GenerationType.Sand, Rarity.Abundant,Rarity.Abundant,2, false){Unlocked = true }, //Big Cactus
            new SpawnElement(1812, SpawnMethod.RandomScatter,MapLayer.ForeGround, MapLayer.BackGround,MapLayer.MidGround, GenerationType.Sand, Rarity.Abundant,Rarity.Abundant,2, false){Unlocked = true }, //Small Cactus

           

            //LAKE
            new SpawnElement(317, SpawnMethod.Poisson, MapLayer.ForeGround, MapLayer.BackGround, MapLayer.MidGround, GenerationType.GrassLake, Rarity.Abundant, Rarity.Abundant, 3, false) { Unlocked = true }, //Lily pad
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

        private bool CheckIfCampSpawns()
        {
            int num = Game1.Utility.RNumber(0, 100);
            if (num < CampSpawnRate)
            {
                return true;
            }
            return false;

        }

        private Camp GetCamp()
        {
            List<IWeightable> list = AllCamps.ToList<IWeightable>();
            return (Camp)WheelSelection.GetSelection(list);
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
        private static void ScatterSpawn(SpawnElement element, IInformationContainer container, Random random)
        {
            if (random.Next(0, 101) < (int)element.Rarity) //odds of first one even spawning at all
            {
                while(random.Next(0, 100) < 99)
                {
                    int xSpawn = random.Next(0, container.AllTiles[(int)element.TileLayer].GetLength(0));
                    int ySpawn = random.Next(0, container.AllTiles[(int)element.TileLayer].GetLength(1));

                    if (container.PathGrid.Weight[xSpawn, ySpawn] == (int)GridStatus.Clear &&
                        container.AllTiles[(int)element.LayerToPlaceOn][xSpawn, ySpawn].GenerationType == element.GenerationType)
                    {

                        if ((int)element.MapLayerToCheckIfEmpty != (int)MapLayer.BackGround)
                        {
                            if (container.AllTiles[(int)element.MapLayerToCheckIfEmpty][xSpawn, ySpawn].GID == -1)
                            {
                                TileUtility.ReplaceTile((int)element.TileLayer, xSpawn, ySpawn, element.GID, container);
                                if (element.IsCrop)
                                {
                                    Crop crop = Game1.AllCrops.GetCropFromGID(element.GID);
                                    TileUtility.AddCropToTile(crop,container.AllTiles[3][xSpawn, ySpawn], xSpawn, ySpawn, (int)MapLayer.ForeGround, container, true);
                                }
                            }
                        }
                        else
                        {
                            TileUtility.ReplaceTile((int)element.TileLayer, xSpawn, ySpawn, element.GID, container);
                            if (element.IsCrop)
                            {
                                Crop crop = Game1.AllCrops.GetCropFromGID(element.GID);
                                TileUtility.AddCropToTile(crop,container.AllTiles[3][xSpawn, ySpawn], xSpawn, ySpawn, (int)MapLayer.ForeGround, container, true);
                            }
                        }

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
           // if (random.Next(0, 101) < (int)element.Rarity) //odds of first one even spawning at all
           // {
                PoissonSampler sampler = new PoissonSampler(element.DistanceBetweenNeighbors, element.DistanceBetweenNeighbors, container.PathGrid, (int)element.OddsOfAdditionalSpawn, element.Rarity);
                sampler.Generate(element.GID, container.AllTiles[(int)element.TileLayer],  (int)element.TileLayer, (int)element.LayerToPlaceOn, (int)element.MapLayerToCheckIfEmpty,container, element.GenerationType, random, element.IsCrop);
            //}
        }

        public static void AddGrassTufts(IInformationContainer container, Tile tile, Tile zeroTile)
        {
            if (tile.GID == -1)
            {

                if (zeroTile.GenerationType == GenerationType.Grass)
                {
                    if (Game1.Utility.RGenerator.Next(0, 10) < 2)
                    {
                        if ((container.Tufts.ContainsKey(tile.TileKey)))
                        {
                        }
                        else
                        {

                            int numberOfGrassTuftsToSpawn = Game1.Utility.RGenerator.Next(1, 4);
                            List<GrassTuft> tuftList = new List<GrassTuft>();
                            for (int g = 0; g < numberOfGrassTuftsToSpawn; g++)
                            {
                                int grassType = Game1.Utility.RGenerator.Next(1, 5);
                                GrassTuft grassTuft = new GrassTuft(container.GraphicsDevice, grassType, new Vector2(TileUtility.GetDestinationRectangle(tile).X
                                    + Game1.Utility.RGenerator.Next(-8, 8), TileUtility.GetDestinationRectangle(tile).Y + Game1.Utility.RGenerator.Next(-8, 8)));
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
