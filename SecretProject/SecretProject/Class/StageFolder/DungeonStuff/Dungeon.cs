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

        public Dungeon(string name, LocationType locationType, StageType stageType, GraphicsDevice graphics, ContentManager content, Texture2D tileSet, TmxMap tmxMap, int dialogueToRetrieve, int backDropNumber) : base( name,  locationType,  stageType,  graphics,  content,  tileSet,  tmxMap,  dialogueToRetrieve,  backDropNumber)
        {

            this.Rooms = new DungeonRoom[MaxDungeonRooms, MaxDungeonRooms];

            this.CurrentRoom = this.Rooms[0];
        }

        public virtual void SwitchRooms(DungeonRoom newRoom)
        {
            this.CurrentRoom = newRoom;
            this.AllTiles.
        }

        public override void Save(BinaryWriter writer)
        {
            this.AllTiles.Save(writer); 
        }

        public override void Load(BinaryReader reader)
        {
            base.Load(reader);
        }
    }
}
