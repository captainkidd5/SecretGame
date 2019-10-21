﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SecretProject.Class.SpriteFolder;
using SecretProject.Class.ItemStuff;
using Microsoft.Xna.Framework.Graphics;

namespace SecretProject.Class.CollisionDetection
{
    //public struct RectangleCollider
    //{
    //    private Vector2 topLeft;
    //    private Vector2 bottomRight;
    //    public Vector2 TopLeft { get { return topLeft; } set { topLeft = value; } }
    //    public Vector2 BottomRight { get { return bottomRight; } set { bottomRight = value; } }

    //    public Vector2 TopRight { get { return new Vector2(bottomRight.X, topLeft.Y); } }
    //    public Vector2 BottomLeft { get { return new Vector2(topLeft.X, bottomRight.Y); } }

    //    public float Top { get { return topLeft.Y; } }
    //    public float Right { get { return bottomRight.X; } }
    //    public float Left { get { return topLeft.X; } }
    //    public float Bottom { get { return bottomRight.Y; } }

    //    public RectangleCollider(Vector2 topLeft, Vector2 bottomRight)
    //    {
    //        this.topLeft = topLeft;
    //        this.bottomRight = bottomRight;


    //    }

    //    public bool Contains(Vector2 Point)
    //    {
    //        return (topLeft.X <= Point.X && bottomRight.X >= Point.X &&
    //                topLeft.Y <= Point.Y && bottomRight.Y >= Point.Y);
    //    }

    //    public bool Intersects(RectangleCollider Rect)
    //    {
    //        return (!(Bottom < Rect.Top ||
    //                   Top > Rect.Bottom ||
    //                   Right < Rect.Left ||
    //                   Left > Rect.Right));
    //    }


    //}

    public class Collider
    {
        protected Texture2D rectangleTexture;

        public bool ShowRectangle { get; set; }
        //0 doesnt check for collisions with other objects, 1 does (player, npcs, moving stuff etc)
        public int CollisionType { get; set; }

        private Vector2 velocity;
        private Rectangle rectangle;

        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }

        public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }

        private Collider()
        {

        }

        public Collider(GraphicsDevice graphicsDevice,Vector2 velocity, Rectangle rectangle, int collisionType = 0)
        {
            this.velocity = velocity;
            this.rectangle = rectangle;
            this.CollisionType = collisionType;

            SetRectangleTexture(graphicsDevice);

            ShowRectangle = true;
        }



        public bool DidCollide(Dictionary<string, ObjectBody> objectBody, Vector2 position)
        {

            foreach (var body in objectBody.Values)
            {
                if (body.DestinationRectangle.Left < Game1.cam.Pos.X + (Game1.ScreenWidth / 2 / Game1.cam.Zoom) && body.DestinationRectangle.Left > Game1.cam.Pos.X - (Game1.ScreenWidth / 2 / Game1.cam.Zoom + 16)
                             && body.DestinationRectangle.Y < Game1.cam.Pos.Y + (Game1.ScreenHeight / 2 / Game1.cam.Zoom + 16) && body.DestinationRectangle.Y > Game1.cam.Pos.Y - (Game1.ScreenHeight / 2 / Game1.cam.Zoom + 16))
                {

                    if (velocity.X > 0 && IsTouchingLeft(rectangle, body, velocity))
                    {
                        velocity.X -= velocity.X; //+ (float).25;
                     //   position.X = body.DestinationRectangle.Left;
                        return true;
                    }



                    if (velocity.X < 0 && IsTouchingRight(rectangle, body, velocity))
                    {
                        velocity.X -= velocity.X; //- (float).25;
                     //   position.X = body.DestinationRectangle.Right;
                        return true;
                    }


                    if (velocity.Y > 0 && IsTouchingTop(rectangle, body, velocity))
                    {
                        velocity.Y -= velocity.Y; //+ (float).25;
                      //  position.Y = body.DestinationRectangle.Top;
                        return true;
                    }

                    if (velocity.Y < 0 && IsTouchingBottom(rectangle, body, velocity))
                    {
                        velocity.Y -= velocity.Y;// - (float).25;
                       // position.Y = body.DestinationRectangle.Bottom;
                        return true;
                    }



                }
            }
            return false;

        }

        public void DidCollide(List<Sprite> sprite)
        {

            foreach (var spr in sprite)
            {
                if (spr.DestinationRectangle.Left < Game1.cam.Pos.X + (Game1.ScreenWidth / 2) - 400 && spr.DestinationRectangle.Left > Game1.cam.Pos.X - (Game1.ScreenWidth / 2) - 400
                             && spr.DestinationRectangle.Y < Game1.cam.Pos.Y + (Game1.ScreenHeight / 2) + 300 && spr.DestinationRectangle.Y > Game1.cam.Pos.Y - (Game1.ScreenHeight / 2) + 300)
                {

                    if (velocity.X > 0 && IsTouchingLeft(rectangle, spr, velocity))
                        velocity.X -= velocity.X; //+ (float).25;

                    if (velocity.X < 0 && IsTouchingRight(rectangle, spr, velocity))
                        velocity.X -= velocity.X; //- (float).25;

                    if (velocity.Y > 0 && IsTouchingTop(rectangle, spr, velocity))
                        velocity.Y -= velocity.Y; //+ (float).25;
                    if (velocity.Y < 0 && IsTouchingBottom(rectangle, spr, velocity))
                        velocity.Y -= velocity.Y;// - (float).25;
                }
            }

        }



        public void DidCollideMagnet(List<Item> item)
        {
            for (int i = 0; i < item.Count; i++)
            {
                if (item[i].Ignored == false)
                {
                    if (item[i].IsMagnetized)
                    {
                        item[i].Magnetize(velocity);
                        //it.IsMagnetized = false;
                    }

                    else if (velocity.X > 0 && IsTouchingLeft(rectangle, item[i].ItemSprite, velocity))
                    {
                        item[i].IsMagnetizable = true;
                    }


                    else if (velocity.X < 0 && IsTouchingRight(rectangle, item[i].ItemSprite, velocity))
                    {
                        item[i].IsMagnetizable = true;
                    }


                    else if (velocity.Y > 0 && IsTouchingTop(rectangle, item[i].ItemSprite, velocity))
                    {
                        item[i].IsMagnetizable = true;
                    }

                    else if (velocity.Y < 0 && IsTouchingBottom(rectangle, item[i].ItemSprite, velocity))
                    {
                        item[i].IsMagnetizable = true;
                    }

                    else
                    {
                        item[i].IsMagnetizable = false;
                    }
                }
            }





        }

        public bool IsTouchingLeft(Rectangle rectangle, ObjectBody obj, Vector2 velocity)
        {
            return rectangle.Right + velocity.X > obj.DestinationRectangle.Left &&
                rectangle.Left < obj.DestinationRectangle.Left &&
                rectangle.Bottom > obj.DestinationRectangle.Top &&
                rectangle.Top < obj.DestinationRectangle.Bottom;
        }
        public bool IsTouchingRight(Rectangle rectangle, ObjectBody obj, Vector2 velocity)
        {
            return rectangle.Left + velocity.X < obj.DestinationRectangle.Right &&
                rectangle.Right > obj.DestinationRectangle.Right &&
                rectangle.Bottom > obj.DestinationRectangle.Top &&
                rectangle.Top < obj.DestinationRectangle.Bottom;
        }
        public bool IsTouchingTop(Rectangle rectangle, ObjectBody obj, Vector2 velocity)
        {
            return rectangle.Bottom + velocity.Y > obj.DestinationRectangle.Top &&
                rectangle.Top < obj.DestinationRectangle.Top &&
                rectangle.Right > obj.DestinationRectangle.Left &&
                rectangle.Left < obj.DestinationRectangle.Right;
        }
        public bool IsTouchingBottom(Rectangle rectangle, ObjectBody obj, Vector2 velocity)
        {
            return rectangle.Top + velocity.Y < obj.DestinationRectangle.Bottom &&
                rectangle.Bottom > obj.DestinationRectangle.Bottom &&
                rectangle.Right > obj.DestinationRectangle.Left &&
                rectangle.Left < obj.DestinationRectangle.Right;
        }

        //Sprite part

        public bool IsTouchingLeft(Rectangle rectangle, Sprite sprite, Vector2 velocity)
        {
            return rectangle.Right + velocity.X > sprite.DestinationRectangle.Left &&
                rectangle.Left < sprite.DestinationRectangle.Left &&
                rectangle.Bottom > sprite.DestinationRectangle.Top &&
                rectangle.Top < sprite.DestinationRectangle.Bottom;
        }
        public bool IsTouchingRight(Rectangle rectangle, Sprite sprite, Vector2 velocity)
        {
            return rectangle.Left + velocity.X < sprite.DestinationRectangle.Right &&
                rectangle.Right > sprite.DestinationRectangle.Right &&
                rectangle.Bottom > sprite.DestinationRectangle.Top &&
                rectangle.Top < sprite.DestinationRectangle.Bottom;
        }
        public bool IsTouchingTop(Rectangle rectangle, Sprite sprite, Vector2 velocity)
        {
            return rectangle.Bottom + velocity.Y > sprite.DestinationRectangle.Top &&
                rectangle.Top < sprite.DestinationRectangle.Top &&
                rectangle.Right > sprite.DestinationRectangle.Left &&
                rectangle.Left < sprite.DestinationRectangle.Right;
        }
        public bool IsTouchingBottom(Rectangle rectangle, Sprite sprite, Vector2 velocity)
        {
            return rectangle.Top + velocity.Y < sprite.DestinationRectangle.Bottom &&
                rectangle.Bottom > sprite.DestinationRectangle.Bottom &&
                rectangle.Right > sprite.DestinationRectangle.Left &&
                rectangle.Left < sprite.DestinationRectangle.Right;
        }
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

    }
}