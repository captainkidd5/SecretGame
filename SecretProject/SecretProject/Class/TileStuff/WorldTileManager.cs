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
       
        public AStarPathFinder PathGrid { get; set; }


        public int tilesetTilesWide { get; set; }
        public int tilesetTilesHigh { get; set; }



        public Chunk[,] ActiveChunks { get; set; }
        public TmxMap MapName { get; set; }
        public Texture2D TileSet { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public ContentManager Content { get; set; }
        public List<float> AllDepths { get; set; }

        public int MaximumChunksLoaded { get; set; }

        public Point ChunkPointUnderPlayerLastFrame { get; set; }
        public Point ChunkPointUnderPlayer { get; set; }
        public Chunk ChunkUnderPlayer { get; set; }

        public Dictionary<string, List<GrassTuft>> AllTufts { get; set; }
        public Dictionary<string, ObjectBody> CurrentObjects { get; set; }
        public Dictionary<string, EditableAnimationFrameHolder> AnimationFrames { get; set; }
        public Dictionary<string, int> TileHitPoints { get; set; }
        public int TileSetNumber { get; set; }
        public bool AbleToDrawTileSelector { get; set; }
        public List<int> DirtGeneratableTiles;
        public List<int> SandGeneratableTiles;
        public List<int> GrassGeneratableTiles;

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

            ActiveChunks = new Chunk[3, 3];

            this.MaximumChunksLoaded = 3;
            ChunkPointUnderPlayerLastFrame = new Point(1000, 1000);

            DirtGeneratableTiles = new List<int>();
            SandGeneratableTiles = new List<int>();
            GrassGeneratableTiles = new List<int>();
            AnimationFrames = new Dictionary<string, EditableAnimationFrameHolder>();
            AllTufts = new Dictionary<string, List<GrassTuft>>();
            TileHitPoints = new Dictionary<string, int>();
            CurrentObjects = new Dictionary<string, ObjectBody>();

            this.ChunkUnderPlayer = new Chunk(0, 0);

        }

        public void LoadInitialChunks()
        {

            ActiveChunks = GetActiveChunkCoord(Game1.Player.position);
            for (int i = 0; i < ActiveChunks.GetLength(0); i++)
            {
                for (int j = 0; j < ActiveChunks.GetLength(1); j++)
                {
                    if (!ActiveChunks[i, j].IsLoaded)
                    {
                        if (TileUtility.CheckIfChunkExistsInMemory(ActiveChunks[i, j].X, ActiveChunks[i, j].Y))
                        {
                            ActiveChunks[i, j].Load();
                        }
                        else
                        {
                            ActiveChunks[i, j].Generate(this);

                        }
                        ActiveChunks[i, j].Save();
                    }
                }
            }
        }


        public Chunk[,] GetActiveChunkCoord(Vector2 playerPos)
        {

            int currentChunkX = (int)(playerPos.X / 16 / TileUtility.ChunkX);
            int currentChunkY = (int)(playerPos.Y / 16 / TileUtility.ChunkY);
            return new Chunk[,]
            {
                { new Chunk(currentChunkX - 1, currentChunkY - 1), new Chunk (currentChunkX, currentChunkY - 1) , new Chunk(currentChunkX + 1, currentChunkY - 1) },
                { new Chunk(currentChunkX - 1, currentChunkY), new Chunk(currentChunkX , currentChunkY), new Chunk (currentChunkX +1, currentChunkY )},
                { new Chunk(currentChunkX - 1, currentChunkY + 1), new Chunk( currentChunkX , currentChunkY + 1), new Chunk( currentChunkX +1, currentChunkY + 1)},
            };

        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            ChunkPointUnderPlayer = new Point((int)(Game1.Player.Position.X / 16 / TileUtility.ChunkX), (int)(Game1.Player.Position.Y / 16 / TileUtility.ChunkY));
            if (ChunkPointUnderPlayerLastFrame != ChunkPointUnderPlayer)
            {
                LoadInitialChunks();
            }
            ChunkPointUnderPlayerLastFrame = ChunkPointUnderPlayer;


            for (int i = 0; i < ActiveChunks.GetLength(0); i++)
            {
                for (int j = 0; j < ActiveChunks.GetLength(1); j++)
                {
                    if (Game1.cam.CameraScreenRectangle.Intersects(ActiveChunks[i, j].GetChunkRectangle()))
                    {
                        if(Game1.Player.Rectangle.Intersects(ActiveChunks[i, j].GetChunkRectangle()))
                        {
                            this.ChunkUnderPlayer = ActiveChunks[i, j];
                        }
                        for (int z = 0; z <5; z++)
                        {
                            for (int x = 0; x < TileUtility.ChunkX; x++)
                            {
                                for (int y = 0; y < TileUtility.ChunkY; y++)
                                {
                                    //update
                                }
                            }
                        }
                    }
                }
            }
        }


        public void DrawTiles(SpriteBatch spriteBatch)
        {

            int starti = (int)(Game1.cam.Pos.X ) - (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2) - 1;

            int startj = (int)(Game1.cam.Pos.Y) - (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2 ) - 1;

            int endi = (int)(Game1.cam.Pos.X) + (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2 ) + 2;

            int endj = (int)(Game1.cam.Pos.Y) + (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2 ) + 2;

            Rectangle ScreenRectangle = new Rectangle(starti, startj, endi, endj);

            for (int i = 0; i < ActiveChunks.GetLength(0); i++)
            {
                for (int j = 0; j < ActiveChunks.GetLength(1); j++)
                {
                    if(ScreenRectangle.Intersects(ActiveChunks[i, j].GetChunkRectangle()))
                    {
                        for (int z = 0; z < 5; z++)
                        {
                            for (int x = 0; x < TileUtility.ChunkX; x++)
                            {
                                for (int y = 0; y < TileUtility.ChunkY; y++)
                                {
                                    Rectangle SourceRectangle = TileUtility.GetSourceRectangle(ActiveChunks[i, j].Tiles[z][x, y],tilesetTilesWide);
                                    Rectangle DestinationRectangle = TileUtility.GetDestinationRectangle(ActiveChunks[i, j].Tiles[z][x, y]);
                                    spriteBatch.Draw(TileSet, new Vector2(DestinationRectangle.X, DestinationRectangle.Y), SourceRectangle, Color.White,
                                    0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z]);

                                    if(ActiveChunks[i, j].Tufts.ContainsKey(ActiveChunks[i, j].Tiles[z][x, y].GetTileKey(0)))
                                    {
                                        for (int t = 0; t < ActiveChunks[i, j].Tufts[ActiveChunks[i, j].Tiles[z][x, y].GetTileKey(0)].Count; t++)
                                        {
                                            ActiveChunks[i, j].Tufts[ActiveChunks[i, j].Tiles[z][x, y].GetTileKey(0)][t].Draw(spriteBatch);
                                        }
                                    }
                                   

                                }
                            }
                        }
                    }
                    

                }
            }
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
