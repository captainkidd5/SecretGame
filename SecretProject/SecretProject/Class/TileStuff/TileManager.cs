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

namespace SecretProject.Class.TileStuff
{
    class TileManager
    {
        
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

        MouseManager myMouse;

        public TileManager(Texture2D tileSet, TmxMap mapName, TmxLayer layerName, MouseManager mouse)
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



            tiles = new Tile[tilesetTilesWide, tilesetTilesHigh];

            myMouse = mouse;

            //add all tiles in buildings layer to object list.

            foreach (TmxLayerTile layerNameTile in layerName.Tiles)
            {
                if(layerNameTile.Gid != 0)
                {
                    tileCounter++;
                }

                tiles[layerNameTile.X, layerNameTile.Y] = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);

                

            }
        }

        //TODO: 
        //need to assign specific replacable objects their own tile properties
        //need "is closest to" method
        //need "replace" method


        public void DrawTiles(SpriteBatch spriteBatch, float depth)
        {

            for (var i = 0; i < tilesetTilesWide; i++)
            {
                for(var j = 0; j < tilesetTilesHigh; j++)
                {
                    if (isBuilding)
                    {
                        if (myMouse.IsHoveringTile(tiles[i, j].DestinationRectangle))
                        {
                            if(myMouse.IsClicked)
                            {
                                ReplaceTile(i, j);

                            }
                            
                        }
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
            //oldX = oldX + 15;
          //  oldY = oldY + 15;


            Tile ReplaceMenttile = new Tile(tiles[oldX, oldY].oldX, tiles[oldX, oldY].oldY, 3335, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);
            tiles[oldX, oldY] = ReplaceMenttile;
        }
        
    }

}