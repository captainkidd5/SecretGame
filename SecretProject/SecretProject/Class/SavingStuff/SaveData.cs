using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.Playable;
using SecretProject.Class.StageFolder;

namespace SecretProject.Class.SavingStuff
{
    [Serializable]
    public class SaveData
    {
        //public Stage Iliad { get; set; } = Game1.Iliad;
        //Game1.Iliad;


        //THINGS TO SAVE:
        //PLAYER: 
        //Position, Name, Inventory, Health, 
        //PLAYERINVENTORY: 
        //currentInventory: List Inventory slots: Item ID for each item
        //STAGE:
        //allTileManagers: List<Tile>: Dictionary Properties (Serialize with savemodule)
        //allObjects: Destination


        
        public float PlayerPosX { get; set; }

        
        public float PlayerPosY { get; set; }

        public string PlayerName { get; set; }

        public int PlayerHealth { get; set; }

        public Inventory PlayerInventory { get; set; }


        public SaveData()
        {
            //Player
            SavePlayer();



        }

        public void SavePlayer()
        {
            PlayerPosX = Game1.Player.Position.X;
            PlayerPosY = Game1.Player.Position.Y;
            PlayerName = Game1.Player.Name;
            PlayerHealth = Game1.Player.Health;

            PlayerInventory = Game1.Player.Inventory;
        }



    }
}
