using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable
{
    public class ClothingPiece
    {
        public Texture2D Texture { get; set; }
        public GraphicsDevice Graphics { get; set; }
        public Rectangle SourceRectangle { get; set; }

        public Vector2 Position { get; set; }

        public Color Color { get; set; }

        public int OldFrame { get; set; }
        public int Row { get; set; }
        public float LayerDepth { get; set; }

        public ClothingPiece(GraphicsDevice graphics,Texture2D texture,Color color, int row, float layerDepth)
        {
            this.Graphics = graphics;
            this.Texture = texture;
            this.Color = color;
            this.Row = row;
            this.LayerDepth = layerDepth;
        }

        public void Update(GameTime gameTime, Vector2 position, int currentFrame, int xAdjustment = 0, int yAdjustment = 0)
        {
            if(this.OldFrame != currentFrame)
            {
                UpdateSourceRectangle(currentFrame,xAdjustment, yAdjustment);
            }
            this.Position = position;

            this.OldFrame = currentFrame;
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, this.SourceRectangle, this.Color, 0f, Game1.Utility.Origin,1f, SpriteEffects.None, this.LayerDepth);
        }

        public void UpdateSourceRectangle(int currentFrame, int xAdjustment = 0, int yAdjustment = 0)
        {
            this.SourceRectangle = new Rectangle(currentFrame * 16 + xAdjustment, this.Row * 16 + yAdjustment, 16, 16);
        }
    }
}
