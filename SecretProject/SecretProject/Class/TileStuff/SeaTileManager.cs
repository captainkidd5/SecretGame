using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.StageFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

using System.Xml.Serialization;

using SecretProject.Class.ObjectFolder;
using SecretProject.Class.Controls;

using Microsoft.Xna.Framework.Content;
using SecretProject.Class.Universal;

namespace SecretProject.Class.TileStuff
{
    public class SeaTileManager
    {

        public TmxMap SeaMap { get; set; }
        public int MapWidth { get; set; } = 1024;
        public int MapHeight { get; set; } = 1024;

        public int TileWidth { get; set; } = 16;
        public int TileHeight { get; set; } = 16;

        public int ChunkTileWidth { get; set; } = 64;
        public int ChunkTileHeight { get; set; } = 64;

        public int NumberOfLayers { get; set; } = 4;

        public TmxLayer[] AllLayers { get; set; }
        public Chunk[] AllChunks { get; set; }

        public List<Tile[,]> AllTiles;
        GraphicsDevice graphics;



        //public int ChunkPixelWidth { get; set; } = 

        #region CONSTRUCTOR

        //TODO LayerDepth List
        public SeaTileManager(TmxMap map, TmxLayer[] allLayers, GraphicsDevice graphics)
        {
            this.SeaMap = map;
            this.AllLayers = allLayers;
           
            for (int i = 0; i < AllLayers.Length; i++)
            {
                AllTiles.Add(new Tile[1024, 1024]);

            }

            //Tiles = new Tile[tilesetTilesWide, tilesetTilesHigh];

            for (int i = 0; i < AllTiles.Count; i++)
            {
                foreach (TmxLayerTile layerNameTile in AllLayers[i].Tiles)
                {
                    Tile tempTile = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid, 100, 100, 1024, 1024);
                    AllTiles[i][layerNameTile.X, layerNameTile.Y] = tempTile;


                }
            }

        }
        //have to make alltiles first and then load from there
        public void LoadContent(ContentManager content, Texture2D tileSet)
        {
            this.SeaMap = content.Load<TmxMap>("Content/Map/sea");


            //AllLayers.

            //AllChunks = new Chunk[256];

            //int currentLayer = 0;

            //for(int c=0; c< AllChunks.Length; c++ )
            //{
            //    AllChunks[c] = new Chunk();
            //    for (int l=0; l<AllLayers.Length; l++)
            //    {
            //        for(int x=0; x<64;x++)
            //        {
            //            for(int y=0; y< 64; y++)
            //            {
            //                AllChunks[c].AllChunkTiles[l][x,y] = new Tile(AllLayers[l].Tiles[64 * c + x, 64*c + y, ])
            //            }
            //        }
            //    }
            //}


            ////AllChunks = new List<Chunk>(256);
            ////for (int z = 0; z < AllChunks.Count; z++)
            ////    for (int z = 0; z < 4; z++)
            ////    {

            ////    }



            //   }


        }
        #endregion
    }
}