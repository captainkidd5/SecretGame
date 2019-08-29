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

            





            //TileColor = Color.DimGray * 1.5f;
            AStarTileValue = 1;

        }

        public float GetTileKey(int mapWidth, int mapHeight)
        {
            float X = (OldX % mapWidth) * 16;
            float Y = (OldY % mapHeight) * 16;
            string keyString = this.GID.ToString() + (Math.Floor(X / 16)).ToString() + ((Math.Floor(Y / 16).ToString()));
            return float.Parse(keyString);
        }


    }

}