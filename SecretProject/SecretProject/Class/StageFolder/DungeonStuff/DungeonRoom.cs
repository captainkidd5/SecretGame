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
    public class DungeonRoom
    {
        public int X { get; set; }
        public int Y { get; set; }
        public TileManager TileManager { get; private set; }

        public DungeonRoom(int x, int y, ILocation currentStage,string name, LocationType locationType, StageType stageType, GraphicsDevice graphics, ContentManager content, Texture2D tileSet, int tilesetNumber,TmxMap tmxMap, int dialogueToRetrieve, int backDropNumber)
        {
            this.X = x;
            this.Y = y;
            this.TileManager = new TileManager(tileSet, tmxMap, graphics, content, tilesetNumber, currentStage);
        }
    }
}
