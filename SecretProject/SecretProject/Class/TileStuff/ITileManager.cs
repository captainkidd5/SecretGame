using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.PathFinding;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.ItemStuff;

namespace SecretProject.Class.TileStuff
{
    public interface ITileManager
    {
        List<Tile[,]> AllTiles { get; set; }
        AStarPathFinder PathGrid { get; set; }
        int tilesetTilesWide { get; set; }
         int tilesetTilesHigh { get; set; }
         int mapWidth { get; set; }
         int mapHeight { get; set; }
         Dictionary<float, List<GrassTuft>> AllTufts { get; set; }

        void UpdateCropTile(Crop crop, ILocation stage);
        void LoadInitialTileObjects(ILocation location);
        void Update(GameTime gameTime, MouseManager mouse);
        void DrawTiles(SpriteBatch spriteBatch);

    }
}
