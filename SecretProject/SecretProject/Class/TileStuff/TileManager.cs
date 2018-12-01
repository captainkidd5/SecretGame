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

namespace SecretProject.Class.TileStuff
{
    class TileManager
    {
        protected int iD;
        protected Texture2D tileSet;
        protected TmxMap mapName;
        protected TmxLayer layerName;
        protected int tilesetTilesWide;
        protected int tilesetTilesHigh;
        protected int tileWidth;
        protected int tileHeight;

        protected int mapWidth;
        protected int mapHeight;

        protected int tileNumber;



        protected int tileCounter;




        protected Tile[,] tiles;

        public Tile[,] Tiles { get { return tiles; } }

        public TileManager(Texture2D tileSet, TmxMap mapName, TmxLayer layerName)
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

            //add all tiles in buildings layer to object list.

            foreach (TmxLayerTile layerNameTile in layerName.Tiles)
            {
                if(layerNameTile.Gid != 0)
                {
                    tileCounter++;
                }

                tiles[layerNameTile.X, layerNameTile.Y] = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid, tilesetTilesHigh, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);

                

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
                    if(tiles[i, j].GID != 0)
                    {
                        spriteBatch.Draw(tileSet, tiles[i, j].DestinationRectangle, tiles[i, j].SourceRectangle, Color.White, (float)0, new Vector2(0, 0), SpriteEffects.None, depth);
                    }  
                    
                }

            }

        }

        public void ReplaceTile(int oldX, int oldY)
        {
            //tiles[oldX, oldY] = new Tile(newX, newY, 3528, tilesetTilesHigh, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);

            tiles[oldX, oldY].GID = 3528;
        }
        
    }

}