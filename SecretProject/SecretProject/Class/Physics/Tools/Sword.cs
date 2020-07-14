using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Dynamics.Joints;
using VelcroPhysics.Factories;
using XMLData.ItemStuff;

namespace SecretProject.Class.Physics.Tools
{

    public class Sword : ICollidable, ITool
    {
        public Body CollisionBody { get; set; }
        public ICollidable Entity { get; set; }
        private RevoluteJoint Joint { get; set; }

        private Sprite Sprite { get; set; }

        private Vector2 SwordTip { get; set; }
        private Vector2 SwordLength { get; set; }

        public int Damage { get; set; }

        public Sword(ICollidable entityData, Vector2 entityPosition, Sprite swordSprite, int damage, float? swordLength, Vector2? customSwordLength)
        {
            this.Entity = entityData;

            if (swordLength != 0)
            {
                this.SwordLength = new Vector2((float)swordLength, 0);
            }
            else
            {
                this.SwordLength = (Vector2)customSwordLength;
            }
            this.SwordTip = new Vector2(entityPosition.X + SwordLength.X);
            this.Damage = damage;
            CreateBody();

            this.Sprite = swordSprite;
        }
        public void CreateBody()
        {
            Body entityStaticBody = BodyFactory.CreateRectangle(Game1.VelcroWorld, 16, 16, 1f);
            entityStaticBody.Position = this.Entity.CollisionBody.Position;
            entityStaticBody.BodyType = BodyType.Static;
            entityStaticBody.IgnoreGravity = true;

            this.CollisionBody = BodyFactory.CreateRectangle(Game1.VelcroWorld, 1, (int)SwordLength.X, 1f);
            this.CollisionBody.BodyType = BodyType.Dynamic;
            this.CollisionBody.Position = entityStaticBody.Position;
            this.CollisionBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Weapon;
            this.CollisionBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Enemy | VelcroPhysics.Collision.Filtering.Category.Solid;
            this.CollisionBody.IgnoreGravity = true;
            this.CollisionBody.OnCollision += OnCollision;

            RevoluteJoint joint = JointFactory.CreateRevoluteJoint(Game1.VelcroWorld,
                this.CollisionBody, entityStaticBody, entityStaticBody.Position);
            joint.LocalAnchorA = new Vector2(0, 0);
            joint.LocalAnchorB = new Vector2(0, 0);
            joint.ReferenceAngle = 0;

            joint.MotorEnabled = true;
            //joint.LowerLimit
            joint.MotorSpeed = 40;

            joint.MaxMotorTorque = 100;
            joint.Enabled = true;
            // joint.
            this.Joint = joint;
            Game1.CurrentStage.DebuggableShapes.Add(new RectangleDebugger(CollisionBody, Game1.CurrentStage.DebuggableShapes));
        }

        private void OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            Console.WriteLine("sword collided");
        }

        public void Update(GameTime gameTime)
        {
            this.Sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            this.Sprite.DrawRotationalSprite(spriteBatch, Sprite.Position, MathHelper.ToDegrees(Joint.JointAngle),
                Sprite.Origin, layerDepth);
            

        }

        public static Sword CreateSword(GraphicsDevice graphics, ICollidable holder, ItemData itemData = null)
        {
            Texture2D texture;
            Sword sword;
            int damage;
            Rectangle sourceRectangle;
            Sprite swordSprite;

            texture = Game1.AllTextures.ItemSpriteSheet;
            sourceRectangle = Game1.ItemVault.GenerateNewItem(itemData.ID, null).SourceTextureRectangle;
            damage = itemData.Damage;


            //Vector2 centralPosition = new Vector2(holder.CollisionBody.Position.X + 16, holder.CollisionBody.Position.Y + 16);
            swordSprite = new Sprite(graphics, texture, sourceRectangle, holder.CollisionBody.Position, (int)sourceRectangle.Width, (int)sourceRectangle.Height) { Origin = new Vector2(16, 16) };
            sword = new Sword(holder, holder.CollisionBody.Position, swordSprite, damage, sourceRectangle.Width, null);

            return sword;
        }

        public void Remove()
        {
            Game1.VelcroWorld.RemoveBody(this.CollisionBody);
            
            //Game1.VelcroWorld.RemoveJoint(this.Joint);
        }


    }
}
