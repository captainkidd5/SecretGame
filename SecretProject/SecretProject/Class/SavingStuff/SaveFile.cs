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

        public void Save()
        {
            mySave = new SaveData();
            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            using (TextWriter tw = new StreamWriter(@"C:\Users\armad\Desktop\SecretGame\SecretProject\SecretProject\GameSave.xml"))
            {
                serializer.Serialize(tw, mySave);
                tw.Close();
            }
        }

        public void Load()
        {
            XmlSerializer deSerializer = new XmlSerializer(typeof(SaveData));

            StreamReader reader = new StreamReader(@"C:\Users\armad\Desktop\SecretGame\SecretProject\SecretProject\GameSave.xml");
            
                mySave = (SaveData)deSerializer.Deserialize(reader);
                //mySave = save;
                reader.Close();
                Game1.Iliad.Player.Position = mySave.Position;
            
        }
    }
}
        
        
                
                
            
            
            
            
        
        

        
    

