using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable
{
    public enum ClothingType
    {
        Hair = 1,
        Shirt = 2,
        Pants = 3,
        Shoes = 4,
        Base = 5
    }
    public class ClothingSet
    {
        public GraphicsDevice Graphics { get; set; }
        public ClothingType ClothingType { get; set; }
        public List<ClothingPiece> Pieces { get; set; }

        public ClothingSet(GraphicsDevice graphics, ClothingType clothingType)
        {
            this.Graphics = graphics;
            this.ClothingType = clothingType;
        }

        public void Update(GameTime gameTime)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
