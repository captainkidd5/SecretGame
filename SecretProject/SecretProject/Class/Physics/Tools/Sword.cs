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

        private Dir SwingDirection { get; set; }

        public Sword(ICollidable entityData, Vector2 entityPosition, Sprite swordSprite, int damage, Dir direction,float? swordLength, Vector2? customSwordLength)
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

            this.SwingDirection = direction;
            this.Sprite = swordSprite;
            CreateBody();

           
        }
        public void CreateBody()
        {
            Body entityStaticBody = BodyFactory.CreateCircle(Game1.VelcroWorld, 5,1f);
            entityStaticBody.Position = new Vector2(this.Entity.CollisionBody.Position.X, this.Entity.CollisionBody.Position.Y - 8);//so that its halfway up the sprite, where the hands usually are!
            entityStaticBody.BodyType = BodyType.Static;
            entityStaticBody.IgnoreGravity = true;
            entityStaticBody.IsSensor = true;

            this.CollisionBody = BodyFactory.CreateRectangle(Game1.VelcroWorld, 2, 32, 1f);

            this.CollisionBody.Position = new Vector2(entityStaticBody.Position.X, entityStaticBody.Position.Y); //X + 16 so the end of the rectangle is attached to the side of the static one

            this.CollisionBody.BodyType = BodyType.Dynamic;
            
            this.CollisionBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Weapon;
            this.CollisionBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Enemy | VelcroPhysics.Collision.Filtering.Category.Solid;
            this.CollisionBody.IgnoreGravity = true;
            this.CollisionBody.OnCollision += OnCollision;
            CollisionBody.FixedRotation = false;

            RevoluteJoint joint = JointFactory.CreateRevoluteJoint(Game1.VelcroWorld,
                entityStaticBody, this.CollisionBody, new Vector2(entityStaticBody.Position.X , entityStaticBody.Position.Y));
            joint.LocalAnchorA = new Vector2(0, 0);
            joint.LocalAnchorB = new Vector2(0, 16);
            float referenceAngle = (float)Math.PI;
            
           // joint.an

            joint.MotorEnabled = true;

            //joint.LowerLimit
            float motorSpeed = 0f;
            
            float torque;
            switch(SwingDirection)
            {
                case Dir.Left:
                    torque = 6000;
                    motorSpeed = -1500;
                    break;
                case Dir.Right:
                    torque = 6000;
                   // referenceAngle = (float)Math.PI;
                    motorSpeed = 1500;
                    break;

                default:
                    torque = 0;
                    break;
            }
            joint.ReferenceAngle = referenceAngle;
            joint.MotorSpeed = motorSpeed;
            joint.MaxMotorTorque = torque;
            joint.Enabled = true;
            // joint.
            this.Joint = joint;
            Game1.CurrentStage.DebuggableShapes.Add(new RectangleDebugger(CollisionBody, Game1.CurrentStage.DebuggableShapes));
            Sprite.Position = CollisionBody.Position + Joint.LocalAnchorB;
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
            this.Sprite.DrawRotationalSprite(spriteBatch, CollisionBody.Position + Joint.LocalAnchorB, CollisionBody.Rotation,
                Sprite.Origin, layerDepth);

            //body.Position = new Vector2(CollisionBody.Position.X, CollisionBody.Position.Y - 1);
            

        }

        public static Sword CreateSword(GraphicsDevice graphics, ICollidable holder, Dir direction, ItemData itemData = null)
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
            sword = new Sword(holder, holder.CollisionBody.Position, swordSprite, damage, direction, sourceRectangle.Width, null);

            return sword;
        }

        public void Remove()
        {
            Game1.VelcroWorld.RemoveBody(this.CollisionBody);
            this.CollisionBody = null;
            
            //Game1.VelcroWorld.RemoveJoint(this.Joint);
        }


    }
}
