using Microsoft.Xna.Framework;

namespace SecretProject.Class.TileStuff
{
    public enum MapLayer
    {
        BackGround = 0,
        MidGround = 1,
        Buildings = 2,
        ForeGround = 3
    }

    public class Tile
    {

        private int gid;
        public int GID { get { return gid - 1; } set { gid = value; } }
        public int Y { get; set; }
        public int X { get; set; }
        public float LayerToDrawAt { get; set; } = 0f;
        public float LayerToDrawAtZOffSet { get; set; } = 0f;
        public float ColorMultiplier { get; set; } = 1f;

        public Rectangle SourceRectangle { get; set; }
        public Rectangle DestinationRectangle { get; set; }




        public Tile(int x, int y, int gID)
        {

            this.X = x;
            this.Y = y;

            this.GID = gID;



        }

        public string GetTileKeyStringNew(int layer, IInformationContainer container)
        {
            return "" + this.X + "," + this.Y + "," + layer;
        }

        public string GetTileKey(int layer)
        {
            string XString = this.X.ToString();
            if (XString.Length < 4)
            {
                XString = XString.PadLeft(4, '0');
            }
            string YString = this.Y.ToString();
            if (YString.Length < 4)
            {
                YString = YString.PadLeft(4, '0');
            }
            return layer.ToString() + XString + YString;

        }

        public int GetTileKeyAsInt(int layer, IInformationContainer container)
        {
            return (this.X << 16) | (this.Y << 9) | layer;
        }




    }

}