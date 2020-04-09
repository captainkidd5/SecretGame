using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Playable.WardrobeStuff.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable.WardrobeStuff.AnimationSetStuff
{
    public class ChoppingSet : AnimationSet
    {
        public ChoppingSet(GraphicsDevice graphics, List<ClothingPiece> clothingPieces, int totalFrames, float speed) : base( graphics, clothingPieces, totalFrames, speed)
        {

        }

        public override void Update(GameTime gameTime, Vector2 position, Dir direction, bool isMoving)
        {

            RunTimer(gameTime);
            if (!isMoving)
            {
                CurrentFrame = 0;
            }
            for (int i = 0; i < Pieces.Count; i++)
            {
                Pieces[i].UpdateChopping(gameTime, position, this.CurrentFrame, direction);
            }
        }
    }
}
