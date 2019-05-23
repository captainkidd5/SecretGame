﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ObjectFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TiledSharp;

namespace SecretProject.Class.TileStuff
{
    public class Tile
    {
        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;

        private int gid;
        public int GID { get { return gid -1; } set { gid = value; } }
        public bool IsSelected { get; set; } = false;

        public int TilesetTilesWide { get; set; } = 0;
        public int TilesetTilesHigh { get; set; } = 0;
        public int MapWidth { get; set; } = 0;
        public int MapHeight { get; set; } = 0;
        public int TileFrame { get; set; } = 0;
        public int TileHeight { get; set; } = 16;
        public int TileWidth { get; set; } = 16;
        public int Column { get; set; } = 0;
        public int Row { get; set; } = 0;
        public int TileNumber { get; set; } = 0;
        public float OldY { get => OldY1; set => OldY1 = value; }
        public float OldY1 { get; set; } = 0;
        public float OldX { get; set; } = 0;

        public bool IsAnimated { get; set; } = false;
        public bool IsAnimating { get; set; } = false;
        public bool IsFinishedAnimating { get; set; } = false;
        public bool KillAnimation { get; set; } = false;
        public float DelayTimer { get; set; } = 0;

        /// <summary>
        /// ////////////////
        /// </summary>
        public bool Plantable { get; set; } = false;
        public int AssociatedItem { get; set; } = 0;


        public double Timer { get; set; } = 0;
        public int CurrentFrame { get; set; } = 0;
        public int TotalFrames { get; set; } = 0;
        public int AddAmount { get; set; } = 0;
        public double Speed { get; set; } = 0;

        public int Probability { get; set; } = 1;

        public bool HasSound { get; set; } = false;

        [XmlIgnore]
        public Color TileColor { get; set; } = Color.White;
        public float ColorMultiplier { get; set; } = 1;

        public bool IsTemporary { get; set; } = false;

        public bool IsPortal { get; set; } = false;
        public string portalDestination { get; set; } = "none";

        public bool Dirt { get; set; } = false;
        public bool Grass { get; set; } = false;
        public bool Stone { get; set; } = false;
        public bool Diggable { get; set; } = false;
        public bool RedRuneStone { get; set; } = false;


        //[XmlIgnore]
        //public Dictionary<string, bool> Properties;
        [XmlArray("TileProperties")]
        public List<string> TileProperties;

       // public List<Tile> AssociatedTiles;

        //  public bool WasJustReplaced { get; set; } = false;

        //--------------------------------------
        //Rectangles
        public Rectangle SourceRectangle;
        public Rectangle DestinationRectangle;

        //objectgroup stuff
        public bool HasObject { get; set; } = false;
        public ObjectBody TileObject { get; set; }


        private Tile()
        {

        }

        public Tile(float x, float y, int gID, int tilesetTilesWide, int tilesetTilesHigh, int mapWidth, int mapHeight, int tileNumber)
        {
            this.TileNumber = tileNumber;

            this.OldX = x;
            this.OldY = y;
            
            this.GID = gID;
            this.TilesetTilesWide = tilesetTilesWide;
            this.TilesetTilesHigh = tilesetTilesHigh;
            this.MapWidth = mapWidth;
            this.MapHeight = mapHeight;

            TileFrame = GID;

            Column = TileFrame % tilesetTilesWide;
            Row = (int)Math.Floor((double)TileFrame / (double)tilesetTilesWide);

            this.X = (x % mapWidth) * TileWidth;
            this.Y = (y % mapHeight) * TileHeight;

            SourceRectangle = new Rectangle(TileWidth * Column, TileHeight * Row, TileWidth, TileHeight);
            DestinationRectangle = new Rectangle((int)X, (int)Y, TileWidth, TileHeight);

            TileColor = Color.White * ColorMultiplier;

            //Properties = new Dictionary<string, bool>();
            TileProperties = new List<string>();
            //AssociatedTiles = new List<Tile>();

        }

        public void Animate(GameTime gameTime, int totalFramesX, double speed)
        {

            if (DelayTimer <= 0)
            {

                Timer -= gameTime.ElapsedGameTime.TotalSeconds;

                if (Timer <= 0)
                {
                    Timer = speed;
                    CurrentFrame++;
                    AddAmount += 16;
                }

                if (CurrentFrame == totalFramesX)
                {
                    CurrentFrame = 0;
                    if (KillAnimation == true)
                    {
                        IsFinishedAnimating = true;
                    }
                    else
                    {
                        AddAmount = 0;

                    }
                }
                SourceRectangle = new Rectangle(TileWidth * Column + AddAmount, TileHeight * Row, TileWidth, TileHeight);
            }
            else
            {
                DelayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
