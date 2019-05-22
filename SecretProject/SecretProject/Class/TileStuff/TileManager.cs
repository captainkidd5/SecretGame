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

        public bool IsBuilding { get; set; } = false;
        public bool isBackground { get; set; } = false;
        public bool IsMidground { get; set; } = false;
        public bool IsForeGround { get; set; } = false;


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


        #region CONSTRUCTOR

        private TileManager()
        {

        }

        public TileManager( Texture2D tileSet, TmxMap mapName, TmxLayer layerName, GraphicsDevice graphicsDevice, ContentManager content, bool isBuilding, int tileSetNumber, float depth)
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

            Tiles = new Tile[tilesetTilesWide, tilesetTilesHigh];

            this.IsBuilding = isBuilding;

            this.Depth = depth;

            foreach (TmxLayerTile layerNameTile in layerName.Tiles)
            {
                Tile tempTile = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);

                Tiles[layerNameTile.X, layerNameTile.Y] = tempTile;

            }

            if(this.IsBuilding)
            {
                for (int i = 0; i < 100; i++)
                {
                    //stone
                    GenerateRandomTiles(6675);
                    GenerateRandomTiles(6475);
                }
            }
            

            for (var i = 0; i < tilesetTilesWide; i++)
            {
                for (var j = 0; j < tilesetTilesHigh; j++)
                {
                    if (Tiles[i, j].GID != 0)
                    {
                        

                        if (mapName.Tilesets[tileSetNumber].Tiles.ContainsKey(Tiles[i, j].GID))
                        {
                            if (mapName.Tilesets[tileSetNumber].Tiles[Tiles[i, j].GID].Properties.ContainsKey("portal"))
                            {
                                Tiles[i, j].IsPortal = true;
                                Tiles[i, j].portalDestination = mapName.Tilesets[tileSetNumber].Tiles[Tiles[i, j].GID].Properties["portal"];

                            }
                            if (mapName.Tilesets[tileSetNumber].Tiles[Tiles[i, j].GID].Properties.ContainsKey("plantable"))
                            {///////////
                                Tiles[i, j].Plantable = true;
                                Tiles[i, j].TileProperties.Add("plantable");
                            }
                            if (mapName.Tilesets[tileSetNumber].Tiles[Tiles[i, j].GID].Properties.ContainsKey("diggable"))
                            {
                                Tiles[i, j].TileProperties.Add("diggable");
                            }
                            if (mapName.Tilesets[tileSetNumber].Tiles[Tiles[i, j].GID].Properties.ContainsValue("dirt"))
                            {
                                Tiles[i, j].TileProperties.Add("dirt");
                            }

                            if (mapName.Tilesets[tileSetNumber].Tiles[Tiles[i, j].GID].Properties.ContainsKey("Probability"))
                                {
                                Tiles[i, j].Probability = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[Tiles[i, j].GID].Properties["Probability"]);
                                 }

                                if (mapName.Tilesets[tileSetNumber].Tiles[Tiles[i, j].GID].Properties.ContainsKey("Animated"))
                                {
                                Tiles[i, j].IsAnimated = true;
                                Tiles[i, j].TotalFrames = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[Tiles[i, j].GID].Properties["Animated"]);
                                Tiles[i, j].Speed = double.Parse(mapName.Tilesets[tileSetNumber].Tiles[Tiles[i, j].GID].Properties["Speed"]);


                                if (mapName.Tilesets[tileSetNumber].Tiles[Tiles[i, j].GID].Properties.ContainsKey("start"))
                                {

                                    Tiles[i, j].IsAnimating = true;

                                }
                                if(mapName.Tilesets[tileSetNumber].Tiles[Tiles[i, j].GID].Properties.ContainsKey("grass"))
                                {
                                    //tiles[i, j].Properties.Add("grass", true);
                                    Tiles[i, j].TileProperties.Add("grass");
                                    Tiles[i, j].AssociatedItem = 2;
                                }
                                if (mapName.Tilesets[tileSetNumber].Tiles[Tiles[i, j].GID].Properties.ContainsKey("stone"))
                                {
                                    //tiles[i, j].Properties.Add("stone", true);
                                    Tiles[i, j].TileProperties.Add("stone");
                                    Tiles[i, j].AssociatedItem = 7;
                                }
                                if (mapName.Tilesets[tileSetNumber].Tiles[Tiles[i, j].GID].Properties.ContainsKey("redRuneStone"))
                                {
                                    //tiles[i, j].Properties.Add("redRuneStone", true);
                                    Tiles[i, j].TileProperties.Add("redRuneStone");
                                    Tiles[i, j].AssociatedItem = 7;

                                    //-1 means the tile above the current one
                                    //tiles[i, j].AssociatedTiles.Add = tiles[i, j - 10];

                                }
                                
                                else
                                {
                                    Tiles[i, j].IsAnimating = false;
                                }

                                //TODO: Make sounds when the player walks
                                if (mapName.Tilesets[tileSetNumber].Tiles[Tiles[i, j].GID].Properties.ContainsKey("step"))
                                {
                                    Tiles[i, j].HasSound = true;
                                }
                                else
                                {
                                    Tiles[i, j].HasSound = false;
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
        public void LoadInitialTileObjects(Home stage)
        {
            for (var i = 0; i < tilesetTilesWide; i++)
            {
                for (var j = 0; j < tilesetTilesHigh; j++)
                {
                    if (Tiles[i, j].GID != 0)
                    {
                        if (mapName.Tilesets[0].Tiles.ContainsKey(Tiles[i, j].GID))
                        {
                            if (mapName.Tilesets[0].Tiles[Tiles[i, j].GID].ObjectGroups.Count > 0)
                            {


                                for (int k = 0; k < mapName.Tilesets[0].Tiles[Tiles[i, j].GID].ObjectGroups[0].Objects.Count; k++)
                                {
                                    TmxObject tempObj = mapName.Tilesets[0].Tiles[Tiles[i, j].GID].ObjectGroups[0].Objects[k];

                                    Tiles[i, j].TileObject = new ObjectBody(graphicsDevice,
                                        new Rectangle(Tiles[i, j].DestinationRectangle.X + (int)Math.Ceiling(tempObj.X),
                                        Tiles[i, j].DestinationRectangle.Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                                        (int)Math.Ceiling(tempObj.Height)), Tiles[i, j].DestinationRectangle.X);

                                    Game1.Iliad.AllObjects.Add(Tiles[i, j].TileObject);

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
            if (mapName.Tilesets[0].Tiles.ContainsKey(Tiles[indexX, indexY].GID))
            {


                for (int k = 0; k < mapName.Tilesets[0].Tiles[Tiles[indexX, indexY].GID].ObjectGroups[0].Objects.Count; k++)
                {
                    TmxObject tempObj = mapName.Tilesets[0].Tiles[Tiles[indexX, indexY].GID].ObjectGroups[0].Objects[k];

                    Tiles[indexX, indexY].TileObject = new ObjectBody(graphicsDevice,
                        new Rectangle(Tiles[indexX, indexY].DestinationRectangle.X + (int)Math.Ceiling(tempObj.X),
                        Tiles[indexX, indexY].DestinationRectangle.Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                        (int)Math.Ceiling(tempObj.Height)), Tiles[indexX, indexY].DestinationRectangle.X);

                    Game1.Iliad.AllObjects.Add(Tiles[indexX, indexY].TileObject);
                }
            }
        }
        #endregion

        

        public void GenerateRandomTiles(int id)
        {
            int newTileX = Game1.Utility.RNumber(1, 100);
            int newTileY = Game1.Utility.RNumber(1, 100);


            if(CheckIfTileAlreadyExists(newTileX, newTileY))
            {
                //Tiles[newTileX, newTileY].GID = 6675;
                Tiles[newTileX, newTileY] = new Tile(newTileX, newTileY, id, 100, 100, 100, 100, 0);

            }
            

        }

        public bool CheckIfTileAlreadyExists(int tileX, int tileY)
        {
            if (Tiles[tileX, tileY].GID != 0)
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
            for (var i = 0; i < tilesetTilesWide; i++)
            {
                for (var j = 0; j < tilesetTilesHigh; j++)
                {

                    if (Tiles[i, j].GID != 0)
                    {
                        if(Tiles[i,j].IsFinishedAnimating)
                        {
                            Destroy(i, j);
                            Tiles[i, j].IsFinishedAnimating = false;
                        }

                        if (mouse.IsHoveringTile(Tiles[i, j].DestinationRectangle))
                        {
                            
                            CurrentIndexX = i;
                            CurrentIndexY = j;

                            if(isBackground)
                            {
                                if (Tiles[i, j].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle) && mapName.Tilesets[0].Tiles.ContainsKey(Tiles[i, j].GID))
                                {

                                    Game1.userInterface.DrawTileSelector = true;
                                    Game1.userInterface.TileSelectorX = Tiles[i, j].DestinationRectangle.X;
                                    Game1.userInterface.TileSelectorY = Tiles[i, j].DestinationRectangle.Y;

                                    if(mapName.Tilesets[0].Tiles[Tiles[i, j].GID].Properties.ContainsKey("plantable")) //this whole thing is devastated, come back TODO
                                        {
                                        Game1.isMyMouseVisible = false;
                                        Game1.userInterface.DrawTileSelector = false;
                                        Game1.myMouseManager.TogglePlantInteraction = true;

                                        }

                                    if (mouse.IsRightClicked)
                                    {
                                        InteractWithBackground(gameTime, i, j);

                                    }
                                }
                                else
                                {
                                    Game1.userInterface.DrawTileSelector = false;
                                    Game1.isMyMouseVisible = true;
                                    Game1.myMouseManager.TogglePlantInteraction = false;
                                }
                            }

                            if (IsBuilding)
                            {
                                
                                if (Tiles[i, j].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle)) //&& mapName.Tilesets[0].Tiles.ContainsKey(tiles[i, j].GID not sure what this was for.
                                {
                                    Game1.isMyMouseVisible = false;
                                    //doesn't work because dirt isn't in building layer!


                                    
        
                                        Game1.myMouseManager.ToggleGeneralInteraction = true;
                                                                            

                                    if (mouse.IsRightClicked)
                                    {
                                        InteractWithBuilding(gameTime, i, j);

                                    }
                                }
                                else
                                {
                                    Game1.isMyMouseVisible = true;
                                }         
                            }                
                        }

                        if (Tiles[i, j].IsAnimated)
                        {
                            if (Tiles[i, j].IsAnimating == true && Tiles[i,j].IsFinishedAnimating == false)
                            {

                                Tiles[i, j].Animate(gameTime, Tiles[i, j].TotalFrames, Tiles[i, j].Speed);
                            }
                        }

                        if(Tiles[i,j].IsTemporary)
                        {
                            Tiles[i, j].GID = 1;
                        }
                    }
                    
                }
            }
        }

        #endregion

        #region DRAW
        public void DrawTiles(SpriteBatch spriteBatch)
        {           
            for (var i = 0; i < tilesetTilesWide; i++)
            {
                for(var j = 0; j < tilesetTilesHigh; j++)
                {                   
                    if (Tiles[i, j].GID != 0)
                    {
                        if(Tiles[i,j].DestinationRectangle.Left < Game1.cam.Pos.X + (Game1.ScreenWidth/2) && Tiles[i, j].DestinationRectangle.Left > Game1.cam.Pos.X - (Game1.ScreenWidth/2)
                             && Tiles[i, j].DestinationRectangle.Y < Game1.cam.Pos.Y + (Game1.ScreenHeight /2) && Tiles[i, j].DestinationRectangle.Y > Game1.cam.Pos.Y - (Game1.ScreenHeight /2))
                        {
                            spriteBatch.Draw(tileSet, Tiles[i, j].DestinationRectangle, Tiles[i, j].SourceRectangle, Tiles[i, j].TileColor * Tiles[i, j].ColorMultiplier, (float)0, new Vector2(0, 0), SpriteEffects.None, Depth);
                            
                        }
                    }                                            
                }
            }
            
        }
        #endregion

        #region REPLACETILES

        
        public void ReplaceTilePermanent(int oldX, int oldY)
        {
            Tile ReplaceMenttile = new Tile(Tiles[oldX, oldY].OldX, Tiles[oldX, oldY].OldY, 0, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);
            Tiles[oldX, oldY] = ReplaceMenttile;
        }

        public void ReplaceTileTemporary(int oldX, int oldY, int GID, float colorMultiplier, int xArrayLength, int yArrayLength)
        {
            if (TempTile != null)
            {
                if (Tiles[CurrentIndexX, CurrentIndexY] == Tiles[OldIndexX , OldIndexY ])
                {
                    Tiles[OldIndexX, OldIndexY].GID = 1;
                }
            }

            Tile ReplaceMenttile = new Tile(Tiles[oldX, oldY].OldX, Tiles[oldX, oldY].OldY, GID, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber) { ColorMultiplier = colorMultiplier};

            TempTile = Tiles[oldX, oldY];
            
            Tiles[oldX, oldY] = ReplaceMenttile;
             // tiles[oldX, oldY].IsTemporary = true;

            OldIndexX = oldX;
            OldIndexY = oldY;

          //  AddTemporaryTiles(TempTile);
        }

        //Basic Replacement.
        public void ReplaceTileWithNewTile(int tileToReplaceX, int tileToReplaceY, int newTileGID)
        {
            Tile ReplaceMenttile = new Tile(Tiles[tileToReplaceX, tileToReplaceY].OldX, Tiles[tileToReplaceX, tileToReplaceY].OldY, newTileGID, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);
            Tiles[tileToReplaceX, tileToReplaceY] = ReplaceMenttile;
        }
        #endregion

        #region INTERACTIONS

        //Used for interactions with background tiles only
        public void InteractWithBackground(GameTime gameTime, int oldX, int oldY)
        {
            if (Game1.userInterface.BottomBar.GetCurrentEquippedTool() == 6)
            {     
                if (Tiles[oldX, oldY].TileProperties.Contains("diggable"))
                {
                  if (Tiles[oldX, oldY].TileProperties.Contains("dirt"))
                  {
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                    ReplaceTileWithNewTile(oldX, oldY, 6074);
                        Tiles[oldX, oldY].TileProperties.Add("plantable");
                  }

                }
            }

            if (Game1.userInterface.BottomBar.GetCurrentEquippedTool() == 9)
            {
                if (Tiles[oldX, oldY].TileProperties.Contains("plantable"))
                {

                        //Game1.myMouseManager.TogglePlantInteraction = true;

                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                    ReplaceTileWithNewTile(oldX, oldY, 6076);
                    Game1.Player.Inventory.RemoveItem(9);
                }
            }
        }

        //must be a building tile
        public void InteractWithBuilding(GameTime gameTime, int oldX, int oldY)
        {
          //  if(mapName.Tilesets[0].Tiles.ContainsKey(tiles[oldX, oldY].GID)
               // {

            
            if(Tiles[oldX, oldY].IsPortal)
            {
                if(Tiles[oldX,oldY].portalDestination == "lodgeInterior" && Game1.userInterface.BottomBar.GetCurrentEquippedTool() == 5)
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
                if ((Tiles[oldX, oldY].TileProperties.Contains("stone") || Tiles[oldX, oldY].TileProperties.Contains("redRuneStone")) && !Tiles[oldX, oldY].IsAnimating && !Game1.Player.CurrentAction.IsAnimating)
                {
                    //SpecificInteraction(gameTime, oldX, oldY, "CutGrassDown", "CutGrassRight", "CutGrassLeft", "CutGrassUp");
                    SpecificInteraction(gameTime, oldX, oldY, "MiningDown", "MiningRight", "MiningLeft", "MiningUp", .25f);
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.StoneSmashInstance, false, 1);
                }
            }


            if (Game1.userInterface.BottomBar.GetCurrentEquippedTool() == 4)
            {

                if (Tiles[oldX,oldY].TileProperties.Contains("grass") && !Tiles[oldX, oldY].IsAnimating && !Game1.Player.CurrentAction.IsAnimating)
                {
                    SpecificInteraction(gameTime, oldX, oldY, "CutGrassDown", "CutGrassRight", "CutGrassLeft", "CutGrassUp", .25f);
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.GrassBreakInstance, false, 1);

                }
            }
             
        }

        //interact without any player animations
        public void GeneralInteraction(GameTime gameTime, int oldX, int oldY)
        {
            Tiles[oldX, oldY].IsAnimating = true;
            Tiles[oldX, oldY].KillAnimation = true;

        }

        //specify which animations you want to use depending on where the player is in relation to the object



        public void SpecificInteraction(GameTime gameTime, int oldX, int oldY, string down, string right, string left, string up, float delayTimer  = 0f)
        {
            //if(tiles[oldX,oldY].AssociatedTiles.Count > 0)
            //{
            //    for(int i = 0; i <= tiles[oldX, oldY].AssociatedTiles.Count; i++)
            //    {

            //    }
            //}
            if (delayTimer != 0f)
            {
                Tiles[oldX, oldY].DelayTimer = delayTimer;
            }
            Tiles[oldX, oldY].IsAnimating = true;
            Tiles[oldX, oldY].KillAnimation = true;

            if (Game1.Player.Position.Y < Tiles[oldX, oldY].Y - 30)
            {
                Game1.Player.controls.Direction = Dir.Down;
            }

            else if (Game1.Player.Position.Y > Tiles[oldX, oldY].Y)
            {
                Game1.Player.controls.Direction = Dir.Up;
            }

            else if (Game1.Player.Position.X < Tiles[oldX, oldY].X)
            {
                Game1.Player.controls.Direction = Dir.Right;
            }
            else if (Game1.Player.Position.X > Tiles[oldX, oldY].X)
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



        public void Destroy(int oldX, int oldY)
        {
            if (Tiles[oldX, oldY].IsFinishedAnimating)
            {
                if(Tiles[oldX, oldY].TileProperties.Count > 0)
                {
                    Game1.GetCurrentStage().AllObjects.Remove(Tiles[oldX, oldY].TileObject);
                    Game1.GetCurrentStage().AllItems.Add(Game1.ItemVault.GenerateNewItem(Tiles[oldX, oldY].AssociatedItem, new Vector2(Tiles[oldX, oldY].DestinationRectangle.X, Tiles[oldX, oldY].DestinationRectangle.Y), true));
                    ReplaceTilePermanent(oldX, oldY);
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