﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.UI.MainMenuStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable.WardrobeStuff.Pieces
{
    public class ClothingPiece : ISaveable
    {
        public Texture2D Texture { get; set; }

        public int Row { get; set; }
        public int Column { get; set; }

        public Vector2 Position { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public float Scale { get; set; }

        public int OldFrame { get; set; }
        
        public float LayerDepth { get; set; }

        public SpriteEffects SpriteEffects { get; set; }
        public int BaseYOffSet { get; set; }

        //COLORS
        public Color Color { get; set; }


        public ClothingPiece(Color defaultColor)
        {
            this.Color = defaultColor;
        }

        public virtual void Cycle(CycleDirection direction)
        {
            this.Row += (int)direction;
            if(Row >= this.Texture.Height / 16 )
            {
                this.Row = 0;
            }
            else if(this.Row <0)
            {
                this.Row = this.Texture.Height / 16 - 1;
            }
        }

        public void Update(GameTime gameTime, Vector2 position, int currentFrame, Dir direction)
        {

            if (this.OldFrame != currentFrame)
            {
                switch (direction)
                {
                    case Dir.Down:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdateWalkDown(currentFrame);
                        break;
                    case Dir.Up:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdateWalkUp(currentFrame);
                        break;
                    case Dir.Left:
                        this.SpriteEffects = SpriteEffects.FlipHorizontally;
                        UpdateWalkRight(currentFrame);
                        break;
                    case Dir.Right:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdateWalkRight(currentFrame);
                        break;

                }


            }
            this.Position = position;

            this.OldFrame = currentFrame;

        }
        #region WALK
        public virtual void UpdateWalkDown(int currentFrame)
        {
            
        }

        public virtual void UpdateWalkUp(int currentFrame)
        {
        }

        public virtual void UpdateWalkRight(int currentFrame)
        {
        }
        #endregion

        #region Chop
        public virtual void UpdateChopDown(int currentFrame)
        {

        }

        public virtual void UpdateChopUp(int currentFrame)
        {
        }

        public virtual void UpdateChopRight(int currentFrame)
        {
        }
        #endregion
        public virtual void Draw(SpriteBatch spriteBatch, float yLayerHeight)
        {
            spriteBatch.Draw(this.Texture, new Vector2(this.Position.X, this.Position.Y + this.BaseYOffSet * this.Scale), this.SourceRectangle, this.Color, 0f, Game1.Utility.Origin, this.Scale, this.SpriteEffects, yLayerHeight + this.LayerDepth);
        }

        public virtual void DrawForCreationWindow(SpriteBatch spriteBatch)
        {
            this.Scale = 6f;
            spriteBatch.Draw(this.Texture, new Vector2(this.Position.X, this.Position.Y + this.BaseYOffSet * this.Scale), this.SourceRectangle, this.Color, 0f, Game1.Utility.Origin, this.Scale, this.SpriteEffects, .9f + this.LayerDepth);
        }


        public void UpdateSourceRectangle(int column, int xAdjustment = 0, int yAdjustment = 0)
        {
            this.SourceRectangle = new Rectangle(column * 16 + xAdjustment, this.Row * 16 + yAdjustment, 16, 16);
        }

        public virtual void ChangeColor(int red, int green, int blue)
        {
            this.Color = new Color(red, green, blue);
        }

       

        public virtual void Load(BinaryReader reader)
        {
            this.Row = reader.ReadInt32();
            this.Color = new Color(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
        }

        public virtual void Save(BinaryWriter writer)
        {
            writer.Write(this.Row);
            writer.Write(this.Color.R);
            writer.Write(this.Color.G);
            writer.Write(this.Color.B);
            
        }
    }
}