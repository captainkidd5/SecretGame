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


        public Chunk(int numberOfLayers)
        {
            AllChunkTiles = new List<Tile[,]>();
            for(int i = 0; i< numberOfLayers; i++)
            {
                AllChunkTiles.Add(new Tile[64, 64]);
            }
        }

        public void LoadTile()
        {
            
        }
    }
}
