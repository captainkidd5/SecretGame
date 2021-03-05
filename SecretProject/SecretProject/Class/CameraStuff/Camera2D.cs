
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.StageFolder;

namespace SecretProject.Class.CameraStuff
{
    public class Camera2D : ISaveable
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
                return new Rectangle((int)(pos.X - (int)(Game1.ScreenWidth / this.Zoom / 2)), (int)(pos.Y - (int)(Game1.ScreenHeight / this.Zoom / 2)), (int)(Game1.ScreenWidth / this.Zoom), (int)(Game1.ScreenHeight / this.Zoom));
            }
        }


        private Camera2D()
        {

        }

        public Camera2D(Viewport viewport)
        {
            zoom = 5.0f;
            rotation = 0.0f;
            pos = Vector2.Zero;

            Origin = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);

            this.MyViewPort = viewport;


        }




        public void Move(Vector2 amount)
        {
            pos += amount;
        }

        public void Follow(Vector2 amount, Rectangle rectangle)
        {
            //OldCamPosition = pos;
            if (!Game1.freeze)
            {



               // pos.X = (float)Math.Round(amount.X);

                //pos.Y = (float)Math.Round(amount.Y);

                 //pos.X += amount.X;
             //   pos.Y += amount.Y;


                const int camera_smoothing = 10;
                const float camera_frame_size_hor = 4;
                const float camera_frame_size_ver = 3;

                float camera_target_x = Math.Min(Math.Max(pos.X, amount.X - camera_frame_size_hor), amount.X + camera_frame_size_hor);
                float camera_target_y = Math.Min(Math.Max(pos.Y, amount.Y - camera_frame_size_ver), amount.Y + camera_frame_size_ver);
                pos.X = ((pos.X) * (camera_smoothing - 1) + camera_target_x) / camera_smoothing;
                pos.Y = ((pos.Y) * (camera_smoothing - 1) + camera_target_y) / camera_smoothing;


                //pos.X = MathHelper.Lerp(pos.X, amount.X, .5f);
                // pos.Y = MathHelper.Lerp(pos.Y, amount.Y, .5f);
                this.ViewPortRectangle = new Rectangle((int)(StageManager.CurrentStage.MapRectangle.X + Game1.ScreenWidth / 2 / zoom),
                  (int)(StageManager.CurrentStage.MapRectangle.Y + Game1.ScreenHeight / 2 / zoom),
                    (int)(StageManager.CurrentStage.MapRectangle.Width - Game1.ScreenWidth / 2 / zoom),
                    (int)(StageManager.CurrentStage.MapRectangle.Height - Game1.ScreenHeight / 2 / zoom));


                if (Game1.Player.LockBounds)
                {
                    if (pos.X < this.ViewPortRectangle.X)
                    {
                        pos.X = this.ViewPortRectangle.X;
                    }
                    if (pos.X > this.ViewPortRectangle.Width)
                    {
                        pos.X = this.ViewPortRectangle.Width;
                    }
                    if (pos.Y < this.ViewPortRectangle.Y)
                    {
                        pos.Y = this.ViewPortRectangle.Y;
                    }
                    if (pos.Y > this.ViewPortRectangle.Height)
                    {
                        pos.Y = this.ViewPortRectangle.Height;
                    }
                }

            }


        }

        public Matrix getTransformation(GraphicsDevice graphicsDevice)
        {

            transform = Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) * Matrix.CreateRotationZ(this.Rotation)
            * Matrix.CreateScale(new Vector3(this.Zoom, this.Zoom, 1)) * Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f,
            graphicsDevice.Viewport.Height * 0.5f, 0));
            return transform;
        }

        public Matrix GetViewMatrix(Vector2 parallax)
        {
            // To add parallax, simply multiply it by the position
            return Matrix.CreateTranslation(new Vector3(-this.Pos * parallax, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateRotationZ(this.Rotation) *
                Matrix.CreateScale(this.Zoom, this.Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(this.Zoom);
        }

        public void Load(BinaryReader reader)
        {
            this.Zoom = reader.ReadSingle();
        }
    }
}
