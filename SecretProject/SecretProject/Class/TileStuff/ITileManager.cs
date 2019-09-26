using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.ObjectFolder;
using SecretProject.Class.PathFinding;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using XMLData.ItemStuff;

namespace SecretProject.Class.TileStuff
{
    public interface ITileManager
    {
        TmxMap MapName { get; set; }
        Texture2D TileSet { get; set; }
        List<Tile[,]> AllTiles { get; set; }
        AStarPathFinder PathGrid { get; set; }

         int TileWidth { get; set; }
         int TileHeight { get; set; }
         int TileSetNumber { get; set; }
        Dictionary<string, List<GrassTuft>> Tufts { get; set; }
         Dictionary<string, ObjectBody> CurrentObjects { get; set; }
        Dictionary<string, EditableAnimationFrameHolder> AnimationFrames { get; set; }
        Dictionary<string, int> TileHitPoints { get; set; }
        Dictionary<string, Chest> Chests { get; set; }
        List<LightSource> Lights { get; set; }
        Dictionary<string, ObjectBody> Objects { get; set; }
        Dictionary<string, Crop> Crops { get; set; }
        GraphicsDevice GraphicsDevice { get; set; }
        ContentManager Content { get; set; }
        List<float> AllDepths { get; set; }
        bool AbleToDrawTileSelector { get; set; }
        void LoadInitialTileObjects(ILocation location);
        void Update(GameTime gameTime, MouseManager mouse);
        void DrawTiles(SpriteBatch spriteBatch);
        void LoadGeneratableTileLists();

        //worldtilemanager specific
        void LoadInitialChunks();
        Chunk ChunkUnderPlayer { get; set; }



    }
}
