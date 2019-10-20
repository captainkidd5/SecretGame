using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.PathFinding;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using XMLData.ItemStuff;

namespace SecretProject.Class.TileStuff
{
    public interface IInformationContainer
    {
         int Type { get; set; }
        GraphicsDevice GraphicsDevice { get; set; }
        TmxMap MapName { get; set; }
        int TileSetDimension { get; set; }
         int MapWidth { get; set; }
         int MapHeight { get; set; }
        int TileSetNumber { get; set; }
        List<Tile[,]> AllTiles { get; set; }
        Dictionary<string, List<GrassTuft>> Tufts { get; set; }
         Dictionary<string, ObjectBody> Objects { get; set; }
         Dictionary<string, EditableAnimationFrameHolder> AnimationFrames { get; set; }
         Dictionary<string, int> TileHitPoints { get; set; }
         Dictionary<string, Chest> Chests { get; set; }
         List<LightSource> Lights { get; set; }
        Dictionary<string, Crop> Crops { get; set; }
        Dictionary<float, string> ForeGroundOffSetDictionary { get; set; }

        AStarPathFinder PathGrid { get; set; }

        //specific to chunks
        int X { get; set; }
         int Y { get; set; }
        int ArrayI { get; set; }
        int ArrayJ { get; set; }
        Rectangle GetChunkRectangle();
        bool Owned { get; set; }
        WorldTileManager TileManager { get; set; }
    }
}
