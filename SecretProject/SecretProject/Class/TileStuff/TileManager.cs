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
            //GenerateTiles(1, 979, "dirt", 2000, 0, world);
            //    //grass
           // GenerateTiles(1, 1079, "dirt", 5000, 0, world);
            //    //redrunestone
           // GenerateTiles(1, 579, "dirt", 500, 0, world);
            ////bluerunestone
           // GenerateTiles(1, 779, "dirt", 1000, 0, world);
            ////thunderbirch
            //GenerateTiles(1, 2264, "dirt", 5000, 0, world);
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

       
       


        public Rectangle GetSourceRectangleWithoutTile(int gid)
        {
            int Column = gid% tilesetTilesWide;
            int Row = (int)Math.Floor((double)gid/ (double)tilesetTilesWide);

            return new Rectangle(16 * Column, 16 * Row, 16, 16);
        }


        public void GenerateTiles(int layerToPlace, int gid, string placementKey, int frequency, int layerToCheckIfEmpty, ILocation stage)
        {
            List<int> acceptableGenerationTiles;
            switch (placementKey)
            {
                case "dirt":
                    acceptableGenerationTiles = Game1.Utility.DirtGeneratableTiles;

                    break;
                case "sand":
                    acceptableGenerationTiles = Game1.Utility.SandGeneratableTiles;

                    break;
                default:
                    acceptableGenerationTiles = Game1.Utility.DirtGeneratableTiles;

                    break;
            }

            for (int g = 0; g < frequency; g++)
            {
                GenerateRandomTiles(layerToPlace, gid, acceptableGenerationTiles, stage, layerToCheckIfEmpty);
            }
        }
        public void GenerateRandomTiles(int layer, int id, List<int> acceptableTiles, ILocation stage, int comparisonLayer = 0)
        {
            int newTileX = Game1.Utility.RNumber(10, this.mapWidth - 10);
            int newTileY = Game1.Utility.RNumber(10, this.mapHeight - 10);
            if (!CheckIfTileAlreadyExists(newTileX, newTileY, layer) && CheckIfTileMatchesGID(newTileX, newTileY, layer, acceptableTiles, comparisonLayer))
            {

                Tile sampleTile = new Tile(newTileX, newTileY, id);
                if (!MapName.Tilesets[TileSetNumber].Tiles[sampleTile.GID].Properties.ContainsKey("spawnWith"))
                {
                    AllTiles[layer][newTileX, newTileY] = new Tile(newTileX, newTileY, id);
                    return;
                }


                if (MapName.Tilesets[TileSetNumber].Tiles[sampleTile.GID].Properties.ContainsKey("spawnWith"))
                {
                    string value = "";
                    MapName.Tilesets[TileSetNumber].Tiles[sampleTile.GID].Properties.TryGetValue("spawnWith", out value);

                    List<Tile> intermediateNewTiles = new List<Tile>();
                    int[] spawnsWith = Game1.Utility.ParseSpawnsWithKey(value);
                    //sampleTile.SpawnsWith = Game1.Utility.ParseSpawnsWithKey(value);
                    for (int index = 0; index < spawnsWith.Length; index++)
                    {
                        string gidX = "";
                        MapName.Tilesets[TileSetNumber].Tiles[spawnsWith[index]].Properties.TryGetValue("relationX", out gidX);
                        string gidY = "";
                        MapName.Tilesets[TileSetNumber].Tiles[spawnsWith[index]].Properties.TryGetValue("relationY", out gidY);
                        string tilePropertyLayer = "";
                        MapName.Tilesets[TileSetNumber].Tiles[spawnsWith[index]].Properties.TryGetValue("layer", out tilePropertyLayer);
                        int intGidX = int.Parse(gidX);
                        int intGidY = int.Parse(gidY);
                        int intTilePropertyLayer = int.Parse(tilePropertyLayer);
                        //4843,4645,4845,4846,4744,4644,4544,4444,4545,4445,4546,4446,4643,4543,4443,4542,4744
                        int totalGID = MapName.Tilesets[TileSetNumber].Tiles[spawnsWith[index]].Id;

                        //basically, if any tile in the associated tiles already contains a tile in the same layer we'll just stop
                        if (!CheckIfTileAlreadyExists(newTileX + intGidX, newTileY + intGidY, layer))
                        {
                            //intermediateAllTiles.Add(AllTiles[intTilePropertyLayer][newTileX + intGidX, newTileY + intGidY]);
                            intermediateNewTiles.Add(new Tile(newTileX + intGidX, newTileY + intGidY, totalGID + 1) { LayerToDrawAt = intTilePropertyLayer });
                            //AllTiles[intTilePropertyLayer][newTileX + intGidX, newTileY + intGidY] = new Tile(newTileX + intGidX, newTileY + intGidY, totalGID + 1, 100, 100, 100, 100);
                        }
                        else
                        {
                            return;
                        }
                        //AllTiles[intTilePropertyLayer][newTileX + intGidX, newTileY + intGidY] = new Tile(newTileX + intGidX, newTileY + intGidY, totalGID +1, 100, 100, 100, 100);
                    }

                    for (int tileSwapCounter = 0; tileSwapCounter < intermediateNewTiles.Count; tileSwapCounter++)
                    {
                        //intermediateNewTiles[tileSwapCounter] = DebugTile;
                        //AssignProperties(intermediateNewTiles[tileSwapCounter], 0, layer, (int)intermediateNewTiles[tileSwapCounter].X, (int)intermediateNewTiles[tileSwapCounter].Y, stage);
                        TileUtility.AssignProperties(intermediateNewTiles[tileSwapCounter], this.GraphicsDevice, this.MapName, mapWidth, mapHeight, this, this.TileSetNumber, layer, (int)intermediateNewTiles[tileSwapCounter].X, (int)intermediateNewTiles[tileSwapCounter].Y, stage);
                        //AddObject(intermediateNewTiles[tileSwapCounter]);

                        AllTiles[(int)intermediateNewTiles[tileSwapCounter].LayerToDrawAt][(int)intermediateNewTiles[tileSwapCounter].X, (int)intermediateNewTiles[tileSwapCounter].Y] = intermediateNewTiles[tileSwapCounter];
                        //AllTiles[intermediateNewTiles[tileSwapCounter]]
                    }
                    AllTiles[layer][newTileX, newTileY] = new Tile(newTileX, newTileY, id);
                }
            }
        }

        public void DestroySpawnWithTiles(Tile baseTile, int xCoord, int yCoord, ILocation world)
        {
            List<Tile> tilesToReturn = new List<Tile>();
            string value = "";
            MapName.Tilesets[TileSetNumber].Tiles[baseTile.GID].Properties.TryGetValue("spawnWith", out value);

            int[] spawnsWith = Game1.Utility.ParseSpawnsWithKey(value);
            if (spawnsWith != null)
            {
                for (int i = 0; i < spawnsWith.Length; i++)
                {
                    string gidX = "";
                    MapName.Tilesets[TileSetNumber].Tiles[spawnsWith[i]].Properties.TryGetValue("relationX", out gidX);
                    string gidY = "";
                    MapName.Tilesets[TileSetNumber].Tiles[spawnsWith[i]].Properties.TryGetValue("relationY", out gidY);
                    string tilePropertyLayer = "";
                    MapName.Tilesets[TileSetNumber].Tiles[spawnsWith[i]].Properties.TryGetValue("layer", out tilePropertyLayer);
                    int intGidX = int.Parse(gidX);
                    int intGidY = int.Parse(gidY);
                    int intTilePropertyLayer = int.Parse(tilePropertyLayer);

                    int totalGID = MapName.Tilesets[TileSetNumber].Tiles[spawnsWith[i]].Id;
                    //tilesToReturn.Add(AllTiles[intTilePropertyLayer][xCoord, yCoord]);


                    if (world.AllObjects.ContainsKey(AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileKey(intTilePropertyLayer)))
                    {
                        world.AllObjects.Remove(AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileKey(intTilePropertyLayer));
                        if (this.CurrentObjects.ContainsKey(AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileKey(intTilePropertyLayer)))
                        {
                            this.CurrentObjects.Remove(AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileKey(intTilePropertyLayer));
                        }
                    }

                    AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY] = new Tile(xCoord + intGidX, yCoord + intGidY, 0);
                }
            }
        }

        //if the GID is anything other than -1 it means there's something there.
        public bool CheckIfTileAlreadyExists(int tileX, int tileY, int layer)
        {
            if (AllTiles[layer][tileX, tileY].GID != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //by default we're seeing if the background layer has an acceptable tile to overwrite
        public bool CheckIfTileMatchesGID(int tileX, int tileY, int layer, List<int> acceptablTiles, int comparisonLayer = 0)
        {
            for (int i = 0; i < acceptablTiles.Count; i++)
            {
                if (AllTiles[comparisonLayer][tileX, tileY].GID == acceptablTiles[i])
                {
                    return true;
                }
            }
            return false;
        }

        #region LOADTILESOBJECTS
        public void LoadInitialTileObjects(ILocation stage)
        {
            
            PathGrid = new AStarPathFinder(mapWidth, mapHeight, AllTiles, stage.AllObjects);

        }
        #endregion

        #region ADDOBJECTSTOBUILDINGS

        #endregion

        public void ReplaceTilePermanent(int layer, int oldX, int oldY, int gid, ILocation stage)
        {
            Tile ReplaceMenttile = new Tile(AllTiles[layer][oldX, oldY].X, AllTiles[layer][oldX, oldY].Y, gid);
            AllTiles[layer][oldX, oldY] = ReplaceMenttile;
            TileUtility.AssignProperties(AllTiles[layer][oldX, oldY], this.GraphicsDevice, this.MapName, mapWidth, mapHeight, this, this.TileSetNumber, layer,
                oldX, oldY, stage);
            //AssignProperties(AllTiles[layer][oldX, oldY], 0, layer, oldX, oldY, stage);
        }

        #region UPDATE
        public bool AbleToDrawTileSelector { get; set; }
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
                                    Destroy(frameholder.Layer, frameholder.OldX, frameholder.OldY, TileUtility.GetDestinationRectangle(AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY]), Game1.GetCurrentStage());
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
                                                    InteractWithBuilding(z, gameTime, i, j, destinationRectangle, Game1.GetCurrentStage());

                                                }

                                            }
                                            //return;

                                        }
                                        if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                        {
                                            if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("action"))
                                            {

                                                ActionHelper(z, i, j, MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["action"], mouse);

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
        public void ActionHelper(int z, int i, int j, string action, MouseManager mouse)
        {
            //new Gid should be one larger, per usual
            string[] information = Game1.Utility.GetActionHelperInfo(action);
            Game1.Player.UserInterface.TileSelectorX = TileUtility.GetDestinationRectangle(AllTiles[z][i, j]).X;
            Game1.Player.UserInterface.TileSelectorY = TileUtility.GetDestinationRectangle(AllTiles[z][i, j]).Y;
            switch (information[0])
            {
                //including animation frame id to replace!

                case "diggable":
                    if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() == 3)
                    {
                        Game1.isMyMouseVisible = false;
                        Game1.Player.UserInterface.DrawTileSelector = true;
                        Game1.myMouseManager.ToggleGeneralInteraction = true;
                        mouse.ChangeMouseTexture(3);

                        if (mouse.IsClicked)
                        {

                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                            TileUtility.ReplaceTile(z, i, j, 86, this);


                        }
                    }
                    break;

                case "plantable":
                    Game1.isMyMouseVisible = false;
                    Game1.Player.UserInterface.DrawTileSelector = true;
                    Game1.myMouseManager.ToggleGeneralInteraction = true;
                    mouse.ChangeMouseTexture(2);

                    if (mouse.IsClicked)
                    {
                        if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem() != null)
                        {
                            Item testItem = Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem();
                            if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().IsPlantable)
                            {
                                if (!Game1.GetCurrentStage().AllCrops.ContainsKey(AllTiles[1][i, j].GetTileKey(1)))
                                {

                                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                                    Crop tempCrop = Game1.AllCrops.GetCropFromID(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().ID);
                                    tempCrop.TileID = AllTiles[1][i, j].GetTileKey(1);
                                    tempCrop.GID++;
                                    TileUtility.ReplaceTile(1, i, j, tempCrop.GID, this);
                                    Game1.GetCurrentStage().AllCrops[AllTiles[1][i, j].GetTileKey(1)] = tempCrop;
                                    Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().ID);


                                }
                            }
                        }

                    }
                    break;
                case "sanctuaryAdd":
                    if (mouse.IsClicked)
                    {
                        if (Game1.Player.Inventory.FindNumberOfItemInInventory(int.Parse(information[1])) > 0)
                        {
                            int newGID;
                            int relationX;
                            int relationY;
                            int layer;
                            int tileToReplaceGID;

                            if (Game1.SanctuaryCheckList.TryFillRequirement(AllTiles[z][i, j].GID))
                            {
                                if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("spawnWith"))
                                {
                                    newGID = int.Parse(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["spawnWith"]);
                                    relationX = int.Parse(MapName.Tilesets[TileSetNumber].Tiles[newGID].Properties["relationX"]);
                                    relationY = int.Parse(MapName.Tilesets[TileSetNumber].Tiles[newGID].Properties["relationY"]);
                                    layer = int.Parse(MapName.Tilesets[TileSetNumber].Tiles[newGID].Properties["layer"]);
                                    tileToReplaceGID = MapName.Tilesets[TileSetNumber].Tiles[newGID].AnimationFrames[0].Id + 1;
                                    TileUtility.ReplaceTile(layer, i + relationX, j + relationY, tileToReplaceGID, this);
                                }
                                Game1.GetCurrentStage().AddTextToAllStrings(Game1.SanctuaryCheckList.AllRequirements.Find(x => x.GID == AllTiles[z][i, j].GID).Name, new Vector2(TileUtility.GetDestinationRectangle(AllTiles[z][i, j]).X, TileUtility.GetDestinationRectangle(AllTiles[z][i, j]).Y - 10),
                                    TileUtility.GetDestinationRectangle(AllTiles[z][i, j]).X, TileUtility.GetDestinationRectangle(AllTiles[z][i, j]).Y - 100, 2f, 3f);


                                TileUtility.ReplaceTile(z, i, j, MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].AnimationFrames[0].Id + 1,this);

                                Game1.Player.Inventory.RemoveItem(int.Parse(information[1]));
                                Game1.GetCurrentStage().ParticleEngine.Color = Color.LightGoldenrodYellow;
                                Game1.GetCurrentStage().ParticleEngine.ActivationTime = 1f;
                                Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(TileUtility.GetDestinationRectangle(AllTiles[z][i, j]).X + 10, TileUtility.GetDestinationRectangle(AllTiles[z][i, j]).Y - 10);

                                Game1.SoundManager.SanctuaryAdd.Play();
                            }
                        }
                    }
                    break;
                case "chestLoot":
                    if (mouse.IsClicked)
                    {
                        Game1.GetCurrentStage().AllChests[AllTiles[z][i, j].GetTileKey(z)].IsUpdating = true;
                    }
                    break;

                case "smelt":
                    if (mouse.IsClicked)
                    {
                        if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() != -50)
                        {


                            Item tempItem = Game1.ItemVault.GenerateNewItem(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool(), null);
                            if (tempItem.SmeltedItem != 0)
                            {
                                Game1.Player.Inventory.RemoveItem(tempItem.ID);
                                Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem(tempItem.SmeltedItem, null));
                            }
                        }
                    }

                    break;
                case "readSanctuary":
                    if (mouse.IsClicked)
                    {
                        Game1.Player.UserInterface.CurrentOpenInterfaceItem = UI.ExclusiveInterfaceItem.SanctuaryCheckList;
                    }
                    break;
                case "triggerLift":
                    if (mouse.IsClicked)
                    {
                        if (Game1.GetCurrentStage().AllSprites.Any(x => x.ID == 232) && Game1.GetCurrentStage().AllSprites.Any(x => x.ID == 233))
                        {
                            Game1.GetCurrentStage().AllSprites.Find(x => x.ID == 232).IsSpinning = true;
                            Game1.GetCurrentStage().AllSprites.Find(x => x.ID == 233).IsSpinning = true;
                            Game1.SoundManager.GearSpin.Play();
                        }
                    }
                    break;
                case "replaceLargeCog":
                    if (mouse.IsClicked)
                    {
                        if (!Game1.GetCurrentStage().AllSprites.Any(x => x.ID == 232))
                        {
                            if (Game1.Player.Inventory.FindNumberOfItemInInventory(232) > 0)
                            {
                                TileUtility.ReplaceTile(3, i, j, -1,this);
                                Game1.GetCurrentStage().AllSprites.Add(new Sprite(this.GraphicsDevice, Game1.AllTextures.Gears, new Rectangle(48, 0, 16, 16), new Vector2(TileUtility.GetDestinationRectangle(AllTiles[z][i, j]).X + 8,
                                    TileUtility.GetDestinationRectangle(AllTiles[z][i, j]).Y + 8), 16, 16)
                                { ID = 232, SpinAmount = 10f, SpinSpeed = 2f, Origin = new Vector2(8, 8) });
                                Game1.SoundManager.CraftMetal.Play();
                                Game1.Player.Inventory.RemoveItem(232);

                            }

                        }
                    }

                    break;
                case "replaceSmallCog":
                    if (mouse.IsClicked)
                    {
                        if (!Game1.GetCurrentStage().AllSprites.Any(x => x.ID == 233))
                        {
                            if (Game1.Player.Inventory.FindNumberOfItemInInventory(233) > 0)
                            {
                                TileUtility.ReplaceTile(3, i, j, -1,this);

                                Game1.GetCurrentStage().AllSprites.Add(new Sprite(this.GraphicsDevice, Game1.AllTextures.Gears, new Rectangle(16, 0, 16, 16),
                                    new Vector2(TileUtility.GetDestinationRectangle(AllTiles[z][i, j]).X + 8, TileUtility.GetDestinationRectangle(AllTiles[z][i, j]).Y + 5), 16, 16)
                                { ID = 233, SpinAmount = -10f, SpinSpeed = 2f, Origin = new Vector2(8, 8) });
                                Game1.SoundManager.CraftMetal.Play();
                                Game1.Player.Inventory.RemoveItem(233);

                            }
                        }
                    }
                    break;


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
            DrawGridItem(spriteBatch);
            
        }
        public void DrawGridItem(SpriteBatch spriteBatch)
        {
            if (this.AbleToDrawTileSelector)
            {
                if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem() != null)
                {


                    if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().PlaceID != 0)
                    {
                        int[] associatedTiles = new int[0];
                        int placeID = Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().PlaceID;
                        Rectangle sourceRectangle = GetSourceRectangleWithoutTile(placeID);
                        if (AllTiles[1][Game1.Player.UserInterface.TileSelectorX / 16, Game1.Player.UserInterface.TileSelectorY / 16].GID != -1)
                        {

                            spriteBatch.Draw(TileSet, new Vector2(Game1.Player.UserInterface.TileSelectorX, Game1.Player.UserInterface.TileSelectorY), sourceRectangle, Color.Red * .5f,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[1]);
                            if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(placeID))
                            {
                                if (MapName.Tilesets[TileSetNumber].Tiles[placeID].Properties.ContainsKey("AssociatedTiles"))
                                {

                                    associatedTiles = Game1.Utility.ParseSpawnsWithKey(MapName.Tilesets[TileSetNumber].Tiles[placeID].Properties["AssociatedTiles"]);
                                }
                            }
                        }

                        else
                        {
                            spriteBatch.Draw(TileSet, new Vector2(Game1.Player.UserInterface.TileSelectorX, Game1.Player.UserInterface.TileSelectorY), sourceRectangle, Color.Green * .5f,
                                        0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[1]);
                            if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(placeID))
                            {


                                if (MapName.Tilesets[TileSetNumber].Tiles[placeID].Properties.ContainsKey("AssociatedTiles"))
                                {

                                    associatedTiles = Game1.Utility.ParseSpawnsWithKey(MapName.Tilesets[TileSetNumber].Tiles[placeID].Properties["AssociatedTiles"]);
                                    for (int a = 0; a < associatedTiles.Length; a++)
                                    {
                                        spriteBatch.Draw(TileSet, new Vector2(Game1.Player.UserInterface.TileSelectorX + int.Parse(MapName.Tilesets[TileSetNumber].Tiles[associatedTiles[a]].Properties["relationX"]) * 16,
                                            Game1.Player.UserInterface.TileSelectorY + int.Parse(MapName.Tilesets[TileSetNumber].Tiles[associatedTiles[a]].Properties["relationY"]) * 16), GetSourceRectangleWithoutTile(associatedTiles[a]), Color.Green * .5f,
                                            0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[int.Parse(MapName.Tilesets[TileSetNumber].Tiles[associatedTiles[a]].Properties["layer"])]);
                                    }
                                }
                            }
                            if (Game1.myMouseManager.IsClicked)
                            {
                                if (Game1.Player.UserInterface.CurrentOpenInterfaceItem != UI.ExclusiveInterfaceItem.ShopMenu)
                                {


                                    if (associatedTiles.Length > 0)
                                    {
                                        for (int a = 0; a < associatedTiles.Length; a++)
                                        {
                                            ReplaceTilePermanent(int.Parse(MapName.Tilesets[TileSetNumber].Tiles[associatedTiles[a]].Properties["layer"]), Game1.Player.UserInterface.TileSelectorX / 16 + int.Parse(MapName.Tilesets[TileSetNumber].Tiles[associatedTiles[a]].Properties["relationX"]),
                                                Game1.Player.UserInterface.TileSelectorY / 16 + int.Parse(MapName.Tilesets[TileSetNumber].Tiles[associatedTiles[a]].Properties["relationY"]), associatedTiles[a] + 1, Game1.GetCurrentStage());
                                        }
                                    }
                                    int soundRandom = Game1.Utility.RGenerator.Next(0, 2);
                                    switch (soundRandom)
                                    {
                                        case 0:
                                            Game1.SoundManager.PlaceItem1.Play();
                                            break;
                                        case 1:
                                            Game1.SoundManager.PlaceItem2.Play();
                                            break;
                                    }
                                    ReplaceTilePermanent(1, Game1.Player.UserInterface.TileSelectorX / 16, Game1.Player.UserInterface.TileSelectorY / 16, placeID + 1, Game1.GetCurrentStage());
                                    Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool());
                                }
                            }
                        }

                    }

                }
            }



        }
        #endregion

        #region REPLACETILES

        public void UpdateCropTile(Crop crop, ILocation stage)
        {
            string tileID = crop.TileID; ;
            int x = int.Parse(tileID.Substring(1, 4));
            int y = int.Parse(tileID.Substring(5, 4));
            ReplaceTilePermanent(1, x, y, crop.GID, stage);
            if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(crop.GID - 1))
            {
                if (MapName.Tilesets[TileSetNumber].Tiles[crop.GID - 1].Properties.ContainsKey("AssociatedTiles"))
                {
                    ReplaceTilePermanent(3, x, y - 1, int.Parse(MapName.Tilesets[TileSetNumber].Tiles[crop.GID - 1].Properties["AssociatedTiles"]), stage);
                }
            }
        }


        #endregion

        #region INTERACTIONS



        public void InteractWithBuilding(int layer, GameTime gameTime, int oldX, int oldY, Rectangle destinationRectangle, ILocation world)
        {

                if (!AnimationFrames.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer)) && !Game1.Player.CurrentAction[0, 0].IsAnimated)
                {
                    if (Game1.Utility.GetRequiredTileTool(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]) == -50)
                    {
                        FinalizeTile(layer, gameTime, oldX, oldY, destinationRectangle, world, delayTimer: .25f);
                        if (TileHitPoints.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer)))
                        {
                            TileHitPoints[AllTiles[layer][oldX, oldY].GetTileKey(layer)]--;
                        }

                    }
                    else if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() == Game1.Utility.GetRequiredTileTool(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]))
                    {
                        switch (Game1.Utility.GetRequiredTileTool(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]))
                        {
                            //bare hands

                            case 0:
                                //InteractWithoutPlayerAnimation(3, gameTime, oldX, oldY - 1, .25f);
                                DoPlayerAnimation(layer, gameTime, oldX, oldY, 9, 10, 11, 12, destinationRectangle, .25f);
                                ToolInteraction(AllTiles[layer][oldX, oldY], layer, oldX, oldY, Game1.Utility.GetTileDestructionSound(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]),
                                    Game1.Utility.GetTileEffectColor(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), world, destinationRectangle, MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"));
                                if (TileHitPoints.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer)))
                                {
                                    TileHitPoints[AllTiles[layer][oldX, oldY].GetTileKey(layer)]--;
                                    //Game1.Player.Inventory.currentInventory.Find(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool()
                                }

                                break;

                            case 1:
                                //InteractWithoutPlayerAnimation(3, gameTime, oldX, oldY - 1, .25f);
                                DoPlayerAnimation(layer, gameTime, oldX, oldY, 5, 6, 7, 8, destinationRectangle, .25f);
                                ToolInteraction(AllTiles[layer][oldX, oldY], layer, oldX, oldY, Game1.Utility.GetTileDestructionSound(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]),
                                    Game1.Utility.GetTileEffectColor(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), world, destinationRectangle, MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"));
                                if (TileHitPoints.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer)))
                                {
                                    TileHitPoints[AllTiles[layer][oldX, oldY].GetTileKey(layer)]--;
                                }
                                break;
                            case 2:
                                //InteractWithoutPlayerAnimation(3, gameTime, oldX, oldY - 1, .25f);
                                DoPlayerAnimation(layer, gameTime, oldX, oldY, 1, 2, 3, 4, destinationRectangle, .25f);
                                ToolInteraction(AllTiles[layer][oldX, oldY], layer, oldX, oldY, Game1.Utility.GetTileDestructionSound(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]),
                                    Game1.Utility.GetTileEffectColor(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), world, destinationRectangle, MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"));
                                if (TileHitPoints.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer)))
                                {
                                    TileHitPoints[AllTiles[layer][oldX, oldY].GetTileKey(layer)]--;
                                }
                                break;
                        }

                    }

                }
            

        }

        public void ToolInteraction(Tile tile, int layer, int x, int y, int setSoundInt, Color particleColor, ILocation world, Rectangle destinationRectangle, bool hasSpawnTiles = false)
        {
            if (TileHitPoints.ContainsKey(AllTiles[layer][x, y].GetTileKey(layer)))
            {

                if (TileHitPoints[tile.GetTileKey(layer)] > 0)
                {
                    Game1.SoundManager.PlaySoundEffectFromInt(false, 1, setSoundInt, 1f);
                    Game1.GetCurrentStage().ParticleEngine.Color = particleColor;
                    Game1.GetCurrentStage().ParticleEngine.ActivationTime = .25f;
                    Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(destinationRectangle.X + 5, destinationRectangle.Y - 20);
                    return;
                }

                if (TileHitPoints[tile.GetTileKey(layer)] < 1)
                {
                    Game1.SoundManager.PlaySoundEffectFromInt(false, 1, setSoundInt, 1f);
                    TileHitPoints.Remove(tile.GetTileKey(layer));
                    if (hasSpawnTiles)
                    {
                        DestroySpawnWithTiles(tile, x, y, world);
                    }
                    if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][x, y].GID].Properties.ContainsKey("AssociatedTiles"))
                    {
                        int[] associatedTiles = Game1.Utility.ParseSpawnsWithKey(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][x, y].GID].Properties["AssociatedTiles"]);

                        for (int i = 0; i < associatedTiles.Length; i++)
                        {
                            if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(associatedTiles[i]))
                            {
                                if (MapName.Tilesets[TileSetNumber].Tiles[associatedTiles[i]].AnimationFrames.Count > 0)
                                {


                                    List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                                    for (int j = 0; j < MapName.Tilesets[TileSetNumber].Tiles[associatedTiles[i]].AnimationFrames.Count; j++)
                                    {
                                        frames.Add(new EditableAnimationFrame(MapName.Tilesets[TileSetNumber].Tiles[associatedTiles[i]].AnimationFrames[j]));
                                    }
                                    EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, x, y, layer, associatedTiles[i]);
                                    this.AnimationFrames.Add(AllTiles[layer][x, y - 1].GetTileKey(layer), frameHolder);
                                }
                            }
                        }
                    }
                }
                if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][x, y].GID].AnimationFrames.Count > 0)
                {
                    List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                    for (int i = 0; i < MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][x, y].GID].AnimationFrames.Count; i++)
                    {
                        frames.Add(new EditableAnimationFrame(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][x, y].GID].AnimationFrames[i]));
                    }
                    EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, x, y, layer, AllTiles[layer][x, y].GID);
                    this.AnimationFrames.Add(AllTiles[layer][x, y].GetTileKey(layer), frameHolder);
                }
                else
                {
                    Destroy(layer, x, y, destinationRectangle, world);
                }

                Game1.GetCurrentStage().ParticleEngine.Color = particleColor;
                Game1.GetCurrentStage().ParticleEngine.ActivationTime = 1f;
                Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(destinationRectangle.X, destinationRectangle.Y);
            }
        }

        public void FinalizeTile(int layer, GameTime gameTime, int oldX, int oldY, Rectangle destinationRectangle, ILocation world, float delayTimer = 0f)
        {
            if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].AnimationFrames.Count > 0)
            {
                List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                for (int i = 0; i < MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].AnimationFrames.Count; i++)
                {
                    frames.Add(new EditableAnimationFrame(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].AnimationFrames[i]));
                }
                EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, oldX, oldY, layer, AllTiles[layer][oldX, oldY].GID);
                this.AnimationFrames.Add(AllTiles[layer][oldX, oldY].GetTileKey(layer), frameHolder);
            }

            if (Game1.GetCurrentStage().AllObjects.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer)))
            {
                Game1.GetCurrentStage().AllObjects.Remove(AllTiles[layer][oldX, oldY].GetTileKey(layer));
                if (this.CurrentObjects.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer)))
                {
                    this.CurrentObjects.Remove(AllTiles[layer][oldX, oldY].GetTileKey(layer));
                }
            }
            //AllTiles[layer][oldX, oldY].HasObject = false;
            if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"))
            {

                DestroySpawnWithTiles(AllTiles[layer][oldX, oldY], oldX, oldY, world);
            }
            //mostly for crops

            GetDrop(layer, oldX, oldY, destinationRectangle);
            if(Game1.GetCurrentStage().AllCrops.ContainsKey(AllTiles[1][oldX, oldY].GetTileKey(layer)))
            {
                Game1.GetCurrentStage().AllCrops.Remove(AllTiles[1][oldX, oldY].GetTileKey(layer));
                if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("AssociatedTiles"))
                {
                    ReplaceTilePermanent(3, oldX, oldY - 1, 0, world);
                }
            }
            Game1.SoundManager.PlaySoundEffectFromInt(false, 1, Game1.Utility.GetTileDestructionSound(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), 1f);

            ReplaceTilePermanent(layer, oldX, oldY, 0, world);


        }

        //specify which animations you want to use depending on where the player is in relation to the object

        public void DoPlayerAnimation(int layer, GameTime gameTime, int oldX, int oldY, int down, int right, int left, int up, Rectangle destinationRectangle, float delayTimer = 0f)
        {



            if (Game1.Player.Position.Y < destinationRectangle.Y - 30)
            {
                Game1.Player.controls.Direction = Dir.Down;
                Game1.Player.PlayAnimation(gameTime, down);
            }

            else if (Game1.Player.Position.Y > destinationRectangle.Y)
            {
                Game1.Player.controls.Direction = Dir.Up;
                Game1.Player.PlayAnimation(gameTime, up);
            }

            else if (Game1.Player.Position.X < destinationRectangle.X)
            {
                Game1.Player.controls.Direction = Dir.Right;
                Game1.Player.PlayAnimation(gameTime, right);
            }
            else if (Game1.Player.Position.X > destinationRectangle.X)
            {
                Game1.Player.controls.Direction = Dir.Left;
                Game1.Player.PlayAnimation(gameTime, left);
            }

        }

        #endregion

        #region DESTROYTILES

        public void Destroy(int layer, int oldX, int oldY, Rectangle destinationRectangle, ILocation world)
        {
            if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[layer][oldX, oldY].GID))
            {
                if (!AnimationFrames.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer)) && Game1.GetCurrentStage().AllObjects.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer)))
                {
                    //ObjectBody newObject = new ObjectBody();
                    Game1.GetCurrentStage().AllObjects.Remove(AllTiles[layer][oldX, oldY].GetTileKey(layer));
                    if (this.CurrentObjects.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer)))
                    {
                        this.CurrentObjects.Remove(AllTiles[layer][oldX, oldY].GetTileKey(layer));
                    }
                    GetDrop(layer, oldX, oldY, destinationRectangle);
                    if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"))
                    {
                        DestroySpawnWithTiles(AllTiles[layer][oldX, oldY], oldX, oldY, world);
                    }



                    ReplaceTilePermanent(layer, oldX, oldY, 0, world);

                }
                else
                {
                    if(Game1.GetCurrentStage().AllObjects.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer)))
                    {
                        Game1.GetCurrentStage().AllObjects.Remove(AllTiles[layer][oldX, oldY].GetTileKey(layer));
                        if (this.CurrentObjects.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer)))
                        {
                            this.CurrentObjects.Remove(AllTiles[layer][oldX, oldY].GetTileKey(layer));
                        }
                    }
                    
                    GetDrop(layer, oldX, oldY, destinationRectangle);
                    //AllTiles[layer][oldX, oldY].ContainsCrop = false;
                    if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"))
                    {
                        DestroySpawnWithTiles(AllTiles[layer][oldX, oldY], oldX, oldY, world);
                    }
                    ReplaceTilePermanent(layer, oldX, oldY, 0, world);
                }
            }

        }

        public void GetDrop(int layer, int x, int y, Rectangle destinationRectangle)
        {
            int gid = AllTiles[layer][x, y].GID;
            List<Loot> tempLoot = Game1.Utility.Parselootkey(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][x, y].GID].Properties["loot"]);

            if (tempLoot != null)
            {


                for (int l = 0; l < tempLoot.Count; l++)
                {
                    int lootCount = Game1.Utility.DetermineLootDrop(tempLoot[l]);
                    for (int d = 0; d < lootCount; d++)
                    {
                        Item item = Game1.ItemVault.GenerateNewItem(tempLoot[l].ID, new Vector2(destinationRectangle.X, destinationRectangle.Y), true);
                        Game1.GetCurrentStage().AllItems.Add(item);
                    }
                }
            }

        }

        #region ANIMATIONFRAMES
       
    }
    #endregion
    #endregion
}