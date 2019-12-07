using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CameraStuff;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.PathFinding;
using SecretProject.Class.PathFinding.PathFinder;
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
        public Dictionary<string, IStorableItem> StoreableItems { get; set; }
        public List<LightSource> Lights { get; set; }
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

            ActiveChunks = new Chunk[MaximumChunksLoaded, MaximumChunksLoaded];

            this.MaximumChunksLoaded = 3;
            ChunkPointUnderPlayerLastFrame = new Point(1000, 1000);

            DirtGeneratableTiles = new List<int>();
            SandGeneratableTiles = new List<int>();
            GrassGeneratableTiles = new List<int>();
            AnimationFrames = new Dictionary<string, EditableAnimationFrameHolder>();
            Tufts = new Dictionary<string, List<GrassTuft>>();
            TileHitPoints = new Dictionary<string, int>();
            CurrentObjects = new Dictionary<string, ICollidable>();

            StoreableItems = new Dictionary<string, IStorableItem>();
            Lights = new List<LightSource>();

            CurrentObjects = new Dictionary<string, ICollidable>();

            this.ChunkUnderPlayer = new Chunk(this, 0, 0, 1, 1);
            this.ChunkUnderMouse = new Chunk(this, 0, 0, 1, 1);

            Game1.GlobalClock.DayChanged += this.HandleClockChange;
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
                    if (this.MapName.Tilesets[this.TileSetNumber].Tiles[i].Properties.ContainsKey("generate"))
                    {
                        switch (this.MapName.Tilesets[this.TileSetNumber].Tiles[i].Properties["generate"])
                        {


                            case "dirt":
                                if (!Game1.Procedural.DirtGeneratableTiles.Contains(i))
                                {
                                    Game1.Procedural.DirtGeneratableTiles.Add(i);
                                }
                                break;
                            case "sand":
                                if (!Game1.Procedural.SandGeneratableTiles.Contains(i))
                                {
                                    Game1.Procedural.SandGeneratableTiles.Add(i);
                                }
                                break;
                            case "grass":
                                if (!Game1.Procedural.GrassGeneratableTiles.Contains(i))
                                {
                                    Game1.Procedural.GrassGeneratableTiles.Add(i);
                                }
                                break;
                            case "grassBasic":
                                if (!Game1.Procedural.GrassGeneratableTiles.Contains(i))
                                {
                                    Game1.Procedural.GrassGeneratableTiles.Add(i);
                                }
                                if (!Game1.Procedural.StandardGeneratableGrassTiles.Contains(i))
                                {
                                    Game1.Procedural.StandardGeneratableGrassTiles.Add(i);
                                }
                                break;
                            case "water":
                                if (!Game1.Procedural.WaterGeneratableTiles.Contains(i))
                                {
                                    Game1.Procedural.WaterGeneratableTiles.Add(i);
                                }
                                break;

                            case "stone":
                                if (!Game1.Procedural.StoneGeneratableTiles.Contains(i))
                                {
                                    Game1.Procedural.StoneGeneratableTiles.Add(i);
                                }
                                break;
                            case "dirtBasic":
                                if (!Game1.Procedural.DirtGeneratableTiles.Contains(i))
                                {
                                    Game1.Procedural.DirtGeneratableTiles.Add(i);
                                }
                                if (!Game1.Procedural.StandardGeneratableDirtTiles.Contains(i))
                                {
                                    Game1.Procedural.StandardGeneratableDirtTiles.Add(i);
                                }
                                break;
                            case "sandRuin":
                                if (!Game1.Procedural.SandRuinGeneratableTiles.Contains(i))
                                {
                                    Game1.Procedural.SandRuinGeneratableTiles.Add(i);
                                }
                                break;
                            case "fence":
                                if (!Game1.Procedural.FenceGeneratableTiles.Contains(i))
                                {
                                    Game1.Procedural.FenceGeneratableTiles.Add(i);
                                }
                                break;

                            case "dirtCliff":
                                if (!Game1.Procedural.DirtCliffGeneratableTiles.Contains(i))
                                {
                                    Game1.Procedural.DirtCliffGeneratableTiles.Add(i);
                                }
                                break;
                        }


                    }
                }
            }

            
        }

        public void LoadInitialChunks()
        {

            ActiveChunks = GetActiveChunkCoord(new Vector2(0, 0));


            for (int i = 0; i < RenderDistance; i++)
            {
                for (int j = 0; j < RenderDistance; j++)
                {
                    if (!ActiveChunks[i, j].IsLoaded)
                    {
                        if (Chunk.CheckIfChunkExistsInMemory(ActiveChunks[i, j].X, ActiveChunks[i, j].Y))
                        {
                            ActiveChunks[i, j].Load();
                        }
                        else
                        {
                            ActiveChunks[i, j].Generate();

                        }
                    }
                }
            }
            GetProperArrayData();
            ChunkUnderPlayer = ActiveChunks[RenderDistance/2, RenderDistance / 2];
            ChunkPointUnderPlayer = ChunkPositions[ChunkPositions.Length / 2 + 1];
            ChunkPointUnderPlayerLastFrame = ChunkPointUnderPlayer;
        }


        public Chunk[,] GetActiveChunkCoord(Vector2 playerPos)
        {

            int currentChunkX = (int)(playerPos.X / 16 / TileUtility.ChunkWidth);
            int currentChunkY = (int)(playerPos.Y / 16 / TileUtility.ChunkHeight);

            Chunk[,] ChunksToReturn = new Chunk[RenderDistance, RenderDistance];
            int x = (RenderDistance/2) * -1;
            int y = (RenderDistance / 2) * -1;
            for (int i = 0; i < RenderDistance; i++)
            {
                for (int j = 0; j < RenderDistance; j++)
                {
                    ChunksToReturn[i, j] = new Chunk(this, currentChunkX + x, currentChunkY + y, i, j);
                    y++;
                }
                y = (RenderDistance / 2) * -1 ;
                x++;
            }
            return ChunksToReturn;


        }

        public Chunk GetChunkFromPosition(Vector2 entityPosition)
        {
            Point ChunkUnderEntity = new Point((int)(entityPosition.X / 16 / TileUtility.ChunkWidth), (int)(entityPosition.Y / 16 / TileUtility.ChunkHeight));
            return ActiveChunks[ChunkUnderEntity.X, ChunkUnderEntity.Y];


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
                                ActiveChunks[i, j] = new Chunk(this, ChunkPositions[ChunkPositions.Length - RenderDistance + i].X - 1,
                                    ChunkPositions[ChunkPositions.Length - RenderDistance + i].Y - 1, i, j);
                                ChunkCheck( ActiveChunks[i, j]);
                            }
                            else
                            {

                                ActiveChunks[i, j] = ActiveChunks[i, j + 1];
                                ReassignArrayIAndJ(ActiveChunks[i, j], i, j);

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
                                ActiveChunks[i, j] = new Chunk(this, ChunkPositions[i].X - 1,
                                    ChunkPositions[i].Y - 1, i, j);
                                ChunkCheck( ActiveChunks[i, j]);
                            }
                            else
                            {

                                ActiveChunks[i, j] = ActiveChunks[i, j - 1];
                                ReassignArrayIAndJ(ActiveChunks[i, j], i, j);

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
                                    ActiveChunks[i, j] = new Chunk(this, ChunkPositions[0].X - 1,
                                    ChunkPositions[0].Y -1, i, j);
                                }
                                else if (j == 1)
                                {
                                    ActiveChunks[i, j] = new Chunk(this, ChunkPositions[5].X - 1,
                                    ChunkPositions[5].Y - 1, i, j);

                                }
                                else if (j == 2)
                                {
                                    ActiveChunks[i, j] = new Chunk(this, ChunkPositions[10].X - 1,
                                    ChunkPositions[10].Y - 1, i, j);
                                }
                                else if (j == 3)
                                {
                                    ActiveChunks[i, j] = new Chunk(this, ChunkPositions[15].X - 1,
                                    ChunkPositions[15].Y - 1, i, j);

                                }
                                else if (j == 4)
                                {
                                    ActiveChunks[i, j] = new Chunk(this, ChunkPositions[20].X - 1,
                                    ChunkPositions[20].Y - 1, i, j);
                                }

                                ChunkCheck( ActiveChunks[i, j]);
                            }

                            else
                            {

                                ActiveChunks[i, j] = ActiveChunks[i - 1, j];
                                ReassignArrayIAndJ(ActiveChunks[i, j], i, j);

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
                                    ActiveChunks[i, j] = new Chunk(this, ChunkPositions[4].X - 1,
                                    ChunkPositions[4].Y - 1, i, j);
                                }
                                else if (j == 1)
                                {
                                    ActiveChunks[i, j] = new Chunk(this, ChunkPositions[9].X - 1,
                                    ChunkPositions[9].Y - 1, i, j);

                                }
                                else if (j == 2)
                                {
                                    ActiveChunks[i, j] = new Chunk(this, ChunkPositions[14].X - 1,
                                    ChunkPositions[14].Y - 1, i, j);
                                }
                                else if (j == 3)
                                {
                                    ActiveChunks[i, j] = new Chunk(this, ChunkPositions[19].X - 1,
                                    ChunkPositions[19].Y - 1, i, j);
                                }
                                else if (j == 4)
                                {
                                    ActiveChunks[i, j] = new Chunk(this, ChunkPositions[24].X - 1,
                                    ChunkPositions[24].Y - 1, i, j);
                                }

                                ChunkCheck(ActiveChunks[i, j]);
                            }

                            else
                            {

                                ActiveChunks[i, j] = ActiveChunks[i + 1, j];
                                ReassignArrayIAndJ(ActiveChunks[i, j], i, j);

                            }
                        }
                    }
                    break;
            }


        }
        public void ReassignArrayIAndJ(Chunk chunk, int i, int j)
        {
            chunk.ArrayI = i;
            chunk.ArrayJ = j;
        }
        public void ChunkCheck( Chunk chunk)
        {
            if (Chunk.CheckIfChunkExistsInMemory(chunk.X, chunk.Y))
            {
                Task.Run(()=>chunk.Load());
            }
            else
            {
                Task.Run(()=>chunk.Generate());
               
            }
        }
        static readonly object locker = new object();

        public Point GetChunkPositionFromCamera(float x, float y)
        {
            int returnX = (int)(x / 16 / 16);
            int returnY = (int)(y / 16 / 16);
            if(x < 0)
            {
                returnX--;
            }
            if(y < 0)
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

            ChunkPointUnderPlayer = ChunkPositions[ChunkPositions.Length/2 + 1];

            if (ChunkPointUnderPlayerLastFrame != ChunkPointUnderPlayer)
            {


                if (ChunkPointUnderPlayer.X > ChunkPointUnderPlayerLastFrame.X)
                {
                    MoveChunks(Dir.Right);
                }
                else if (ChunkPointUnderPlayer.X < ChunkPointUnderPlayerLastFrame.X)
                {

                    MoveChunks(Dir.Left);

                }
                else if (ChunkPointUnderPlayer.Y > ChunkPointUnderPlayerLastFrame.Y)
                {

                    MoveChunks(Dir.Down);

                }
                else if (ChunkPointUnderPlayer.Y < ChunkPointUnderPlayerLastFrame.Y)
                {

                    MoveChunks(Dir.Up);

                }


            }
            ChunkUnderPlayer = ActiveChunks[RenderDistance/2, RenderDistance / 2];
            this.StoreableItems = ChunkUnderPlayer.StoreableItems;
            ChunkPointUnderPlayerLastFrame = ChunkPointUnderPlayer;



            int starti = (int)(Game1.cam.Pos.X) - (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2) - 1;

            int startj = (int)(Game1.cam.Pos.Y) - (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2) - 1;

            int endi = (int)(Game1.ScreenWidth / Game1.cam.Zoom);

            int endj = (int)(Game1.ScreenHeight / Game1.cam.Zoom + 64);

            this.ScreenRectangle = new Rectangle(starti, startj, endi, endj);

            for (int a = WorldTileManager.RenderDistance / 2 - 1; a < WorldTileManager.RenderDistance / 2 + 2; a++)
            {
                for (int b = WorldTileManager.RenderDistance / 2 - 1; b < WorldTileManager.RenderDistance / 2 + 2; b++)
                {
                    if (ActiveChunks[a, b].IsLoaded)
                    {


                        if (ActiveChunks[a, b].GetChunkRectangle().Intersects(this.ScreenRectangle))
                        {

                            List<string> AnimationFrameKeysToRemove = new List<string>();
                            foreach (EditableAnimationFrameHolder frameholder in ActiveChunks[a, b].AnimationFrames.Values)
                            {
                                frameholder.Frames[frameholder.Counter].CurrentDuration -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                                if (frameholder.Frames[frameholder.Counter].CurrentDuration <= 0)
                                {
                                    frameholder.Frames[frameholder.Counter].CurrentDuration = frameholder.Frames[frameholder.Counter].AnchorDuration;
                                    TileUtility.ReplaceTile(frameholder.Layer, frameholder.OldX, frameholder.OldY, frameholder.Frames[frameholder.Counter].ID + 1, ActiveChunks[a, b]);
                                    if (frameholder.Counter == frameholder.Frames.Count - 1)
                                    {
                                        if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(frameholder.OriginalTileID))
                                        {
                                            if (frameholder.SelfDestruct || MapName.Tilesets[TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("relationX"))
                                            {
                                                //needs to refer to first tile ?
                                                int frameolDX = frameholder.OldX;
                                                TileUtility.ReplaceTile(frameholder.Layer, frameholder.OldX, frameholder.OldY, frameholder.OriginalTileID + 1, ActiveChunks[a, b]);
                                                AnimationFrameKeysToRemove.Add(ActiveChunks[a, b].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].GetTileKeyStringNew(frameholder.Layer, ActiveChunks[a, b]));
                                                if (frameholder.SelfDestruct)
                                                {
                                                    TileUtility.FinalizeTile(frameholder.Layer, gameTime, frameholder.OldX, frameholder.OldY, TileUtility.GetDestinationRectangle(ActiveChunks[a, b].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY]), ActiveChunks[a, b]);
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
                                ActiveChunks[a, b].AnimationFrames.Remove(key);
                            }
                            Rectangle ActiveChunkRectangle = ActiveChunks[a, b].GetChunkRectangle();


                            if (mouse.IsHoveringTile(ActiveChunkRectangle))
                            {
                                ChunkUnderMouse = ActiveChunks[a, b];
                            }

                            int mouseI = (int)(Game1.myMouseManager.WorldMousePosition.X / 16 - (ChunkUnderMouse.X * 16));
                            int mouseJ = (int)(Game1.myMouseManager.WorldMousePosition.Y / 16 - (ChunkUnderMouse.Y * 16));
                            int playerI = (int)(Game1.Player.position.X / 16 - (ChunkUnderPlayer.X * 16));
                            int playerJ = (int)(Game1.Player.position.Y / 16 - (ChunkUnderPlayer.Y * 16));

                            for (int z = 0; z < 4; z++)
                            {
                                if (Game1.Player.IsMoving)
                                {
                                    if (z == 0)
                                    {
                                        if (playerI < ChunkUnderPlayer.AllTiles[z].GetLength(0) &&
                                            playerJ < ChunkUnderPlayer.AllTiles[z].GetLength(1) &&
                                            playerI >= 0 &&
                                            playerJ >= 0 &&
                                             ChunkUnderPlayer.AllTiles[z][playerI, playerJ] != null)
                                        {


                                            if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(ChunkUnderPlayer.AllTiles[z][playerI, playerJ].GID))
                                            {
                                                if (MapName.Tilesets[TileSetNumber].Tiles[ChunkUnderPlayer.AllTiles[z][playerI, playerJ].GID].Properties.ContainsKey("step"))
                                                {

                                                    Game1.Player.WalkSoundEffect = int.Parse(MapName.Tilesets[TileSetNumber].Tiles[ChunkUnderPlayer.AllTiles[z][playerI, playerJ].GID].Properties["step"]);
                                                }
                                            }
                                        }

                                    }
                                }
                                if (mouseI < 16 && mouseJ < 16 && mouseI >= 0 && mouseJ >= 0)
                                {


                                    if (ChunkUnderMouse.AllTiles[z][mouseI, mouseJ] != null)
                                    {


                                        if (ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GID != -1)
                                        {

                                            //int TileKey = ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GetTileKeyAsInt(z, ChunkUnderMouse);



                                            if (ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle))
                                            {


                                                Game1.Player.UserInterface.DrawTileSelector = true;
                                                Game1.Player.UserInterface.TileSelector.IndexX = mouseI;
                                                Game1.Player.UserInterface.TileSelector.IndexY = mouseJ;
                                                Game1.Player.UserInterface.TileSelector.WorldX = ChunkUnderMouse.X * 16 * 16 + mouseI * 16;
                                                Game1.Player.UserInterface.TileSelector.WorldY = ChunkUnderMouse.Y * 16 * 16 + mouseJ * 16;


                                                if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GID))
                                                {



                                                    if (MapName.Tilesets[TileSetNumber].Tiles[ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GID].Properties.ContainsKey("destructable"))
                                                    {

                                                        Game1.isMyMouseVisible = false;


                                                        mouse.ChangeMouseTexture(((CursorType)Game1.Utility.GetRequiredTileTool(MapName.Tilesets[TileSetNumber].Tiles[ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GID].Properties["destructable"])));

                                                        Game1.myMouseManager.ToggleGeneralInteraction = true;

                                                        if (mouse.IsClicked)
                                                        {
                                                            TileUtility.InteractWithDestructableTile(z, gameTime, mouseI, mouseJ, ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].DestinationRectangle,ChunkUnderMouse);

                                                        }

                                                    }


                                                    if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GID))
                                                    {
                                                        if (MapName.Tilesets[TileSetNumber].Tiles[ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GID].Properties.ContainsKey("action"))
                                                        {

                                                            TileUtility.ActionHelper(z, mouseI, mouseJ, MapName.Tilesets[TileSetNumber].Tiles[ChunkUnderMouse.AllTiles[z][mouseI, mouseJ].GID].Properties["action"], mouse, ChunkUnderMouse);

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
                        }

                        if (GridItem != null)
                        {
                            GridItem.ChunkUpdate(gameTime, this, ChunkUnderMouse);
                        }

                    }
                }
            }



        }





        public void DrawTiles(SpriteBatch spriteBatch)
        {



            for (int a = 0; a < RenderDistance; a++)
            {
                for (int b = 0; b < RenderDistance; b++)
                {
                    if (ActiveChunks[a, b].IsLoaded)
                    {


                        if (ScreenRectangle.Intersects(ActiveChunks[a, b].GetChunkRectangle()))
                        {
                            if (Game1.GetCurrentStage().ShowBorders)
                            {
                                spriteBatch.Draw(ActiveChunks[a, b].RectangleTexture, new Vector2(ActiveChunks[a, b].GetChunkRectangle().X, ActiveChunks[a, b].GetChunkRectangle().Y), color: Color.White, layerDepth: 1f);
                                spriteBatch.DrawString(Game1.AllTextures.MenuText, ActiveChunks[a, b].ArrayI.ToString() + ActiveChunks[a, b].ArrayJ.ToString(),
                                    new Vector2(ActiveChunks[a, b].GetChunkRectangle().X + 100, ActiveChunks[a, b].GetChunkRectangle().Y), Color.White, 0f, Game1.Utility.Origin, 5f, SpriteEffects.None, 1f);
                            }
                            for (int z = 0; z < 4; z++)
                            {
                                for (int i = 0; i < TileUtility.ChunkWidth; i++)
                                {
                                    for (int j = 0; j < TileUtility.ChunkHeight; j++)
                                    {
                                        if (ActiveChunks[a, b].AllTiles[z][i, j].GID != -1)
                                        {

                                            if (z == 3)
                                            {
                                                spriteBatch.Draw(TileSet, new Vector2(ActiveChunks[a, b].AllTiles[z][i, j].DestinationRectangle.X, ActiveChunks[a, b].AllTiles[z][i, j].DestinationRectangle.Y), ActiveChunks[a, b].AllTiles[z][i, j].SourceRectangle, Color.White,
                                                0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z] + ActiveChunks[a, b].AllTiles[z][i, j].LayerToDrawAtZOffSet);

                                            }
                                            else
                                            {
                                                spriteBatch.Draw(TileSet, new Vector2(ActiveChunks[a, b].AllTiles[z][i, j].DestinationRectangle.X, ActiveChunks[a, b].AllTiles[z][i, j].DestinationRectangle.Y), ActiveChunks[a, b].AllTiles[z][i, j].SourceRectangle, Color.White,
                                            0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z]);
                                            }

                                        }



                                    }
                                }
                            }
                            foreach (KeyValuePair<string, List<GrassTuft>> entry in ActiveChunks[a, b].Tufts)
                            {
                                for (int i = 0; i < entry.Value.Count; i++)
                                {

                                    entry.Value[i].Draw(spriteBatch);

                                }
                            }

                            if (GridItem != null)
                            {
                                GridItem.ChunkDraw(spriteBatch, this, ChunkUnderMouse);

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



        public void UpdateCropTile()
        {
            for (int i = 0; i < this.ActiveChunks.GetLength(0); i++)
            {
                for (int j = 0; j < this.ActiveChunks.GetLength(1); j++)
                {
                    for (int x = 0; x < ActiveChunks[i, j].Crops.Count; x++)
                    {
                        ActiveChunks[i, j].Crops.ElementAt(x).Value.CurrentGrowth = Game1.GlobalClock.TotalDays - ActiveChunks[i, j].Crops.ElementAt(x).Value.DayPlanted;

                        ActiveChunks[i, j].Crops.ElementAt(x).Value.UpdateGrowthCycle();

                        TileUtility.ReplaceTile(3, ActiveChunks[i, j].Crops.ElementAt(x).Value.X, ActiveChunks[i, j].Crops.ElementAt(x).Value.Y, ActiveChunks[i, j].Crops.ElementAt(x).Value.GID, ActiveChunks[i, j]);
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


            return PathGrid;

        }
    }
}
