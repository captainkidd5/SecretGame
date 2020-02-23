using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.PathFinding;
using SecretProject.Class.PathFinding.PathFinder;
using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using SecretProject.Class.TileStuff.SanctuaryStuff;
using SecretProject.Class.TileStuff.SpawnStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public ObstacleGrid PathGrid { get; set; }


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
        public Chunk ChunkUnderPlayerLastFrame { get; set; }
        public Chunk ChunkUnderMouse { get; set; }
        public Dictionary<string, List<GrassTuft>> Tufts { get; set; }
        public Dictionary<string, List<ICollidable>> Objects { get; set; }
        public Dictionary<string, EditableAnimationFrameHolder> AnimationFrames { get; set; }
        public Dictionary<string, int> TileHitPoints { get; set; }
        public Dictionary<string, IStorableItemBuilding> StoreableItems { get; set; }
        public List<LightSource> NightTimeLights { get; set; }
        public List<LightSource> AllDayTimeLights { get; set; }
        public Dictionary<string, ICollidable> CurrentObjects { get; set; }
        public int TileSetNumber { get; set; }
        public bool AbleToDrawTileSelector { get; set; }

        public GridItem GridItem { get; set; }

        public List<int> DirtGeneratableTiles;
        public List<int> SandGeneratableTiles;
        public List<int> GrassGeneratableTiles;

        public Dictionary<string, Crop> Crops { get; set; }

        public Rectangle ScreenRectangle { get; set; }


        // List<ICollidable> ITileManager.Objects { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public PathFinder PathFinder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //Number of chunks loaded from disk at once
        public static int RenderDistance { get; set; }

        //Number of Chunks fully active 
        public static int FullyActiveChunks { get; set; }
        public List<SPlot> AllPlots { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        Point[] ChunkPositions;

        public int PlayerI { get; set; }
        public int PlayerJ { get; set; }
        public List<Item> AllItems { get; set; }

        int OldPlayerI;
        int OldPlayerJ;

        public Dictionary<int, TmxTilesetTile> TileSetDictionary { get; set; }
        public WorldTileManager(World world, Texture2D tileSet, List<TmxLayer> allLayers, TmxMap mapName, int numberOfLayers, int worldWidth, int worldHeight, GraphicsDevice graphicsDevice, ContentManager content, int tileSetNumber, List<float> allDepths)
        {
            this.MapName = mapName;
            this.TileSet = tileSet;

            this.TileWidth = 16;
            this.TileHeight = 16;

            this.tilesetTilesWide = tileSet.Width / this.TileWidth;
            this.tilesetTilesHigh = tileSet.Height / this.TileHeight;



            this.mapWidth = worldWidth;
            this.mapHeight = worldHeight;
            this.mapWidth = worldWidth;
            this.mapHeight = worldHeight;

            this.GraphicsDevice = graphicsDevice;
            this.Content = content;

            this.AllDepths = allDepths;

            this.ActiveChunks = new Chunk[this.MaximumChunksLoaded, this.MaximumChunksLoaded];

            this.MaximumChunksLoaded = 3;
            this.ChunkPointUnderPlayerLastFrame = new Point(1000, 1000);

            DirtGeneratableTiles = new List<int>();
            SandGeneratableTiles = new List<int>();
            GrassGeneratableTiles = new List<int>();
            this.AnimationFrames = new Dictionary<string, EditableAnimationFrameHolder>();
            this.Tufts = new Dictionary<string, List<GrassTuft>>();
            this.TileHitPoints = new Dictionary<string, int>();
            this.CurrentObjects = new Dictionary<string, ICollidable>();

            this.StoreableItems = new Dictionary<string, IStorableItemBuilding>();
            this.NightTimeLights = new List<LightSource>();
            this.AllDayTimeLights = new List<LightSource>();

            this.CurrentObjects = new Dictionary<string, ICollidable>();

            this.ChunkUnderPlayer = new Chunk(this, 0, 0, 1, 1);
            this.ChunkUnderPlayerLastFrame = this.ChunkUnderPlayer;
            this.ChunkUnderMouse = new Chunk(this, 0, 0, 1, 1);

            Game1.GlobalClock.DayChanged += HandleClockChange;
            RenderDistance = 5;
            FullyActiveChunks = 3;
            ChunkPositions = new Point[RenderDistance * RenderDistance];

            this.PlayerI = 0;
            this.PlayerJ = 0;

            this.OldPlayerI = 0;
            this.OldPlayerJ = 0;

            this.TileSetDictionary = this.MapName.Tilesets[this.TileSetNumber].Tiles;

        }

        public void LoadGeneratableTileLists()
        {


            for (int i = 0; i < 10000; i++)
            {
                if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(i))
                {

                    string generationProperty = string.Empty;
                    bool didSucceed = this.TileSetDictionary[i].Properties.TryGetValue("generate", out generationProperty);
                    if(didSucceed)
                    {
                        GenerationType generationIndex = (GenerationType)Enum.Parse(typeof(GenerationType), generationProperty);
                        if (!Game1.Procedural.GetTilingContainerFromGID((GenerationType)generationIndex).GeneratableTiles.Contains(i))
                        {
                            Game1.Procedural.AllTilingContainers[(GenerationType)generationIndex].GeneratableTiles.Add(i);
                        }
                    }
                    else
                    {
                       // throw new Exception("GenerationType value not supported. Check to make sure the tiled value of generate property is supported. Capitals matter.");

                    }
                       
                }
            }
        }

        public void LoadInitialChunks(Vector2 position)
        {

            this.ActiveChunks = GetActiveChunkCoord(position);


            for (int i = 0; i < RenderDistance; i++)
            {
                for (int j = 0; j < RenderDistance; j++)
                {
                    Chunk chunk = ActiveChunks[i, j];
                    if (!chunk.IsLoaded)
                    {
                        if (Chunk.CheckIfChunkExistsInMemory(chunk.ChunkPath, chunk.X, chunk.Y))
                        {
                            chunk.Load();
                        }
                        else
                        {
                            chunk.Generate();

                        }
                    }
                }
            }
            GetProperArrayData();
            this.ChunkUnderPlayer = this.ActiveChunks[RenderDistance / 2, RenderDistance / 2];
            this.ChunkPointUnderPlayer = ChunkPositions[ChunkPositions.Length / 2 + 1];
            this.ChunkPointUnderPlayerLastFrame = this.ChunkPointUnderPlayer;
        }

        public void AddItem(Item item, Vector2 position)
        {
            GetChunkFromPosition(position).AllItems.Add(item);
        }

        public List<Item> GetItems(Vector2 position)
        {
            return GetChunkFromPosition(position).AllItems;
        }

        public Chunk[,] GetActiveChunkCoord(Vector2 playerPos)
        {

            int currentChunkX = (int)(playerPos.X / 16 / TileUtility.ChunkWidth);
            int currentChunkY = (int)(playerPos.Y / 16 / TileUtility.ChunkHeight);

            Chunk[,] ChunksToReturn = new Chunk[RenderDistance, RenderDistance];
            int x = (RenderDistance / 2) * -1;
            int y = (RenderDistance / 2) * -1;
            for (int i = 0; i < RenderDistance; i++)
            {
                for (int j = 0; j < RenderDistance; j++)
                {
                    ChunksToReturn[i, j] = new Chunk(this, currentChunkX + x, currentChunkY + y, i, j);
                    y++;
                }
                y = (RenderDistance / 2) * -1;
                x++;
            }
            return ChunksToReturn;


        }

        public Chunk GetChunkFromPosition(Vector2 entityPosition)
        {
            Point ChunkUnderEntity = new Point((int)(entityPosition.X / 16 / TileUtility.ChunkWidth), (int)(entityPosition.Y / 16 / TileUtility.ChunkHeight));
            Chunk chunk = ChunkUtility.GetChunk(ChunkUnderEntity.X, ChunkUnderEntity.Y, this.ActiveChunks);
            if (chunk == null)
            {
                return ChunkUnderPlayer;
            }
            return chunk;


        }

        public void MoveChunks(Dir direction)
        {

            switch (direction)
            {
                //CORRECT
                case Dir.Down:
                    //shifts everything down one
                    for (int i = 0; i < RenderDistance; i++)
                    {
                        for (int j = 0; j < RenderDistance; j++)
                        {
                            if (j == RenderDistance - 1)
                            {
                                this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[ChunkPositions.Length - RenderDistance + i].X - 1,
                                    ChunkPositions[ChunkPositions.Length - RenderDistance + i].Y - 1, i, j);
                                ChunkCheck(this.ActiveChunks[i, j]);
                            }
                            else
                            {
                                if (j == 0)
                                {
                                    SaveAndDiscardChunk(this.ActiveChunks[i, j]);
                                }
                                this.ActiveChunks[i, j] = this.ActiveChunks[i, j + 1];
                                ReassignArrayIAndJ(this.ActiveChunks[i, j], i, j);

                            }
                        }
                    }
                    break;
                case Dir.Up:
                    //shifts everything up one
                    for (int i = RenderDistance - 1; i > -1; i--)
                    {
                        for (int j = RenderDistance - 1; j > -1; j--)
                        {
                            if (j == 0)
                            {
                                this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[i].X - 1,
                                    ChunkPositions[i].Y - 1, i, j);
                                ChunkCheck(this.ActiveChunks[i, j]);
                            }
                            else
                            {
                                if (j == RenderDistance - 1)
                                {
                                    SaveAndDiscardChunk(this.ActiveChunks[i, j]);
                                }
                                this.ActiveChunks[i, j] = this.ActiveChunks[i, j - 1];
                                ReassignArrayIAndJ(this.ActiveChunks[i, j], i, j);

                            }
                        }
                    }
                    break;
                case Dir.Left:
                    for (int i = RenderDistance - 1; i > -1; i--)
                    {
                        for (int j = RenderDistance - 1; j > -1; j--)
                        {
                            if (i == 0)
                            {
                                if (j == 0)
                                {
                                    this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[0].X - 1,
                                    ChunkPositions[0].Y - 1, i, j);
                                }
                                else if (j == 1)
                                {
                                    this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[5].X - 1,
                                    ChunkPositions[5].Y - 1, i, j);

                                }
                                else if (j == 2)
                                {
                                    this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[10].X - 1,
                                    ChunkPositions[10].Y - 1, i, j);
                                }
                                else if (j == 3)
                                {
                                    this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[15].X - 1,
                                    ChunkPositions[15].Y - 1, i, j);

                                }
                                else if (j == 4)
                                {
                                    this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[20].X - 1,
                                    ChunkPositions[20].Y - 1, i, j);
                                }

                                ChunkCheck(this.ActiveChunks[i, j]);
                            }

                            else
                            {
                                if (i == RenderDistance - 1)
                                {
                                    SaveAndDiscardChunk(this.ActiveChunks[i, j]);
                                }
                                this.ActiveChunks[i, j] = this.ActiveChunks[i - 1, j];
                                ReassignArrayIAndJ(this.ActiveChunks[i, j], i, j);

                            }
                        }
                    }
                    break;
                //CORRECT
                case Dir.Right:
                    for (int i = 0; i < RenderDistance; i++)
                    {
                        for (int j = 0; j < RenderDistance; j++)
                        {
                            if (i == RenderDistance - 1)
                            {
                                if (j == 0)
                                {
                                    this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[4].X - 1,
                                    ChunkPositions[4].Y - 1, i, j);
                                }
                                else if (j == 1)
                                {
                                    this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[9].X - 1,
                                    ChunkPositions[9].Y - 1, i, j);

                                }
                                else if (j == 2)
                                {
                                    this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[14].X - 1,
                                    ChunkPositions[14].Y - 1, i, j);
                                }
                                else if (j == 3)
                                {
                                    this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[19].X - 1,
                                    ChunkPositions[19].Y - 1, i, j);
                                }
                                else if (j == 4)
                                {
                                    this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[24].X - 1,
                                    ChunkPositions[24].Y - 1, i, j);
                                }

                                ChunkCheck(this.ActiveChunks[i, j]);
                            }

                            else
                            {
                                if (i == 0)
                                {
                                    SaveAndDiscardChunk(this.ActiveChunks[i, j]);
                                }
                                this.ActiveChunks[i, j] = this.ActiveChunks[i + 1, j];
                                ReassignArrayIAndJ(this.ActiveChunks[i, j], i, j);

                            }
                        }
                    }
                    break;
            }


        }
        /// <summary>
        /// Simply notifies chunk that its indices in the Active Chunk Array have changed.
        /// Used when shifting chunks around in the array without having to generate or load new ones.
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public void ReassignArrayIAndJ(Chunk chunk, int i, int j)
        {
            chunk.ArrayI = i;
            chunk.ArrayJ = j;
        }
        public void ChunkCheck(Chunk chunk)
        {

            if (!chunk.IsLoaded && !chunk.IsDoneLoading)
            {
                if (Chunk.CheckIfChunkExistsInMemory(chunk.ChunkPath, chunk.X, chunk.Y))
                {
                    Task.Run(() => chunk.Load());
                }
                else
                {
                    Task.Run(() => chunk.Generate());
                }
            }

        }



        public void SaveAndDiscardChunk(Chunk chunk)
        {
            if (chunk.IsDoneLoading && chunk.IsLoaded)
            {
                Chunk newChunk = chunk;
                Task.Run(() => newChunk.Save());
            }
        }

        public Point GetChunkPositionFromCamera(float x, float y)
        {
            int returnX = (int)(x / 16 / 16);
            int returnY = (int)(y / 16 / 16);
            if (x < 0)
            {
                returnX--;
            }
            if (y < 0)
            {
                returnY--;
            }

            return new Point(returnX, returnY);
        }

        public void GetProperArrayData()
        {
            Point rootPosition = GetChunkPositionFromCamera(Game1.Player.position.X, Game1.Player.position.Y);
            int i = 0;


            for (int y = rootPosition.Y - 1; y < rootPosition.Y + RenderDistance - 1; y++)
            {
                for (int x = rootPosition.X - 1; x < rootPosition.X + RenderDistance - 1; x++)
                {
                    ChunkPositions[i++] = new Point(x, y);
                }
            }

        }
        public void Update(GameTime gameTime, MouseManager mouse)
        {
            GetProperArrayData();

            this.ChunkPointUnderPlayer = ChunkPositions[ChunkPositions.Length / 2 + 1];

            if (this.ChunkPointUnderPlayerLastFrame != this.ChunkPointUnderPlayer)
            {


                if (this.ChunkPointUnderPlayer.X > this.ChunkPointUnderPlayerLastFrame.X)
                {
                    MoveChunks(Dir.Right);
                }
                else if (this.ChunkPointUnderPlayer.X < this.ChunkPointUnderPlayerLastFrame.X)
                {

                    MoveChunks(Dir.Left);

                }
                else if (this.ChunkPointUnderPlayer.Y > this.ChunkPointUnderPlayerLastFrame.Y)
                {

                    MoveChunks(Dir.Down);

                }
                else if (this.ChunkPointUnderPlayer.Y < this.ChunkPointUnderPlayerLastFrame.Y)
                {

                    MoveChunks(Dir.Up);

                }


            }
            this.ChunkUnderPlayerLastFrame = this.ChunkUnderPlayer;
            this.ChunkUnderPlayer = this.ActiveChunks[RenderDistance / 2, RenderDistance / 2];
            this.StoreableItems = this.ChunkUnderPlayer.StoreableItems;
            this.ChunkPointUnderPlayerLastFrame = this.ChunkPointUnderPlayer;

            Game1.Player.WorldSquarePosition = new Vector2((int)(Game1.Player.Position.X / 16), (int)(Game1.Player.Position.Y / 16));

            for (int a = WorldTileManager.RenderDistance / 2 - 1; a < WorldTileManager.RenderDistance / 2 + 2; a++)
            {
                for (int b = WorldTileManager.RenderDistance / 2 - 1; b < WorldTileManager.RenderDistance / 2 + 2; b++)
                {
                    ChunkCheck(this.ActiveChunks[a, b]);
                }
            }


            for (int a = WorldTileManager.RenderDistance / 2 - 1; a < WorldTileManager.RenderDistance / 2 + 2; a++)
            {
                for (int b = WorldTileManager.RenderDistance / 2 - 1; b < WorldTileManager.RenderDistance / 2 + 2; b++)
                {
                    Chunk currentChunk = this.ActiveChunks[a, b];
                    if (currentChunk.IsLoaded)
                    {


                        if (currentChunk.GetChunkRectangle().Intersects(Game1.cam.CameraScreenRectangle))
                        {

                            List<string> AnimationFrameKeysToRemove = new List<string>();
                            foreach (EditableAnimationFrameHolder frameholder in currentChunk.AnimationFrames.Values)
                            {
                                frameholder.Frames[frameholder.Counter].CurrentDuration -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                                if (frameholder.Frames[frameholder.Counter].CurrentDuration <= 0)
                                {
                                    frameholder.Frames[frameholder.Counter].CurrentDuration = frameholder.Frames[frameholder.Counter].AnchorDuration;
                                    currentChunk.AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(frameholder.Frames[frameholder.Counter].ID, 100);
                                    if (frameholder.HasNewSource)
                                    {
                                        Rectangle newSourceRectangle = currentChunk.AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].SourceRectangle;
                                        currentChunk.AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].SourceRectangle = new Rectangle(newSourceRectangle.X + frameholder.OriginalXOffSet,
                                            newSourceRectangle.Y + frameholder.OriginalYOffSet, frameholder.OriginalWidth, frameholder.OriginalHeight);
                                    }


                                    //TileUtility.ReplaceTile(frameholder.Layer, frameholder.OldX, frameholder.OldY, frameholder.Frames[frameholder.Counter].ID + 1, this);

                                    if (frameholder.Counter == frameholder.Frames.Count - 1)
                                    {
                                        if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(frameholder.OriginalTileID))
                                        {

                                            if (this.MapName.Tilesets[this.TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("destructable"))
                                            {

                                                //currentChunk.AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY] = new Tile(frameholder.OldX, frameholder.OldY, frameholder.OriginalTileID + 1);
                                                // currentChunk.AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].TileKey = currentChunk.AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].GetTileKeyStringNew(frameholder.Layer, currentChunk);
                                                AnimationFrameKeysToRemove.Add(currentChunk.AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].TileKey);
                                                if (this.MapName.Tilesets[this.TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("destructable"))
                                                {
                                                    TileUtility.FinalizeTile(frameholder.Layer, gameTime, frameholder.OldX, frameholder.OldY, currentChunk);
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
                                currentChunk.AnimationFrames.Remove(key);
                            }
                            Rectangle ActiveChunkRectangle = currentChunk.GetChunkRectangle();


                            if (mouse.IsHoveringTile(ActiveChunkRectangle))
                            {
                                this.ChunkUnderMouse = currentChunk;
                            }

                            for (int item = 0; item < currentChunk.AllItems.Count; item++)
                            {
                                currentChunk.AllItems[item].Update(gameTime);
                            }

                        }



                    }
                    else
                    {
                        ChunkCheck(currentChunk);
                    }
                }
            }
            OldPlayerI = PlayerI;
            OldPlayerJ = PlayerJ;
            int mouseI = ChunkUtility.GetLocalChunkCoord((int)mouse.WorldMousePosition.X);
            int mouseJ = ChunkUtility.GetLocalChunkCoord((int)mouse.WorldMousePosition.Y);
            PlayerI = (int)((Game1.Player.ColliderRectangle.X + 8) / 16 - (this.ChunkUnderPlayer.X * 16));
            PlayerJ = (int)(Game1.Player.ColliderRectangle.Y / 16 - (this.ChunkUnderPlayer.Y * 16));
            if (PlayerI > 15)
            {
                PlayerI = 15;
            }
            if (PlayerJ > 15)
            {
                PlayerJ = 15;
            }


            for (int z = 0; z < 4; z++)
            {

                if (Game1.Player.IsMoving)
                {
                    if (z == 0)
                    {
                        if (PlayerI < this.ChunkUnderPlayer.AllTiles[z].GetLength(0) &&
                            PlayerJ < this.ChunkUnderPlayer.AllTiles[z].GetLength(1) &&
                            PlayerI >= 0 &&
                            PlayerJ >= 0 &&

                             this.ChunkUnderPlayer.AllTiles[z][PlayerI, PlayerJ] != null)
                        {
                            this.ChunkUnderPlayerLastFrame.PathGrid.UpdateGrid(OldPlayerI, OldPlayerJ, GridStatus.Clear);
                            this.ChunkUnderPlayer.PathGrid.UpdateGrid(PlayerI, PlayerJ, GridStatus.Obstructed);


                            if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(this.ChunkUnderPlayer.AllTiles[z][PlayerI, PlayerJ].GID))
                            {
                                if (this.MapName.Tilesets[this.TileSetNumber].Tiles[this.ChunkUnderPlayer.AllTiles[z][PlayerI, PlayerJ].GID].Properties.ContainsKey("step"))
                                {

                                    Game1.Player.WalkSoundEffect = int.Parse(this.MapName.Tilesets[this.TileSetNumber].Tiles[this.ChunkUnderPlayer.AllTiles[z][PlayerI, PlayerJ].GID].Properties["step"]);
                                }
                            }
                        }

                    }
                }
                if (mouseI < 16 && mouseJ < 16 && mouseI >= 0 && mouseJ >= 0)
                {
                    Tile tileUnderMouse = this.ChunkUnderMouse.AllTiles[z][mouseI, mouseJ];
                    if (tileUnderMouse != null)
                    {


                        if (tileUnderMouse.GID != -1)
                        {


                            if (tileUnderMouse.DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle))
                            {


                                Game1.Player.UserInterface.DrawTileSelector = true;
                                Game1.Player.UserInterface.TileSelector.IndexX = mouseI;
                                Game1.Player.UserInterface.TileSelector.IndexY = mouseJ;
                                Game1.Player.UserInterface.TileSelector.WorldX = this.ChunkUnderMouse.X * 16 * 16 + mouseI * 16;
                                Game1.Player.UserInterface.TileSelector.WorldY = this.ChunkUnderMouse.Y * 16 * 16 + mouseJ * 16;


                                if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(tileUnderMouse.GID))
                                {



                                    if (this.MapName.Tilesets[this.TileSetNumber].Tiles[tileUnderMouse.GID].Properties.ContainsKey("destructable"))
                                    {

                                        Game1.isMyMouseVisible = false;

                                        if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem() != null)
                                        {
                                            if ((AnimationType)Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ItemType == (AnimationType)Game1.Utility.GetRequiredTileTool(this.MapName.Tilesets[this.TileSetNumber].Tiles[tileUnderMouse.GID].Properties["destructable"]))
                                            {
                                                mouse.ChangeMouseTexture(((CursorType)Game1.Utility.GetRequiredTileTool(this.MapName.Tilesets[this.TileSetNumber].Tiles[tileUnderMouse.GID].Properties["destructable"])));
                                            }
                                        }



                                        Game1.myMouseManager.ToggleGeneralInteraction = true;

                                        if (mouse.IsClicked)
                                        {
                                            TileUtility.InteractWithDestructableTile(z, gameTime, mouseI, mouseJ, tileUnderMouse.DestinationRectangle, this.ChunkUnderMouse);

                                        }

                                    }


                                    if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(tileUnderMouse.GID))
                                    {
                                        if (this.MapName.Tilesets[this.TileSetNumber].Tiles[tileUnderMouse.GID].Properties.ContainsKey("action"))
                                        {

                                            TileUtility.ActionHelper(z, mouseI, mouseJ, this.MapName.Tilesets[this.TileSetNumber].Tiles[tileUnderMouse.GID].Properties["action"], mouse, this.ChunkUnderMouse);

                                        }
                                    }

                                }

                            }
                            else
                            {
                                Game1.Player.UserInterface.DrawTileSelector = false;
                            }
                        }
                    }

                }



            }

            if (this.GridItem != null)
            {
                this.GridItem.ChunkUpdate(gameTime, this, this.ChunkUnderMouse);
            }

        }


        public void DrawTiles(SpriteBatch spriteBatch)
        {



            for (int a = 0; a < RenderDistance; a++)
            {
                for (int b = 0; b < RenderDistance; b++)
                {
                    if (this.ActiveChunks[a, b].IsLoaded)
                    {
  

                        Chunk chunk = this.ActiveChunks[a, b];
                        int startI = CheckArrayLimits((Game1.cam.CameraScreenRectangle.X - 48) / 16 - chunk.X * 16);
                        int endI = CheckArrayLimits(Game1.cam.CameraScreenRectangle.X / 16 + (Game1.cam.CameraScreenRectangle.Width + 32) / 16 - chunk.X * 16);

                        int startY = CheckArrayLimits((Game1.cam.CameraScreenRectangle.Y - 48) / 16 - chunk.Y * 16);
                        int endY = CheckArrayLimits(Game1.cam.CameraScreenRectangle.Y / 16 + (Game1.cam.CameraScreenRectangle.Height + 128) / 16 - chunk.Y * 16);

                        if (Game1.GetCurrentStage().ShowBorders)
                        {
                            spriteBatch.Draw(chunk.RectangleTexture, new Vector2(chunk.GetChunkRectangle().X, chunk.GetChunkRectangle().Y), color: Color.White, layerDepth: 1f);
                            spriteBatch.DrawString(Game1.AllTextures.MenuText, chunk.ArrayI.ToString() + chunk.ArrayJ.ToString(),
                                new Vector2(chunk.GetChunkRectangle().X + 100, chunk.GetChunkRectangle().Y), Color.White, 0f, Game1.Utility.Origin, 5f, SpriteEffects.None, 1f);
                        }
                        for (int z = 0; z < 4; z++)
                        {
                            for (int i = startI; i <= endI; i++)
                            {
                                for (int j = startY; j <= endY; j++)
                                {
                                    Tile tile = chunk.AllTiles[z][i, j];
                                    if (chunk.AllTiles[z][i, j].GID != -1)
                                    {

                                        if (z == 3)
                                        {

                                            spriteBatch.Draw(this.TileSet, new Vector2(tile.DestinationRectangle.X, tile.DestinationRectangle.Y), tile.SourceRectangle, Color.White * tile.ColorMultiplier,
                                            0f, Game1.Utility.Origin, 1f, SpriteEffects.None, this.AllDepths[z] + tile.LayerToDrawAtZOffSet);

                                        }
                                        else
                                        {
                                            spriteBatch.Draw(this.TileSet, new Vector2(tile.DestinationRectangle.X, tile.DestinationRectangle.Y), tile.SourceRectangle, Color.White,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, this.AllDepths[z]);
                                        }

                                    }
                                    if (Game1.GetCurrentStage().ShowBorders)
                                    {
                                        if (ActiveChunks[a, b].PathGrid.Weight[i, j] == (int)GridStatus.Obstructed)
                                        {
                                            spriteBatch.Draw(this.TileSet, new Vector2(tile.DestinationRectangle.X, tile.DestinationRectangle.Y), tile.SourceRectangle, Color.Red,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, 1f);
                                        }
                                    }




                                }
                            }
                        }

                        DrawGrassTufts(spriteBatch, chunk);
                        DrawItems(spriteBatch, chunk);
                        
                    }
                }
            }
            if (this.GridItem != null)
            {
                this.GridItem.ChunkDraw(spriteBatch, this, this.ChunkUnderMouse);

            }
        }

        public void DrawGrassTufts(SpriteBatch spriteBatch, Chunk chunk)
        {
            foreach (KeyValuePair<string, List<GrassTuft>> entry in chunk.Tufts)
            {
                for (int i = 0; i < entry.Value.Count; i++)
                {

                    entry.Value[i].Draw(spriteBatch);

                }
            }
        }
        public void DrawItems(SpriteBatch spriteBatch, Chunk chunk)
        {
            for (int item = 0; item < chunk.AllItems.Count; item++)
            {
                chunk.AllItems[item].Draw(spriteBatch);
            }
        }

        public void LoadInitialTileObjects(ILocation location)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// We dont want the index of the array to be out of bounds. If it is just adjust it to min or max values.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int CheckArrayLimits(int index)
        {

            if (index < 0)
            {
                index = 0;
            }
            else if (index > 15)
            {
                index = 15;
            }
            return index;
        }

        public void UpdateCropTile()
        {
            for (int i = 0; i < this.ActiveChunks.GetLength(0); i++)
            {
                for (int j = 0; j < this.ActiveChunks.GetLength(1); j++)
                {
                    for (int x = 0; x < this.ActiveChunks[i, j].Crops.Count; x++)
                    {
                        Crop crop = this.ActiveChunks[i, j].Crops.ElementAt(x).Value;
                        crop.CurrentGrowth = Game1.GlobalClock.TotalDays - crop.DayPlanted;
                        if (crop.CurrentGrowth >= crop.DaysToGrow)
                        {
                            crop.CurrentGrowth = crop.DaysToGrow - 1;
                        }
                        if (crop.CurrentGrowth < this.MapName.Tilesets[this.TileSetNumber].Tiles[crop.BaseGID].AnimationFrames.Count)
                        {
                            int newGid = this.MapName.Tilesets[this.TileSetNumber].Tiles[crop.BaseGID].AnimationFrames[crop.CurrentGrowth].Id + 1;
                            crop.UpdateGrowthCycle(newGid);

                            TileUtility.ReplaceTile(3, crop.X, crop.Y, crop.GID, this.ActiveChunks[i, j]);
                        }

                    }
                }

            }
        }
        public void HandleClockChange(object sender, EventArgs eventArgs)
        {

            UpdateCropTile();
        }

        public ObstacleGrid GetPathGrid(Vector2 entityPosition)
        {


            return this.PathGrid;

        }

        public void SaveTiles()
        {
            SaveAllChunks();
        }

        public void SaveAllChunks()
        {
            for (int i = 0; i < this.ActiveChunks.GetLength(0); i++)
            {
                for (int j = 0; j < this.ActiveChunks.GetLength(1); j++)
                {
                    this.ActiveChunks[i, j].Save();
                }
            }
        }
    }
}
