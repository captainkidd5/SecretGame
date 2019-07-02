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

namespace SecretProject.Class.TileStuff
{
    public class TileManager
    {
        protected Game1 game;

        protected Texture2D tileSet;
        protected TmxMap mapName;
        protected TmxLayer layerName;


        //--------------------------------------
        //Map Specifications
        public int tilesetTilesWide { get; set; }
        public int tilesetTilesHigh { get; set; }
        public int mapWidth { get; set; }
        public int mapHeight { get; set; }
        //--------------------------------------
        //Tile Specificications
        public int iD { get; set; }
        public int tileWidth { get; set; }
        public int tileHeight { get; set; }
        public int tileNumber { get; set; }


        //--------------------------------------
        //Counting
        public int tileCounter { get; set; }


        //--------------------------------------
        //2D Array of All Tiles

        [XmlIgnore]
        public Tile[,] Tiles { get; set; }


        public bool isActive = false;
        public bool isPlacement { get; set; } = false;

        public bool isInClickingRangeOfPlayer = false;

        [XmlIgnore]
        ContentManager content;
        [XmlIgnore]
        GraphicsDevice graphicsDevice;

        public int ReplaceTileGid { get; set; }

        // List<TmxObject> tileObjects;

        public int CurrentIndexX { get; set; }
        public int CurrentIndexY { get; set; }

        public Tile TempTile { get; set; }

        public int OldIndexX { get; set; }
        public int OldIndexY { get; set; }

        public float Depth { get; set; }

        /// <summary>
        /// background = 0, buildings = 1, midground =2, foreground =3, placement =4
        /// </summary>
        public int LayerIdentifier { get; set; }

        public List<TmxLayer> AllLayers;
        public List<Tile[,]> AllTiles;

        public List<float> AllDepths;

        public bool TileInteraction { get; set; } = false;

        public Tile DebugTile { get; set; } = new Tile(40, 40, 4714, 100, 100, 100, 100);

        public int TileSetNumber { get; set; }
        public AStarPathFinder PathGrid { get; set; }


        #region CONSTRUCTOR

        private TileManager()
        {

        }
        //TODO LayerDepth List
        public TileManager(Texture2D tileSet, TmxMap mapName, List<TmxLayer> allLayers, GraphicsDevice graphicsDevice, ContentManager content, int tileSetNumber, List<float> allDepths)
        {
            this.tileSet = tileSet;
            this.mapName = mapName;

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


                }
            }
            for (int i = 0; i < mapName.ObjectGroups["Portal"].Objects.Count; i++)
            {
                string keyFrom;
                string keyTo;
                string safteyX;
                string safteyY;


                mapName.ObjectGroups["Portal"].Objects[i].Properties.TryGetValue("standardFrom", out keyFrom);
                mapName.ObjectGroups["Portal"].Objects[i].Properties.TryGetValue("standardTo", out keyTo);
                mapName.ObjectGroups["Portal"].Objects[i].Properties.TryGetValue("SafteyOffSetX", out safteyX);
                mapName.ObjectGroups["Portal"].Objects[i].Properties.TryGetValue("SafteyOffSetY", out safteyY);
                Portal portal = new Portal(int.Parse(keyFrom), int.Parse(keyTo), int.Parse(safteyX), int.Parse(safteyY));


                int portalX = (int)mapName.ObjectGroups["Portal"].Objects[i].X;
                int portalY = (int)mapName.ObjectGroups["Portal"].Objects[i].Y;
                int portalWidth = (int)mapName.ObjectGroups["Portal"].Objects[i].Width;
                int portalHeight = (int)mapName.ObjectGroups["Portal"].Objects[i].Height;

                portal.PortalStart = new Rectangle(portalX, portalY, portalWidth, portalHeight);

                Game1.GetCurrentStage().AllPortals.Add(portal);
            }

            //specify GID which is 1 larger than one on tileset, idk why
            //brown tall grass
            //GenerateTiles(3, 6394, "dirt", 2000, 0);
            //green tall grass
            //GenerateTiles(3, 6393, "dirt", 2000, 0);
            //    //stone
            GenerateTiles(1, 6675, "dirt", 50, 0);
            //    //grass
            GenerateTiles(1, 6475, "dirt", 50, 0);
            //    //redrunestone
            GenerateTiles(1, 5681, "dirt", 100, 0);
            ////bluerunestone
            GenerateTiles(1, 5881, "dirt", 100, 0);
            ////thunderbirch
            GenerateTiles(1, 4845, "dirt", 200, 0);
            ////crown of swords
            GenerateTiles(1, 6388, "sand", 50, 0);
            ////dandelion
            GenerateTiles(1, 6687, "sand", 100, 0);
            //juicyfruit
            GenerateTiles(1, 6589, "dirt", 50, 0);
            //orchardTree
            GenerateTiles(1, 4245, "dirt", 200, 0);
            //bubblegum
            // GenerateTiles(1, 6191, "dirt", 200, 0);


            for (int z = 0; z < AllTiles.Count; z++)
            {
                for (int i = 0; i < tilesetTilesWide; i++)
                {
                    for (int j = 0; j < tilesetTilesHigh; j++)
                    {

                        if (AllTiles[z][i, j].GID != 0)
                        {
                            if (mapName.Tilesets[tileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                            {

                                AssignProperties(AllTiles[z][i, j], 0);

                            }
                        }
                    }
                }
            }
        }
        #endregion
        public void AssignProperties(Tile tileToAssign, int tileSetNumber)
        {
            if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("newSource"))
            {
                int[] rectangleCoords = Game1.Utility.GetNewTileSourceRectangle(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["newSource"]);
                tileToAssign.SourceRectangle = new Rectangle(tileToAssign.SourceRectangle.X + rectangleCoords[0], tileToAssign.SourceRectangle.Y + rectangleCoords[1], rectangleCoords[2], rectangleCoords[3]);
                tileToAssign.DestinationRectangle = new Rectangle(tileToAssign.DestinationRectangle.X + rectangleCoords[0], tileToAssign.DestinationRectangle.Y + rectangleCoords[1],
                    tileToAssign.DestinationRectangle.Width, tileToAssign.DestinationRectangle.Height);
            }

            if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("portal"))
            {
                tileToAssign.IsPortal = true;
                tileToAssign.portalDestination = mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["portal"];
            }
            if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("plantable"))
            {
                tileToAssign.Plantable = true;
                tileToAssign.RequiredTool = 167;
            }
            if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("diggable"))
            {
                tileToAssign.Diggable = true;
                tileToAssign.RequiredTool = 3;
            }
            if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsValue("dirt"))
            {
                tileToAssign.Dirt = true;
            }

            if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("Probability"))
            {
                tileToAssign.Probability = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["Probability"]);
            }

            if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("lightSource"))
            {
                tileToAssign.IsLightSource = true;
                tileToAssign.LightType = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["lightSource"]);
                Game1.GetCurrentStage().AllLights.Add(new LightSource(tileToAssign.LightType, new Vector2(tileToAssign.X, tileToAssign.Y)));
            }

            if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("AnimatedX") || mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("AnimatedY"))
            {
                tileToAssign.IsAnimated = true;
                if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("AnimatedX"))
                {
                    tileToAssign.TotalFramesX = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["AnimatedX"]);
                    tileToAssign.Speed = double.Parse(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["Speed"]);
                }
                if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("AnimatedY"))
                {
                    tileToAssign.TotalFramesY = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["AnimatedY"]);
                    tileToAssign.Speed = double.Parse(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["Speed"]);
                }

                if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("action"))
                {
                    tileToAssign.HasAction = true;
                }


                if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("start"))
                {
                    tileToAssign.IsAnimating = true;
                    tileToAssign.Kill = false;

                }

            }
            if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("destructable"))
            {
                //tiles[i, j].Properties.Add("grass", true);
                tileToAssign.Destructable = true;
                tileToAssign.HitPoints = Game1.Utility.GetTileHitpoints(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["destructable"]);
                tileToAssign.RequiredTool = Game1.Utility.GetRequiredTileTool(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["destructable"]);
                tileToAssign.TileDestructionColor = Game1.Utility.GetTileEffectColor(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["destructable"]);
                tileToAssign.SoundValue = Game1.Utility.GetTileDestructionSound(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["destructable"]);


            }
            if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("spawnWith"))
            {
                tileToAssign.HasAdditionalTiles = true;
            }
            if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("action"))
            {
                tileToAssign.actionKey = (int.Parse(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["action"]));

            }
            if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("step"))
            {
                tileToAssign.HasSound = true;
                tileToAssign.SoundValue = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["step"]);
                //grass = 1, stone = 2, wood = 3, sand = 4
            }
            if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("tileSelectorAllowed"))
            {
                tileToAssign.TileSelectorAllowed = true;

            }
            if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("layer"))
            {
                tileToAssign.LayerToDrawAt = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["layer"]);
                //grass = 1, stone = 2, wood = 3, sand = 4
            }

            if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("loot"))
            {
                tileToAssign.Loot = Game1.Utility.Parselootkey(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["loot"]);
            }
            if (tileToAssign.LayerToDrawAt == 3)
            {
                //use random in order to deal with tiles which spawn at the exact same y layer.

                int randomInt = Game1.Utility.RGenerator.Next(1, 1000);
                float randomFloat = (float)(randomInt * .000001);
                tileToAssign.LayerToDrawAtZOffSet = (tileToAssign.DestinationRectangle.Y + tileToAssign.DestinationRectangle.Height) * .0001f + randomFloat;
            }

        }


        public void GenerateTiles(int layerToPlace, int gid, string placementKey, int frequency, int layerToCheckIfEmpty)
        {
            int[] acceptableGenerationTiles;
            switch (placementKey)
            {
                case "dirt":
                    acceptableGenerationTiles = new int[16]
                {
                    6065,6066, 6067, 6068,
                    6165,6166, 6167, 6168,
                    6265,6266, 6267, 6268,
                    6265,6266, 6267, 6268,
                };
                    break;
                case "sand":
                    acceptableGenerationTiles = new int[4]
                {
                    4402, 4403,
                    4502, 4503
                };
                    break;
                default:
                    acceptableGenerationTiles = new int[16]
                {
                    6065,6066, 6067, 6068,
                    6165,6166, 6167, 6168,
                    6265,6266, 6267, 6268,
                    6265,6266, 6267, 6268,
                };
                    break;
            }

            for (int g = 0; g < frequency; g++)
            {
                GenerateRandomTiles(layerToPlace, gid, acceptableGenerationTiles, layerToCheckIfEmpty);
            }
        }
        public void GenerateRandomTiles(int layer, int id, int[] acceptableTiles, int comparisonLayer = 0)
        {
            int newTileX = Game1.Utility.RNumber(10, 90);
            int newTileY = Game1.Utility.RNumber(10, 90);
            if (!CheckIfTileAlreadyExists(newTileX, newTileY, layer) && CheckIfTileMatchesGID(newTileX, newTileY, layer, acceptableTiles, comparisonLayer))
            {

                Tile sampleTile = new Tile(newTileX, newTileY, id, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight);
                if (!mapName.Tilesets[TileSetNumber].Tiles[sampleTile.GID].Properties.ContainsKey("spawnWith"))
                {
                    AllTiles[layer][newTileX, newTileY] = new Tile(newTileX, newTileY, id, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight);
                    return;
                }


                if (mapName.Tilesets[TileSetNumber].Tiles[sampleTile.GID].Properties.ContainsKey("spawnWith"))
                {
                    string value = "";
                    mapName.Tilesets[TileSetNumber].Tiles[sampleTile.GID].Properties.TryGetValue("spawnWith", out value);

                    List<Tile> intermediateNewTiles = new List<Tile>();
                    sampleTile.SpawnsWith = Game1.Utility.ParseSpawnsWithKey(value);
                    for (int index = 0; index < sampleTile.SpawnsWith.Length; index++)
                    {
                        string gidX = "";
                        mapName.Tilesets[TileSetNumber].Tiles[sampleTile.SpawnsWith[index]].Properties.TryGetValue("relationX", out gidX);
                        string gidY = "";
                        mapName.Tilesets[TileSetNumber].Tiles[sampleTile.SpawnsWith[index]].Properties.TryGetValue("relationY", out gidY);
                        string tilePropertyLayer = "";
                        mapName.Tilesets[TileSetNumber].Tiles[sampleTile.SpawnsWith[index]].Properties.TryGetValue("layer", out tilePropertyLayer);
                        int intGidX = int.Parse(gidX);
                        int intGidY = int.Parse(gidY);
                        int intTilePropertyLayer = int.Parse(tilePropertyLayer);
                        //4843,4645,4845,4846,4744,4644,4544,4444,4545,4445,4546,4446,4643,4543,4443,4542,4744
                        int totalGID = mapName.Tilesets[TileSetNumber].Tiles[sampleTile.SpawnsWith[index]].Id;

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
                        AssignProperties(intermediateNewTiles[tileSwapCounter], 0);
                        //AddObject(intermediateNewTiles[tileSwapCounter]);

                        AllTiles[(int)intermediateNewTiles[tileSwapCounter].LayerToDrawAt][(int)intermediateNewTiles[tileSwapCounter].OldX, (int)intermediateNewTiles[tileSwapCounter].OldY] = intermediateNewTiles[tileSwapCounter];
                        //AllTiles[intermediateNewTiles[tileSwapCounter]]
                    }
                    AllTiles[layer][newTileX, newTileY] = new Tile(newTileX, newTileY, id, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight) { SpawnsWith = sampleTile.SpawnsWith };
                }
            }
        }

        public void DestroySpawnWithTiles(Tile baseTile, int xCoord, int yCoord)
        {
            List<Tile> tilesToReturn = new List<Tile>();
            string value = "";
            mapName.Tilesets[TileSetNumber].Tiles[baseTile.GID].Properties.TryGetValue("spawnWith", out value);
            baseTile.SpawnsWith = Game1.Utility.ParseSpawnsWithKey(value);
            if (baseTile.SpawnsWith != null)
            {
                for (int i = 0; i < baseTile.SpawnsWith.Length; i++)
                {
                    string gidX = "";
                    mapName.Tilesets[TileSetNumber].Tiles[baseTile.SpawnsWith[i]].Properties.TryGetValue("relationX", out gidX);
                    string gidY = "";
                    mapName.Tilesets[TileSetNumber].Tiles[baseTile.SpawnsWith[i]].Properties.TryGetValue("relationY", out gidY);
                    string tilePropertyLayer = "";
                    mapName.Tilesets[TileSetNumber].Tiles[baseTile.SpawnsWith[i]].Properties.TryGetValue("layer", out tilePropertyLayer);
                    int intGidX = int.Parse(gidX);
                    int intGidY = int.Parse(gidY);
                    int intTilePropertyLayer = int.Parse(tilePropertyLayer);

                    int totalGID = mapName.Tilesets[TileSetNumber].Tiles[baseTile.SpawnsWith[i]].Id;
                    //tilesToReturn.Add(AllTiles[intTilePropertyLayer][xCoord, yCoord]);
                    if (AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].HasObject)
                    {
                        Game1.GetCurrentStage().AllObjects.Remove(AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].TileObject);
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
        public bool CheckIfTileMatchesGID(int tileX, int tileY, int layer, int[] gidArray, int comparisonLayer = 0)
        {
            for (int i = 0; i < gidArray.Length; i++)
            {
                if (AllTiles[comparisonLayer][tileX, tileY].GID == gidArray[i])
                {
                    return true;
                }
            }
            return false;
        }

        #region LOADTILESOBJECTS
        public void LoadInitialTileObjects()
        {
            for (int z = 0; z < AllTiles.Count; z++)
            {
                if (z == 1)
                {
                    for (var i = 0; i < tilesetTilesWide; i++)
                    {
                        for (var j = 0; j < tilesetTilesHigh; j++)
                        {
                            if (AllTiles[z][i, j].GID != 0)
                            {
                                if (mapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                {
                                    if (mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].ObjectGroups.Count > 0)
                                    {


                                        for (int k = 0; k < mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].ObjectGroups[0].Objects.Count; k++)
                                        {
                                            TmxObject tempObj = mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].ObjectGroups[0].Objects[k];


                                            AllTiles[z][i, j].TileObject = new ObjectBody(graphicsDevice,
                                                new Rectangle(AllTiles[z][i, j].DestinationRectangle.X + (int)Math.Ceiling(tempObj.X),
                                                AllTiles[z][i, j].DestinationRectangle.Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                                                (int)Math.Ceiling(tempObj.Height)), AllTiles[z][i, j].DestinationRectangle.X);

                                            AllTiles[z][i, j].HasObject = true;
                                            AllTiles[z][i, j].AStarTileValue = 0;
                                            Game1.GetCurrentStage().AllObjects.Add(AllTiles[z][i, j].TileObject); // not gonna work for saving, gotta figure out.

                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
            PathGrid = new AStarPathFinder(tilesetTilesWide, tilesetTilesHigh, AllTiles[1]);

        }
        #endregion

        #region ADDOBJECTSTOBUILDINGS

        public void AddObjectToBuildingTile(Tile tile, int indexX, int indexY)
        {
            for (int z = 0; z < AllTiles.Count; z++)
            {
                AddObject(AllTiles[z][indexX, indexY]);

            }
        }

        public void AddObject(Tile tile)
        {
            if (mapName.Tilesets[TileSetNumber].Tiles.ContainsKey(tile.GID))
            {
                for (int k = 0; k < mapName.Tilesets[TileSetNumber].Tiles[tile.GID].ObjectGroups[0].Objects.Count; k++)
                {
                    TmxObject tempObj = mapName.Tilesets[TileSetNumber].Tiles[tile.GID].ObjectGroups[0].Objects[k];

                    tile.TileObject = new ObjectBody(graphicsDevice,
                        new Rectangle(tile.DestinationRectangle.X + (int)Math.Ceiling(tempObj.X),
                        tile.DestinationRectangle.Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                        (int)Math.Ceiling(tempObj.Height)), tile.DestinationRectangle.X);
                    tile.HasObject = true;
                    Game1.GetCurrentStage().AllObjects.Add(tile.TileObject);
                }
            }
        }
        #endregion



        #region UPDATE
        public void Update(GameTime gameTime, MouseManager mouse)
        {
            //Game1.myMouseManager.TogglePlantInteraction = false;
            for (int z = 0; z < AllTiles.Count; z++)
            {
                for (var i = 0; i < mapWidth; i++)
                {
                    for (var j = 0; j < mapHeight; j++)
                    {

                        if (AllTiles[z][i, j].GID != 0)
                        {
                            if (AllTiles[z][i, j].IsFinishedAnimating)
                            {
                                Destroy(z, i, j);
                                AllTiles[z][i, j].IsFinishedAnimating = false;
                            }

                            if (z == 0)
                            {
                                if (AllTiles[z][i, j].HasSound && Game1.Player.IsMoving && Game1.Player.Rectangle.Intersects(AllTiles[z][i, j].DestinationRectangle))
                                {
                                    Game1.SoundManager.PlaySoundEffectFromInt(false, 1, AllTiles[z][i, j].SoundValue, .75f);
                                }
                            }

                            if (mouse.IsHoveringTile(AllTiles[z][i, j].DestinationRectangle))
                            {
                                CurrentIndexX = i;
                                CurrentIndexY = j;

                                //IE layer is background.
                                if (z == 0)
                                {
                                    if (AllTiles[z][i, j].Diggable || AllTiles[z][i, j].Plantable)
                                    {


                                        if (AllTiles[z][i, j].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle))
                                        {
                                            Game1.isMyMouseVisible = false;
                                            Game1.Player.UserInterface.DrawTileSelector = true;
                                            this.DebugTile = AllTiles[z][i, j];
                                            Game1.Player.UserInterface.TileSelectorX = AllTiles[z][i, j].DestinationRectangle.X;
                                            Game1.Player.UserInterface.TileSelectorY = AllTiles[z][i, j].DestinationRectangle.Y;
                                            Game1.myMouseManager.ToggleGeneralInteraction = true;
                                            mouse.ChangeMouseTexture(AllTiles[z][i, j].RequiredTool);
                                            if (mouse.IsClicked)
                                            {
                                                InteractWithBackground(z, gameTime, i, j);

                                            }
                                        }
                                    }
                                    else
                                    {
                                        Game1.Player.UserInterface.DrawTileSelector = false;
                                        Game1.isMyMouseVisible = true;

                                    }
                                }
                                if (z == 1)
                                {


                                    if (AllTiles[z][i, j].Destructable && AllTiles[z][i, j].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle)) //&& mapName.Tilesets[0].Tiles.ContainsKey(tiles[i, j].GID not sure what this was for.
                                    {
                                        Game1.isMyMouseVisible = false;
                                        Game1.Player.UserInterface.DrawTileSelector = true;
                                        Game1.Player.UserInterface.TileSelectorX = AllTiles[z][i, j].DestinationRectangle.X;
                                        Game1.Player.UserInterface.TileSelectorY = AllTiles[z][i, j].DestinationRectangle.Y;

                                        mouse.ChangeMouseTexture(AllTiles[z][i, j].RequiredTool);

                                        Game1.myMouseManager.ToggleGeneralInteraction = true;


                                        if (mouse.IsClicked)
                                        {
                                            InteractWithBuilding(z, gameTime, i, j);

                                        }
                                    }
                                    else
                                    {
                                        //Game1.isMyMouseVisible = true;
                                        //Game1.Player.UserInterface.DrawTileSelector = false;
                                    }
                                    if (AllTiles[z][i, j].actionKey >= 0 && AllTiles[z][i, j].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle))
                                    {
                                        if (mouse.IsRightClicked)
                                        {
                                            ActionHelper(z, i, j, AllTiles[z][i, j].actionKey);
                                        }

                                    }
                                }
                            }

                            if (AllTiles[z][i, j].IsAnimated)
                            {
                                if (AllTiles[z][i, j].IsAnimating == true && AllTiles[z][i, j].IsFinishedAnimating == false)
                                {

                                    //AllTiles[z][i, j].AnimateOnlyX(gameTime, AllTiles[z][i, j].TotalFramesX, AllTiles[z][i, j].Speed);
                                    AllTiles[z][i, j].AnimateDynamic(gameTime, AllTiles[z][i, j].TotalFramesX, AllTiles[z][i, j].TotalFramesY, 16, 16, AllTiles[z][i, j].Speed, AllTiles[z][i, j].Kill);
                                }
                            }

                            if (AllTiles[z][i, j].IsTemporary)
                            {
                                AllTiles[z][i, j].GID = 1;
                            }

                        }

                    }
                }
            }
        }
        public void ActionHelper(int z, int i, int j, int action)
        {
            //new Gid should be one larger, per usual
            switch (action)
            {
                //furnace
                case 1:
                    if (AllTiles[z][i, j].GID == 4654)
                    {
                        AllTiles[z][i, j] = new Tile(i, j, 4255, this.tilesetTilesWide, this.tilesetTilesHigh, this.mapWidth, this.mapHeight);
                        AssignProperties(AllTiles[z][i, j], TileSetNumber);

                        AllTiles[z][i - 1, j] = new Tile(i - 1, j, 4254, this.tilesetTilesWide, this.tilesetTilesHigh, this.mapWidth, this.mapHeight);
                        AssignProperties(AllTiles[z][i - 1, j], TileSetNumber);
                    }
                    if (AllTiles[z][i, j].GID == 4653)
                    {
                        AllTiles[z][i, j] = new Tile(i, j, 4254, this.tilesetTilesWide, this.tilesetTilesHigh, this.mapWidth, this.mapHeight);
                        AssignProperties(AllTiles[z][i, j], TileSetNumber);

                        AllTiles[z][i + 1, j] = new Tile(i + 1, j, 4255, this.tilesetTilesWide, this.tilesetTilesHigh, this.mapWidth, this.mapHeight);
                        AssignProperties(AllTiles[z][i + 1, j], TileSetNumber);
                    }

                    break;

            }
        }
            #endregion

            #region DRAW
            public void DrawTiles(SpriteBatch spriteBatch)
        {
            for (int z = 0; z < AllTiles.Count; z++)
            {
                for (var i = 0; i < mapWidth; i++)
                {
                    for (var j = 0; j < mapHeight; j++)
                    {
                        if (AllTiles[z][i, j].GID != 0)
                        {
                            if (AllTiles[z][i, j].DestinationRectangle.Left < Game1.cam.Pos.X + (Game1.ScreenWidth / 2 / Game1.cam.Zoom) && AllTiles[z][i, j].DestinationRectangle.Left > Game1.cam.Pos.X - (Game1.ScreenWidth / 2 / Game1.cam.Zoom + 16) - 200
                                 && AllTiles[z][i, j].DestinationRectangle.Y < Game1.cam.Pos.Y + (Game1.ScreenHeight / 2 / Game1.cam.Zoom + 16) && AllTiles[z][i, j].DestinationRectangle.Y > Game1.cam.Pos.Y - (Game1.ScreenHeight / 2 / Game1.cam.Zoom + 16) - 200)
                            {
                                spriteBatch.Draw(tileSet, new Vector2(AllTiles[z][i, j].DestinationRectangle.X, AllTiles[z][i, j].DestinationRectangle.Y), AllTiles[z][i, j].SourceRectangle, Game1.GlobalClock.TimeOfDayColor,
                                    0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z] + AllTiles[z][i, j].LayerToDrawAtZOffSet);


                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region REPLACETILES


        public void ReplaceTilePermanent(int layer, int oldX, int oldY, int gid)
        {
            Tile ReplaceMenttile = new Tile(AllTiles[layer][oldX, oldY].OldX, AllTiles[layer][oldX, oldY].OldY, gid, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight);
            AllTiles[layer][oldX, oldY] = ReplaceMenttile;
        }

        public void ReplaceTileTemporary(int layer, int oldX, int oldY, int GID, float colorMultiplier, int xArrayLength, int yArrayLength)
        {
            if (TempTile != null)
            {
                if (AllTiles[layer][CurrentIndexX, CurrentIndexY] == AllTiles[layer][OldIndexX, OldIndexY])
                {
                    AllTiles[layer][OldIndexX, OldIndexY].GID = 1;
                }
            }

            Tile ReplaceMenttile = new Tile(AllTiles[layer][oldX, oldY].OldX, AllTiles[layer][oldX, oldY].OldY, GID, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight) { ColorMultiplier = colorMultiplier };

            TempTile = AllTiles[layer][oldX, oldY];

            AllTiles[layer][oldX, oldY] = ReplaceMenttile;
            // tiles[oldX, oldY].IsTemporary = true;

            OldIndexX = oldX;
            OldIndexY = oldY;

            //  AddTemporaryTiles(TempTile);
        }

        //Basic Replacement.
        public void ReplaceTileWithNewTile(int layer, int tileToReplaceX, int tileToReplaceY, int newTileGID)
        {
            Tile ReplaceMenttile = new Tile(AllTiles[layer][tileToReplaceX, tileToReplaceY].OldX, AllTiles[layer][tileToReplaceX, tileToReplaceY].OldY, newTileGID, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight);
            AllTiles[layer][tileToReplaceX, tileToReplaceY] = ReplaceMenttile;
        }
        #endregion

        #region INTERACTIONS

        //Used for interactions with background tiles only
        public void InteractWithBackground(int layer, GameTime gameTime, int oldX, int oldY)
        {
            if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() == 3)
            {
                if (AllTiles[layer][oldX, oldY].Diggable)
                {
                    if (AllTiles[layer][oldX, oldY].Dirt)
                    {
                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                        ReplaceTileWithNewTile(layer, oldX, oldY, 6074);
                        AllTiles[layer][oldX, oldY].Plantable = true;
                    }

                }
            }
            if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem() != null)
            {
                if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().IsPlantable)
                {
                    if (AllTiles[layer][oldX, oldY].Plantable)
                    {

                        //Game1.myMouseManager.TogglePlantInteraction = true;

                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);

                        AllTiles[layer][oldX, oldY].Crop = Game1.AllCrops.GetCropFromID(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().ID);
                        Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().ID);
                        ReplaceTileWithNewTile(layer, oldX, oldY, AllTiles[layer][oldX, oldY].Crop.GID + 5);

                    }
                }
            }

        }

        //must be a building tile
        public void InteractWithBuilding(int layer, GameTime gameTime, int oldX, int oldY)
        {


            if (AllTiles[layer][oldX, oldY].IsPortal)
            {
                if (AllTiles[layer][oldX, oldY].portalDestination == "lodgeInterior" && Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() == 5)
                {
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DoorOpenInstance, false, 1);
                    Game1.Player.controls.Direction = Dir.Up;
                    Game1.gameStages = Stages.LodgeInteior;
                    Game1.Player.position.X = 878;
                    Game1.Player.position.Y = 809;
                }
            }
            if (AllTiles[layer][oldX, oldY].Destructable)
            {
                if (!AllTiles[layer][oldX, oldY].IsAnimating && !Game1.Player.CurrentAction.IsAnimated)
                {
                    if (AllTiles[layer][oldX, oldY].RequiredTool == -50)
                    {
                        InteractWithoutPlayerAnimation(layer, gameTime, oldX, oldY, .25f);
                        AllTiles[layer][oldX, oldY].HitPoints--;
                    }
                    else if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() == AllTiles[layer][oldX, oldY].RequiredTool)
                    {
                        switch (AllTiles[layer][oldX, oldY].RequiredTool)
                        {
                            //bare hands

                            case 0:
                                //InteractWithoutPlayerAnimation(3, gameTime, oldX, oldY - 1, .25f);
                                InteractWithPlayerAnimation(layer, gameTime, oldX, oldY, 9, 10, 11, 12, .25f);
                                ToolInteraction(AllTiles[layer][oldX, oldY], oldX, oldY, AllTiles[layer][oldX, oldY].SoundValue, AllTiles[layer][oldX, oldY].TileDestructionColor, AllTiles[layer][oldX, oldY].HasAdditionalTiles);
                                AllTiles[layer][oldX, oldY].HitPoints--;
                                break;

                            case 1:
                                //InteractWithoutPlayerAnimation(3, gameTime, oldX, oldY - 1, .25f);
                                InteractWithPlayerAnimation(layer, gameTime, oldX, oldY, 5, 6, 7, 8, .25f);
                                ToolInteraction(AllTiles[layer][oldX, oldY], oldX, oldY, AllTiles[layer][oldX, oldY].SoundValue, AllTiles[layer][oldX, oldY].TileDestructionColor, AllTiles[layer][oldX, oldY].HasAdditionalTiles);
                                AllTiles[layer][oldX, oldY].HitPoints--;
                                break;
                            case 2:
                                //InteractWithoutPlayerAnimation(3, gameTime, oldX, oldY - 1, .25f);
                                InteractWithPlayerAnimation(layer, gameTime, oldX, oldY, 1, 2, 3, 4, .25f);
                                ToolInteraction(AllTiles[layer][oldX, oldY], oldX, oldY, AllTiles[layer][oldX, oldY].SoundValue, AllTiles[layer][oldX, oldY].TileDestructionColor, AllTiles[layer][oldX, oldY].HasAdditionalTiles);
                                AllTiles[layer][oldX, oldY].HitPoints--;
                                break;
                        }

                    }
                    else
                    {

                    }
                }
            }

        }

        public void ToolInteraction(Tile tile, int x, int y, int setSoundInt, Color particleColor, bool hasSpawnTiles = false)
        {
            if (tile.HitPoints >= 1)
            {
                Game1.SoundManager.PlaySoundEffectFromInt(false, 1, setSoundInt, 1f);
                Game1.GetCurrentStage().ParticleEngine.Color = particleColor;
                Game1.GetCurrentStage().ParticleEngine.ActivationTime = .25f;
                Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(tile.DestinationRectangle.X + 5, tile.DestinationRectangle.Y - 20);
            }

            if (tile.HitPoints < 1)
            {
                Game1.SoundManager.PlaySoundEffectFromInt(false, 1, setSoundInt, 1f);
                tile.IsFinishedAnimating = true;
                if (hasSpawnTiles)
                {
                    DestroySpawnWithTiles(tile, x, y);
                }

                Game1.GetCurrentStage().ParticleEngine.Color = particleColor;
                Game1.GetCurrentStage().ParticleEngine.ActivationTime = 1f;
                Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(tile.DestinationRectangle.X, tile.DestinationRectangle.Y);
            }
        }


        //interact without any player animations
        public void InteractWithoutPlayerAnimation(int layer, GameTime gameTime, int oldX, int oldY, float delayTimer = 0f)
        {
            if (delayTimer != 0f)
            {
                AllTiles[layer][oldX, oldY].DelayTimer = delayTimer;
            }
            AllTiles[layer][oldX, oldY].IsAnimating = true;
            AllTiles[layer][oldX, oldY].KillAnimation = true;

            if (AllTiles[layer][oldX, oldY].HasObject)
            {
                Game1.GetCurrentStage().AllObjects.Remove(AllTiles[layer][oldX, oldY].TileObject);
                AllTiles[layer][oldX, oldY].TileObject = null;
                AllTiles[layer][oldX, oldY].HasObject = false;
                if (AllTiles[layer][oldX, oldY].HasAdditionalTiles)
                {

                    DestroySpawnWithTiles(AllTiles[layer][oldX, oldY], oldX, oldY);
                }
                GetDrop(AllTiles[layer][oldX, oldY]);

                Game1.SoundManager.PlaySoundEffectFromInt(false, 1, AllTiles[layer][oldX, oldY].SoundValue, 1f);

                ReplaceTilePermanent(layer, oldX, oldY, 0);
            }




        }

        //specify which animations you want to use depending on where the player is in relation to the object

        public void InteractWithPlayerAnimation(int layer, GameTime gameTime, int oldX, int oldY, int down, int right, int left, int up, float delayTimer = 0f)
        {

            if (delayTimer != 0f)
            {
                AllTiles[layer][oldX, oldY].DelayTimer = delayTimer;
            }
            if (AllTiles[layer][oldX, oldY].IsAnimated)
            {
                AllTiles[layer][oldX, oldY].IsAnimating = true;
                AllTiles[layer][oldX, oldY].KillAnimation = true;
            }


            if (Game1.Player.Position.Y < AllTiles[layer][oldX, oldY].Y - 30)
            {
                Game1.Player.controls.Direction = Dir.Down;
                Game1.Player.PlayAnimation(gameTime, down);
            }

            else if (Game1.Player.Position.Y > AllTiles[layer][oldX, oldY].Y)
            {
                Game1.Player.controls.Direction = Dir.Up;
                Game1.Player.PlayAnimation(gameTime, up);
            }

            else if (Game1.Player.Position.X < AllTiles[layer][oldX, oldY].X)
            {
                Game1.Player.controls.Direction = Dir.Right;
                Game1.Player.PlayAnimation(gameTime, right);
            }
            else if (Game1.Player.Position.X > AllTiles[layer][oldX, oldY].X)
            {
                Game1.Player.controls.Direction = Dir.Left;
                Game1.Player.PlayAnimation(gameTime, left);
            }

        }

        #endregion

        #region DESTROYTILES

        public void Destroy(int layer, int oldX, int oldY)
        {
            if (AllTiles[layer][oldX, oldY].IsAnimated)
            {
                if (AllTiles[layer][oldX, oldY].IsFinishedAnimating && AllTiles[layer][oldX, oldY].HasObject)
                {
                    //ObjectBody newObject = new ObjectBody();
                    Game1.GetCurrentStage().AllObjects.Remove(AllTiles[layer][oldX, oldY].TileObject);
                    AllTiles[layer][oldX, oldY].TileObject = null;
                    AllTiles[layer][oldX, oldY].HasObject = false;
                    GetDrop(AllTiles[layer][oldX, oldY]);
                    if (AllTiles[layer][oldX, oldY].HasAdditionalTiles)
                    {
                        DestroySpawnWithTiles(AllTiles[layer][oldX, oldY], oldX, oldY);
                    }


                    ReplaceTilePermanent(layer, oldX, oldY, 0);

                }
            }
            else
            {
                Game1.GetCurrentStage().AllObjects.Remove(AllTiles[layer][oldX, oldY].TileObject);
                AllTiles[layer][oldX, oldY].TileObject = null;
                AllTiles[layer][oldX, oldY].HasObject = false;
                GetDrop(AllTiles[layer][oldX, oldY]);
                if (AllTiles[layer][oldX, oldY].HasAdditionalTiles)
                {
                    DestroySpawnWithTiles(AllTiles[layer][oldX, oldY], oldX, oldY);
                }
                ReplaceTilePermanent(layer, oldX, oldY, 0);
            }
        }

        public void GetDrop(Tile tile)
        {
            if (tile.Loot != null)
            {
                for (int l = 0; l < tile.Loot.Count; l++)
                {
                    int lootCount = Game1.Utility.DetermineLootDrop(tile.Loot[l]);
                    for (int d = 0; d < lootCount; d++)
                    {
                        Item item = Game1.ItemVault.GenerateNewItem(tile.Loot[l].ID, new Vector2(tile.DestinationRectangle.X, tile.DestinationRectangle.Y), true);
                        Game1.GetCurrentStage().AllItems.Add(item);
                    }
                }
            }
            else
            {
                for (int i = 0; i < tile.NumberOfItemsToSpawn; i++)
                {
                    Item item = Game1.ItemVault.GenerateNewItem(tile.AssociatedItem, new Vector2(tile.DestinationRectangle.X, tile.DestinationRectangle.Y), true);
                    //item.IsTossable = true;
                    Game1.GetCurrentStage().AllItems.Add(item);
                }
            }
        }

    }
    #endregion
}