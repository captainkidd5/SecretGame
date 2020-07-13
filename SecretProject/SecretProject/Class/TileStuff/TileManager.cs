﻿using Microsoft.Xna.Framework;
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
using SecretProject.Class.TileStuff.SpawnStuff;
using SecretProject.Class.TileStuff.TileModifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TiledSharp;
using VelcroPhysics.Dynamics;
using XMLData.ItemStuff;

namespace SecretProject.Class.TileStuff
{

    public class TileManager : ISaveable
    {
        public TmxStageBase Stage;
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
        public Dictionary<string, List<Body>> Objects { get; set; }
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


        public List<int[,]> AdjacentNoise { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Random Random { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<SPlot> AllPlots { get; set; }
        public bool WasModifiedDuringInterval { get; set; }

        public Dictionary<int, TmxTilesetTile> TileSetDictionary { get; set; }
        public Dictionary<string, Sprite> QuestIcons { get; set; }
        public TileModificationHandler TileModificationHandler { get; set; }

        #region CONSTRUCTORS


        //Instantiate with a premade map
        public TileManager(Texture2D tileSet, TmxMap mapName, GraphicsDevice graphicsDevice, ContentManager content, int tileSetNumber, TmxStageBase currentStage, bool addMapPortals = true,int presetDimension = 0)
        {
            this.Stage = currentStage;
            this.Type = 0;
            this.TileSet = tileSet;
            this.MapName = mapName;

            this.AllDepths = new List<float>()
            {
                .01f,
                .1f,
                .2f,
                .3f,
                .5f,
                .7f
            };

            this.TileWidth = 16;
            this.TileHeight = 16;

            this.TileSetDimension = tileSet.Width / this.TileWidth;


            if (presetDimension != 0)
            {
                this.MapWidth = presetDimension;
                this.MapHeight = presetDimension;
            }
            else
            {
                this.MapWidth = mapName.Width;

                this.MapHeight = mapName.Height;
            }



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

            for (int i = 0; i < this.AllDepths.Count; i++)
            {
                this.AllTiles.Add(new Tile[this.MapWidth, this.MapHeight]);

            }
            this.Objects = new Dictionary<string, List<Body>>();

            this.PathGrid = new ObstacleGrid(this.MapWidth, this.MapHeight);
            //if (Game1.IsFirstTimeStartup)
            //{
            //    StartNew();
            //}
            this.TileModificationHandler = new TileModificationHandler();

            #region PORTALS
            if(addMapPortals)
            {
                for (int i = 0; i < mapName.ObjectGroups["Portal"].Objects.Count; i++)
                    FetchPortals(mapName);
            }
            
            #endregion

            currentStage.AllNightLights = this.NightTimeLights;
            currentStage.AllDayTimeLights = this.DayTimeLights;

        }

        private void FetchPortals(TmxMap mapName)
        {
            #region PORTALS
            for (int i = 0; i < mapName.ObjectGroups["Portal"].Objects.Count; i++)
            {
                string keyFrom, keyTo, safteyX, safteyY, click;



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
                if (!this.Stage.AllPortals.Contains(portal))
                {
                    this.Stage.AllPortals.Add(portal);
                }


                if (!Game1.PortalGraph.HasEdge((Stages)portal.From, (Stages)portal.To))
                {
                    Game1.PortalGraph.AddEdge((Stages)portal.From, (Stages)portal.To);
                }

            }
            #endregion
        }

        /// <summary>
        /// Runs only when new save is started. Will not run on load game. See LoadNewSave for that.
        /// </summary>
        public void StartNew(bool isBasedOnPremadeMap = true)
        {
            TmxMap map = this.MapName;
            List<TmxLayer> allLayers = new List<TmxLayer>()
            {
                map.Layers["bedRock"],
                map.Layers["background"],
            map.Layers["midGround"],
           map.Layers["buildings"],
           map.Layers["foreGround"],
           map.Layers["front"]
        };
            for (int i = 0; i < this.AllTiles.Count; i++)
            {
                foreach (TmxLayerTile layerNameTile in allLayers[i].Tiles)
                {
                    Tile tempTile = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid) { LayerToDrawAt = i };

                    this.AllTiles[i][layerNameTile.X, layerNameTile.Y] = tempTile;

                }
            }
            if (isBasedOnPremadeMap) //don't assign properties if we don't have to.
            {
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
                if (this.Stage == Game1.Town || this.Stage == Game1.HomeStead)
                {
                    AddInitialGrassTufts();
                }
            }

            

        }
        private void AddInitialGrassTufts()
        {
            for (int i = 0; i < this.MapWidth; i++)
            {
                for (int j = 0; j < this.MapHeight; j++)
                {
                    if (PathGrid.Weight[i, j] == (int)GridStatus.Clear)
                    {
                        bool canAddGrass = true;
                        for (int layer = (int)MapLayer.Buildings; layer < (int)MapLayer.Front; layer++)
                        {
                            if (AllTiles[layer][i, j].GID != -1) //do not add grass if anything above midground isn't clear.
                            {
                                canAddGrass = false;
                                break;
                            }
                        }


                        if (canAddGrass && AllTiles[(int)MapLayer.ForeGround][i, j].TileKey != null)
                        {
                            SpawnHolder.AddGrassTufts((TileManager)this, AllTiles[(int)MapLayer.ForeGround][i, j], AllTiles[(int)MapLayer.MidGround][i, j]);
                        }

                    }
                }
            }
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
                        if (!Game1.Procedural.GetTilingTileManagerFromGID((GenerationType)generationIndex).GeneratableTiles.Contains(i))
                        {
                            Game1.Procedural.AllTilingTileManagers[(GenerationType)generationIndex].GeneratableTiles.Add(i);
                        }
                    }
                    // throw new Exception("GenerationType value not supported. Check to make sure the tiled value of generate property is supported. Capitals matter.");



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
            int starti = (int)(Game1.cam.Pos.X / 16) - (int)(Game1.ScreenWidth / Game1.CurrentStage.Cam.Zoom / 2 / 16) - 1;
            if (starti < 0)
            {
                starti = 0;
            }
            int startj = (int)(Game1.cam.Pos.Y / 16) - (int)(Game1.ScreenHeight / Game1.CurrentStage.Cam.Zoom / 2 / 16) - 1;
            if (startj < 0)
            {
                startj = 0;
            }
            int endi = (int)(Game1.cam.Pos.X / 16) + (int)(Game1.ScreenWidth / Game1.CurrentStage.Cam.Zoom / 2 / 16) + 2;
            if (endi > this.MapWidth)
            {
                endi = this.MapWidth;
            }
            int endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.CurrentStage.Cam.Zoom / 2 / 16) + 2;
            if (endj > this.MapWidth)
            {
                endj = this.MapWidth;
            }
            UpdateAnimationFrames(gameTime);


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
                        if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(this.AllTiles[(int)MapLayer.MidGround][playerI, playerJ].GID))
                        {
                            if (this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[(int)MapLayer.MidGround][playerI, playerJ].GID].Properties.ContainsKey("step"))
                            {

                                Game1.Player.WalkSoundEffect = int.Parse(this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[(int)MapLayer.MidGround][playerI, playerJ].GID].Properties["step"]);
                            }
                            else if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(this.AllTiles[(int)MapLayer.BackGround][playerI, playerJ].GID))
                            {
                                if (this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[(int)MapLayer.BackGround][playerI, playerJ].GID].Properties.ContainsKey("step"))
                                {

                                    Game1.Player.WalkSoundEffect = int.Parse(this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[(int)MapLayer.BackGround][playerI, playerJ].GID].Properties["step"]);
                                }
                            }
                        }
                        else if (this.MapName.Tilesets[this.TileSetNumber].Tiles.ContainsKey(this.AllTiles[(int)MapLayer.BackGround][playerI, playerJ].GID))
                        {
                            if (this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[(int)MapLayer.BackGround][playerI, playerJ].GID].Properties.ContainsKey("step"))
                            {

                                Game1.Player.WalkSoundEffect = int.Parse(this.MapName.Tilesets[this.TileSetNumber].Tiles[this.AllTiles[(int)MapLayer.BackGround][playerI, playerJ].GID].Properties["step"]);
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

                            if(this.AllTiles == null)
                            {
                                return;
                            }


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
                Game1.CurrentStage.AllNightLights = this.NightTimeLights;
            }
            if (this.GridItem != null)
            {
                this.GridItem.NormalUpdate(gameTime, this, this);
            }
            else
            {
                Game1.Player.UserInterface.DrawTileSelector = false;
            }
            this.TileModificationHandler.Update(gameTime);
            for (int item = 0; item < this.AllItems.Count; item++)
            {
                this.AllItems[item].Update(gameTime);
            }

        }

        public void UpdateAnimationFrames(GameTime gameTime)
        {
            List<string> AnimationFrameKeysToRemove = new List<string>();
            foreach (EditableAnimationFrameHolder frameholder in AnimationFrames.Values)
            {
                if (Game1.GlobalClock.SecondsPassedToday > frameholder.Frames[frameholder.Counter].TargetDuration)
                {
                    Tile animationTile = AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY];
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
                                if (frameholder.Terminates)
                                {

                                    TileUtility.FinalizeTile(frameholder.Layer, gameTime, frameholder.OldX, frameholder.OldY, this);
                                }

                            }
                        }

                        frameholder.Counter = 0;
                        frameholder.SetNewTarget(frameholder.Counter);

                    }
                    else
                    {
                        frameholder.Counter++;
                        frameholder.SetNewTarget(frameholder.Counter);

                    }
                }
            }

            foreach (string key in AnimationFrameKeysToRemove)
            {
                AnimationFrames.Remove(key);
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

                    entry.Value[grass].Draw(spriteBatch, this.TileSet);

                }
            }
            for (int z = 0; z < this.AllTiles.Count; z++)
            {
                for (var i = starti; i < endi; i++)
                {
                    for (var j = startj; j < endj; j++)
                    {
                        Tile tile = this.AllTiles[z][i, j];
                        if (tile.GID != -1)
                        {

                            Color tileColor = Color.White;
                            if (Game1.CurrentStage.ShowBorders)
                            {
                                if (this.Objects.ContainsKey(AllTiles[z][i, j].TileKey))
                                {
                                    tileColor = Color.Red;
                                }
                            }



                            if (z >= (int)MapLayer.ForeGround)
                            {
                                spriteBatch.Draw(this.TileSet, tile.Position, tile.SourceRectangle, tileColor * tile.ColorMultiplier,
                                tile.Rotation, tile.Origin, 1f, SpriteEffects.None, this.AllDepths[z] + tile.LayerToDrawAtZOffSet);

                            }

                            else
                            {
                                spriteBatch.Draw(this.TileSet, tile.Position, tile.SourceRectangle, tileColor,
                                tile.Rotation, tile.Origin, 1f, SpriteEffects.None, this.AllDepths[z]);
                            }

                            if (Game1.Player.UserInterface.CommandConsole.DisplayTileIndex)
                            {
                                spriteBatch.DrawString(Game1.AllTextures.MenuText, i + "," + j, tile.Position, Color.White, 0f, Game1.Utility.Origin, .25f, SpriteEffects.None, 1f);
                                //spriteBatch.DrawString(font, text, fontLocation, tint, 0f, Game1.Utility.Origin, 1f,SpriteEffects.None, layerDepth: .73f);
                            }


                        }
                    }
                }
            }
            if (this.GridItem != null)
            {
                this.GridItem.NormalDraw(spriteBatch, this, this);
            }
            DrawGrassTufts(spriteBatch);
            for (int item = 0; item < this.AllItems.Count; item++)
            {
                this.AllItems[item].Draw(spriteBatch);
            }



        }
        private void DrawGrassTufts(SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<string, List<GrassTuft>> entry in this.Tufts)
            {
                for (int i = 0; i < entry.Value.Count; i++)
                {

                    entry.Value[i].Draw(spriteBatch, this.TileSet);

                }
            }
        }

        public void LoadInitialChunks(Vector2 position)
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
                    int newGid = this.MapName.Tilesets[this.TileSetNumber].Tiles[crop.BaseGID].AnimationFrames[crop.CurrentGrowth].Id;
                    crop.UpdateGrowthCycle(newGid);

                    TileUtility.ReplaceTile((int)MapLayer.ForeGround, crop.X, crop.Y, crop.GID, this);
                }

            }


        }

        public void HandleClockChange(object sender, EventArgs eventArgs)
        {
            if (Game1.CurrentStage == this.Stage)
            {
                UpdateCropTile();
            }

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
            this.Tufts = null;

        }

        public void Save(BinaryWriter binaryWriter)
        {

            binaryWriter.Write(this.AllTiles.Count);

            binaryWriter.Write(this.MapWidth);
            for (int z = 0; z < this.AllTiles.Count; z++)
            {
                for (int i = 0; i < this.MapWidth; i++)
                {
                    for (int j = 0; j < this.MapHeight; j++)
                    {
                        binaryWriter.Write(this.AllTiles[z][i, j].GID + 1);
                    }
                }
            }

            //binaryWriter.Write(this.StoreableItems.Count);
            //foreach (KeyValuePair<string, IStorableItemBuilding> storeableItem in this.StoreableItems)
            //{

            //    binaryWriter.Write(storeableItem.Key);
            //    binaryWriter.Write((int)storeableItem.Value.StorableItemType);
            //    binaryWriter.Write(storeableItem.Value.Size);
            //    binaryWriter.Write(storeableItem.Value.Location.X);
            //    binaryWriter.Write(storeableItem.Value.Location.Y);
            //    for (int s = 0; s < storeableItem.Value.Size; s++)
            //    {
            //        binaryWriter.Write(storeableItem.Value.Inventory.currentInventory[s].ItemCount);
            //        if (storeableItem.Value.Inventory.currentInventory[s].ItemCount > 0)
            //        {
            //            binaryWriter.Write(storeableItem.Value.Inventory.currentInventory[s].Item.ID);
            //        }
            //        else
            //        {
            //            binaryWriter.Write(-1);
            //        }

            //    }

            //}

            binaryWriter.Write(this.AllItems.Count);
            for (int item = 0; item < AllItems.Count; item++)
            {
                binaryWriter.Write(AllItems[item].ID);
                binaryWriter.Write(AllItems[item].WorldPosition.X);
                binaryWriter.Write(AllItems[item].WorldPosition.Y);
            }
            //binaryWriter.Write(this.Stage.AllPortals.Count);
            //foreach(Portal portal in this.Stage.AllPortals)
            //{
            //    portal.Save(binaryWriter);
            //}

            //binaryWriter.Write(this.Crops.Count);
            //foreach (KeyValuePair<string, Crop> crop in this.Crops)
            //{
            //    binaryWriter.Write(crop.Key);
            //    binaryWriter.Write(crop.Value.X);
            //    binaryWriter.Write(crop.Value.Y);
            //    binaryWriter.Write(crop.Value.ItemID);
            //    binaryWriter.Write(crop.Value.Name);
            //    binaryWriter.Write(crop.Value.GID);
            //    binaryWriter.Write(crop.Value.BaseGID);
            //    binaryWriter.Write(crop.Value.DaysToGrow);
            //    binaryWriter.Write(crop.Value.CurrentGrowth);
            //    binaryWriter.Write(crop.Value.Harvestable);
            //    binaryWriter.Write(crop.Value.DayPlanted);

            //}

            binaryWriter.Write(this.Tufts.Count);
            foreach (KeyValuePair<string, List<GrassTuft>> tuft in this.Tufts)
            {
                binaryWriter.Write(tuft.Key);
                binaryWriter.Write(tuft.Value.Count);

                for (int i = 0; i < tuft.Value.Count; i++)
                {
                    binaryWriter.Write(tuft.Value[i].GrassType);
                    binaryWriter.Write(tuft.Value[i].Position.X);
                    binaryWriter.Write(tuft.Value[i].Position.Y);
                    tuft.Value[i].UnloadBody();
                }
                
            }
        }

        public void Load(BinaryReader binaryReader)
        {

            int layerCount = binaryReader.ReadInt32();
            int mapWidth = binaryReader.ReadInt32();
            this.MapWidth = mapWidth;
            this.MapHeight = mapWidth;
            this.AllTiles = new List<Tile[,]>();
            this.AllItems = new List<Item>();
            this.Objects = new Dictionary<string, List<Body>>();

            for (int i = 0; i < layerCount; i++)
            {
                this.AllTiles.Add(new Tile[mapWidth, mapWidth]);

            }
            for (int z = 0; z < layerCount; z++)
            {

                for (int i = 0; i < mapWidth; i++)
                {
                    for (int j = 0; j < mapWidth; j++)
                    {
                        this.AllTiles[z][i, j] = new Tile(i, j, binaryReader.ReadInt32());
                    }
                }
            }

            //  if (this.Stage != Game1.Town)
            //{
            for (int z = 0; z < layerCount; z++)
            {

                for (int i = 0; i < mapWidth; i++)
                {
                    for (int j = 0; j < mapWidth; j++)
                    {
                        TileUtility.AssignProperties(AllTiles[z][i, j], z, i, j, this);
                    }
                }
            }
            //this.StoreableItems = new Dictionary<string, IStorableItemBuilding>();
            //int storableItemCount = binaryReader.ReadInt32();
            //for (int c = 0; c < storableItemCount; c++)
            //{
            //    string storageKey = binaryReader.ReadString();
            //    int storableItemType = binaryReader.ReadInt32();
            //    StorableItemType itemType = (StorableItemType)storableItemType;
            //    int inventorySize = binaryReader.ReadInt32();
            //    float locationX = binaryReader.ReadSingle();
            //    float locationY = binaryReader.ReadSingle();

            //    switch (itemType)
            //    {
            //        case StorableItemType.Chest:
            //            Chest storeageItemToAdd = new Chest(storageKey, inventorySize, new Vector2(locationX, locationY), this.GraphicsDevice, false);
            //            for (int i = 0; i < inventorySize; i++)
            //            {
            //                int numberOfItemsInSlot = binaryReader.ReadInt32();
            //                int itemID = binaryReader.ReadInt32();

            //                for (int j = 0; j < numberOfItemsInSlot; j++)
            //                {
            //                    storeageItemToAdd.Inventory.currentInventory[i].AddItemToSlot(Game1.ItemVault.GenerateNewItem(itemID, null, false));
            //                }
            //            }

            //            this.StoreableItems.Add(storageKey, storeageItemToAdd);
            //            break;

            //        case StorableItemType.Cauldron:
            //            Cauldron cauldronToAdd = new Cauldron(storageKey, inventorySize, new Vector2(locationX, locationY), this.GraphicsDevice);
            //            for (int i = 0; i < inventorySize; i++)
            //            {
            //                int numberOfItemsInSlot = binaryReader.ReadInt32();
            //                int itemID = binaryReader.ReadInt32();

            //                for (int j = 0; j < numberOfItemsInSlot; j++)
            //                {
            //                    cauldronToAdd.Inventory.currentInventory[i].AddItemToSlot(Game1.ItemVault.GenerateNewItem(itemID, null, false));
            //                }
            //            }

            //            this.StoreableItems.Add(storageKey, cauldronToAdd);
            //            break;
            //    }
            //}
            int itemCount = binaryReader.ReadInt32();
            for (int item = 0; item < itemCount; item++)
            {
                Game1.ItemVault.GenerateNewItem(binaryReader.ReadInt32(), new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle()), true, this.AllItems);
            }
            //int portalCount = binaryReader.ReadInt32();
            //for(int p = 0; p < portalCount; p++)
            //{
            //    Portal portal = new Portal();
            //    portal.Load(binaryReader);
            //    if (!this.Stage.AllPortals.Contains(portal))
            //    {
            //        this.Stage.AllPortals.Add(portal);
            //    }


            //    if (!Game1.PortalGraph.HasEdge(portal.From, portal.To))
            //    {
            //        Game1.PortalGraph.AddEdge(portal.From, portal.To);
            //    }
            //}

            //  int cropCount = binaryReader.ReadInt32();
            //  for (int c = 0; c < cropCount; c++)
            //  {
            //      string cropKey = binaryReader.ReadString();
            //      int cropX = binaryReader.ReadInt32();
            //      int cropY = binaryReader.ReadInt32();
            //      int itemID = binaryReader.ReadInt32();
            //      string name = binaryReader.ReadString();
            //      int gid = binaryReader.ReadInt32();
            //      int baseGID = binaryReader.ReadInt32();
            //      int daysToGrow = binaryReader.ReadInt32();
            //      int currentGrow = binaryReader.ReadInt32();
            //      bool harvestable = binaryReader.ReadBoolean();
            //      int dayPlanted = binaryReader.ReadInt32();
            //      //int newCurrentGrowth = Game1.GlobalClock.TotalDays - dayPlanted;
            //      //if (newCurrentGrowth > daysToGrow)
            //      //{
            //      //    newCurrentGrowth = daysToGrow;
            //      //}
            //      if (currentGrow >= this.MapName.Tilesets[this.TileSetNumber].Tiles[baseGID].AnimationFrames.Count)
            //      {
            //          currentGrow = this.MapName.Tilesets[this.TileSetNumber].Tiles[baseGID].AnimationFrames.Count - 1;
            //      }
            //      Crop crop = new Crop()
            //      {
            //          ItemID = itemID,
            //          Name = name,
            //          X = cropX,
            //          Y = cropY,
            //          BaseGID = baseGID,
            //          DaysToGrow = daysToGrow,

            //          Harvestable = harvestable,
            //          DayPlanted = dayPlanted,
            //          CurrentGrowth = currentGrow,

            //          GID = this.MapName.Tilesets[this.TileSetNumber].Tiles[baseGID].AnimationFrames[currentGrow].Id + 1,
            //      };
            //      if (!this.Crops.ContainsKey(cropKey))
            //      {
            //          this.Crops.Add(cropKey, crop);
            //      }

            //      TileUtility.ReplaceTile(3, crop.X, crop.Y, crop.GID, this);
            //  }

            this.Tufts = new Dictionary<string, List<GrassTuft>>();
            int tuftListCount = binaryReader.ReadInt32();

            for (int i = 0; i < tuftListCount; i++)
            {
                string key = binaryReader.ReadString();
                int smallListCount = binaryReader.ReadInt32();
                List<GrassTuft> tufts = new List<GrassTuft>();
                for (int j = 0; j < smallListCount; j++)
                {
                    GrassTuft tuft = new GrassTuft(this.GraphicsDevice, binaryReader.ReadInt32(),
                        new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle()), Stage);
                    tuft.TuftsIsPartOf = tufts;
                   // tuft.LoadBody(Stage);
                    tufts.Add(tuft);
                }
                if(!Tufts.ContainsKey(key))
                this.Tufts.Add(key, tufts);
            }
            //  }

        }

        public void AddTileModification(Tile tile, ITileModifiable tileModifiable)
        {
            this.TileModificationHandler.AddModification(tile, tileModifiable);
        }

        #endregion

        #endregion

    }

}