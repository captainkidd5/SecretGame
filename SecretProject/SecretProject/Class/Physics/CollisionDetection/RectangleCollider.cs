﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using System;
using System.Collections.Generic;

namespace SecretProject.Class.CollisionDetection
{


    public class RectangleCollider : ICollidable
    {
        public HitBoxType HitBoxType { get; set; }
        protected Texture2D rectangleTexture;

        public bool ShowRectangle { get; set; }
        //0 doesnt check for collisions with other objects, 1 does (player, npcs, moving stuff etc)
        private Rectangle rectangle;



        public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }


        public ColliderType ColliderType { get; set; }
        public bool IsUpdating { get; set; }
        public Dir InitialShuffDirection { get; set; }
        public IEntity Entity { get; set; }

        private RectangleCollider()
        {

        }

        public RectangleCollider(GraphicsDevice graphicsDevice, Rectangle rectangle, IEntity entity, ColliderType colliderType = ColliderType.inert)
        {
            this.HitBoxType = HitBoxType.Rectangle;
            this.rectangle = rectangle;
            this.ColliderType = colliderType;

          //  SetRectangleTexture(graphicsDevice);

            this.ShowRectangle = true;
            this.Entity = entity;
            this.IsUpdating = false;
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

        public bool HandleMove(Vector2 callPosition, ref Vector2 moveAmount, ICollidable objectBody)
        {
            bool didEitherCollide = false;
            Vector2 newMove = Vector2.Zero;
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


        public void Update(Vector2 entityPosition)
        {
            this.Rectangle = new Rectangle((int)entityPosition.X, (int)entityPosition.Y, this.Rectangle.Width, this.Rectangle.Height);
        }

        public void DrawDebug(SpriteBatch spriteBatch)
        {
            
        }
    }
}