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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TiledSharp;
using XMLData.ItemStuff;

namespace SecretProject.Class.TileStuff
{
    public class WorldTileManager : ITileManager
    {
        public ILocation Stage { get; set; }
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


        public Dictionary<string, Crop> Crops { get; set; }

        public Rectangle ScreenRectangle { get; set; }


        // List<ICollidable> ITileManager.Objects { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public PathFinder PathFinder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //Number of chunks loaded from disk at once
        public static int RenderDistance { get; set; }


        public List<SPlot> AllPlots { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        Point[,] ChunkPositions;

        public int PlayerI { get; set; }
        public int PlayerJ { get; set; }
        public List<Item> AllItems { get; set; }

        int OldPlayerI;
        int OldPlayerJ;

        public Dictionary<int, TmxTilesetTile> TileSetDictionary { get; set; }

        public GridStatus PreviousTileUnderPlayerGridStatus { get; set; }

        public Object ChunkLocker { get; set; }

        public Texture2D ChunkTexture { get; set; }

        public WorldTileManager(World world, Texture2D tileSet,  TmxMap mapName, int worldWidth, int worldHeight, GraphicsDevice graphicsDevice, ContentManager content, int tileSetNumber)
        {
            this.Stage = world;
            this.MapName = mapName;
            this.TileSet = tileSet;

            this.TileWidth = 16;
            this.TileHeight = 16;

            this.tilesetTilesWide = tileSet.Width / this.TileWidth;
            this.tilesetTilesHigh = tileSet.Height / this.TileHeight;

            this.AllDepths = new List<float>()
            {
                .1f,
                .2f,
                .3f,
                .5f,
            };

            this.mapWidth = worldWidth;
            this.mapHeight = worldHeight;
            this.mapWidth = worldWidth;
            this.mapHeight = worldHeight;

            this.GraphicsDevice = graphicsDevice;
            this.Content = content;



           

            this.MaximumChunksLoaded = 3;
            this.ChunkPointUnderPlayerLastFrame = new Point(1000, 1000);


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
            RenderDistance = 9; //render distance MUST be odd.
            this.ActiveChunks = new Chunk[RenderDistance, RenderDistance];

            ChunkPositions = new Point[RenderDistance, RenderDistance];

            this.PlayerI = 0;
            this.PlayerJ = 0;

            this.OldPlayerI = 0;
            this.OldPlayerJ = 0;

            this.TileSetDictionary = this.MapName.Tilesets[this.TileSetNumber].Tiles;

            this.PathGrid = Game1.Town.AllTiles.PathGrid;
            this.AllTiles = Game1.Town.AllTiles.AllTiles;

            this.ChunkLocker = new object();
            this.ChunkTexture = SetChunkTexture(this.GraphicsDevice);
        }




        /// <summary>
        /// Loads tiling dictionaries in from tileset and assigns them into Game1.procedural so that the wangmanager knows what to do.
        /// </summary>
        public void LoadGeneratableTileLists()
        {
            string generationProperty = string.Empty;
            for (int i = 0; i < 10000; i++)
            {
                if (TileSetDictionary.ContainsKey(i))
                {


                    bool didSucceed = this.TileSetDictionary[i].Properties.TryGetValue("generate", out generationProperty);
                    if (didSucceed)
                    {
                        GenerationType generationIndex = (GenerationType)Enum.Parse(typeof(GenerationType), generationProperty);
                        if (!Game1.Procedural.GetTilingContainerFromGID((GenerationType)generationIndex).GeneratableTiles.Contains(i))
                        {
                            Game1.Procedural.AllTilingContainers[(GenerationType)generationIndex].GeneratableTiles.Add(i);
                        }
                    }
                    // throw new Exception("GenerationType value not supported. Check to make sure the tiled value of generate property is supported. Capitals matter.");



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
            this.ChunkPointUnderPlayer = ChunkPositions[0, 0];
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


            int x = (RenderDistance / 2) * -1;
            int y = (RenderDistance / 2) * -1;

            int numUnrecycledChunks = 0;
            for (int i = 0; i < RenderDistance; i++)
            {
                for (int j = 0; j < RenderDistance; j++)
                {
                    bool hasChunkBeenFilled = false;
                    for (int b = 0; b < RenderDistance;b++)
                    {
                        
                        for (int g = 0; g < RenderDistance; g++)
                        {
                            if (ActiveChunks[b, g] != null)
                            {


                                if (ActiveChunks[b, g].X == currentChunkX + x && ActiveChunks[b, g].Y == currentChunkY + y)
                                {
                                    ActiveChunks[i, j] = ActiveChunks[b, g];
                                    hasChunkBeenFilled = true;
                                    break;
                                }
                                if (hasChunkBeenFilled)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    if(!hasChunkBeenFilled)
                    {
                        ActiveChunks[i, j] = new Chunk(this, currentChunkX + x, currentChunkY + y, i, j);
                        numUnrecycledChunks++;
                    }
                            
                    y++;
                }
                y = (RenderDistance / 2) * -1;
                x++;
            }
            return ActiveChunks;


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
            lock (this.ChunkLocker)
            {


                switch (direction)
                {

                    case Dir.Down:
                        //shifts everything down one
                        for (int i = 0; i < RenderDistance; i++)
                        {
                            for (int j = 0; j < RenderDistance; j++)
                            {
                                if (j == RenderDistance - 1) // if J index is at the bottom and will become a new chunk
                                {
                                    this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[i, j].X,
                                        ChunkPositions[i, j].Y, i, j);
                                    ChunkCheck(this.ActiveChunks[i, j]);
                                }
                                else
                                {
                                    if (j == 0) //Chunk is in top row (we are moving down) so it will be saved and removed from memory.
                                    {
                                        SaveAndDiscardChunk(this.ActiveChunks[i, j]);
                                    }
                                    this.ActiveChunks[i, j] = this.ActiveChunks[i, j + 1]; //the new chunk on the top row will be the one which was originally below it.
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
                                if (j == 0) // We are moving up. Top row chunks will get new chunks.
                                {
                                    this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[i, j].X,
                                         ChunkPositions[i, j].Y, i, j);
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
                                    if (j >= 0)
                                    {
                                        this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[i, j].X,
                                        ChunkPositions[i, j].Y, i, j);
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
                    case Dir.Right:
                        for (int i = 0; i < RenderDistance; i++)
                        {
                            for (int j = 0; j < RenderDistance; j++)
                            {
                                if (i == RenderDistance - 1)
                                {
                                    if (j >= 0)
                                    {
                                        this.ActiveChunks[i, j] = new Chunk(this, ChunkPositions[i, j].X,
                                         ChunkPositions[i, j].Y, i, j);
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
            //int i = 0;
            int posX = rootPosition.X - RenderDistance / 2;
            int posY = rootPosition.Y - RenderDistance / 2;

            for (int i = 0; i < ChunkPositions.GetLength(0); i++)
            {
                for (int j = 0; j < ChunkPositions.GetLength(1); j++)
                {
                    ChunkPositions[i, j] = new Point(posX, posY);
                    posY++;
                }
                posX++;
                posY = rootPosition.Y - RenderDistance / 2;
            }
        }

        public void HandleNewChunkMove()
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
        public void Update(GameTime gameTime, MouseManager mouse)
        {
            GetProperArrayData();

            this.ChunkPointUnderPlayer = ChunkPositions[0, 0];

            if (this.ChunkPointUnderPlayerLastFrame != this.ChunkPointUnderPlayer)
            {
                HandleNewChunkMove();
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
                            UpdateAnimationFrames(gameTime, currentChunk);
                            
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
            PlayerI = (int)((Game1.Player.ColliderRectangle.X + 4) / 16 - (this.ChunkUnderPlayer.X * 16));
            PlayerJ = (int)(Game1.Player.ColliderRectangle.Y / 16 - (this.ChunkUnderPlayer.Y * 16));
            if (PlayerI > 15)
            {
                PlayerI = 15;
            }
            if (PlayerJ > 15)
            {
                PlayerJ = 15;
            }

            int inverseLayer = 4;
            for (int z = 0; z < 4; z++)
            {
                inverseLayer--;
                if (Game1.Player.IsMoving)
                {
                    if (z == 0)
                    {
                        if (PlayerI < this.ChunkUnderPlayer.AllTiles[z].GetLength(0) &&
                            PlayerJ < this.ChunkUnderPlayer.AllTiles[z].GetLength(1) &&
                            PlayerI >= 0 &&
                            PlayerJ >= 0 &&
                             OldPlayerI >= 0
                            && OldPlayerJ >= 0 &&

                             this.ChunkUnderPlayer.AllTiles[z][PlayerI, PlayerJ] != null)
                        {
                            if (OldPlayerI != PlayerI || OldPlayerJ != PlayerJ)
                            {
                                GridStatus oldGridStatus = GridStatus.Clear;
                                for(int layerz = 0; layerz < 4; layerz++)
                                {
                                    if (this.ChunkUnderPlayerLastFrame.AllTiles[layerz][OldPlayerI, OldPlayerJ]!= null && this.ChunkUnderPlayerLastFrame.Objects.ContainsKey(this.ChunkUnderPlayerLastFrame.AllTiles[layerz][OldPlayerI, OldPlayerJ].TileKey))
                                    {
                                        oldGridStatus = GridStatus.Obstructed;
                                    }
                                }
                                

                                this.ChunkUnderPlayerLastFrame.PathGrid.UpdateGrid(OldPlayerI, OldPlayerJ, oldGridStatus);

                            }

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
                    Tile tileUnderMouse = this.ChunkUnderMouse.AllTiles[inverseLayer][mouseI, mouseJ];
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



                                        Game1.MouseManager.ToggleGeneralInteraction = true;

                                        if (mouse.IsClicked)
                                        {
                                            TileUtility.InteractWithDestructableTile(inverseLayer, gameTime, mouseI, mouseJ, tileUnderMouse.DestinationRectangle, this.ChunkUnderMouse);

                                        }

                                    }


                                    if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(tileUnderMouse.GID))
                                    {
                                        if (this.MapName.Tilesets[this.TileSetNumber].Tiles[tileUnderMouse.GID].Properties.ContainsKey("action"))
                                        {

                                            TileUtility.ActionHelper(inverseLayer, mouseI, mouseJ, this.MapName.Tilesets[this.TileSetNumber].Tiles[tileUnderMouse.GID].Properties["action"], mouse, this.ChunkUnderMouse);

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
                        int startI = ChunkUtility.CheckArrayLimits((Game1.cam.CameraScreenRectangle.X - 48) / 16 - chunk.X * 16);
                        int endI = ChunkUtility.CheckArrayLimits(Game1.cam.CameraScreenRectangle.X / 16 + (Game1.cam.CameraScreenRectangle.Width + 32) / 16 - chunk.X * 16);

                        int startY = ChunkUtility.CheckArrayLimits((Game1.cam.CameraScreenRectangle.Y - 48) / 16 - chunk.Y * 16);
                        int endY = ChunkUtility.CheckArrayLimits(Game1.cam.CameraScreenRectangle.Y / 16 + (Game1.cam.CameraScreenRectangle.Height + 128) / 16 - chunk.Y * 16);

                        if (Game1.GetCurrentStage().ShowBorders)
                        {
                            spriteBatch.Draw(this.ChunkTexture, new Vector2(chunk.GetChunkRectangle().X, chunk.GetChunkRectangle().Y), color: Color.White, layerDepth: 1f);
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
                                    if (tile.GID != -1)
                                    {

                                        if (z == 3)
                                        {

                                            spriteBatch.Draw(this.TileSet, tile.Position, tile.SourceRectangle, Color.White * tile.ColorMultiplier,
                                            0f, Game1.Utility.Origin, 1f, SpriteEffects.None, this.AllDepths[z] + tile.LayerToDrawAtZOffSet);

                                        }
                                        else
                                        {
                                            spriteBatch.Draw(this.TileSet, tile.Position, tile.SourceRectangle, Color.White,
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
                        foreach (KeyValuePair<string, Sprite> sprite in ActiveChunks[a,b].QuestIcons)
                        {
                            sprite.Value.Draw(spriteBatch, .85f);
                        }

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

        public void UpdateAnimationFrames(GameTime gameTime, Chunk currentChunk)
        {
            List<string> AnimationFrameKeysToRemove = new List<string>();
            foreach (EditableAnimationFrameHolder frameholder in currentChunk.AnimationFrames.Values)
            {
                frameholder.Frames[frameholder.Counter].CurrentDuration -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameholder.Frames[frameholder.Counter].CurrentDuration <= 0)
                {
                    frameholder.Frames[frameholder.Counter].CurrentDuration = frameholder.Frames[frameholder.Counter].AnchorDuration;
                    Tile animationTile = currentChunk.AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY];
                    animationTile.SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(frameholder.Frames[frameholder.Counter].ID, 100);
                    if (frameholder.HasNewSource)
                    {
                        Rectangle newSourceRectangle = animationTile.SourceRectangle;
                        animationTile.SourceRectangle = new Rectangle(newSourceRectangle.X + frameholder.OriginalXOffSet,
                            newSourceRectangle.Y + frameholder.OriginalYOffSet, frameholder.OriginalWidth, frameholder.OriginalHeight);
                    }
                    if (frameholder.Counter == frameholder.Frames.Count - 1)
                    {
                        if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(frameholder.OriginalTileID))
                        {

                            if (this.TileSetDictionary[frameholder.OriginalTileID].Properties.ContainsKey("destructable"))
                            {

                                AnimationFrameKeysToRemove.Add(animationTile.TileKey);

                                TileUtility.FinalizeTile(frameholder.Layer, gameTime, frameholder.OldX, frameholder.OldY, currentChunk);

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
        }

        public void LoadInitialTileObjects(ILocation location)
        {
            throw new NotImplementedException();
        }



        public void UpdateCropTile()
        {
            if (this.Stage == Game1.OverWorld)
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

        public void Save(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void Load(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public void Unload()
        {
          
        }

        public Texture2D SetChunkTexture(GraphicsDevice graphicsDevice)
        {

                Rectangle chunkRectangle = new Rectangle(0,0, 256,256);
                var Colors = new List<Color>();
                for (int y = 0; y < chunkRectangle.Height; y++)
                {
                    for (int x = 0; x < chunkRectangle.Width; x++)
                    {
                        if (x == 0 || //left side
                            y == 0 || //top side
                            x == chunkRectangle.Width - 1 || //right side
                            y == chunkRectangle.Height - 1) //bottom side
                        {
                            Colors.Add(new Color(255, 255, 255, 255));
                        }
                        else
                        {
                            Colors.Add(new Color(0, 0, 0, 0));

                        }

                    }
                }
                Texture2D texture =  new Texture2D(graphicsDevice, chunkRectangle.Width, chunkRectangle.Height);
                texture.SetData<Color>(Colors.ToArray());
            return texture;
            
        }
    }
}
