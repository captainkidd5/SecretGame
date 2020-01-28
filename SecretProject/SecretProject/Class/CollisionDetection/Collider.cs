using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using System;
using System.Collections.Generic;

namespace SecretProject.Class.CollisionDetection
{


    public class Collider : ICollidable
    {
        protected Texture2D rectangleTexture;

        public bool ShowRectangle { get; set; }
        //0 doesnt check for collisions with other objects, 1 does (player, npcs, moving stuff etc)
        private Rectangle rectangle;



        public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }


        public ColliderType ColliderType { get; set; }
        public string LocationKey { get; set; }
        public bool IsUpdating { get; set; }
        public Dir InitialShuffDirection { get; set; }
        public IEntity Entity { get; set; }

        private Collider()
        {

        }

        public Collider(GraphicsDevice graphicsDevice, Rectangle rectangle, IEntity entity, ColliderType colliderType = ColliderType.inert)
        {
            this.rectangle = rectangle;
            this.ColliderType = colliderType;

            SetRectangleTexture(graphicsDevice);

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
            spriteBatch.Draw(rectangleTexture, new Vector2(this.Rectangle.X, this.Rectangle.Y), Color.White);




        }
        public virtual void Draw(SpriteBatch spriteBatch, float layerDepth)
        {

            spriteBatch.Draw(rectangleTexture, new Vector2(this.Rectangle.X, this.Rectangle.Y), color: Color.White, layerDepth: layerDepth);

        }

        public void Update(GameTime gameTime, Dir direction)
        {
            throw new NotImplementedException();
        }

        public void Shuff(GameTime gameTime, int direction)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void SelfDestruct()
        {
            throw new NotImplementedException();
        }
    }
}