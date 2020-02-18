using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.UI.MainMenuStuff;
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
        public SaveSlot CurrentSave;
        




        public SaveLoadManager()
        {

            //MainMenuData = new SaveFile(0, @"Content/SaveFiles/Settings/MainMenu.dat");


        }






        public void Save(SaveSlot saveSlot)
        {


                File.WriteAllText(saveSlot.SavePath, string.Empty);
            
            FileStream fileStream = File.OpenWrite(saveSlot.SavePath);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
           

                GameSerializer.SaveGameFile(binaryWriter, OutputMessage, 1, saveSlot);
            

            


            binaryWriter.Flush();
            binaryWriter.Close();
        }

        public void Load(GraphicsDevice graphics, SaveSlot saveSlot)
        {



                FileStream fileStream = File.OpenRead(System.IO.Directory.GetFiles(System.IO.Directory.GetDirectories(@"Content/SaveFiles/GameSaves")[saveSlot.ID])[0]);
                BinaryReader binaryReader = new BinaryReader(fileStream);


                    GameSerializer.LoadGameFile(binaryReader, 1, saveSlot);

            CurrentSave = saveSlot;

                binaryReader.Close();
            

        }



    }
}














