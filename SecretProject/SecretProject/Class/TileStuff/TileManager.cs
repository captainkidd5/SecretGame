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
                    if (layerNameTile.HorizontalFlip)
                    {
                        tempTile.HorizontalFlip = true;
                    }
                    if (layerNameTile.VerticalFlip)
                    {
                        tempTile.VeritcalFlip = true;
                    }
                    if (layerNameTile.DiagonalFlip)
                    {
                        tempTile.DiagonalFlip = true;
                    }
                    AllTiles[i][layerNameTile.X, layerNameTile.Y] = tempTile;


                }
            }
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

                Game1.GetCurrentStage().AllPortals.Add(portal);
            }

            //specify GID which is 1 larger than one on tileset, idk why
            //brown tall grass
            //GenerateTiles(3, 6394, "dirt", 2000, 0);
            //green tall grass
            //GenerateTiles(3, 6393, "dirt", 2000, 0);
            //    //stone
            //GenerateTiles(1, 6675, "dirt", 50, 0);
            ////    //grass
            //GenerateTiles(1, 6475, "dirt", 50, 0);
            ////    //redrunestone
            //GenerateTiles(1, 5681, "dirt", 100, 0);
            //////bluerunestone
            //GenerateTiles(1, 5881, "dirt", 100, 0);
            //////thunderbirch
            //GenerateTiles(1, 4845, "dirt", 200, 0);
            //////crown of swords
            //GenerateTiles(1, 6388, "sand", 50, 0);
            //////dandelion
            //GenerateTiles(1, 6687, "sand", 100, 0);
            ////juicyfruit
            //GenerateTiles(1, 6589, "dirt", 50, 0);
            ////orchardTree
            //GenerateTiles(1, 4245, "dirt", 200, 0);
            //bubblegum
            // GenerateTiles(1, 6191, "dirt", 200, 0);


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
            if (mapName.Tilesets[tileSetNumber].Tiles.ContainsKey(tileToAssign.GID))
            {


                if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("newSource"))
                {
                    int[] rectangleCoords = Game1.Utility.GetNewTileSourceRectangle(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["newSource"]);
                    tileToAssign.SourceRectangle = new Rectangle(tileToAssign.SourceRectangle.X + rectangleCoords[0], tileToAssign.SourceRectangle.Y + rectangleCoords[1], rectangleCoords[2], rectangleCoords[3]);
                    tileToAssign.DestinationRectangle = new Rectangle(tileToAssign.DestinationRectangle.X + rectangleCoords[0], tileToAssign.DestinationRectangle.Y + rectangleCoords[1],
                        tileToAssign.DestinationRectangle.Width, tileToAssign.DestinationRectangle.Height);
                }


                if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("lightSource"))
                {
                    int lightType = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["lightSource"]);
                    Game1.GetCurrentStage().AllLights.Add(new LightSource(lightType, new Vector2(tileToAssign.X, tileToAssign.Y)));
                }

                if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("AnimatedX") || mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("AnimatedY"))
                {
                    tileToAssign.IsAnimated = true;
                    if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("AnimatedX"))
                    {
                        tileToAssign.TotalFramesX = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["AnimatedX"]);

                    }
                    if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("AnimatedY"))
                    {
                        tileToAssign.TotalFramesY = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["AnimatedY"]);

                    }



                    if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("start"))
                    {
                        tileToAssign.IsAnimating = true;
                        tileToAssign.Kill = false;

                    }

                }
                if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("destructable"))
                {
                    tileToAssign.HitPoints = Game1.Utility.GetTileHitpoints(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["destructable"]);

                }

                if (mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("layer"))
                {
                    tileToAssign.LayerToDrawAt = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["layer"]);
                    //grass = 1, stone = 2, wood = 3, sand = 4
                }

                if (tileToAssign.LayerToDrawAt == 3)
                {
                    //use random in order to deal with tiles which spawn at the exact same y layer.

                    int randomInt = Game1.Utility.RGenerator.Next(1, 1000);
                    float randomFloat = (float)(randomInt * .000001);
                    tileToAssign.LayerToDrawAtZOffSet = (tileToAssign.DestinationRectangle.Y + tileToAssign.DestinationRectangle.Height) * .0001f + randomFloat;
                }
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
                    int[] spawnsWith = Game1.Utility.ParseSpawnsWithKey(value);
                    //sampleTile.SpawnsWith = Game1.Utility.ParseSpawnsWithKey(value);
                    for (int index = 0; index < spawnsWith.Length; index++)
                    {
                        string gidX = "";
                        mapName.Tilesets[TileSetNumber].Tiles[spawnsWith[index]].Properties.TryGetValue("relationX", out gidX);
                        string gidY = "";
                        mapName.Tilesets[TileSetNumber].Tiles[spawnsWith[index]].Properties.TryGetValue("relationY", out gidY);
                        string tilePropertyLayer = "";
                        mapName.Tilesets[TileSetNumber].Tiles[spawnsWith[index]].Properties.TryGetValue("layer", out tilePropertyLayer);
                        int intGidX = int.Parse(gidX);
                        int intGidY = int.Parse(gidY);
                        int intTilePropertyLayer = int.Parse(tilePropertyLayer);
                        //4843,4645,4845,4846,4744,4644,4544,4444,4545,4445,4546,4446,4643,4543,4443,4542,4744
                        int totalGID = mapName.Tilesets[TileSetNumber].Tiles[spawnsWith[index]].Id;

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
                    AllTiles[layer][newTileX, newTileY] = new Tile(newTileX, newTileY, id, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight);
                }
            }
        }

        public void DestroySpawnWithTiles(Tile baseTile, int xCoord, int yCoord)
        {
            List<Tile> tilesToReturn = new List<Tile>();
            string value = "";
            mapName.Tilesets[TileSetNumber].Tiles[baseTile.GID].Properties.TryGetValue("spawnWith", out value);

            int[] spawnsWith = Game1.Utility.ParseSpawnsWithKey(value);
            if (spawnsWith != null)
            {
                for (int i = 0; i < spawnsWith.Length; i++)
                {
                    string gidX = "";
                    mapName.Tilesets[TileSetNumber].Tiles[spawnsWith[i]].Properties.TryGetValue("relationX", out gidX);
                    string gidY = "";
                    mapName.Tilesets[TileSetNumber].Tiles[spawnsWith[i]].Properties.TryGetValue("relationY", out gidY);
                    string tilePropertyLayer = "";
                    mapName.Tilesets[TileSetNumber].Tiles[spawnsWith[i]].Properties.TryGetValue("layer", out tilePropertyLayer);
                    int intGidX = int.Parse(gidX);
                    int intGidY = int.Parse(gidY);
                    int intTilePropertyLayer = int.Parse(tilePropertyLayer);

                    int totalGID = mapName.Tilesets[TileSetNumber].Tiles[spawnsWith[i]].Id;
                    //tilesToReturn.Add(AllTiles[intTilePropertyLayer][xCoord, yCoord]);


                    if (Game1.GetCurrentStage().AllObjects[AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileObjectKey()] != null)
                    {
                        Game1.GetCurrentStage().AllObjects[AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileObjectKey()] = null;
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
                    for (var i = 0; i < mapWidth; i++)
                    {
                        for (var j = 0; j < mapHeight; j++)
                        {
                            int testGID = AllTiles[z][i, j].GID;
                            if (AllTiles[z][i, j].GID != -1)
                            {
                                AllTiles[1][i, j].AStarTileValue = 0;
                                if (mapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                {

                                    if (mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].ObjectGroups.Count > 0)
                                    {


                                        for (int k = 0; k < mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].ObjectGroups[0].Objects.Count; k++)
                                        {
                                            TmxObject tempObj = mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].ObjectGroups[0].Objects[k];


                                            ObjectBody tempObjectBody = new ObjectBody(graphicsDevice,
                                                new Rectangle(AllTiles[z][i, j].DestinationRectangle.X + (int)Math.Ceiling(tempObj.X),
                                                AllTiles[z][i, j].DestinationRectangle.Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                                                (int)Math.Ceiling(tempObj.Height)), AllTiles[z][i, j].GID);



                                            Game1.GetCurrentStage().AllObjects[AllTiles[z][i, j].GetTileObjectKey()] = tempObjectBody; // not gonna work for saving, gotta figure out.

                                        }
                                        AllTiles[z][i, j].AStarTileValue = 0;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            PathGrid = new AStarPathFinder(mapWidth, mapHeight, AllTiles[1]);

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
                    ObjectBody transformedObject = new ObjectBody(graphicsDevice,
                       new Rectangle(tile.DestinationRectangle.X + (int)Math.Ceiling(tempObj.X),
                       tile.DestinationRectangle.Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                       (int)Math.Ceiling(tempObj.Height)), tile.GID);
                    if (tile.DiagonalFlip)
                    {
                        if (tile.HorizontalFlip)
                        {
                            transformedObject.Rectangle = new Rectangle(transformedObject.Rectangle.Width, transformedObject.Rectangle.Y - -transformedObject.Rectangle.Height,
                                transformedObject.Rectangle.Width, transformedObject.Rectangle.Height);
                        }
                        if (tile.VeritcalFlip)
                        {
                            //tile.TileObject.Rectangle.Y = tile.TileObject.Rectangle.Y - tile.TileObject.Rectangle.Height;
                            transformedObject.Rectangle = new Rectangle(transformedObject.Rectangle.X, transformedObject.Rectangle.Y - -transformedObject.Rectangle.Height,
                                transformedObject.Rectangle.Width, transformedObject.Rectangle.Height);
                        }

                    }
                    else if (tile.HorizontalFlip && !tile.DiagonalFlip)
                    {

                        transformedObject.Rectangle = new Rectangle(transformedObject.Rectangle.Width, transformedObject.Rectangle.Y - -transformedObject.Rectangle.Height,
                                transformedObject.Rectangle.Width, transformedObject.Rectangle.Height);
                    }
                    else if (tile.VeritcalFlip && !tile.DiagonalFlip)
                    {
                        transformedObject.Rectangle = new Rectangle(transformedObject.Rectangle.X, transformedObject.Rectangle.Y - -transformedObject.Rectangle.Height,
                                transformedObject.Rectangle.Width, transformedObject.Rectangle.Height);
                    }

                    else
                    {

                    }

                    // tile.HasObject = true;
                    Game1.GetCurrentStage().AllObjects[tile.GetTileObjectKey()] = transformedObject;
                }
            }
        }
        #endregion



        #region UPDATE
        public void Update(GameTime gameTime, MouseManager mouse)
        {
            //Game1.myMouseManager.TogglePlantInteraction = false;
            Game1.Player.UserInterface.DrawTileSelector = false;

            for (int z = 0; z < AllTiles.Count; z++)
            {
                for (var i = 0; i < mapWidth; i++)
                {
                    for (var j = 0; j < mapHeight; j++)
                    {



                        if (AllTiles[z][i, j].GID != -1)
                        {

                            if (AllTiles[z][i, j].IsFinishedAnimating)
                            {
                                Destroy(z, i, j);
                                AllTiles[z][i, j].IsFinishedAnimating = false;
                            }

                            if (z == 0)
                            {
                                if (mapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                {
                                    if (mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("step") && Game1.Player.IsMoving && Game1.Player.Rectangle.Intersects(AllTiles[z][i, j].DestinationRectangle))
                                    {
                                        Game1.SoundManager.PlaySoundEffectFromInt(false, 1, Game1.Utility.GetRequiredTileTool(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["step"]), .75f);
                                    }
                                }

                            }

                            if (AllTiles[z][i, j].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle))
                            {
                                //Game1.Player.UserInterface.DrawTileSelector = true;
                                if (mouse.IsHoveringTile(AllTiles[z][i, j].DestinationRectangle))
                                {
                                    CurrentIndexX = i;
                                    CurrentIndexY = j;

                                    if (mapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                    {
                                        //IE layer is background.
                                        if (z == 0)
                                        {
                                            if (AllTiles[1][i, j].GID == -1 && mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("action"))
                                            {

                                                    InteractWithBackground(z, gameTime, mouse, i, j, mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["action"]);


                                            }

                                        }
                                        if (z == 1)
                                        {


                                            if (AllTiles[1][i, j].GID == -1 && mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("action")) //&& mapName.Tilesets[0].Tiles.ContainsKey(tiles[i, j].GID not sure what this was for.
                                            {
                                                Game1.Player.UserInterface.DrawTileSelector = true;
                                                Game1.isMyMouseVisible = false;
                                                Game1.Player.UserInterface.TileSelectorX = AllTiles[z][i, j].DestinationRectangle.X;
                                                Game1.Player.UserInterface.TileSelectorY = AllTiles[z][i, j].DestinationRectangle.Y;

                                                mouse.ChangeMouseTexture(Game1.Utility.GetRequiredTileTool(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["destructable"]));

                                                Game1.myMouseManager.ToggleGeneralInteraction = true;



                                            }
                                            if (mouse.IsClicked)
                                            {
                                                InteractWithBuilding(z, gameTime, i, j);

                                            }


                                            if (mapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                            {

                                                if (mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("action"))
                                                {


                                                    if (int.Parse(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["action"]) >= 0 && AllTiles[z][i, j].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle))
                                                    {
                                                        if (mouse.IsRightClicked)
                                                        {
                                                            ActionHelper(z, i, j, int.Parse(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["action"]));
                                                        }

                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }


                            if (AllTiles[z][i, j].IsAnimated)
                            {
                                if (AllTiles[z][i, j].IsAnimating == true && AllTiles[z][i, j].IsFinishedAnimating == false)
                                {

                                    //AllTiles[z][i, j].AnimateOnlyX(gameTime, AllTiles[z][i, j].TotalFramesX, AllTiles[z][i, j].Speed);

                                    AllTiles[z][i, j].AnimateDynamic(gameTime, AllTiles[z][i, j].TotalFramesX, AllTiles[z][i, j].TotalFramesY, 16, 16, float.Parse(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["Speed"]), AllTiles[z][i, j].Kill);
                                }
                            }

                            //if (AllTiles[z][i, j].IsTemporary)
                            //{
                            //    AllTiles[z][i, j].GID = 1;
                            //}

                        }

                    }
                }
            }
        }
        //TODO: Fix animation frames
        public void AnimationFrameAnimate(Tile tile)
        {
            if (mapName.Tilesets[tile.GID].Tiles[tile.GID].AnimationFrames.Count > 0)
            {
                // tile = new Tile()
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
                        ReplaceTilePermanent(1, i, j, 4657);

                        ReplaceTilePermanent(1, i - 1, j, 4658);
                        AllTiles[z][i, j - 2].IsAnimating = true;
                        AllTiles[z][i, j - 2].Kill = false;
                        AllTiles[z][i, j - 2].IsFinishedAnimating = false;
                        AllTiles[z][i, j - 2].IsAnimated = true;

                    }
                    else if (AllTiles[z][i, j].GID == 4653)
                    {
                        ReplaceTilePermanent(1, i, j, 4656);

                        ReplaceTilePermanent(1, i + 1, j, 4657);
                        AllTiles[z][i + 1, j - 2].IsAnimating = true;
                        AllTiles[z][i + 1, j - 2].Kill = false;
                        AllTiles[z][i + 1, j - 2].IsFinishedAnimating = false;
                        AllTiles[z][i + 1, j - 2].IsAnimated = true;

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
                        if (AllTiles[z][i, j].GID != -1)
                        {
                            if (AllTiles[z][i, j].DestinationRectangle.Left < Game1.cam.Pos.X + (Game1.ScreenWidth / 2 / Game1.cam.Zoom) && AllTiles[z][i, j].DestinationRectangle.Left > Game1.cam.Pos.X - (Game1.ScreenWidth / 2 / Game1.cam.Zoom + 16) - 200
                                 && AllTiles[z][i, j].DestinationRectangle.Y < Game1.cam.Pos.Y + (Game1.ScreenHeight / 2 / Game1.cam.Zoom + 16) && AllTiles[z][i, j].DestinationRectangle.Y > Game1.cam.Pos.Y - (Game1.ScreenHeight / 2 / Game1.cam.Zoom + 16) - 200)
                            {


                                AllDepths[3] = .4f + (float)(AllTiles[z][i, j].DestinationRectangle.Bottom + AllTiles[z][i, j].DestinationRectangle.Height / mapHeight * AllTiles[z][i, j].TileHeight) / (float)10000;

                                if (AllTiles[z][i, j].DiagonalFlip)
                                {
                                    if (AllTiles[z][i, j].HorizontalFlip)
                                    {
                                        spriteBatch.Draw(tileSet, new Vector2(AllTiles[z][i, j].DestinationRectangle.X + AllTiles[z][i, j].TileWidth, AllTiles[z][i, j].DestinationRectangle.Y), AllTiles[z][i, j].SourceRectangle, Game1.GlobalClock.TimeOfDayColor,
                                    (float)Math.PI / 2f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z] + AllTiles[z][i, j].LayerToDrawAtZOffSet);
                                    }
                                    if (AllTiles[z][i, j].VeritcalFlip)
                                    {
                                        spriteBatch.Draw(tileSet, new Vector2(AllTiles[z][i, j].DestinationRectangle.X, AllTiles[z][i, j].DestinationRectangle.Y + AllTiles[z][i, j].TileHeight), AllTiles[z][i, j].SourceRectangle, Game1.GlobalClock.TimeOfDayColor,
                                    (float)(3 * Math.PI / 2f), Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z] + AllTiles[z][i, j].LayerToDrawAtZOffSet);
                                    }

                                }
                                else if (AllTiles[z][i, j].HorizontalFlip && !AllTiles[z][i, j].DiagonalFlip)
                                {

                                    spriteBatch.Draw(tileSet, new Vector2(AllTiles[z][i, j].DestinationRectangle.X, AllTiles[z][i, j].DestinationRectangle.Y), AllTiles[z][i, j].SourceRectangle, Game1.GlobalClock.TimeOfDayColor,
                                    0f, Game1.Utility.Origin, 1f, SpriteEffects.FlipHorizontally, AllDepths[z] + AllTiles[z][i, j].LayerToDrawAtZOffSet);
                                }
                                else if (AllTiles[z][i, j].VeritcalFlip && !AllTiles[z][i, j].DiagonalFlip)
                                {
                                    spriteBatch.Draw(tileSet, new Vector2(AllTiles[z][i, j].DestinationRectangle.X, AllTiles[z][i, j].DestinationRectangle.Y), AllTiles[z][i, j].SourceRectangle, Game1.GlobalClock.TimeOfDayColor,
                                    0f, Game1.Utility.Origin, 1f, SpriteEffects.FlipVertically, AllDepths[z] + AllTiles[z][i, j].LayerToDrawAtZOffSet);
                                }

                                else
                                {
                                    spriteBatch.Draw(tileSet, new Vector2(AllTiles[z][i, j].DestinationRectangle.X, AllTiles[z][i, j].DestinationRectangle.Y), AllTiles[z][i, j].SourceRectangle, Game1.GlobalClock.TimeOfDayColor,
                                    0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z] + AllTiles[z][i, j].LayerToDrawAtZOffSet);
                                }



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
            AssignProperties(AllTiles[layer][oldX, oldY], 0);
        }
        public void UpdateCropTile(Crop crop)
        {
            string tileID = crop.TileID;
            int layer = int.Parse(tileID[0].ToString());
            int x = int.Parse(tileID[1].ToString() + tileID[2].ToString());
            int y = int.Parse(tileID[3].ToString() + tileID[4].ToString());
            ReplaceTilePermanent(1, x, y, crop.GID);
            if (mapName.Tilesets[TileSetNumber].Tiles.ContainsKey(crop.GID - 1))
            {
                if (mapName.Tilesets[TileSetNumber].Tiles[crop.GID - 1].Properties.ContainsKey("AssociatedTiles"))
                {
                    ReplaceTilePermanent(3, x, y - 1, int.Parse(mapName.Tilesets[TileSetNumber].Tiles[crop.GID - 1].Properties["AssociatedTiles"]));
                }
            }

            //AllTiles[layer][x, y] = new Tile(x, y, AllTiles[layer][x, y].GID + 1, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight);
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

            Tile ReplaceMenttile = new Tile(AllTiles[layer][oldX, oldY].OldX, AllTiles[layer][oldX, oldY].OldY, GID, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight);

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
        public void InteractWithBackground(int layer, GameTime gameTime, MouseManager mouse, int oldX, int oldY, string key)
        {

            switch (key)
            {
                case "diggable":
                    Game1.isMyMouseVisible = false;
                    Game1.Player.UserInterface.DrawTileSelector = true;
                    this.DebugTile = AllTiles[layer][oldX, oldY];
                    Game1.Player.UserInterface.TileSelectorX = AllTiles[layer][oldX, oldY].DestinationRectangle.X;
                    Game1.Player.UserInterface.TileSelectorY = AllTiles[layer][oldX, oldY].DestinationRectangle.Y;
                    Game1.myMouseManager.ToggleGeneralInteraction = true;
                    mouse.ChangeMouseTexture(3);

                    if (mouse.IsClicked)
                    {
                        //InteractWithBackground(z, gameTime, i, j);
                        if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() == 3)
                        {
                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                            ReplaceTileWithNewTile(layer, oldX, oldY, 86);

                        }
                    }
                    break;

                case "plantable":
                    Game1.isMyMouseVisible = false;
                    Game1.Player.UserInterface.DrawTileSelector = true;
                    this.DebugTile = AllTiles[layer][oldX, oldY];
                    Game1.Player.UserInterface.TileSelectorX = AllTiles[layer][oldX, oldY].DestinationRectangle.X;
                    Game1.Player.UserInterface.TileSelectorY = AllTiles[layer][oldX, oldY].DestinationRectangle.Y;
                    Game1.myMouseManager.ToggleGeneralInteraction = true;
                    mouse.ChangeMouseTexture(2);

                    if (mouse.IsClicked)
                    {
                        if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem() != null)
                        {
                            if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().IsPlantable)
                            {
                                if (!Game1.GetCurrentStage().AllCrops.ContainsKey(AllTiles[layer][oldX, oldY].GetTileObjectKey()))
                                {

                                    //Game1.myMouseManager.TogglePlantInteraction = true;

                                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                                    Crop tempCrop = Game1.AllCrops.GetCropFromID(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().ID);
                                    string oldXString = oldX.ToString();
                                    if (oldXString.Length < 2)
                                    {
                                        oldXString.Insert(0, "0");
                                    }
                                    if (oldXString.Length < 2)
                                    {
                                        oldXString.Insert(0, "0");
                                    }
                                    string oldYString = oldY.ToString();
                                    if (oldYString.Length < 2)
                                    {
                                        oldYString.Insert(0, "0");
                                    }
                                    if (oldYString.Length < 2)
                                    {
                                        oldYString.Insert(0, "0");
                                    }
                                    tempCrop.TileID = layer.ToString() + oldXString + oldYString;
                                    tempCrop.GID++;
                                    //AllTiles[layer][oldX, oldY].Crop = Game1.AllCrops.GetCropFromID(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().ID);
                                    ReplaceTileWithNewTile(1, oldX, oldY, tempCrop.GID);
                                    Game1.GetCurrentStage().AllCrops[AllTiles[layer][oldX, oldY].GetTileObjectKey()] = tempCrop;
                                    Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().ID);


                                }
                            }
                        }

                    }
                    break;

            }


        }

        //must be a building tile
        public void InteractWithBuilding(int layer, GameTime gameTime, int oldX, int oldY)
        {


            if (mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("portal"))
            {
                switch (mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["portal"])
                {
                    case ("lodgeInterior"):
                        {
                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DoorOpenInstance, false, 1);
                            Game1.Player.controls.Direction = Dir.Up;
                            Game1.gameStages = Stages.LodgeInteior;
                            Game1.Player.position.X = 878;
                            Game1.Player.position.Y = 809;
                            break;
                        }
                    case ("elixirShop"):
                        {
                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DoorOpenInstance, false, 1);
                            Game1.Player.controls.Direction = Dir.Up;
                            Game1.SwitchStage(Game1.GetCurrentStageInt(), 9);
                            Game1.Player.position.X = 878;
                            Game1.Player.position.Y = 809;
                            break;
                        }
                }

                if (mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["portal"] == "lodgeInterior" && Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() == 5)
                {
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DoorOpenInstance, false, 1);
                    Game1.Player.controls.Direction = Dir.Up;
                    Game1.gameStages = Stages.LodgeInteior;
                    Game1.Player.position.X = 878;
                    Game1.Player.position.Y = 809;
                }
            }
            if (mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("destructable"))
            {
                if (!AllTiles[layer][oldX, oldY].IsAnimating && !Game1.Player.CurrentAction.IsAnimated)
                {
                    if (Game1.Utility.GetRequiredTileTool(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]) == -50)
                    {
                        InteractWithoutPlayerAnimation(layer, gameTime, oldX, oldY, .25f);
                        AllTiles[layer][oldX, oldY].HitPoints--;
                    }
                    else if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() == Game1.Utility.GetRequiredTileTool(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]))
                    {
                        switch (Game1.Utility.GetRequiredTileTool(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]))
                        {
                            //bare hands

                            case 0:
                                //InteractWithoutPlayerAnimation(3, gameTime, oldX, oldY - 1, .25f);
                                InteractWithPlayerAnimation(layer, gameTime, oldX, oldY, 9, 10, 11, 12, .25f);
                                ToolInteraction(AllTiles[layer][oldX, oldY], oldX, oldY, Game1.Utility.GetTileDestructionSound(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), Game1.Utility.GetTileEffectColor(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"));
                                AllTiles[layer][oldX, oldY].HitPoints--;
                                break;

                            case 1:
                                //InteractWithoutPlayerAnimation(3, gameTime, oldX, oldY - 1, .25f);
                                InteractWithPlayerAnimation(layer, gameTime, oldX, oldY, 5, 6, 7, 8, .25f);
                                ToolInteraction(AllTiles[layer][oldX, oldY], oldX, oldY, Game1.Utility.GetTileDestructionSound(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), Game1.Utility.GetTileEffectColor(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"));
                                AllTiles[layer][oldX, oldY].HitPoints--;
                                break;
                            case 2:
                                //InteractWithoutPlayerAnimation(3, gameTime, oldX, oldY - 1, .25f);
                                InteractWithPlayerAnimation(layer, gameTime, oldX, oldY, 1, 2, 3, 4, .25f);
                                ToolInteraction(AllTiles[layer][oldX, oldY], oldX, oldY, Game1.Utility.GetTileDestructionSound(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), Game1.Utility.GetTileEffectColor(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"));
                                AllTiles[layer][oldX, oldY].HitPoints--;
                                break;
                        }

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

            if (Game1.GetCurrentStage().AllObjects[AllTiles[layer][oldX, oldY].GetTileObjectKey()] != null)
            {
                Game1.GetCurrentStage().AllObjects[AllTiles[layer][oldX, oldY].GetTileObjectKey()] = null;
            }
            //AllTiles[layer][oldX, oldY].HasObject = false;
            if (mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"))
            {

                DestroySpawnWithTiles(AllTiles[layer][oldX, oldY], oldX, oldY);
            }
            //mostly for crops
            if (mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("AssociatedTiles"))
            {
                ReplaceTilePermanent(3, oldX, oldY - 1, 0);
                //AllTiles[0][oldX, oldY].ContainsCrop = false;
            }
            GetDrop(layer, oldX, oldY);

            Game1.SoundManager.PlaySoundEffectFromInt(false, 1, Game1.Utility.GetTileDestructionSound(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), 1f);

            ReplaceTilePermanent(layer, oldX, oldY, 0);





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
                if (AllTiles[layer][oldX, oldY].IsFinishedAnimating && Game1.GetCurrentStage().AllObjects[AllTiles[layer][oldX, oldY].GetTileObjectKey()] != null)
                {
                    //ObjectBody newObject = new ObjectBody();
                    Game1.GetCurrentStage().AllObjects[AllTiles[layer][oldX, oldY].GetTileObjectKey()] = null;
                    GetDrop(layer, oldX, oldY);
                    if (mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"))
                    {
                        DestroySpawnWithTiles(AllTiles[layer][oldX, oldY], oldX, oldY);
                    }



                    ReplaceTilePermanent(layer, oldX, oldY, 0);

                }
            }
            else
            {
                Game1.GetCurrentStage().AllObjects[AllTiles[layer][oldX, oldY].GetTileObjectKey()] = null;

                GetDrop(layer, oldX, oldY);
                //AllTiles[layer][oldX, oldY].ContainsCrop = false;
                if (mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"))
                {
                    DestroySpawnWithTiles(AllTiles[layer][oldX, oldY], oldX, oldY);
                }
                ReplaceTilePermanent(layer, oldX, oldY, 0);
            }
        }

        public void GetDrop(int layer, int x, int y)
        {
            List<Loot> tempLoot = Game1.Utility.Parselootkey(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][x, y].GID].Properties["loot"]);

            if (tempLoot != null)
            {


                for (int l = 0; l < tempLoot.Count; l++)
                {
                    int lootCount = Game1.Utility.DetermineLootDrop(tempLoot[l]);
                    for (int d = 0; d < lootCount; d++)
                    {
                        Item item = Game1.ItemVault.GenerateNewItem(tempLoot[l].ID, new Vector2(AllTiles[layer][x, y].DestinationRectangle.X, AllTiles[layer][x, y].DestinationRectangle.Y), true);
                        Game1.GetCurrentStage().AllItems.Add(item);
                    }
                }
            }

        }

    }
    #endregion
}