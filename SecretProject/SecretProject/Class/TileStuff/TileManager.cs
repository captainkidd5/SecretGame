using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.PathFinding;
using SecretProject.Class.Playable;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using SecretProject.Class.TileStuff.SanctuaryStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TiledSharp;
using XMLData.ItemStuff;

namespace SecretProject.Class.TileStuff
{
    /// <summary>
    /// background = 0, buildings = 1, midground =2, foreground =3, 
    /// </summary>
    public class TileManager : ITileManager, IInformationContainer, ISaveable
    {
        public ILocation Stage { get; set; }
        public int Type { get; set; }
        protected Game1 game;
        public Texture2D TileSet { get; set; }
        public TmxMap MapName { get; set; }
        protected TmxLayer layerName;
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public int iD { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int tileNumber { get; set; }
        public int tileCounter { get; set; }
        public Tile[,] Tiles { get; set; }
        public bool isActive = false;
        public bool isInClickingRangeOfPlayer = false;
        public ContentManager Content { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public int ReplaceTileGid { get; set; }
        public Tile TempTile { get; set; }
        public int OldIndexX { get; set; }
        public int OldIndexY { get; set; }
        public float Depth { get; set; }
        public int LayerIdentifier { get; set; }
        public List<Tile[,]> AllTiles { get; set; }
        public List<float> AllDepths { get; set; }
        public bool TileInteraction { get; set; } = false;
        public Tile DebugTile { get; set; } = new Tile(40, 40, 4714);
        public int TileSetNumber { get; set; }
        public ObstacleGrid PathGrid { get; set; }

        public List<int> DirtGeneratableTiles;
        public List<int> SandGeneratableTiles;
        public List<int> GrassGeneratableTiles;
        public Dictionary<string, EditableAnimationFrameHolder> AnimationFrames { get; set; }
        public Dictionary<string, List<GrassTuft>> Tufts { get; set; }
        public Dictionary<string, int> TileHitPoints { get; set; }
        // public List<ICollidable> Objects { get; set; }
        public Dictionary<string, List<ICollidable>> Objects { get; set; }
        public Dictionary<string, IStorableItemBuilding> StoreableItems { get; set; }
        public List<LightSource> NightTimeLights { get; set; }
        public List<LightSource> DayTimeLights { get; set; }
        public Dictionary<float, string> ForeGroundOffSetDictionary { get; set; }

        public bool AbleToDrawTileSelector { get; set; }
        public GridItem GridItem { get; set; }

        public int TileSetDimension { get; set; }

        public Dictionary<string, Crop> Crops { get; set; }
        public List<Item> AllItems { get; set; }

        //not relevant:
        public int X { get; set; }
        public int Y { get; set; }
        public int NumberOfLayers { get; set; }
        public Chunk ChunkUnderPlayer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ArrayI { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ArrayJ { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Chunk[,] ActiveChunks { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ITileManager ITileManager { get; set; }
        public List<int[,]> AdjacentNoise { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Random Random { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<SPlot> AllPlots { get; set; }
        public bool WasModifiedDuringInterval { get; set; }

        public Dictionary<int, TmxTilesetTile> TileSetDictionary { get; set; }
        public Dictionary<string, Sprite> QuestIcons { get; set; }

        #region CONSTRUCTORS


        public TileManager(Texture2D tileSet, TmxMap mapName, GraphicsDevice graphicsDevice, ContentManager content, int tileSetNumber, ILocation currentStage)
        {
            this.Stage = currentStage;
            this.ITileManager = this;
            this.Type = 0;
            this.TileSet = tileSet;
            this.MapName = mapName;

            this.AllDepths = new List<float>()
            {
                .1f,
                .2f,
                .3f,
                .5f,
            };

            this.TileWidth = mapName.Tilesets[tileSetNumber].TileWidth;
            this.TileHeight = mapName.Tilesets[tileSetNumber].TileHeight;

            this.TileSetDimension = tileSet.Width / this.TileWidth;

            this.MapWidth = mapName.Width;
            this.MapHeight = mapName.Height;

            this.tileCounter = 0;

            this.GraphicsDevice = graphicsDevice;
            this.Content = content;


            this.AllTiles = new List<Tile[,]>();
            this.TileSetNumber = tileSetNumber;
            DirtGeneratableTiles = new List<int>();
            SandGeneratableTiles = new List<int>();
            GrassGeneratableTiles = new List<int>();
            this.AnimationFrames = new Dictionary<string, EditableAnimationFrameHolder>();
            this.Tufts = new Dictionary<string, List<GrassTuft>>();
            this.TileHitPoints = new Dictionary<string, int>();
            this.NightTimeLights = new List<LightSource>();
            this.DayTimeLights = new List<LightSource>();
            this.StoreableItems = new Dictionary<string, IStorableItemBuilding>();
            this.Crops = new Dictionary<string, Crop>();
            this.AllItems = new List<Item>();
            this.ForeGroundOffSetDictionary = new Dictionary<float, string>();
            this.TileSetDictionary = this.MapName.Tilesets[this.TileSetNumber].Tiles;
            Game1.GlobalClock.DayChanged += HandleClockChange;
            QuestIcons = new Dictionary<string, Sprite>();
            for (int i = 0; i < 4; i++)
            {
                this.AllTiles.Add(new Tile[mapName.Width, mapName.Height]);

            }
            this.Objects = new Dictionary<string, List<ICollidable>>();

            this.PathGrid = new ObstacleGrid(this.MapWidth, this.MapHeight);
            if (Game1.IsFirstTimeStartup)
            {
                StartNew(mapName);
            }

            #region PORTALS
            for (int i = 0; i < mapName.ObjectGroups["Portal"].Objects.Count; i++)
            {
                string keyFrom;
                string keyTo;
                string safteyX;
                string safteyY;
                string click;


                mapName.ObjectGroups["Portal"].Objects[i].Properties.TryGetValue("standardFrom", out keyFrom);
                mapName.ObjectGroups["Portal"].Objects[i].Properties.TryGetValue("standardTo", out keyTo);
                mapName.ObjectGroups["Portal"].Objects[i].Properties.TryGetValue("SafteyOffSetX", out safteyX);
                mapName.ObjectGroups["Portal"].Objects[i].Properties.TryGetValue("SafteyOffSetY", out safteyY);
                mapName.ObjectGroups["Portal"].Objects[i].Properties.TryGetValue("Click", out click);
                Portal portal = new Portal((int)Enum.Parse(typeof(Stages), keyFrom), (int)Enum.Parse(typeof(Stages), keyTo), int.Parse(safteyX), int.Parse(safteyY), bool.Parse(click));


                int portalX = (int)mapName.ObjectGroups["Portal"].Objects[i].X;
                int portalY = (int)mapName.ObjectGroups["Portal"].Objects[i].Y;
                int portalWidth = (int)mapName.ObjectGroups["Portal"].Objects[i].Width;
                int portalHeight = (int)mapName.ObjectGroups["Portal"].Objects[i].Height;

                portal.PortalStart = new Rectangle(portalX, portalY, portalWidth, portalHeight);

                currentStage.AllPortals.Add(portal);

                if (!Game1.PortalGraph.HasEdge(portal.From, portal.To))
                {
                    Game1.PortalGraph.AddEdge(portal.From, portal.To);
                }

            }
            #endregion

            currentStage.AllNightLights = this.NightTimeLights;
            currentStage.AllDayTimeLights = this.DayTimeLights;

        }

        /// <summary>
        /// Runs only when new save is started. Will not run on load game. See LoadNewSave for that.
        /// </summary>
        public void StartNew(TmxMap map)
        {
            List<TmxLayer> allLayers = new List<TmxLayer>()
            {
                map.Layers["background"],
            map.Layers["midGround"],
           map.Layers["buildings"],
           map.Layers["foreGround"]
        };

            for (int i = 0; i < this.AllTiles.Count; i++)
            {
                foreach (TmxLayerTile layerNameTile in allLayers[i].Tiles)
                {
                    Tile tempTile = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid) { LayerToDrawAt = i };

                    this.AllTiles[i][layerNameTile.X, layerNameTile.Y] = tempTile;

                }
            }
            for (int z = 0; z < this.AllTiles.Count; z++)
            {
                for (int i = 0; i < this.MapWidth; i++)
                {
                    for (int j = 0; j < this.MapHeight; j++)
                    {


                        TileUtility.AssignProperties(this.AllTiles[z][i, j], z, i, j, this);



                    }
                }
            }

        }

        public void AddItem(Item item, Vector2 position)
        {
            this.AllItems.Add(item);
        }
        public List<Item> GetItems(Vector2 position)
        {
            return AllItems;
        }


        #region UPDATE


        public void Update(GameTime gameTime, MouseManager mouse)
        {
            int oldLightCount = this.NightTimeLights.Count;
            this.AbleToDrawTileSelector = false;
            //Game1.myMouseManager.TogglePlantInteraction = false;
            Game1.Player.UserInterface.DrawTileSelector = false;
            List<string> AnimationFrameKeysToRemove = new List<string>();
            int starti = (int)(Game1.cam.Pos.X / 16) - (int)(Game1.ScreenWidth / Game1.GetCurrentStage().Cam.Zoom / 2 / 16) - 1;
            if (starti < 0)
            {
                starti = 0;
            }
            int startj = (int)(Game1.cam.Pos.Y / 16) - (int)(Game1.ScreenHeight / Game1.GetCurrentStage().Cam.Zoom / 2 / 16) - 1;
            if (startj < 0)
            {
                startj = 0;
            }
            int endi = (int)(Game1.cam.Pos.X / 16) + (int)(Game1.ScreenWidth / Game1.GetCurrentStage().Cam.Zoom / 2 / 16) + 2;
            if (endi > this.MapWidth)
            {
                endi = this.MapWidth;
            }
            int endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.GetCurrentStage().Cam.Zoom / 2 / 16) + 2;
            if (endj > this.MapWidth)
            {
                endj = this.MapWidth;
            }
            foreach (EditableAnimationFrameHolder frameholder in this.AnimationFrames.Values)
            {
                frameholder.Frames[frameholder.Counter].CurrentDuration -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameholder.Frames[frameholder.Counter].CurrentDuration <= 0)
                {
                    frameholder.Frames[frameholder.Counter].CurrentDuration = frameholder.Frames[frameholder.Counter].AnchorDuration;
                    this.AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(frameholder.Frames[frameholder.Counter].ID, 100);
                    if (frameholder.HasNewSource)
                    {
                        Rectangle newSourceRectangle = this.AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].SourceRectangle;
                        // Rectangle originalTileRectangle =
                        this.AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].SourceRectangle = new Rectangle(newSourceRectangle.X + frameholder.OriginalXOffSet, newSourceRectangle.Y + frameholder.OriginalYOffSet, frameholder.OriginalWidth, frameholder.OriginalHeight);
                    }


                    //TileUtility.ReplaceTile(frameholder.Layer, frameholder.OldX, frameholder.OldY, frameholder.Frames[frameholder.Counter].ID + 1, this);

                    if (frameholder.Counter == frameholder.Frames.Count - 1)
                    {
                        if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(frameholder.OriginalTileID))
                        {

                            if (this.MapName.Tilesets[this.TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("destructable") || this.MapName.Tilesets[this.TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("relationX"))
                            {

                                //needs to refer to first tile ?
                                int frameolDX = frameholder.OldX;
                                this.AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY] = new Tile(frameholder.OldX, frameholder.OldY, frameholder.OriginalTileID + 1);
                                AnimationFrameKeysToRemove.Add(this.AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].GetTileKeyStringNew(frameholder.Layer, this));
                                if (this.MapName.Tilesets[this.TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("destructable"))
                                {
                                    TileUtility.FinalizeTile(frameholder.Layer, gameTime, frameholder.OldX, frameholder.OldY, this);
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
                this.AnimationFrames.Remove(key);
            }


            Game1.Player.CollideOccured = false;
            int mouseI = (int)(Game1.MouseManager.WorldMousePosition.X / 16);
            int mouseJ = (int)(Game1.MouseManager.WorldMousePosition.Y / 16);
            int playerI = (int)((Game1.Player.Position.X + 16) / 16);
            int playerJ = (int)((Game1.Player.Position.Y + 16) / 16);


            for (int z = 0; z < this.AllTiles.Count; z++)
            {

                if (Game1.Player.IsMoving)
                {

                    if (playerI < this.AllTiles[z].GetLength(0) &&
                        playerJ < this.AllTiles[z].GetLength(1) &&
                        this.AllTiles[z][playerI, playerJ] != null)
                    {
                        //Prioritize midground layer for step sound effects
                        if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(this.AllTiles[1][playerI, playerJ].GID))
                        {
                            if (this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[1][playerI, playerJ].GID].Properties.ContainsKey("step"))
                            {

                                Game1.Player.WalkSoundEffect = int.Parse(this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[1][playerI, playerJ].GID].Properties["step"]);
                            }
                            else if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(this.AllTiles[0][playerI, playerJ].GID))
                            {
                                if (this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[0][playerI, playerJ].GID].Properties.ContainsKey("step"))
                                {

                                    Game1.Player.WalkSoundEffect = int.Parse(this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[0][playerI, playerJ].GID].Properties["step"]);
                                }
                            }
                        }
                        else if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(this.AllTiles[0][playerI, playerJ].GID))
                        {
                            if (this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[0][playerI, playerJ].GID].Properties.ContainsKey("step"))
                            {

                                Game1.Player.WalkSoundEffect = int.Parse(this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[0][playerI, playerJ].GID].Properties["step"]);
                            }
                        }

                    }


                }
                if (mouseI - 5 < 0)
                {

                }

                for (int mi = mouseI - 5; mi < mouseI + 5; mi++)
                {
                    for (int mj = mouseJ - 5; mj < mouseJ + 5; mj++)
                    {
                        if (mi >= 0 && mi < MapWidth && mj >= 0 && mj < MapWidth)
                        {




                            Rectangle newIntersectionRectangle = new Rectangle(this.AllTiles[z][mi, mj].DestinationRectangle.X,
                                this.AllTiles[z][mi, mj].DestinationRectangle.Y,
                                this.AllTiles[z][mi, mj].SourceRectangle.Width,
                                this.AllTiles[z][mi, mj].SourceRectangle.Height);
                            if (Game1.Player.ClickRangeRectangle.Intersects(newIntersectionRectangle))
                            {


                                //trees new source layer isnt the same tile as destructable, need to fix
                                if (Game1.MouseManager.WorldMouseRectangle.Intersects(newIntersectionRectangle))
                                {
                                    if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(this.AllTiles[z][mi, mj].GID))
                                    {


                                        if (this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[z][mi, mj].GID].Properties.ContainsKey("destructable"))
                                        {
                                            Game1.isMyMouseVisible = false;
                                            if (Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem() != null)
                                            {
                                                if ((AnimationType)(Game1.Player.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ItemType) == (AnimationType)Game1.Utility.GetRequiredTileTool(this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[z][mi, mj].GID].Properties["destructable"]))
                                                {
                                                    mouse.ChangeMouseTexture(((CursorType)Game1.Utility.GetRequiredTileTool(this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[z][mi, mj].GID].Properties["destructable"])));
                                                    Game1.MouseManager.ToggleGeneralInteraction = true;
                                                }
                                            }


                                            if (mouse.IsClicked)
                                            {
                                                TileUtility.InteractWithDestructableTile(z, gameTime, mi, mj, this.AllTiles[z][mi, mj].DestinationRectangle, this);

                                            }

                                        }
                                        //return;


                                        if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(this.AllTiles[z][mi, mj].GID))
                                        {
                                            if (this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[z][mi, mj].GID].Properties.ContainsKey("action"))
                                            {

                                                TileUtility.ActionHelper(z, mi, mj, this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[z][mi, mj].GID].Properties["action"], mouse, this);

                                            }
                                        }

                                    }
                                }
                            }

                        }
                    }
                }
                if (mouseI < this.AllTiles[z].GetLength(0) && mouseJ < this.AllTiles[z].GetLength(1) && mouseI >= 0 && mouseJ >= 0)
                {


                    if (this.AllTiles[z][mouseI, mouseJ] != null)
                    {
                        if (this.AllTiles[z][mouseI, mouseJ].GID != -1)
                        {
                            // int TileKey = this.AllTiles[z][mouseI, mouseJ].GetTileKeyAsInt(z, this);


                            if (this.AllTiles[z][mouseI, mouseJ].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle))
                            {

                                Game1.Player.UserInterface.DrawTileSelector = true;
                                Game1.Player.UserInterface.TileSelector.IndexX = mouseI;
                                Game1.Player.UserInterface.TileSelector.IndexY = mouseJ;
                                Game1.Player.UserInterface.TileSelector.WorldX = mouseI * 16;
                                Game1.Player.UserInterface.TileSelector.WorldY = mouseJ * 16;


                            }
                            else
                            {
                                // Game1.Player.UserInterface.DrawTileSelector = false;
                            }
                        }

                    }
                }


            }
            if (this.NightTimeLights.Count != oldLightCount)
            {
                Game1.GetCurrentStage().AllNightLights = this.NightTimeLights;
            }
            if (this.GridItem != null)
            {
                this.GridItem.NormalUpdate(gameTime, this, this);
            }

            for (int item = 0; item < this.AllItems.Count; item++)
            {
                this.AllItems[item].Update(gameTime);
            }

        }

        #endregion


        #region DRAW

        public void DrawTiles(SpriteBatch spriteBatch)
        {

            int starti = (int)(Game1.cam.Pos.X / 16) - (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2 / 16) - 1;
            if (starti < 0)
            {
                starti = 0;
            }
            int startj = (int)(Game1.cam.Pos.Y / 16) - (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2 / 16) - 1;
            if (startj < 0)
            {
                startj = 0;
            }
            int endi = (int)(Game1.cam.Pos.X / 16) + (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2 / 16) + 2;
            if (endi > this.MapWidth)
            {
                endi = this.MapWidth;
            }
            int endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2 / 16) + 5;
            if (endj > this.MapHeight)
            {
                endj = this.MapWidth;
            }
            if (startj < 0 || endj < 0 || starti < 0 || endi < 0 || endi > this.MapWidth || endj > this.MapHeight)
            {
                return;
            }
            foreach (KeyValuePair<string, List<GrassTuft>> entry in this.Tufts)
            {
                for (int grass = 0; grass < entry.Value.Count; grass++)
                {

                    entry.Value[grass].Draw(spriteBatch);

                }
            }
            for (int z = 0; z < this.AllTiles.Count; z++)
            {
                for (var i = starti; i < endi; i++)
                {
                    for (var j = startj; j < endj; j++)
                    {
                        if (this.AllTiles[z][i, j].GID != -1)
                        {





                            if (z == 3)
                            {
                                spriteBatch.Draw(this.TileSet, new Vector2(this.AllTiles[z][i, j].DestinationRectangle.X, this.AllTiles[z][i, j].DestinationRectangle.Y), this.AllTiles[z][i, j].SourceRectangle, Color.White * this.AllTiles[z][i, j].ColorMultiplier,
                                0f, Game1.Utility.Origin, 1f, SpriteEffects.None, this.AllDepths[z] + this.AllTiles[z][i, j].LayerToDrawAtZOffSet);

                            }
                            else
                            {
                                spriteBatch.Draw(this.TileSet, new Vector2(this.AllTiles[z][i, j].DestinationRectangle.X, this.AllTiles[z][i, j].DestinationRectangle.Y), this.AllTiles[z][i, j].SourceRectangle, Color.White,
                                0f, Game1.Utility.Origin, 1f, SpriteEffects.None, this.AllDepths[z]);
                            }

                            if (Game1.GetCurrentStage().ShowBorders)
                            {
                                spriteBatch.DrawString(Game1.AllTextures.MenuText, i + "," + j, new Vector2(this.AllTiles[z][i, j].DestinationRectangle.X, this.AllTiles[z][i, j].DestinationRectangle.Y), Color.White, 0f, Game1.Utility.Origin, .25f, SpriteEffects.None, 1f);
                                //spriteBatch.DrawString(font, text, fontLocation, tint, 0f, Game1.Utility.Origin, 1f,SpriteEffects.None, layerDepth: .73f);
                            }


                        }
                    }
                }
            }
            if (Game1.GetCurrentStageInt() == Stages.PlayerHouse && this.GridItem != null)
            {
                this.GridItem.NormalDraw(spriteBatch, this, this);
            }
            for (int item = 0; item < this.AllItems.Count; item++)
            {
                this.AllItems[item].Draw(spriteBatch);
            }



        }

        public void LoadGeneratableTileLists()
        {
            throw new NotImplementedException();
        }

        public void LoadInitialChunks(Vector2 position)
        {
            throw new NotImplementedException();
        }

        public Chunk GetChunkFromPosition(Vector2 entityPosition)
        {
            throw new NotImplementedException();
        }
        public void UpdateCropTile()
        {

            for (int x = 0; x < this.Crops.Count; x++)
            {
                Crop crop = this.Crops.ElementAt(x).Value;
                crop.CurrentGrowth = Game1.GlobalClock.TotalDays - crop.DayPlanted;
                if (crop.CurrentGrowth < this.MapName.Tilesets[this.TileSetNumber].Tiles[crop.BaseGID].AnimationFrames.Count)
                {
                    int newGid = this.MapName.Tilesets[this.TileSetNumber].Tiles[crop.BaseGID].AnimationFrames[crop.CurrentGrowth].Id + 1;
                    crop.UpdateGrowthCycle(newGid);

                    TileUtility.ReplaceTile(3, crop.X, crop.Y, crop.GID, this);
                }

            }


        }

        public void HandleClockChange(object sender, EventArgs eventArgs)
        {
            UpdateCropTile();
        }

        public Rectangle GetChunkRectangle()
        {
            throw new NotImplementedException();
        }

        public void SaveTiles()
        {

        }

        public void Unload()
        {
            this.AllTiles = null;
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(this.AllTiles.Count);
            writer.Write(this.MapWidth);
            for (int z = 0; z < this.AllTiles.Count; z++)
            {
                for (int i = 0; i < this.MapWidth; i++)
                {
                    for (int j = 0; j < this.MapHeight; j++)
                    {
                        writer.Write(this.AllTiles[z][i, j].GID + 1);
                    }
                }
            }
        }

        public void Load(BinaryReader reader)
        {
           
            int layerCount = reader.ReadInt32();
            int tileCount = reader.ReadInt32();

            this.AllTiles = new List<Tile[,]>();
            for (int i = 0; i < 4; i++)
            {
                this.AllTiles.Add(new Tile[tileCount, tileCount]);

            }
            for (int z = 0; z < layerCount; z++)
            {

                for (int i = 0; i < tileCount; i++)
                {
                    for (int j = 0; j < tileCount; j++)
                    {
                        this.AllTiles[z][i, j] = new Tile(i, j, reader.ReadInt32());
                    }
                }
            }
            if(this.Stage != Game1.Town)
            {
                for (int z = 0; z < layerCount; z++)
                {

                    for (int i = 0; i < tileCount; i++)
                    {
                        for (int j = 0; j < tileCount; j++)
                        {
                            TileUtility.AssignProperties(AllTiles[z][i, j], z, i, j, this);
                        }
                    }
                }
            }
            
        }

        #endregion

        #endregion

    }

}