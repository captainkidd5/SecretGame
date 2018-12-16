using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    public class Tile
    {
        //--------------------------------------
        //Map Specifications
        private int tilesetTilesWide;
        private int tilesetTilesHigh;
        private int mapWidth;
        private int mapHeight;

        //--------------------------------------
        //Tile General Specifications
        private int tileFrame;
        private int tileHeight = 16;
        private int tileWidth = 16;
        private int column;
        private int row;
        private int tileNumber;
        public float X { get; set; }
        public float Y { get; set; }
        private int gID;
        public int GID { get { return gID; } set { gID = value; } }

        private bool isMoving = false;
        public bool IsMoving { get { return isMoving; } set { isMoving = value; } }

        private bool isSelected = false;
        public bool IsSelected { get { return isSelected; } set { isSelected = value; } }

        public float oldX;
        public float oldY;

        //--------------------------------------
        //Rectangles
        public Rectangle SourceRectangle;
        public Rectangle DestinationRectangle;



        public Tile(float x, float y, int gID, int tilesetTilesWide, int tilesetTilesHigh, int mapWidth, int mapHeight, int tileNumber)
        {
            this.tileNumber = tileNumber;

            this.oldX = x;
            this.oldY = y;
            
            this.gID = gID;
            this.tilesetTilesWide = tilesetTilesWide;
            this.tilesetTilesHigh = tilesetTilesHigh;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;

            tileFrame = gID - 1;

            column = tileFrame % tilesetTilesWide;
            row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);

            this.X = (x % mapWidth) * tileWidth;
            this.Y = (y % mapWidth) * tileHeight;

            SourceRectangle = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);
            DestinationRectangle = new Rectangle((int)X, (int)Y, tileWidth, tileHeight);
            

        }

        
    }
}
