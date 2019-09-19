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
        //temporarily unused
        public List<Tile[,]> AllTiles { get; set; }
        public int mapWidth { get; set; }
        public int mapHeight { get; set; }
        public Dictionary<string, List<GrassTuft>> AllTufts { get; set; }
        public Dictionary<string, ObjectBody> CurrentObjects { get; set; }
        public AStarPathFinder PathGrid { get; set; }


        public int tilesetTilesWide { get; set; }
        public int tilesetTilesHigh { get; set; }



        public Chunk[,] LoadedChunks { get; set; }
        public TmxMap MapName { get; set; }
        public Texture2D TileSet { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public ContentManager Content { get; set; }
        public List<float> AllDepths { get; set; }

        public int MaximumChunksLoaded { get; set; }


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

            LoadedChunks = new Chunk[3, 3];
            this.MaximumChunksLoaded = 3;

        }


        public List<int> GetActiveChunkCoord(float coord)
        {

            int currentChunk = (int)(coord * 16 / 32);
            return new List<int>() { currentChunk - 1, currentChunk, currentChunk + 1 };

        }
        public void Update(GameTime gameTime, MouseManager mouse)
        {
            //LoadedChunks[]
            List<int> xcoords = GetActiveChunkCoord(Game1.Player.position.X);
            List<int> ycoords = GetActiveChunkCoord(Game1.Player.position.Y);
            for(int x = 0; x < 3; x++)
            {
                
                for(int y = 0; y < 3; y++)
                {
                    LoadedChunks[x,y] = 
                }
            }


            int starti = (int)(Game1.cam.Pos.X / 16) - (int)(Game1.ScreenWidth / Game1.GetCurrentStage().Cam.Zoom / 2 / 16) - 1;

            int startj = (int)(Game1.cam.Pos.Y / 16) - (int)(Game1.ScreenHeight / Game1.GetCurrentStage().Cam.Zoom / 2 / 16) - 1;

            int endi = (int)(Game1.cam.Pos.X / 16) + (int)(Game1.ScreenWidth / Game1.GetCurrentStage().Cam.Zoom / 2 / 16) + 2;

            int endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.GetCurrentStage().Cam.Zoom / 2 / 16) + 2;
            for(int i =0; i < LoadedChunks.GetLength(0); i++)
            {
                for(int j = 0; j < LoadedChunks.GetLength(1); j++)
                {
                    for(int z = 0; z < 5; z++)
                    {
                        for(int x = 0; x < TileUtility.ChunkX; x++)
                        {
                            for(int y =0; y < TileUtility.ChunkY;y++)
                            {
                                //LoadedChunks[i, j].Tiles[x,y]
                            }
                        }
                    }
                    
                }
            }
        }
        public void DrawTiles(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void LoadInitialTileObjects(ILocation location)
        {
            throw new NotImplementedException();
        }



        public void UpdateCropTile(Crop crop, ILocation stage)
        {
            throw new NotImplementedException();
        }
    }
}
