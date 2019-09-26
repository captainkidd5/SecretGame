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

        public Dictionary<string, List<GrassTuft>> Tufts { get; set; }
        public Dictionary<string, ObjectBody> Objects { get; set; }
        public Dictionary<string, EditableAnimationFrameHolder> AnimationFrames { get; set; }
        public Dictionary<string, int> TileHitPoints { get; set; }
        public Dictionary<string, Chest> Chests { get; set; }
        public List<LightSource> Lights { get; set; }
        public Dictionary<string, ObjectBody> CurrentObjects { get; set; }
        public int TileSetNumber { get; set; }
        public bool AbleToDrawTileSelector { get; set; }
        public List<int> DirtGeneratableTiles;
        public List<int> SandGeneratableTiles;
        public List<int> GrassGeneratableTiles;

        public Dictionary<string,Crop> Crops { get; set; }

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
            Tufts = new Dictionary<string, List<GrassTuft>>();
            TileHitPoints = new Dictionary<string, int>();
            CurrentObjects = new Dictionary<string, ObjectBody>();

            Chests = new Dictionary<string, Chest>();
            Lights = new List<LightSource>();

            CurrentObjects = new Dictionary<string, ObjectBody>();

            this.ChunkUnderPlayer = new Chunk(this, 0, 0);



        }

        public void LoadGeneratableTileLists()
        {
            for (int i = 0; i < 10000; i++)
            {
                if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(i))
                {
                    if (this.MapName.Tilesets[this.TileSetNumber].Tiles[i].Properties.ContainsKey("generate"))
                    {
                        switch (this.MapName.Tilesets[this.TileSetNumber].Tiles[i].Properties["generate"])
                        {


                            case "dirt":
                                if (!Game1.Utility.DirtGeneratableTiles.Contains(i))
                                {
                                    Game1.Utility.DirtGeneratableTiles.Add(i);
                                }
                                break;
                            case "sand":
                                if (!Game1.Utility.SandGeneratableTiles.Contains(i))
                                {
                                    Game1.Utility.SandGeneratableTiles.Add(i);
                                }
                                break;
                            case "grass":
                                if (!Game1.Utility.GrassGeneratableTiles.Contains(i))
                                {
                                    Game1.Utility.GrassGeneratableTiles.Add(i);
                                }
                                break;
                            case "dirtBasic":
                                if (!Game1.Utility.DirtGeneratableTiles.Contains(i))
                                {
                                    Game1.Utility.DirtGeneratableTiles.Add(i);
                                }
                                if (!Game1.Utility.StandardGeneratableDirtTiles.Contains(i))
                                {
                                    Game1.Utility.StandardGeneratableDirtTiles.Add(i);
                                }
                                break;
                        }


                    }
                }
            }
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
                 new Chunk(this,currentChunkX - 1, currentChunkY - 1), new Chunk (this,currentChunkX, currentChunkY - 1) , new Chunk(this,currentChunkX + 1, currentChunkY - 1) ,
                 new Chunk(this,currentChunkX - 1, currentChunkY), new Chunk(this,currentChunkX , currentChunkY), new Chunk (this,currentChunkX +1, currentChunkY ),
                 new Chunk(this,currentChunkX - 1, currentChunkY + 1), new Chunk(this, currentChunkX , currentChunkY + 1), new Chunk(this, currentChunkX +1, currentChunkY + 1)
            };

        }

        public void CheckActiveChunks()
        {
            List<Point> pointsToCheck = ChunkPointsWhichShouldBeActive(Game1.Player.position);


            for (int i = 0; i < pointsToCheck.Count; i++)
            {
                if (!ActiveChunks.Any(x => x.X == pointsToCheck[i].X && x.Y == pointsToCheck[i].Y))
                {

                    Chunk ChunkToAdd = new Chunk(this, pointsToCheck[i].X, pointsToCheck[i].Y);
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
            for (int i = ActiveChunks.Count - 1; i >= 0; i--)
            {
                if (!pointsToCheck.Any(x => x.X == ActiveChunks[i].X && x.Y == ActiveChunks[i].Y))
                {
                    ActiveChunks[i].Save();
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
                ChunkUnderPlayer = ActiveChunks.Find(x => x.X == ChunkPointUnderPlayer.X && x.Y == ChunkPointUnderPlayer.Y);
                this.Chests = ChunkUnderPlayer.Chests;
                this.CurrentObjects = ChunkUnderPlayer.Objects;
                this.Chests = ChunkUnderPlayer.Chests;

            }
            ChunkPointUnderPlayerLastFrame = ChunkPointUnderPlayer;


            int starti = (int)(Game1.cam.Pos.X) - (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2) - 1;

            int startj = (int)(Game1.cam.Pos.Y) - (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2) - 1;

            int endi = (int)(Game1.cam.Pos.X) + (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2) + 2;

            int endj = (int)(Game1.cam.Pos.Y) + (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2) + 2;

            Rectangle ScreenRectangle = new Rectangle(starti, startj, endi, endj);
            List<string> AnimationFrameKeysToRemove = new List<string>();
            for (int a = 0; a < ActiveChunks.Count; a++)
            {
                
                foreach (EditableAnimationFrameHolder frameholder in ActiveChunks[a].AnimationFrames.Values)
                {
                    frameholder.Frames[frameholder.Counter].CurrentDuration -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (frameholder.Frames[frameholder.Counter].CurrentDuration <= 0)
                    {
                        frameholder.Frames[frameholder.Counter].CurrentDuration = frameholder.Frames[frameholder.Counter].AnchorDuration;
                        ActiveChunks[a].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY] = new Tile(frameholder.OldX, frameholder.OldY,
                            frameholder.Frames[frameholder.Counter].ID + 1);
                        if (frameholder.Counter == frameholder.Frames.Count - 1)
                        {
                            if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(frameholder.OriginalTileID))
                            {

                                if (MapName.Tilesets[TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("destructable") || MapName.Tilesets[TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("relationX"))
                                {

                                    //needs to refer to first tile ?
                                    int frameolDX = frameholder.OldX;
                                    ActiveChunks[a].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY] = new Tile(frameholder.OldX, frameholder.OldY, frameholder.OriginalTileID + 1);
                                    AnimationFrameKeysToRemove.Add(ActiveChunks[a].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].GetTileKey(frameholder.Layer));
                                    if (MapName.Tilesets[TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("destructable"))
                                    {
                                        TileUtility.FinalizeTile(frameholder.Layer, gameTime,frameholder.OldX, frameholder.OldY, TileUtility.GetDestinationRectangle(ActiveChunks[a].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY], ActiveChunks[a].X, ActiveChunks[a].Y), Game1.GetCurrentStage(), ActiveChunks[a]);
                                    }

                                }
                            }

                            frameholder.Counter = 0;


                        }
                        else
                        {

                            frameholder.Counter++;

                        }

                    }
                }

                foreach (string key in AnimationFrameKeysToRemove)
                {
                    AnimationFrames.Remove(key);
                }

                if (ScreenRectangle.Intersects(ActiveChunks[a].GetChunkRectangle()))
                {
                    for (int z = 0; z < 5; z++)
                    {
                        for (int i = 0; i < TileUtility.ChunkX; i++)
                        {
                            for (int j = 0; j < TileUtility.ChunkY; j++)
                            {
                                string TileKey = ActiveChunks[a].AllTiles[z][i, j].GetTileKey(z);
                                Rectangle destinationRectangle = TileUtility.GetDestinationRectangle(ActiveChunks[a].AllTiles[z][i, j]);
                                Game1.Player.UserInterface.TileSelectorX = destinationRectangle.X;
                                Game1.Player.UserInterface.TileSelectorY = destinationRectangle.Y;
                                if (z == 0)
                                {
                                    if (ActiveChunks[a].Tufts.ContainsKey(TileKey))
                                    {
                                        for (int t = 0; t < ActiveChunks[a].Tufts[TileKey].Count; t++)
                                        {
                                            ActiveChunks[a].Tufts[TileKey][t].Update(gameTime);
                                        }
                                    }
                                }


                                if (destinationRectangle.Intersects(Game1.Player.ClickRangeRectangle))
                                {

                                    if (mouse.IsHoveringTile(destinationRectangle))
                                    {
                                        this.AbleToDrawTileSelector = true;
                                        //CurrentIndexX = i;
                                        //CurrentIndexY = j;

                                        if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(ActiveChunks[a].AllTiles[z][i, j].GID))
                                        {

                                            if (z == 1)
                                            {

                                                if (MapName.Tilesets[TileSetNumber].Tiles[ActiveChunks[a].AllTiles[z][i, j].GID].Properties.ContainsKey("destructable"))
                                                {
                                                    Game1.Player.UserInterface.DrawTileSelector = true;
                                                    Game1.isMyMouseVisible = false;
                                                    

                                                    mouse.ChangeMouseTexture(Game1.Utility.GetRequiredTileTool(MapName.Tilesets[TileSetNumber].Tiles[ActiveChunks[a].AllTiles[z][i, j].GID].Properties["destructable"]));

                                                    Game1.myMouseManager.ToggleGeneralInteraction = true;

                                                    if (mouse.IsClicked)
                                                    {
                                                        TileUtility.InteractWithBuilding(z, gameTime, i, j, destinationRectangle, Game1.GetCurrentStage(), ActiveChunks[a]);

                                                    }

                                                }

                                            }
                                            if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(ActiveChunks[a].AllTiles[z][i, j].GID))
                                            {
                                                if (MapName.Tilesets[TileSetNumber].Tiles[ActiveChunks[a].AllTiles[z][i, j].GID].Properties.ContainsKey("action"))
                                                {

                                                    TileUtility.ActionHelper(z, i, j, MapName.Tilesets[TileSetNumber].Tiles[ActiveChunks[a].AllTiles[z][i, j].GID].Properties["action"], mouse, ActiveChunks[a]);

                                                }
                                            }

                                        }
                                    }
                                }

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

            for (int a = 0; a < ActiveChunks.Count; a++)
            {
                if (ScreenRectangle.Intersects(ActiveChunks[a].GetChunkRectangle()))
                {
                    for (int z = 0; z < 5; z++)
                    {
                        for (int i = 0; i < TileUtility.ChunkX; i++)
                        {
                            for (int j = 0; j < TileUtility.ChunkY; j++)
                            {
                                if (ActiveChunks[a].AllTiles[z][i, j].GID != -1)
                                {


                                    Rectangle SourceRectangle = TileUtility.GetSourceRectangle(ActiveChunks[a].AllTiles[z][i, j], tilesetTilesWide);
                                    Rectangle DestinationRectangle = TileUtility.GetDestinationRectangle(ActiveChunks[a].AllTiles[z][i, j]);
                                    

                                    if (z == 0)
                                    {
                                        if (ActiveChunks[a].Tufts.ContainsKey(ActiveChunks[a].AllTiles[z][i, j].GetTileKey(0)))
                                        {
                                            for (int t = 0; t < ActiveChunks[a].Tufts[ActiveChunks[a].AllTiles[z][i, j].GetTileKey(0)].Count; t++)
                                            {
                                                ActiveChunks[a].Tufts[ActiveChunks[a].AllTiles[z][i, j].GetTileKey(0)][t].Draw(spriteBatch);
                                            }
                                        }
                                    }
                                    if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(ActiveChunks[a].AllTiles[z][a, j].GID))
                                    {
                                        if (MapName.Tilesets[this.TileSetNumber].Tiles[ActiveChunks[a].AllTiles[z][a, j].GID].Properties.ContainsKey("newSource"))
                                        {
                                            int[] rectangleCoords = Game1.Utility.GetNewTileSourceRectangle(MapName.Tilesets[this.TileSetNumber].Tiles[ActiveChunks[a].AllTiles[z][a, j].GID].Properties["newSource"]);


                                            SourceRectangle = new Rectangle(SourceRectangle.X + rectangleCoords[0], SourceRectangle.Y + rectangleCoords[1],
                                                SourceRectangle.Width + rectangleCoords[2], SourceRectangle.Height + rectangleCoords[3]);


                                            DestinationRectangle = new Rectangle(DestinationRectangle.X + rectangleCoords[0], DestinationRectangle.Y + rectangleCoords[1],
                                                DestinationRectangle.Width, DestinationRectangle.Height);
                                        }
                                    }
                                    if (z == 3)
                                    {
                                        spriteBatch.Draw(TileSet, new Vector2(DestinationRectangle.X, DestinationRectangle.Y), SourceRectangle, Game1.GlobalClock.TimeOfDayColor,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z] + ActiveChunks[a].AllTiles[z][i, j].LayerToDrawAtZOffSet);

                                    }
                                    else
                                    {
                                        spriteBatch.Draw(TileSet, new Vector2(DestinationRectangle.X, DestinationRectangle.Y), SourceRectangle, Color.White,
                                    0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z]);
                                    }
                                    
                                }



                            }
                        }
                    }
                    TileUtility.DrawGridItem(spriteBatch, this, ActiveChunks[a].AllTiles, ActiveChunks[a]);

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
