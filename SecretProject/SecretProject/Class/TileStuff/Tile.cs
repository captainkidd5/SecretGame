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
        public float Y { get; set; }
        public float X { get; set; }
        public float LayerToDrawAt { get; set; }
        public float LayerToDrawAtZOffSet { get; set; }


        private Tile()
        {

        }

        public Tile(float x, float y, int gID, int tilesetTilesWide, int tilesetTilesHigh, int mapWidth, int mapHeight)
        {

            this.X = x;
            this.Y = y;

            this.GID = gID;

        }

        public float GetTileKey(int mapWidth, int mapHeight)
        {
            float X = (this.X % mapWidth) * 16;
            float Y = (this.Y % mapHeight) * 16;
            string keyString = this.GID.ToString() + (Math.Floor(X / 16)).ToString() + ((Math.Floor(Y / 16).ToString()));
            return float.Parse(keyString);
        }




    }

}