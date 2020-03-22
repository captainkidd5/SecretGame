using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable.WardrobeStuff.AnimationSetStuff
{
 
    public class AnimationSet
    {
        public GraphicsDevice Graphics { get; set; }
        public List<ClothingPiece> Pieces { get; set; }
        public int CurrentFrame { get; set; }
        public int TotalFrames { get; set; }
        public float AnimationSpeed { get; set; }
        public SimpleTimer AnimationTimer { get; set; }


        public AnimationSet(GraphicsDevice graphics, List<ClothingPiece> clothingPieces)
        {
            this.Graphics = graphics;
            this.Pieces = clothingPieces;

            this.Pieces = new List<ClothingPiece>();

            AnimationTimer = new SimpleTimer(.15f);
        }

        public virtual void Update(GameTime gameTime, Vector2 position)
        {

            RunTimer(gameTime);
            for(int i =0; i < Pieces.Count; i++)
            {
                Pieces[i].Update(gameTime, position, this.CurrentFrame);
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Pieces.Count; i++)
            {
                Pieces[i].Draw(spriteBatch);
            }
        }

       public virtual void RunTimer(GameTime gameTime)
        {
            if (AnimationTimer.Run(gameTime))
            {
                this.CurrentFrame ++;
                if (this.CurrentFrame >= this.TotalFrames)
                {
                    this.CurrentFrame = 0;
                }
            }
        }
    }
}
