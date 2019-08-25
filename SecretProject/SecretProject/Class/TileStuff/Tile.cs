using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.ObjectFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TiledSharp;
using XMLData.ItemStuff;

namespace SecretProject.Class.TileStuff
{
    public class Tile
    {
        public float X { get; set; }
        public float Y { get; set; }

        private int gid;
        public int GID { get { return gid - 1; } set { gid = value; } }
        public bool IsSelected { get; set; }

        public float OldY { get; set; }

        public float OldX { get; set; }


        public float LayerToDrawAt { get; set; } = 0f;
        public float LayerToDrawAtZOffSet { get; set; } = 0f;


        public int HitPoints { get; set; } = 1;
        public int AStarTileValue { get; set; }


        //--------------------------------------
        //Rectangles
        public Rectangle SourceRectangle;
        public Rectangle DestinationRectangle;


        public bool HorizontalFlip { get; set; }
        public bool VeritcalFlip { get; set; }
        public bool DiagonalFlip { get; set; }



        private Tile()
        {

        }

        public Tile(float x, float y, int gID, int tilesetTilesWide, int tilesetTilesHigh, int mapWidth, int mapHeight)
        {
            this.IsSelected = false;
            this.OldX = x;
            this.OldY = y;

            this.GID = gID;


            int Column = GID % tilesetTilesWide;
            int Row = (int)Math.Floor((double)GID / (double)tilesetTilesWide);

            this.X = (x % mapWidth) * 16;
            this.Y = (y % mapHeight) * 16;

            SourceRectangle = new Rectangle(16 * Column, 16 * Row, 16, 16);

            DestinationRectangle = new Rectangle((int)X, (int)Y, 16, 16);



            //TileColor = Color.DimGray * 1.5f;
            AStarTileValue = 1;

        }

        public int GetTileKey()
        {
            string keyString = this.GID.ToString() + (this.X / 16).ToString() + (this.Y / 16).ToString();
            return int.Parse(keyString);
        }


    }

}