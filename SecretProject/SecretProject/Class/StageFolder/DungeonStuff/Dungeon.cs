using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace SecretProject.Class.StageFolder.DungeonStuff
{
    public class Dungeon : TmxStageBase
    {

        public DungeonRoom CurrentRoom { get; private set; }
        public List<DungeonRoom> Rooms { get; private set; }

        public Dungeon(string name, LocationType locationType, StageType stageType, GraphicsDevice graphics, ContentManager content, Texture2D tileSet, TmxMap tmxMap, int dialogueToRetrieve, int backDropNumber) : base( name,  locationType,  stageType,  graphics,  content,  tileSet,  tmxMap,  dialogueToRetrieve,  backDropNumber)
        {

            this.Rooms = new List<DungeonRoom>();
            this.CurrentRoom = this.Rooms[0];
        }
    }
}
