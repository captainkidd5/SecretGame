using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace SecretProject.Class.SavingStuff
{
    public class SaveLoadManager
    {
        public string fileName;
        public string OutputMessage;

        public List<SaveFile> AllSaves { get; set; }

        public SaveLoadManager()
        {
            //mySave = new SaveData();
            AllSaves = new List<SaveFile>()
            {
                new SaveFile(1,@"Content/SaveFiles/BinarySave1.dat" ),
                new SaveFile(2,@"Content/SaveFiles/BinarySave3.dat" ),
                new SaveFile(3,@"Content/SaveFiles/BinarySave3.dat" ),
            };

        }

        public SaveFile GetSaveFileFromID(int ID)
        {
            return AllSaves[ID - 1];
        }

        public void Save(SaveFile saveFile)
        {


            //ORDER REALLY MATTERS
            FileStream fileStream = File.OpenWrite(saveFile.Path);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);

            GameSerializer.WritePlayer(Game1.Player, binaryWriter, OutputMessage, 1);
            //GameSerializer.WriteWorld(Game1.World, binaryWriter, 1);
            GameSerializer.WriteClock(Game1.GlobalClock, binaryWriter, 1);

            binaryWriter.Flush();
            binaryWriter.Close();
        }

        public void Load(GraphicsDevice graphics, SaveFile saveFile)
        {

            FileStream fileStream = File.OpenRead(saveFile.Path);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            GameSerializer.ReadPlayer(Game1.Player, binaryReader, 1);
            // GameSerializer.ReadWorld(Game1.World, graphics, binaryReader, 1);
            GameSerializer.ReadClock(Game1.GlobalClock, binaryReader, 1);

            binaryReader.Close();


        }





        public void SetWorldItems(List<KeyValuePair<int, Vector2>> items)
        {

        }


    }
}














