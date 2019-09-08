﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.SpriteFolder
{
    public class GrassTuft
    {
        public int GrassType { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle DestinationRectangle { get; set; }
        public float Rotation { get; set; }
        public float RotationCap { get; set; }
        public bool HasShuffed { get; set; }
        public float ShuffSpeed { get; set; }
        public GrassTuft(int grassType,Vector2 position)
        {
            this.GrassType = grassType;
            this.Position = position;
            this.DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, 16, 16);
            this.Rotation = 0f;
            this.RotationCap = .25f;
            this.ShuffSpeed = 2f;
            this.HasShuffed = false;
        }
        public void Update(GameTime gameTime)
        {
            if(this.HasShuffed)
            {
                RotateBackToOrigin(gameTime);
            }
            
            if(Game1.Player.Rectangle.Intersects(DestinationRectangle))
                {
                   Shuff(gameTime, (int)Game1.Player.controls.Direction);
                }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
         
            switch (GrassType)
            {
                case 1:
                    spriteBatch.Draw(Game1.AllTextures.TallGrass, DestinationRectangle, new Rectangle(0, 0, 16, 16),
                        Color.White, Rotation, new Vector2(8,8), SpriteEffects.None, .5f + (DestinationRectangle.Top + DestinationRectangle.Height) * .00001f);
                    break;
                case 2:
                    spriteBatch.Draw(Game1.AllTextures.TallGrass, DestinationRectangle, new Rectangle(16, 0, 16, 16),
                        Color.White, Rotation, new Vector2(8, 8), SpriteEffects.None, .5f + (DestinationRectangle.Top + DestinationRectangle.Height) * .00001f);
                    break;
                case 3:
                    spriteBatch.Draw(Game1.AllTextures.TallGrass, DestinationRectangle, new Rectangle(32, 0, 16, 16),
                        Color.White, Rotation, new Vector2(8, 8), SpriteEffects.None, .5f + (DestinationRectangle.Top + DestinationRectangle.Height) * .00001f);
                    break;
            }
        }

        public void Shuff(GameTime gameTime, int direction)
        {
            if(direction == (int)Dir.Right)
            {
                if (this.Rotation < RotationCap)
                {
                    this.Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds * ShuffSpeed;
                    this.HasShuffed = false;
                }
                else
                {
                    this.HasShuffed = true;
                }
                
            }
            else if (direction == (int)Dir.Left)
            {
                if (this.Rotation > RotationCap - 1)
                {
                    this.Rotation -= (float)gameTime.ElapsedGameTime.TotalSeconds * ShuffSpeed;
                    this.HasShuffed = false;
                }
                else
                {
                    this.HasShuffed = true;
                }
            }
            
            
        }

        public void RotateBackToOrigin(GameTime gameTime)
        {
            if (this.Rotation > 0 )
            {
                this.Rotation -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if(Rotation < 0)
            {
                this.Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                this.HasShuffed = false;
            }
        }
    }
}
