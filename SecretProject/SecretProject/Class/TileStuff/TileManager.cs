using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

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
        
        

        protected Tile[,] tiles;

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



            tiles = new Tile[tilesetTilesWide, tilesetTilesHigh];
            tileNumber = 0;

            foreach (TmxLayerTile layerNameTile in layerName.Tiles)
            {
                tiles[layerNameTile.X, layerNameTile.Y] = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid, tilesetTilesHigh, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);
                tileNumber++;

            }
        }

        //TODO: fill array correctly with tiles from tileset.
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
                    }  //need to change x and y
                    
                }

            }

        }
        
    }

}