using Microsoft.Xna.Framework;
using SecretProject.Class.Playable;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;
using System.IO;

namespace SecretProject.Class.SavingStuff
{
    public static class GameSerializer
    {
        /// <summary>
        /// Write: Uses a binary writer to generate bytes representative of properties we pass into the binary writer. 
        /// Read: Uses the binary writer to read the bytes back in the same order they were written. Reads them in a specific way
        /// depending on the datatype we ask it to read. Must be a basic datatype as far as I know.
        /// </summary>
        /// <param name="outPutMessage"></param>
        /// <param name="thingToAppend"></param>
        /// 
        //Can use this to check what values we are passing in.

        public static float Version = 1f;
       

        public static void AppendOutputMessage(string outPutMessage, string thingToAppend)
        {
            outPutMessage = outPutMessage + " " + thingToAppend;
        }

        public static void SaveMainMenu(BinaryWriter writer, string OutputMessage, float version)
        {

            for (int i = 0; i < Game1.mainMenu.ChooseGameMenu.AllSaveSlots.Count; i++)
            {
                Game1.mainMenu.ChooseGameMenu.AllSaveSlots[i].SaveString(writer);
            }

        }

        public static void LoadMainMenu(BinaryReader reader, float version)
        {
            for(int i =0; i < Game1.mainMenu.ChooseGameMenu.AllSaveSlots.Count; i++)
            {
                Game1.mainMenu.ChooseGameMenu.AllSaveSlots[i].LoadString(reader);
            }

        }



        //order really really matters
        public static void SaveGameFile(BinaryWriter writer, string OutputMessage, float version)
        {
            
            Game1.Player.Save(writer);
            if(Game1.OverWorld.IsLoaded)
            {
                Game1.OverWorld.SaveLocation();
            }
            
            Game1.GlobalClock.Save(writer);
        }

        public static void LoadGameFile(BinaryReader reader, float version)
        {
            Game1.Player.Load(reader);
            Game1.GlobalClock.Load(reader);

        }







    }
}
