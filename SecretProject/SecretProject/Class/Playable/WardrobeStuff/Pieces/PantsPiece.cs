using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SavingStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable.WardrobeStuff
{
    public class PantsPiece : IClothing, ISaveable
    {
        public Texture2D Texture { get; set; }

        public int Row { get; set; }
        public int Column { get; set; }

        public Vector2 Position { get; set; }
        public Rectangle SourceRectangle { get; set; }

        public int OldFrame { get; set; }
        public Color Color { get; set; }
        public float LayerDepth { get; set; }

        public SpriteEffects SpriteEffects { get; set; }

        public PantsPiece()
        {
            this.Texture = Game1.AllTextures.PantsAtlas;
            this.Color = Color.White;
            this.LayerDepth = .00000009f;
            this.SpriteEffects = SpriteEffects.None;

        }
        public void Update(GameTime gameTime, Vector2 position, int currentFrame, Dir direction)
        {

            if (this.OldFrame != currentFrame)
            {
                switch (direction)
                {
                    case Dir.Down:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdateDown(currentFrame);
                        break;
                    case Dir.Up:
                        this.SpriteEffects = SpriteEffects.None;
                        UpdateUp(currentFrame);
                        break;
                    case Dir.Left:
                        this.SpriteEffects = SpriteEffects.FlipHorizontally;
                        UpdateRight(currentFrame);
                        break;
                    case Dir.Right:
                        this.SpriteEffects = SpriteEffects.None;
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
            switch (currentFrame)
            {
                case 0:
                    break;
                case 1:
                    xAdjustment = 16;
                    break;

                case 2:
                    xAdjustment = 32;
                    break;
                case 3:
                    break;
                case 4:
                    xAdjustment = 16;
                    break;
                case 5:
                    xAdjustment = 32;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        public void UpdateUp(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 9;

            switch (currentFrame)
            {
                case 0:
                    break;
                case 1:
                    xAdjustment = 16;
                    break;

                case 2:
                    xAdjustment = 32;
                    break;
                case 3:
                    break;
                case 4:
                    xAdjustment = 16;
                    break;
                case 5:
                    xAdjustment = 32;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }

        public void UpdateRight(int currentFrame)
        {
            int xAdjustment = 0;
            int yAdjustment = 0;
            int column = 3;

            switch (currentFrame)
            {
                case 0:
                    break;
                case 1:
                    xAdjustment = 16;
                    break;

                case 2:
                    xAdjustment = 32;
                    break;
                case 3:
                    xAdjustment = 48;
                    break;
                case 4:
                    xAdjustment = 64;
                    break;
                case 5:
                    xAdjustment = 80;
                    break;
            }
            UpdateSourceRectangle(column, xAdjustment, yAdjustment);
        }
        #endregion


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, new Vector2(this.Position.X, this.Position.Y + 18), this.SourceRectangle, this.Color, 0f, Game1.Utility.Origin, 1f, this.SpriteEffects, .5f + this.LayerDepth);
        }

        public void UpdateSourceRectangle(int column, int xAdjustment = 0, int yAdjustment = 0)
        {
            this.SourceRectangle = new Rectangle(column * 16 + xAdjustment, this.Row * 16 + yAdjustment , 16, 16);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(this.Row);
        }

        public void Load(BinaryReader reader)
        {
            this.Row = reader.ReadInt32();
        }
    }
}
