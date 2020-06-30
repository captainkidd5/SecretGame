using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.Universal;

namespace SecretProject.Class.CollisionDetection.ProjectileStuff
{

    public class Projectile : IEntity
    {
        public GraphicsDevice Graphics { get; private set; }
        public Collider ColliderFiredFrom { get; set; }
 
        public Vector2 CurrentPosition { get; set; }
        public Vector2 PositionToMoveTowards { get; set; }
        public Vector2 DirectionVector { get; set; }
        public Vector2 PrimaryVelocity { get; set; }

        public Dir DirectionFiredFrom { get; set; }

        public float Rotation { get; private set; }
        public float Speed { get; private set; }

        public Rectangle SourceRectangle { get; set; }
        public Collider Collider { get; set; }

        public List<Projectile> AllProjectiles { get; set; }

        public bool DamagesPlayer { get; set; }
        public SoundEffect MissSound { get; set; }

        SimpleTimer TimeToLive;

        public int DamageValue { get; set; }
        public Projectile(GraphicsDevice graphics, Collider colliderFiredFrom, Dir directionFiredFrom, Vector2 startPosition, float rotation, float speed, Vector2 positionToMoveToward, List<Projectile> allProjectiles, bool damagesPlayer, int damage)
        {
            this.Graphics = graphics;
            this.ColliderFiredFrom = colliderFiredFrom;
            this.DirectionFiredFrom = directionFiredFrom;
            this.CurrentPosition = startPosition;
            this.PositionToMoveTowards = positionToMoveToward;
            this.DirectionVector = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            this.DirectionVector.Normalize();
            this.Rotation = rotation;
            this.Speed = speed;
            this.PrimaryVelocity = new Vector2(Speed, Speed);
            this.Collider = new Collider(this.Graphics, new Rectangle((int)startPosition.X, (int)startPosition.Y, 4, 4), this, ColliderType.Projectile);
            this.AllProjectiles = allProjectiles;
            this.DamagesPlayer = damagesPlayer;
            this.SourceRectangle = Game1.ItemVault.GenerateNewItem(280, null).SourceTextureRectangle;

            this.TimeToLive = new SimpleTimer(8f);
            this.MissSound = Game1.SoundManager.ArrowMiss;
            this.DamageValue = damage;
        }

        public virtual void Update(GameTime gameTime)
        {
            this.Speed -= .01f;
            if (this.Speed <= 0)
            {
                this.Speed = 0;
                // Game1.CurrentStage.AllTiles.AddItem(Game1.ItemVault.GenerateNewItem(280, this.CurrentPosition, true, Game1.CurrentStage.AllTiles.GetItems(this.CurrentPosition)), this.CurrentPosition);
                Game1.CurrentStage.AllProjectiles.Remove(this);
                return;
            }
            this.PrimaryVelocity = new Vector2(Speed, Speed);
            this.Collider.Rectangle = new Rectangle((int)this.CurrentPosition.X, (int)this.CurrentPosition.Y, 4, 4);
            List<ICollidable> returnObjects = new List<ICollidable>();
            Game1.CurrentStage.QuadTree.Retrieve(returnObjects, this.Collider);
            for (int i = 0; i < returnObjects.Count; i++)
            {

                if (returnObjects[i].ColliderType == ColliderType.PlayerMainCollider)
                {

                    
                    if (DamagesPlayer)
                    {
                        if (this.Collider.Rectangle.Intersects(Game1.Player.MainCollider.Rectangle))
                        {
                            Game1.Player.TakeDamage(this.DamageValue);
                            this.AllProjectiles.Remove(this);
                        }
                    }
                }

                if (returnObjects[i].ColliderType == ColliderType.Enemy)
                {
                    if (this.Collider.IsIntersecting(returnObjects[i]))
                    {

    
                        if ((returnObjects[i].Rectangle != this.ColliderFiredFrom.Rectangle))
                        {
                            returnObjects[i].Entity.DamageCollisionInteraction(this.DamageValue, 5, this.DirectionFiredFrom);
                            this.AllProjectiles.Remove(this);
                            return;
                        }


                    }
                }
                else if (returnObjects[i].ColliderType == ColliderType.inert)
                {
                    if (this.Collider.IsIntersecting(returnObjects[i]))
                    {
                        Miss();
                        this.AllProjectiles.Remove(this);
                        return;

                    }
                }

                else
                {


                }




            }

            this.CurrentPosition += this.DirectionVector * (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;

            if(TimeToLive.Run(gameTime))
            {
                this.AllProjectiles.Remove(this);
                return;
            }

        }

        public virtual void Miss()
        {
            Game1.SoundManager.PlaySoundEffect(this.MissSound, true, .15f);
            Game1.CurrentStage.ParticleEngine.ActivationTime = .05f;
            Game1.CurrentStage.ParticleEngine.EmitterLocation = this.CurrentPosition;
            Game1.CurrentStage.ParticleEngine.Color = Color.White;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.ItemSpriteSheet, this.CurrentPosition, this.SourceRectangle, Color.White, this.Rotation + 2.4f, Game1.Utility.Origin, 1f, SpriteEffects.None, 1f);
        }

        public void MouseCollisionInteraction()
        {
            throw new NotImplementedException();
        }

        public void DamageCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        //for homing arrows?
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
