using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Media;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.ItemStuff.Items;
using SecretProject.Class.Playable;
using SecretProject.Class.Stage;

namespace SecretProject.Class.SpriteFolder
{
    public class Sprite
    {


        protected Texture2D texture;
        public Vector2 Position;
        protected Texture2D rectangleTexture;

        public bool ShowRectangle { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            }

        }

        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public bool IsDrawn { get; set; } = true;
        public float LayerDepth { get; set; }
        public SoundEffect Bubble { get; set; }
        public string Name { get; set; }
        public float Speed { get; set; }
        public Color Color { get; set; } = Color.White;
        public Vector2 Velocity { get; set; }
        public bool IsBobbing { get; set; } = false;
        public bool PickedUp { get; set; } = false;
        public bool IsWorldItem { get; set; } = false;
        public double Timer { get; set; }
        public SoundEffectInstance BubbleInstance { get; set; }
        public AnimatedSprite Anim { get; set; }

        ContentManager content;

        public Sprite(GraphicsDevice graphicsDevice, ContentManager content, Texture2D texture, Vector2 position, bool bob)
        {

            this.texture = texture;
            this.rectangleTexture = texture;
            this.Position = position;
            this.IsBobbing = bob;

            SetRectangleTexture(graphicsDevice, texture);

            Timer = 0d;

            ScaleX = 1f;
            ScaleY = 1f;

            Bubble = content.Load<SoundEffect>("SoundEffects/bubble");

            BubbleInstance = Bubble.CreateInstance();
            BubbleInstance.IsLooped = false;
            this.content = content;
        }

      

        public virtual void Update(GameTime gameTime)
        {
 
        }

        public virtual void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (IsDrawn)
            {
                LayerDepth = layerDepth;

                spriteBatch.Draw(texture, Position, color: Color.White, layerDepth: layerDepth, scale: new Vector2(ScaleX, ScaleY));
            }
            
        }

        public void Bobber(GameTime gameTime)
        {

            
            if(IsBobbing)
            {
                Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Timer > 2d)
                {
                    Timer = 0d;
                }
                if (Timer < 1d)
                {
                    this.Position.Y += (.03f);
                }
                
                if(Timer >= 1d && Timer < 2d)
                {
                    this.Position.Y -= (.03f);
                }
                
            }
        }

        /*
        public void Magnetize(Vector2 playerpos)
        {
            if (IsWorldItem)
            {

                if (ScaleX <= 0f || ScaleY <= 0f)
                {
                    if (IsDrawn)
                    {
                        BubbleInstance.Play();
                        PickedUp = true;
                    }

                    IsDrawn = false;
                }
                this.Position.X -= playerpos.X;
                this.Position.Y -= playerpos.Y;
                ScaleX -= .1f;
                ScaleY -= .1f;
            }
            
        }
        */

        

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

    }
}

