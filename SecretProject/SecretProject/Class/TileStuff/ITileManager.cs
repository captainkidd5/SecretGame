using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;

using SecretProject.Class.PathFinding;
using SecretProject.Class.PathFinding.PathFinder;
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
        ObstacleGrid PathGrid { get; set; }

         int TileWidth { get; set; }
         int TileHeight { get; set; }
         int TileSetNumber { get; set; }
        Dictionary<string, List<GrassTuft>> Tufts { get; set; }
        Dictionary<string, List<ICollidable>> Objects { get; set; }
        Dictionary<string, EditableAnimationFrameHolder> AnimationFrames { get; set; }
        Dictionary<string, int> TileHitPoints { get; set; }
        Dictionary<string, IStorableItem> StoreableItems { get; set; }
        List<LightSource> Lights { get; set; }
        Dictionary<string, Crop> Crops { get; set; }
        GraphicsDevice GraphicsDevice { get; set; }
        ContentManager Content { get; set; }
        List<float> AllDepths { get; set; }
        bool AbleToDrawTileSelector { get; set; }

        GridItem GridItem { get; set; }
        void LoadInitialTileObjects(ILocation location);
        void Update(GameTime gameTime, MouseManager mouse);
        void DrawTiles(SpriteBatch spriteBatch);
        void LoadGeneratableTileLists();

        //worldtilemanager specific
        void LoadInitialChunks();
        Chunk ChunkUnderPlayer { get; set; }
        Chunk GetChunkFromPosition(Vector2 entityPosition);
        void UpdateCropTile();
        void HandleClockChange(object sender, EventArgs eventArgs);
        Chunk[,] ActiveChunks { get; set; }
    }
}
