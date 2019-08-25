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
    public class SeaTile
    {
        //public float X { get; set; } = 0;
        //public float Y { get; set; } = 0;

        //private int gid;
        //public int GID { get { return gid - 1; } set { gid = value; } }
        //public bool IsSelected { get; set; } = false;

        //public int TilesetTilesWide { get; set; } = 0;
        //public int TilesetTilesHigh { get; set; } = 0;
        //public int TileFrame { get; set; } = 0;
        //public int TileHeight { get; set; } = 16;
        //public int TileWidth { get; set; } = 16;
        //public int Column { get; set; } = 0;
        //public int Row { get; set; } = 0;
        //public float OldY { get => OldY1; set => OldY1 = value; }
        //public float OldY1 { get; set; } = 0;
        //public float OldX { get; set; } = 0;

        //public bool IsAnimated { get; set; } = false;
        //public bool IsAnimating { get; set; } = false;
        //public bool IsFinishedAnimating { get; set; } = false;
        //public bool KillAnimation { get; set; } = false;
        //public float DelayTimer { get; set; } = 0;
        //public bool Kill { get; set; } = true;


        //public double Timer { get; set; } = 0;
        //public int CurrentFrame { get; set; } = 0;
        //public int TotalFramesX { get; set; } = 0;
        //public int TotalFramesY { get; set; } = 0;
        ////AddAmount used for animation frames
        //public int AddAmountX { get; set; } = 0;
        //public int AddAmountY { get; set; } = 0;
        //public double Speed { get; set; } = 0;



        //public Color TileColor { get; set; } = Color.White;

        //public bool IsTemporary { get; set; } = false;

        //public float LayerToDrawAt { get; set; } = 0f;
        //public float LayerToDrawAtZOffSet { get; set; } = 0f;


        //public int HitPoints { get; set; } = 1;
        //public int AStarTileValue { get; set; }


        ////[XmlIgnore]
        ////public Dictionary<string, bool> Properties;


        //// public List<Tile> AssociatedTiles;

        ////  public bool WasJustReplaced { get; set; } = false;

        ////--------------------------------------
        ////Rectangles
        //public Rectangle SourceRectangle;
        //public Rectangle DestinationRectangle;

        ////objectgroup stuff
        //public bool HasObject { get; set; } = false;
        //public ObjectBody TileObject { get; set; }



        //private SeaTile()
        //{

        //}

        //public SeaTile(float x, float y, int gID, int tilesetTilesWide, int tilesetTilesHigh, int mapWidth, int mapHeight)
        //{

        //    this.OldX = x;
        //    this.OldY = y;

        //    this.GID = gID;
        //    this.TilesetTilesWide = tilesetTilesWide;
        //    this.TilesetTilesHigh = tilesetTilesHigh;


        //    TileFrame = GID;

        //    Column = TileFrame % tilesetTilesWide;
        //    Row = (int)Math.Floor((double)TileFrame / (double)tilesetTilesWide);

        //    this.X = (x % mapWidth) * TileWidth;
        //    this.Y = (y % mapHeight) * TileHeight;

        //    SourceRectangle = new Rectangle(TileWidth * Column, TileHeight * Row, TileWidth, TileHeight);
        //    DestinationRectangle = new Rectangle((int)X, (int)Y, TileWidth, TileHeight);

        //    //TileColor = Color.DimGray * 1.5f;
        //    AStarTileValue = 1;

        //}

        //public void AnimateOnlyX(GameTime gameTime, int totalFramesX, double speed)
        //{

        //    if (DelayTimer <= 0)
        //    {

        //        Timer -= gameTime.ElapsedGameTime.TotalSeconds;

        //        if (Timer <= 0)
        //        {
        //            Timer = speed;
        //            CurrentFrame++;
        //            AddAmountX += 16;
        //        }

        //        if (CurrentFrame == totalFramesX)
        //        {
        //            CurrentFrame = 0;
        //            if (KillAnimation == true)
        //            {
        //                IsFinishedAnimating = true;
        //            }
        //            else
        //            {
        //                AddAmountX = 0;

        //            }
        //        }
        //        SourceRectangle = new Rectangle(TileWidth * Column + AddAmountX, TileHeight * Row, TileWidth, TileHeight);
        //    }
        //    else
        //    {
        //        DelayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        //    }
        //}

        //public void AnimateDynamic(GameTime gameTime, int totalFramesX, int totalFramesY, int addAmountX, int addAmountY, double speed, bool kill = true)
        //{
        //    if (DelayTimer <= 0)
        //    {

        //        Timer -= gameTime.ElapsedGameTime.TotalSeconds;

        //        if (Timer <= 0)
        //        {
        //            Timer = speed;
        //            CurrentFrame++;
        //            if (TotalFramesX > 0)
        //            {
        //                AddAmountX += addAmountX;
        //            }
        //            if (totalFramesY > 0)
        //            {
        //                AddAmountY += addAmountY;
        //            }

        //        }

        //        if (totalFramesX > 0 && totalFramesY == 0)
        //        {
        //            if (CurrentFrame == totalFramesX)
        //            {
        //                CurrentFrame = 0;
        //                if (KillAnimation == true && kill == true)
        //                {
        //                    IsFinishedAnimating = true;
        //                }
        //                else
        //                {
        //                    AddAmountX = 0;

        //                }
        //            }
        //        }

        //        else if (totalFramesX == 0 && totalFramesY > 0)
        //        {
        //            if (CurrentFrame == totalFramesY)
        //            {
        //                CurrentFrame = 0;
        //                if (KillAnimation == true && kill == true)
        //                {
        //                    IsFinishedAnimating = true;
        //                }
        //                else
        //                {
        //                    AddAmountY = 0;

        //                }
        //            }
        //        }

        //        else if (CurrentFrame == totalFramesX && CurrentFrame == totalFramesY)
        //        {
        //            CurrentFrame = 0;
        //            //set kill to false if we don't want the animation to ever end
        //            if (KillAnimation == true && kill == true)
        //            {
        //                IsFinishedAnimating = true;
        //            }
        //            else
        //            {
        //                AddAmountX = 0;
        //                AddAmountY = 0;

        //            }
        //        }

        //        if (totalFramesX > 0)
        //        {
        //            SourceRectangle = new Rectangle(TileWidth * Column + AddAmountX, TileHeight * Row, TileWidth, TileHeight);
        //        }
        //        else if (totalFramesY > 0)
        //        {
        //            SourceRectangle = new Rectangle(TileWidth * Column, TileHeight * Row - AddAmountY, TileWidth, TileHeight);
        //        }
        //        else if (totalFramesY > 0 && totalFramesX > 0)
        //        {
        //            SourceRectangle = new Rectangle(TileWidth * Column + AddAmountX, TileHeight * Row + AddAmountY, TileWidth, TileHeight);
        //        }


        //    }
        //    else
        //    {
        //        DelayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        //    }
        //}
    }

}
