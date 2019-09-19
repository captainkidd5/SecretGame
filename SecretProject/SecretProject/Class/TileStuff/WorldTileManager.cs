using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ObjectFolder;
using SecretProject.Class.PathFinding;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using TiledSharp;
using XMLData.ItemStuff;

namespace SecretProject.Class.TileStuff
{
    public class WorldTileManager : ITileManager
    {
        public List<Tile[,]> AllTiles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int mapWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int mapHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

       
        public AStarPathFinder PathGrid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int tilesetTilesWide { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int tilesetTilesHigh { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
 
        public Dictionary<string, List<GrassTuft>> AllTufts { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Dictionary<string, ObjectBody> CurrentObjects { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<Chunk> LoadedChunks { get; set; }
        public TmxMap MapName { get; set; }
        public Texture2D TileSet { get; set; }
        public int TileWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TileHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GraphicsDevice GraphicsDevice { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ContentManager Content { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<float> AllDepths { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public WorldTileManager(World world, Texture2D tileSet, List<TmxLayer> allLayers, TmxMap mapName, int numberOfLayers, int worldWidth, int worldHeight, GraphicsDevice graphicsDevice, ContentManager content, int tileSetNumber, List<float> allDepths)
        {
            this.MapName = mapName;
            this.TileSet = tileSet;

            TileWidth = 16;
            TileHeight = 16;

            tilesetTilesWide = tileSet.Width / TileWidth;
            tilesetTilesHigh = tileSet.Height / TileHeight;



            mapWidth = worldWidth;
            mapHeight = worldHeight;
            this.mapWidth = worldWidth;
            this.mapHeight = worldHeight;

            this.GraphicsDevice = graphicsDevice;
            this.Content = content;

            this.AllDepths = allDepths;
        }

        public void DrawTiles(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void LoadInitialTileObjects(ILocation location)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            throw new NotImplementedException();
        }

        public void UpdateCropTile(Crop crop, ILocation stage)
        {
            throw new NotImplementedException();
        }
    }
}
