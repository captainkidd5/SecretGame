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
using SecretProject.Class.Universal;

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
        public SoundEffect Bubble { get; set; }        public string Name { get; set; }
        public float Speed { get; set; }
        public Color Color { get; set; } = Color.White;
        public Vector2 Velocity { get; set; }
        public bool IsBobbing { get; set; } = false;
        public bool IsTossed { get; set; } = false;
        public bool PickedUp { get; set; } = false;
        public bool IsWorldItem { get; set; } = false;
        public double BobberTimer { get; set; }
        public double TossTimer { get; set; }


        public bool IsAnimated { get; set; } = false;
        public bool IsAnimating { get; set; } = false;
        public AnimatedSprite Anim { get; set; }





        public bool IsBeingDragged { get; set; } = false;

        ContentManager content;

        public Sprite(GraphicsDevice graphicsDevice, ContentManager content, Texture2D texture, Vector2 position, bool bob, float layerDepth)
        {

            this.texture = texture;
            this.rectangleTexture = texture;
            this.Position = position;
            this.IsBobbing = bob;

            SetRectangleTexture(graphicsDevice, texture);

            BobberTimer = 0d;
            TossTimer = 0d;
            ScaleX = 1f;
            ScaleY = 1f;


            this.LayerDepth = layerDepth;
            this.content = content;

            if(IsAnimated)
            {
                Anim = new AnimatedSprite(graphicsDevice, Game1.ItemAtlas, Rows, Columns, Rows * Columns);
            }

        }

      

        public virtual void Update(GameTime gameTime, Vector2 position)
        {
            if(IsBeingDragged)
            {
                this.Position = position;
            }

            if(IsAnimating)
            {
                Anim.Update(gameTime);
            }
 
        }

        public virtual void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (IsDrawn)
            {
                LayerDepth = layerDepth;

                if (IsAnimating)
                {
                    Anim.Draw(spriteBatch, Position, layerDepth);
                }
                else
                {
                    spriteBatch.Draw(texture, Position, color: Color.White, layerDepth: layerDepth, scale: new Vector2(ScaleX, ScaleY));
                }

                
            }

            
        }

         public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsDrawn)
            {
 

                spriteBatch.Draw(texture, Position, color: Color.White, layerDepth: LayerDepth, scale: new Vector2(ScaleX, ScaleY));
            }
            
        }

        public void Bobber(GameTime gameTime)
        {

            
            if(IsBobbing)
            {
                BobberTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (BobberTimer > 2d)
                {
                    BobberTimer = 0d;
                }
                if (BobberTimer < 1d)
                {
                    this.Position.Y += (.03f);
                }
                
                if(BobberTimer >= 1d && BobberTimer < 2d)
                {
                    this.Position.Y -= (.03f);
                }
                
            }
        }

        public void Toss(GameTime gameTime, float x, float y)
        {
            if (IsTossed == false)
            {



                TossTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (TossTimer < .5)
                {
                    this.Position.X += x * .1f;
                    this.Position.Y += y * .1f;
                    
                }
                if (TossTimer >= .5)
                {
                    IsTossed = true;
                }
            }
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

    }
}

