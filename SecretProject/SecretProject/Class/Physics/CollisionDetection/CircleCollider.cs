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
        private bool DidCollideRectangle(Vector2 velocity, ICollidable objectBody)
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
                    bool hasIntersected = false;
                    if (this.Circle.IntersectsCircle(otherCircle))
                    {

                        Vector2 tangent = Circle.GetTangent(otherCircle);
                        if (tangent == Vector2.Zero)
                        {
                            tangent = new Vector2(.000001f, .000001f);
                        }
                        else if (tangent == Vector2.One)
                        {
                            Console.WriteLine("oops");
                        }
                        tangent.Normalize();

                        //for the scenario that collision is exactly perpendicular. 
                        if (tangent.X == -1)
                        {
                            tangent.X = -.5f;
                        }
                        else if (tangent.X == 1)
                        {
                            tangent.X = .5f;
                        }
                        if (tangent.Y == -1)
                        {
                            tangent.Y = -.5f;
                        }
                        else if (tangent.Y == 1)
                        {
                            tangent.Y = .5f;
                        }

                        float slideDirection = moveAmount.X * tangent.X + moveAmount.Y * tangent.Y;
                        if (slideDirection >= 0)
                        {
                            moveAmount += tangent;
                        }
                        else
                        {
                            moveAmount -= tangent;
                        }
                        hasIntersected = true;

                       //is intersecting outer renctangle AND inner circle forces a move of 
                        //this collider and repositions the circle center as well as the outer rectangle
                    }
                    if(hasIntersected)
                    {
                        return true;
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

                    bool collided = DidCollideRectangle(newMove, objectBody);
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

                    bool collided = DidCollideRectangle(newMove, objectBody);
                    if (collided)
                    {
                        moveAmount = new Vector2(moveAmount.X, 0);
                        didEitherCollide = true;
                    }
                }
                return didEitherCollide;
            }
        }



        public void Update(Rectangle rectangle)
        {
            this.Rectangle = rectangle;
        }

        /// <summary>
        /// Updates circle to be centered in rectangle.
        /// </summary>
        private void UpdateCirclePosition()
        {
            this.Circle.Center = new Vector2(this.Rectangle.X + this.Rectangle.Width / 2, this.Rectangle.Y + this.Rectangle.Height / 2);
        }



        public void DrawDebug(SpriteBatch spriteBatch)
        {
            Vector2 drawPosition = new Vector2(Circle.Center.X - this.Circle.Radius / 2, Circle.Center.Y + this.Circle.Radius / 2);
            spriteBatch.Draw(this.Circle.DebugTexture, new Vector2(Circle.Center.X - this.Circle.Radius / 2, Circle.Center.Y - this.Circle.Radius / 2), color: Color.White * .5f, layerDepth: 1f);

            float length = (Game1.Player.MainCollider.Circle.Center - this.Circle.Center).Length();
            spriteBatch.DrawString(Game1.AllTextures.MenuText, "Center = " + Circle.Center.X + "," + Circle.Center.Y + "\n" +
                "Radius = " + Circle.Radius +
                "\n DistanceToPlayer = " + length, new Vector2(drawPosition.X, drawPosition.Y - 8),
                Color.White, 0f, Game1.Utility.Origin, .25f, SpriteEffects.None, Utility.StandardTextDepth);


        }

    }
}