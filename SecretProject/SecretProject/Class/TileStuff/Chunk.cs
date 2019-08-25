using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.TileStuff
{
    public class Chunk
    {
        public List<Tile[,]> AllChunkTiles { get; set; }
        public Rectangle Rectangle { get; set; }
        public bool IsActive { get; set; }

        public Chunk(int numberOfLayers, List<Tile[,]> tilesToLoad, int size)
        {
            AllChunkTiles = tilesToLoad;
           
            IsActive = false;
            
        }

        public void LoadRectangle()
        {
            this.Rectangle = new Rectangle((int)AllChunkTiles[0][0, 0].X, (int)AllChunkTiles[0][0, 0].Y, 1024,
                1024);
        }
    }
}
