using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable
{
 
    public class AnimationSet
    {
        public GraphicsDevice Graphics { get; set; }
        public List<ClothingPiece> Pieces { get; set; }


        public AnimationSet(GraphicsDevice graphics, List<ClothingPiece> clothingPieces)
        {
            this.Graphics = graphics;
            this.Pieces = clothingPieces;

            this.Pieces = new List<ClothingPiece>();
        }

        public virtual void Update(GameTime gameTime, Vector2 position, int currentFrame)
        {
            for(int i =0; i < Pieces.Count; i++)
            {
                Pieces[i].Update(gameTime, position, currentFrame);
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Pieces.Count; i++)
            {
                Pieces[i].Draw(spriteBatch);
            }
        }

       // public 
    }
}
