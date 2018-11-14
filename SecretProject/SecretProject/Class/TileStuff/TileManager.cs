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
        protected SpriteBatch spriteBatch;
        protected Texture2D tileSet;
        protected TmxMap mapName;
        protected TmxLayer layerName;
        protected float depth;
        protected int tilesetTilesWide;
        protected int tilesetTilesHigh;
        protected int tileWidth;
        protected int tileHeight;

        protected int mapWidth;
        protected int mapHeight;
        
        

        protected Tile[,] tiles;

        public TileManager(SpriteBatch spriteBatch, Texture2D tileSet, TmxMap mapName, TmxLayer layerName, float depth, int tilesetTilesWide)
        {
            this.spriteBatch = spriteBatch;
            this.tileSet = tileSet;
            this.mapName = mapName;
            this.layerName = layerName;
            this.depth = depth;
            this.tilesetTilesWide = tilesetTilesWide;

            tileWidth = mapName.Tilesets[0].TileWidth;
            tileHeight = mapName.Tilesets[0].TileHeight;

            tilesetTilesWide = tileSet.Width / tileWidth;
            tilesetTilesHigh = tileSet.Height / tileHeight;

            mapWidth = mapName.Width;
            mapHeight = mapName.Height;



            tiles = new Tile[tilesetTilesHigh, tilesetTilesWide];
        }

        //TODO: fill array correctly with tiles from tileset.
        //need "is closest to" method
        //need "replace" method

        public void LoadTiles()
        {
            foreach (TmxLayerTile layerNameTile in layerName.Tiles)
            {
                tiles[layerNameTile.X, layerNameTile.Y] = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid, tilesetTilesHigh, tilesetTilesHigh, mapWidth, mapHeight);

            }
        }

        public void DrawTiles()
        {
            int column = 0;
            for (var i = 0; i < tileHeight; i++)
            {
                for(var j = 0; j < tileWidth; j++)
                {
                    if(tiles[j, i].GID != 0)
                    {
                        spriteBatch.Draw(tileSet, tiles[j, i].SetSourceRectangle(), tiles[j, i].SetDestinationRectangle(), Color.White, (float)0, new Vector2(0, 0), SpriteEffects.None, depth);
                    }  
                }

            }

        }






        public void DrawTiles2()
        {
            for (var i = 0; i < layerName.Tiles.Count; i++)
            {
                int gid = layerName.Tiles[i].Gid;


                if (gid == 0)
                {

                }

                else
                {
                    int tileFrame = gid - 1;
                    int column = tileFrame % tilesetTilesWide;
                    int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);

                    float x = (i % mapName.Width) * mapName.TileWidth;
                    float y = (float)Math.Floor(i / (double)mapName.Width) * mapName.TileHeight;

                    Rectangle tileSetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);

                    spriteBatch.Draw(tileSet, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tileSetRec, Color.White, (float)0, new Vector2(0, 0), SpriteEffects.None, depth);
                }
            }
        }
    }

}