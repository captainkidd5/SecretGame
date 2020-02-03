using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;

namespace SecretProject.Class.CollisionDetection.ProjectileStuff
{

    public class Projectile : IEntity
    {
        public GraphicsDevice Graphics { get; private set; }
        public Vector2 CurrentPosition { get; set; }
        public Vector2 PositionToMoveTowards { get; set; }
        public Vector2 DirectionVector { get; set; }
        public Vector2 PrimaryVelocity { get; set; }

        public float Rotation { get; private set; }
        public float Speed { get; private set; }

        public Rectangle SourceRectangle { get; private set; }
        public Collider Collider { get; set; }

        public List<Projectile> AllProjectiles { get; set; }

        public Projectile(GraphicsDevice graphics, Vector2 startPosition, float rotation, float speed, Vector2 positionToMoveToward, List<Projectile> allProjectiles)
        {
            this.Graphics = graphics;
            this.CurrentPosition = startPosition;
            this.PositionToMoveTowards = positionToMoveToward;
            this.DirectionVector = Vector2.Zero;
            this.Rotation = rotation;
            this.Speed = speed;
            this.PrimaryVelocity = new Vector2(Speed, Speed);
            this.Collider = new Collider(this.Graphics, new Rectangle((int)startPosition.X, (int)startPosition.Y, 4, 4), this, ColliderType.Projectile);
            this.AllProjectiles = allProjectiles;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void MouseCollisionInteraction()
        {
            throw new NotImplementedException();
        }

        public void PlayerCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public bool MoveTowardsPoint(Vector2 goal, GameTime gameTime)
        {
            // If we're already at the goal return immediatly

            if (this.CurrentPosition == goal) return true;

            // Find direction from current position to goal
            Vector2 direction = Vector2.Normalize(goal - this.CurrentPosition);
            this.DirectionVector = direction;
            // Move in that direction
            this.CurrentPosition += direction * this.PrimaryVelocity;// * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If we moved PAST the goal, move it back to the goal
            if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - this.CurrentPosition)) + 1) < 0.1f)
                this.CurrentPosition = goal;

            // Return whether we've reached the goal or not
            return this.CurrentPosition == goal;
        }
    }
}
