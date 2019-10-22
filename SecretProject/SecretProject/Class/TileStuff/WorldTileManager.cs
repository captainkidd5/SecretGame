using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.NPCStuff.Enemies;
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
        public Chunk ChunkUnderMouse { get; set; }
        public Dictionary<string, List<GrassTuft>> Tufts { get; set; }
        public Dictionary<string, ICollidable> Objects { get; set; }
        public Dictionary<string, EditableAnimationFrameHolder> AnimationFrames { get; set; }
        public Dictionary<string, int> TileHitPoints { get; set; }
        public Dictionary<string, Chest> Chests { get; set; }
        public List<LightSource> Lights { get; set; }
        public Dictionary<string, ICollidable> CurrentObjects { get; set; }
        public int TileSetNumber { get; set; }
        public bool AbleToDrawTileSelector { get; set; }

        public bool DrawGridObject { get; set; }
        public Rectangle GridObjectSourceRectangle { get; set; }
        public int GridObjectSourceRectangleOffSetX { get; set; }
        public int GridObjectSourceRectangleOffSetY { get; set; }
        public Color GridDrawColor { get; set; }

        public List<int> DirtGeneratableTiles;
        public List<int> SandGeneratableTiles;
        public List<int> GrassGeneratableTiles;

        public Dictionary<string, Crop> Crops { get; set; }

        public Rectangle ScreenRectangle { get; set; }

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

            Chests = new Dictionary<string, Chest>();
            Lights = new List<LightSource>();

            CurrentObjects = new Dictionary<string, ICollidable>();

            this.ChunkUnderPlayer = new Chunk(this, 0, 0, 1, 1);
            this.ChunkUnderMouse = new Chunk(this, 0, 0, 1, 1);

            Game1.GlobalClock.DayChanged += this.HandleClockChange;
            this.GridDrawColor = Color.White;
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
                            case "water":
                                if (!Game1.Utility.WaterGeneratableTiles.Contains(i))
                                {
                                    Game1.Utility.WaterGeneratableTiles.Add(i);
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

            ActiveChunks = GetActiveChunkCoord(new Vector2(0, 0));


            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!ActiveChunks[i, j].IsLoaded)
                    {
                        if (TileUtility.CheckIfChunkExistsInMemory(ActiveChunks[i, j].X, ActiveChunks[i, j].Y))
                        {
                            ActiveChunks[i, j].Load();
                        }
                        else
                        {
                            ActiveChunks[i, j].Generate(0);

                        }
                        ActiveChunks[i, j].Save();
                    }
                }
            }

            ChunkUnderPlayer = ActiveChunks[1, 1];
            ChunkPointUnderPlayer = new Point((int)(Game1.Player.Position.X / 16 / TileUtility.ChunkX), (int)(Game1.Player.Position.Y / 16 / TileUtility.ChunkY));
            ChunkPointUnderPlayerLastFrame = ChunkPointUnderPlayer;
        }


        public Chunk[,] GetActiveChunkCoord(Vector2 playerPos)
        {

            int currentChunkX = (int)(playerPos.X / 16 / TileUtility.ChunkX);
            int currentChunkY = (int)(playerPos.Y / 16 / TileUtility.ChunkY);

            Chunk[,] ChunksToReturn = new Chunk[3, 3];
            int x = -1;
            int y = -1;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    ChunksToReturn[i, j] = new Chunk(this, currentChunkX + x, currentChunkY + y, i, j);
                    y++;
                }
                y = -1;
                x++;
            }
            return ChunksToReturn;


        }

        public Chunk GetChunkFromPosition(Vector2 entityPosition)
        {
            Point ChunkUnderEntity = new Point((int)(entityPosition.X / 16 / TileUtility.ChunkX), (int)(entityPosition.Y / 16 / TileUtility.ChunkY));
            return ActiveChunks[ChunkUnderEntity.X, ChunkUnderEntity.Y];


        }

        public void UpdateGrid(Dir directionPlayerMoved)
        {
            int currentChunkX = ActiveChunks[1, 1].X;
            int currentChunkY = ActiveChunks[1, 1].Y;
            switch (directionPlayerMoved)
            {
                case Dir.Down:


                    ActiveChunks[0, 0] = ActiveChunks[0, 1];
                    ActiveChunks[1, 0] = ActiveChunks[1, 1];
                    ActiveChunks[2, 0] = ActiveChunks[2, 1];

                    ActiveChunks[0, 1] = ActiveChunks[0, 2];
                    ActiveChunks[1, 1] = ActiveChunks[1, 2];
                    ActiveChunks[2, 1] = ActiveChunks[2, 2];

                    for (int i = 0; i < 3; i++)
                    {
                        ActiveChunks[i, 2] = new Chunk(this, currentChunkX - 1 + i, currentChunkY + 2, i, 2);
                        if (TileUtility.CheckIfChunkExistsInMemory(ActiveChunks[i, 2].X, ActiveChunks[i, 2].Y))
                        {
                            ActiveChunks[i, 2].Load();
                        }
                        else
                        {
                            ActiveChunks[i, 2].Generate(0);
                            ActiveChunks[i, 2].Save();
                        }
                    }

                    break;
                //correct
                case Dir.Up:

                    ActiveChunks[0, 2] = ActiveChunks[0, 1];
                    ActiveChunks[1, 2] = ActiveChunks[1, 1];
                    ActiveChunks[2, 2] = ActiveChunks[2, 1];

                    ActiveChunks[0, 1] = ActiveChunks[0, 0];
                    ActiveChunks[1, 1] = ActiveChunks[1, 0];
                    ActiveChunks[2, 1] = ActiveChunks[2, 0];

                    for (int i = 0; i < 3; i++)
                    {
                        ActiveChunks[i, 0] = new Chunk(this, currentChunkX - 1 + i, currentChunkY - 2, i, 0);
                        if (TileUtility.CheckIfChunkExistsInMemory(ActiveChunks[i, 0].X, ActiveChunks[i, 0].Y))
                        {
                            ActiveChunks[i, 0].Load();
                        }
                        else
                        {
                            ActiveChunks[i, 0].Generate(0);
                            ActiveChunks[i, 0].Save();
                        }
                    }

                    break;

                case Dir.Left:

                    ActiveChunks[2, 0] = ActiveChunks[1, 0];
                    ActiveChunks[2, 1] = ActiveChunks[1, 1];
                    ActiveChunks[2, 2] = ActiveChunks[1, 2];

                    ActiveChunks[1, 0] = ActiveChunks[0, 0];
                    ActiveChunks[1, 1] = ActiveChunks[0, 1];
                    ActiveChunks[1, 2] = ActiveChunks[0, 2];

                    for (int i = 0; i < 3; i++)
                    {
                        ActiveChunks[0, i] = new Chunk(this, currentChunkX - 2, currentChunkY - 1 + i, 0, i);
                        if (TileUtility.CheckIfChunkExistsInMemory(ActiveChunks[0, i].X, ActiveChunks[0, i].Y))
                        {
                            ActiveChunks[0, i].Load();
                        }
                        else
                        {
                            ActiveChunks[0, i].Generate(0);
                            ActiveChunks[0, i].Save();
                        }
                    }

                    break;

                case Dir.Right:

                    ActiveChunks[0, 0] = ActiveChunks[1, 0];
                    ActiveChunks[0, 1] = ActiveChunks[1, 1];
                    ActiveChunks[0, 2] = ActiveChunks[1, 2];

                    ActiveChunks[1, 0] = ActiveChunks[2, 0];
                    ActiveChunks[1, 1] = ActiveChunks[2, 1];
                    ActiveChunks[1, 2] = ActiveChunks[2, 2];

                    for (int i = 0; i < 3; i++)
                    {
                        ActiveChunks[2, i] = new Chunk(this, currentChunkX + 2, currentChunkY - 1 + i, 2, i);
                        if (TileUtility.CheckIfChunkExistsInMemory(ActiveChunks[2, i].X, ActiveChunks[2, i].Y))
                        {
                            ActiveChunks[2, i].Load();
                        }
                        else
                        {
                            ActiveChunks[2, i].Generate(0);
                            ActiveChunks[2, i].Save();
                        }
                    }

                    break;
            }

        }


        public void Update(GameTime gameTime, MouseManager mouse)
        {
            ChunkPointUnderPlayer = new Point((int)Math.Floor(Game1.Player.Position.X / 16 / TileUtility.ChunkX), (int)Math.Floor(Game1.Player.Position.Y / 16 / TileUtility.ChunkY));

            if (ChunkPointUnderPlayerLastFrame != ChunkPointUnderPlayer)
            {
                // ActiveChunks[1, 1].X = ChunkPointUnderPlayer.X;
                // ActiveChunks[1, 1].Y = ChunkPointUnderPlayer.Y;
                if (ChunkPointUnderPlayer.X > ChunkPointUnderPlayerLastFrame.X)
                {
                    UpdateGrid(Dir.Right);
                }
                else if (ChunkPointUnderPlayer.X < ChunkPointUnderPlayerLastFrame.X)
                {
                    UpdateGrid(Dir.Left);
                }
                else if (ChunkPointUnderPlayer.Y > ChunkPointUnderPlayerLastFrame.Y)
                {
                    UpdateGrid(Dir.Down);
                }
                else if (ChunkPointUnderPlayer.Y < ChunkPointUnderPlayerLastFrame.Y)
                {
                    UpdateGrid(Dir.Up);
                }





            }
            ChunkUnderPlayer = ActiveChunks[1, 1];
            this.Chests = ChunkUnderPlayer.Chests;
            this.CurrentObjects = ChunkUnderPlayer.Objects;
            ChunkPointUnderPlayerLastFrame = ChunkPointUnderPlayer;


            int starti = (int)(Game1.cam.Pos.X) - (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2) - 1;

            int startj = (int)(Game1.cam.Pos.Y) - (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2) - 1;

            int endi = (int)(Game1.ScreenWidth / Game1.cam.Zoom);

            int endj = (int)(Game1.ScreenHeight / Game1.cam.Zoom + 64);

            this.ScreenRectangle = new Rectangle(starti, startj, endi, endj);

            for (int a = 0; a < ActiveChunks.GetLength(0); a++)
            {
                for (int b = 0; b < ActiveChunks.GetLength(1); b++)
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
                                TileUtility.ReplaceTile(frameholder.Layer, frameholder.OldX, frameholder.OldY, frameholder.Frames[frameholder.Counter].ID + 1, ActiveChunks[a, b], false);
                                if (frameholder.Counter == frameholder.Frames.Count - 1)
                                {
                                    if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(frameholder.OriginalTileID))
                                    {

                                        if (MapName.Tilesets[TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("destructable") || MapName.Tilesets[TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("relationX"))
                                        {

                                            //needs to refer to first tile ?
                                            int frameolDX = frameholder.OldX;
                                            TileUtility.ReplaceTile(frameholder.Layer, frameholder.OldX, frameholder.OldY, frameholder.OriginalTileID + 1, ActiveChunks[a, b], false);
                                            AnimationFrameKeysToRemove.Add(ActiveChunks[a, b].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].GetTileKey(frameholder.Layer));
                                            if (MapName.Tilesets[TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("destructable"))
                                            {
                                                // Rectangle testDestinationRectangle = TileUtility.GetDestinationRectangle(ActiveChunks[a,b].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY]);
                                                TileUtility.FinalizeTile(frameholder.Layer, gameTime, frameholder.OldX, frameholder.OldY, TileUtility.GetDestinationRectangle(ActiveChunks[a, b].AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY]), Game1.GetCurrentStage(), ActiveChunks[a, b]);
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
                        for (int z = 0; z < 4; z++)
                        {
                            for (int i = 0; i < TileUtility.ChunkX; i++)
                            {
                                for (int j = 0; j < TileUtility.ChunkY; j++)
                                {
                                    if (ActiveChunks[a, b].AllTiles[z][i, j].GID != -1)
                                    {

                                        string TileKey = ActiveChunks[a, b].AllTiles[z][i, j].GetTileKey(z);

                                        if (z == 0)
                                        {
                                            //if (ActiveChunks[a, b].Tufts.ContainsKey(TileKey))
                                            //{
                                            //    for (int t = 0; t < ActiveChunks[a, b].Tufts[TileKey].Count; t++)
                                            //    {
                                            //        ActiveChunks[a, b].Tufts[TileKey][t].Update(gameTime, Game1.Player.controls.Direction);
                                            //    }
                                            //}
                                        }


                                        if (ActiveChunks[a, b].AllTiles[z][i, j].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle))
                                        {

                                            if (mouse.IsHoveringTile(ActiveChunks[a, b].AllTiles[z][i, j].DestinationRectangle))
                                            {
                                                this.AbleToDrawTileSelector = true;
                                                Game1.Player.UserInterface.TileSelector.IndexX = i;
                                                Game1.Player.UserInterface.TileSelector.IndexY = j;
                                                Game1.Player.UserInterface.TileSelector.WorldX = ActiveChunks[a, b].X * 16 * 16 + i * 16;
                                                Game1.Player.UserInterface.TileSelector.WorldY = ActiveChunks[a, b].Y * 16 * 16 + j * 16;
                                                //CurrentIndexX = i;
                                                //CurrentIndexY = j;

                                                if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(ActiveChunks[a, b].AllTiles[z][i, j].GID))
                                                {

                                                    if (z == 1)
                                                    {

                                                        if (MapName.Tilesets[TileSetNumber].Tiles[ActiveChunks[a, b].AllTiles[z][i, j].GID].Properties.ContainsKey("destructable"))
                                                        {
                                                            Game1.Player.UserInterface.DrawTileSelector = true;
                                                            Game1.isMyMouseVisible = false;


                                                            mouse.ChangeMouseTexture(((CursorType)Game1.Utility.GetRequiredTileTool(MapName.Tilesets[TileSetNumber].Tiles[ActiveChunks[a, b].AllTiles[z][i, j].GID].Properties["destructable"])));

                                                            Game1.myMouseManager.ToggleGeneralInteraction = true;

                                                            if (mouse.IsClicked)
                                                            {
                                                                TileUtility.InteractWithBuilding(z, gameTime, i, j, ActiveChunks[a, b].AllTiles[z][i, j].DestinationRectangle, Game1.GetCurrentStage(), ActiveChunks[a, b]);

                                                            }

                                                        }

                                                    }
                                                    if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(ActiveChunks[a, b].AllTiles[z][i, j].GID))
                                                    {
                                                        if (MapName.Tilesets[TileSetNumber].Tiles[ActiveChunks[a, b].AllTiles[z][i, j].GID].Properties.ContainsKey("action"))
                                                        {

                                                            TileUtility.ActionHelper(z, i, j, MapName.Tilesets[TileSetNumber].Tiles[ActiveChunks[a, b].AllTiles[z][i, j].GID].Properties["action"], mouse, ActiveChunks[a, b]);

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


                    foreach (Boar boar in ActiveChunks[a, b].Enemies)
                    {
                        // boar.Update(gameTime, ActiveChunks[a, b].Objects, mouse, ActiveChunks[a, b]);
                    }
                }
            }

           
            TileUtility.UpdateGridItem(this, ChunkUnderMouse);
        }





        public void DrawTiles(SpriteBatch spriteBatch)
        {



            for (int a = 0; a < ActiveChunks.GetLength(0); a++)
            {
                for (int b = 0; b < ActiveChunks.GetLength(1); b++)
                {
                    if (ScreenRectangle.Intersects(ActiveChunks[a, b].GetChunkRectangle()))
                    {
                        if(Game1.GetCurrentStage().ShowBorders)
                        {
                            spriteBatch.Draw(ActiveChunks[a, b].RectangleTexture, new Vector2(ActiveChunks[a, b].GetChunkRectangle().X, ActiveChunks[a, b].GetChunkRectangle().Y), color: Color.White, layerDepth: 1f);
                        }
                        for (int z = 0; z < 4; z++)
                        {
                            for (int i = 0; i < TileUtility.ChunkX; i++)
                            {
                                for (int j = 0; j < TileUtility.ChunkY; j++)
                                {
                                    if (ActiveChunks[a, b].AllTiles[z][i, j].GID != -1)
                                    {

                                        if (z == 0)
                                        {
                                            if (ActiveChunks[a, b].Tufts.ContainsKey(ActiveChunks[a, b].AllTiles[z][i, j].GetTileKey(0)))
                                            {
                                                for (int t = 0; t < ActiveChunks[a, b].Tufts[ActiveChunks[a, b].AllTiles[z][i, j].GetTileKey(0)].Count; t++)
                                                {
                                                    ActiveChunks[a, b].Tufts[ActiveChunks[a, b].AllTiles[z][i, j].GetTileKey(0)][t].Draw(spriteBatch);
                                                }
                                            }
                                        }
                                        
                                        if (z == 3)
                                        {
                                            spriteBatch.Draw(TileSet, new Vector2(ActiveChunks[a, b].AllTiles[z][i, j].DestinationRectangle.X, ActiveChunks[a, b].AllTiles[z][i, j].DestinationRectangle.Y), ActiveChunks[a, b].AllTiles[z][i, j].SourceRectangle, Game1.GlobalClock.TimeOfDayColor,
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
                        if (this.DrawGridObject)
                        {
                            spriteBatch.Draw(TileSet, new Vector2(Game1.Player.UserInterface.TileSelector.WorldX + this.GridObjectSourceRectangleOffSetX, Game1.Player.UserInterface.TileSelector.WorldY + GridObjectSourceRectangleOffSetY), this.GridObjectSourceRectangle, this.GridDrawColor,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[3]);

                        }


                        foreach (Boar boar in ActiveChunks[a, b].Enemies)
                        {
                            boar.Draw(spriteBatch);
                            boar.DrawDebug(spriteBatch, .1f);
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

                        TileUtility.UpdateCropTile(ActiveChunks[i, j].Crops.ElementAt(x).Value, Game1.GetCurrentStage(), ActiveChunks[i, j]);
                    }
                }

            }
        }
        public void HandleClockChange(object sender, EventArgs eventArgs)
        {

            UpdateCropTile();
        }

        public AStarPathFinder GetPathGrid(Vector2 entityPosition)
        {

            Chunk ChunkCopy = GetChunkFromPosition(entityPosition);
            return new AStarPathFinder(TileUtility.ChunkX, TileUtility.ChunkY, ChunkCopy.AllTiles, ChunkCopy.Objects);

        }
    }
}
