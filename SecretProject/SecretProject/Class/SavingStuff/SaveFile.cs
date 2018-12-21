using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using SecretProject.Class.Playable;
using Microsoft.Xna.Framework;

namespace SecretProject.Class.SavingStuff
{
    public class SaveFile
    {
        public string fileName;

        public SaveData mySave;

        // SaveLoadManager manager;

        public void Save()
        {
            mySave = new SaveData();

            //  Stream stream = File.Open("gametest.dat", FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            using (TextWriter tw = new StreamWriter(@"C:\Users\SecretDingo\Desktop\SecretGame\SecretProject\SecretProject\GameSave.xml"))
            {
                serializer.Serialize(tw, mySave);
                tw.Close();
            }

            //stream.Close();
        }


        public void Load()
        {
            //mySave = null;

            XmlSerializer deSerializer = new XmlSerializer(typeof(SaveData));

            StreamReader reader = new StreamReader(@"C:\Users\SecretDingo\Desktop\SecretGame\SecretProject\SecretProject\GameSave.xml");
            
                mySave = (SaveData)deSerializer.Deserialize(reader);
                //mySave = save;
                reader.Close();
                Game1._iliad.Player.Position = mySave.Position;
            
        }
    }
}
        
        
                
                
            
            
            
            
        
        

        
    

