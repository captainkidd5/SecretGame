using Microsoft.Xna.Framework;
using SecretProject.Class.Playable;
using SecretProject.Class.TileStuff;
using SecretProject.Class.UI.MainMenuStuff;
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

        /// <summary>
        /// Save main menu preferences such as sound, video, etc
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="OutputMessage"></param>
        /// <param name="version"></param>
        public static void SaveMainMenu(BinaryWriter writer, string OutputMessage, float version)
        {

            

        }

        /// <summary>
        /// Used to load preferences such as sound, video, etc
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="version"></param>
        public static void LoadMainMenu(BinaryReader reader, float version)
        {
          

        }



        //order really really matters
        public static void SaveGameFile(BinaryWriter writer, string OutputMessage, float version, SaveSlot saveSlot)
        {
            string saveNameString = Game1.Player.Name + "\n Year " + Game1.GlobalClock.Calendar.CurrentYear + ", " + Game1.GlobalClock.Calendar.CurrentMonth.ToString() + " " + Game1.GlobalClock.Calendar.CurrentDay.ToString();
            writer.Write(saveNameString); //only used to identify save name on main menu when choosing to load a game.
            writer.Write(saveSlot.SavePath);
            writer.Write(saveSlot.ChunkPath);
            writer.Write(saveSlot.UnChunkPath);

            Game1.Player.Save(writer);
            if(Game1.OverWorld.IsLoaded)
            {
                Game1.OverWorld.SaveLocation();
            }
            if(Game1.UnderWorld.IsLoaded)
            {
                Game1.UnderWorld.SaveLocation();
            }
            
            Game1.GlobalClock.Save(writer);

            Game1.cam.Save(writer);
            Game1.WorldQuestHolder.Save(writer);
        }

        public static void LoadGameFile(BinaryReader reader, float version, SaveSlot saveSlot)
        {
            saveSlot.String = reader.ReadString();
            saveSlot.SavePath = reader.ReadString();
            saveSlot.ChunkPath = reader.ReadString();
            saveSlot.UnChunkPath = reader.ReadString();
            Game1.Player.Load(reader);
            Game1.GlobalClock.Load(reader);

            Game1.cam.Load(reader);
            Game1.WorldQuestHolder.Load(reader);
        }







    }
}
