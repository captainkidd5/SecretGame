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
        MouseManager myMouse;
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


        #region CONSTRUCTOR

        private TileManager()
        {

        }
        //TODO LayerDepth List
        public TileManager( Texture2D tileSet, TmxMap mapName, List<TmxLayer> allLayers, GraphicsDevice graphicsDevice, ContentManager content, int tileSetNumber, List<float> allDepths)
        {
            this.tileSet = tileSet;
            this.mapName = mapName;
            this.layerName = layerName;

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
            AllTiles = new List<Tile[,]>()
            {
                new Tile[100,100],
                new Tile[100,100],
                new Tile[100,100],
                new Tile[100,100],
                new Tile[100,100]
            };


            Tiles = new Tile[tilesetTilesWide, tilesetTilesHigh];



            for(int i =0; i < AllTiles.Count; i++)
            {
                foreach(TmxLayerTile layerNameTile in AllLayers[i].Tiles)
                {
                    Tile tempTile = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);
                    AllTiles[i][layerNameTile.X, layerNameTile.Y] = tempTile;

                    
                }
            }

            //foreach (TmxLayerTile layerNameTile in layerName.Tiles)
            //{

            //    Tile tempTile = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);

            //    Tiles[layerNameTile.X, layerNameTile.Y] = tempTile;

            //}
            //if (AllLayers[z] == AllLayers[1])
            //{
                for (int g = 0; g < 500; g++)
                {
                    //stone
                    GenerateRandomTiles(1, 6675);
                    //grass
                    GenerateRandomTiles(1, 6475);

                    //redrunestone
                    GenerateRandomTiles(1, 5681);
                }
            //}

            for (int z = 0; z < AllTiles.Count; z++)
            {

                for (int i = 0; i < tilesetTilesWide; i++)
                {
                    for (int j = 0; j < tilesetTilesHigh; j++)
                    {
                        //if foreground
                        if (z == 3)
                        {
                            if (j < 99  && AllTiles[1][i, j + 1].GID == 5680)
                            {
                                AllTiles[3][i, j] = new Tile(i, j, 5581, 100, 100, 100, 100, 0) { IsAnimated = true, Speed = .15f };
                            }
                        }
                        if (AllTiles[z][i, j].GID != 0)
                        {


                            if (mapName.Tilesets[tileSetNumber].Tiles.ContainsKey(AllTiles[z][i, j].GID))
                            {
                                if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("portal"))
                                {
                                    AllTiles[z][i, j].IsPortal = true;
                                    AllTiles[z][i, j].portalDestination = mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["portal"];

                                }
                                if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("plantable"))
                                {///////////
                                    AllTiles[z][i, j].Plantable = true;
                                    AllTiles[z][i, j].TileProperties.Add("plantable");
                                }
                                if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("diggable"))
                                {
                                    AllTiles[z][i, j].TileProperties.Add("diggable");
                                }
                                if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsValue("dirt"))
                                {
                                    AllTiles[z][i, j].TileProperties.Add("dirt");
                                }

                                if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("Probability"))
                                {
                                    AllTiles[z][i, j].Probability = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["Probability"]);
                                }

                                if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("Animated"))
                                {
                                    AllTiles[z][i, j].IsAnimated = true;
                                    AllTiles[z][i, j].TotalFrames = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["Animated"]);
                                    AllTiles[z][i, j].Speed = double.Parse(mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties["Speed"]);


                                    if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("start"))
                                    {

                                        AllTiles[z][i, j].IsAnimating = true;

                                    }
                                    if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("grass"))
                                    {
                                        //tiles[i, j].Properties.Add("grass", true);
                                        AllTiles[z][i, j].TileProperties.Add("grass");
                                        AllTiles[z][i, j].AssociatedItem = 2;
                                    }
                                    if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("stone"))
                                    {
                                        //tiles[i, j].Properties.Add("stone", true);
                                        AllTiles[z][i, j].TileProperties.Add("stone");
                                        AllTiles[z][i, j].AssociatedItem = 7;
                                    }
                                    if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("redRuneStone"))
                                    {
                                        //tiles[i, j].Properties.Add("redRuneStone", true);
                                        AllTiles[z][i, j].TileProperties.Add("redRuneStone");
                                        AllTiles[z][i, j].AssociatedItem = 7;
                                        // Game1.GetCurrentStage().ForeGroundTiles.Tiles[i, j].GID = 5580;

                                    }

                                    else
                                    {
                                        AllTiles[z][i, j].IsAnimating = false;
                                    }

                                    //TODO: Make sounds when the player walks
                                    if (mapName.Tilesets[tileSetNumber].Tiles[AllTiles[z][i, j].GID].Properties.ContainsKey("step"))
                                    {
                                        AllTiles[z][i, j].HasSound = true;
                                    }
                                    else
                                    {
                                        AllTiles[z][i, j].HasSound = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            
            this.JaggedTiles = GetJaggedTiles();
        }
        #endregion
        public bool TileInteraction { get; set; } = false;

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

                                            Game1.Iliad.AllObjects.Add(AllTiles[z][i, j].TileObject);

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

                        Game1.Iliad.AllObjects.Add(AllTiles[z][indexX, indexY].TileObject);
                    }
                }
            }
        }
        #endregion

        

        public void GenerateRandomTiles(int layer, int id)
        {
            int newTileX = Game1.Utility.RNumber(1, 100);
            int newTileY = Game1.Utility.RNumber(1, 100);


            if(!CheckIfTileAlreadyExists(newTileX, newTileY, layer))
            {
                //Tiles[newTileX, newTileY].GID = 6675;
                AllTiles[layer][newTileX, newTileY] = new Tile(newTileX, newTileY, id, 100, 100, 100, 100, 0);

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

                            if (mouse.IsHoveringTile(AllTiles[z][i, j].DestinationRectangle))
                            {

                                CurrentIndexX = i;
                                CurrentIndexY = j;

                                //IE layer is building.
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
                                        //doesn't work because dirt isn't in building layer!
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

                                    AllTiles[z][i, j].Animate(gameTime, AllTiles[z][i, j].TotalFrames, AllTiles[z][i, j].Speed);
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
                            if (AllTiles[z][i, j].DestinationRectangle.Left < Game1.cam.Pos.X + (Game1.ScreenWidth / 2) && AllTiles[z][i, j].DestinationRectangle.Left > Game1.cam.Pos.X - (Game1.ScreenWidth / 2)
                                 && AllTiles[z][i, j].DestinationRectangle.Y < Game1.cam.Pos.Y + (Game1.ScreenHeight / 2) && AllTiles[z][i, j].DestinationRectangle.Y > Game1.cam.Pos.Y - (Game1.ScreenHeight / 2))
                            {
                                spriteBatch.Draw(tileSet, AllTiles[z][i, j].DestinationRectangle, AllTiles[z][i, j].SourceRectangle, AllTiles[z][i, j].TileColor * AllTiles[z][i, j].ColorMultiplier, (float)0, new Vector2(0, 0), SpriteEffects.None, AllDepths[z]);

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
            Tile ReplaceMenttile = new Tile(AllTiles[layer][oldX, oldY].OldX, AllTiles[layer][oldX, oldY].OldY, gid, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);
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

            Tile ReplaceMenttile = new Tile(Tiles[oldX, oldY].OldX, Tiles[oldX, oldY].OldY, GID, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber) { ColorMultiplier = colorMultiplier};

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
            Tile ReplaceMenttile = new Tile(AllTiles[layer][tileToReplaceX, tileToReplaceY].OldX, AllTiles[layer][tileToReplaceX, tileToReplaceY].OldY, newTileGID, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);
            AllTiles[layer][tileToReplaceX, tileToReplaceY] = ReplaceMenttile;
        }
        #endregion

        #region INTERACTIONS

        //Used for interactions with background tiles only
        public void InteractWithBackground(int layer, GameTime gameTime, int oldX, int oldY)
        {
            if (Game1.userInterface.BottomBar.GetCurrentEquippedTool() == 6)
            {     
                if (AllTiles[layer][oldX, oldY].TileProperties.Contains("diggable"))
                {
                  if (AllTiles[layer][oldX, oldY].TileProperties.Contains("dirt"))
                  {
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                    ReplaceTileWithNewTile(layer, oldX, oldY, 6074);
                        AllTiles[layer][oldX, oldY].TileProperties.Add("plantable");
                  }

                }
            }

            if (Game1.userInterface.BottomBar.GetCurrentEquippedTool() == 9)
            {
                if (AllTiles[layer][oldX, oldY].TileProperties.Contains("plantable"))
                {

                        //Game1.myMouseManager.TogglePlantInteraction = true;

                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                    ReplaceTileWithNewTile(layer, oldX, oldY, 6076);
                    Game1.Player.Inventory.RemoveItem(9);
                }
            }
        }

        //must be a building tile
        public void InteractWithBuilding(int layer, GameTime gameTime, int oldX, int oldY)
        {
          //  if(mapName.Tilesets[0].Tiles.ContainsKey(tiles[oldX, oldY].GID)
               // {

            
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

            if (Game1.userInterface.BottomBar.GetCurrentEquippedTool() == 8)
            {
                if ((AllTiles[layer][oldX, oldY].TileProperties.Contains("stone") && !AllTiles[layer][oldX, oldY].IsAnimating && !Game1.Player.CurrentAction.IsAnimating))
                {
                    //SpecificInteraction(gameTime, oldX, oldY, "CutGrassDown", "CutGrassRight", "CutGrassLeft", "CutGrassUp");
                    SpecificInteraction(layer, gameTime, oldX, oldY, "MiningDown", "MiningRight", "MiningLeft", "MiningUp", .25f);
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.StoneSmashInstance, false, 1);
                }

                if(AllTiles[layer][oldX, oldY].TileProperties.Contains("redRuneStone") && !AllTiles[layer][oldX, oldY].IsAnimating && !Game1.Player.CurrentAction.IsAnimating)
                {
                    SpecificInteraction(layer, gameTime, oldX, oldY, "MiningDown", "MiningRight", "MiningLeft", "MiningUp", .25f);
                    GeneralInteraction(3, gameTime, oldX, oldY - 1, .25f);
                    //ReplaceTilePermanent(3, oldX, oldY - 1, 0);
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.StoneSmashInstance, false, 1);
                }
            }


            if (Game1.userInterface.BottomBar.GetCurrentEquippedTool() == 4)
            {

                if (AllTiles[layer][oldX,oldY].TileProperties.Contains("grass") && !AllTiles[layer][oldX, oldY].IsAnimating && !Game1.Player.CurrentAction.IsAnimating)
                {
                    SpecificInteraction(layer, gameTime, oldX, oldY, "CutGrassDown", "CutGrassRight", "CutGrassLeft", "CutGrassUp", .25f);
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.GrassBreakInstance, false, 1);

                }
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



        public void SpecificInteraction(int layer, GameTime gameTime, int oldX, int oldY, string down, string right, string left, string up, float delayTimer  = 0f)
        {
            //if(tiles[oldX,oldY].AssociatedTiles.Count > 0)
            //{
            //    for(int i = 0; i <= tiles[oldX, oldY].AssociatedTiles.Count; i++)
            //    {

            //    }
            //}
            if (delayTimer != 0f)
            {
                AllTiles[layer][oldX, oldY].DelayTimer = delayTimer;
            }
            AllTiles[layer][oldX, oldY].IsAnimating = true;
            AllTiles[layer][oldX, oldY].KillAnimation = true;

            if (Game1.Player.Position.Y < AllTiles[layer][oldX, oldY].Y - 30)
            {
                Game1.Player.controls.Direction = Dir.Down;
            }

            else if (Game1.Player.Position.Y > AllTiles[layer][oldX, oldY].Y)
            {
                Game1.Player.controls.Direction = Dir.Up;
            }

            else if (Game1.Player.Position.X < AllTiles[layer][oldX, oldY].X)
            {
                Game1.Player.controls.Direction = Dir.Right;
            }
            else if (Game1.Player.Position.X > AllTiles[layer][oldX, oldY].X)
            {
                Game1.Player.controls.Direction = Dir.Left;
            }




            //Game1.Player.IsMoving = false;
            if (Game1.Player.controls.Direction == Dir.Down)
            {
                Game1.Player.PlayAnimation(gameTime, down);
            }
            else if (Game1.Player.controls.Direction == Dir.Right)
            {
                Game1.Player.PlayAnimation(gameTime, right);
            }
            else if (Game1.Player.controls.Direction == Dir.Left)
            {
                Game1.Player.PlayAnimation(gameTime, left);
            }
            else if (Game1.Player.controls.Direction == Dir.Up)
            {
                Game1.Player.PlayAnimation(gameTime, up);
            }
        }

        #endregion

        #region DESTROYTILES



        public void Destroy(int layer, int oldX, int oldY)
        {
            if (AllTiles[layer][oldX, oldY].IsFinishedAnimating)
            {
                if(AllTiles[layer][oldX, oldY].TileProperties.Count > 0)
                {
                    Game1.GetCurrentStage().AllObjects.Remove(AllTiles[layer][oldX, oldY].TileObject);
                    Game1.GetCurrentStage().AllItems.Add(Game1.ItemVault.GenerateNewItem(AllTiles[layer][oldX, oldY].AssociatedItem, new Vector2(AllTiles[layer][oldX, oldY].DestinationRectangle.X, AllTiles[layer][oldX, oldY].DestinationRectangle.Y), true));
                    ReplaceTilePermanent(layer, oldX, oldY, 0);
                }
            }

        }

        public Tile[][] GetJaggedTiles()
        {
            Tile[][] jaggedArray = new Tile[100][];
            for(int i =0; i < Tiles.GetLength(0); i++)
            {
                jaggedArray[i] = new Tile[Tiles.GetLength(1)];
                for(int j=0; j< Tiles.GetLength(1); j++)
                {
                    jaggedArray[i][j] = Tiles[i, j];
                }
            }
            return jaggedArray;
        }

        public Tile[,] GetDimensionalTiles(Tile[][] jaggedTiles)
        {
            Tile[,] dimensionalTiles = new Tile[100, 100];
            for(int i=0; i<jaggedTiles.Length; i++)
            {
                for(int j=0; j<jaggedTiles[0].Length; j++)
                {
                    dimensionalTiles[i, j] = jaggedTiles[i][j];
                }
            }
            return dimensionalTiles;
        }

                        
   
    }
        #endregion

    
}