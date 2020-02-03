using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.NPCStuff.Enemies;

namespace SecretProject.Class.CollisionDetection.ProjectileStuff
{

    public class Projectile : IEntity
    {
        public GraphicsDevice Graphics { get; private set; }
        public Vector2 CurrentPosition { get; set; }
        public Vector2 PositionToMoveTowards { get; set; }
        public Vector2 DirectionVector { get; set; }
        public Vector2 PrimaryVelocity { get; set; }

        public Dir DirectionFiredFrom { get; set; }

        public float Rotation { get; private set; }
        public float Speed { get; private set; }

        public Rectangle SourceRectangle { get; private set; }
        public Collider Collider { get; set; }

        public List<Projectile> AllProjectiles { get; set; }

        public Projectile(GraphicsDevice graphics, Dir directionFiredFrom, Vector2 startPosition, float rotation, float speed, Vector2 positionToMoveToward, List<Projectile> allProjectiles)
        {
            this.Graphics = graphics;
            this.DirectionFiredFrom = directionFiredFrom;
            this.CurrentPosition = startPosition;
            this.PositionToMoveTowards = positionToMoveToward;
            this.DirectionVector = Vector2.Zero;
            this.Rotation = rotation;
            this.Speed = speed;
            this.PrimaryVelocity = new Vector2(Speed, Speed);
            this.Collider = new Collider(this.Graphics, new Rectangle((int)startPosition.X, (int)startPosition.Y, 4, 4), this, ColliderType.Projectile);
            this.AllProjectiles = allProjectiles;

            this.SourceRectangle = Game1.ItemVault.GenerateNewItem(280, null).SourceTextureRectangle;
        }

        public void Update(GameTime gameTime)
        {
            this.Collider.Rectangle = new Rectangle((int)this.CurrentPosition.X, (int)this.CurrentPosition.Y, 4,4);
            List<ICollidable> returnObjects = new List<ICollidable>();
            Game1.GetCurrentStage().QuadTree.Retrieve(returnObjects, this.Collider);
            for (int i = 0; i < returnObjects.Count; i++)
            {
                //if obj collided with item in list stop it from moving boom badda bing


                if (returnObjects[i].ColliderType == ColliderType.Enemy)
                {
                    if (this.Collider.IsIntersecting(returnObjects[i]))
                    {
                        returnObjects[i].Entity.PlayerCollisionInteraction(1, 5, this.DirectionFiredFrom);

                    }
                }

                else
                {


                }


                // IsMoving = false;



            }
            MoveTowardsPoint(this.PositionToMoveTowards, gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.ItemSpriteSheet, this.CurrentPosition, this.SourceRectangle, Color.White, this.Rotation, Game1.Utility.Origin, 1f, SpriteEffects.None, 1f);
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
            this.CurrentPosition += direction * this.PrimaryVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;// * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If we moved PAST the goal, move it back to the goal
            if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - this.CurrentPosition)) + 1) < 0.1f)
                this.CurrentPosition = goal;

            // Return whether we've reached the goal or not
            return this.CurrentPosition == goal;
        }
    }
}
