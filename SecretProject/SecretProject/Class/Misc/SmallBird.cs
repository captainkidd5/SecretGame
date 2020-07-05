using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Misc
{
    public class SmallBird : FunItems
    {
        private Texture2D Texture { get; set; }
        private Vector2 Position { get; set; }
        private Sprite AnimatedSprite{ get; set; }

        private Vector2 DestinationPosition { get; set; }

        private float Speed { get; set; } = 2f;
        public List<FunItems> FunItems { get; set; }

        public SmallBird(GraphicsDevice graphics, Vector2 position, List<FunItems> funItems)
        {
            this.Texture = Game1.AllTextures.ButterFlys;
            this.Position = position;
            this.AnimatedSprite = new Sprite(graphics, Texture, 0, 32, 16, 16, 2, .25f, Position);

            this.DestinationPosition = new Vector2(1600, 1600);
            this.FunItems = funItems;
        }

        private bool MoveTowardsPoint(Vector2 goal, GameTime gameTime)
        {
            // If we're already at the goal return immediatly
            if (this.Position == goal) return true;

            // Find direction from current position to goal
            Vector2 direction = Vector2.Normalize(goal - this.Position);

            // Move in that direction
            this.Position += direction * this.Speed;

            // If we moved PAST the goal, move it back to the goal
            if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - this.Position)) + 1) < 0.1f)
                this.Position = goal;

            // Return whether we've reached the goal or not
            return this.Position == goal;
        }

        public void Update(GameTime gameTime)
        {
            if(MoveTowardsPoint(this.DestinationPosition, gameTime))
            {
                FunItems.Remove(this);
            }
            this.AnimatedSprite.UpdateAnimations(gameTime, this.Position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.AnimatedSprite.DrawAnimation(spriteBatch, this.Position, .95f);
        }
    }
}
