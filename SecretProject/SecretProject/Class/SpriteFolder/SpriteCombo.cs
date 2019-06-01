using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.SpriteFolder
{
    class SpriteCombo
    {
        public GraphicsDevice Graphics { get; set; }
        public Texture2D AtlasTexture { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Rectangle DestinationRectangle { get; set; }

        public float TextureScaleX { get; set; }
        public float TextureScaleY { get; set; }

        public float ColorMultiplier { get; set; } = 1f;

        public bool IsAnimated { get; set; } = false;

        public Vector2 Position { get; set; }
        public float ScaleX { get; set; } = 1f;
        public float ScaleY { get; set; } = 1f;

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
        public SpriteCombo(GraphicsDevice graphics, Texture2D atlasTexture, Rectangle sourceRectangle, Rectangle destinationRectangle)
        {
            this.Graphics = graphics;
            this.AtlasTexture = atlasTexture;
            this.SourceRectangle = sourceRectangle;
            this.DestinationRectangle = destinationRectangle;
        }

        //for animated sprites
        public SpriteCombo(GraphicsDevice graphics, Texture2D atlasTexture, int firstFrameX, int firstFrameY, int frameWidth, int frameHeight, int totalFrames,
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
            else
            {
                this.DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(SourceRectangle.Width * ScaleX), (int)(SourceRectangle.Height * ScaleY));
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

            else
            {
                spriteBatch.Draw(AtlasTexture, sourceRectangle: SourceRectangle, destinationRectangle: DestinationRectangle, color: Color.White * ColorMultiplier,
                scale: new Vector2(TextureScaleX, TextureScaleY));
            }
            
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
                    this.Position.Y += (.03f);
                }

                if (BobberTimer >= 1d && BobberTimer < 2d)
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



    }
}
