using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.SpriteFolder
{
    public class Sprite
    {
        public GraphicsDevice Graphics { get; set; }
        public Texture2D AtlasTexture { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Rectangle DestinationRectangle { get; set; }

        public float TextureScaleX { get; set; } = 1f;
        public float TextureScaleY { get; set; } = 1f;
        public float LayerDepth { get; set; } = 1f;

        public float ColorMultiplier { get; set; } = 1f;

        public bool IsAnimated { get; set; } = false;

        //keep as field
        public Vector2 Position;
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsWorldItem { get; set; } = false;
        public bool PickedUp { get; set; } = false;
        public bool IsBeingDragged { get; set; } = false;

        public bool IsBobbing { get; set; } = false;
        public bool IsTossed { get; set; } = false;
        public float BobberTimer { get; set; }
        public float TossTimer { get; set; }

        //For Animation use only
        public int CurrentFrame { get; set; }
        public int FirstFrameX { get; set; }
        public int FirstFrameY { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public int TotalFrames { get; set; }

        public float AnimationSpeed { get; set; }
        public float AnimationTimer { get; set; }




        //for non animated sprites
        public Sprite(GraphicsDevice graphics, Texture2D atlasTexture, Rectangle sourceRectangle, Vector2 position, int width, int height)
        {
            this.Graphics = graphics;
            this.AtlasTexture = atlasTexture;
            this.SourceRectangle = sourceRectangle;
            this.Position = position;
            this.Width = width;
            this.Height = height;
            this.DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        //for animated sprites
        public Sprite(GraphicsDevice graphics, Texture2D atlasTexture, int firstFrameX, int firstFrameY, int frameWidth, int frameHeight, int totalFrames,
            float animationSpeed)
        {
            this.Graphics = graphics;
            this.AtlasTexture = atlasTexture;
            this.FirstFrameX = firstFrameX;
            this.FirstFrameY = firstFrameY;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.TotalFrames = totalFrames;
            this.AnimationSpeed = animationSpeed;
        }

        public void Update(GameTime gameTime)
        {
            if(IsAnimated)
            {
                UpdateAnimations(gameTime);
            }

        }

        //public void UpdateDestinationRectangle()
        //{
        //    this.DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(DestinationRectangle.Width * TextureScaleX), (int)(DestinationRectangle.Height * TextureScaleY));
        //}

        public void Update(GameTime gameTime, Vector2 position)
        {

            if(IsBeingDragged)
            {
                this.Position = position;
            }
            if (IsAnimated)
            {
                UpdateAnimations(gameTime);
            }
            else
            {
                this.DestinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)(Width * TextureScaleX), (int)(Height * TextureScaleY));
            }
        }

        public void UpdateAnimations(GameTime gameTime)
        {
            AnimationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(AnimationTimer <= 0)
            {
                CurrentFrame++;
                AnimationTimer = AnimationSpeed;
            }
            if(CurrentFrame == TotalFrames)
            {
                CurrentFrame = 0;
            }
            SourceRectangle = new Rectangle((int)(this.FirstFrameX + this.FrameWidth * this.CurrentFrame), (int)this.FirstFrameY, (int)this.FrameWidth, (int)this.FrameHeight);
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            DestinationRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(Width * TextureScaleX), (int)(Height * TextureScaleY));

            spriteBatch.Draw(AtlasTexture, sourceRectangle: SourceRectangle, destinationRectangle: DestinationRectangle,
                    color: Color.White * ColorMultiplier, layerDepth: this.LayerDepth, scale:new Vector2(TextureScaleX, TextureScaleY));

            
        }

        public void Bobber(GameTime gameTime)
        {


            if (IsBobbing)
            {
                BobberTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (BobberTimer > 2f)
                {
                    BobberTimer = 0f;
                }
                if (BobberTimer < 1f)
                {
                    this.Position.Y += .03f;
                }

                if (BobberTimer >= 1d && BobberTimer < 2d)
                {
                    this.Position.Y -= .03f;
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

        public void PlayOnce(GameTime gameTime)
        {


            AnimationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (AnimationTimer <= 0)
            {
                CurrentFrame++;
                AnimationTimer = AnimationSpeed;
            }
            if (CurrentFrame == TotalFrames)
            {
                CurrentFrame = 0;
                IsAnimated = false;
            }

        }


    }
}
