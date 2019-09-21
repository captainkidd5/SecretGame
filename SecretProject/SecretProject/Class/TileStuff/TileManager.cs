using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.StageFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

using System.Xml.Serialization;

using SecretProject.Class.ObjectFolder;
using SecretProject.Class.Controls;

using Microsoft.Xna.Framework.Content;
using SecretProject.Class.Universal;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.PathFinding;
using XMLData.ItemStuff;
using SecretProject.Class.SpriteFolder;

namespace SecretProject.Class.TileStuff
{
    /// <summary>
    /// background = 0, buildings = 1, midground =2, foreground =3, placement =4
    /// </summary>
    public class TileManager : ITileManager
    {
        protected Game1 game;
        public Texture2D TileSet { get; set; }
        public TmxMap MapName { get;set; }
        protected TmxLayer layerName;
        public int tilesetTilesWide { get; set; }
        public int tilesetTilesHigh { get; set; }
        public int mapWidth { get; set; }
        public int mapHeight { get; set; }
        public int iD { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int tileNumber { get; set; }
        public int tileCounter { get; set; }
        public Tile[,] Tiles { get; set; }
        public bool isActive = false;
        public bool isPlacement { get; set; } = false;
        public bool isInClickingRangeOfPlayer = false;
        public ContentManager Content { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public int ReplaceTileGid { get; set; }
        public int CurrentIndexX { get; set; }
        public int CurrentIndexY { get; set; }
        public Tile TempTile { get; set; }
        public int OldIndexX { get; set; }
        public int OldIndexY { get; set; }
        public float Depth { get; set; }
        public int LayerIdentifier { get; set; }
        public List<TmxLayer> AllLayers;
        public List<Tile[,]> AllTiles { get; set; }
        public List<float> AllDepths { get; set; }
        public bool TileInteraction { get; set; } = false;
        public Tile DebugTile { get; set; } = new Tile(40, 40, 4714);
        public int TileSetNumber { get; set; }
        public AStarPathFinder PathGrid { get; set; }
        public List<int> DirtGeneratableTiles;
        public List<int> SandGeneratableTiles;
        public List<int> GrassGeneratableTiles;
        public Dictionary<string, EditableAnimationFrameHolder> AnimationFrames { get; set; }
        public Dictionary<string, List<GrassTuft>> AllTufts { get; set; }
        public Dictionary<string, int> TileHitPoints { get; set; }
        public Dictionary<string, ObjectBody> CurrentObjects { get; set; }
        public bool AbleToDrawTileSelector { get; set; }

        #region CONSTRUCTORS

        private TileManager()
        {

        }

        public TileManager(Texture2D tileSet, TmxMap mapName, List<TmxLayer> allLayers, GraphicsDevice graphicsDevice, ContentManager content, int tileSetNumber, List<float> allDepths, ILocation currentStage)
        {
            this.TileSet = tileSet;
            this.MapName = mapName;

            TileWidth = mapName.Tilesets[tileSetNumber].TileWidth;
            TileHeight = mapName.Tilesets[tileSetNumber].TileHeight;

            tilesetTilesWide = tileSet.Width / TileWidth;
            tilesetTilesHigh = tileSet.Height / TileHeight;

            mapWidth = mapName.Width;
            mapHeight = mapName.Height;

            this.tileCounter = 0;

            this.GraphicsDevice = graphicsDevice;
            this.Content = content;

            this.AllDepths = allDepths;
            this.AllLayers = allLayers;
            AllTiles = new List<Tile[,]>();
            this.TileSetNumber = tileSetNumber;
            DirtGeneratableTiles = new List<int>();
            SandGeneratableTiles = new List<int>();
            GrassGeneratableTiles = new List<int>();
            AnimationFrames = new Dictionary<string, EditableAnimationFrameHolder>();
            AllTufts = new Dictionary<string, List<GrassTuft>>();
            TileHitPoints = new Dictionary<string, int>();
            CurrentObjects = new Dictionary<string, ObjectBody>();
            for (int i = 0; i < allLayers.Count; i++)
            {
                AllTiles.Add(new Tile[mapName.Width, mapName.Height]);

            }


            for (int i = 0; i < AllTiles.Count; i++)
            {
                foreach (TmxLayerTile layerNameTile in AllLayers[i].Tiles)
                {
                    Tile tempTile = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid);
                    AllTiles[i][layerNameTile.X, layerNameTile.Y] = tempTile;

                    if (mapName.Tilesets[tileSetNumber].Tiles.ContainsKey(AllTiles[i][layerNameTile.X, layerNameTile.Y].GID))
                    {

                        if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[i][layerNameTile.X, layerNameTile.Y].GID].Properties.ContainsKey("generate"))
                        {
                            switch (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[i][layerNameTile.X, layerNameTile.Y].GID].Properties["generate"])
                            {


                                case "dirt":
                                    if (!Game1.Utility.DirtGeneratableTiles.Contains(AllTiles[i][layerNameTile.X, layerNameTile.Y].GID))
                                    {
                                        Game1.Utility.DirtGeneratableTiles.Add(AllTiles[i][layerNameTile.X, layerNameTile.Y].GID);
                                    }
                                    break;
                                case "sand":
                                    if (!Game1.Utility.SandGeneratableTiles.Contains(AllTiles[i][layerNameTile.X, layerNameTile.Y].GID))
                                    {
                                        Game1.Utility.SandGeneratableTiles.Add(AllTiles[i][layerNameTile.X, layerNameTile.Y].GID);
                                    }
                                    break;
                                case "grass":
                                    if (!Game1.Utility.GrassGeneratableTiles.Contains(AllTiles[i][layerNameTile.X, layerNameTile.Y].GID))
                                    {
                                        Game1.Utility.GrassGeneratableTiles.Add(AllTiles[i][layerNameTile.X, layerNameTile.Y].GID);
                                    }
                                    break;
                            }


                        }
                    }


                }
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
                Portal portal = new Portal(int.Parse(keyFrom), int.Parse(keyTo), int.Parse(safteyX), int.Parse(safteyY), bool.Parse(click));


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

            #region RANDOMGENERATETILES
            //specify GID which is 1 larger than one on tileset, idk why
            //brown tall grass
            // GenerateTiles(3, 6394, "dirt", 2000, 0);
            //green tall grass
            // GenerateTiles(3, 6393, "dirt", 2000, 0);
            //    //stone
            //    GenerateTiles(1, 979, "dirt", 50, 0, currentStage);
            ////    //grass
            //  GenerateTiles(1, 1079, "dirt", 50, 0, currentStage);
            ////    //redrunestone
            //GenerateTiles(1, 579, "dirt", 50, 0, currentStage);
            //////bluerunestone
            // GenerateTiles(1, 779, "dirt", 100, 0, currentStage);
            //////thunderbirch
            //    GenerateTiles(1, 2264, "dirt", 200, 0, currentStage);
            //////crown of swords
            //GenerateTiles(1, 6388, "sand", 50, 0);
            //////dandelion
            //GenerateTiles(1, 6687, "sand", 100, 0);
            ////juicyfruit
            //GenerateTiles(1, 1586, "dirt", 500, 0);
            ////orchardTree
            //   GenerateTiles(1, 1664, "dirt", 200, 0, currentStage);
            //bubblegum
            // GenerateTiles(1, 6191, "dirt", 200, 0);
            #endregion

            for (int z = 0; z < AllTiles.Count; z++)
            {
                for (int i = 0; i < this.mapWidth; i++)
                {
                    for (int j = 0; j < this.mapHeight; j++)
                    {
                        if (AllTiles[z][i, j].GID != -1)
                        {
                            if (mapName.Tilesets[tileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                            {

                                //AssignProperties(AllTiles[z][i, j], 0, z, i, j, currentStage);
                                TileUtility.AssignProperties(AllTiles[z][i, j], graphicsDevice, mapName, mapWidth, mapHeight, this, tileSetNumber, z, i, j, currentStage);


                            }
                        }
                    }
                }
            }

        }
        public int NumberOfLayers { get; set; }
        //FOR PROCEDURAL
        #region PROCEDURALGENERATION CONSTRUCTOR

        public TileManager(World world, Texture2D tileSet, List<TmxLayer> allLayers, TmxMap mapName, int numberOfLayers, int worldWidth, int worldHeight, GraphicsDevice graphicsDevice, ContentManager content, int tileSetNumber, List<float> allDepths)
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

            this.NumberOfLayers = numberOfLayers;

            this.tileCounter = 0;

            this.GraphicsDevice = graphicsDevice;
            this.Content = content;

            this.AllDepths = allDepths;

            AllTiles = new List<Tile[,]>();
            this.TileSetNumber = tileSetNumber;
            DirtGeneratableTiles = new List<int>();
            SandGeneratableTiles = new List<int>();
            GrassGeneratableTiles = new List<int>();
            AnimationFrames = new Dictionary<string, EditableAnimationFrameHolder>();
            AllTufts = new Dictionary<string, List<GrassTuft>>();
            TileHitPoints = new Dictionary<string, int>();
            CurrentObjects = new Dictionary<string, ObjectBody>();


            for (int i = 0; i < NumberOfLayers; i++)
            {
                AllTiles.Add(new Tile[worldWidth, worldHeight]);

            }
            for (int i = 0; i < 10000; i++)
            {
                if (mapName.Tilesets[tileSetNumber].Tiles.ContainsKey(i))
                {
                    if (mapName.Tilesets[tileSetNumber].Tiles[i].Properties.ContainsKey("generate"))
                    {
                        switch (mapName.Tilesets[tileSetNumber].Tiles[i].Properties["generate"])
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

            float chanceToBeDirt = .45f;
            for (int z = 0; z < NumberOfLayers; z++)
            {
                for (int i = 0; i < worldWidth; i++)
                {
                    for (int j = 0; j < worldHeight; j++)
                    {
                        if (z >= 1)
                        {
                            AllTiles[z][i, j] = new Tile(i, j, 0);
                        }

                        else
                        {
                            if (Game1.Utility.RFloat(0, 1) > chanceToBeDirt)
                            {
                                AllTiles[z][i, j] = new Tile(i, j, 1106);
                            }
                            else
                            {
                                AllTiles[z][i, j] = new Tile(i, j, 1116);
                            }

                        }


                    }
                }

            }
            for (int i = 0; i < 5; i++)
            {
                AllTiles[0] = TileUtility.DoSimulation(AllTiles[0], tilesetTilesWide, tilesetTilesHigh, worldWidth, worldHeight);
            }

            TileUtility.PlaceChests(AllTiles, world, tilesetTilesWide, tilesetTilesHigh, worldWidth, worldHeight, graphicsDevice);

            for (int i = 0; i < worldWidth; i++)
            {
                for (int j = 0; j < worldHeight; j++)
                {
                    //TileUtility.ReassignTileForTiling(this.AllTiles, i, j, worldWidth, worldHeight);
                    if (Game1.Utility.RGenerator.Next(1, TileUtility.GrassSpawnRate) == 5)
                    {
                        if (Game1.Utility.GrassGeneratableTiles.Contains(AllTiles[0][i, j].GID))
                        {

                            int numberOfGrassTuftsToSpawn = Game1.Utility.RGenerator.Next(1, 4);
                            List<GrassTuft> tufts = new List<GrassTuft>();
                            for (int g = 0; g < numberOfGrassTuftsToSpawn; g++)
                            {
                                int grassType = Game1.Utility.RGenerator.Next(1, 4);
                                tufts.Add(new GrassTuft(grassType, new Vector2(TileUtility.GetDestinationRectangle(AllTiles[0][i, j]).X, TileUtility.GetDestinationRectangle(AllTiles[0][i, j]).Y)));

                            }
                            this.AllTufts[AllTiles[0][i, j].GetTileKey(0)] = tufts;
                        }
                    }
                }
            }

            Portal portal = new Portal(3, 0, 0, -50, true);
            TileUtility.SpawnBaseCamp(AllTiles, tilesetTilesWide, tilesetTilesHigh, worldWidth, worldHeight);



            portal.PortalStart = new Rectangle(worldWidth * 16 / 2 + 120, worldHeight * 16 / 2 + 120, 50, 50);

            world.AllPortals.Add(portal);
            Game1.PortalGraph.AddEdge(portal.From, portal.To);
            //    }

            //specify GID which is 1 larger than one on tileset, idk why
            //brown tall grass
            // GenerateTiles(3, 6394, "dirt", 2000, 0);
            //green tall grass
            // GenerateTiles(3, 6393, "dirt", 2000, 0,world);
            //stone
            TileUtility.GenerateTiles(1, 979, "dirt", 2000, 0, this,AllTiles,world);
            //    //grass
           // GenerateTiles(1, 1079, "dirt", 5000, 0, world);
            //    //redrunestone
           // GenerateTiles(1, 579, "dirt", 500, 0, world);
            ////bluerunestone
           // GenerateTiles(1, 779, "dirt", 1000, 0, world);
            ////thunderbirch
            TileUtility.GenerateTiles(1, 2264, "dirt", 5000, 0, this, AllTiles, world);
            //////crown of swords
            //GenerateTiles(1, 6388, "sand", 50, 0);
            //////dandelion
            //GenerateTiles(1, 6687, "sand", 100, 0);
            ////juicyfruit
          //  GenerateTiles(1, 1586, "dirt", 500, 0, world);
            ////orchardTree
           // GenerateTiles(1, 1664, "dirt", 800, 0, world);
            //bubblegum
            //GenerateTiles(1, 6191, "dirt", 200, 0, world);


            for (int z = 0; z < AllTiles.Count; z++)
            {
                for (int i = 0; i < this.mapWidth; i++)
                {
                    for (int j = 0; j < this.mapHeight; j++)
                    {

                        if (AllTiles[z][i, j].GID != 0)
                        {
                            if (MapName.Tilesets[tileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                            {

                                //AssignProperties(AllTiles[z][i, j], 0, z, i, j, world);
                                TileUtility.AssignProperties(AllTiles[z][i, j], graphicsDevice, mapName, mapWidth, mapHeight, this, tileSetNumber, z, i, j, world);

                            }
                        }
                    }
                }
            }

        }

        #endregion
        #endregion

        #region LOADTILESOBJECTS
        public void LoadInitialTileObjects(ILocation stage)
        {
            
            PathGrid = new AStarPathFinder(mapWidth, mapHeight, AllTiles, stage.AllObjects);

        }
        #endregion

        #region UPDATE
        
       // Dictionary<string, TileStuff.EditableAnimationFrameHolder> ITileManager.AnimationFrames { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            AbleToDrawTileSelector = false;
            CurrentObjects.Clear();
            //Game1.myMouseManager.TogglePlantInteraction = false;
            Game1.Player.UserInterface.DrawTileSelector = false;
            List<string> AnimationFrameKeysToRemove = new List<string>();
            int starti = (int)(Game1.cam.Pos.X / 16) - (int)(Game1.ScreenWidth /Game1.GetCurrentStage().Cam.Zoom / 2 / 16) - 1;
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
            if (endi > this.mapWidth)
            {
                endi = this.mapWidth;
            }
            int endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.GetCurrentStage().Cam.Zoom / 2 / 16) + 2;
            if (endj > this.mapWidth)
            {
                endj = this.mapWidth;
            }

            foreach (EditableAnimationFrameHolder frameholder in AnimationFrames.Values)
            {
                frameholder.Frames[frameholder.Counter].CurrentDuration -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameholder.Frames[frameholder.Counter].CurrentDuration <= 0)
                {
                    frameholder.Frames[frameholder.Counter].CurrentDuration = frameholder.Frames[frameholder.Counter].AnchorDuration;
                    AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY] = new Tile(frameholder.OldX, frameholder.OldY,
                        frameholder.Frames[frameholder.Counter].ID + 1);
                    if (frameholder.Counter == frameholder.Frames.Count - 1)
                    {
                        if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(frameholder.OriginalTileID))
                        {

                            if (MapName.Tilesets[TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("destructable") || MapName.Tilesets[TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("relationX"))
                            {

                                //needs to refer to first tile ?
                                int frameolDX = frameholder.OldX;
                                AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY] = new Tile(frameholder.OldX, frameholder.OldY, frameholder.OriginalTileID + 1);
                                AnimationFrameKeysToRemove.Add(AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].GetTileKey(frameholder.Layer));
                                if (MapName.Tilesets[TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("destructable"))
                                {
                                    TileUtility.Destroy(frameholder.Layer, frameholder.OldX, frameholder.OldY, TileUtility.GetDestinationRectangle(AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY]), Game1.GetCurrentStage(),this, AllTiles);
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


            Game1.Player.CollideOccured = false;

            for (int z = 0; z < AllTiles.Count; z++)
            {
                for (var i = starti; i < endi; i++)
                {
                    for (var j = startj; j < endj; j++)
                    {

                        if (AllTiles[z][i, j].GID != -1)
                        {
                            string TileKey = AllTiles[z][i, j].GetTileKey(z);
                            Rectangle destinationRectangle = TileUtility.GetDestinationRectangle(AllTiles[z][i, j]);
                            if (AllTufts.ContainsKey(TileKey))
                            {
                                for (int t = 0; t < AllTufts[TileKey].Count; t++)
                                {
                                    AllTufts[TileKey][t].Update(gameTime);
                                }
                            }
                            if (Game1.GetCurrentStage().AllObjects.ContainsKey(TileKey))
                            {
                                CurrentObjects.Add(TileKey, Game1.GetCurrentStage().AllObjects[TileKey]);

                            }

                            if (z == 0)
                            {
                                if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                {
                                    if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("step") && Game1.Player.IsMoving && Game1.Player.Rectangle.Intersects(destinationRectangle))
                                    {
                                        Game1.SoundManager.PlaySoundEffectFromInt(false, 1, Game1.Utility.GetRequiredTileTool(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["step"]), .75f);
                                    }
                                }

                            }

                            if (destinationRectangle.Intersects(Game1.Player.ClickRangeRectangle))
                            {
                                
                                if (mouse.IsHoveringTile(destinationRectangle))
                                {
                                    this.AbleToDrawTileSelector = true;
                                    CurrentIndexX = i;
                                    CurrentIndexY = j;

                                    if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                    {

                                        if (z == 1)
                                        {

                                            if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("destructable"))
                                            {
                                                Game1.Player.UserInterface.DrawTileSelector = true;
                                                Game1.isMyMouseVisible = false;
                                                Game1.Player.UserInterface.TileSelectorX = destinationRectangle.X;
                                                Game1.Player.UserInterface.TileSelectorY = destinationRectangle.Y;

                                                mouse.ChangeMouseTexture(Game1.Utility.GetRequiredTileTool(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["destructable"]));

                                                Game1.myMouseManager.ToggleGeneralInteraction = true;

                                                if (mouse.IsClicked)
                                                {
                                                    TileUtility.InteractWithBuilding(z, gameTime, i, j, destinationRectangle, Game1.GetCurrentStage(),this, AllTiles);

                                                }

                                            }
                                            //return;

                                        }
                                        if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                        {
                                            if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("action"))
                                            {

                                                TileUtility.ActionHelper(z, i, j, MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["action"], mouse,this,AllTiles);

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
        
        #endregion

        
        #region DRAW
       
        public void DrawTiles(SpriteBatch spriteBatch)
        {

            int starti = (int)(Game1.cam.Pos.X / 16) - (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2 / 16) - 1;
            if (starti < 0)
            {
                starti = (int)(Game1.cam.Pos.X / 16) - (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2 / 16);
            }
            int startj = (int)(Game1.cam.Pos.Y / 16) - (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2 / 16) - 1;
            if (startj < 0)
            {
                startj = (int)(Game1.cam.Pos.Y / 16) - (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2 / 16);
            }
            int endi = (int)(Game1.cam.Pos.X / 16) + (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2 / 16) + 2;
            if (endi > this.mapWidth)
            {
                endi = this.mapWidth;
            }
            int endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2 / 16) + 2;
            if (endj > this.mapHeight)
            {
                endj = this.mapWidth;
            }
            if (startj < 0 || endj < 0 || starti < 0 || endi < 0 || endi > mapWidth || endj > mapHeight)
            {
                return;
            }
            for (int z = 0; z < AllTiles.Count; z++)
            {
                for (var i = starti; i < endi; i++)
                {
                    for (var j = startj; j < endj; j++)
                    {
                        if (AllTiles[z][i, j].GID != -1)
                        {
                            Rectangle SourceRectangle = TileUtility.GetSourceRectangle(AllTiles[z][i, j], this.tilesetTilesWide);
                            Rectangle DestinationRectangle = TileUtility.GetDestinationRectangle(AllTiles[z][i, j]);
                            if (AllTufts.ContainsKey(AllTiles[z][i, j].GetTileKey(z)))
                            {
                                for (int t = 0; t < AllTufts[AllTiles[z][i, j].GetTileKey(z)].Count; t++)
                                {
                                    AllTufts[AllTiles[z][i, j].GetTileKey(z)][t].Draw(spriteBatch);
                                }
                            }
                            if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                            {
                                if (MapName.Tilesets[this.TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("newSource"))
                                {
                                    int[] rectangleCoords = Game1.Utility.GetNewTileSourceRectangle(MapName.Tilesets[this.TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["newSource"]);


                                    SourceRectangle = new Rectangle(SourceRectangle.X + rectangleCoords[0], SourceRectangle.Y + rectangleCoords[1],
                                        SourceRectangle.Width + rectangleCoords[2], SourceRectangle.Height + rectangleCoords[3]);


                                    DestinationRectangle = new Rectangle(DestinationRectangle.X + rectangleCoords[0], DestinationRectangle.Y + rectangleCoords[1],
                                        DestinationRectangle.Width, DestinationRectangle.Height);
                                }
                            }

                            if (z == 3)
                            {
                                spriteBatch.Draw(TileSet, new Vector2(DestinationRectangle.X, DestinationRectangle.Y), SourceRectangle, Game1.GlobalClock.TimeOfDayColor,
                                0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z] + AllTiles[z][i, j].LayerToDrawAtZOffSet);

                            }
                            else
                            {
                                spriteBatch.Draw(TileSet, new Vector2(DestinationRectangle.X, DestinationRectangle.Y), SourceRectangle, Color.White,
                                0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z]);
                            }

                            if (Game1.GetCurrentStage().ShowBorders)
                            {
                                spriteBatch.DrawString(Game1.AllTextures.MenuText, i + "," + j, new Vector2(DestinationRectangle.X, DestinationRectangle.Y), Color.White, 0f, Game1.Utility.Origin, .25f, SpriteEffects.None, 1f);
                                //spriteBatch.DrawString(font, text, fontLocation, tint, 0f, Game1.Utility.Origin, 1f,SpriteEffects.None, layerDepth: .73f);
                            }


                        }
                    }
                }
            }
            TileUtility.DrawGridItem(spriteBatch,this,AllTiles);
            
        }
        
        #endregion

        #region REPLACETILES


        #endregion

        #region INTERACTIONS




        //specify which animations you want to use depending on where the player is in relation to the object

        

        #endregion


       
    }

}