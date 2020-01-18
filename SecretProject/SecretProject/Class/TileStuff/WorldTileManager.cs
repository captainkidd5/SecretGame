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

        Point[] ChunkPositions;
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
            this.ChunkUnderMouse = new Chunk(this, 0, 0, 1, 1);

            Game1.GlobalClock.DayChanged += HandleClockChange;
            RenderDistance = 5;
            FullyActiveChunks = 3;
            ChunkPositions = new Point[RenderDistance * RenderDistance];

        }

        public void LoadGeneratableTileLists()
        {
            for (int i = 0; i < 10000; i++)
            {
                if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(i))
                {
                    //If this errors its because there's a property in tiled which contains generate but has a value which isn't supported. Capitals matter!
                    if (this.MapName.Tilesets[this.TileSetNumber].Tiles[i].Properties.ContainsKey("generate"))
                    {
                        GenerationIndex generationIndex = (GenerationIndex)Enum.Parse(typeof(GenerationIndex), this.MapName.Tilesets[this.TileSetNumber].Tiles[i].Properties["generate"]);
                        if (!Game1.Procedural.AllTilingContainers[(int)generationIndex].GeneratableTiles.Contains(i))
                        {
                            Game1.Procedural.AllTilingContainers[(int)generationIndex].GeneratableTiles.Add(i);
                        }
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
                    if (!this.ActiveChunks[i, j].IsLoaded)
                    {
                        if (Chunk.CheckIfChunkExistsInMemory(this.ActiveChunks[i, j].X, this.ActiveChunks[i, j].Y))
                        {
                            this.ActiveChunks[i, j].Load();
                        }
                        else
                        {
                            this.ActiveChunks[i, j].Generate();

                        }
                    }
                }
            }
            GetProperArrayData();
            this.ChunkUnderPlayer = this.ActiveChunks[RenderDistance / 2, RenderDistance / 2];
            this.ChunkPointUnderPlayer = ChunkPositions[ChunkPositions.Length / 2 + 1];
            this.ChunkPointUnderPlayerLastFrame = this.ChunkPointUnderPlayer;
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
            return this.ActiveChunks[ChunkUnderEntity.X, ChunkUnderEntity.Y];


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
                if (Chunk.CheckIfChunkExistsInMemory(chunk.X, chunk.Y))
                {
                    Task.Run(() => chunk.Load());
                }
                else
                {
                    Task.Run(() => chunk.Generate());
                }
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
            this.ChunkUnderPlayer = this.ActiveChunks[RenderDistance / 2, RenderDistance / 2];
            this.StoreableItems = this.ChunkUnderPlayer.StoreableItems;
            this.ChunkPointUnderPlayerLastFrame = this.ChunkPointUnderPlayer;

            for (int a = WorldTileManager.RenderDistance / 2 - 1; a < WorldTileManager.RenderDistance / 2 + 2; a++)
            {
                for (int b = WorldTileManager.RenderDistance / 2 - 1; b < WorldTileManager.RenderDistance / 2 + 2; b++)
                {
                    ChunkCheck(this.ActiveChunks[a, b]);
                }
            }


            //        int starti = (int)(Game1.cam.Pos.X) - (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2) - 1;

            //int startj = (int)(Game1.cam.Pos.Y) - (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2) - 1;

            //int endi = (int)(Game1.ScreenWidth / Game1.cam.Zoom);

            //int endj = (int)(Game1.ScreenHeight / Game1.cam.Zoom + 64);

            //this.ScreenRectangle = new Rectangle(starti, startj, endi, endj);

            for (int a = WorldTileManager.RenderDistance / 2 - 1; a < WorldTileManager.RenderDistance / 2 + 2; a++)
            {
                for (int b = WorldTileManager.RenderDistance / 2 - 1; b < WorldTileManager.RenderDistance / 2 + 2; b++)
                {
                    if (this.ActiveChunks[a, b].IsLoaded)
                    {


                        if (this.ActiveChunks[a, b].GetChunkRectangle().Intersects(Game1.cam.CameraScreenRectangle))
                        {

                            List<string> AnimationFrameKeysToRemove = new List<string>();
                            foreach (EditableAnimationFrameHolder frameholder in this.ActiveChunks[a, b].AnimationFrames.Values)
                            {
                                frameholder.Frames[frameholder.Counter].CurrentDuration -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                                if (frameholder.Frames[frameholder.Counter].CurrentDuration <= 0)
                                {
                                    frameholder.Frames[frameholder.Counter].CurrentDuration = frameholder.Frames[frameholder.Counter].AnchorDuration;
                                    this.ActiveChunks[a, b].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(frameholder.Frames[frameholder.Counter].ID, 100);
                                    if (frameholder.HasNewSource)
                                    {
                                        Rectangle newSourceRectangle = this.ActiveChunks[a, b].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].SourceRectangle;
                                        this.ActiveChunks[a, b].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].SourceRectangle = new Rectangle(newSourceRectangle.X + frameholder.OriginalXOffSet,
                                            newSourceRectangle.Y + frameholder.OriginalYOffSet, frameholder.OriginalWidth, frameholder.OriginalHeight);
                                    }


                                    //TileUtility.ReplaceTile(frameholder.Layer, frameholder.OldX, frameholder.OldY, frameholder.Frames[frameholder.Counter].ID + 1, this);

                                    if (frameholder.Counter == frameholder.Frames.Count - 1)
                                    {
                                        if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(frameholder.OriginalTileID))
                                        {

                                            if (this.MapName.Tilesets[this.TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("destructable"))
                                            {

                                                //this.ActiveChunks[a, b].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY] = new Tile(frameholder.OldX, frameholder.OldY, frameholder.OriginalTileID + 1);
                                                // this.ActiveChunks[a, b].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].TileKey = this.ActiveChunks[a, b].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].GetTileKeyStringNew(frameholder.Layer, this.ActiveChunks[a, b]);
                                                AnimationFrameKeysToRemove.Add(this.ActiveChunks[a, b].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].TileKey);
                                                if (this.MapName.Tilesets[this.TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("destructable"))
                                                {
                                                    TileUtility.FinalizeTile(frameholder.Layer, gameTime, frameholder.OldX, frameholder.OldY, this.ActiveChunks[a, b]);
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
                                this.ActiveChunks[a, b].AnimationFrames.Remove(key);
                            }
                            Rectangle ActiveChunkRectangle = this.ActiveChunks[a, b].GetChunkRectangle();


                            if (mouse.IsHoveringTile(ActiveChunkRectangle))
                            {
                                this.ChunkUnderMouse = this.ActiveChunks[a, b];
                            }


                        }



                    }
                    else
                    {
                        ChunkCheck(this.ActiveChunks[a, b]);
                    }
                }
            }

            int mouseI = TileUtility.GetLocalChunkCoord((int)mouse.WorldMousePosition.X);
            int mouseJ = TileUtility.GetLocalChunkCoord((int)mouse.WorldMousePosition.Y);
            int playerI = (int)(Game1.Player.position.X / 16 - (this.ChunkUnderPlayer.X * 16));
            int playerJ = (int)(Game1.Player.position.Y / 16 - (this.ChunkUnderPlayer.Y * 16));

            for (int z = 0; z < 4; z++)
            {

                //for (int mi = mouseI - 5; mi < mouseI + 5; mi++)
                //{
                //    for (int mj = mouseJ - 5; mj < mouseJ + 5; mj++)
                //    {

                //        Rectangle newIntersectionRectangle = new Rectangle(this.ChunkUnderPlayer.AllTiles[z][mi, mj].DestinationRectangle.X,
                //               this.ChunkUnderPlayer.AllTiles[z][mi, mj].DestinationRectangle.Y,
                //               this.ChunkUnderPlayer.AllTiles[z][mi, mj].SourceRectangle.Width,
                //               this.ChunkUnderPlayer.AllTiles[z][mi, mj].SourceRectangle.Height);
                //    }
                //}
                if (Game1.Player.IsMoving)
                {
                    if (z == 0)
                    {
                        if (playerI < this.ChunkUnderPlayer.AllTiles[z].GetLength(0) &&
                            playerJ < this.ChunkUnderPlayer.AllTiles[z].GetLength(1) &&
                            playerI >= 0 &&
                            playerJ >= 0 &&
                             this.ChunkUnderPlayer.AllTiles[z][playerI, playerJ] != null)
                        {


                            if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(this.ChunkUnderPlayer.AllTiles[z][playerI, playerJ].GID))
                            {
                                if (this.MapName.Tilesets[this.TileSetNumber].Tiles[this.ChunkUnderPlayer.AllTiles[z][playerI, playerJ].GID].Properties.ContainsKey("step"))
                                {

                                    Game1.Player.WalkSoundEffect = int.Parse(this.MapName.Tilesets[this.TileSetNumber].Tiles[this.ChunkUnderPlayer.AllTiles[z][playerI, playerJ].GID].Properties["step"]);
                                }
                            }
                        }

                    }
                }
                if (mouseI < 16 && mouseJ < 16 && mouseI >= 0 && mouseJ >= 0)
                {

                    if (this.ChunkUnderMouse.AllTiles[z][mouseI, mouseJ] != null)
                    {


                        if (this.ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GID != -1)
                        {

                            //int TileKey = ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GetTileKeyAsInt(z, ChunkUnderMouse);



                            if (this.ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle))
                            {


                                Game1.Player.UserInterface.DrawTileSelector = true;
                                Game1.Player.UserInterface.TileSelector.IndexX = mouseI;
                                Game1.Player.UserInterface.TileSelector.IndexY = mouseJ;
                                Game1.Player.UserInterface.TileSelector.WorldX = this.ChunkUnderMouse.X * 16 * 16 + mouseI * 16;
                                Game1.Player.UserInterface.TileSelector.WorldY = this.ChunkUnderMouse.Y * 16 * 16 + mouseJ * 16;


                                if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(this.ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GID))
                                {



                                    if (this.MapName.Tilesets[this.TileSetNumber].Tiles[this.ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GID].Properties.ContainsKey("destructable"))
                                    {

                                        Game1.isMyMouseVisible = false;

                                        if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem() != null)
                                        {
                                            if ((AnimationType)Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ItemType == (AnimationType)Game1.Utility.GetRequiredTileTool(this.MapName.Tilesets[this.TileSetNumber].Tiles[this.ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GID].Properties["destructable"]))
                                            {
                                                mouse.ChangeMouseTexture(((CursorType)Game1.Utility.GetRequiredTileTool(this.MapName.Tilesets[this.TileSetNumber].Tiles[this.ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GID].Properties["destructable"])));
                                            }
                                        }



                                        Game1.myMouseManager.ToggleGeneralInteraction = true;

                                        if (mouse.IsClicked)
                                        {
                                            TileUtility.InteractWithDestructableTile(z, gameTime, mouseI, mouseJ, this.ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].DestinationRectangle, this.ChunkUnderMouse);

                                        }

                                    }


                                    if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(this.ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GID))
                                    {
                                        if (this.MapName.Tilesets[this.TileSetNumber].Tiles[this.ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GID].Properties.ContainsKey("action"))
                                        {

                                            TileUtility.ActionHelper(z, mouseI, mouseJ, this.MapName.Tilesets[this.TileSetNumber].Tiles[this.ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GID].Properties["action"], mouse, this.ChunkUnderMouse);

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
            Tile tile = TileUtility.GetChunkTile((int)Game1.Player.ColliderRectangle.X, (int)Game1.Player.ColliderRectangle.Y + 16, 3, this.ActiveChunks);
            if (tile != null)
            {
                tile.ColorMultiplier = .25f;
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
                        if (Game1.cam.CameraScreenRectangle.Intersects(this.ActiveChunks[a, b].GetChunkRectangle()))
                        {
                            int startI = (Game1.cam.CameraScreenRectangle.X - 48 )/ 16 - this.ActiveChunks[a, b].X * 16;
                            int endI = Game1.cam.CameraScreenRectangle.X / 16 + (Game1.cam.CameraScreenRectangle.Width + 32) / 16  - this.ActiveChunks[a, b].X * 16;

                            int startY = (Game1.cam.CameraScreenRectangle.Y - 48) / 16  - this.ActiveChunks[a, b].Y * 16;
                            int endY = Game1.cam.CameraScreenRectangle.Y / 16 + (Game1.cam.CameraScreenRectangle.Height + 48 )/ 16 - this.ActiveChunks[a, b].Y * 16;


                            if (startI < 0)
                            {
                                startI = 0;
                            }
                            else if (startI > 15)
                            {
                                startI = 15;
                            }
                            if (endI < 0)
                            {
                                endI = 0;
                            }
                            else if (endI > 15)
                            {
                                endI = 15;
                            }



                            if (startY < 0)
                            {
                                startY = 0;
                            }
                            else if (startY > 15)
                            {
                                startY = 15;
                            }
                            if (endY < 0)
                            {
                                endY = 0;
                            }
                            else if (endY > 15)
                            {
                                endY = 15;
                            }



                            if (Game1.GetCurrentStage().ShowBorders)
                            {
                                spriteBatch.Draw(this.ActiveChunks[a, b].RectangleTexture, new Vector2(this.ActiveChunks[a, b].GetChunkRectangle().X, this.ActiveChunks[a, b].GetChunkRectangle().Y), color: Color.White, layerDepth: 1f);
                                spriteBatch.DrawString(Game1.AllTextures.MenuText, this.ActiveChunks[a, b].ArrayI.ToString() + this.ActiveChunks[a, b].ArrayJ.ToString(),
                                    new Vector2(this.ActiveChunks[a, b].GetChunkRectangle().X + 100, this.ActiveChunks[a, b].GetChunkRectangle().Y), Color.White, 0f, Game1.Utility.Origin, 5f, SpriteEffects.None, 1f);
                            }
                            for (int z = 0; z < 4; z++)
                            {
                                for (int i = startI; i <= endI; i++)
                                {
                                    for (int j = startY; j <= endY; j++)
                                    {
                                        if (this.ActiveChunks[a, b].AllTiles[z][i, j].GID != -1)
                                        {

                                            if (z == 3)
                                            {
                                                //..float testOffset = this.ActiveChunks[a, b].AllTiles[z][i, j].LayerToDrawAtZOffSet;
                                                //if (testAllOffsets.Contains(testOffset))
                                                //{
                                                //    totalRepeats++;
                                                //}
                                                //else
                                                //{
                                                //    testAllOffsets.Add(testOffset);
                                                //}
                                                spriteBatch.Draw(this.TileSet, new Vector2(this.ActiveChunks[a, b].AllTiles[z][i, j].DestinationRectangle.X, this.ActiveChunks[a, b].AllTiles[z][i, j].DestinationRectangle.Y), this.ActiveChunks[a, b].AllTiles[z][i, j].SourceRectangle, Color.White * this.ActiveChunks[a, b].AllTiles[z][i, j].ColorMultiplier,
                                                0f, Game1.Utility.Origin, 1f, SpriteEffects.None, this.AllDepths[z] + this.ActiveChunks[a, b].AllTiles[z][i, j].LayerToDrawAtZOffSet);

                                            }
                                            else
                                            {
                                                spriteBatch.Draw(this.TileSet, new Vector2(this.ActiveChunks[a, b].AllTiles[z][i, j].DestinationRectangle.X, this.ActiveChunks[a, b].AllTiles[z][i, j].DestinationRectangle.Y), this.ActiveChunks[a, b].AllTiles[z][i, j].SourceRectangle, Color.White,
                                            0f, Game1.Utility.Origin, 1f, SpriteEffects.None, this.AllDepths[z]);
                                            }

                                        }



                                    }
                                }
                            }
                            foreach (KeyValuePair<string, List<GrassTuft>> entry in this.ActiveChunks[a, b].Tufts)
                            {
                                for (int i = 0; i < entry.Value.Count; i++)
                                {

                                    entry.Value[i].Draw(spriteBatch);

                                }
                            }
                            //if (totalRepeats > 0)
                            //{
                            //    Console.WriteLine(totalRepeats.ToString());
                            //}





                        }
                    }
                }
            }
            if (this.GridItem != null)
            {
                this.GridItem.ChunkDraw(spriteBatch, this, this.ChunkUnderMouse);

            }
        }

        public void LoadInitialTileObjects(ILocation location)
        {
            throw new NotImplementedException();
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
    }
}
