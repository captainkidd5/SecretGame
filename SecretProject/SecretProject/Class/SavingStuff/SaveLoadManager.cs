using System;
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

        public SaveLoadManager()
        {
            mySave = new SaveData();
        }

        public void Save()
        {
            mySave = new SaveData();
            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            using (FileStream tw = new FileStream(@"Content/SaveFiles/GameTestSave3.xml", FileMode.Create))
            {
                serializer.Serialize(tw, mySave);
                tw.Close();
            }
        }

        public void Load()
        {
            //Load XML file with all pre-made items into itemVault.
            //Game1.ItemVault.RawItems.Load(@"Content/StartUpData/itemData.xml");

            XmlSerializer deSerializer = new XmlSerializer(typeof(SaveData));

            StreamReader reader = new StreamReader(@"Content/SaveFiles/GameTestSave3.xml");
            
                mySave = (SaveData)deSerializer.Deserialize(reader);
            AssignLoad();
                reader.Close();

        }

        public void AssignLoad()
        {
            LoadPlayer();
        }

        public void LoadPlayer()
        {
            Game1.Player.Position = new Vector2(mySave.PlayerPosX, mySave.PlayerPosY);
            Game1.Player.Health = mySave.PlayerHealth;
            Game1.Player.Name = mySave.PlayerName;
        }



        public void SetWorldItems(List<KeyValuePair<int, Vector2>> items)
        {

        }

    //    public WorldItem CheckItemDataBase(int id)
    //    {
    //        switch(id)
    //        {
    //            //case 1: return new WorldItem()
    //            //    break:
    //        }
    //    }
    }
}
        
        
                
                
            
            
            
            
        
        

        
    

