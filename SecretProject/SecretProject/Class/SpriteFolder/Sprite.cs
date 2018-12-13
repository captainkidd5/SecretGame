using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Media;
using SecretProject.Class.Playable;

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

        public float scaleX;
        public float scaleY;
        public bool isDrawn = true;

        protected Texture2D rectangleTexture;

        public bool ShowRectangle { get; set; }

        public float LayerDepth;

        public int Rows { get; set; }

        public int Columns { get; set; }

        AnimatedSprite anim;

        public bool isBobbing = false;
        public bool isMagnetized = false;
       
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }

        }

        public double timer;



        public Sprite(GraphicsDevice graphicsDevice, Texture2D texture, Vector2 position, bool bob)
        {

            _texture = texture;
            this.rectangleTexture = texture;
            this.Position = position;
            this.isBobbing = bob;

            SetRectangleTexture(graphicsDevice, texture);

            timer = 0d;

            scaleX = 1f;
            scaleY = 1f;
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
            Bobber(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (isDrawn)
            {
                LayerDepth = layerDepth;

                spriteBatch.Draw(_texture, Position, color: Color.White, layerDepth: layerDepth, scale: new Vector2(scaleX, scaleY));
            }
            
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

        public void Bobber(GameTime gameTime)
        {

            
            if(isBobbing)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timer > 2d)
                {
                    timer = 0d;
                }
                if (timer < 1d)
                {
                    this.Position.Y += (.03f);
                }
                
                if(timer >= 1d && timer < 2d)
                {
                    this.Position.Y -= (.03f);
                }
                
            }
        }

        public void Magnetize(Vector2 playerpos)
        {
            if(scaleX <= 0f || scaleY <=0f)
            {
                isDrawn = false;
            }
            this.Position.X -= playerpos.X;
            this.Position.Y -= playerpos.Y;
            scaleX -= .1f;
            scaleY -= .1f;
        }

    }
}

