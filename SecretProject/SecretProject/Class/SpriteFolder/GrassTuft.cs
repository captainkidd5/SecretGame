using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Misc;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Physics;
using SecretProject.Class.Playable;
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
using VelcroPhysics.Utilities;

namespace SecretProject.Class.SpriteFolder
{
    public class GrassTuft : IEntity
    {
        private GraphicsDevice Graphics { get; set; }
        public int GrassType { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle DestinationRectangle { get; set; }


        public float YOffSet { get; set; }

        public Rectangle SourceRectangle { get; set; }

        public string LocationKey { get; set; }

        public bool IsUpdating { get; set; }
        public IEntity Entity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected Texture2D rectangleTexture;
        public List<GrassTuft> TuftsIsPartOf { get; set; }

        public float LayerDepth { get; set; }

        public Vector2 GrassOffset { get; set; }


        public Body CollisionBody { get; set; }


        public bool BodyLoaded { get; set; }



        public float Rotation { get; set; }
        public float RotationCap { get; set; }
        public bool StartShuff { get; set; }
        public float ShuffSpeed { get; set; }

        public Dir InitialShuffDirection { get; set; }
        public Dir ShuffDirection { get; set; }
        public bool ShuffDirectionPicked { get; set; }

        public GrassTuft(GraphicsDevice graphics, int grassType, Vector2 position, TmxStageBase stage)
        {
            this.Graphics = graphics;
            this.GrassType = grassType;
            this.Position = position;
            this.DestinationRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, 16, 32);

            this.YOffSet = Game1.Utility.RFloat(.00000001f, Utility.ForeGroundMultiplier);


            this.RotationCap = .25f;
            this.ShuffSpeed = 2f;

            this.IsUpdating = false;


            int Column = 534 % 100;
            int Row = (int)Math.Floor((double)534 / (double)100);
            this.SourceRectangle = new Rectangle(16 * Column + 16 * this.GrassType, 16 * Row - 16, 16, 32);



            this.LayerDepth = .5f + (this.DestinationRectangle.Y + 8) * Utility.ForeGroundMultiplier + this.YOffSet;
            this.GrassOffset = new Vector2(8, 24);

            if (!BodyLoaded)
                CreateBody(stage);

        }

        public void CreateBody(TmxStageBase stage)
        {
            // float radius = 4f;
            //GRASS ITSELF
            this.CollisionBody = BodyFactory.CreateRectangle(Game1.VelcroWorld, DestinationRectangle.Width / 2, 2, 1f);

            this.CollisionBody.Position = new Vector2(this.DestinationRectangle.X + SourceRectangle.Width / 8,
               this.DestinationRectangle.Y + SourceRectangle.Height / 4);
            CollisionBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Solid;
            CollisionBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Player | VelcroPhysics.Collision.Filtering.Category.Weapon;

            CollisionBody.BodyType = BodyType.Static;
            CollisionBody.Restitution = 0f;
            CollisionBody.IgnoreGravity = true;
            CollisionBody.Mass = 1f;
            CollisionBody.IsSensor = true;
            CollisionBody.Friction = .2f;
            this.CollisionBody.OnCollision += OnCollision;
            this.CollisionBody.OnSeparation += OnSeparation;


            stage.DebuggableShapes.Add(new RectangleDebugger(CollisionBody, stage.DebuggableShapes));

            BodyLoaded = true;



        }

        public void UnloadBody()
        {
            this.CollisionBody = null;

            this.BodyLoaded = false;
        }



        private void OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            if (fixtureB.CollisionCategories == VelcroPhysics.Collision.Filtering.Category.Weapon)
            {
                SelfDestruct();
            }
            else
            {


                if (!this.StartShuff && !this.ShuffDirectionPicked)
                {
                    Vector2 linearVelocity = fixtureB.Body.LinearVelocity;

                    if (linearVelocity.X > 0)
                        this.InitialShuffDirection = Dir.Right;
                    else
                        this.InitialShuffDirection = Dir.Left;
                    this.StartShuff = true;
                    Game1.CurrentStage.UpdatingGrassTufts.Add(this);
                    Game1.Player.CurrentSpeed = Player.BaseSpeed * .25f;
                }
            }

        }

        private void OnSeparation(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            Game1.Player.CurrentSpeed = Player.BaseSpeed;
        }

        public void Update(GameTime gameTime, List<GrassTuft> tufts)
        {

            if (!this.StartShuff && !this.ShuffDirectionPicked)
            {
                this.StartShuff = true;

            }



            if (!this.StartShuff && this.ShuffDirectionPicked)
            {
                RotateBackToOrigin(gameTime, tufts);
                //this.ShuffDirectionPicked = false;
                //  this.IsUpdating = false;
            }
            if (this.StartShuff)
            {
                Shuff(gameTime, (int)this.InitialShuffDirection);
                this.ShuffDirectionPicked = true;
            }

        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {


            spriteBatch.Draw(texture, this.DestinationRectangle, this.SourceRectangle,
                Color.White, this.Rotation, this.GrassOffset, SpriteEffects.None, this.LayerDepth);


        }

        public void SelfDestruct()
        {
            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.GrassCut, true, .25f);
            TmxStageBase location = Game1.CurrentStage;
            location.ParticleEngine.ActivationTime = .25f;
            location.ParticleEngine.Color = Color.Green;
            location.ParticleEngine.EmitterLocation = new Vector2(this.DestinationRectangle.X, this.DestinationRectangle.Y - 5);
            location.ParticleEngine.LayerDepth = .5f + (this.DestinationRectangle.Y) * Utility.ForeGroundMultiplier + this.YOffSet;
            if (Game1.Utility.RGenerator.Next(0, 5) < 2)
            {
                location.AllTiles.AddItem(Game1.ItemVault.GenerateNewItem(1092, this.Position, true, Game1.CurrentStage.AllTiles.GetItems(this.Position)), this.Position);
            }
            if (Game1.Utility.RNumber(0, 3) == 1)
            {
                Game1.CurrentStage.FunBox.AddRandomGrassCreature(new Vector2(this.Position.X, this.Position.Y + 4));
            }
            
            if (this.TuftsIsPartOf != null)
                this.TuftsIsPartOf.Remove(this);

            Game1.VelcroWorld.RemoveBody(this.CollisionBody);




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

        public void RotateBackToOrigin(GameTime gameTime, List<GrassTuft> tufts)
        {
            if (this.Rotation > 0)
            {
                this.Rotation -= (float)gameTime.ElapsedGameTime.TotalSeconds / 2;

                if (this.Rotation <= 0)
                {
                    this.StartShuff = false;
                    this.IsUpdating = false;
                    this.ShuffDirectionPicked = false;
                    tufts.Remove(this);
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
                    tufts.Remove(this);
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
