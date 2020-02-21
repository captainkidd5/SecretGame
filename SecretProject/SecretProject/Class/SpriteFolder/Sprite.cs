using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Universal;
using System;

namespace SecretProject.Class.SpriteFolder
{
    public class Sprite : ICollidable
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
        public Vector2 Velocity { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsWorldItem { get; set; } = false;
        public bool PickedUp { get; set; } = false;
        public bool IsBeingDragged { get; set; } = false;
        public bool IsTossed { get; set; } = false;

        public float TossTimer { get; set; }


        //For Animation use only
        public int CurrentFrameCounter { get; set; }
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


        public Vector2 Origin { get; set; }
        public int ID { get; set; }
        public float Rotation { get; set; }
        public float RotationAnchor { get; set; }
        public bool IsSpinning { get; set; }
        public float SpinAmount { get; set; }
        public float SpinSpeed { get; set; }

        public Vector2 destinationVector = new Vector2(0, 0);

        public bool Flip { get; set; }
        //for collider
        public Rectangle Rectangle { get; set; }
        public ColliderType ColliderType { get; set; }
        public string LocationKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Dir InitialShuffDirection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEntity Entity { get; set; }
        public bool IsUpdating { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public bool ChangesFrames { get; set; } // set to false if you don't want the animations to continue but still need a certain number of frames to maintain animation functionality

        //for non animated sprites
        public Sprite(GraphicsDevice graphics, Texture2D atlasTexture, Rectangle sourceRectangle, Vector2 position, int width, int height)
        {
            this.Graphics = graphics;
            this.AtlasTexture = atlasTexture;
            this.SourceRectangle = sourceRectangle;
            Position = position;
            this.Velocity = Vector2.Zero;
            this.Width = width;
            this.Height = height;
            this.Rotation = 0f;
            this.RotationAnchor = 0f;
            this.Origin = Game1.Utility.Origin;

        }

        //for animated sprites
        public Sprite(GraphicsDevice graphics, Texture2D atlasTexture, int firstFrameX, int firstFrameY, int frameWidth, int frameHeight, int totalFrames,
            float animationSpeed, Vector2 positionToDrawTo, int offSetX = 0, int offSetY = 0, bool changeFrames = true)
        {
            this.Graphics = graphics;
            this.AtlasTexture = atlasTexture;
            this.FirstFrameX = firstFrameX;
            this.FirstFrameY = firstFrameY;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.TotalFrames = totalFrames;
            this.AnimationSpeed = animationSpeed;
            Position = positionToDrawTo;
            this.OffSetX = offSetX;
            this.OffSetY = offSetY;
            this.Color = Color.White;
            this.Rotation = 0f;
            this.RotationAnchor = 0f;
            this.ChangesFrames = changeFrames;

            this.SourceRectangle = new Rectangle((int)(this.FirstFrameX + this.FrameWidth * this.CurrentFrame), (int)this.FirstFrameY, (int)this.FrameWidth, (int)this.FrameHeight);
        }



        public void Update(GameTime gameTime)
        {
            if (this.IsAnimated)
            {
                UpdateAnimations(gameTime, Position);
            }
            // this.Rectangle = new Rectangle((int)position.X, (int)position.Y, (int)(Width * TextureScaleX), (int)(Height * TextureScaleY));
            this.DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(this.Width * this.TextureScaleX), (int)(this.Height * this.TextureScaleY));
            this.Rectangle = this.DestinationRectangle;
        }

        //only for dragging sprites in the UI
        public void Update(GameTime gameTime, Vector2 position)
        {


            if (this.IsSpinning)
            {
                Spin(gameTime, this.SpinAmount, this.SpinSpeed);
            }
            this.DestinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)(this.Width * this.TextureScaleX), (int)(this.Height * this.TextureScaleY));
            this.Rectangle = this.DestinationRectangle;
            Position = position;

        }

        public void UpdateAnimations(GameTime gameTime, Vector2 position)
        {
            Position = position;
            this.AnimationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (this.AnimationTimer <= 0)
            {
                if(this.ChangesFrames)
                {
                    this.CurrentFrame++;
                }
              
                this.AnimationTimer = this.AnimationSpeed;
            }
            if (this.CurrentFrame == this.TotalFrames)
            {
                this.CurrentFrame = 0;
            }
            this.SourceRectangle = new Rectangle((int)(this.FirstFrameX + this.FrameWidth * this.CurrentFrame), (int)this.FirstFrameY, (int)this.FrameWidth, (int)this.FrameHeight);

        }


        public void UpdateSourceRectangle()
        {
            this.SourceRectangle = new Rectangle((int)(this.FirstFrameX + this.FrameWidth * this.CurrentFrame), (int)this.FirstFrameY, (int)this.FrameWidth, (int)this.FrameHeight);
        }


        public void UpdateAnimationsBackwards(GameTime gameTime, Vector2 position)
        {
            Position = position;
            this.AnimationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (this.AnimationTimer <= 0)
            {
                this.CurrentFrame--;
                this.AnimationTimer = this.AnimationSpeed;
            }
            if (this.CurrentFrame == -1)
            {
                this.CurrentFrame = this.TotalFrames - 1;
            }
            this.SourceRectangle = new Rectangle((int)(this.FirstFrameX + this.FrameWidth * this.CurrentFrame), (int)this.FirstFrameY, (int)this.FrameWidth, (int)this.FrameHeight);

        }

        public void UpdateAnimationPosition(Vector2 position)
        {
            destinationVector = new Vector2(position.X + this.OffSetX, position.Y + this.OffSetY);
            this.DestinationRectangle = new Rectangle((int)position.X + this.OffSetX, (int)position.Y + this.OffSetY, this.FrameWidth, this.FrameHeight);
            this.Rectangle = this.DestinationRectangle;
        }

        public void UpdatePlayerPartAnimations(GameTime gameTime, Vector2 position)
        {
            Position = position;
            this.AnimationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (this.AnimationTimer <= 0)
            {
                if(this.ChangesFrames)
                {
                    this.CurrentFrame++;
                }
                
                this.AnimationTimer = this.AnimationSpeed;
            }
            if (this.CurrentFrame == this.TotalFrames)
            {
                this.CurrentFrame = 0;
            }
            this.SourceRectangle = new Rectangle((int)(this.FirstFrameX + this.FrameWidth * this.CurrentFrame), (int)this.FirstFrameY, (int)this.FrameWidth, (int)this.FrameHeight);
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            this.DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(this.Width * this.TextureScaleX), (int)(this.Height * this.TextureScaleY));

            spriteBatch.Draw(this.AtlasTexture, sourceRectangle: this.SourceRectangle, destinationRectangle: this.DestinationRectangle,
                    color: Color.White * this.ColorMultiplier, rotation: this.Rotation, origin: this.Origin, layerDepth: layerDepth, scale: new Vector2(this.TextureScaleX, this.TextureScaleY));


        }

        public void DrawFromUIToWorld(SpriteBatch spriteBatch, float layerDepth)
        {
            this.DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(this.Width * this.TextureScaleX), (int)(this.Height * this.TextureScaleY));

            spriteBatch.Draw(this.AtlasTexture, sourceRectangle: this.SourceRectangle, destinationRectangle: this.DestinationRectangle,
                    color: Color.White * this.ColorMultiplier, rotation: this.Rotation, origin: this.Origin, layerDepth: 1f, scale: new Vector2(this.TextureScaleX, this.TextureScaleY));
        }

        public void DrawAnimation(SpriteBatch spriteBatch, Vector2 currentPosition, float layerDepth, float rotation = 0f)
        {
            if (this.Flip)
            {
                spriteBatch.Draw(this.AtlasTexture, currentPosition, sourceRectangle: this.SourceRectangle,
                    color: this.Color * this.ColorMultiplier, effects: SpriteEffects.FlipHorizontally, rotation: rotation, layerDepth: layerDepth, scale: new Vector2(this.TextureScaleX, this.TextureScaleY));
            }
            else
            {
                spriteBatch.Draw(this.AtlasTexture, currentPosition, sourceRectangle: this.SourceRectangle,
                    color: this.Color * this.ColorMultiplier, rotation: rotation, layerDepth: layerDepth, scale: new Vector2(this.TextureScaleX, this.TextureScaleY));
            }

        }

        public void DrawScalableAnimation(SpriteBatch spriteBatch, Vector2 currentPosition, float layerDepth, float rotation = 0f, float scale = 1f)
        {
            if (this.Flip)
            {
                spriteBatch.Draw(this.AtlasTexture, currentPosition, sourceRectangle: this.SourceRectangle,
                    color: this.Color * this.ColorMultiplier, effects: SpriteEffects.FlipHorizontally, rotation: rotation, layerDepth: layerDepth, scale: new Vector2(scale, scale));
            }
            else
            {
                spriteBatch.Draw(this.AtlasTexture, currentPosition, this.SourceRectangle, Color.White, 0f, Game1.Utility.Origin, scale, SpriteEffects.None, layerDepth);
            }


        }

        //for ship sprites
        public void DrawRotationalSprite(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 origin, float layerDepth)
        {

            spriteBatch.Draw(this.AtlasTexture, position, this.SourceRectangle, Color.White, rotation, origin, 1f, SpriteEffects.None, layerDepth);
        }

  

        public void Toss(GameTime gameTime, float x, float y)
        {
            if (this.IsTossed == false)
            {



                this.TossTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (this.TossTimer < 1)
                {
                    switch (Game1.Player.controls.Direction)
                    {
                        case Dir.Right:
                            Position.X += x * 1f * Game1.Utility.RGenerator.Next(1, 2);
                            Position.Y += y * .1f;
                            break;
                        case Dir.Left:
                            Position.X -= x * 1f * Game1.Utility.RGenerator.Next(1, 2);
                            Position.Y += y * .1f;
                            break;
                        case Dir.Up:
                            Position.X += x * .1f;
                            Position.Y -= y * 1f * Game1.Utility.RGenerator.Next(1, 2);
                            break;
                        case Dir.Down:
                            Position.X += x * .1f;
                            Position.Y += y * 1.25f * Game1.Utility.RGenerator.Next(1, 2);
                            this.LayerDepth = 1f;
                            break;
                    }



                }
                if (this.TossTimer >= 1)
                {
                    this.IsTossed = true;
                }
            }
        }

        public void Spin(GameTime gameTime, float amount, float speed)
        {
            if (amount > 0)
            {
                if (this.RotationAnchor < amount)
                {
                    this.Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
                }
                else
                {

                    this.IsSpinning = false;
                    this.RotationAnchor = 0f;
                    return;
                }

            }


            if (amount < 0)
            {
                if (this.RotationAnchor > amount)
                {
                    this.Rotation -= (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
                }
                else
                {
                    this.IsSpinning = false;
                    return;
                }
            }

        }

        public void Eject(GameTime gameTime)
        {

        }

        public void PlayOnce(GameTime gameTime, Vector2 position)
        {
            Position = position;
            destinationVector = new Vector2(position.X + this.OffSetX, position.Y + this.OffSetY);

            this.AnimationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.AnimationTimer <= 0)
            {

                    this.CurrentFrameCounter++;
                

                this.AnimationTimer = this.AnimationSpeed;
            }
            if (this.CurrentFrameCounter == this.TotalFrames)
            {
                this.CurrentFrameCounter = 0;
                this.IsAnimated = false;
            }

            if(this.ChangesFrames)
            {
                this.CurrentFrame = CurrentFrameCounter;
            }
            this.SourceRectangle = new Rectangle((int)(this.FirstFrameX + this.FrameWidth * this.CurrentFrame), (int)this.FirstFrameY, (int)this.FrameWidth, (int)this.FrameHeight);

        }
        public void SetFrame(int newFrame)
        {
            this.CurrentFrame = newFrame;
            this.SourceRectangle = new Rectangle((int)(this.FirstFrameX + this.FrameWidth * this.CurrentFrame), (int)this.FirstFrameY, (int)this.FrameWidth, (int)this.FrameHeight);
            //DestinationRectangle = new Rectangle((int)this.Position.X + OffSetX, (int)this.Position.Y + OffSetY, FrameWidth, FrameHeight);
        }

        public void Flash(GameTime gameTime, float duration, Color color)
        {
            duration -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (duration >= 0)
            {
                this.Color = color;
            }
            else
            {
                this.Color = Color.White;
            }

        }

        public void Update(GameTime gameTime, Dir direction)
        {
            throw new NotImplementedException();
        }

        public void UpdateAnimationTool(GameTime gameTime, float amount, float speed)
        {
            Spin(gameTime, amount, speed);
        }

        public void Shuff(GameTime gameTime, int direction)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void SelfDestruct()
        {
            throw new NotImplementedException();
        }
    }
}
