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

        public Vector2 JointCenter { get; set; }

        public float BaseRotation { get; set; } = 3 * MathHelper.Pi / 4; //sowrds are tilted to the top left in the sprite sheet. Need to rotate them back to 0.

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
            Body entityStaticBody = BodyFactory.CreateCircle(Game1.VelcroWorld, 1,1f);
            entityStaticBody.Position = new Vector2(this.Entity.CollisionBody.Position.X, this.Entity.CollisionBody.Position.Y -16);//so that its halfway up the sprite, where the hands usually are!
            entityStaticBody.BodyType = BodyType.Static;
            entityStaticBody.IgnoreGravity = true;
            JointCenter = entityStaticBody.Position;
            //entityStaticBody.IsSensor = true;

            this.CollisionBody = BodyFactory.CreateRectangle(Game1.VelcroWorld, 16, 4, 1f);

            this.CollisionBody.Position = new Vector2(entityStaticBody.Position.X + 16, entityStaticBody.Position.Y); //X + 

            this.CollisionBody.BodyType = BodyType.Dynamic;
            
            this.CollisionBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Weapon;
            this.CollisionBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Enemy |
                VelcroPhysics.Collision.Filtering.Category.Solid;
            this.CollisionBody.IgnoreGravity = true;
            CollisionBody.Mass = .5f;
            
            this.CollisionBody.OnCollision += OnCollision;
            CollisionBody.FixedRotation = false;

            RevoluteJoint joint = JointFactory.CreateRevoluteJoint(Game1.VelcroWorld,
                entityStaticBody, this.CollisionBody, new Vector2(entityStaticBody.Position.X , entityStaticBody.Position.Y)); //Joints connect bodies, not fixtures.
            joint.LocalAnchorA = new Vector2(0, 0);
            joint.LocalAnchorB = new Vector2(-16, 0); //create pivot point on left side of rectangle, but in middle of anchor circle.
            float referenceAngle = 0;
            
           // joint.an

            joint.MotorEnabled = true;

            //joint.LowerLimit
            float motorSpeed = 0f;
            
            float torque;
            switch(SwingDirection)
            {
                case Dir.Down:
                    motorSpeed = (float)(Math.PI * 2 * -1); //half rotation per second Backward.
                    torque = 100000;
                    CollisionBody.Rotation = (float)(Math.PI); // start quarter a rotation earlier, e.a on left side of player
                    break;
                case Dir.Up:
                    motorSpeed = (float)(Math.PI * 2 ); //half rotation per second Foward.
                    torque = 100000;
                    CollisionBody.Rotation = (float)(Math.PI ); // start quarter a rotation earlier, e.a on left side of player
                    break;
                case Dir.Left:
                    torque = 100000;
                    motorSpeed = (float)(Math.PI * 2 * -1); //half rotation per second backwards.
                    CollisionBody.Rotation = (float)(Math.PI * 3/2); // start quarter a rotation earlier, e.a on top side of player
                    break;
                case Dir.Right:
                    torque = 100000;
                    motorSpeed = (float)(Math.PI * 2); //half rotation per second fowards.
                    CollisionBody.Rotation = (float)(Math.PI * 3 / 2); // start quarter a rotation earlier, e.a on top side of player
                    break;

                default:
                    torque = 0;
                    break;
            }

            joint.ReferenceAngle = referenceAngle;
            joint.MotorSpeed = motorSpeed;
            joint.MaxMotorTorque = torque;
            joint.Enabled = true;
            
            joint.CollideConnected = false;
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
            this.Sprite.DrawRotationalSprite(spriteBatch, JointCenter , BaseRotation +  CollisionBody.Rotation,
                Sprite.Origin, layerDepth);

        }

        public static Sword CreateSword(GraphicsDevice graphics, ICollidable holder, Dir direction, ItemData itemData = null)
        {
            Texture2D texture;
            Sword sword;
            int damage;
            Rectangle sourceRectangle;
            Sprite swordSprite;

            texture = Game1.AllTextures.ItemSpriteSheet;
            sourceRectangle = Game1.ItemVault.GetSourceRectangle(itemData.ID); ;
            damage = itemData.Damage;

            swordSprite = new Sprite(graphics, texture, sourceRectangle, holder.CollisionBody.Position, (int)sourceRectangle.Width, (int)sourceRectangle.Height) { Origin = new Vector2(16, 16) }; //handle is in bottom right hand corner of sword sprites.
            sword = new Sword(holder, holder.CollisionBody.Position, swordSprite, damage, direction, sourceRectangle.Width, null);

            return sword;
        }

        public void Remove()
        {
            Game1.VelcroWorld.RemoveBody(this.CollisionBody);
            this.CollisionBody = null;
            
        }


    }
}
