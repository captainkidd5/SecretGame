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
    public class GrassTuft : ICollidable
    {
        public int GrassType { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle DestinationRectangle { get; set; }
        public float Rotation { get; set; }
        public float RotationCap { get; set; }
        public bool StartShuff { get; set; }
        public float ShuffSpeed { get; set; }
        public float YOffSet { get; set; }

        public Dir InitialShuffDirection { get; set; }
        public Dir ShuffDirection { get; set; }
        public bool ShuffDirectionPicked { get; set; }

        public Rectangle Rectangle { get; set; }

        public Rectangle SourceRectangle { get; set; }

        public ColliderType ColliderType { get; set; }
        public string LocationKey { get; set; }

        public bool IsUpdating { get; set; }
        public IEntity Entity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected Texture2D rectangleTexture;
        public List<GrassTuft> TuftsIsPartOf { get; set; }

        public GrassTuft(GraphicsDevice graphics, int grassType, Vector2 position)
        {
            this.GrassType = grassType;
            this.Position = position;
            this.DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, 16, 32);
            this.Rotation = 0f;
            this.RotationCap = .25f;
            this.ShuffSpeed = 2f;
            this.StartShuff = false;
            this.YOffSet = Game1.Utility.RFloat(.00000001f, Game1.Utility.ForeGroundMultiplier);
            this.ShuffDirection = Dir.Left;
            this.ShuffDirectionPicked = false;

            this.Rectangle = new Rectangle(DestinationRectangle.X, DestinationRectangle.Y, 8, 8);

            this.ColliderType = ColliderType.grass;
            this.IsUpdating = false;
            SetRectangleTexture(graphics);
            this.SourceRectangle = new Rectangle(grassType * 16, 0, 16, 32);


        }
        public void Update(GameTime gameTime)
        {



            if (!StartShuff && !ShuffDirectionPicked)
            {
                this.StartShuff = true;

            }



            if (!this.StartShuff && ShuffDirectionPicked)
            {
                RotateBackToOrigin(gameTime);
                //this.ShuffDirectionPicked = false;
                //  this.IsUpdating = false;
            }
            if (this.StartShuff)
            {
                Shuff(gameTime, (int)this.InitialShuffDirection);
                ShuffDirectionPicked = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {


            spriteBatch.Draw(Game1.AllTextures.TallGrass, DestinationRectangle, SourceRectangle,
                Color.White, Rotation, new Vector2(8, 24), SpriteEffects.None, .5f + (DestinationRectangle.Y) * Game1.Utility.ForeGroundMultiplier + YOffSet);

        }

        public void SelfDestruct()
        {
            Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.BushCut);
            Game1.GetCurrentStage().ParticleEngine.ActivationTime = .25f;
            Game1.GetCurrentStage().ParticleEngine.Color = Color.Green;
            Game1.GetCurrentStage().ParticleEngine.EmitterLocation = new Vector2(this.Rectangle.X, this.Rectangle.Y - 5);
            Game1.GetCurrentStage().ParticleEngine.LayerDepth = .5f + (DestinationRectangle.Y) * Game1.Utility.ForeGroundMultiplier + YOffSet;
            TuftsIsPartOf.Remove(this);
        }

        public void Shuff(GameTime gameTime, int direction)
        {
            if (!ShuffDirectionPicked)
            {
                if (direction == (int)Dir.Up || direction == (int)Dir.Down)
                {
                    this.ShuffDirection = (Dir)Game1.Utility.RGenerator.Next(2, 4);
                }
                if (direction == (int)Dir.Right)
                {
                    this.ShuffDirection = Dir.Right;
                }
                if (direction == (int)(Dir.Left))
                {
                    this.ShuffDirection = Dir.Left;
                }

            }
            else
            {
                if (ShuffDirection == Dir.Right)
                {
                    if (this.Rotation < RotationCap + .5)
                    {
                        this.Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds * ShuffSpeed;
                    }
                    else if (Game1.Player.Rectangle.Intersects(this.DestinationRectangle))
                    {

                    }
                    else
                    {
                        this.StartShuff = false;
                    }


                }
                else if (ShuffDirection == Dir.Left)
                {
                    if (this.Rotation > RotationCap - 1)
                    {
                        this.Rotation -= (float)gameTime.ElapsedGameTime.TotalSeconds * ShuffSpeed;
                    }
                    else if (Game1.Player.Rectangle.Intersects(this.DestinationRectangle))
                    {

                    }
                    else
                    {
                        this.StartShuff = false;
                    }
                }
            }

        }

        public void RotateBackToOrigin(GameTime gameTime)
        {
            if (this.Rotation > 0)
            {
                this.Rotation -= (float)gameTime.ElapsedGameTime.TotalSeconds / 2;

                if (Rotation <= 0)
                {
                    this.StartShuff = false;
                    this.IsUpdating = false;
                    this.ShuffDirectionPicked = false;
                    return;
                }
            }
            else if (Rotation < 0)
            {
                this.Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds / 2;
                if (Rotation >= 0)
                {
                    this.StartShuff = false;
                    this.IsUpdating = false;
                    this.ShuffDirectionPicked = false;
                    return;
                }
            }
            else
            {
                this.StartShuff = false;
                this.IsUpdating = false;
                this.ShuffDirectionPicked = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(rectangleTexture, new Vector2(Rectangle.X, Rectangle.Y), color: Color.White, layerDepth: layerDepth);
        }

        private void SetRectangleTexture(GraphicsDevice graphicsDevice)
        {
            var Colors = new List<Color>();
            for (int y = 0; y < Rectangle.Height; y++)
            {
                for (int x = 0; x < Rectangle.Width; x++)
                {
                    if (x == 0 || //left side
                        y == 0 || //top side
                        x == Rectangle.Width - 1 || //right side
                        y == Rectangle.Height - 1) //bottom side
                    {
                        Colors.Add(new Color(255, 255, 255, 255));
                    }
                    else
                    {
                        Colors.Add(new Color(0, 0, 0, 0));

                    }

                }
            }
            rectangleTexture = new Texture2D(graphicsDevice, Rectangle.Width, Rectangle.Height);
            rectangleTexture.SetData<Color>(Colors.ToArray());
        }
    }
}
