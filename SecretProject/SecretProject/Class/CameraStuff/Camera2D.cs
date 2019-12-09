﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SecretProject.Class.CameraStuff
{
    public class Camera2D
    {
        protected float zoom;
        public Matrix transform;
        public Vector2 pos;
        protected float rotation;

        public Vector2 Origin;

        public float Zoom { get { return zoom; } set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } }
        public float Rotation { get { return rotation; } set { rotation = value; } }
        public Vector2 Pos { get { return pos; } set { pos = value; } }

        public Viewport MyViewPort { get; set; }

        public Rectangle ViewPortRectangle { get; set; }
        public Rectangle CameraScreenRectangle
        {
            get
            {
                return new Rectangle((int)(pos.X - (int)(Game1.ScreenWidth / Zoom /2)), (int)(pos.Y - (int)(Game1.ScreenHeight / Zoom/2)), (int)(Game1.ScreenWidth / Zoom /2 ), (int)(Game1.ScreenHeight / Zoom / 2));
            }
        }


        private Camera2D()
        {

        }

        public Camera2D(Viewport viewport)
        {
            zoom = 3.0f;
            rotation = 0.0f;
            pos = Vector2.Zero;

            Origin = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);

            MyViewPort = viewport;


        }





        public void Move(Vector2 amount)
        {
            pos += amount;
        }

        public void Follow(Vector2 amount, Rectangle rectangle)
        {

            pos.X = amount.X;
            pos.Y = amount.Y;
            ViewPortRectangle = new Rectangle((int)(Game1.GetCurrentStage().MapRectangle.X + Game1.ScreenWidth / 2 / zoom),
              (int)(Game1.GetCurrentStage().MapRectangle.Y + Game1.ScreenHeight / 2 / zoom),
                (int)(Game1.GetCurrentStage().MapRectangle.Width - Game1.ScreenWidth / 2 / zoom),
                (int)(Game1.GetCurrentStage().MapRectangle.Height - Game1.ScreenHeight / 2 / zoom));


            if(Game1.Player.LockBounds)
            {
                if (pos.X < ViewPortRectangle.X)
                {
                    pos.X = ViewPortRectangle.X;
                }
                if (pos.X > ViewPortRectangle.Width)
                {
                    pos.X = ViewPortRectangle.Width;
                }
                if (pos.Y < ViewPortRectangle.Y)
                {
                    pos.Y = ViewPortRectangle.Y;
                }
                if (pos.Y > ViewPortRectangle.Height)
                {
                    pos.Y = ViewPortRectangle.Height;
                }
            }
            



        }

        public Matrix getTransformation(GraphicsDevice graphicsDevice)
        {
            
                transform = Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) * Matrix.CreateRotationZ(Rotation)
                * Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) * Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f,
                graphicsDevice.Viewport.Height * 0.5f, 0));
            return transform;
        }

        public Matrix GetViewMatrix(Vector2 parallax)
        {
            // To add parallax, simply multiply it by the position
            return Matrix.CreateTranslation(new Vector3(-Pos * parallax, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }
    }
}
