﻿using SecretProject.Class.PathFinding;
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
        Extremely = 1,
        Very = 5,
        Rare = 15,
        Uncommon = 30,
        Common = 50,
        Abundant = 70
       

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
                new SpawnElement(2963, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant, 3, false, 15, 20){Unlocked = true }, //tree
                new SpawnElement(978, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,1, false, 3, 2){Unlocked = true }, //Stone
                new SpawnElement(1580, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,1, false, 3, 4){Unlocked = true }, //Stick
                new SpawnElement(1579, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,1, false, 3, 3){Unlocked = true }, //Stone
                new SpawnElement(1277, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, Rarity.Abundant,7, false, 3, 1){Unlocked = true }, //Steel
               // new SpawnElement(3663, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt,  Rarity.Abundant,3, false, 15, 20){Unlocked = true }, //tree
               // new SpawnElement(878, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 25){Unlocked = true }, //Stone2
                 //new SpawnElement(1277, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 15){Unlocked = true }, //Steel Vein
                 
            //    new SpawnElement(1580, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 50, true){Unlocked = true }, //rock
            //    new SpawnElement(1580, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 50, false){Unlocked = true }, //rock
            //    new SpawnElement(1579, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 50, false){Unlocked = true }, //Stick
            //    new SpawnElement(1580, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 50, true){Unlocked = true }, //Stick
            //    new SpawnElement(979, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Grass, 150){Unlocked = true }, //Stone
            //    new SpawnElement(2264, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 20){Unlocked = true }, //tree
            //    new SpawnElement(2964, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 20){Unlocked = true }, //tree
            //    new SpawnElement(3664, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 20){Unlocked = true }, //tree
            //     new SpawnElement(4264, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 20){Unlocked = true }, //Pine tree
            //    new SpawnElement(2264, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 5){Unlocked = true }, //ThunderBirch
            //    new SpawnElement(1079, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 50){Unlocked = true }, //GrassTuft
            //    new SpawnElement(1079, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 50){Unlocked = true }, //GrassTuft
            //    new SpawnElement(1586, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 5){Unlocked = true }, //Clue Fruit
            //    new SpawnElement(3161, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 5, false, true){Unlocked = true }, //big stump
            //    new SpawnElement(1664, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 5, true){Unlocked = true }, //Oak Tree
            //    new SpawnElement(1381, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 15, true){Unlocked = true }, //Pumpkin
            //    new SpawnElement(1381, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 15, true){Unlocked = true }, //Pumpkin
                
            //    new SpawnElement(1580, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 50, true){Unlocked = true }, //Stick
            //    new SpawnElement(1582, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 5, true){Unlocked = true }, //Red Mushroom
            //    new SpawnElement(1583, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 5, true){Unlocked = true }, //Blue Mushroom
            //     new SpawnElement(1682, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 25, true){Unlocked = true }, //Carrotte
            //     new SpawnElement(1682, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 25){Unlocked = true }, //Carrotte
            //    new SpawnElement(1581, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 50, true){Unlocked = true }, //rock
            //    new SpawnElement(3439, MapLayer.ForeGround, MapLayer.ForeGround, GenerationType.DirtCliff, 50){Unlocked = true }, //mineshaft


            //    //STONE BIOME
            //    new SpawnElement(1278, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Stone, 50){Unlocked = true }, //Steel Vein
            //    new SpawnElement(1277, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Stone, 50){Unlocked = true }, //Copper Vein

            //    new SpawnElement(1276, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Stone, 25){Unlocked = true }, // Monzanite Vein

            //    new SpawnElement(1275, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Stone, 25){Unlocked = true }, // Bluenate Vein
            //     new SpawnElement(1274, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Stone, 25){Unlocked = true }, // Red Vein
            //    ////CROPS - goes by GID, not by item id
            //    ///
            //    new SpawnElement(487, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 15){Unlocked = true }, //bloomberry

            //    new SpawnElement(287, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 15){Unlocked = true }, //bloodcorn

            //     new SpawnElement(687, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 5){Unlocked = true }, //Brine Bulb


            //      new SpawnElement(493, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 2){Unlocked = true }, //Yallon

            //       new SpawnElement(2272, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 50){Unlocked = true }, //ThunderBirch seed

            //       new SpawnElement(1671, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 50){Unlocked = true }, //Oak seed

            //    //DESERT
            new SpawnElement(663, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Sand, Rarity.Abundant,3, false, 15, 20){Unlocked = true }, //palm tree
            new SpawnElement(1682, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Sand, Rarity.Abundant,4, false, 10, 2){Unlocked = true }, //Red Shell
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
            if(num < CampSpawnRate)
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
            for(int i =0;  i < this.OverWorldSpawnElements.Count; i++)
            {
                if(gid == this.OverWorldSpawnElements[i].GID - 1)
                {
                    this.OverWorldSpawnElements[i].Unlocked = true;
                }
            }
        }

        public static void SpawnOverWorld(SpawnHolder holder, IInformationContainer container, Random random)
        {
            foreach (SpawnElement element in holder.OverWorldSpawnElements)
            {
                if(random.Next(0,101) < (int)element.Rarity)
                {
                    PoissonSampler sampler = new PoissonSampler(element.DistanceBetweenNeighbors, 6, container.PathGrid, element.Tries);
                    sampler.Generate(element.GID, container.AllTiles[(int)element.MapLayerToPlace], (int)element.MapLayerToPlace, container, element.GenerationType);
                }
                
            }
           
        }
    }
}
