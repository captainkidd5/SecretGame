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

        public SaveFile MainMenuData { get; set; }


        public SaveLoadManager()
        {
            //mySave = new SaveData();
            AllSaves = new List<SaveFile>()
            {
                new SaveFile(1,@"Content/SaveFiles/GameSaves/BinarySave1.dat" ),
                new SaveFile(2,@"Content/SaveFiles/GameSaves/BinarySave3.dat" ),
                new SaveFile(3,@"Content/SaveFiles/GameSaves/BinarySave3.dat" ),
            };

            MainMenuData = new SaveFile(0, @"Content/SaveFiles/Settings/MainMenu.dat");

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

        public void Save(SaveFile saveFile, bool isGameSave = true)
        {
            if(File.Exists(saveFile.Path))
            {
                File.WriteAllText(saveFile.Path, string.Empty);
            }
            FileStream fileStream = File.OpenWrite(saveFile.Path);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
           

            binaryWriter.Write(this.CurrentSave);
            if(isGameSave)
            {
                GameSerializer.SaveGameFile(binaryWriter, OutputMessage, 1);
            }
            else
            {
                GameSerializer.SaveMainMenu(binaryWriter, OutputMessage, 1);
            }
            


            binaryWriter.Flush();
            binaryWriter.Close();
        }

        public void Load(GraphicsDevice graphics, SaveFile saveFile, bool isGameSave = true)
        {
            if (File.Exists(saveFile.Path))
            {


                FileStream fileStream = File.OpenRead(saveFile.Path);
                BinaryReader binaryReader = new BinaryReader(fileStream);
                this.CurrentSave = binaryReader.ReadInt32();
                if (isGameSave)
                {
                    GameSerializer.LoadGameFile(binaryReader, 1);
                }
                else
                {
                    GameSerializer.LoadMainMenu(binaryReader, 1);
                }



                binaryReader.Close();
            }

        }





        public void SetWorldItems(List<KeyValuePair<int, Vector2>> items)
        {

        }


    }
}














