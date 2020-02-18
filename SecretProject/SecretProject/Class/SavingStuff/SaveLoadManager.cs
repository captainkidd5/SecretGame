using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace SecretProject.Class.SavingStuff
{
    public enum SaveType
    {
        GameSave = 1,
        MenuSave = 2
    }
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
                //new SaveFile(1,@"Content/SaveFiles/GameSaves/BinarySave1.dat" ),
                //new SaveFile(2,@"Content/SaveFiles/GameSaves/BinarySave2.dat" ),
                //new SaveFile(3,@"Content/SaveFiles/GameSaves/BinarySave3.dat" ),
            };

            MainMenuData = new SaveFile(0, @"Content/SaveFiles/Settings/MainMenu.dat");


        }

        public SaveFile GetSaveFileFromID(int ID)
        {
            return AllSaves[ID - 1];
        }

        public bool CheckIfSaveEmpty(int iD)
        {
            if(AllSaves.Count < iD)
            {
                return true;
            }
            if (File.Exists(AllSaves[iD - 1].Path))
            {


                if (new FileInfo(AllSaves[iD - 1].Path).Length != 0)
                {
                    return false;
                }
            }
                return true;
            
        }

        /// <summary>
        /// Menu data is consistent across saves and should never be deleted. 
        /// </summary>
        /// <param name="saveType">SaveType.GameSave will save the current save, MenuSave will save the data that appears when loading from main menu, as well as settings, for now</param>
        public void SaveGameState(SaveType saveType)
        {
            if(saveType == SaveType.GameSave)
            {
                Save(Game1.SaveLoadManager.GetSaveFileFromID(Game1.SaveLoadManager.CurrentSave),true);
            }
            else if(saveType == SaveType.MenuSave)
            {
                Save(MainMenuData,false);
            }
            else
            {
                throw new System.Exception("Invalid savetype");
            }
        }

        private void Save(SaveFile saveFile, bool isGameSave )
        {

            if (File.Exists(saveFile.Path))
            {
                File.WriteAllText(saveFile.Path, string.Empty);
            }
            FileStream fileStream = File.OpenWrite(saveFile.Path);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
           

            binaryWriter.Write(saveFile.);
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



    }
}














