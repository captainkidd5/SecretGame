using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable.WardrobeStuff.AnimationSetStuff
{
    public class BasicMovementSet : AnimationSet
    {
        public BasicMovementSet(GraphicsDevice graphics, List<ClothingPiece> clothingPieces) : base(graphics, clothingPieces)
        {

        }


        public override void Update(GameTime gameTime, Vector2 position, int currentFrame)
        {
            for (int i = 0; i < Pieces.Count; i++)
            {
                Pieces[i].Update(gameTime, position, currentFrame);
            }
        }
    }
}
