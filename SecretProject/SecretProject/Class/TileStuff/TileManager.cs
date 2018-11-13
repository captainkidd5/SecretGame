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
        SpriteBatch spriteBatch;
        Texture2D tileSet;
        TmxMap mapName;
        TmxLayer layerName;
        float depth;
        int tilesetTilesWide;
        int tilesetTilesHigh;
        int tileWidth;
        int tileHeight;
        

        protected int[,] tiles;

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



            tiles = new int[tilesetTilesHigh, tilesetTilesWide];
        }

        //TODO: fill array correctly with tiles from tileset.

        public void AddTiles()
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



        public void DrawTiles()
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