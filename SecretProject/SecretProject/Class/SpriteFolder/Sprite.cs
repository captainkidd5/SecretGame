using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.NPCStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public Vector2 Origin { get; set; }
        public int ID { get; set; }
        public float Rotation { get; set; }
        public float RotationAnchor { get; set; }
        public bool IsSpinning { get; set; }
        public float SpinAmount { get; set; }
        public float SpinSpeed { get; set; }

        Vector2 destinationVector = new Vector2(0, 0);

        public bool Flip { get; set; }
        //for collider
        public Rectangle Rectangle { get; set; }
        public ColliderType ColliderType { get; set; }
        public string LocationKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Dir InitialShuffDirection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEntity Entity { get; set; }
        public bool IsUpdating { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }




        //for non animated sprites
        public Sprite(GraphicsDevice graphics, Texture2D atlasTexture, Rectangle sourceRectangle, Vector2 position, int width, int height)
        {
            this.Graphics = graphics;
            this.AtlasTexture = atlasTexture;
            this.SourceRectangle = sourceRectangle;
            this.Position = position;
            this.Velocity = Vector2.Zero;
            this.Width = width;
            this.Height = height;
            this.Rotation = 0f;
            this.RotationAnchor = 0f;
            this.Origin = Game1.Utility.Origin;
            
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
            this.Rotation = 0f;
            this.RotationAnchor = 0f;
            
        }



        public void Update(GameTime gameTime)
        {
            if(IsAnimated)
            {
                UpdateAnimations(gameTime,Position);
            }
            // this.Rectangle = new Rectangle((int)position.X, (int)position.Y, (int)(Width * TextureScaleX), (int)(Height * TextureScaleY));
            this.DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(Width * TextureScaleX), (int)(Height * TextureScaleY));
            this.Rectangle = DestinationRectangle;
        }


        public void Update(GameTime gameTime, Vector2 position)
        {


            if(this.IsSpinning)
            {
                Spin(gameTime, this.SpinAmount, this.SpinSpeed);
            }
                this.DestinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)(Width * TextureScaleX), (int)(Height * TextureScaleY));
            this.Rectangle = DestinationRectangle;
            this.Position = position;
            
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
            
        }

        public void UpdateAnimationPosition(Vector2 position)
        {
            destinationVector = new Vector2(position.X + OffSetX, position.Y + OffSetY);
            DestinationRectangle = new Rectangle((int)position.X + OffSetX, (int)position.Y + OffSetY, FrameWidth, FrameHeight);
            this.Rectangle = DestinationRectangle;
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
                    color: Color.White * ColorMultiplier,rotation: this.Rotation,origin: this.Origin, layerDepth: layerDepth, scale:new Vector2(TextureScaleX, TextureScaleY));

            
        }

        public void DrawFromUIToWorld(SpriteBatch spriteBatch, float layerDepth)
        {
            DestinationRectangle = new Rectangle((int)this.Position.X , (int)this.Position.Y , (int)(Width * TextureScaleX), (int)(Height * TextureScaleY));

            spriteBatch.Draw(AtlasTexture, sourceRectangle: SourceRectangle, destinationRectangle: DestinationRectangle,
                    color: Color.White * ColorMultiplier, rotation: this.Rotation, origin: this.Origin, layerDepth: 1f, scale: new Vector2(TextureScaleX, TextureScaleY));
        }

        public void DrawAnimation(SpriteBatch spriteBatch, Vector2 currentPosition, float layerDepth)
        {
           if(this.Flip)
            {
                spriteBatch.Draw(AtlasTexture, destinationVector, sourceRectangle: SourceRectangle,
                    color: this.Color * ColorMultiplier,effects: SpriteEffects.FlipHorizontally, layerDepth: layerDepth, scale: new Vector2(TextureScaleX, TextureScaleY));
            }
            else
            {
                spriteBatch.Draw(AtlasTexture, destinationVector, sourceRectangle: SourceRectangle,
                    color: this.Color * ColorMultiplier, layerDepth: layerDepth, scale: new Vector2(TextureScaleX, TextureScaleY));
            }
            
        }

        //for ship sprites
        public void DrawRotationalSprite(SpriteBatch spriteBatch, Vector2 position,float rotation, Vector2 origin, float layerDepth)
        {

            spriteBatch.Draw(this.AtlasTexture, position, this.SourceRectangle, Color.White, rotation, origin, 1f,SpriteEffects.None, layerDepth );
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

        public void Spin(GameTime gameTime, float amount, float speed)
        {
            if(amount >  0)
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
            

            if(amount < 0)
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
            this.Position = position;
            this.destinationVector = new Vector2(position.X + OffSetX, position.Y + OffSetY);

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

        public void Update(GameTime gameTime, Dir direction)
        {
            throw new NotImplementedException();
        }

        public void UpdateAnimationTool(GameTime gameTime,float amount, float speed)
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
