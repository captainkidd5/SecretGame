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



        public Chunk[,] ActiveChunks { get; set; }
        public TmxMap MapName { get; set; }
        public Texture2D TileSet { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public ContentManager Content { get; set; }
        public List<float> AllDepths { get; set; }

        public int MaximumChunksLoaded { get; set; }

        public Chunk ChunkUnderPlayerLastFrame { get; set; }
        public Chunk ChunkUnderPlayer { get; set; }


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
                        if (TileUtility.CheckIfChunkExistsInMemory(i, j))
                        {
                            ActiveChunks[i, j].Load();
                        }
                        else
                        {
                            ActiveChunks[i, j].Generate();
                        }

                        ActiveChunks[i, j].Save();
                    }
                }
            }
        }


        public Chunk[,] GetActiveChunkCoord(Vector2 playerPos)
        {

            int currentChunkX = (int)(playerPos.X * 16 % 32);
            int currentChunkY = (int)(playerPos.Y * 16 % 32);
            return new Chunk[,]
            {
                { new Chunk(currentChunkX - 1, currentChunkY - 1), new Chunk (currentChunkX, currentChunkY - 1) , new Chunk(currentChunkX + 1, currentChunkY - 1) },
                { new Chunk(currentChunkX - 1, currentChunkY), new Chunk(currentChunkX , currentChunkY), new Chunk (currentChunkX +1, currentChunkY )},
                { new Chunk(currentChunkX - 1, currentChunkY + 1), new Chunk( currentChunkX , currentChunkY + 1), new Chunk( currentChunkX +1, currentChunkY + 1)},
            };

        }
        public void Update(GameTime gameTime, MouseManager mouse)
        {
            ChunkUnderPlayer = new Chunk((int)(Game1.Player.Position.X * 16 % 32), (int)(Game1.Player.Position.Y * 16 % 32));
            if (ChunkUnderPlayerLastFrame != ChunkUnderPlayer)
            {
                ActiveChunks = GetActiveChunkCoord(Game1.Player.Position);
            }
            ChunkUnderPlayerLastFrame = ChunkUnderPlayer;

            //for (int x =0; x < chunkCoords.GetLength(0); x++)
            //{
            //    for(int y = 0; y < chunkCoords.GetLength(1); y++)
            //    {

            //    }
            //}
            //LoadedChunks = 


            //int starti = (int)(Game1.cam.Pos.X / 16) - (int)(Game1.ScreenWidth / Game1.GetCurrentStage().Cam.Zoom / 2 / 16) - 1;

            //int startj = (int)(Game1.cam.Pos.Y / 16) - (int)(Game1.ScreenHeight / Game1.GetCurrentStage().Cam.Zoom / 2 / 16) - 1;

            //int endi = (int)(Game1.cam.Pos.X / 16) + (int)(Game1.ScreenWidth / Game1.GetCurrentStage().Cam.Zoom / 2 / 16) + 2;

            //int endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.GetCurrentStage().Cam.Zoom / 2 / 16) + 2;
            for (int i = 0; i < ActiveChunks.GetLength(0); i++)
            {
                for (int j = 0; j < ActiveChunks.GetLength(1); j++)
                {
                    for (int z = 0; z < 5; z++)
                    {
                        for (int x = 0; x < TileUtility.ChunkX; x++)
                        {
                            for (int y = 0; y < TileUtility.ChunkY; y++)
                            {
                                //LoadedChunks[i, j].Tiles[x,y]
                            }
                        }
                    }

                }
            }
        }
        public Rectangle GetDestinationRectangle(Tile tile)
        {
            int Column = tile.GID % tilesetTilesWide;
            int Row = (int)Math.Floor((double)tile.GID / (double)tilesetTilesWide);

            float X = (tile.X % mapWidth) * 16;
            float Y = (tile.Y % mapHeight) * 16;
            return new Rectangle((int)X, (int)Y, 16, 16);
        }
        public Rectangle GetSourceRectangle(Tile tile)
        {
            int Column = tile.GID % tilesetTilesWide;
            int Row = (int)Math.Floor((double)tile.GID / (double)tilesetTilesWide);

            return new Rectangle(16 * Column, 16 * Row, 16, 16);
        }

        public void DrawTiles(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < ActiveChunks.GetLength(0); i++)
            {
                for (int j = 0; j < ActiveChunks.GetLength(1); j++)
                {
                    for (int z = 0; z < 5; z++)
                    {
                        for (int x = 0; x < TileUtility.ChunkX; x++)
                        {
                            for (int y = 0; y < TileUtility.ChunkY; y++)
                            {
                                Rectangle SourceRectangle = GetSourceRectangle(ActiveChunks[i, j].Tiles[z][x, y]);
                                Rectangle DestinationRectangle = GetDestinationRectangle(ActiveChunks[i, j].Tiles[z][x, y]);
                                spriteBatch.Draw(TileSet, new Vector2(DestinationRectangle.X, DestinationRectangle.Y), SourceRectangle, Color.White,
                                0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z]);
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
