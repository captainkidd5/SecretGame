using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Stage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

using SecretProject.Class.ObjectFolder;
using SecretProject.Class.Controls;
using SecretProject.Class.Universal;
using SecretProject.Class.ItemStuff;
using Microsoft.Xna.Framework.Content;
using SecretProject.Class.Playable;

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
        protected int tilesetTilesWide;
        protected int tilesetTilesHigh;
        protected int mapWidth;
        protected int mapHeight;
        //--------------------------------------
        //Tile Specificications
        protected int iD;
        protected int tileWidth;
        protected int tileHeight;
        protected int tileNumber;


        //--------------------------------------
        //Counting
        protected int tileCounter;


        //--------------------------------------
        //2D Array of All Tiles
        protected Tile[,] tiles;
        public Tile[,] Tiles { get { return tiles; } }

        public bool IsBuilding { get; set; } = false;
        public bool isBackground { get; set; } = false;
        public bool IsMidground { get; set; } = false;
        public bool IsForeGround { get; set; } = false;


        public bool isActive = false;
        public bool isPlacement { get; set; } = false;

        public bool isInClickingRangeOfPlayer = false;


        MouseManager myMouse;
        ContentManager content;
        GraphicsDevice graphicsDevice;

        public int ReplaceTileGid { get; set; }

        List<TmxObject> tileObjects;

        public int CurrentIndexX { get; set; }
        public int CurrentIndexY { get; set; }

        public Tile TempTile { get; set; }

        public int OldIndexX { get; set; }
        public int OldIndexY { get; set; }


        #region CONSTRUCTOR

        public TileManager( Texture2D tileSet, TmxMap mapName, TmxLayer layerName, GraphicsDevice graphicsDevice, ContentManager content, bool isBuilding, int tileSetNumber)
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

            tiles = new Tile[tilesetTilesWide, tilesetTilesHigh];


            this.IsBuilding = isBuilding;

            foreach (TmxLayerTile layerNameTile in layerName.Tiles)
            {
                Tile tempTile = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);

                tiles[layerNameTile.X, layerNameTile.Y] = tempTile;

            }



            for (var i = 0; i < tilesetTilesWide; i++)
            {
                for (var j = 0; j < tilesetTilesHigh; j++)
                {
                    if (tiles[i, j].GID != 0)
                    {
                        if (mapName.Tilesets[tileSetNumber].Tiles.ContainsKey(tiles[i, j].GID))
                        {
                            if (mapName.Tilesets[tileSetNumber].Tiles[tiles[i, j].GID].Properties.ContainsKey("portal"))
                            {
                                tiles[i, j].IsPortal = true;
                                tiles[i, j].portalDestination = mapName.Tilesets[tileSetNumber].Tiles[tiles[i, j].GID].Properties["portal"];

                            }

                            if(mapName.Tilesets[tileSetNumber].Tiles[tiles[i, j].GID].Properties.ContainsKey("Probability"))
                            {
                                tiles[i, j].Probability = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[tiles[i, j].GID].Properties["Probability"]);
                            }

                                if (mapName.Tilesets[tileSetNumber].Tiles[tiles[i, j].GID].Properties.ContainsKey("Animated"))
                            {
                                tiles[i, j].IsAnimated = true;
                                tiles[i, j].TotalFrames = int.Parse(mapName.Tilesets[tileSetNumber].Tiles[tiles[i, j].GID].Properties["Animated"]);
                                tiles[i, j].Speed = double.Parse(mapName.Tilesets[tileSetNumber].Tiles[tiles[i, j].GID].Properties["Speed"]);


                                if (mapName.Tilesets[tileSetNumber].Tiles[tiles[i, j].GID].Properties.ContainsKey("start"))
                                {

                                    tiles[i, j].IsAnimating = true;

                                }
                                else
                                {
                                    tiles[i, j].IsAnimating = false;
                                }

                                if (mapName.Tilesets[tileSetNumber].Tiles[tiles[i, j].GID].Properties.ContainsKey("step"))
                                {
                                    //  game.Exit();
                                    tiles[i, j].HasSound = true;

                                }
                                else
                                {
                                    tiles[i, j].HasSound = false;
                                }
                            }
                        }
                    }
                }
            }                            
        }
        #endregion
        public bool TileInteraction { get; set; } = false;

        #region LOADTILESOBJECTS
        public void LoadInitialTileObjects()
        {
            for (var i = 0; i < tilesetTilesWide; i++)
            {
                for (var j = 0; j < tilesetTilesHigh; j++)
                {
                    if (tiles[i, j].GID != 0)
                    {
                        if (mapName.Tilesets[0].Tiles.ContainsKey(tiles[i, j].GID))
                        {
                            if (mapName.Tilesets[0].Tiles[tiles[i, j].GID].ObjectGroups.Count > 0)
                            {


                                for (int k = 0; k < mapName.Tilesets[0].Tiles[tiles[i, j].GID].ObjectGroups[0].Objects.Count; k++)
                                {
                                    TmxObject tempObj = mapName.Tilesets[0].Tiles[tiles[i, j].GID].ObjectGroups[0].Objects[k];

                                    tiles[i, j].TileObject = new ObjectBody(graphicsDevice,
                                        new Rectangle(tiles[i, j].DestinationRectangle.X + (int)Math.Ceiling(tempObj.X),
                                        tiles[i, j].DestinationRectangle.Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                                        (int)Math.Ceiling(tempObj.Height)), tiles[i, j].DestinationRectangle.X);

                                    Game1.Iliad.allObjects.Add(tiles[i, j].TileObject);

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
            if (mapName.Tilesets[0].Tiles.ContainsKey(tiles[indexX, indexY].GID))
            {


                for (int k = 0; k < mapName.Tilesets[0].Tiles[tiles[indexX, indexY].GID].ObjectGroups[0].Objects.Count; k++)
                {
                    TmxObject tempObj = mapName.Tilesets[0].Tiles[tiles[indexX, indexY].GID].ObjectGroups[0].Objects[k];

                    tiles[indexX, indexY].TileObject = new ObjectBody(graphicsDevice,
                        new Rectangle(tiles[indexX, indexY].DestinationRectangle.X + (int)Math.Ceiling(tempObj.X),
                        tiles[indexX, indexY].DestinationRectangle.Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                        (int)Math.Ceiling(tempObj.Height)), tiles[indexX, indexY].DestinationRectangle.X);

                    Game1.Iliad.allObjects.Add(tiles[indexX, indexY].TileObject);
                }
            }
        }
        #endregion


        public void GenerateRandomTiles(int tileX, int tileY)
        {
          //  if tiles[tileX, tileY].
        }


        #region UPDATE
        public void Update(GameTime gameTime, MouseManager mouse)
        {
            
            for (var i = 0; i < tilesetTilesWide; i++)
            {
                for (var j = 0; j < tilesetTilesHigh; j++)
                {

                    if (tiles[i, j].GID != 0)
                    {
                        if(tiles[i,j].IsFinishedAnimating)
                        {
                            Destroy(i, j);
                            tiles[i, j].IsFinishedAnimating = false;
                        }

                        if (mouse.IsHoveringTile(tiles[i, j].DestinationRectangle))
                        {
                            
                            CurrentIndexX = i;
                            CurrentIndexY = j;
                            // IsBeingSelected(i, j);
                            // if(isActive)
                            //{

                            if(isBackground)
                            {
                                if (tiles[i, j].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle) && mapName.Tilesets[0].Tiles.ContainsKey(tiles[i, j].GID))
                                {
                                    Game1.userInterface.DrawTileSelector = true;
                                    Game1.userInterface.TileSelectorX = tiles[i, j].DestinationRectangle.X;
                                    Game1.userInterface.TileSelectorY = tiles[i, j].DestinationRectangle.Y;
                                    if (mouse.IsRightClicked)
                                    {
                                        InteractWithBackground(gameTime, i, j);

                                    }
                                }
                                else
                                {
                                    Game1.userInterface.DrawTileSelector = false;
                                }
                            }

                            if (IsBuilding)
                            {
                                
                                if (tiles[i, j].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle) && mapName.Tilesets[0].Tiles.ContainsKey(tiles[i, j].GID))
                                {
                                    Game1.isMyMouseVisible = false;
                                    Game1.myMouseManager.ToggleNewMouseMode = true;

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

                            //}                    
                        }

                        

                        if (tiles[i, j].IsAnimated)
                        {
                            if (tiles[i, j].IsAnimating == true && tiles[i,j].IsFinishedAnimating == false)
                            {


                                tiles[i, j].Animate(gameTime, tiles[i, j].TotalFrames, tiles[i, j].Speed);
                            }
                        }

                        if(tiles[i,j].IsTemporary)
                        {
                            tiles[i, j].GID = 1;
                        }
                    }
                    
                }
            }
        }

        #endregion

        #region DRAW
        public void DrawTiles(SpriteBatch spriteBatch, float depth)
        {           
            for (var i = 0; i < tilesetTilesWide; i++)
            {
                for(var j = 0; j < tilesetTilesHigh; j++)
                {                   
                    if (tiles[i, j].GID != 0)
                    {
                        if(tiles[i,j].DestinationRectangle.Left < Game1.cam.Pos.X + (Game1.ScreenWidth/2) && tiles[i, j].DestinationRectangle.Left > Game1.cam.Pos.X - (Game1.ScreenWidth/2)
                             && tiles[i, j].DestinationRectangle.Y < Game1.cam.Pos.Y + (Game1.ScreenHeight /2) && tiles[i, j].DestinationRectangle.Y > Game1.cam.Pos.Y - (Game1.ScreenHeight /2))
                        {
                            spriteBatch.Draw(tileSet, tiles[i, j].DestinationRectangle, tiles[i, j].SourceRectangle, tiles[i, j].TileColor * tiles[i, j].ColorMultiplier, (float)0, new Vector2(0, 0), SpriteEffects.None, depth);
                            
                        }
                    }                                            
                }
            }
            
        }
        #endregion

        #region REPLACETILES

        
        public void ReplaceTilePermanent(int oldX, int oldY)
        {
            Tile ReplaceMenttile = new Tile(tiles[oldX, oldY].OldX, tiles[oldX, oldY].OldY, 0, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);
            tiles[oldX, oldY] = ReplaceMenttile;
        }



        public void ReplaceTileTemporary(int oldX, int oldY, int GID, float colorMultiplier, int xArrayLength, int yArrayLength)
        {
            if (TempTile != null)
            {
                if (Tiles[CurrentIndexX, CurrentIndexY] == Tiles[OldIndexX , OldIndexY ])
                {
                    tiles[OldIndexX, OldIndexY].GID = 1;
                }
            }

            Tile ReplaceMenttile = new Tile(tiles[oldX, oldY].OldX, tiles[oldX, oldY].OldY, GID, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber) { ColorMultiplier = colorMultiplier};

            TempTile = tiles[oldX, oldY];
            
            tiles[oldX, oldY] = ReplaceMenttile;
             // tiles[oldX, oldY].IsTemporary = true;

            OldIndexX = oldX;
            OldIndexY = oldY;

          //  AddTemporaryTiles(TempTile);
        }

        //Basic Replacement.
        public void ReplaceTileWithNewTile(int tileToReplaceX, int tileToReplaceY, int newTileGID)
        {
            Tile ReplaceMenttile = new Tile(tiles[tileToReplaceX, tileToReplaceY].OldX, tiles[tileToReplaceX, tileToReplaceY].OldY, newTileGID, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);
            tiles[tileToReplaceX, tileToReplaceY] = ReplaceMenttile;
        }
        #endregion

        #region INTERACTIONS

        //Used for interactions with background tiles only
        public void InteractWithBackground(GameTime gameTime, int oldX, int oldY)
        {
            if (Game1.userInterface.BottomBar.GetCurrentEquippedTool() == "shovel")
            {

            
                if (mapName.Tilesets[0].Tiles[tiles[oldX, oldY].GID].Properties.ContainsKey("diggable"))
                {
                  if (mapName.Tilesets[0].Tiles[tiles[oldX, oldY].GID].Properties.ContainsValue("dirt"))
                  {
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DigDirtInstance, false, 1);
                    ReplaceTileWithNewTile(oldX, oldY, 6074);
                  }

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
                if(Tiles[oldX,oldY].portalDestination == "lodgeInterior" && Game1.userInterface.BottomBar.GetCurrentEquippedTool() == "lodgeKey")
                {
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.DoorOpenInstance, false, 1);
                    Game1.Player.controls.Direction = Dir.Up;
                    Game1.gameStages = Stages.LodgeInteior;
                    Game1.Player.position.X = 878;
                    Game1.Player.position.Y = 809;
                }
            }


           if(mapName.Tilesets[0].Tiles[tiles[oldX, oldY].GID].Properties.ContainsKey("stone"))
            {
                //SpecificInteraction(gameTime, oldX, oldY, "CutGrassDown", "CutGrassRight", "CutGrassLeft", "CutGrassUp");
                GeneralInteraction(gameTime, oldX, oldY);
                Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.StoneSmashInstance, false, 1);
            }



            if (Game1.userInterface.BottomBar.GetCurrentEquippedTool() == "secateur")
            {



                if (mapName.Tilesets[0].Tiles[tiles[oldX, oldY].GID].Properties.ContainsKey("grass"))
                {
                    SpecificInteraction(gameTime, oldX, oldY, "CutGrassDown", "CutGrassRight", "CutGrassLeft", "CutGrassUp");
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.GrassBreakInstance, false, 1);

                }
            }
             
        }

        //interact without any animations
        public void GeneralInteraction(GameTime gameTime, int oldX, int oldY)
        {
            tiles[oldX, oldY].IsAnimating = true;
            tiles[oldX, oldY].KillAnimation = true;

        }

        //specify which animations you want to use depending on where the player is in relation to the object



        public void SpecificInteraction(GameTime gameTime, int oldX, int oldY, string down, string right, string left, string up)
        {
            tiles[oldX, oldY].IsAnimating = true;
            tiles[oldX, oldY].KillAnimation = true;

            if (Game1.Player.Position.Y < tiles[oldX, oldY].Y - 30)
            {
                Game1.Player.controls.Direction = Dir.Down;
            }

            else if (Game1.Player.Position.Y > tiles[oldX, oldY].Y)
            {
                Game1.Player.controls.Direction = Dir.Up;
            }

            else if (Game1.Player.Position.X < tiles[oldX, oldY].X)
            {
                Game1.Player.controls.Direction = Dir.Right;
            }
            else if (Game1.Player.Position.X > tiles[oldX, oldY].X)
            {
                Game1.Player.controls.Direction = Dir.Left;
            }




            //Game1.Player.IsMoving = false;
            if (Game1.Player.controls.Direction == Dir.Down)
            {
                Game1.Player.PlayAnimation(gameTime, down);
                Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.GrassBreakInstance, false, 1);
            }
            else if (Game1.Player.controls.Direction == Dir.Right)
            {
                Game1.Player.PlayAnimation(gameTime, right);
                Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.GrassBreakInstance, false, 1);
            }
            else if (Game1.Player.controls.Direction == Dir.Left)
            {
                Game1.Player.PlayAnimation(gameTime, left);
                Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.GrassBreakInstance, false, 1);
            }
            else if (Game1.Player.controls.Direction == Dir.Up)
            {
                Game1.Player.PlayAnimation(gameTime, up);
                Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.GrassBreakInstance, false, 1);
            }
        }

        #endregion

        #region DESTROYTILES



        public void Destroy(int oldX, int oldY)
        {
            if (tiles[oldX, oldY].IsFinishedAnimating)
            {
                if (mapName.Tilesets[0].Tiles[tiles[oldX, oldY].GID].Properties.ContainsKey("grass"))
                {
                    Game1.Iliad.allObjects.Remove(tiles[oldX, oldY].TileObject);

                    Game1.Iliad.allItems.Add(new WorldItem("grass", graphicsDevice, content, new Vector2(tiles[oldX, oldY].DestinationRectangle.X, tiles[oldX, oldY].DestinationRectangle.Y)) { IsTossable = true });

                    ReplaceTilePermanent(oldX, oldY);
                }



                else if (mapName.Tilesets[0].Tiles[tiles[oldX, oldY].GID].Properties.ContainsKey("stone"))
                {
                    Game1.Iliad.allObjects.Remove(tiles[oldX, oldY].TileObject);

                    Game1.Iliad.allItems.Add(new WorldItem("stone", graphicsDevice, content, new Vector2(tiles[oldX, oldY].DestinationRectangle.X, tiles[oldX, oldY].DestinationRectangle.Y)) { IsTossable = true });

                    ReplaceTilePermanent(oldX, oldY);
                }
            }

        }

                        
   
        }
        #endregion

    
}