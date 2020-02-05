using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.StageFolder;
using System;
using System.Collections.Generic;

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
            this.DestinationRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, 16, 32);
            this.Rotation = 0f;
            this.RotationCap = .25f;
            this.ShuffSpeed = 2f;
            this.StartShuff = false;
            this.YOffSet = Game1.Utility.RFloat(.00000001f, Game1.Utility.ForeGroundMultiplier);
            this.ShuffDirection = Dir.Left;
            this.ShuffDirectionPicked = false;

            this.Rectangle = new Rectangle(this.DestinationRectangle.X, this.DestinationRectangle.Y, 8, 8);

            this.ColliderType = ColliderType.grass;
            this.IsUpdating = false;
            SetRectangleTexture(graphics);
            this.SourceRectangle = new Rectangle(grassType * 16, 0, 16, 32);


        }
        public void Update(GameTime gameTime)
        {



            if (!this.StartShuff && !this.ShuffDirectionPicked)
            {
                this.StartShuff = true;

            }



            if (!this.StartShuff && this.ShuffDirectionPicked)
            {
                RotateBackToOrigin(gameTime);
                //this.ShuffDirectionPicked = false;
                //  this.IsUpdating = false;
            }
            if (this.StartShuff)
            {
                Shuff(gameTime, (int)this.InitialShuffDirection);
                this.ShuffDirectionPicked = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {


            spriteBatch.Draw(Game1.AllTextures.TallGrass, this.DestinationRectangle, this.SourceRectangle,
                Color.White, this.Rotation, new Vector2(8, 24), SpriteEffects.None, .5f + (this.DestinationRectangle.Y) * Game1.Utility.ForeGroundMultiplier + this.YOffSet);

        }

        public void SelfDestruct()
        {
            Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.GrassCut, true, .25f);
            ILocation location = Game1.GetCurrentStage();
            location.ParticleEngine.ActivationTime = .25f;
            location.ParticleEngine.Color = Color.Green;
            location.ParticleEngine.EmitterLocation = new Vector2(this.Rectangle.X, this.Rectangle.Y - 5);
            location.ParticleEngine.LayerDepth = .5f + (this.DestinationRectangle.Y) * Game1.Utility.ForeGroundMultiplier + this.YOffSet;
            location.AllTiles.AddItem(Game1.ItemVault.GenerateNewItem(1092, this.Position, true, Game1.GetCurrentStage().AllTiles.GetItems(this.Position)), this.Position);
            this.TuftsIsPartOf.Remove(this);
        }

        public void Shuff(GameTime gameTime, int direction)
        {
            if (!this.ShuffDirectionPicked)
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
                if (this.ShuffDirection == Dir.Right)
                {
                    if (this.Rotation < this.RotationCap + .5)
                    {
                        this.Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds * this.ShuffSpeed;
                    }
                    else if (Game1.Player.Rectangle.Intersects(this.DestinationRectangle))
                    {

                    }
                    else
                    {
                        this.StartShuff = false;
                    }


                }
                else if (this.ShuffDirection == Dir.Left)
                {
                    if (this.Rotation > this.RotationCap - 1)
                    {
                        this.Rotation -= (float)gameTime.ElapsedGameTime.TotalSeconds * this.ShuffSpeed;
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

                if (this.Rotation <= 0)
                {
                    this.StartShuff = false;
                    this.IsUpdating = false;
                    this.ShuffDirectionPicked = false;
                    return;
                }
            }
            else if (this.Rotation < 0)
            {
                this.Rotation += (float)gameTime.ElapsedGameTime.TotalSeconds / 2;
                if (this.Rotation >= 0)
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
            spriteBatch.Draw(rectangleTexture, new Vector2(this.Rectangle.X, this.Rectangle.Y), color: Color.White, layerDepth: layerDepth);
        }

        private void SetRectangleTexture(GraphicsDevice graphicsDevice)
        {
            var Colors = new List<Color>();
            for (int y = 0; y < this.Rectangle.Height; y++)
            {
                for (int x = 0; x < this.Rectangle.Width; x++)
                {
                    if (x == 0 || //left side
                        y == 0 || //top side
                        x == this.Rectangle.Width - 1 || //right side
                        y == this.Rectangle.Height - 1) //bottom side
                    {
                        Colors.Add(new Color(255, 255, 255, 255));
                    }
                    else
                    {
                        Colors.Add(new Color(0, 0, 0, 0));

                    }

                }
            }
            rectangleTexture = new Texture2D(graphicsDevice, this.Rectangle.Width, this.Rectangle.Height);
            rectangleTexture.SetData<Color>(Colors.ToArray());
        }
    }
}
