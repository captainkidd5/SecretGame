using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Physics;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;

namespace SecretProject.Class.CollisionDetection
{


    public class CircleCollider : ICollidable
    {
        public HitBoxType HitBoxType { get; set; }
        protected Texture2D rectangleTexture;

        public bool ShowRectangle { get; set; }
        //0 doesnt check for collisions with other objects, 1 does (player, npcs, moving stuff etc)




        public Rectangle Rectangle { get; set; }


        public ColliderType ColliderType { get; set; }
        public IEntity Entity { get; set; }


        public Circle Circle;


        public CircleCollider(GraphicsDevice graphicsDevice, Rectangle rectangle, Circle circle, IEntity entity, ColliderType colliderType = ColliderType.inert)
        {
            this.HitBoxType = HitBoxType.Circle;
            this.Circle = circle;
            UpdateRectangleFromNewCircle();

            this.ColliderType = colliderType;

            //  SetRectangleTexture(graphicsDevice);

            this.ShowRectangle = true;
            this.Entity = entity;

        }

        /// <summary>
        /// Create rectangle around circle
        /// </summary>
        private void UpdateRectangleFromNewCircle()
        {
            this.Rectangle = new Rectangle((int)(this.Circle.Center.X - this.Circle.Radius), (int)(this.Circle.Center.Y - this.Circle.Radius),
                (int)this.Circle.Diameter(), (int)this.Circle.Diameter());
        }



        //SINGULAR

        public bool IsIntersecting(ICollidable objectBody)
        {
            if (this.Rectangle.Intersects(objectBody.Rectangle))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DidCollide(Vector2 velocity, ICollidable objectBody)
        {
            Rectangle rect = this.Rectangle;

            rect.X += (int)velocity.X;
            rect.Y += (int)velocity.Y;

            if (rect.Intersects(objectBody.Rectangle))
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        private float GetBounceAngle(Circle other)
        {
            return (float)Math.Atan2(other.Center.Y - this.Circle.Center.Y, other.Center.X - this.Circle.Center.X);
        }

        private float GetBounceDistance(Circle other)
        {

            return this.Circle.Radius + other.Radius - this.Circle.GetDistanceBetweenCircleEdges(other);
        }

        /// <summary>
        /// Recalculates this collider's circle position after a collision has occurred with another circle. Repositions the outer rectangle as well.
        /// </summary>
        /// <param name="otherCircle"></param>
        private void CalculateNewCircleCenter(Circle otherCircle)
        {
            float bounceDistance = GetBounceDistance(otherCircle);
            float bounceAngle = GetBounceAngle(otherCircle);
            float newDistance = (float)Math.Cos(bounceAngle) * bounceDistance;
            Circle.Center = new Vector2(Circle.Center.X + newDistance, Circle.Center.Y + newDistance);
            UpdateRectangleFromNewCircle();
        }

        public bool HandleMove(Vector2 callPosition, ref Vector2 moveAmount, ICollidable objectBody)
        {
            bool didEitherCollide = false;
            Vector2 newMove = Vector2.Zero;
            if (objectBody.HitBoxType == HitBoxType.Circle)
            {
                Circle otherCircle = (objectBody as CircleCollider).Circle;
                if (IsIntersecting(objectBody))
                {
                    UpdateCirclePosition();
                    if (this.Circle.IntersectsCircle(otherCircle))
                    {
                        Console.WriteLine("circle intersects!");
                        CalculateNewCircleCenter(otherCircle);
                        Game1.Player.Position = this.Circle.Center;
                        return true; //is intersecting outer renctangle AND inner circle forces a move of 
                        //this collider and repositions the circle center as well as the outer rectangle
                    }
                    return false; //is intersecting outer rectangle but not circle does not count.
                }
                return false; //neither intersecting outer rectangle nor inner circle.
            }
            else //handle collision normally
            {



                //Check collision in X direction
                if (moveAmount.X != 0f)
                {
                    newMove.Y = 0;
                    newMove.X = moveAmount.X;

                    bool collided = DidCollide(newMove, objectBody);
                    if (collided)
                    {
                        moveAmount = new Vector2(0, moveAmount.Y);
                        didEitherCollide = true;
                    }
                }

                //Check collision in Y direction
                if (moveAmount.Y != 0f)
                {
                    newMove.Y = moveAmount.Y;
                    newMove.X = 0;

                    bool collided = DidCollide(newMove, objectBody);
                    if (collided)
                    {
                        moveAmount = new Vector2(moveAmount.X, 0);
                        didEitherCollide = true;
                    }
                }
                return didEitherCollide;
            }
        }



        private void SetCircleTexture(GraphicsDevice graphicsDevice)
        {

        }


        public void Update(Vector2 entityPosition)
        {
            this.Rectangle = new Rectangle((int)entityPosition.X, (int)entityPosition.Y, this.Rectangle.Width, this.Rectangle.Height);
        }

        private void UpdateCirclePosition()
        {
            this.Circle.Center = new Vector2(this.Rectangle.X + this.Rectangle.Width / 2, this.Rectangle.Y + this.Rectangle.Height / 2);
        }



        public void DrawDebug(SpriteBatch spriteBatch)
        {
            Vector2 drawPosition = new Vector2(Circle.Center.X - this.Circle.Radius / 2, Circle.Center.Y + this.Circle.Radius / 2);
            spriteBatch.Draw(this.Circle.DebugTexture, new Vector2(Circle.Center.X - this.Circle.Radius/2, Circle.Center.Y + this.Circle.Radius/2), color: Color.White * .5f, layerDepth: 1f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Center = " + Circle.Center.X + "," + Circle.Center.Y + "\n" + "Radius = " + Circle.Radius, new Vector2(drawPosition.X, drawPosition.Y - 64),
                Color.White, 0f, Game1.Utility.Origin, 1.25f, SpriteEffects.None, Utility.StandardTextDepth);

            
        }

    }
}