using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff.SpawnStuff
{
    public class SpawnHolder
    {
        public List<OverworldSpawnElement> OverWorldSpawnElements { get; set; }

        public SpawnHolder()
        {
            this.OverWorldSpawnElements = new List<OverworldSpawnElement>()
            {
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass), //tree

                new OverworldSpawnElement(2264, MapLayer.ForeGround, MapLayer.BackGround, GenerationType.Dirt),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),
                new OverworldSpawnElement(979, MapLayer.ForeGround, MapLayer.Buildings, GenerationType.Grass),

            };

            //TileUtility.GenerateRandomlyDistributedTiles(3, 979, GenerationType.Dirt, 50, 0, this, true); //tree
            //TileUtility.GenerateRandomlyDistributedTiles(3, 2264, GenerationType.Dirt, 5, 0, this, true); //THUNDERBIRCH
            //TileUtility.GenerateRandomlyDistributedTiles(2, 1079, GenerationType.Dirt, 50, 0, this, true); //GRASSTUFT
            //TileUtility.GenerateRandomlyDistributedTiles(2, 1079, GenerationType.Grass, 50, 1, this); //GRASSTUFT
            //TileUtility.GenerateRandomlyDistributedTiles(2, 1586, GenerationType.Dirt, 5, 0, this, true); //CLUEFRUIT
            //TileUtility.GenerateRandomlyDistributedTiles(3, 1664, GenerationType.Dirt, 5, 0, this, true); //OAKTREE
            //TileUtility.GenerateRandomlyDistributedTiles(2, 1294, GenerationType.Dirt, 5, 0, this, true); //SPROUTERA
            //TileUtility.GenerateRandomlyDistributedTiles(2, 1381, GenerationType.Dirt, 10, 0, this, true); //pumpkin
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
            //TileUtility.GenerateRandomlyDistributedTiles(2, 1580, GenerationType.Dirt, 15, 0, this, true); //stick
            //TileUtility.GenerateRandomlyDistributedTiles(2, 1582, GenerationType.Dirt, 5, 0, this, true); //RED MUSHROOM
            //TileUtility.GenerateRandomlyDistributedTiles(2, 1583, GenerationType.Dirt, 5, 0, this, true); //BLUE MUSHROOM

            //TileUtility.GenerateRandomlyDistributedTiles(3, 293, GenerationType.Dirt, 100, 0, this, true); //ZodFern

            ////SANDRUINS
            //TileUtility.GenerateRandomlyDistributedTiles(3, 1853, GenerationType.SandRuin, 5, 1, this); //Chest
            //TileUtility.GenerateRandomlyDistributedTiles(3, 2548, GenerationType.SandRuin, 5, 1, this); //ancient pillar (tall)
            //TileUtility.GenerateRandomlyDistributedTiles(3, 2549, GenerationType.SandRuin, 5, 1, this); //ancient pillar (short)

            ////CLIFFWALL
            //TileUtility.GenerateRandomlyDistributedTiles(3, 3439, GenerationType.DirtCliffBottom, 100, 2, this, false, true, 1); //Mine Shaft

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
        }
    }
}
