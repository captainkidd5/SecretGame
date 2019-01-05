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

namespace SecretProject.Class.TileStuff
{
    class TileManager
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

                tiles[layerNameTile.X, layerNameTile.Y] = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);

            }


            if (isBuilding)
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

                                    tiles[i,j].TileObject = new ObjectBody(graphicsDevice,
                                        new Rectangle(tiles[i, j].DestinationRectangle.X + (int)Math.Ceiling(tempObj.X),
                                        tiles[i, j].DestinationRectangle.Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                                        (int)Math.Ceiling(tempObj.Height)), tiles[i, j].DestinationRectangle.X);

                                    Iliad.allObjects.Add(tiles[i,j].TileObject);
                                }
                            }
                        }
                    }
                }
            }      
        }

        //TODO: 

        public void DrawTiles(SpriteBatch spriteBatch, float depth)
        {
            

            for (var i = 0; i < tilesetTilesWide; i++)
            {
                for(var j = 0; j < tilesetTilesHigh; j++)
                {
                    if (myMouse.IsHoveringTile(tiles[i, j].DestinationRectangle))
                    {

                        // IsBeingSelected(i, j);
                        // if(isActive)
                        //{
                        if (isBuilding)
                        {
                            if (myMouse.IsClicked)
                            {
                                ReplaceTile(i, j);
                                //if (isBuilding)
                                //{
                                 //   Iliad.allObjects.Add(new ObjectBody(graphicsDevice, tiles[i, j].DestinationRectangle));
                                //}
                            }
                        }
                            
                        //}                    
                    }
                    if (tiles[i, j].GID != 0)
                    {
                     
                        spriteBatch.Draw(tileSet, tiles[i, j].DestinationRectangle, tiles[i, j].SourceRectangle, Color.White, (float)0, new Vector2(0, 0), SpriteEffects.None, depth);
                    }                   
                }
            }
        }
        public void ReplaceTile(int oldX, int oldY)
        {
            if (mapName.Tilesets[0].Tiles.ContainsKey(tiles[oldX, oldY].GID))
            {

                if (mapName.Tilesets[0].Tiles[tiles[oldX, oldY].GID].Properties.ContainsKey("grass"))
                {
                    Iliad.allObjects.Remove(tiles[oldX, oldY].TileObject);
                    Iliad.allItems.Add(new WorldItem("grass", graphicsDevice, content, new Vector2(tiles[oldX, oldY].DestinationRectangle.X, tiles[oldX, oldY].DestinationRectangle.Y)));

                    Tile ReplaceMenttile = new Tile(tiles[oldX, oldY].OldX, tiles[oldX, oldY].OldY, 0, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);
                    tiles[oldX, oldY] = ReplaceMenttile;
                }
                else
                {

                }
            }
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