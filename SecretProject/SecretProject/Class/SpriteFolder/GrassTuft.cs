using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Misc;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.StageFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Handlers;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Dynamics.Joints;
using VelcroPhysics.Factories;

namespace SecretProject.Class.SpriteFolder
{
    public class GrassTuft : IEntity
    {
        private GraphicsDevice Graphics { get; set; }
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

        public float LayerDepth { get; set; }

        public Vector2 GrassOffset { get; set; }

        public RectangleCollider RectangleCollider;

        public Body Body { get; set; }
        public Body PinBox { get; set; }

        public GrassTuft(GraphicsDevice graphics, int grassType, Vector2 position, TmxStageBase stage)
        {
            this.Graphics = graphics;
            this.GrassType = grassType;
            this.Position = position;
            this.DestinationRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, 16, 32);
            this.Rotation = 0f;
            this.RotationCap = .25f;
            this.ShuffSpeed = 2f;
            this.StartShuff = false;
            this.YOffSet = Game1.Utility.RFloat(.00000001f, Utility.ForeGroundMultiplier);
            this.ShuffDirection = Dir.Left;
            this.ShuffDirectionPicked = false;

            this.Rectangle = new Rectangle(this.DestinationRectangle.X, this.DestinationRectangle.Y, 8, 8);

            this.ColliderType = ColliderType.grass;
            this.IsUpdating = false;


            int Column = 534 % 100;
            int Row = (int)Math.Floor((double)534 / (double)100);
            this.SourceRectangle = new Rectangle(16 * Column + 16 * this.GrassType, 16 * Row - 16, 16, 32);



            this.LayerDepth = .5f + (this.DestinationRectangle.Y + 8) * Utility.ForeGroundMultiplier + this.YOffSet;
            this.GrassOffset = new Vector2(8, 24);

            this.RectangleCollider = new RectangleCollider(graphics, this.Rectangle, this, ColliderType.grass);

            Rectangle PinBoxRectangle = SecretPhysics.CreatePinBox(position, 32, 32);
            //CONNECTOR

            PinBox = BodyFactory.CreateRectangle(Game1.VelcroWorld, PinBoxRectangle.Width, PinBoxRectangle.Height, 1f);
            PinBox.Position = new Vector2(this.Position.X + PinBoxRectangle.Width / 2f, this.Position.Y + PinBoxRectangle.Height / 2);
            PinBox.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Pinboard;
            PinBox.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Grass;
            PinBox.BodyType = BodyType.Static;
            PinBox.Restitution = .2f;
            PinBox.Mass = 1f;
            PinBox.Friction = .8f;
  


            //GRASS ITSELF
            this.Body = BodyFactory.CreateRectangle(Game1.VelcroWorld, this.Rectangle.Width, this.Rectangle.Height, 1f, 
                new Vector2(this.Rectangle.X + SourceRectangle.Width/2, this.Rectangle.Y + SourceRectangle.Height / 2));
            Body.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Grass;
            Body.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Player;
            Body.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Mouse;
            Body.BodyType = BodyType.Dynamic;
            Body.Restitution = .7f;
            Body.Mass = .2f;
            Body.Friction = .2f;
          //  this.Body.OnCollision += OnCollision;
            this.Body.FixedRotation = false;

            float damp = .23f, hz = 17;

            WeldJoint root = JointFactory.CreateWeldJoint(Game1.VelcroWorld, this.Body, PinBox, new Vector2(this.PinBox.Position.X, this.PinBox.Position.Y - Rectangle.Height /2),
                 new Vector2(this.PinBox.Position.X, this.PinBox.Position.Y - Rectangle.Height / 2), true);
            root.CollideConnected = false;
            root.FrequencyHz = hz;
            root.DampingRatio = damp;

        }

        private void DrawDebugLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness = 2f)
        {
            Vector2 delta = end - start;
            float rot = (float)Math.Atan2(delta.Y, delta.X);
            spriteBatch.Draw(Game1.AllTextures.redPixel, start, new Rectangle(0, 0, 1, 1),
                color, rot,new Vector2(0,.5f), new Vector2(delta.Length(), thickness), SpriteEffects.None, .99f);
        }

        private void OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
           // fixtureA.
            Console.WriteLine("hi");
           // SelfDestruct();
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
                Shuff((int)this.InitialShuffDirection);
                this.ShuffDirectionPicked = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {

            Vector2 grassVector = ((Body.Position - new Vector2(PinBox.Position.X, PinBox.Position.Y)) * 2);
            Vector2 realPos = PinBox.Position + grassVector;
            DrawDebugLine(spriteBatch, PinBox.Position, realPos, Color.Green);
            spriteBatch.Draw(texture, this.DestinationRectangle, this.SourceRectangle,
                Color.White, this.Body.Rotation, this.GrassOffset, SpriteEffects.None, this.LayerDepth);

        }

        public void SelfDestruct()
        {
            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.GrassCut, true, .25f);
            TmxStageBase location = Game1.CurrentStage;
            location.ParticleEngine.ActivationTime = .25f;
            location.ParticleEngine.Color = Color.Green;
            location.ParticleEngine.EmitterLocation = new Vector2(this.Rectangle.X, this.Rectangle.Y - 5);
            location.ParticleEngine.LayerDepth = .5f + (this.DestinationRectangle.Y) * Utility.ForeGroundMultiplier + this.YOffSet;
            if (Game1.Utility.RGenerator.Next(0, 5) < 2)
            {
                location.AllTiles.AddItem(Game1.ItemVault.GenerateNewItem(1092, this.Position, true, Game1.CurrentStage.AllTiles.GetItems(this.Position)), this.Position);
            }
            if (Game1.Utility.RNumber(0, 3) == 1)
            {
                Game1.CurrentStage.FunBox.AddRandomGrassCreature(new Vector2(this.Position.X, this.Position.Y + 4));
            }
            Game1.VelcroWorld.RemoveBody(this.Body);
            if (this.TuftsIsPartOf != null)
                this.TuftsIsPartOf.Remove(this);






        }

        public void Shuff(int direction)
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
                        this.Rotation += (float).5f * this.ShuffSpeed;
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
                        this.Rotation -= .5f * this.ShuffSpeed;
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
            //  spriteBatch.Draw(rectangleTexture, new Vector2(this.Rectangle.X, this.Rectangle.Y), color: Color.White, layerDepth: layerDepth);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void DamageCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom)
        {
            throw new NotImplementedException();
        }

        public void MouseCollisionInteraction()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
