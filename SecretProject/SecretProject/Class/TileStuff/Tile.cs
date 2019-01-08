using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ObjectFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace SecretProject.Class.TileStuff
{
    public class Tile
    {
        public float X { get; set; }
        public float Y { get; set; }

        public int GID { get { return GID1 -1; } set { GID1 = value; } }
        public bool IsMoving { get; set; } = false;
        public bool IsSelected { get; set; } = false;

        public int TilesetTilesWide { get; set; }
        public int TilesetTilesHigh { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public int TileFrame { get; set; }
        public int TileHeight { get; set; } = 16;
        public int TileWidth { get; set; } = 16;
        public int Column { get; set; }
        public int Row { get; set; }
        public int TileNumber { get; set; }
        public int GID1 { get; set; }
        public float OldY { get => OldY1; set => OldY1 = value; }
        public float OldY1 { get; set; }
        public float OldX { get; set; }

        public bool IsAnimated { get; set; } = false;

        public int Timer { get; set; }
        public int CurrentFrame { get; set; } = 0;
        public int TotalFrames { get; set; }

        //--------------------------------------
        //Rectangles
        public Rectangle SourceRectangle;
        public Rectangle DestinationRectangle;

        //objectgroup stuff
        public ObjectBody TileObject { get; set; }




        public Tile(float x, float y, int gID, int tilesetTilesWide, int tilesetTilesHigh, int mapWidth, int mapHeight, int tileNumber)
        {
            this.TileNumber = tileNumber;

            this.OldX = x;
            this.OldY = y;
            
            this.GID1 = gID;
            this.TilesetTilesWide = tilesetTilesWide;
            this.TilesetTilesHigh = tilesetTilesHigh;
            this.MapWidth = mapWidth;
            this.MapHeight = mapHeight;

            TileFrame = gID - 1;

            Column = TileFrame % tilesetTilesWide;
            Row = (int)Math.Floor((double)TileFrame / (double)tilesetTilesWide);

            this.X = (x % mapWidth) * TileWidth;
            this.Y = (y % mapWidth) * TileHeight;

            SourceRectangle = new Rectangle(TileWidth * Column, TileHeight * Row, TileWidth, TileHeight);
            DestinationRectangle = new Rectangle((int)X, (int)Y, TileWidth, TileHeight);
            

        }

        public void Animate(GameTime gameTime, int totalFrames, int speed)
        {
            

            Timer -= (int)gameTime.ElapsedGameTime.TotalSeconds;

            int addAmount = CurrentFrame * 16;

            if (Timer <= 0)
            {
                CurrentFrame++;
                Timer = speed;
            }
            if (CurrentFrame == totalFrames)
                CurrentFrame = 0;

         //   this.X = (OldX % MapWidth) * TileWidth + addAmount;
         //   this.Y = (OldY % MapWidth) * TileHeight;
            SourceRectangle = new Rectangle(TileWidth * Column + addAmount, TileHeight * Row, TileWidth, TileHeight);

        }


    }

    public class TileHitboxInfo
    {
        public int iD;
        public Rectangle hitBox;

        public TileHitboxInfo(int iD, TmxObject obj)
        {
            this.iD = iD;
            hitBox = new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height);
        }
    }
}
