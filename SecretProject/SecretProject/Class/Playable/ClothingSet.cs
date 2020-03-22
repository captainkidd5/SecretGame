using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable
{
 
    public class ClothingSet
    {
        public GraphicsDevice Graphics { get; set; }
        public ClothingType ClothingType { get; set; }
        public List<ClothingPiece> Pieces { get; set; }

        public  Texture2D Texture { get; set; }
        public float LayerDepth { get; set; }

        public ClothingSet(GraphicsDevice graphics, ClothingType clothingType, Texture2D texture, float layerDepth)
        {
            this.Graphics = graphics;
            this.ClothingType = clothingType;
            this.Texture = texture;
            this.LayerDepth = layerDepth;
        }

        public void Update(GameTime gameTime, Vector2 position)
        {
            for(int i =0; i < Pieces.Count; i++)
            {
                Pieces[i].Update(gameTime, position);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Pieces.Count; i++)
            {
                Pieces[i].Draw(spriteBatch, this.LayerDepth);
            }
        }
    }
}
