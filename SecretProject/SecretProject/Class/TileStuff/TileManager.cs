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


        MouseManager myMouse;
        ContentManager content;
        GraphicsDevice graphicsDevice;

        public int ReplaceTileGid { get; set; }

        List<TmxObject> tileObjects;

        public int CurrentIndexX { get; set; }
        public int CurrentIndexY { get; set; }

        public Tile TempTile { get; set; }

        #region CONSTRUCTOR

        public TileManager(Game1 game, Texture2D tileSet, TmxMap mapName, TmxLayer layerName, MouseManager mouse, GraphicsDevice graphicsDevice, ContentManager content, bool isBuilding)
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

            myMouse = mouse;

            this.game = game;

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
                                if(mapName.Tilesets[0].Tiles[tiles[i, j].GID].Properties.ContainsKey("Animated"))
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

                                if(mapName.Tilesets[0].Tiles[tiles[i, j].GID].Properties.ContainsKey("step"))
                                {
                                  //  game.Exit();
                                    tiles[i, j].HasSound = true;
                                    
                                }
                                else
                                {
                                    tiles[i, j].HasSound = false;
                                }

                                
                            }

                                if (isBuilding)
                                {

                                    for (int k = 0; k < mapName.Tilesets[0].Tiles[tiles[i, j].GID].ObjectGroups[0].Objects.Count; k++)
                                    {
                                        TmxObject tempObj = mapName.Tilesets[0].Tiles[tiles[i, j].GID].ObjectGroups[0].Objects[k];

                                        tiles[i, j].TileObject = new ObjectBody(graphicsDevice,
                                            new Rectangle(tiles[i, j].DestinationRectangle.X + (int)Math.Ceiling(tempObj.X),
                                            tiles[i, j].DestinationRectangle.Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                                            (int)Math.Ceiling(tempObj.Height)), tiles[i, j].DestinationRectangle.X);

                                        Iliad.allObjects.Add(tiles[i, j].TileObject);
                                    }


                                }
                            }
                        }
                    }
                }
            
        }
        #endregion
        public bool TileInteraction { get; set; } = false;


        #region UPDATE
        public void Update(GameTime gameTime)
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

                        if(tiles[i,j].HasSound == true)
                        {
                            Intersect(i, j, Game1.Player);
                        }
                        if (myMouse.IsHoveringTile(tiles[i, j].DestinationRectangle))
                        {
                            CurrentIndexX = i;
                            CurrentIndexY = j;
                            // IsBeingSelected(i, j);
                            // if(isActive)
                            //{
                            if (isBuilding)
                            {
                                
                                if (myMouse.IsRightClicked)
                                {
                                    Interact(i, j);

                                    //if (isBuilding)
                                    //{
                                    //   Iliad.allObjects.Add(new ObjectBody(graphicsDevice, tiles[i, j].DestinationRectangle));
                                    //}
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
                    }
                }
            }
        }

        #endregion
        //TODO: 

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
                            spriteBatch.Draw(tileSet, tiles[i, j].DestinationRectangle, tiles[i, j].SourceRectangle, Color.White, (float)0, new Vector2(0, 0), SpriteEffects.None, depth);
                        }
                    }
                     
                        
                }
            }
        }
        #endregion

        //TODO: get sounds to play when walked over
        public void Intersect(int XCor, int YCor, Player player)
        {
            if(player.Rectangle.Intersects(tiles[XCor,YCor].DestinationRectangle))
            {
                Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.StoneStepInstance, false, 0);
            }
        }



        public void Interact(int oldX, int oldY)
        {
            if (mapName.Tilesets[0].Tiles.ContainsKey(tiles[oldX, oldY].GID))
            {

                if (mapName.Tilesets[0].Tiles[tiles[oldX, oldY].GID].Properties.ContainsKey("grass"))
                {
                    tiles[oldX, oldY].IsAnimating = true;
                    tiles[oldX, oldY].KillAnimation = true;
                }
             }
        }

        public void Destroy(int oldX, int oldY)
        {
                   if (tiles[oldX, oldY].IsFinishedAnimating)
                    {



                        Iliad.allObjects.Remove(tiles[oldX, oldY].TileObject);

                        Iliad.allItems.Add(new WorldItem("grass", graphicsDevice, content, new Vector2(tiles[oldX, oldY].DestinationRectangle.X, tiles[oldX, oldY].DestinationRectangle.Y)) { IsTossable = true });

                    ReplaceTilePermanent(oldX, oldY);
                    }                
   
        }

        public void ReplaceTilePermanent(int oldX, int oldY)
        {
 


                Tile ReplaceMenttile = new Tile(tiles[oldX, oldY].OldX, tiles[oldX, oldY].OldY, 0, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);
                tiles[oldX, oldY] = ReplaceMenttile;
        }

        public void ReplaceTileTemporary(int oldX, int oldY, int GID)
        {
          //if (TempTile != null)
          //{
               //tiles[oldX, oldY] = TempTile;
           // }
            

            Tile ReplaceMenttile = new Tile(tiles[oldX, oldY].OldX, tiles[oldX, oldY].OldY, GID, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);
            TempTile = tiles[oldX, oldY];
            tiles[oldX, oldY] = ReplaceMenttile;
        }
        

        //NEEDS WORK
        public void IsBeingSelected(int oldX, int oldY)
        {
            

            if(tiles[oldX, oldY].IsSelected == true)
            {
               // Game1.isMyMouseVisible = false;
                
                if(myMouse.IsClicked)
                {
                   tiles[oldX, oldY].IsSelected = false;
                    Game1.isMyMouseVisible = true;
                }
            }
            if(myMouse.IsClicked)
            {
                tiles[oldX, oldY].IsSelected = true;
            }

        }

    }
}