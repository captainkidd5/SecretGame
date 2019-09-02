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

        public int OffSetX { get; set; }
        public int OffSetY { get; set; }

        public Rectangle[] PlayerPartSourceRectangles { get; set; }

        //for clouds
        public float Speed { get; set; }

        public Color Color { get; set; }





        //for non animated sprites
        public Sprite(GraphicsDevice graphics, Texture2D atlasTexture, Rectangle sourceRectangle, Vector2 position, int width, int height)
        {
            this.Graphics = graphics;
            this.AtlasTexture = atlasTexture;
            this.SourceRectangle = sourceRectangle;
            this.Position = position;
            this.Width = width;
            this.Height = height;
            
        }

        //for animated sprites
        public Sprite(GraphicsDevice graphics, Texture2D atlasTexture, int firstFrameX, int firstFrameY, int frameWidth, int frameHeight, int totalFrames,
            float animationSpeed, Vector2 positionToDrawTo, int offSetX = 0, int offSetY = 0)
        {
            this.Graphics = graphics;
            this.AtlasTexture = atlasTexture;
            this.FirstFrameX = firstFrameX;
            this.FirstFrameY = firstFrameY;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.TotalFrames = totalFrames;
            this.AnimationSpeed = animationSpeed;
            this.Position = positionToDrawTo;
            this.OffSetX = offSetX;
            this.OffSetY = offSetY;
            this.Color = Color.White;
        }



        public void Update(GameTime gameTime)
        {
            if(IsAnimated)
            {
                UpdateAnimations(gameTime,Position);
            }

        }


        public void Update(GameTime gameTime, Vector2 position)
        {

            if(IsBeingDragged)
            {
                this.Position = position;
            }
            if (IsAnimated)
            {
                UpdateAnimations(gameTime, position);
            }
            else
            {
                this.DestinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)(Width * TextureScaleX), (int)(Height * TextureScaleY));
            }
        }

        public void UpdateShip(GameTime gameTime, Vector2 position)
        {
        }

        public void UpdateAnimations(GameTime gameTime, Vector2 position)
        {
            this.Position = position;
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
            DestinationRectangle = new Rectangle((int)position.X + OffSetX, (int)position.Y + OffSetY, FrameWidth, FrameHeight);
        }

        public void UpdatePlayerPartAnimations(GameTime gameTime, Vector2 position)
        {
            this.Position = position;
            AnimationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (AnimationTimer <= 0)
            {
                CurrentFrame++;
                AnimationTimer = AnimationSpeed;
            }
            if (CurrentFrame == TotalFrames)
            {
                CurrentFrame = 0;
            }
            SourceRectangle = new Rectangle((int)(this.FirstFrameX + this.FrameWidth * this.CurrentFrame), (int)this.FirstFrameY, (int)this.FrameWidth, (int)this.FrameHeight);
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            DestinationRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(Width * TextureScaleX), (int)(Height * TextureScaleY));

            spriteBatch.Draw(AtlasTexture, sourceRectangle: SourceRectangle, destinationRectangle: DestinationRectangle,
                    color: Color.White * ColorMultiplier, layerDepth: layerDepth, scale:new Vector2(TextureScaleX, TextureScaleY));

            
        }

        public void DrawAnimation(SpriteBatch spriteBatch, Vector2 currentPosition, float layerDepth)
        {
            //something wrong with destination rectangle after animation
           
            spriteBatch.Draw(AtlasTexture,  new Vector2(DestinationRectangle.X, DestinationRectangle.Y),sourceRectangle: SourceRectangle,
                    color: this.Color * ColorMultiplier, layerDepth: layerDepth, scale: new Vector2(TextureScaleX, TextureScaleY));
        }



        //for ship sprites
        public void DrawRotationalSprite(SpriteBatch spriteBatch, Vector2 position,float rotation, Vector2 origin, float layerDepth)
        {
            //public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

            spriteBatch.Draw(this.AtlasTexture, position, this.SourceRectangle, Color.White, rotation, origin, 1f,SpriteEffects.None, layerDepth );
           // spriteBatch.Draw(this.AtlasTexture, this.DestinationRectangle, this.SourceRectangle, Color.White, rotation, origin, SpriteEffects.None, layerDepth);
        }

        public void Bobber(GameTime gameTime)
        {


            if (IsBobbing)
            {
                BobberTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (BobberTimer > 3)
                {
                    BobberTimer = 0f;
                }
                if (BobberTimer < 1.5f)
                {
                    this.Position.Y += .05f;
                }

                if (BobberTimer >= 1.5f && BobberTimer < 3f)
                {
                    this.Position.Y -= .05f;
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
                    switch (Game1.Player.controls.Direction)
                    {
                        case Dir.Right:
                            this.Position.X += x * 1f * Game1.Utility.RGenerator.Next(1,2);
                            this.Position.Y += y * .1f;
                            break;
                        case Dir.Left:
                            this.Position.X -= x * 1f * Game1.Utility.RGenerator.Next(1, 2);
                            this.Position.Y += y * .1f;
                            break;
                        case Dir.Up:
                            this.Position.X += x * .1f;
                            this.Position.Y -= y * 1f * Game1.Utility.RGenerator.Next(1, 2);
                            break;
                        case Dir.Down:
                            this.Position.X += x * .1f;
                            this.Position.Y += y * 1.25f * Game1.Utility.RGenerator.Next(1, 2);
                            this.LayerDepth = 1f;
                            break;
                    }

                    

                }
                if (TossTimer >= .5)
                {
                    IsTossed = true;
                }
            }
        }

        public void PlayOnce(GameTime gameTime, Vector2 position)
        {
            this.Position = position;

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
            SourceRectangle = new Rectangle((int)(this.FirstFrameX + this.FrameWidth * this.CurrentFrame), (int)this.FirstFrameY, (int)this.FrameWidth, (int)this.FrameHeight);

        }
        public void SetFrame(int newFrame)
        {
            CurrentFrame = newFrame;
            SourceRectangle = new Rectangle((int)(this.FirstFrameX + this.FrameWidth * this.CurrentFrame), (int)this.FirstFrameY, (int)this.FrameWidth, (int)this.FrameHeight);
            //DestinationRectangle = new Rectangle((int)this.Position.X + OffSetX, (int)this.Position.Y + OffSetY, FrameWidth, FrameHeight);
        }

    }
}
