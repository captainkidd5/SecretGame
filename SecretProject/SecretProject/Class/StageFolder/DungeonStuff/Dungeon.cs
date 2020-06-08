using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace SecretProject.Class.StageFolder.DungeonStuff
{
    public class Dungeon : TmxStageBase
    {
        public static int MaxDungeonRooms = 100;

        public DungeonRoom CurrentRoom { get; private set; }
        public DungeonRoom[,] Rooms { get; private set; }
        public ContentManager Content { get; private set; }

        public Dungeon(string name, LocationType locationType, StageType stageType, GraphicsDevice graphics, ContentManager content, Texture2D tileSet, TmxMap tmxMap, int dialogueToRetrieve, int backDropNumber) : base( name,  locationType,  stageType,  graphics,  content,  tileSet,  tmxMap,  dialogueToRetrieve,  backDropNumber)
        {

            this.Rooms = new DungeonRoom[MaxDungeonRooms, MaxDungeonRooms];
            this.Content = content;
           // this.CurrentRoom = this.Rooms[0];
        }

        public override void AssignPath(string startPath)
        {

            this.SavePath = startPath + "/" + Game1.Player.Name + "Dungeons/" + this.StageName;
            if (File.Exists(this.SavePath))
            {

            }
            else
            {
                File.WriteAllText(this.SavePath, string.Empty);
            }


        }

        public virtual void SwitchRooms(int x, int y)
        {
            DungeonRoom room;
            string path = this.SavePath + x + y + ".dat";
            room = new DungeonRoom(this, x, y, this.Content);

            if (File.Exists(path))
            {
                
                room.Load(path);
            }
            else
            {
                room.Generate(path);
            }
            this.Rooms[x, y] = room;

        }

        public override void Save(BinaryWriter writer)
        {

            this.AllTiles.Save(writer);
            writer.Write(this.SavePath);
        }

        public override void Load(BinaryReader reader)
        {
            this.AllTiles.Load(reader);
            this.SavePath = reader.ReadString();
        }


    }
}
