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
        public int tileWidth{ get; set; } 
        public int tileHeight { get; set; }
        public int tileNumber { get; set; }


        //--------------------------------------
        //Counting
        public int tileCounter { get; set; }


        //--------------------------------------
        //2D Array of All Tiles
        
        [XmlIgnore]
        public Tile[,] Tiles { get; set; }

        [XmlArray("JaggedTiles")]
        public Tile[][] JaggedTiles { get; set; }  

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



            for (int i = 0; i < allLayers.Count; i++)
            {
                AllTiles.Add(new Tile[mapName.Width, mapName.Height]);

            }

            //Tiles = new Tile[tilesetTilesWide, tilesetTilesHigh];

            for (int i = 0; i < AllTiles.Count; i++)
            {
                foreach (TmxLayerTile layerNameTile in AllLayers[i].Tiles)
                {
                    Tile tempTile = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight);
                    AllTiles[i][layerNameTile.X, layerNameTile.Y] = tempTile;


                }
            }

            for (int prelimZ = 0; prelimZ < AllTiles.Count; prelimZ++)
            {
                for (int prelimI = 0; prelimI < tilesetTilesWide; prelimI++)
                {
                    for (int prelimJ = 0; prelimJ < tilesetTilesHigh; prelimJ++)
                    {
                        if (AllTiles[prelimZ][prelimI, prelimJ].GID != 0)
                        {
                            if (mapName.Tilesets[tileSetNumber].Tiles.ContainsKey(AllTiles[prelimZ][prelimI, prelimJ].GID))
                            {
                                if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[prelimZ][prelimI, prelimJ].GID].Properties.ContainsKey("spawnWith"))
                                {
                                    string value = "";
                                    mapName.Tilesets[tileSetNumber].Tiles[AllTiles[prelimZ][prelimI, prelimJ].GID].Properties.TryGetValue("spawnWith", out value);


                                    AllTiles[prelimZ][prelimI, prelimJ].SpawnsWith = Game1.Utility.ParseSpawnsWithKey(value);
                                }
                            }
                        }

                    }
                }
            }


                //stone
                GenerateTiles(1, 6675, "dirt", 100, 0);
                //grass
                GenerateTiles(1, 6475, "dirt", 100, 0);
                //redrunestone
                GenerateTiles(1, 5681, "dirt", 100, 0);
                GenerateTiles(1, 5881, "dirt", 100, 0);
            //thunderbirch
                GenerateTiles(1, 4845, "dirt", 100, 0);


                for (int z = 0; z < AllTiles.Count; z++)
                {
                    for (int i = 0; i < tilesetTilesWide; i++)
                    {
                        for (int j = 0; j < tilesetTilesHigh; j++)
                        {
                            //if foreground
                            if (z == 3)
                            {
                                //tile is red runestop bottom
                                if (j < 99 && AllTiles[1][i, j + 1].GID == 5680)
                                {
                                    AllTiles[3][i, j] = new Tile(i, j, 5581, 100, 100, 100, 100) { IsAnimated = true, Speed = .15f, TotalFramesX = 7 };
                                }
                                if (j < 99 && AllTiles[1][i, j + 1].GID == 5880)
                                {
                                    AllTiles[3][i, j] = new Tile(i, j, 5781, 100, 100, 100, 100) { IsAnimated = true, Speed = .15f, TotalFramesX = 7 };
                                }
                            }
                            if (AllTiles[z][i, j].GID != 0)
                            {
                                if (mapName.Tilesets[tileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                {

                                    AllTiles[z][i, j].LayerToDrawAt = AllDepths[z];
                                    if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("portal"))
                                    {
                                        AllTiles[z][i, j].IsPortal = true;
                                        AllTiles[z][i, j].portalDestination = mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["portal"];
                                    }
                                    if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("plantable"))
                                    {///////////
                                        AllTiles[z][i, j].Plantable = true;
                                    }
                                    if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("diggable"))
                                    {
                                        AllTiles[z][i, j].Diggable = true;
                                    }
                                    if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsValue("dirt"))
                                    {
                                        AllTiles[z][i, j].Dirt = true;
                                    }

                                    if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("Probability"))
                                    {
                                        AllTiles[z][i, j].Probability = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["Probability"]);
                                    }
                                    if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("Tree"))
                                    {
                                        AllTiles[z][i, j].Tree = true;
                                        AllTiles[z][i, j].AssociatedItem = 123;

                                    }

                                    if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("AnimatedX") || mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("AnimatedY"))
                                    {
                                        AllTiles[z][i, j].IsAnimated = true;
                                        if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("AnimatedX"))
                                        {
                                            AllTiles[z][i, j].TotalFramesX = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["AnimatedX"]);
                                            AllTiles[z][i, j].Speed = double.Parse(mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["Speed"]);
                                        }
                                        if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("AnimatedY"))
                                        {
                                            AllTiles[z][i, j].TotalFramesY = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["AnimatedY"]);
                                            AllTiles[z][i, j].Speed = double.Parse(mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["Speed"]);
                                        }



                                        if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("start"))
                                        {
                                            AllTiles[z][i, j].IsAnimating = true;
                                            AllTiles[z][i, j].Kill = false;

                                        }
                                        if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("grass"))
                                        {
                                            //tiles[i, j].Properties.Add("grass", true);
                                            AllTiles[z][i, j].Grass = true;
                                            AllTiles[z][i, j].AssociatedItem = 129;
                                        }
                                        if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("stone"))
                                        {
                                            //tiles[i, j].Properties.Add("stone", true);
                                            AllTiles[z][i, j].Stone = true;
                                            AllTiles[z][i, j].AssociatedItem = 130;
                                        }
                                        if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("redRuneStone"))
                                        {
                                            //tiles[i, j].Properties.Add("redRuneStone", true);
                                            AllTiles[z][i, j].RedRuneStone = true;
                                            AllTiles[z][i, j].AssociatedItem = 169;
                                            // Game1.GetCurrentStage().ForeGroundTiles.Tiles[i, j].GID = 5580;
                                        }
                                        if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("blueRuneStone"))
                                        {
                                            //tiles[i, j].Properties.Add("redRuneStone", true);
                                            AllTiles[z][i, j].BlueRuneStone = true;
                                            AllTiles[z][i, j].AssociatedItem = 149;
                                            // Game1.GetCurrentStage().ForeGroundTiles.Tiles[i, j].GID = 5580;
                                        }
                                    }
                                    if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("step"))
                                    {
                                        AllTiles[z][i, j].HasSound = true;
                                        AllTiles[z][i, j].SoundValue = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["step"]);
                                        //grass = 1, stone = 2, wood = 3, sand = 4
                                    }

                                }
                            }
                        }
                    }
                }
                //this.JaggedTiles = GetJaggedTiles();
            
        }
        #endregion

        

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

                //if(AllTiles[layer][prelimI, prelimJ].SpawnsWith = Game1.Utility.ParseSpawnsWithKey(value);

                //Tiles[newTileX, newTileY].GID = 6675;
                //if (AllTiles[layer][newTileX, newTileY].)

                AllTiles[layer][newTileX, newTileY] = new Tile(newTileX, newTileY, id, 100, 100, 100, 100);
                Tile sampleTile = new Tile(newTileX, newTileY, id, 100, 100, 100, 100);
                if (mapName.Tilesets[0].Tiles[sampleTile.GID].Properties.ContainsKey("spawnWith"))
                {
                    string value = "";
                    mapName.Tilesets[0].Tiles[sampleTile.GID].Properties.TryGetValue("spawnWith", out value);

                    List<Tile> intermediateAllTiles = new List<Tile>();
                    List<Tile> intermediateNewTiles = new List<Tile>();
                    sampleTile.SpawnsWith = Game1.Utility.ParseSpawnsWithKey(value);
                    for (int index = 0; index < sampleTile.SpawnsWith.Length; index++)
                    {
                        string gidX = "";
                        mapName.Tilesets[0].Tiles[sampleTile.SpawnsWith[index]].Properties.TryGetValue("relationX", out gidX);
                        string gidY = "";
                        mapName.Tilesets[0].Tiles[sampleTile.SpawnsWith[index]].Properties.TryGetValue("relationY", out gidY);
                        string tilePropertyLayer = "";
                        mapName.Tilesets[0].Tiles[sampleTile.SpawnsWith[index]].Properties.TryGetValue("layer", out tilePropertyLayer);
                        int intGidX = int.Parse(gidX);
                        int intGidY = int.Parse(gidY);
                        int intTilePropertyLayer = int.Parse(tilePropertyLayer);

                        int totalGID = mapName.Tilesets[0].Tiles[sampleTile.SpawnsWith[index]].Id;

                        if (!CheckIfTileAlreadyExists(newTileX + intGidX, newTileY + intGidY, layer))
                        {
                            //intermediateAllTiles.Add(AllTiles[intTilePropertyLayer][newTileX + intGidX, newTileY + intGidY]);
                            intermediateNewTiles.Add(new Tile(newTileX + intGidX, newTileY + intGidY, totalGID + 1, 100, 100, 100, 100) { LayerToDrawAt = intTilePropertyLayer });
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
                        AllTiles[(int)intermediateNewTiles[tileSwapCounter].LayerToDrawAt][(int)intermediateNewTiles[tileSwapCounter].X, (int)intermediateNewTiles[tileSwapCounter].Y] = intermediateNewTiles[tileSwapCounter];
                        //AllTiles[intermediateNewTiles[tileSwapCounter]]
                    }
                }
            }
        }
        //not working
        public List<Tile> GetSpawnWithTiles(Tile baseTile, int xCoord, int yCoord)
        {
            List<Tile> tilesToReturn = new List<Tile>();
            if(baseTile.SpawnsWith != null)
            {
                for(int i=0; i< baseTile.SpawnsWith.Length;i++)
                {
                    string gidX = "";
                    mapName.Tilesets[0].Tiles[baseTile.SpawnsWith[i]].Properties.TryGetValue("relationX", out gidX);
                    string gidY = "";
                    mapName.Tilesets[0].Tiles[baseTile.SpawnsWith[i]].Properties.TryGetValue("relationY", out gidY);
                    string tilePropertyLayer = "";
                    mapName.Tilesets[0].Tiles[baseTile.SpawnsWith[i]].Properties.TryGetValue("layer", out tilePropertyLayer);
                    int intGidX = int.Parse(gidX);
                    int intGidY = int.Parse(gidY);
                    int intTilePropertyLayer = int.Parse(tilePropertyLayer);

                    int totalGID = mapName.Tilesets[0].Tiles[baseTile.SpawnsWith[i]].Id;
                    tilesToReturn.Add(AllTiles[intTilePropertyLayer][xCoord + intGidX, yCoord + intGidY]);
                }
            }
            return tilesToReturn;
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
                                if (mapName.Tilesets[0].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                {
                                    if (mapName.Tilesets[0].Tiles[AllTiles[z][i, j].GID].ObjectGroups.Count > 0)
                                    {


                                        for (int k = 0; k < mapName.Tilesets[0].Tiles[AllTiles[z][i, j].GID].ObjectGroups[0].Objects.Count; k++)
                                        {
                                            TmxObject tempObj = mapName.Tilesets[0].Tiles[AllTiles[z][i, j].GID].ObjectGroups[0].Objects[k];


                                            AllTiles[z][i, j].TileObject = new ObjectBody(graphicsDevice,
                                                new Rectangle(AllTiles[z][i, j].DestinationRectangle.X + (int)Math.Ceiling(tempObj.X),
                                                AllTiles[z][i, j].DestinationRectangle.Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                                                (int)Math.Ceiling(tempObj.Height)), AllTiles[z][i, j].DestinationRectangle.X);

                                            AllTiles[z][i, j].HasObject = true;
                                            Game1.GetCurrentStage().AllObjects.Add(AllTiles[z][i, j].TileObject); // not gonna work for saving, gotta figure out.

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

        #region ADDOBJECTSTOBUILDINGS

        public void AddObjectToBuildingTile(Tile tile, int indexX, int indexY)
        {
            for (int z = 0; z < AllTiles.Count; z++)
            {
                if (mapName.Tilesets[0].Tiles.ContainsKey(AllTiles[z][indexX, indexY].GID))
                {
                    for (int k = 0; k < mapName.Tilesets[0].Tiles[AllTiles[z][indexX, indexY].GID].ObjectGroups[0].Objects.Count; k++)
                    {
                        TmxObject tempObj = mapName.Tilesets[0].Tiles[AllTiles[z][indexX, indexY].GID].ObjectGroups[0].Objects[k];

                        AllTiles[z][indexX, indexY].TileObject = new ObjectBody(graphicsDevice,
                            new Rectangle(AllTiles[z][indexX, indexY].DestinationRectangle.X + (int)Math.Ceiling(tempObj.X),
                            AllTiles[z][indexX, indexY].DestinationRectangle.Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                            (int)Math.Ceiling(tempObj.Height)), AllTiles[z][indexX, indexY].DestinationRectangle.X);
                        AllTiles[z][indexX, indexY].HasObject = true;
                        Game1.Iliad.AllObjects.Add(AllTiles[z][indexX, indexY].TileObject);
                    }
                }
            }
        }
        #endregion

        

        #region UPDATE
        public void Update(GameTime gameTime, MouseManager mouse)
        {
            Game1.myMouseManager.TogglePlantInteraction = false;
            for (int z = 0; z < AllTiles.Count; z++)
            {
                for (var i = 0; i < tilesetTilesWide; i++)
                {
                    for (var j = 0; j < tilesetTilesHigh; j++)
                    {

                        if (AllTiles[z][i, j].GID != 0)
                        {
                            if (AllTiles[z][i, j].IsFinishedAnimating)
                            {
                                Destroy(z,i, j);
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
                                    
                                    if (AllTiles[z][i, j].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle) && mapName.Tilesets[0].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                                    {

                                        Game1.userInterface.DrawTileSelector = true;
                                        Game1.userInterface.TileSelectorX = AllTiles[z][i, j].DestinationRectangle.X;
                                        Game1.userInterface.TileSelectorY = AllTiles[z][i, j].DestinationRectangle.Y;

                                        Game1.myMouseManager.ToggleGeneralInteraction = true;

                                        //if (mapName.Tilesets[0].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("plantable")) //this whole thing is devastated, come back TODO
                                        //{
                                        //    Game1.isMyMouseVisible = false;
                                        //    Game1.userInterface.DrawTileSelector = false;
                                        //    Game1.myMouseManager.TogglePlantInteraction = true;

                                        //}

                                        if (mouse.IsRightClicked)
                                        {
                                            InteractWithBackground(z, gameTime, i, j);

                                        }
                                    }
                                    else
                                    {
                                        Game1.userInterface.DrawTileSelector = false;
                                        Game1.isMyMouseVisible = true;
                                        Game1.myMouseManager.TogglePlantInteraction = false;
                                    }
                                }
                                if (z == 1)
                                {

                                    if (AllTiles[z][i, j].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle)) //&& mapName.Tilesets[0].Tiles.ContainsKey(tiles[i, j].GID not sure what this was for.
                                    {
                                        Game1.isMyMouseVisible = false;
                                        Game1.userInterface.DrawTileSelector = true;
                                        Game1.userInterface.TileSelectorX = AllTiles[z][i, j].DestinationRectangle.X;
                                        Game1.userInterface.TileSelectorY = AllTiles[z][i, j].DestinationRectangle.Y;



                                        Game1.myMouseManager.ToggleGeneralInteraction = true;


                                        if (mouse.IsRightClicked)
                                        {
                                            InteractWithBuilding(z, gameTime, i, j);

                                        }
                                    }
                                    else
                                    {
                                        Game1.isMyMouseVisible = true;
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
        #endregion

        #region DRAW
        public void DrawTiles(SpriteBatch spriteBatch)
        {
            for (int z = 0; z < AllTiles.Count; z++)
            {
                for (var i = 0; i < tilesetTilesWide; i++)
                {
                    for (var j = 0; j < tilesetTilesHigh; j++)
                    {
                        if (AllTiles[z][i, j].GID != 0)
                        {
                            if (AllTiles[z][i, j].DestinationRectangle.Left < Game1.cam.Pos.X + (Game1.ScreenWidth/2 / Game1.cam.Zoom) && AllTiles[z][i, j].DestinationRectangle.Left > Game1.cam.Pos.X - (Game1.ScreenWidth/2 / Game1.cam.Zoom +16)
                                 && AllTiles[z][i, j].DestinationRectangle.Y < Game1.cam.Pos.Y + (Game1.ScreenHeight /2 / Game1.cam.Zoom + 16) && AllTiles[z][i, j].DestinationRectangle.Y > Game1.cam.Pos.Y -( Game1.ScreenHeight /2 / Game1.cam.Zoom + 16))
                            {
                                spriteBatch.Draw(tileSet, AllTiles[z][i, j].DestinationRectangle, AllTiles[z][i, j].SourceRectangle,Game1.GlobalClock.TimeOfDayColor, (float)0, new Vector2(0, 0), SpriteEffects.None, AllDepths[z]);

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
                if (AllTiles[layer][CurrentIndexX, CurrentIndexY] == AllTiles[layer][OldIndexX , OldIndexY ])
                {
                    AllTiles[layer][OldIndexX, OldIndexY].GID = 1;
                }
            }

            Tile ReplaceMenttile = new Tile(AllTiles[layer][oldX, oldY].OldX, AllTiles[layer][oldX, oldY].OldY, GID, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight) { ColorMultiplier = colorMultiplier};

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
            if (Game1.userInterface.BottomBar.GetCurrentEquippedTool() == 3)
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

            if (Game1.userInterface.BottomBar.GetCurrentEquippedTool() == 128)
            {
                if (AllTiles[layer][oldX, oldY].Plantable)
                {

                        //Game1.myMouseManager.TogglePlantInteraction = true;

                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                    ReplaceTileWithNewTile(layer, oldX, oldY, 6076);
                    Game1.Player.Inventory.RemoveItem(128);
                }
            }
        }

        //must be a building tile
        public void InteractWithBuilding(int layer, GameTime gameTime, int oldX, int oldY)
        {

            
            if(AllTiles[layer][oldX, oldY].IsPortal)
            {
                if(AllTiles[layer][oldX,oldY].portalDestination == "lodgeInterior" && Game1.userInterface.BottomBar.GetCurrentEquippedTool() == 5)
                {
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DoorOpenInstance, false, 1);
                    Game1.Player.controls.Direction = Dir.Up;
                    Game1.gameStages = Stages.LodgeInteior;
                    Game1.Player.position.X = 878;
                    Game1.Player.position.Y = 809;
                }
            }
            switch (Game1.userInterface.BottomBar.GetCurrentEquippedTool())
            {
                case 0:
                    if (AllTiles[layer][oldX, oldY].Tree && !AllTiles[layer][oldX, oldY].IsAnimating && !Game1.Player.CurrentAction.IsAnimated)
                    {
                        Game1.SoundManager.PlaySoundEffectFromInt(false, 1, 3, 1f);
                        SpecificInteraction(layer, gameTime, oldX, oldY, 9, 10, 11, 12, .25f);
                        AllTiles[layer][oldX, oldY].IsFinishedAnimating = true;

                        //Needs work
                        for(int b=0; b< GetSpawnWithTiles(AllTiles[layer][oldX, oldY], oldX, oldY).Count; b++)
                        {
                            
                            Tile newTile = new Tile(GetSpawnWithTiles(AllTiles[layer][oldX, oldY],
                                oldX, oldY)[b].X, GetSpawnWithTiles(AllTiles[layer][oldX, oldY], oldX, oldY)[b].Y, 5606, 100, 100, 100, 100);
                            GetSpawnWithTiles(AllTiles[layer][oldX, oldY], oldX, oldY)[b] = newTile;
                        }

                        
                        Game1.GetCurrentStage().ParticleEngine.Color = Color.WhiteSmoke;
                        Game1.GetCurrentStage().ParticleEngine.ActivationTime = 1f;
                        Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(AllTiles[layer][oldX, oldY].DestinationRectangle.X, AllTiles[layer][oldX, oldY].DestinationRectangle.Y);
                    }
                    break;
                case 1:
                    if ((AllTiles[layer][oldX, oldY].Stone && !AllTiles[layer][oldX, oldY].IsAnimating && !Game1.Player.CurrentAction.IsAnimated))
                    {
                        Game1.SoundManager.PlaySoundEffectFromInt(false, 1, 8, 1f);
                        SpecificInteraction(layer, gameTime, oldX, oldY, 5, 6, 7, 8, .25f);
                        Game1.GetCurrentStage().ParticleEngine.ActivationTime = 1f;
                        Game1.GetCurrentStage().ParticleEngine.Color = Color.White;
                        Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(AllTiles[layer][oldX, oldY].DestinationRectangle.X, AllTiles[layer][oldX, oldY].DestinationRectangle.Y);

                    }

                    if (AllTiles[layer][oldX, oldY].RedRuneStone && !AllTiles[layer][oldX, oldY].IsAnimating && !Game1.Player.CurrentAction.IsAnimated)
                    {
                        Game1.SoundManager.StoneSmash.Play();
                        SpecificInteraction(layer, gameTime, oldX, oldY, 5, 6, 7, 8, .25f);
                        GeneralInteraction(3, gameTime, oldX, oldY - 1, .25f);
                        Game1.GetCurrentStage().ParticleEngine.ActivationTime = 1f;
                        Game1.GetCurrentStage().ParticleEngine.Color = Color.Red;
                        Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(AllTiles[layer][oldX, oldY].DestinationRectangle.X, AllTiles[layer][oldX, oldY].DestinationRectangle.Y);
                    }

                    if (AllTiles[layer][oldX, oldY].BlueRuneStone && !AllTiles[layer][oldX, oldY].IsAnimating && !Game1.Player.CurrentAction.IsAnimated)
                    {
                        Game1.SoundManager.StoneSmash.Play();
                        SpecificInteraction(layer, gameTime, oldX, oldY, 5, 6, 7, 8, .25f);
                        GeneralInteraction(3, gameTime, oldX, oldY - 1, .25f);
                        Game1.GetCurrentStage().ParticleEngine.ActivationTime = 1f;
                        Game1.GetCurrentStage().ParticleEngine.Color = Color.Blue;
                        Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(AllTiles[layer][oldX, oldY].DestinationRectangle.X, AllTiles[layer][oldX, oldY].DestinationRectangle.Y);

                    }
                    break;

                case 2:
                    if (AllTiles[layer][oldX, oldY].Grass && !AllTiles[layer][oldX, oldY].IsAnimating && !Game1.Player.CurrentAction.IsAnimated)
                    {
                        Game1.SoundManager.PlaySoundEffectFromInt(false, 1, 6, 1f);
                        SpecificInteraction(layer, gameTime, oldX, oldY, 1, 2, 3, 4, .25f);
                        Game1.GetCurrentStage().ParticleEngine.ActivationTime = 1f;
                        Game1.GetCurrentStage().ParticleEngine.Color = Color.Green;
                        Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(AllTiles[layer][oldX, oldY].DestinationRectangle.X, AllTiles[layer][oldX, oldY].DestinationRectangle.Y);


                    }
                    break;


               // default:
                   // throw new Exception("Item with id " + Game1.userInterface.BottomBar.GetCurrentEquippedTool() + " has not been assigned an action");

            }


        }

        //interact without any player animations
        public void GeneralInteraction(int layer, GameTime gameTime, int oldX, int oldY, float delayTimer = 0f)
        {
            if (delayTimer != 0f)
            {
                AllTiles[layer][oldX, oldY].DelayTimer = delayTimer;
            }
            AllTiles[layer][oldX, oldY].IsAnimating = true;
            AllTiles[layer][oldX, oldY].KillAnimation = true;

        }

        //specify which animations you want to use depending on where the player is in relation to the object
        
        public void SpecificInteraction(int layer, GameTime gameTime, int oldX, int oldY, int down, int right, int left, int up, float delayTimer  = 0f)
        {

            if (delayTimer != 0f)
            {
                AllTiles[layer][oldX, oldY].DelayTimer = delayTimer;
            }
            if(AllTiles[layer][oldX, oldY].IsAnimated)
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
            if(AllTiles[layer][oldX, oldY].IsAnimated)
            {
                if (AllTiles[layer][oldX, oldY].IsFinishedAnimating && AllTiles[layer][oldX, oldY].HasObject)
                {
                    //ObjectBody newObject = new ObjectBody();
                    Game1.GetCurrentStage().AllObjects.Remove(AllTiles[layer][oldX, oldY].TileObject);
                    AllTiles[layer][oldX, oldY].TileObject = null;
                    AllTiles[layer][oldX, oldY].HasObject = false;
                    Game1.GetCurrentStage().AllItems.Add(Game1.ItemVault.GenerateNewItem(AllTiles[layer][oldX, oldY].AssociatedItem, new Vector2(AllTiles[layer][oldX, oldY].DestinationRectangle.X, AllTiles[layer][oldX, oldY].DestinationRectangle.Y), true));
                    ReplaceTilePermanent(layer, oldX, oldY, 0);

                }
            }
            else
            {
                Game1.GetCurrentStage().AllObjects.Remove(AllTiles[layer][oldX, oldY].TileObject);
                AllTiles[layer][oldX, oldY].TileObject = null;
                AllTiles[layer][oldX, oldY].HasObject = false;
                Game1.GetCurrentStage().AllItems.Add(Game1.ItemVault.GenerateNewItem(AllTiles[layer][oldX, oldY].AssociatedItem, new Vector2(AllTiles[layer][oldX, oldY].DestinationRectangle.X, AllTiles[layer][oldX, oldY].DestinationRectangle.Y), true));
                ReplaceTilePermanent(layer, oldX, oldY, 0);
            }
            

        }

    }
        #endregion 
}