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


using SecretProject.Class.Controls;

using Microsoft.Xna.Framework.Content;
using SecretProject.Class.Universal;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.PathFinding;
using XMLData.ItemStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.CollisionDetection;

namespace SecretProject.Class.TileStuff
{
    /// <summary>
    /// background = 0, buildings = 1, midground =2, foreground =3, 
    /// </summary>
    public class TileManager : ITileManager, IInformationContainer
    {
        public int Type { get; set; }
        protected Game1 game;
        public Texture2D TileSet { get; set; }
        public TmxMap MapName { get;set; }
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
        public Dictionary<string, List<GrassTuft>> Tufts { get; set; }
        public Dictionary<string, int> TileHitPoints { get; set; }
        public Dictionary<string, ICollidable> CurrentObjects { get; set; }
        public Dictionary<string, Chest> Chests { get; set; }
        public List<LightSource> Lights { get; set; }
        public Dictionary<string, ICollidable> Objects { get; set; }
        public Dictionary<float, string> ForeGroundOffSetDictionary { get; set; }

        public bool AbleToDrawTileSelector { get; set; }
        public bool DrawGridObject { get; set; }
        public Rectangle GridObjectSourceRectangle { get; set; }
        public int GridObjectSourceRectangleOffSetX { get; set; }
        public int GridObjectSourceRectangleOffSetY { get; set; }
        public Color GridDrawColor { get; set; }

        public int TileSetDimension { get; set; }

        public Dictionary<string, Crop> Crops { get; set; }

        //not relevant:
        public int X { get; set; }
        public int Y { get; set; }
        public int NumberOfLayers { get; set; }
        public Chunk ChunkUnderPlayer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ArrayI { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ArrayJ { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Chunk[,] ActiveChunks { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Owned { get; set; }
        WorldTileManager IInformationContainer.TileManager { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #region CONSTRUCTORS

        private TileManager()
        {

        }

        public TileManager(Texture2D tileSet, TmxMap mapName, List<TmxLayer> allLayers, GraphicsDevice graphicsDevice, ContentManager content, int tileSetNumber, List<float> allDepths, ILocation currentStage)
        {
            this.Type = 0;
            this.TileSet = tileSet;
            this.MapName = mapName;

            TileWidth = mapName.Tilesets[tileSetNumber].TileWidth;
            TileHeight = mapName.Tilesets[tileSetNumber].TileHeight;

            TileSetDimension = tileSet.Width / TileWidth;

            MapWidth = mapName.Width;
            MapHeight = mapName.Height;

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
            Tufts = new Dictionary<string, List<GrassTuft>>();
            TileHitPoints = new Dictionary<string, int>();
            Objects = new Dictionary<string, ICollidable>();
            CurrentObjects = new Dictionary<string, ICollidable>();
            Lights = new List<LightSource>();
            Chests = new Dictionary<string, Chest>();
            Crops = new Dictionary<string, Crop>();
            Owned = true;
            ForeGroundOffSetDictionary = new Dictionary<float, string>();
            Game1.GlobalClock.DayChanged += this.HandleClockChange;
            this.GridDrawColor = Color.White;
            for (int i = 0; i < allLayers.Count; i++)
            {
                AllTiles.Add(new Tile[mapName.Width, mapName.Height]);

            }


            for (int i = 0; i < AllTiles.Count; i++)
            {
                foreach (TmxLayerTile layerNameTile in AllLayers[i].Tiles)
                {
                    Tile tempTile = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid) { LayerToDrawAt = i };
                   
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

                portal.PortalStart = new Rectangle(portalX - portalWidth/2, portalY - portalHeight/2, portalWidth, portalHeight);

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
           // TileUtility.GenerateTiles(1, 1079, "dirt", 5000, 0, this);
            ////    //redrunestone
            //GenerateTiles(1, 579, "dirt", 50, 0, currentStage);
            //////bluerunestone
            // GenerateTiles(1, 779, "dirt", 100, 0, currentStage);
            //////thunderbirch
                //TileUtility.GenerateTiles(1, 2264, "dirt", 500, 0,this);
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
                for (int i = 0; i < this.MapWidth; i++)
                {
                    for (int j = 0; j < this.MapHeight; j++)
                    {


                                TileUtility.AssignProperties(AllTiles[z][i, j],  z, i, j,this);


                    }
                }
            }
            currentStage.AllLights = this.Lights;
        }

       


        #region LOADTILESOBJECTS
        public void LoadInitialTileObjects(ILocation stage)
        {
            
            PathGrid = new AStarPathFinder(MapWidth, MapHeight, AllTiles, this.Objects);

        }


        #endregion

        #region UPDATE


        public void Update(GameTime gameTime, MouseManager mouse)
        {
            int oldLightCount = this.Lights.Count;
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
            if (endi > this.MapWidth)
            {
                endi = this.MapWidth;
            }
            int endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.GetCurrentStage().Cam.Zoom / 2 / 16) + 2;
            if (endj > this.MapWidth)
            {
                endj = this.MapWidth;
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
                                    TileUtility.FinalizeTile(frameholder.Layer, gameTime,frameholder.OldX, frameholder.OldY, TileUtility.GetDestinationRectangle(AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY]), Game1.GetCurrentStage(),this);
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
                            if (Tufts.ContainsKey(TileKey))
                            {
                                for (int t = 0; t < Tufts[TileKey].Count; t++)
                                {
                                    Tufts[TileKey][t].Update(gameTime);
                                }
                            }
                            if(!CurrentObjects.ContainsKey(TileKey))
                            {
                                if (Objects.ContainsKey(TileKey))
                                {
                                    CurrentObjects.Add(TileKey, Objects[TileKey]);

                                }
                            }
                           

                            if (z == 0)
                            {
                                if(Game1.Player.IsMoving)
                                {
                                    if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                    {
                                        if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("step") && Game1.Player.IsMoving && Game1.Player.Rectangle.Intersects(AllTiles[z][i, j].DestinationRectangle))
                                        {
                                            Game1.SoundManager.PlaySoundEffectFromInt(false, 1, (int)Game1.Utility.GetRequiredTileTool(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["step"]), .75f);
                                        }
                                    }
                                }
                                

                            }

                            if (AllTiles[z][i, j].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle))
                            {

                                if (mouse.IsHoveringTile(AllTiles[z][i, j].DestinationRectangle))
                                {
                                    this.AbleToDrawTileSelector = true;
                                    Game1.Player.UserInterface.TileSelector.IndexX = i;
                                    Game1.Player.UserInterface.TileSelector.IndexY = j;
                                    Game1.Player.UserInterface.TileSelector.WorldX = i * 16;
                                    Game1.Player.UserInterface.TileSelector.WorldY = j * 16;

                                    if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                    {

                                        if (z == 1)
                                        {

                                            if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("destructable"))
                                            {
                                                Game1.Player.UserInterface.DrawTileSelector = true;
                                                Game1.isMyMouseVisible = false;

                                                // Game1.Player.UserInterface.TileSelector. = destinationRectangle.X;
                                                //Game1.Player.UserInterface.TileSelectorY = destinationRectangle.Y;

                                                mouse.ChangeMouseTexture((CursorType)Game1.Utility.GetRequiredTileTool(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["destructable"]));

                                                Game1.myMouseManager.ToggleGeneralInteraction = true;

                                                if (mouse.IsClicked)
                                                {
                                                    TileUtility.InteractWithBuilding(z, gameTime, i, j, AllTiles[z][i, j].DestinationRectangle, Game1.GetCurrentStage(), this);

                                                }

                                            }
                                            //return;

                                        }
                                        if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                        {
                                            if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("action"))
                                            {

                                                TileUtility.ActionHelper(z, i, j, MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["action"], mouse, this);

                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }

                    
                }
            }
            if(this.Lights.Count != oldLightCount)
            {
                Game1.GetCurrentStage().AllLights = this.Lights;
            }
            TileUtility.UpdateGridItem(this, this);
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
            if (endi > this.MapWidth)
            {
                endi = this.MapWidth;
            }
            int endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2 / 16) + 5;
            if (endj > this.MapHeight)
            {
                endj = this.MapWidth;
            }
            if (startj < 0 || endj < 0 || starti < 0 || endi < 0 || endi > MapWidth || endj > MapHeight)
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

                            if (Tufts.ContainsKey(AllTiles[z][i, j].GetTileKey(z)))
                            {
                                for (int t = 0; t < Tufts[AllTiles[z][i, j].GetTileKey(z)].Count; t++)
                                {
                                    Tufts[AllTiles[z][i, j].GetTileKey(z)][t].Draw(spriteBatch);
                                }
                            }
                           

                            if (z == 3)
                            {
                                spriteBatch.Draw(TileSet, new Vector2(AllTiles[z][i, j].DestinationRectangle.X, AllTiles[z][i, j].DestinationRectangle.Y), AllTiles[z][i, j].SourceRectangle, Game1.GlobalClock.TimeOfDayColor,
                                0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z] + AllTiles[z][i, j].LayerToDrawAtZOffSet);

                            }
                            else
                            {
                                spriteBatch.Draw(TileSet, new Vector2(AllTiles[z][i, j].DestinationRectangle.X, AllTiles[z][i, j].DestinationRectangle.Y), AllTiles[z][i, j].SourceRectangle, Color.White,
                                0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z]);
                            }

                            if (Game1.GetCurrentStage().ShowBorders)
                            {
                                spriteBatch.DrawString(Game1.AllTextures.MenuText, i + "," + j, new Vector2(AllTiles[z][i, j].DestinationRectangle.X, AllTiles[z][i, j].DestinationRectangle.Y), Color.White, 0f, Game1.Utility.Origin, .25f, SpriteEffects.None, 1f);
                                //spriteBatch.DrawString(font, text, fontLocation, tint, 0f, Game1.Utility.Origin, 1f,SpriteEffects.None, layerDepth: .73f);
                            }


                        }
                    }
                }
            }
            if (this.DrawGridObject)
            {
                spriteBatch.Draw(TileSet, new Vector2(Game1.Player.UserInterface.TileSelector.WorldX + this.GridObjectSourceRectangleOffSetX,
                    Game1.Player.UserInterface.TileSelector.WorldY + GridObjectSourceRectangleOffSetY), this.GridObjectSourceRectangle,
                    this.GridDrawColor, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[3]);

            }

        }

        public void LoadGeneratableTileLists()
        {
            throw new NotImplementedException();
        }

        public void LoadInitialChunks()
        {
            throw new NotImplementedException();
        }

        public Chunk GetChunkFromPosition(Vector2 entityPosition)
        {
            throw new NotImplementedException();
        }
        public void UpdateCropTile()
        {

                for (int x = 0; x < Crops.Count; x++)
                {
                   Crops.ElementAt(x).Value.CurrentGrowth = Game1.GlobalClock.TotalDays - Crops.ElementAt(x).Value.DayPlanted;

                   Crops.ElementAt(x).Value.UpdateGrowthCycle();

                    TileUtility.UpdateCropTile(Crops.ElementAt(x).Value, Game1.GetCurrentStage(), this);
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

        #endregion

        #endregion

    }

}