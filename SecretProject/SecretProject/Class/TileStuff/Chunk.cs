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
        public bool IsActive { get; set; } = false;

        public Chunk(int numberOfLayers)
        {
            AllChunkTiles = new List<Tile[,]>();
            for(int i = 0; i< numberOfLayers; i++)
            {
                AllChunkTiles.Add(new Tile[64, 64]);
            }
            
        }

        public void LoadRectangle()
        {
            this.Rectangle = new Rectangle((int)AllChunkTiles[1][0, 0].X, (int)AllChunkTiles[1][0, 0].Y, 1024,
                1024);
        }
    }
}
