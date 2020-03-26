using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable.WardrobeStuff
{
    public class HairPiece : IClothing
    {
        public Texture2D Texture{ get; set; }

        public int Row { get; set; }
        public int Column { get; set; }

        public Vector2 Position { get; set; }
        public Rectangle SourceRectangle { get; set; }

        public int OldFrame { get; set; }
        public Color Color { get; set; }
        public float LayerDepth { get; set; }

        public SpriteEffects SpriteEffects { get; set; }

        public HairPiece()
        {
            this.Texture = Game1.AllTextures.HairAtlas;
            this.Color = Color.White;
            this.LayerDepth = 00000011f;
            this.SpriteEffects = SpriteEffects.None;

        }
        public void Update(GameTime gameTime, Vector2 position, int currentFrame, Dir direction)
        {
            if (this.OldFrame != currentFrame)
            {
                switch (direction)
                {
                    case Dir.Down:
                        UpdateDown(currentFrame);
                        break;
                    case Dir.Up:
                        UpdateUp(currentFrame);
                        break;
                    case Dir.Left:
                        UpdateLeft(currentFrame);
                        break;
                    case Dir.Right:
                        UpdateRight(currentFrame);
                        break;

                }

               
            }
            this.Position = position;

            this.OldFrame = currentFrame;

        }
        #region DIRECTION UPDATES
        public void UpdateDown(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 0;
            switch(currentFrame)
            {
                case 0:
                    break;
                case 1:
                    yAdjustment = -1;
                    break;

                case 2:
                    yAdjustment = -1;
                    break;
                case 3:
                    break;
                case 4:
                    yAdjustment = -1;
                    break;
                case 5:
                    yAdjustment = -1;
                    break;
            }
            UpdateSourceRectangle(currentFrame);
        }
        public void UpdateUp(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
        }
        public void UpdateLeft(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
        }
        public void UpdateRight(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
        }
        #endregion


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, this.SourceRectangle, this.Color, 0f, Game1.Utility.Origin, 1f, this.SpriteEffects, this.LayerDepth);
        }

        public void UpdateSourceRectangle(int column, int xAdjustment = 0, int yAdjustment = 0)
        {
            this.SourceRectangle = new Rectangle(column * 16 + xAdjustment, this.Row * 16 + yAdjustment, 16, 16);
        }
    }
}
