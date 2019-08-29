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
    public class ProceduralTileManager : ITileManager
    {
        protected Game1 game;

        protected Texture2D tileSet;



        public int tilesetTilesWide { get; set; }
        public int tilesetTilesHigh { get; set; }
        public int WorldWidth { get; set; }
        public int WorldHeight { get; set; }


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

        public List<Tile[,]> AllTiles { get; set; }

        public List<float> AllDepths;

        public bool TileInteraction { get; set; } = false;

        public Tile DebugTile { get; set; } = new Tile(40, 40, 4714, 100, 100, 100, 100);

        public int TileSetNumber { get; set; }
        public AStarPathFinder PathGrid { get; set; }

        public List<int> DirtGeneratableTiles;
        public List<int> SandGeneratableTiles;
        public List<int> GrassGeneratableTiles;

        public Dictionary<float, EditableAnimationFrameHolder> AnimationFrames { get; set; }

        public int NumberOfLayers { get; set; }
        public int mapWidth { get; set; }
        public int mapHeight { get; set; }

        TmxMap MapName;
        #region CONSTRUCTOR

        private ProceduralTileManager()
        {

        }

        public ProceduralTileManager(World world, Texture2D tileSet, List<TmxLayer> allLayers, TmxMap mapName, int numberOfLayers, int worldWidth, int worldHeight, GraphicsDevice graphicsDevice, ContentManager content, int tileSetNumber, List<float> allDepths, World currentStage)
        {
            this.MapName = mapName;
            this.tileSet = tileSet;

            tileWidth = 16;
            tileHeight = 16;

            tilesetTilesWide = tileSet.Width / tileWidth;
            tilesetTilesHigh = tileSet.Height / tileHeight;

            WorldWidth = worldWidth;
            WorldHeight = worldHeight;
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
            AnimationFrames = new Dictionary<float, EditableAnimationFrameHolder>();

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
                        }


                    }
                }
            }




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
                            AllTiles[z][i, j] = new Tile(i, j, 1005, tilesetTilesWide, tilesetTilesHigh, worldWidth, worldHeight);
                        }



                    }
                }

            }

            //for (int i = 0; i < Map.ObjectGroups["Portal"].Objects.Count; i++)
            //    {
            //        string keyFrom;
            //        string keyTo;
            //        string safteyX;
            //        string safteyY;
            //        string click;


            //        Map.ObjectGroups["Portal"].Objects[i].Properties.TryGetValue("standardFrom", out keyFrom);
            //        Map.ObjectGroups["Portal"].Objects[i].Properties.TryGetValue("standardTo", out keyTo);
            //        Map.ObjectGroups["Portal"].Objects[i].Properties.TryGetValue("SafteyOffSetX", out safteyX);
            //        Map.ObjectGroups["Portal"].Objects[i].Properties.TryGetValue("SafteyOffSetY", out safteyY);
            //        Map.ObjectGroups["Portal"].Objects[i].Properties.TryGetValue("Click", out click);
            //        Portal portal = new Portal(int.Parse(keyFrom), int.Parse(keyTo), int.Parse(safteyX), int.Parse(safteyY), bool.Parse(click));


            //        int portalX = (int)Map.ObjectGroups["Portal"].Objects[i].X;
            //        int portalY = (int)Map.ObjectGroups["Portal"].Objects[i].Y;
            //        int portalWidth = (int)Map.ObjectGroups["Portal"].Objects[i].Width;
            //        int portalHeight = (int)Map.ObjectGroups["Portal"].Objects[i].Height;

            //        portal.PortalStart = new Rectangle(portalX, portalY, portalWidth, portalHeight);

            //        currentStage.AllPortals.Add(portal);
            //    }

            //specify GID which is 1 larger than one on tileset, idk why
            //brown tall grass
            // GenerateTiles(3, 6394, "dirt", 2000, 0);
            //green tall grass
            // GenerateTiles(3, 6393, "dirt", 2000, 0,world);
            //stone
            GenerateTiles(1, 979, "dirt", 1000, 0, world);
            //    //grass
            GenerateTiles(1, 1079, "dirt", 1000, 0, world);
            //    //redrunestone
            GenerateTiles(1, 579, "dirt", 500, 0, world);
            ////bluerunestone
            GenerateTiles(1, 779, "dirt", 1000, 0, world);
            ////thunderbirch
            GenerateTiles(1, 2264, "dirt", 1000, 0, world);
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
                for (int i = 0; i < this.WorldWidth; i++)
                {
                    for (int j = 0; j < this.WorldHeight; j++)
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
        public void AssignProperties(Tile tileToAssign, int tileSetNumber, int layer, int oldX, int oldY, ILocation stage)
        {
            if (MapName.Tilesets[tileSetNumber].Tiles.ContainsKey(tileToAssign.GID))
            {


                //Note: For some fuckin reason everything here is doubled. So if you want to spawn more trees etc make sure you specify HALF of what rectangle offsets should be. 
                //come kiss me when you figure it out in a few months ;O


                if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].AnimationFrames.Count > 0 && !MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("idleStart"))
                {
                    List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                    for (int i = 0; i < MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].AnimationFrames.Count; i++)
                    {
                        frames.Add(new EditableAnimationFrame(MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].AnimationFrames[i]));
                    }
                    EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, oldX, oldY, layer, tileToAssign.GID);
                    this.AnimationFrames.Add(tileToAssign.GetTileKey(this.WorldWidth, this.WorldHeight), frameHolder);
                }
                if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("lightSource"))
                {
                    int lightType = int.Parse(MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["lightSource"]);
                    stage.AllLights.Add(new LightSource(lightType, new Vector2(GetDestinationRectangle(tileToAssign).X, GetDestinationRectangle(tileToAssign).Y)));
                }


                if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("destructable"))
                {
                    tileToAssign.HitPoints = Game1.Utility.GetTileHitpoints(MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["destructable"]);

                }

                if (MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties.ContainsKey("layer"))
                {
                    tileToAssign.LayerToDrawAt = int.Parse(MapName.Tilesets[tileSetNumber].Tiles[tileToAssign.GID].Properties["layer"]);
                    //grass = 1, stone = 2, wood = 3, sand = 4
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


        public void GenerateTiles(int layerToPlace, int gid, string placementKey, int frequency, int layerToCheckIfEmpty, ILocation world)
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
                GenerateRandomTiles(layerToPlace, gid, acceptableGenerationTiles, world, layerToCheckIfEmpty);
            }
        }
        public void GenerateRandomTiles(int layer, int id, List<int> acceptableTiles, ILocation stage, int comparisonLayer = 0)
        {
            int newTileX = Game1.Utility.RNumber(50, WorldWidth - 50);
            int newTileY = Game1.Utility.RNumber(50, WorldHeight - 50);
            if (!CheckIfTileAlreadyExists(newTileX, newTileY, layer) && CheckIfTileMatchesGID(newTileX, newTileY, layer, acceptableTiles, comparisonLayer))
            {

                Tile sampleTile = new Tile(newTileX, newTileY, id, tilesetTilesWide, tilesetTilesHigh, WorldWidth, WorldHeight);
                if (!MapName.Tilesets[TileSetNumber].Tiles[sampleTile.GID].Properties.ContainsKey("spawnWith"))
                {
                    AllTiles[layer][newTileX, newTileY] = new Tile(newTileX, newTileY, id, tilesetTilesWide, tilesetTilesHigh, WorldWidth, WorldHeight);
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
                            intermediateNewTiles.Add(new Tile(newTileX + intGidX, newTileY + intGidY, totalGID + 1, tilesetTilesWide, tilesetTilesHigh, WorldWidth, WorldHeight) { LayerToDrawAt = intTilePropertyLayer });
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
                    AllTiles[layer][newTileX, newTileY] = new Tile(newTileX, newTileY, id, tilesetTilesWide, tilesetTilesHigh, WorldWidth, WorldHeight);
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


                    if (world.AllObjects.ContainsKey(AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileKey(this.WorldWidth, this.WorldHeight)))
                    {
                        world.AllObjects.Remove(AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY].GetTileKey(this.WorldWidth, this.WorldHeight));
                    }

                    AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY] = new Tile(xCoord + intGidX, yCoord + intGidY, 0, tilesetTilesWide, tilesetTilesHigh, WorldWidth, WorldHeight);
                }
            }
        }

        public void LoadInitialTileObjects(ILocation location)
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
                                if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                {

                                    if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].ObjectGroups.Count > 0)
                                    {


                                        for (int k = 0; k < MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].ObjectGroups[0].Objects.Count; k++)
                                        {
                                            TmxObject tempObj = MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].ObjectGroups[0].Objects[k];


                                            ObjectBody tempObjectBody = new ObjectBody(graphicsDevice,
                                                new Rectangle(GetDestinationRectangle(AllTiles[z][i, j]).X + (int)Math.Ceiling(tempObj.X),
                                                GetDestinationRectangle(AllTiles[z][i, j]).Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                                                (int)Math.Ceiling(tempObj.Height)), AllTiles[z][i, j].GID);



                                            location.AllObjects[AllTiles[z][i, j].GetTileKey(this.WorldWidth, this.WorldHeight)] = tempObjectBody; // not gonna work for saving, gotta figure out.

                                        }
                                        AllTiles[z][i, j].AStarTileValue = 0;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            PathGrid = new AStarPathFinder(WorldWidth, WorldHeight, AllTiles[1]);
        }


        //if the GID is anything other than -1 it means there's something there.
        public bool CheckIfTileAlreadyExists(int tileX, int tileY, int layer)
        {
            //int debuggid = AllTiles[layer][tileX, tileY].GID;
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







        #region ADDOBJECTSTOBUILDINGS

        public void AddObjectToBuildingTile(Tile tile, int indexX, int indexY, Rectangle destinationRectangle, World stage)
        {
            for (int z = 0; z < AllTiles.Count; z++)
            {
                AddObject(AllTiles[z][indexX, indexY], stage, destinationRectangle);

            }
        }

        public void AddObject(Tile tile, World stage, Rectangle destinationRectangle)
        {
            if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(tile.GID))
            {
                for (int k = 0; k < MapName.Tilesets[TileSetNumber].Tiles[tile.GID].ObjectGroups[0].Objects.Count; k++)
                {

                    TmxObject tempObj = MapName.Tilesets[TileSetNumber].Tiles[tile.GID].ObjectGroups[0].Objects[k];
                    ObjectBody transformedObject = new ObjectBody(graphicsDevice,
                       new Rectangle(destinationRectangle.X + (int)Math.Ceiling(tempObj.X),
                       destinationRectangle.Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                       (int)Math.Ceiling(tempObj.Height)), tile.GID);

                    // tile.HasObject = true;
                    stage.AllObjects[tile.GetTileKey(this.WorldWidth, this.WorldHeight)] = transformedObject;
                }
            }
        }
        #endregion

        public void ReplaceTilePermanent(int layer, int oldX, int oldY, int gid, ILocation stage)
        {
            Tile ReplaceMenttile = new Tile(AllTiles[layer][oldX, oldY].X, AllTiles[layer][oldX, oldY].Y, gid, tilesetTilesWide, tilesetTilesHigh, WorldWidth, WorldHeight);
            AllTiles[layer][oldX, oldY] = ReplaceMenttile;
            AssignProperties(AllTiles[layer][oldX, oldY], 0, layer, oldX, oldY, stage);
        }

        #region UPDATE

        public void Update(GameTime gameTime, MouseManager mouse)
        {
            //Game1.myMouseManager.TogglePlantInteraction = false;
            Game1.Player.UserInterface.DrawTileSelector = false;
            List<float> AnimationFrameKeysToRemove = new List<float>();
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
            if (endi > this.WorldWidth)
            {
                endi = (int)(Game1.cam.Pos.X / 16) + (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2 / 16) + 1;
            }
            int endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2 / 16) + 2;
            if (endj > this.WorldWidth)
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
                        frameholder.Frames[frameholder.Counter].ID + 1, this.tilesetTilesWide, this.tilesetTilesHigh, this.WorldWidth, this.WorldHeight);
                    if (frameholder.Counter == frameholder.Frames.Count - 1)
                    {
                        if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(frameholder.OriginalTileID))
                        {

                            if (MapName.Tilesets[TileSetNumber].Tiles[frameholder.OriginalTileID].Properties.ContainsKey("destructable"))
                            {

                                //needs to refer to first tile ?
                                int frameolDX = frameholder.OldX;
                                AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY] = new Tile(frameholder.OldX, frameholder.OldY, frameholder.OriginalTileID + 1,
                                    this.tilesetTilesWide, this.tilesetTilesHigh, this.WorldWidth, this.WorldHeight);
                                AnimationFrameKeysToRemove.Add(AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY].GetTileKey(this.WorldWidth, this.WorldHeight));
                                Destroy(frameholder.Layer, frameholder.OldX, frameholder.OldY, GetDestinationRectangle(AllTiles[frameholder.Layer][frameholder.OldX, frameholder.OldY]), Game1.GetCurrentStage());
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

            foreach (int key in AnimationFrameKeysToRemove)
            {
                AnimationFrames.Remove(key);
            }




            for (int z = 0; z < AllTiles.Count; z++)
            {
                for (var i = starti; i < endi; i++)
                {
                    for (var j = startj; j < endj; j++)
                    {

                        Rectangle destinationRectangle = GetDestinationRectangle(AllTiles[z][i, j]);


                        if (AllTiles[z][i, j].GID != -1)
                        {



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
                                        if (z == 0)
                                        {
                                            if (AllTiles[1][i, j].GID == -1 && MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("action"))
                                            {

                                                InteractWithBackground(z, gameTime, mouse, i, j, MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["action"], destinationRectangle);


                                            }

                                        }
                                        if (z == 1)
                                        {


                                            if (AllTiles[1][i, j].GID != -1 && MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("destructable")) //&& mapName.Tilesets[0].Tiles.ContainsKey(tiles[i, j].GID not sure what this was for.
                                            {
                                                Game1.Player.UserInterface.DrawTileSelector = true;
                                                Game1.isMyMouseVisible = false;
                                                Game1.Player.UserInterface.TileSelectorX = destinationRectangle.X;
                                                Game1.Player.UserInterface.TileSelectorY = destinationRectangle.Y;

                                                mouse.ChangeMouseTexture(Game1.Utility.GetRequiredTileTool(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["destructable"]));

                                                Game1.myMouseManager.ToggleGeneralInteraction = true;



                                            }
                                            if (mouse.IsClicked)
                                            {
                                                InteractWithBuilding(z, gameTime, i, j, destinationRectangle, Game1.GetCurrentStage());

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
        //TODO: Fix animation frames
        public void AnimationFrameAnimate(Tile tile)
        {
            if (MapName.Tilesets[tile.GID].Tiles[tile.GID].AnimationFrames.Count > 0)
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
                    //if (AllTiles[z][i, j].GID == 4654)
                    //{
                    //    ReplaceTilePermanent(1, i, j, 4657);

                    //    ReplaceTilePermanent(1, i - 1, j, 4658);
                    //    AllTiles[z][i, j - 2].IsAnimating = true;
                    //    AllTiles[z][i, j - 2].Kill = false;
                    //    AllTiles[z][i, j - 2].IsFinishedAnimating = false;
                    //    AllTiles[z][i, j - 2].IsAnimated = true;

                    //}
                    //else if (AllTiles[z][i, j].GID == 4653)
                    //{
                    //    ReplaceTilePermanent(1, i, j, 4656);

                    //    ReplaceTilePermanent(1, i + 1, j, 4657);
                    //    AllTiles[z][i + 1, j - 2].IsAnimating = true;
                    //    AllTiles[z][i + 1, j - 2].Kill = false;
                    //    AllTiles[z][i + 1, j - 2].IsFinishedAnimating = false;
                    //    AllTiles[z][i + 1, j - 2].IsAnimated = true;

                    //}



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
            if (endi > this.WorldWidth)
            {
                endi = (int)(Game1.cam.Pos.X / 16) + (int)(Game1.ScreenWidth / Game1.cam.Zoom / 2 / 16) + 1;
            }
            int endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2 / 16) + 2;
            if (endj > this.WorldWidth)
            {
                endj = (int)(Game1.cam.Pos.Y / 16) + (int)(Game1.ScreenHeight / Game1.cam.Zoom / 2 / 16) + 1;
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


                            if (MapName.Tilesets[this.TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("newSource"))
                            {
                                int[] rectangleCoords = Game1.Utility.GetNewTileSourceRectangle(MapName.Tilesets[this.TileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["newSource"]);


                                SourceRectangle = new Rectangle(SourceRectangle.X + rectangleCoords[0], SourceRectangle.Y + rectangleCoords[1],
                                    SourceRectangle.Width + rectangleCoords[2], SourceRectangle.Height + rectangleCoords[3]);


                                DestinationRectangle = new Rectangle(DestinationRectangle.X + rectangleCoords[0], DestinationRectangle.Y + rectangleCoords[1],
                                    DestinationRectangle.Width, DestinationRectangle.Height);
                            }
                            int randomInt = Game1.Utility.RGenerator.Next(1, 1000);
                            float randomFloat = (float)(randomInt * .000001);
                            float layerToDrawAtZOffSet = (DestinationRectangle.Y + DestinationRectangle.Height) * .0001f + randomFloat;

                            AllDepths[3] = .4f + (float)(DestinationRectangle.Bottom + DestinationRectangle.Height / WorldHeight * this.tileHeight) / (float)10000;


                            spriteBatch.Draw(tileSet, new Vector2(DestinationRectangle.X, DestinationRectangle.Y), SourceRectangle, Game1.GlobalClock.TimeOfDayColor,
                            0f, Game1.Utility.Origin, 1f, SpriteEffects.None, AllDepths[z] + layerToDrawAtZOffSet);

                        }
                    }
                }
            }
        }
        #endregion

        #region REPLACETILES





        public void ReplaceTileTemporary(int layer, int oldX, int oldY, int GID, float colorMultiplier, int xArrayLength, int yArrayLength)
        {
            if (TempTile != null)
            {
                if (AllTiles[layer][CurrentIndexX, CurrentIndexY] == AllTiles[layer][OldIndexX, OldIndexY])
                {
                    AllTiles[layer][OldIndexX, OldIndexY].GID = 1;
                }
            }

            Tile ReplaceMenttile = new Tile(AllTiles[layer][oldX, oldY].X, AllTiles[layer][oldX, oldY].Y, GID, tilesetTilesWide, tilesetTilesHigh, WorldWidth, WorldHeight);

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
            Tile ReplaceMenttile = new Tile(AllTiles[layer][tileToReplaceX, tileToReplaceY].X, AllTiles[layer][tileToReplaceX, tileToReplaceY].Y, newTileGID, tilesetTilesWide, tilesetTilesHigh, WorldWidth, WorldHeight);
            AllTiles[layer][tileToReplaceX, tileToReplaceY] = ReplaceMenttile;
        }
        #endregion

        #region INTERACTIONS

        //Used for interactions with background tiles only
        public void InteractWithBackground(int layer, GameTime gameTime, MouseManager mouse, int oldX, int oldY, string key, Rectangle destinationRectangle)
        {

            switch (key)
            {
                case "diggable":
                    Game1.isMyMouseVisible = false;
                    Game1.Player.UserInterface.DrawTileSelector = true;
                    this.DebugTile = AllTiles[layer][oldX, oldY];
                    Game1.Player.UserInterface.TileSelectorX = destinationRectangle.X;
                    Game1.Player.UserInterface.TileSelectorY = destinationRectangle.Y;
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
                    Game1.Player.UserInterface.TileSelectorX = destinationRectangle.X;
                    Game1.Player.UserInterface.TileSelectorY = destinationRectangle.Y;
                    Game1.myMouseManager.ToggleGeneralInteraction = true;
                    mouse.ChangeMouseTexture(2);

                    if (mouse.IsClicked)
                    {
                        if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem() != null)
                        {
                            if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().IsPlantable)
                            {
                                if (!Game1.GetCurrentStage().AllCrops.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(this.WorldWidth, this.WorldHeight)))
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
                                    Game1.GetCurrentStage().AllCrops[AllTiles[layer][oldX, oldY].GetTileKey(this.WorldWidth, this.WorldHeight)] = tempCrop;
                                    Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().ID);


                                }
                            }
                        }

                    }
                    break;

            }


        }

        //must be a building tile
        public void InteractWithBuilding(int layer, GameTime gameTime, int oldX, int oldY, Rectangle destinationRectangle, ILocation world)
        {


            if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("portal"))
            {
                switch (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["portal"])
                {
                    case ("lodgeInterior"):
                        {
                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DoorOpenInstance, false, 1);
                            Game1.Player.controls.Direction = Dir.Up;
                            Game1.gameStages = Stages.World;
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

                if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["portal"] == "lodgeInterior" && Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool() == 5)
                {
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DoorOpenInstance, false, 1);
                    Game1.Player.controls.Direction = Dir.Up;
                    Game1.gameStages = Stages.World;
                    Game1.Player.position.X = 878;
                    Game1.Player.position.Y = 809;
                }
            }
            if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("destructable"))
            {
                if (!AnimationFrames.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(this.WorldWidth, this.WorldHeight)) && !Game1.Player.CurrentAction[0, 0].IsAnimated)
                {
                    if (Game1.Utility.GetRequiredTileTool(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]) == -50)
                    {
                        InteractWithoutPlayerAnimation(layer, gameTime, oldX, oldY, destinationRectangle, world, delayTimer: .25f);
                        AllTiles[layer][oldX, oldY].HitPoints--;
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
                                AllTiles[layer][oldX, oldY].HitPoints--;
                                break;

                            case 1:
                                //InteractWithoutPlayerAnimation(3, gameTime, oldX, oldY - 1, .25f);
                                InteractWithPlayerAnimation(layer, gameTime, oldX, oldY, 5, 6, 7, 8, destinationRectangle, .25f);
                                ToolInteraction(AllTiles[layer][oldX, oldY], layer, oldX, oldY, Game1.Utility.GetTileDestructionSound(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]),
                                    Game1.Utility.GetTileEffectColor(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), world, destinationRectangle, MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"));
                                AllTiles[layer][oldX, oldY].HitPoints--;
                                break;
                            case 2:
                                //InteractWithoutPlayerAnimation(3, gameTime, oldX, oldY - 1, .25f);
                                InteractWithPlayerAnimation(layer, gameTime, oldX, oldY, 1, 2, 3, 4, destinationRectangle, .25f);
                                ToolInteraction(AllTiles[layer][oldX, oldY], layer, oldX, oldY, Game1.Utility.GetTileDestructionSound(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]),
                                    Game1.Utility.GetTileEffectColor(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), world, destinationRectangle, MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"));
                                AllTiles[layer][oldX, oldY].HitPoints--;
                                break;
                        }

                    }

                }
            }

        }

        public void ToolInteraction(Tile tile, int layer, int x, int y, int setSoundInt, Color particleColor, ILocation world, Rectangle destinationRectangle, bool hasSpawnTiles = false)
        {
            if (tile.HitPoints >= 1)
            {
                Game1.SoundManager.PlaySoundEffectFromInt(false, 1, setSoundInt, 1f);
                Game1.GetCurrentStage().ParticleEngine.Color = particleColor;
                Game1.GetCurrentStage().ParticleEngine.ActivationTime = .25f;
                Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(destinationRectangle.X + 5, destinationRectangle.Y - 20);
            }

            if (tile.HitPoints < 1)
            {
                Game1.SoundManager.PlaySoundEffectFromInt(false, 1, setSoundInt, 1f);
                //tile.IsFinishedAnimating = true;
                if (hasSpawnTiles)
                {
                    DestroySpawnWithTiles(tile, x, y, world);
                }
                if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][x, y].GID].AnimationFrames.Count > 0)
                {
                    List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
                    for (int i = 0; i < MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][x, y].GID].AnimationFrames.Count; i++)
                    {
                        frames.Add(new EditableAnimationFrame(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][x, y].GID].AnimationFrames[i]));
                    }
                    EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, x, y, layer, AllTiles[layer][x, y].GID);
                    this.AnimationFrames.Add(AllTiles[layer][x, y].GetTileKey(this.WorldWidth, this.WorldHeight), frameHolder);
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


        //interact without any player animations
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
                this.AnimationFrames.Add(AllTiles[layer][oldX, oldY].GetTileKey(this.WorldWidth, this.WorldHeight), frameHolder);
            }

            if (Game1.GetCurrentStage().AllObjects.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(this.WorldWidth, this.WorldHeight)))
            {
                Game1.GetCurrentStage().AllObjects.Remove(AllTiles[layer][oldX, oldY].GetTileKey(this.WorldWidth, this.WorldHeight));
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
                Game1.GetCurrentStage().AllCrops.Remove(AllTiles[0][oldX, oldY].GetTileKey(this.WorldWidth, this.WorldHeight));
                //AllTiles[0][oldX, oldY].ContainsCrop = false;
            }
            GetDrop(layer, oldX, oldY, destinationRectangle);

            Game1.SoundManager.PlaySoundEffectFromInt(false, 1, Game1.Utility.GetTileDestructionSound(MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties["destructable"]), 1f);

            ReplaceTilePermanent(layer, oldX, oldY, 0, world);





        }

        //specify which animations you want to use depending on where the player is in relation to the object

        public void InteractWithPlayerAnimation(int layer, GameTime gameTime, int oldX, int oldY, int down, int right, int left, int up, Rectangle destinationRectangle, float delayTimer = 0f)
        {


            //if (mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].AnimationFrames.Count > 0)
            //{
            //    List<EditableAnimationFrame> frames = new List<EditableAnimationFrame>();
            //    for (int i = 0; i < mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].AnimationFrames.Count; i++)
            //    {
            //        frames.Add(new EditableAnimationFrame(mapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].AnimationFrames[i]));
            //    }
            //    EditableAnimationFrameHolder frameHolder = new EditableAnimationFrameHolder(frames, oldX, oldY, layer, AllTiles[layer][oldX, oldY].GID);
            //    this.AnimationFrames.Add(AllTiles[layer][oldX, oldY].GetTileObjectKey(), frameHolder);
            //}


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
                if (!AnimationFrames.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(this.WorldWidth, this.WorldHeight)) && Game1.GetCurrentStage().AllObjects.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(this.WorldWidth, this.WorldHeight)))
                {
                    //ObjectBody newObject = new ObjectBody();
                    Game1.GetCurrentStage().AllObjects.Remove(AllTiles[layer][oldX, oldY].GetTileKey(this.WorldWidth, this.WorldHeight));
                    GetDrop(layer, oldX, oldY, destinationRectangle);
                    if (MapName.Tilesets[TileSetNumber].Tiles[AllTiles[layer][oldX, oldY].GID].Properties.ContainsKey("spawnWith"))
                    {
                        DestroySpawnWithTiles(AllTiles[layer][oldX, oldY], oldX, oldY, world);
                    }



                    ReplaceTilePermanent(layer, oldX, oldY, 0, world);

                }
                else
                {
                    Game1.GetCurrentStage().AllObjects.ContainsKey(AllTiles[layer][oldX, oldY].GetTileKey(this.WorldWidth, this.WorldHeight));
                    Game1.GetCurrentStage().AllObjects.Remove(AllTiles[layer][oldX, oldY].GetTileKey(this.WorldWidth, this.WorldHeight));
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

        public void UpdateCropTile(Crop crop, ILocation stage)
        {
            string tileID = crop.TileID;
            int layer = int.Parse(tileID[0].ToString());
            int x = int.Parse(tileID[1].ToString() + tileID[2].ToString());
            int y = int.Parse(tileID[3].ToString() + tileID[4].ToString());
            ReplaceTilePermanent(1, x, y, crop.GID, stage);
            if (MapName.Tilesets[TileSetNumber].Tiles.ContainsKey(crop.GID - 1))
            {
                if (MapName.Tilesets[TileSetNumber].Tiles[crop.GID - 1].Properties.ContainsKey("AssociatedTiles"))
                {
                    ReplaceTilePermanent(3, x, y - 1, int.Parse(MapName.Tilesets[TileSetNumber].Tiles[crop.GID - 1].Properties["AssociatedTiles"]), stage);
                }
            }
        }



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
}