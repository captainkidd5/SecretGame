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

        public ClothingPiece(GraphicsDevice graphics,Texture2D texture,Color color)
        {
            this.Graphics = graphics;
            this.Texture = texture;
            this.Color = color;
        }

        public void Update(GameTime gameTime, Vector2 position)
        {
            this.Position = position;
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(this.Texture, this.Position, this.SourceRectangle, this.Color, 0f, Game1.Utility.Origin,1f, SpriteEffects.None, layerDepth);
        }
    }
}
