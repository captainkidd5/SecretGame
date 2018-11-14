using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    class Tile
    {
        private float x;
        private float y;
        private int gID;

        private int tilesetTilesWide;
        private int tilesetTilesHigh;
        private int mapWidth;
        private int mapHeight;

        private int tileFrame;

        private Texture2D texture;

        private int tileHeight = 16;
        private int tileWidth = 16;



        public float X { get { return x; } set { x = value; } }
        public float Y { get { return y; } set { y = value; } }
        public int GID { get { return gID; } set { gID = value; } }

        public Tile(float x, float y, int gID, int tilesetTilesWide, int tilesetTilesHigh, int mapWidth, int mapHeight)
        {
            this.x = x;
            this.y = y;
            this.gID = gID;
            this.tilesetTilesWide = tilesetTilesWide;
            this.tilesetTilesHigh = tilesetTilesHigh;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;

            tileFrame = gID - 1;
        }

        public Rectangle SetDestinationRectangle()
        {
            


                    int column = tileFrame % tilesetTilesWide;
                    int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);

                    Rectangle tileRectangle = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);
                    return tileRectangle;

                    //spriteBatch.Draw(tileSet, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tileRectangle, Color.White, (float)0, new Vector2(0, 0), SpriteEffects.None, depth);

        }

        public Rectangle SetSourceRectangle()
        {
            Rectangle soureRectangle = new Rectangle((int)x, (int)y, tileWidth, tileHeight);
            return soureRectangle;
        }
    }
}
