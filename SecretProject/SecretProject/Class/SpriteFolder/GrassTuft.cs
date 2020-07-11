using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Misc;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Physics;
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

        public ColliderType ColliderType { get; set; }
        public string LocationKey { get; set; }

        public bool IsUpdating { get; set; }
        public IEntity Entity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected Texture2D rectangleTexture;
        public List<GrassTuft> TuftsIsPartOf { get; set; }

        public float LayerDepth { get; set; }

        public Vector2 GrassOffset { get; set; }

        public RectangleCollider RectangleCollider;

        public Body RotatableBody { get; set; }
        public Body AnchorBody { get; set; }


        public bool BodyLoaded { get; set; }


        public GrassTuft(GraphicsDevice graphics, int grassType, Vector2 position, TmxStageBase stage)
        {
            this.Graphics = graphics;
            this.GrassType = grassType;
            this.Position = position;
            this.DestinationRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, 16, 32);

            this.YOffSet = Game1.Utility.RFloat(.00000001f, Utility.ForeGroundMultiplier);



            this.ColliderType = ColliderType.grass;
            this.IsUpdating = false;


            int Column = 534 % 100;
            int Row = (int)Math.Floor((double)534 / (double)100);
            this.SourceRectangle = new Rectangle(16 * Column + 16 * this.GrassType, 16 * Row - 16, 16, 32);



            this.LayerDepth = .5f + (this.DestinationRectangle.Y + 8) * Utility.ForeGroundMultiplier + this.YOffSet;
            this.GrassOffset = new Vector2(8, 24);

            this.RectangleCollider = new RectangleCollider(graphics, this.DestinationRectangle, this, ColliderType.grass);

            if (!BodyLoaded)
                LoadBody(stage);

        }

        public void LoadBody(TmxStageBase stage)
        {
            // float radius = 4f;
            //GRASS ITSELF
            this.RotatableBody = BodyFactory.CreateRectangle(Game1.VelcroWorld, DestinationRectangle.Width / 2, DestinationRectangle.Height / 2, 1f);

            this.RotatableBody.Position = new Vector2(this.DestinationRectangle.X + SourceRectangle.Width / 4,
               this.DestinationRectangle.Y + SourceRectangle.Height);
            RotatableBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Solid;
            RotatableBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Player;

            RotatableBody.BodyType = BodyType.Dynamic;
            RotatableBody.Restitution = .7f;
            RotatableBody.IgnoreGravity = true;
            this.RotatableBody.FixedRotation = false;
            RotatableBody.Mass = .2f;
            RotatableBody.Friction = .2f;
            this.RotatableBody.OnCollision += OnCollision;


            stage.DebuggableShapes.Add(new RectangleDebugger(RotatableBody, stage.DebuggableShapes));

            BodyLoaded = true;

            this.AnchorBody = BodyFactory.CreateRectangle(Game1.VelcroWorld, DestinationRectangle.Width, DestinationRectangle.Height /2, 1f);

            this.AnchorBody.Position = new Vector2(this.DestinationRectangle.X + SourceRectangle.Width / 2,
               this.DestinationRectangle.Y + SourceRectangle.Height );
            AnchorBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.None;

            AnchorBody.BodyType = BodyType.Static;
            AnchorBody.IsSensor = true;
            AnchorBody.Restitution = .7f;
            AnchorBody.IgnoreGravity = true;
            AnchorBody.Mass = .2f;
            AnchorBody.Friction = .2f;

            float damp = .05f, hz = 2f;
            WeldJoint joint = JointFactory.CreateWeldJoint(Game1.VelcroWorld, AnchorBody, RotatableBody, this.AnchorBody.Position, AnchorBody.Position,true);
            joint.CollideConnected = false;
            joint.FrequencyHz = hz;
            joint.DampingRatio = damp;
        }

        public void UnloadBody()
        {
            this.RotatableBody = null;
            this.BodyLoaded = false;
        }

        private void DrawDebugLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness = 2f)
        {
            Vector2 delta = end - start;
            float rotation = (float)Math.Atan2(delta.Y, delta.X);
            spriteBatch.Draw(Game1.AllTextures.redPixel, start, new Rectangle(0, 0, 1, 1),
                color, rotation, new Vector2(0, .5f), new Vector2(delta.Length(), thickness), SpriteEffects.None, .99f);
        }

        private void OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            //  Body.Rotation += 5f;
            // Console.WriteLine("hi");
            // SelfDestruct();
        }

        public void Update(GameTime gameTime)
        {



        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
    
  
            if (this.RotatableBody.Rotation != 0)
            {
                Console.WriteLine("hi");
            }

            spriteBatch.Draw(texture, this.RotatableBody.Position, this.SourceRectangle,
                Color.White, MathHelper.ToDegrees(RotatableBody.Rotation), this.GrassOffset, 1f, SpriteEffects.None, this.LayerDepth);

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
            Game1.VelcroWorld.RemoveBody(this.RotatableBody);
            if (this.TuftsIsPartOf != null)
                this.TuftsIsPartOf.Remove(this);






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
