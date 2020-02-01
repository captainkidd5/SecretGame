using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.SpawnStuff
{
    public class SpawnHolder
    {
        public List<SpawnElement> OverWorldSpawnElements { get; set; }
        public List<SpawnElement> UnderWorldSpawnElements { get; set; }


        public SpawnHolder()
        {
            this.OverWorldSpawnElements = new List<SpawnElement>()
            {
                
                new SpawnElement(979, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 50){Unlocked = true }, //Stone
                new SpawnElement(1581, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 50, true){Unlocked = true }, //rock
                new SpawnElement(1580, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Grass, 50, true){Unlocked = true }, //Stick
                new SpawnElement(1580, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 50, true){Unlocked = true }, //Stick
                new SpawnElement(979, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Grass, 150){Unlocked = true }, //Stone
                new SpawnElement(2264, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 20){Unlocked = true }, //tree
                new SpawnElement(2964, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 20){Unlocked = true }, //tree
                new SpawnElement(3664, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 20){Unlocked = true }, //tree
                new SpawnElement(2264, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 5){Unlocked = true }, //ThunderBirch
                new SpawnElement(1079, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 50){Unlocked = true }, //GrassTuft
                new SpawnElement(1079, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 50){Unlocked = true }, //GrassTuft
                new SpawnElement(1586, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 5){Unlocked = true }, //Clue Fruit
                new SpawnElement(3161, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Grass, 5, false, true){Unlocked = true }, //big stump
                new SpawnElement(1664, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 5, true){Unlocked = true }, //Oak Tree
                new SpawnElement(1381, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 50, true){Unlocked = false }, //Pumpkin
                new SpawnElement(1278, MapLayer.ForeGround, MapLayer.MidGround, GenerationType.Stone, 50){Unlocked = true }, //Steel Vein
                new SpawnElement(1580, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 50, true){Unlocked = true }, //Stick
                new SpawnElement(1582, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 5, true){Unlocked = true }, //Red Mushroom
                new SpawnElement(1583, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 5, true){Unlocked = true }, //Blue Mushroom
                 new SpawnElement(1682, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 25, true){Unlocked = true }, //Carrotte
                new SpawnElement(1581, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 50, true){Unlocked = true }, //rock
                new SpawnElement(3439, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.DirtCliffBottom, 100,false, true){Unlocked = true }, //mineshaft

                ////CROPS - goes by GID, not by item id
                new SpawnElement(487, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 50, true){Unlocked = true }, //bloomberry

                new SpawnElement(287, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt, 50, true){Unlocked = true }, //bloodcorn

                //DESERT
                new SpawnElement(664, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Sand, 5, true){Unlocked = true }, //palm tree
                new SpawnElement(1286, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Sand, 5, true){Unlocked = true }, //thorn bush
                new SpawnElement(976, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Sand, 50, true){Unlocked = true }, //desert stone

                //SWAMP
                new SpawnElement(4764, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.LandSwamp, 5, true){Unlocked = true }, //Swamp tree
                new SpawnElement(1681, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.LandSwamp, 5, true){Unlocked = true }, //Swamp vine


                //TileUtility.GenerateRandomlyDistributedTiles(2, 1286, GenerationType.Sand, 10, 0, this, true); //THORN
            //TileUtility.GenerateRandomlyDistributedTiles(2, 664, GenerationType.Sand, 10, 0, this, true);
            };
            //TileUtility.GenerateRandomlyDistributedTiles(2, 1164, GenerationType.Grass, 2, 1, this); //WILLOW
            ////TileUtility.GenerateRandomlyDistributedTiles(2, 1002, GenerationType.Stone, 5, 1, this); //FISSURE
            //TileUtility.GenerateRandomlyDistributedTiles(3, 1476, GenerationType.Grass, 6, 0, this); //FallenOak
            //TileUtility.GenerateRandomlyDistributedTiles(3, 1278, GenerationType.Stone, 5, 1, this); //Steel Vein
            //TileUtility.GenerateRandomlyDistributedTiles(3, 1277, GenerationType.Stone, 5, 1, this); //Steel Vein
            //TileUtility.GenerateRandomlyDistributedTiles(3, 1276, GenerationType.Stone, 5, 1, this); //Steel Vein
            //TileUtility.GenerateRandomlyDistributedTiles(3, 1275, GenerationType.Stone, 5, 1, this); //Steel Vein
            //TileUtility.GenerateRandomlyDistributedTiles(3, 1274, GenerationType.Stone, 5, 1, this); //Steel Vein
            //TileUtility.GenerateRandomlyDistributedTiles(3, 1278, GenerationType.Stone, 5, 1, this); //Steel Vein
            //TileUtility.GenerateRandomlyDistributedTiles(2, 1581, GenerationType.Dirt, 15, 0, this, true); //ROCK
            //TileUtility.GenerateRandomlyDistributedTiles(2, 1581, GenerationType.Dirt, 15, 0, this, true); //ROCK
            //TileUtility.GenerateRandomlyDistributedTiles(2, 1580, GenerationType.Dirt, 15, 0, this, true); //stick

            //TileUtility.GenerateRandomlyDistributedTiles(3, 293, GenerationType.Dirt, 100, 0, this, true); //ZodFern

            ////SANDRUINS
            //TileUtility.GenerateRandomlyDistributedTiles(3, 1853, GenerationType.SandRuin, 5, 1, this); //Chest
            //TileUtility.GenerateRandomlyDistributedTiles(3, 2548, GenerationType.SandRuin, 5, 1, this); //ancient pillar (tall)
            //TileUtility.GenerateRandomlyDistributedTiles(3, 2549, GenerationType.SandRuin, 5, 1, this); //ancient pillar (short)


            //TileUtility.GenerateRandomlyDistributedTiles(2, 1573, GenerationType.Sand, 10, 0, this, true); //Reeds

            //// TileUtility.GenerateTiles(1, 2964, Game1.Utility.GrassGeneratableTiles,, 5, 0, this); //PINE
            //TileUtility.GenerateRandomlyDistributedTiles(2, 1286, GenerationType.Sand, 10, 0, this, true); //THORN
            //TileUtility.GenerateRandomlyDistributedTiles(2, 664, GenerationType.Sand, 10, 0, this, true);
            //// TileUtility.GenerateTiles(1, 4615, "water", 5, 0, this);
            ////TileUtility.GenerateTiles(1, 4414, "water", 5, 0, this);
            //TileUtility.GenerateRandomlyDistributedTiles(3, 2964, GenerationType.Grass, 25, 1, this); //oak2
            //TileUtility.GenerateRandomlyDistributedTiles(3, 3664, GenerationType.Grass, 25, 1, this); //oak3
            //TileUtility.GenerateRandomlyDistributedTiles(3, 2964, GenerationType.Dirt, 25, 0, this, true); //oak2
            //TileUtility.GenerateRandomlyDistributedTiles(3, 3664, GenerationType.Dirt, 25, 0, this, true); //oak3

            this.UnderWorldSpawnElements = new List<SpawnElement>()
            {
            };

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
    }
}
