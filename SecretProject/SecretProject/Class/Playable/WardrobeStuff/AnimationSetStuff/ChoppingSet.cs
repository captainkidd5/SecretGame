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
        public ToolPiece ToolPiece { get; set; }

        public bool HasGottenDirection { get; set; }
        public Dir Direction { get; set; }

        public ChoppingSet(string name, GraphicsDevice graphics, List<ClothingPiece> clothingPieces, int totalFrames, float speed) : base(name, graphics, clothingPieces, totalFrames, speed)
        {

        }

        public override void Update(GameTime gameTime, Vector2 position, Dir direction, bool isMoving)
        {
            throw new Exception("Chopping set tried to update normally!");
        }

        public override bool UpdateOnce(GameTime gameTime, Vector2 position, Dir direction)
        {
            if(!HasGottenDirection)
            {
                this.Direction = direction;
                HasGottenDirection = true;
            }
            Game1.Player.EnableControls = false;
            RunTimer(gameTime);
            if (CurrentFrame == this.TotalFrames)
            {
                CurrentFrame = 0;
                this.HasGottenDirection = false;
                return true;

            }
            for (int i = 0; i < Pieces.Count; i++)
            {
                Pieces[i].UpdateChopping(gameTime, position, this.CurrentFrame, Direction);
            }
            if(Game1.Player.Wardrobe.ToolPiece != null)
            {
                Game1.Player.Wardrobe.ToolPiece.UpdateChopping(gameTime, position, this.CurrentFrame, Direction);
            }
            
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch, float yLayerHeight)
        {
            base.Draw(spriteBatch, yLayerHeight);

            Game1.Player.Wardrobe.ToolPiece.Draw(spriteBatch, yLayerHeight);
        }
    }
}
