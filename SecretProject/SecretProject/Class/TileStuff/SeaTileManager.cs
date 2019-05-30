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

        public List<TmxLayer> AllLayers { get; set; }
        public Chunk[] AllChunks { get; set; }

        public List<Tile[,]> AllTiles;
        GraphicsDevice graphics;

        public Texture2D TileSet { get; set; }



        //public int ChunkPixelWidth { get; set; } = 

        #region CONSTRUCTOR

        //TODO LayerDepth List
        public SeaTileManager(Texture2D tileSet, TmxMap mapName, List<TmxLayer> allLayers, GraphicsDevice graphicsDevice, ContentManager content, int tileSetNumber, List<float> allDepths)
        {
            this.SeaMap = mapName;
            this.AllLayers = allLayers;
            AllTiles = new List<Tile[,]>();
            for (int i = 0; i < AllLayers.Count; i++)
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
            //this.SeaMap = content.Load<TmxMap>("Content/Map/sea");
            this.TileSet = tileSet;

            //AllLayers.

            AllChunks = new Chunk[256];

            int currentTileIndexX = 0;
            int currentTileIndexY = 0;

            int anchorIndexX = 0;
            int anchorIndexY = 0;



            for (int c = 0; c < AllChunks.Length; c++)
            {
                AllChunks[c] = new Chunk(AllLayers.Count);


                
                for (int l = 0; l < AllLayers.Count; l++)
                {
                    currentTileIndexX = anchorIndexX;
                    currentTileIndexY = anchorIndexY;

                    for (int x = 0; x < 64; x++)// 8 * 8 = 64
                    {

                        for (int y = 0; y < 64; y++)//
                        {
                            AllChunks[c].AllChunkTiles[l][x, y] = AllTiles[l][currentTileIndexX, currentTileIndexY];
                            currentTileIndexY++;
                        }
                        currentTileIndexX++;
                        currentTileIndexY = anchorIndexY;

                        //if (currentTileIndexY == 1024)
                        //{
                        //    currentTileIndexY = 0;
                        //}

                    }
                    
                }
                if (currentTileIndexX != 0 && currentTileIndexX % 1024 == 0)
                {
                    anchorIndexY += 64;
                    anchorIndexX = 0;
                }
                else
                {
                    anchorIndexX += 64;
                }


                AllChunks[c].LoadRectangle();
            }

        }
        //have to make alltiles first and then load from there
        public void LoadContent(ContentManager content, Texture2D tileSet)
        {

            for (int c = 0; c < AllChunks.Length; c++)
            {
                AllChunks[c].LoadRectangle();
            }

        }


        #endregion

        public void DrawTiles(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < AllChunks.Length; i++)
            {
                if(Game1.Player.Rectangle.Intersects(AllChunks[i].Rectangle))
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int x = 0; x < AllChunks[i].AllChunkTiles[l].GetLength(0); x++)
                        {
                            for (int y = 0; y < AllChunks[i].AllChunkTiles[l].GetLength(1); y++)
                            {
                                spriteBatch.Draw(TileSet, AllChunks[i].AllChunkTiles[l][x, y].DestinationRectangle, AllChunks[i].AllChunkTiles[l][x, y].SourceRectangle, AllChunks[i].AllChunkTiles[l][x, y].TileColor);
                            }
                        }
                    }
                }
                
            }
        }
    }
}