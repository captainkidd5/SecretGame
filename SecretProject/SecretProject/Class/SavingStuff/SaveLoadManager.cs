using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace SecretProject.Class.SavingStuff
{
    public class SaveLoadManager
    {
        public string OutputMessage;
        public int CurrentSave = 1;
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

        public bool CheckIfSaveEmpty(int iD)
        {
            if (File.Exists(AllSaves[iD - 1].Path))
            {


                if (new FileInfo(AllSaves[iD - 1].Path).Length != 0)
                {
                    return false;
                }
            }
                return true;
            
        }

        public void Save()
        {
            FileStream fileStream = File.OpenWrite(GetSaveFileFromID(this.CurrentSave).Path);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
            File.WriteAllText(GetSaveFileFromID(this.CurrentSave).Path, string.Empty);

            binaryWriter.Write(this.CurrentSave);
            GameSerializer.Save(binaryWriter, OutputMessage, 1);


            binaryWriter.Flush();
            binaryWriter.Close();
        }

        public void Load(GraphicsDevice graphics, SaveFile saveFile)
        {

            FileStream fileStream = File.OpenRead(saveFile.Path);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            this.CurrentSave = binaryReader.ReadInt32();
            GameSerializer.Load(binaryReader, 1);


            binaryReader.Close();


        }





        public void SetWorldItems(List<KeyValuePair<int, Vector2>> items)
        {

        }


    }
}














