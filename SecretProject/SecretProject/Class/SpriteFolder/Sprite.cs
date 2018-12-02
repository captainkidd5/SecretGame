using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Media;

namespace SecretProject.Class.SpriteFolder
{
    public class Sprite
    {


        protected Texture2D _texture;
        public Vector2 Position;
        public Vector2 Velocity;
        public Color Color = Color.White;
        public float Speed;
        public string Name;

        protected Texture2D rectangleTexture;

        public bool ShowRectangle { get; set; }

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
                return new Rectangle((int)Position.X + TextureAdjustmentX, (int)Position.Y + TextureAdjustmentY, _texture.Width + WidthSubtractor, _texture.Height + HeightSubtractor);
            }

        }

        public Sprite(Texture2D texture)
        {

        }

        public Sprite(GraphicsDevice graphicsDevice, Texture2D texture, float layerDepth, int adjustingPoint)
        {
            _texture = texture;

            this.LayerDepth = layerDepth;

            //TextureAdjustmentX = 0;
            //TextureAdjustmentY = 0;


            touchingPoint = new Vector2(this.Rectangle.X, (this.Rectangle.Y + adjustingPoint));

            anim = new AnimatedSprite(graphicsDevice, _texture, Rows, Columns);


        }

        public Sprite(GraphicsDevice graphicsDevice, Texture2D texture, Vector2 position) : this(texture)
        {

            _texture = texture;
            this.rectangleTexture = texture;
            this.Position = position;



            SetRectangleTexture(graphicsDevice, texture);
        }



        private void SetRectangleTexture(GraphicsDevice graphicsDevice, Texture2D texture)
        {
            var Colors = new List<Color>();
            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    if (x == 0 || //left side
                        y == 0 || //top side
                        x == texture.Width - 1 || //right side
                        y == texture.Height - 1) //bottom side
                    {
                        Colors.Add(new Color(255, 255, 255, 255));
                    }
                    else
                    {
                        Colors.Add(new Color(0, 0, 0, 0));

                    }

                }
            }
            rectangleTexture = new Texture2D(graphicsDevice, texture.Width, texture.Height);
            rectangleTexture.SetData<Color>(Colors.ToArray());
        }


        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            LayerDepth = layerDepth;

            spriteBatch.Draw(_texture, Position, color: Color.White, layerDepth: layerDepth);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);

            if (ShowRectangle)
            {
                if (rectangleTexture != null)
                {
                    spriteBatch.Draw(rectangleTexture, Position, Color.White);
                }

            }
        }


        #region COLLISION
        bool IsTouchingLeft(Sprite sprite)
        {
            return this.Rectangle.Right + this.Velocity.X > sprite.Rectangle.Left &&
                this.Rectangle.Left < sprite.Rectangle.Left &&
                this.Rectangle.Bottom > sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Bottom;
        }
        bool IsTouchingRight(Sprite sprite)
        {
            return this.Rectangle.Left + this.Velocity.X < sprite.Rectangle.Right &&
                this.Rectangle.Right > sprite.Rectangle.Right &&
                this.Rectangle.Bottom > sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Bottom;
        }
        bool IsTouchingTop(Sprite sprite)
        {
            return this.Rectangle.Bottom + this.Velocity.Y > sprite.Rectangle.Top &&
                this.Rectangle.Top < sprite.Rectangle.Top &&
                this.Rectangle.Right > sprite.Rectangle.Left &&
                this.Rectangle.Left < sprite.Rectangle.Right;
        }
        bool IsTouchingBottom(Sprite sprite)
        {
            return this.Rectangle.Top + this.Velocity.Y < sprite.Rectangle.Bottom &&
                this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
                this.Rectangle.Right > sprite.Rectangle.Left &&
                this.Rectangle.Left < sprite.Rectangle.Right;
        }



        #endregion


    }
}

