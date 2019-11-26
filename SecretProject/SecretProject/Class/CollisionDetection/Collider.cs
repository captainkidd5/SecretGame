﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SecretProject.Class.SpriteFolder;
using SecretProject.Class.ItemStuff;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;

namespace SecretProject.Class.CollisionDetection
{


    public class Collider : ICollidable
    {
        protected Texture2D rectangleTexture;

        public bool ShowRectangle { get; set; }
        //0 doesnt check for collisions with other objects, 1 does (player, npcs, moving stuff etc)

        private Vector2 velocity;
        private Rectangle rectangle;

        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }

        public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }


        public ColliderType ColliderType { get; set; }
        public int LocationKey { get; set; }
        public bool IsUpdating { get; set; }
        public Dir InitialShuffDirection { get; set; }
        public IEntity Entity { get; set; }

        private Collider()
        {

        }

        public Collider(GraphicsDevice graphicsDevice, Vector2 velocity, Rectangle rectangle, IEntity entity, ColliderType colliderType = ColliderType.inert)
        {
            this.velocity = velocity;
            this.rectangle = rectangle;
            this.ColliderType = colliderType;

            SetRectangleTexture(graphicsDevice);

            ShowRectangle = true;
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
        public bool DidCollide(ICollidable objectBody, Vector2 position)
        {


            if (this.Rectangle.Intersects(objectBody.Rectangle))
            {
                if (velocity.X > 0 && IsTouchingLeft(rectangle, objectBody, velocity))
                {
                    velocity.X -= velocity.X; //+ (float).25;

                    return true;
                }



                if (velocity.X < 0 && IsTouchingRight(rectangle, objectBody, velocity))
                {
                    velocity.X -= velocity.X; //- (float).25;
                                              //   position.X = objectBody.DestinationRectangle.Right;
                    return true;
                }


                if (velocity.Y > 0 && IsTouchingTop(rectangle, objectBody, velocity))
                {
                    velocity.Y -= velocity.Y; //+ (float).25;
                                              //  position.Y = objectBody.DestinationRectangle.Top;
                    return true;
                }

                if (velocity.Y < 0 && IsTouchingBottom(rectangle, objectBody, velocity))
                {
                    velocity.Y -= velocity.Y;// - (float).25;
                                             // position.Y = objectBody.DestinationRectangle.Bottom;
                    return true;
                }
            }



            return false;

        }

 




        public bool IsTouchingLeft(Rectangle rectangle, ICollidable obj, Vector2 velocity)
        {
            return rectangle.Right + velocity.X > obj.Rectangle.Left &&
                rectangle.Left < obj.Rectangle.Left &&
                rectangle.Bottom > obj.Rectangle.Top &&
                rectangle.Top < obj.Rectangle.Bottom;
        }
        public bool IsTouchingRight(Rectangle rectangle, ICollidable obj, Vector2 velocity)
        {
            return rectangle.Left + velocity.X < obj.Rectangle.Right &&
                rectangle.Right > obj.Rectangle.Right &&
                rectangle.Bottom > obj.Rectangle.Top &&
                rectangle.Top < obj.Rectangle.Bottom;
        }
        public bool IsTouchingTop(Rectangle rectangle, ICollidable obj, Vector2 velocity)
        {
            return rectangle.Bottom + velocity.Y > obj.Rectangle.Top &&
                rectangle.Top < obj.Rectangle.Top &&
                rectangle.Right > obj.Rectangle.Left &&
                rectangle.Left < obj.Rectangle.Right;
        }
        public bool IsTouchingBottom(Rectangle rectangle, ICollidable obj, Vector2 velocity)
        {
            return rectangle.Top + velocity.Y < obj.Rectangle.Bottom &&
                rectangle.Bottom > obj.Rectangle.Bottom &&
                rectangle.Right > obj.Rectangle.Left &&
                rectangle.Left < obj.Rectangle.Right;
        }

        //Sprite part

        
        private void SetRectangleTexture(GraphicsDevice graphicsDevice)
        {
            var Colors = new List<Color>();
            for (int y = 0; y < Rectangle.Height; y++)
            {
                for (int x = 0; x < Rectangle.Width; x++)
                {
                    if (x == 0 || //left side
                        y == 0 || //top side
                        x == Rectangle.Width - 1 || //right side
                        y == Rectangle.Height - 1) //bottom side
                    {
                        Colors.Add(new Color(255, 255, 255, 255));
                    }
                    else
                    {
                        Colors.Add(new Color(0, 0, 0, 0));

                    }

                }
            }
            rectangleTexture = new Texture2D(graphicsDevice, Rectangle.Width, Rectangle.Height);
            rectangleTexture.SetData<Color>(Colors.ToArray());
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(rectangleTexture, new Vector2(Rectangle.X, Rectangle.Y), Color.White);




        }
        public virtual void Draw(SpriteBatch spriteBatch, float layerDepth)
        {

            spriteBatch.Draw(rectangleTexture, new Vector2(Rectangle.X, Rectangle.Y), color: Color.White, layerDepth: layerDepth);

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