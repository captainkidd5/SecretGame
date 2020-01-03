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

        public SaveLoadManager()
        {
            //mySave = new SaveData();

        }

        public void Save()
        {


            //ORDER REALLY MATTERS
            FileStream fileStream = File.OpenWrite(@"Content/SaveFiles/BinarySave.dat");
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);

            GameSerializer.WritePlayer(Game1.Player, binaryWriter, OutputMessage, 1);
            //GameSerializer.WriteWorld(Game1.World, binaryWriter, 1);
            GameSerializer.WriteClock(Game1.GlobalClock, binaryWriter, 1);

            binaryWriter.Flush();
            binaryWriter.Close();
        }

        public void Load(GraphicsDevice graphics)
        {

            FileStream fileStream = File.OpenRead(@"Content/SaveFiles/BinarySave.dat");
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














