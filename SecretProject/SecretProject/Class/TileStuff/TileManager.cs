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

        public bool isBuilding = false;
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

        public TileManager( Texture2D tileSet, TmxMap mapName, TmxLayer layerName, GraphicsDevice graphicsDevice, ContentManager content, bool isBuilding)
        {
            this.tileSet = tileSet;
            this.mapName = mapName;
            this.layerName = layerName;

            tileWidth = mapName.Tilesets[0].TileWidth;
            tileHeight = mapName.Tilesets[0].TileHeight;

            tilesetTilesWide = tileSet.Width / tileWidth;
            tilesetTilesHigh = tileSet.Height / tileHeight;

            mapWidth = mapName.Width;
            mapHeight = mapName.Height;

            this.tileCounter = 0;

            this.graphicsDevice = graphicsDevice;
            this.content = content;

            tiles = new Tile[tilesetTilesWide, tilesetTilesHigh];


            this.isBuilding = isBuilding;

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
                        if (mapName.Tilesets[0].Tiles.ContainsKey(tiles[i, j].GID))
                        {
                            if (mapName.Tilesets[0].Tiles[tiles[i, j].GID].Properties.ContainsKey("Animated"))
                            {
                                tiles[i, j].IsAnimated = true;
                                tiles[i, j].TotalFrames = int.Parse(mapName.Tilesets[0].Tiles[tiles[i, j].GID].Properties["Animated"]);
                                tiles[i, j].Speed = double.Parse(mapName.Tilesets[0].Tiles[tiles[i, j].GID].Properties["Speed"]);


                                if (mapName.Tilesets[0].Tiles[tiles[i, j].GID].Properties.ContainsKey("start"))
                                {

                                    tiles[i, j].IsAnimating = true;

                                }
                                else
                                {
                                    tiles[i, j].IsAnimating = false;
                                }

                                if (mapName.Tilesets[0].Tiles[tiles[i, j].GID].Properties.ContainsKey("step"))
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
                            if (isBuilding)
                            {
                                
                                if (tiles[i, j].DestinationRectangle.Intersects(Game1.Player.ClickRangeRectangle) && mapName.Tilesets[0].Tiles.ContainsKey(tiles[i, j].GID))
                                {
                                    Game1.isMyMouseVisible = false;
                                    Game1.myMouseManager.ToggleNewMouseMode = true;

                                    if (mouse.IsRightClicked)
                                    {
                                        Interact(gameTime, i, j);

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

        public void ReplaceTilePermanent(int oldX, int oldY)
        {
            Tile ReplaceMenttile = new Tile(tiles[oldX, oldY].OldX, tiles[oldX, oldY].OldY, 0, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);
            tiles[oldX, oldY] = ReplaceMenttile;
        }
        public Tile[] temporaryTiles = new Tile[20];


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


        public void Interact(GameTime gameTime, int oldX, int oldY)
        {
            if (Game1.userInterface.BottomBar.GetCurrentEquippedTool() == "secateur")
            {



                if (mapName.Tilesets[0].Tiles[tiles[oldX, oldY].GID].Properties.ContainsKey("grass"))
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
                        Game1.Player.PlayAnimation(gameTime, "CutGrassDown");
                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.GrassBreakInstance, false, 1);
                    }
                    else if (Game1.Player.controls.Direction == Dir.Right)
                    {
                        Game1.Player.PlayAnimation(gameTime, "CutGrassRight");
                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.GrassBreakInstance, false, 1);
                    }
                    else if (Game1.Player.controls.Direction == Dir.Left)
                    {
                        Game1.Player.PlayAnimation(gameTime, "CutGrassLeft");
                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.GrassBreakInstance, false, 1);
                    }
                    else if (Game1.Player.controls.Direction == Dir.Up)
                    {
                        Game1.Player.PlayAnimation(gameTime, "CutGrassUp");
                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.GrassBreakInstance, false, 1);
                    }


                }
            }
             
        }

        public void Destroy(int oldX, int oldY)
        {
                   if (tiles[oldX, oldY].IsFinishedAnimating)
                    {
                        Game1.Iliad.allObjects.Remove(tiles[oldX, oldY].TileObject);

                        Game1.Iliad.allItems.Add(new WorldItem("grass", graphicsDevice, content, new Vector2(tiles[oldX, oldY].DestinationRectangle.X, tiles[oldX, oldY].DestinationRectangle.Y)) { IsTossable = true });

                    ReplaceTilePermanent(oldX, oldY);
                    }                
   
        }   

    }
}