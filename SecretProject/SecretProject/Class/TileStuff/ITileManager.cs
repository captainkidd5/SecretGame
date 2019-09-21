using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
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
        int tilesetTilesWide { get; set; }
         int tilesetTilesHigh { get; set; }
         int mapWidth { get; set; }
         int mapHeight { get; set; }
         int TileWidth { get; set; }
         int TileHeight { get; set; }
        Dictionary<string, List<GrassTuft>> AllTufts { get; set; }
         Dictionary<string, ObjectBody> CurrentObjects { get; set; }
        Dictionary<string, EditableAnimationFrameHolder> AnimationFrames { get; set; }
        Dictionary<string, int> TileHitPoints { get; set; }
        GraphicsDevice GraphicsDevice { get; set; }
        ContentManager Content { get; set; }
        List<float> AllDepths { get; set; }

        void UpdateCropTile(Crop crop, ILocation stage);
        void LoadInitialTileObjects(ILocation location);
        void Update(GameTime gameTime, MouseManager mouse);
        void DrawTiles(SpriteBatch spriteBatch);
        

    }
}
