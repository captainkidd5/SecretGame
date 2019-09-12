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

        protected Texture2D tileSet;
        protected TmxMap MapName;
        protected TmxLayer layerName;


        public int tilesetTilesWide { get; set; }
        public int tilesetTilesHigh { get; set; }
        public int mapWidth { get; set; }
        public int mapHeight { get; set; }


        public int iD { get; set; }
        public int tileWidth { get; set; }
        public int tileHeight { get; set; }
        public int tileNumber { get; set; }



        public int tileCounter { get; set; }


        public Tile[,] Tiles { get; set; }


        public bool isActive = false;
        public bool isPlacement { get; set; } = false;

        public bool isInClickingRangeOfPlayer = false;
        ContentManager content;

        GraphicsDevice graphicsDevice;

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

        public List<float> AllDepths;

        public bool TileInteraction { get; set; } = false;

        public Tile DebugTile { get; set; } = new Tile(40, 40, 4714, 100, 100, 100, 100);

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
            this.tileSet = tileSet;
            this.MapName = mapName;

            tileWidth = mapName.Tilesets[tileSetNumber].TileWidth;
            tileHeight = mapName.Tilesets[tileSetNumber].TileHeight;

            tilesetTilesWide = tileSet.Width / tileWidth;
            tilesetTilesHigh = tileSet.Height / tileHeight;

            mapWidth = mapName.Width;
            mapHeight = mapName.Height;

            this.tileCounter = 0;

            this.graphicsDevice = graphicsDevice;
            this.content = content;

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
                    Tile tempTile = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight);
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
                        if (AllTiles[z][i, j].GID != 0)
                        {
                            if (mapName.Tilesets[tileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                            {

                                AssignProperties(AllTiles[z][i, j], 0, z, i, j, currentStage);

                            }
                        }
                    }
                }
            }

        }
        public int NumberOfLayers { get; set; }
        //FOR PROCEDURAL
        #region PROCEDURALGENERATION CONSTRUCTOR
        public TileManager(World world, Texture2D tileSet, List<TmxLayer> allLayers, TmxMap mapName, int numberOfLayers, int worldWidth, int worldHeight, GraphicsDevice graphicsDevice, ContentManager content, int tileSetNumber, List<float> allDepths, World currentStage)
        {
            this.MapName = mapName;
            this.tileSet = tileSet;

            tileWidth = 16;
            tileHeight = 16;

            tilesetTilesWide = tileSet.Width / tileWidth;
            tilesetTilesHigh = tileSet.Height / tileHeight;

            mapWidth = worldWidth;
            mapHeight = worldHeight;
            this.mapWidth = worldWidth;
            this.mapHeight = worldHeight;

            this.NumberOfLayers = numberOfLayers;

            this.tileCounter = 0;

            this.graphicsDevice = graphicsDevice;
            this.content = content;

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
                            AllTiles[z][i, j] = new Tile(i, j, 0, tilesetTilesWide, tilesetTilesHigh, worldWidth, worldHeight);
                        }

                        else
                        {
                            if (Game1.Utility.RFloat(0, 1) > chanceToBeDirt)
                            {
                                AllTiles[z][i, j] = new Tile(i, j, 1106, tilesetTilesWide, tilesetTilesHigh, worldWidth, worldHeight);
                            }
                            else
                            {
                                AllTiles[z][i, j] = new Tile(i, j, 1116, tilesetTilesWide, tilesetTilesHigh, worldWidth, worldHeight);
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
                    TileUtility.ReassignTileForTiling(this.AllTiles, i, j, worldWidth, worldHeight);
                    if (Game1.Utility.RGenerator.Next(1, TileUtility.GrassSpawnRate) == 5)
                    {
                        if (Game1.Utility.GrassGeneratableTiles.Contains(AllTiles[0][i, j].GID))
                        {

                            int numberOfGrassTuftsToSpawn = Game1.Utility.RGenerator.Next(1, 4);
                            List<GrassTuft> tufts = new List<GrassTuft>();
                            for (int g = 0; g < numberOfGrassTuftsToSpawn; g++)
                            {
                                int grassType = Game1.Utility.RGenerator.Next(1, 4);
                                tufts.Add(new GrassTuft(grassType, new Vector2(GetDestinationRectangle(AllTiles[0][i, j]).X, GetDestinationRectangle(AllTiles[0][i, j]).Y)));

                            }
                            this.AllTufts[AllTiles[0][i, j].GetTileKey(0, mapWidth, mapHeight)] = tufts;
                        }
                    }
                }
            }

            Portal portal = new Portal(3, 2, 0, -50, true);
            TileUtility.SpawnBaseCamp(AllTiles, tilesetTilesWide, tilesetTilesHigh, worldWidth, worldHeight);



            portal.PortalStart = new Rectangle(worldWidth * 16 / 2 + 120, worldHeight * 16 / 2 + 120, 50, 50);

            currentStage.AllPortals.Add(portal);
            Game1.PortalGraph.AddEdge(portal.From, portal.To);
            //    }

            //specify GID which is 1 larger than one on tileset, idk why
            //brown tall grass
            // GenerateTiles(3, 6394, "dirt", 2000, 0);
            //green tall grass
            // GenerateTiles(3, 6393, "dirt", 2000, 0,world);
            //stone
            GenerateTiles(1, 979, "dirt", 2000, 0, world);
            //    //grass
            GenerateTiles(1, 1079, "dirt", 5000, 0, world);
            //    //redrunestone
            GenerateTiles(1, 579, "dirt", 500, 0, world);
            ////bluerunestone
            GenerateTiles(1, 779, "dirt", 1000, 0, world);
            ////thunderbirch
            GenerateTiles(1, 2264, "dirt", 5000, 0, world);
            //////crown of swords
            //GenerateTiles(1, 6388, "sand", 50, 0);
            //////dandelion
            //GenerateTiles(1, 6687, "sand", 100, 0);
            ////juicyfruit
            GenerateTiles(1, 1586, "dirt", 500, 0, world);
            ////orchardTree
            GenerateTiles(1, 1664, "dirt", 800, 0, world);
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

                                AssignProperties(AllTiles[z][i, j], 0, z, i, j, currentStage);

                            }
                        }
                    }
                }
            }

        }

        #endregion
        #endregion
        public void AssignProperties(Tile tileToAssign, int tileSetNumber, int layer, int oldX, int oldY, ILocation stage)
        {
            if (MapName.Tilesets[tileSetNumber].Tiles.ContainsKey(tileToAssign.GID))
            {
                if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].AnimationFrames.Count > 0 && !MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("idleStart"))
                {
                    List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                    for (int i = 0; i < MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].AnimationFrames.Count; i++)
                    {
                        frames.Add(new EditableAnimationFrame(MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].AnimationFrames[i]));
                    }
                    EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, oldX, oldY, layer, tileToAssign.GID);
                    this.AnimationFrames.Add(tileToAssign.GetTileKey(layer, this.mapWidth, this.mapHeight), frameHolder);
                }
                if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("lightSource"))
                {
                    int lightType = int.Parse(MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["lightSource"]);
                    stage.AllLights.Add(new LightSource(lightType, new Vector2(GetDestinationRectangle(tileToAssign).X, GetDestinationRectangle(tileToAssign).Y)));
                }


                if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("destructable"))
                {
                    TileHitPoints[tileToAssign.GetTileKey(layer, mapWidth, mapHeight)] = Game1.Utility.GetTileHitpoints(MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["destructable"]);

                }

                if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("layer"))
                {
                    tileToAssign.LayerToDrawAt = int.Parse(MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["layer"]);
                    //grass = 1, stone = 2, wood = 3, sand = 4
                }

                if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("action"))
                {
                    if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["action"] == "chestLoot")
                    {
                        if (!stage.AllChests.ContainsKey(tileToAssign.GetTileKey(layer, mapWidth, mapHeight)))
                        {
                            stage.AllChests.Add(tileToAssign.GetTileKey(layer, mapWidth, mapHeight), new Chest(tileToAssign.GetTileKey(layer, mapWidth, mapHeight), 3,
                                    new Vector2(tileToAssign.X % mapWidth * 16,
                               tileToAssign.Y % mapHeight * 16), this.graphicsDevice, true));
                        }

                    }
                }
                if (layer == 3)
                {
                    int randomInt = Game1.Utility.RGenerator.Next(1, 1000);
                    float randomFloat = (float)(randomInt * .0000001);
                    tileToAssign.LayerToDrawAtZOffSet = (GetDestinationRectangle(tileToAssign).Top + GetDestinationRectangle(tileToAssign).Height) * .00001f + randomFloat;
                }
            }
        }

        public Rectangle GetDestinationRectangle(Tile tile)
        {
            int Column = tile.GID % tilesetTilesWide;
            int Row = (int)Math.Floor((double)tile.GID / (double)tilesetTilesWide);

            float X = (tile.X % mapWidth) * 16;
            float Y = (tile.Y % mapHeight) * 16;



            return new Rectangle((int)X, (int)Y, 16, 16);
        }

        public Rectangle GetSourceRectangle(Tile tile)
        {
            int Column = tile.GID % tilesetTilesWide;
            int Row = (int)Math.Floor((double)tile.GID / (double)tilesetTilesWide);

            float X = (tile.X % mapWidth) * 16;
            float Y = (tile.Y % mapHeight) * 16;

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

                Tile sampleTile = new Tile(newTileX, newTileY, id, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight);
                if (!MapName.Tilesets[TileSetNumber].Tiles[sampleTile.GID].Properties.ContainsKey("spawnWith"))
                {
                    AllTiles[layer][newTileX, newTileY] = new Tile(newTileX, newTileY, id, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight);
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
                            intermediateNewTiles.Add(new Tile(newTileX + intGidX, newTileY + intGidY, totalGID + 1, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight) { LayerToDrawAt = intTilePropertyLayer });
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
                        AssignProperties(intermediateNewTiles[tileSwapCounter], 0, layer, (int)intermediateNewTiles[tileSwapCounter].X, (int)intermediateNewTiles[tileSwapCounter].Y, stage);
                        //AddObject(intermediateNewTiles[tileSwapCounter]);

                        AllTiles[(int)intermediateNewTiles[tileSwapCounter].LayerToDrawAt][(int)intermediateNewTiles[tileSwapCounter].X, (int)intermediateNewTiles[tileSwapCounter].Y] = intermediateNewTiles[tileSwapCounter];
                        //AllTiles[intermediateNewTiles[tileSwapCounter]]
                    }
                    AllTiles[layer][newTileX, newTileY] = new Tile(newTileX, newTileY, id, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight);
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


                    if (world.AllObjects.ContainsKey(AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileKey(intTilePropertyLayer, this.mapWidth, this.mapHeight)))
                    {
                        world.AllObjects.Remove(AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileKey(intTilePropertyLayer, this.mapWidth, this.mapHeight));
                        if (this.CurrentObjects.ContainsKey(AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileKey(intTilePropertyLayer, this.mapWidth, this.mapHeight)))
                        {
                            this.CurrentObjects.Remove(AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileKey(intTilePropertyLayer, this.mapWidth, this.mapHeight));
                        }
                    }

                    AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY] = new Tile(xCoord + intGidX, yCoord + intGidY, 0, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight);
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
            for (int z = 0; z < AllTiles.Count; z++)
            {
                if (z == 1)
                {
                    for (var i = 0; i < mapWidth; i++)
                    {
                        for (var j = 0; j < mapHeight; j++)
                        {
                            int testGID = AllTiles[z][i, j].GID;
                            if (AllTiles[z][i, j].GID != -1)
                            {

                                if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                {

                                    if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].ObjectGroups.Count > 0)
                                    {


                                        for (int k = 0; k < MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].ObjectGroups[0].Objects.Count; k++)
                                        {
                                            TmxObject tempObj = MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].ObjectGroups[0].Objects[k];


                                            ObjectBody tempObjectBody = new ObjectBody(graphicsDevice,
                                                new Rectangle(GetDestinationRectangle(AllTiles[z][i, j]).X + (int)Math.Ceiling(tempObj.X),
                                                GetDestinationRectangle(AllTiles[z][i, j]).Y + (int)Math.Ceiling(tempObj.Y) - 5, (int)Math.Ceiling(tempObj.Width),
                                                (int)Math.Ceiling(tempObj.Height) + 5), AllTiles[z][i, j].GID);

                                            string key = AllTiles[z][i, j].GetTileKey(z, this.mapWidth, this.mapHeight);

                                            stage.AllObjects.Add(key, tempObjectBody); // not gonna work for saving, gotta figure out.

                                        }

                                    }
                                }
                            }
                        }

                    }
                }
            }
            PathGrid = new AStarPathFinder(mapWidth, mapHeight, AllTiles, stage.AllObjects);

        }
        #endregion

        #region ADDOBJECTSTOBUILDINGS

        #endregion

        public void ReplaceTilePermanent(int layer, int oldX, int oldY, int gid, ILocation stage)
        {
            Tile ReplaceMenttile = new Tile(AllTiles[layer][oldX, oldY].X, AllTiles[layer][oldX, oldY].Y, gid, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight);
            AllTiles[layer][oldX, oldY] = ReplaceMenttile;
            AssignProperties(AllTiles[layer][oldX, oldY], 0, layer, oldX, oldY, stage);
        }

        #region UPDATE


        public void Update(GameTime gameTime, MouseManager mouse)
        {
            CurrentObjects.Clear();
            //Game1.myMouseManager.TogglePlantInteraction = false;
            Game1.Player.UserInterface.DrawTileSelector = false;
            List<string> AnimationFrameKeysToRemove = new List<string>();
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
                endi = (int)(Game1.cam.Pos.X / 16) + (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2 / 16) + 1;
            }
            int endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2 / 16) + 2;
            if (endj > this.mapWidth)
            {
                endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2 / 16) + 1;
            }

            foreach (EditableAnimationFrameHolder frameholder in AnimationFrames.Values)
            {
                frameholder.Frames[frameholder.Counter].CurrentDuration -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameholder.Frames[frameholder.Counter].CurrentDuration <= 0)
                {
                    frameholder.Frames[frameholder.Counter].CurrentDuration = frameholder.Frames[frameholder.Counter].AnchorDuration;
                    AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY] = new Tile(frameholder.OldX, frameholder.OldY,
                        frameholder.Frames[frameholder.Counter].ID + 1, this.tilesetTilesWide, this.tilesetTilesHigh, this.mapWidth, this.mapHeight);
                    if (frameholder.Counter == frameholder.Frames.Count - 1)
                    {
                        if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(frameholder.OriginalTileID))
                        {

                            if (MapName.Tilesets[TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("destructable") || MapName.Tilesets[TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("relationX"))
                            {

                                //needs to refer to first tile ?
                                int frameolDX = frameholder.OldX;
                                AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY] = new Tile(frameholder.OldX, frameholder.OldY, frameholder.OriginalTileID + 1,
                                    this.tilesetTilesWide, this.tilesetTilesHigh, this.mapWidth, this.mapHeight);
                                AnimationFrameKeysToRemove.Add(AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].GetTileKey(frameholder.Layer, this.mapWidth, this.mapHeight));
                                if (MapName.Tilesets[TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("destructable"))
                                {
                                    Destroy(frameholder.Layer, frameholder.OldX, frameholder.OldY, GetDestinationRectangle(AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY]), Game1.GetCurrentStage());
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
                            string TileKey = AllTiles[z][i, j].GetTileKey(z, this.mapWidth, this.mapHeight);
                            Rectangle destinationRectangle = GetDestinationRectangle(AllTiles[z][i, j]);
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
            Game1.Player.UserInterface.TileSelectorX = GetDestinationRectangle(AllTiles[z][i, j]).X;
            Game1.Player.UserInterface.TileSelectorY = GetDestinationRectangle(AllTiles[z][i, j]).Y;
            switch (information[0])
            {
                //including animation frame id to replace!

                case "diggable":
                    Game1.isMyMouseVisible = false;
                    Game1.Player.UserInterface.DrawTileSelector = true;
                    Game1.myMouseManager.ToggleGeneralInteraction = true;
                    mouse.ChangeMouseTexture(3);

                    if (mouse.IsClicked)
                    {
                        if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() == 3)
                        {
                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                            ReplaceTile(z, i, j, 86);

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
                                if (!Game1.GetCurrentStage().AllCrops.ContainsKey(AllTiles[z][i, j].GetTileKey(z, this.mapWidth, this.mapHeight)))
                                {

                                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                                    Crop tempCrop = Game1.AllCrops.GetCropFromID(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().ID);
                                    tempCrop.TileID = AllTiles[z][i, j].GetTileKey(z, this.mapWidth, this.mapHeight);
                                    tempCrop.GID++;
                                    ReplaceTile(1, i, j, tempCrop.GID);
                                    Game1.GetCurrentStage().AllCrops[AllTiles[z][i, j].GetTileKey(z, this.mapWidth, this.mapHeight)] = tempCrop;
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
                                    ReplaceTile(layer, i + relationX, j + relationY, tileToReplaceGID);
                                }
                                Game1.GetCurrentStage().AddTextToAllStrings(Game1.SanctuaryCheckList.AllRequirements.Find(x => x.GID == AllTiles[z][i, j].GID).Name, new Vector2(GetDestinationRectangle(AllTiles[z][i, j]).X, GetDestinationRectangle(AllTiles[z][i, j]).Y - 10),
                                    GetDestinationRectangle(AllTiles[z][i, j]).X, GetDestinationRectangle(AllTiles[z][i, j]).Y - 100, 2f, 3f);


                                ReplaceTile(z, i, j, MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].AnimationFrames[0].Id + 1);

                                Game1.Player.Inventory.RemoveItem(int.Parse(information[1]));
                                Game1.GetCurrentStage().ParticleEngine.Color = Color.LightGoldenrodYellow;
                                Game1.GetCurrentStage().ParticleEngine.ActivationTime = 1f;
                                Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(GetDestinationRectangle(AllTiles[z][i, j]).X + 10, GetDestinationRectangle(AllTiles[z][i, j]).Y - 10);

                                Game1.SoundManager.SanctuaryAdd.Play();
                            }
                        }
                    }
                    break;
                case "chestLoot":
                    if (mouse.IsClicked)
                    {
                        Game1.GetCurrentStage().AllChests[AllTiles[z][i, j].GetTileKey(z, mapWidth, mapHeight)].IsUpdating = true;
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
                                ReplaceTile(3, i, j, -1);
                                Game1.GetCurrentStage().AllSprites.Add(new Sprite(this.graphicsDevice, Game1.AllTextures.Gears, new Rectangle(48, 0, 16, 16), new Vector2(GetDestinationRectangle(AllTiles[z][i, j]).X + 8,
                                    GetDestinationRectangle(AllTiles[z][i, j]).Y + 8), 16, 16)
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
                                ReplaceTile(3, i, j, -1);

                                Game1.GetCurrentStage().AllSprites.Add(new Sprite(this.graphicsDevice, Game1.AllTextures.Gears, new Rectangle(16, 0, 16, 16),
                                    new Vector2(GetDestinationRectangle(AllTiles[z][i, j]).X + 8, GetDestinationRectangle(AllTiles[z][i, j]).Y + 5), 16, 16)
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
                endi = (int)(Game1.cam.Pos.X / 16) + (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2 / 16) + 1;
            }
            int endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2 / 16) + 2;
            if (endj > this.mapHeight)
            {
                endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2 / 16) + 1;
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
                            Rectangle SourceRectangle = GetSourceRectangle(AllTiles[z][i, j]);
                            Rectangle DestinationRectangle = GetDestinationRectangle(AllTiles[z][i, j]);
                            if (AllTufts.ContainsKey(AllTiles[z][i, j].GetTileKey(z, this.mapWidth, this.mapHeight)))
                            {
                                for (int t = 0; t < AllTufts[AllTiles[z][i, j].GetTileKey(z, this.mapWidth, this.mapHeight)].Count; t++)
                                {
                                    AllTufts[AllTiles[z][i, j].GetTileKey(z, this.mapWidth, this.mapHeight)][t].Draw(spriteBatch);
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
                                spriteBatch.Draw(tileSet, new Vector2(DestinationRectangle.X, DestinationRectangle.Y), SourceRectangle, Game1.GlobalClock.TimeOfDayColor,
                                0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z] + AllTiles[z][i, j].LayerToDrawAtZOffSet);

                            }
                            else
                            {
                                spriteBatch.Draw(tileSet, new Vector2(DestinationRectangle.X, DestinationRectangle.Y), SourceRectangle, Color.White,
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

        public void ReplaceTile(int layer, int tileToReplaceX, int tileToReplaceY, int newTileGID)
        {
            Tile ReplaceMenttile = new Tile(AllTiles[layer][tileToReplaceX, tileToReplaceY].X, AllTiles[layer][tileToReplaceX, tileToReplaceY].Y, newTileGID, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight);
            AllTiles[layer][tileToReplaceX, tileToReplaceY] = ReplaceMenttile;
        }
        #endregion

        #region INTERACTIONS



        public void InteractWithBuilding(int layer, GameTime gameTime, int oldX, int oldY, Rectangle destinationRectangle, ILocation world)
        {

            if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("destructable"))
            {
                if (!AnimationFrames.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight)) && !Game1.Player.CurrentAction[0, 0].IsAnimated)
                {
                    if (Game1.Utility.GetRequiredTileTool(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]) == -50)
                    {
                        InteractWithoutPlayerAnimation(layer, gameTime, oldX, oldY, destinationRectangle, world, delayTimer: .25f);
                        if (TileHitPoints.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer, mapWidth, mapHeight)))
                        {
                            TileHitPoints[AllTiles[layer][oldX, oldY].GetTileKey(layer, mapWidth, mapHeight)]--;
                        }

                    }
                    else if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() == Game1.Utility.GetRequiredTileTool(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]))
                    {
                        switch (Game1.Utility.GetRequiredTileTool(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]))
                        {
                            //bare hands

                            case 0:
                                //InteractWithoutPlayerAnimation(3, gameTime, oldX, oldY - 1, .25f);
                                InteractWithPlayerAnimation(layer, gameTime, oldX, oldY, 9, 10, 11, 12, destinationRectangle, .25f);
                                ToolInteraction(AllTiles[layer][oldX, oldY], layer, oldX, oldY, Game1.Utility.GetTileDestructionSound(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]),
                                    Game1.Utility.GetTileEffectColor(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), world, destinationRectangle, MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"));
                                if (TileHitPoints.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer, mapWidth, mapHeight)))
                                {
                                    TileHitPoints[AllTiles[layer][oldX, oldY].GetTileKey(layer, mapWidth, mapHeight)]--;
                                    //Game1.Player.Inventory.currentInventory.Find(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool()
                                }

                                break;

                            case 1:
                                //InteractWithoutPlayerAnimation(3, gameTime, oldX, oldY - 1, .25f);
                                InteractWithPlayerAnimation(layer, gameTime, oldX, oldY, 5, 6, 7, 8, destinationRectangle, .25f);
                                ToolInteraction(AllTiles[layer][oldX, oldY], layer, oldX, oldY, Game1.Utility.GetTileDestructionSound(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]),
                                    Game1.Utility.GetTileEffectColor(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), world, destinationRectangle, MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"));
                                if (TileHitPoints.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer, mapWidth, mapHeight)))
                                {
                                    TileHitPoints[AllTiles[layer][oldX, oldY].GetTileKey(layer, mapWidth, mapHeight)]--;
                                }
                                break;
                            case 2:
                                //InteractWithoutPlayerAnimation(3, gameTime, oldX, oldY - 1, .25f);
                                InteractWithPlayerAnimation(layer, gameTime, oldX, oldY, 1, 2, 3, 4, destinationRectangle, .25f);
                                ToolInteraction(AllTiles[layer][oldX, oldY], layer, oldX, oldY, Game1.Utility.GetTileDestructionSound(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]),
                                    Game1.Utility.GetTileEffectColor(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), world, destinationRectangle, MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"));
                                if (TileHitPoints.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer, mapWidth, mapHeight)))
                                {
                                    TileHitPoints[AllTiles[layer][oldX, oldY].GetTileKey(layer, mapWidth, mapHeight)]--;
                                }
                                break;
                        }

                    }

                }
            }

        }

        public void ToolInteraction(Tile tile, int layer, int x, int y, int setSoundInt, Color particleColor, ILocation world, Rectangle destinationRectangle, bool hasSpawnTiles = false)
        {
            if (TileHitPoints.ContainsKey(AllTiles[layer][x, y].GetTileKey(layer, mapWidth, mapHeight)))
            {

                if (TileHitPoints[tile.GetTileKey(layer, mapWidth, mapHeight)] > 0)
                {
                    Game1.SoundManager.PlaySoundEffectFromInt(false, 1, setSoundInt, 1f);
                    Game1.GetCurrentStage().ParticleEngine.Color = particleColor;
                    Game1.GetCurrentStage().ParticleEngine.ActivationTime = .25f;
                    Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(destinationRectangle.X + 5, destinationRectangle.Y - 20);
                    return;
                }

                if (TileHitPoints[tile.GetTileKey(layer, mapWidth, mapHeight)] < 1)
                {
                    Game1.SoundManager.PlaySoundEffectFromInt(false, 1, setSoundInt, 1f);
                    TileHitPoints.Remove(tile.GetTileKey(layer, mapWidth, mapHeight));
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
                                    this.AnimationFrames.Add(AllTiles[layer][x, y - 1].GetTileKey(layer, this.mapWidth, this.mapHeight), frameHolder);
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
                    this.AnimationFrames.Add(AllTiles[layer][x, y].GetTileKey(layer, this.mapWidth, this.mapHeight), frameHolder);
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

        public void InteractWithoutPlayerAnimation(int layer, GameTime gameTime, int oldX, int oldY, Rectangle destinationRectangle, ILocation world, float delayTimer = 0f)
        {
            if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].AnimationFrames.Count > 0)
            {
                List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                for (int i = 0; i < MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].AnimationFrames.Count; i++)
                {
                    frames.Add(new EditableAnimationFrame(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].AnimationFrames[i]));
                }
                EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, oldX, oldY, layer, AllTiles[layer][oldX, oldY].GID);
                this.AnimationFrames.Add(AllTiles[layer][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight), frameHolder);
            }

            if (Game1.GetCurrentStage().AllObjects.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight)))
            {
                Game1.GetCurrentStage().AllObjects.Remove(AllTiles[layer][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight));
                if (this.CurrentObjects.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight)))
                {
                    this.CurrentObjects.Remove(AllTiles[layer][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight));
                }
            }
            //AllTiles[layer][oldX, oldY].HasObject = false;
            if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"))
            {

                DestroySpawnWithTiles(AllTiles[layer][oldX, oldY], oldX, oldY, world);
            }
            //mostly for crops
            if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("AssociatedTiles"))
            {
                ReplaceTilePermanent(3, oldX, oldY - 1, 0, world);
                Game1.GetCurrentStage().AllCrops.Remove(AllTiles[0][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight));
                //AllTiles[0][oldX, oldY].ContainsCrop = false;
            }
            GetDrop(layer, oldX, oldY, destinationRectangle);

            Game1.SoundManager.PlaySoundEffectFromInt(false, 1, Game1.Utility.GetTileDestructionSound(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), 1f);

            ReplaceTilePermanent(layer, oldX, oldY, 0, world);


        }

        //specify which animations you want to use depending on where the player is in relation to the object

        public void InteractWithPlayerAnimation(int layer, GameTime gameTime, int oldX, int oldY, int down, int right, int left, int up, Rectangle destinationRectangle, float delayTimer = 0f)
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
                if (!AnimationFrames.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight)) && Game1.GetCurrentStage().AllObjects.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight)))
                {
                    //ObjectBody newObject = new ObjectBody();
                    Game1.GetCurrentStage().AllObjects.Remove(AllTiles[layer][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight));
                    if (this.CurrentObjects.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight)))
                    {
                        this.CurrentObjects.Remove(AllTiles[layer][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight));
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
                    if(Game1.GetCurrentStage().AllObjects.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight)))
                    {
                        Game1.GetCurrentStage().AllObjects.Remove(AllTiles[layer][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight));
                        if (this.CurrentObjects.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight)))
                        {
                            this.CurrentObjects.Remove(AllTiles[layer][oldX, oldY].GetTileKey(layer, this.mapWidth, this.mapHeight));
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
        public class EditableAnimationFrame
        {
            public float CurrentDuration { get; set; }
            public float AnchorDuration { get; set; }
            public int ID { get; set; }

            public EditableAnimationFrame(AnimationFrameHolder frame)
            {
                this.CurrentDuration = frame.Duration;
                this.AnchorDuration = frame.Duration;
                this.ID = frame.Id;

            }
        }

        public class EditableAnimationFrameHolder
        {
            public List<EditableAnimationFrame> Frames { get; set; }
            public float Timer { get; set; }
            public int Counter { get; set; }
            public int OldX { get; }
            public int OldY { get; }
            public int Layer { get; set; }
            public int OriginalTileID { get; set; }
            public bool Repeats { get; set; }

            public EditableAnimationFrameHolder(List<EditableAnimationFrame> frames, int oldX, int oldY, int layer, int originalTileID)
            {
                this.Frames = frames;
                this.Counter = 0;
                this.Timer = frames[Counter].AnchorDuration;
                this.OldX = oldX;
                this.OldY = oldY;
                this.Layer = layer;
                this.OriginalTileID = originalTileID;
            }
        }
    }
    #endregion
    #endregion
}