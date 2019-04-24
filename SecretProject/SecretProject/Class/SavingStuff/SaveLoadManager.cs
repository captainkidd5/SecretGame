﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using SecretProject.Class.Playable;
using Microsoft.Xna.Framework;
using SecretProject.Class.ItemStuff;

namespace SecretProject.Class.SavingStuff
{
    public class SaveLoadManager
    {
        public string fileName;

        public SaveData mySave;

        public void Save()
        {
            mySave = new SaveData();
            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            using (TextWriter tw = new StreamWriter(@"Content/SaveFiles/GameTestSave.xml"))
            {
                serializer.Serialize(tw, mySave);
                tw.Close();
            }
        }

        public void Load()
        {
            XmlSerializer deSerializer = new XmlSerializer(typeof(SaveData));

            StreamReader reader = new StreamReader(@"Content/SaveFiles/GameTestSave.xml");
            
                mySave = (SaveData)deSerializer.Deserialize(reader);
                //mySave = save;
                reader.Close();
            #region Player
            Game1.Iliad.Player.Position = mySave.Position;
                Game1.Iliad.Player.Health = mySave.PlayerHealth;
            #endregion
            #region PlayerInventory
         //   Game1.Player.Inventory.currentInventory = mySave.PlayerInventory;

            #endregion
        }
        //yah
        public void SetWorldItems(List<KeyValuePair<int, Vector2>> items)
        {

        }

        public WorldItem CheckItemDataBase(int id)
        {
            switch(id)
            {
                //case 1: return new WorldItem()
                //    break:
            }
        }
    }
}
        
        
                
                
            
            
            
            
        
        

        
    
