using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.ObjectFolder;
using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;

namespace SecretProject.Class.SavingStuff
{
    public class SaveData
    {
        //public Stage Iliad { get; set; } = Game1.Iliad;
        //Game1.Iliad;


        //THINGS TO SAVE:
        //PLAYER: 
        //Position, Name, Inventory, Health, (DONE)
        //PLAYERINVENTORY: 
        //currentInventory: List Inventory slots: Item ID for each item (DONE)
        //STAGE:
        //allTileManagers: List<Tile>: Dictionary Properties (Serialize with savemodule)
        //allObjects: Destination


        
        public float PlayerPosX { get; set; }
        public float PlayerPosY { get; set; }
        public string PlayerName { get; set; }
        public int PlayerHealth { get; set; }
        public Inventory PlayerInventory { get; set; }

        /// <summary>
        /// stage
        /// </summary>
        /// 

        public int HomeTileWidth { get; set; }
        public int HomeTileHeight { get; set; }
        public int HomeTilesetTilesWide { get; set; }
        public int HomeTilesetTilesHigh { get; set; }

        public TileManager HomeBackGroundTiles { get; set; }
        TileManager HomeBuildingsTiles;
        TileManager HomeMidGroundTiles;
        TileManager HomeForeGroundTiles;
        TileManager HomePlaceMentTiles;

        List<ObjectBody> HomeAllObjects;
        List<Sprite> HomeAllSprites;
        List<Item> HomeAllItems;
        List<ActionTimer> HomeAllActions;
        public bool HomeTilesLoaded { get; set; }

        public int HomeTilesetNumber { get; set; }




        //public Home HomeIsland { get; set; }

        //public List<Home> AllStages;
        //public List<TileManager> StageTiles;


        public SaveData()
        {
            //Player
            SavePlayer();
            SaveHome();
            



        }

        public void SavePlayer()
        {
            PlayerPosX = Game1.Player.Position.X;
            PlayerPosY = Game1.Player.Position.Y;
            PlayerName = Game1.Player.Name;
            PlayerHealth = Game1.Player.Health;

            PlayerInventory = Game1.Player.Inventory;
        }

        public void SaveHome()
        {

            HomeTileWidth = Game1.Wilderness.TileWidth;
            HomeTileHeight = Game1.Wilderness.TileHeight;
            HomeTilesetTilesWide = Game1.Wilderness.TilesetTilesWide;
            HomeTilesetTilesHigh = Game1.Wilderness.TilesetTilesHigh;



            HomeTilesetNumber = Game1.Wilderness.TileSetNumber;

            HomeAllObjects = Game1.Wilderness.AllObjects;
            HomeAllSprites = Game1.Wilderness.AllSprites;
            HomeAllItems = Game1.Wilderness.AllItems;
            HomeAllActions= Game1.Wilderness.AllActions;

            HomeTilesLoaded = Game1.Wilderness.TilesLoaded;


        }



    }
}
