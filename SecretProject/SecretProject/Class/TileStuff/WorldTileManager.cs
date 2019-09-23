using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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



        public List<Chunk> ActiveChunks { get; set; }
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
        public Dictionary<string, Chest> AllChests { get; set; }
        public List<LightSource> AllLights { get; set; }
        public Dictionary<string, ObjectBody> AllObjects { get; set; }
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

            ActiveChunks = new List<Chunk>();

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


            foreach (Chunk chunk in ActiveChunks)
            {


                if (!chunk.IsLoaded)
                {
                    if (TileUtility.CheckIfChunkExistsInMemory(chunk.X, chunk.Y))
                    {
                        chunk.Load();
                    }
                    else
                    {
                        chunk.Generate(this);

                    }
                    chunk.Save();
                }
            }


        }


        public List<Chunk> GetActiveChunkCoord(Vector2 playerPos)
        {

            int currentChunkX = (int)(playerPos.X / 16 / TileUtility.ChunkX);
            int currentChunkY = (int)(playerPos.Y / 16 / TileUtility.ChunkY);
            return new List<Chunk>
            {
                 new Chunk(currentChunkX - 1, currentChunkY - 1), new Chunk (currentChunkX, currentChunkY - 1) , new Chunk(currentChunkX + 1, currentChunkY - 1) ,
                 new Chunk(currentChunkX - 1, currentChunkY), new Chunk(currentChunkX , currentChunkY), new Chunk (currentChunkX +1, currentChunkY ),
                 new Chunk(currentChunkX - 1, currentChunkY + 1), new Chunk( currentChunkX , currentChunkY + 1), new Chunk( currentChunkX +1, currentChunkY + 1)
            };

        }

        public void CheckActiveChunks()
        {
            List<Point> pointsToCheck = ChunkPointsWhichShouldBeActive(Game1.Player.position);


            for (int i = 0; i < pointsToCheck.Count; i++)
            {
                if (!ActiveChunks.Any(x => x.X == pointsToCheck[i].X && x.Y == pointsToCheck[i].Y))
                {

                    Chunk ChunkToAdd = new Chunk(pointsToCheck[i].X, pointsToCheck[i].Y);
                    if (!ChunkToAdd.IsLoaded)
                    {
                        if (TileUtility.CheckIfChunkExistsInMemory(ChunkToAdd.X, ChunkToAdd.Y))
                        {
                            ChunkToAdd.Load();
                        }
                        else
                        {
                            ChunkToAdd.Generate(this);

                        }
                        ChunkToAdd.Save();
                    }
                    ActiveChunks.Add(ChunkToAdd);
                }
                

                

            }
            for(int i =ActiveChunks.Count - 1; i >= 0;i--)
            {
                if (!pointsToCheck.Any(x => x.X == ActiveChunks[i].X && x.Y == ActiveChunks[i].Y))
                {
                    ActiveChunks.RemoveAt(i);
                }
            }

        }

        public List<Point> ChunkPointsWhichShouldBeActive(Vector2 playerPos)
        {
            int currentChunkX = (int)(playerPos.X / 16 / TileUtility.ChunkX);
            int currentChunkY = (int)(playerPos.Y / 16 / TileUtility.ChunkY);
            return new List<Point>
            {
                 new Point(currentChunkX - 1, currentChunkY - 1), new Point (currentChunkX, currentChunkY - 1) , new Point(currentChunkX + 1, currentChunkY - 1) ,
                 new Point(currentChunkX - 1, currentChunkY), new Point(currentChunkX , currentChunkY), new Point(currentChunkX +1, currentChunkY ) ,
                 new Point(currentChunkX - 1, currentChunkY + 1), new Point( currentChunkX , currentChunkY + 1), new Point( currentChunkX +1, currentChunkY + 1)
            };
        }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            ChunkPointUnderPlayer = new Point((int)(Game1.Player.Position.X / 16 / TileUtility.ChunkX), (int)(Game1.Player.Position.Y / 16 / TileUtility.ChunkY));
            if (ChunkPointUnderPlayerLastFrame != ChunkPointUnderPlayer)
            {
                CheckActiveChunks();
            }
            ChunkPointUnderPlayerLastFrame = ChunkPointUnderPlayer;


            int starti = (int)(Game1.cam.Pos.X) - (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2) - 1;

            int startj = (int)(Game1.cam.Pos.Y) - (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2) - 1;

            int endi = (int)(Game1.cam.Pos.X) + (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2) + 2;

            int endj = (int)(Game1.cam.Pos.Y) + (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2) + 2;

            Rectangle ScreenRectangle = new Rectangle(starti, startj, endi, endj);
            for (int i = 0; i < ActiveChunks.Count; i++)
            {



                if (ScreenRectangle.Intersects(ActiveChunks[i].GetChunkRectangle()))
                {
                    if (Game1.Player.Rectangle.Intersects(ActiveChunks[i].GetChunkRectangle()))
                    {
                        this.ChunkUnderPlayer = ActiveChunks[i];
                    }
                    for (int z = 0; z < 5; z++)
                    {
                        for (int x = 0; x < TileUtility.ChunkX; x++)
                        {
                            for (int y = 0; y < TileUtility.ChunkY; y++)
                            {
                                if(z == 0)
                                {
                                    if (ActiveChunks[i].Tufts.ContainsKey(ActiveChunks[i].Tiles[z][x, y].GetTileKey(0)))
                                    {
                                        for (int t = 0; t < ActiveChunks[i].Tufts[ActiveChunks[i].Tiles[z][x, y].GetTileKey(0)].Count; t++)
                                        {
                                            ActiveChunks[i].Tufts[ActiveChunks[i].Tiles[z][x, y].GetTileKey(0)][t].Update(gameTime);
                                        }
                                    }
                                }
                                
                                //update
                            }
                        }
                    }
                }
            }

        }


        public void DrawTiles(SpriteBatch spriteBatch)
        {

            int starti = (int)(Game1.cam.Pos.X) - (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2) - 1;

            int startj = (int)(Game1.cam.Pos.Y) - (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2) - 1;

            int endi = (int)(Game1.cam.Pos.X) + (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2) + 2;

            int endj = (int)(Game1.cam.Pos.Y) + (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2) + 2;

            Rectangle ScreenRectangle = new Rectangle(starti, startj, endi, endj);

            for (int i = 0; i < ActiveChunks.Count; i++)
            {
                if (ScreenRectangle.Intersects(ActiveChunks[i].GetChunkRectangle()))
                {
                    for (int z = 0; z < 5; z++)
                    {
                        for (int x = 0; x < TileUtility.ChunkX; x++)
                        {
                            for (int y = 0; y < TileUtility.ChunkY; y++)
                            {
                                Rectangle SourceRectangle = TileUtility.GetSourceRectangle(ActiveChunks[i].Tiles[z][x, y], tilesetTilesWide);
                                Rectangle DestinationRectangle = TileUtility.GetDestinationRectangle(ActiveChunks[i].Tiles[z][x, y]);
                                spriteBatch.Draw(TileSet, new Vector2(DestinationRectangle.X, DestinationRectangle.Y), SourceRectangle, Color.White,
                                0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z]);

                                if(z == 0)
                                {
                                    if (ActiveChunks[i].Tufts.ContainsKey(ActiveChunks[i].Tiles[z][x, y].GetTileKey(0)))
                                    {
                                        for (int t = 0; t < ActiveChunks[i].Tufts[ActiveChunks[i].Tiles[z][x, y].GetTileKey(0)].Count; t++)
                                        {
                                            ActiveChunks[i].Tufts[ActiveChunks[i].Tiles[z][x, y].GetTileKey(0)][t].Draw(spriteBatch);
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

        //public AStarPathFinder GetPathGrid(Vector2 entiyPosition)
        //{
        //    //lets create a path grid of the sorrounding 8 chunks around the entity

        //}
    }
}
