using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;



using SecretProject.Class.SpriteFolder;

namespace SecretProject.Class.ObjectFolder
{
    public class Object
    {

        protected Texture2D rectangleTexture;

        public bool ShowRectangle { get; set; }


        public Vector2 Position;
        public Vector2 Velocity;
        public Color Color = Color.White;
        public float Speed;

        public string Name;

        public float LayerDepth;

        public delegate bool isTouchingTop(bool touch);

        public bool WalkBehind;

        public int HeightSubtractor { get; set; }
        public int WidthSubtractor { get; set; }

        public Vector2 touchingPoint;
        public int AdjustingPoint;

        public Vector2 TouchingPoint { get; set; }

        public int TextureAdjustmentX { get; set; }


        public int TextureAdjustmentY { get; set; }

        


        public int Rows { get; set; }

        public int Columns { get; set; }

        AnimatedSprite anim;

        int Width;
        int Height;


        public Rectangle Rectangle
        {
            get
            {

                return new Rectangle((int)Position.X + TextureAdjustmentX, (int)Position.Y + TextureAdjustmentY, (int)Width + WidthSubtractor, (int)Height + HeightSubtractor);


            }

        }

        public Object(float layerDepth, int adjustingPoint, Vector2 position, int height, int width)
        {


            this.Position = position;

            this.LayerDepth = layerDepth;



            touchingPoint = new Vector2(this.Rectangle.X, (this.Rectangle.Y + adjustingPoint));

            this.Height = height;
            this.Width = width;

        }

        public Object(Vector2 position, int height, int width)
        {
            this.Position = position;

            this.Height = height;
            this.Width = width;

            ShowRectangle = true;
        }

        public Object(GraphicsDevice graphicsDevice, Vector2 position, int height, int width)
        {



            this.Position = position;

            this.Height = height;
            this.Width = width;

            SetRectangleTexture(graphicsDevice);
        }

        private void SetRectangleTexture(GraphicsDevice graphicsDevice)
        {
            var Colors = new List<Color>();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (x == 0 || //left side
                        y == 0 || //top side
                        x == Width - 1 || //right side
                        y == Height - 1) //bottom side
                    {
                        Colors.Add(new Color(255, 255, 255, 255));
                    }
                    else
                    {
                        Colors.Add(new Color(0, 0, 0, 0));

                    }

                }
            }
            rectangleTexture = new Texture2D(graphicsDevice, Width, Height);
            rectangleTexture.SetData<Color>(Colors.ToArray());
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (ShowRectangle)
            {
                if (rectangleTexture != null)
                {
                    spriteBatch.Draw(rectangleTexture, Position, Color.White);
                }
            }
        }

        protected bool IsTouchingLeft(Sprite sprite)
        {
            return this.Rectangle.Right + this.Velocity.X > sprite.Rectangle.Left &&
                this.Rectangle.Left < sprite.Rectangle.Left &&
                this.Rectangle.Bottom > sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Bottom;
        }
        protected bool IsTouchingRight(Sprite sprite)
        {
            return this.Rectangle.Left + this.Velocity.X < sprite.Rectangle.Right &&
                this.Rectangle.Right > sprite.Rectangle.Right &&
                this.Rectangle.Bottom > sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Bottom;
        }
        protected bool IsTouchingTop(Sprite sprite)
        {
            return this.Rectangle.Bottom + this.Velocity.Y > sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Top &&
                this.Rectangle.Right > sprite.Rectangle.Left &&
                this.Rectangle.Left < sprite.Rectangle.Right;
        }
        protected bool IsTouchingBottom(Sprite sprite)
        {
            return this.Rectangle.Top + this.Velocity.Y < sprite.Rectangle.Bottom &&
                this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
                this.Rectangle.Right > sprite.Rectangle.Left &&
                this.Rectangle.Left < sprite.Rectangle.Right;
        }


        #region COLLISION
        protected bool IsTouchingLeft(Object obj)
        {
            return this.Rectangle.Right + this.Velocity.X > obj.Rectangle.Left &&
                this.Rectangle.Left < obj.Rectangle.Left &&
                this.Rectangle.Bottom > obj.Rectangle.Top &&
                this.Rectangle.Top < obj.Rectangle.Bottom;
        }
        protected bool IsTouchingRight(Object obj)
        {
            return this.Rectangle.Left + this.Velocity.X < obj.Rectangle.Right &&
                this.Rectangle.Right > obj.Rectangle.Right &&
                this.Rectangle.Bottom > obj.Rectangle.Top &&
                this.Rectangle.Top < obj.Rectangle.Bottom;
        }
        protected bool IsTouchingTop(Object obj)
        {
            return this.Rectangle.Bottom + this.Velocity.Y > obj.Rectangle.Top &&
                this.Rectangle.Top < obj.Rectangle.Top &&
                this.Rectangle.Right > obj.Rectangle.Left &&
                this.Rectangle.Left < obj.Rectangle.Right;
        }
        protected bool IsTouchingBottom(Object obj)
        {
            return this.Rectangle.Top + this.Velocity.Y < obj.Rectangle.Bottom &&
                this.Rectangle.Bottom > obj.Rectangle.Bottom &&
                this.Rectangle.Right > obj.Rectangle.Left &&
                this.Rectangle.Left < obj.Rectangle.Right;
        }
        #endregion
    }
}
