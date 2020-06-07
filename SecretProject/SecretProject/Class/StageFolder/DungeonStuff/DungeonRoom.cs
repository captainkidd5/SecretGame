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
        public TileManager TileManager { get; private set; }

        public DungeonRoom(string name, LocationType locationType, StageType stageType, GraphicsDevice graphics, ContentManager content, int tileSetNumber, Texture2D tileSet, string tmxMapPath, int dialogueToRetrieve, int backDropNumber)
        {
            //this.TileManager = new TileManager(tileSet, new TmxMap(this.TmxMapPath), )
        }
    }
}
