using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Physics;
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
            Circle.Center = new Vector2(newDistance, newDistance);
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

                    if (this.Circle.IntersectsCircle(otherCircle))
                    {
                        CalculateNewCircleCenter(otherCircle);
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




        private void SetRectangleTexture(GraphicsDevice graphicsDevice)
        {
            var Colors = new List<Color>();
            for (int y = 0; y < this.Rectangle.Height; y++)
            {
                for (int x = 0; x < this.Rectangle.Width; x++)
                {
                    if (x == 0 || //left side
                        y == 0 || //top side
                        x == this.Rectangle.Width - 1 || //right side
                        y == this.Rectangle.Height - 1) //bottom side
                    {
                        Colors.Add(new Color(255, 255, 255, 255));
                    }
                    else
                    {
                        Colors.Add(new Color(0, 0, 0, 0));

                    }

                }
            }
            rectangleTexture = new Texture2D(graphicsDevice, this.Rectangle.Width, this.Rectangle.Height);
            rectangleTexture.SetData<Color>(Colors.ToArray());
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(rectangleTexture, new Vector2(this.Rectangle.X, this.Rectangle.Y), Color.White);




        }
        public virtual void Draw(SpriteBatch spriteBatch, float layerDepth)
        {

            //  spriteBatch.Draw(rectangleTexture, new Vector2(this.Rectangle.X, this.Rectangle.Y), color: Color.White, layerDepth: layerDepth);

        }

    }
}