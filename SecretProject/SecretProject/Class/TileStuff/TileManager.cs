using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Stage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

using SecretProject.Class.ObjectFolder;
using SecretProject.Class.Controls;
using SecretProject.Class.Universal;

namespace SecretProject.Class.TileStuff
{
    class TileManager
    {
        protected Game1 game;

        protected Texture2D tileSet;
        protected TmxMap mapName;
        protected TmxLayer layerName;


        //--------------------------------------
        //Map Specifications
        protected int tilesetTilesWide;
        protected int tilesetTilesHigh;
        protected int mapWidth;
        protected int mapHeight;
        //--------------------------------------
        //Tile Specificications
        protected int iD;
        protected int tileWidth;
        protected int tileHeight;
        protected int tileNumber;


        //--------------------------------------
        //Counting
        protected int tileCounter;


        //--------------------------------------
        //2D Array of All Tiles
        protected Tile[,] tiles;
        public Tile[,] Tiles { get { return tiles; } }

        public bool isBuilding = false;
        public bool isActive = false;

        MouseManager myMouse;

        GraphicsDevice graphicsDevice;

        private int replaceTileGid;

        public int ReplaceTileGid { get { return replaceTileGid; } set { replaceTileGid = value; } }

        List<TmxObject> tileObjects;


     

        public TileManager(Game1 game, Texture2D tileSet, TmxMap mapName, TmxLayer layerName, MouseManager mouse, GraphicsDevice graphicsDevice, bool isBuilding)
        {
            this.tileSet = tileSet;
            this.mapName = mapName;
            this.layerName = layerName;

            tileWidth = mapName.Tilesets[0].TileWidth;
            tileHeight = mapName.Tilesets[0].TileHeight;

            tilesetTilesWide = tileSet.Width / tileWidth;
            tilesetTilesHigh = tileSet.Height / tileHeight;

            mapWidth = mapName.Width;
            mapHeight = mapName.Height;
            
            this.tileCounter = 0;

            this.graphicsDevice = graphicsDevice;

            tiles = new Tile[tilesetTilesWide, tilesetTilesHigh];

            myMouse = mouse;

            this.game = game;

            this.isBuilding = isBuilding;
            //add all tiles in buildings layer to object list.


            //tilecustomobjects
            //TODO: assign rectangles to corresponding Gid tiles

            /*
            for (int i = 1; i < layerName.Tiles.Count; i++)
            {
                if (mapName.Tilesets[0].Tiles.ContainsKey(i))
                {

                    tileObjects.Add(mapName.Tilesets[0].Tiles[i].ObjectGroups[0].Objects[0]);
                    tileHitInfo.Add(new TileHitboxInfo(i, mapName.Tilesets[0].Tiles[i].ObjectGroups[0].Objects[0]));
                }
            }
            */
            foreach (TmxLayerTile layerNameTile in layerName.Tiles)
            {

                tiles[layerNameTile.X, layerNameTile.Y] = new Tile(layerNameTile.X, layerNameTile.Y, layerNameTile.Gid, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber);

            }

            //Iliad.allObjects.Add(new ObjectBody(graphicsDevice, new Rectangle((int)mapName.Tilesets[0].Tiles[3532].ObjectGroups[0].Objects[0].X,
            //  (int)mapName.Tilesets[0].Tiles[3532].ObjectGroups[0].Objects[0].Y, 5, 5)));
            //TODO: Need to adjust rectangles so they draw to tile destination position +- offset etc
            // if (isBuilding)
            // {

            if (isBuilding)
            {


                for (var i = 0; i < tilesetTilesWide; i++)
                {
                    for (var j = 0; j < tilesetTilesHigh; j++)
                    {
                        if (tiles[i, j].GID != 0)
                        {
                            if (mapName.Tilesets[0].Tiles.ContainsKey(tiles[i, j].GID))
                            {
                                for (int k = 0; k < mapName.Tilesets[0].Tiles[tiles[i, j].GID].ObjectGroups[0].Objects.Count; k++)
                                {
                                    TmxObject tempObj = mapName.Tilesets[0].Tiles[tiles[i, j].GID].ObjectGroups[0].Objects[k];
                                    Iliad.allObjects.Add(new ObjectBody(graphicsDevice,
                                        new Rectangle(tiles[i , j].DestinationRectangle.X + (int)Math.Ceiling(tempObj.X),
                                        tiles[i, j].DestinationRectangle.Y + (int)Math.Ceiling(tempObj.Y), (int)Math.Ceiling(tempObj.Width),
                                        (int)Math.Ceiling(tempObj.Height))));
                                }
                            }

                        }
                    }

                }
            }
                
                /*
            if(mapName.Tilesets[0].Tiles.ContainsKey(3532))
                {
                Iliad.allObjects.Add(new ObjectBody(graphicsDevice,
                                        new Rectangle(tiles[45,41].DestinationRectangle.X,
                                        tiles[45, 41].DestinationRectangle.Y, (int)10,
                                        (int)10)));
            }
            */

            // }


        }


        //TODO: 
        //need to assign specific replacable objects their own tile properties
        //need "is closest to" method
        //need "replace" method
        //replaced background is not transparent ?
        public void DrawTiles(SpriteBatch spriteBatch, float depth)
        {

            for (var i = 0; i < tilesetTilesWide; i++)
            {
                for(var j = 0; j < tilesetTilesHigh; j++)
                {
                    if (myMouse.IsHoveringTile(tiles[i, j].DestinationRectangle))
                    {

                       // IsBeingSelected(i, j);
                       // if(isActive)
                        //{
                            /*if (myMouse.IsClicked)
                            {
                                ReplaceTile(i, j);
                                if(isBuilding)
                                {
                                    Iliad.allObjects.Add(new ObjectBody(graphicsDevice, tiles[i, j].DestinationRectangle));
                                }
                            }
                            */
                        //}                    
                    }
                    if (tiles[i, j].GID != 0)
                    {
                     
                        spriteBatch.Draw(tileSet, tiles[i, j].DestinationRectangle, tiles[i, j].SourceRectangle, Color.White, (float)0, new Vector2(0, 0), SpriteEffects.None, depth);
                    }                   
                }
            }
        }
        public void ReplaceTile(int oldX, int oldY)
        {
            Tile ReplaceMenttile = new Tile(tiles[oldX, oldY].oldX, tiles[oldX, oldY].oldY, ReplaceTileGid + 1, tilesetTilesWide, tilesetTilesHigh, mapWidth, mapHeight, tileNumber); //gid is plus 1 for some reason
            tiles[oldX, oldY] = ReplaceMenttile;
        }        

        //NEEDS WORK
        public void IsBeingSelected(int oldX, int oldY)
        {
            

            if(tiles[oldX, oldY].IsSelected == true)
            {
               // Game1.isMyMouseVisible = false;
                
                if(myMouse.IsClicked)
                {
                   tiles[oldX, oldY].IsSelected = false;
                    Game1.isMyMouseVisible = true;
                }
            }
            if(myMouse.IsClicked)
            {
                tiles[oldX, oldY].IsSelected = true;
            }

        }

    }
}