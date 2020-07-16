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

    public class Tool : ICollidable
    {
        public Body CollisionBody { get; set; }
        public ICollidable Entity { get; set; }
        protected RevoluteJoint Joint { get; set; }

        private Sprite Sprite { get; set; }

        protected Vector2 Tip { get; set; }
        protected Vector2 ToolLength { get; set; }

        public int Damage { get; set; }

        protected Dir SwingDirection { get; set; }

        public Vector2 JointCenter { get; set; }

        public float BaseRotation { get; set; } = 3 * MathHelper.Pi / 4; //sowrds are tilted to the top left in the sprite sheet. Need to rotate them back to 0.

        public Tool(ICollidable entityData, Vector2 entityPosition, Sprite swordSprite, int damage, Dir direction, float? toolLength, Vector2? customToolLength)
        {
            this.Entity = entityData;

            if (toolLength != 0)
            {
                this.ToolLength = new Vector2((float)toolLength, 0);
            }
            else
            {
                this.ToolLength = (Vector2)customToolLength;
            }
            this.Tip = new Vector2(entityPosition.X + ToolLength.X);
            this.Damage = damage;

            this.SwingDirection = direction;
            this.Sprite = swordSprite;
            CreateBody();


        }

        /// <summary>
        /// Create joint of which the tool image will rotate around.
        /// </summary>
        /// <param name="staticBody">The "pinboard" on which to stick the rotating shape.</param>
        protected virtual void CreateJoint(Body staticBody)
        {
            RevoluteJoint joint = JointFactory.CreateRevoluteJoint(Game1.VelcroWorld,
               staticBody, this.CollisionBody, new Vector2(staticBody.Position.X, staticBody.Position.Y)); //Joints connect bodies, not fixtures.
            joint.LocalAnchorA = new Vector2(0, 0);
            joint.LocalAnchorB = new Vector2(-16, 0); //create pivot point on left side of rectangle, but in middle of anchor circle.
            float referenceAngle = 0;

            // joint.an

            joint.MotorEnabled = true;

            //joint.LowerLimit
            float motorSpeed = 0f;

            float torque;
            switch (SwingDirection)
            {
                case Dir.Down:
                    motorSpeed = (float)(Math.PI * 2 * -1); //half rotation per second Backward.
                    torque = 400000;
                    CollisionBody.Rotation = (float)(Math.PI); // start quarter a rotation earlier, e.a on left side of player
                    break;
                case Dir.Up:
                    motorSpeed = (float)(Math.PI * 2); //half rotation per second Foward.
                    torque = 100000;
                    CollisionBody.Rotation = (float)(Math.PI); // start quarter a rotation earlier, e.a on left side of player
                    break;
                case Dir.Left:
                    torque = 100000;
                    motorSpeed = (float)(Math.PI * 2 * -1); //half rotation per second backwards.
                    CollisionBody.Rotation = (float)(Math.PI * 3 / 2); // start quarter a rotation earlier, e.a on top side of player
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
        }
        public virtual void CreateBody()
        {
            Body entityStaticBody = BodyFactory.CreateCircle(Game1.VelcroWorld, 1, 1f);
            entityStaticBody.Position = new Vector2(this.Entity.CollisionBody.Position.X, this.Entity.CollisionBody.Position.Y - 16);//so that its halfway up the sprite, where the hands usually are!
            entityStaticBody.BodyType = BodyType.Static;
            entityStaticBody.IgnoreGravity = true;
            JointCenter = entityStaticBody.Position;
            //entityStaticBody.IsSensor = true;

            this.CollisionBody = BodyFactory.CreateRectangle(Game1.VelcroWorld, 16, 4, 1f);

            this.CollisionBody.Position = new Vector2(entityStaticBody.Position.X + 16, entityStaticBody.Position.Y); //Move rectangle entirely outside of circle. 

            this.CollisionBody.BodyType = BodyType.Dynamic;

            this.CollisionBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Weapon;
            this.CollisionBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Enemy |
                VelcroPhysics.Collision.Filtering.Category.Solid;
            this.CollisionBody.IgnoreGravity = true;
            CollisionBody.Mass = .5f;

            this.CollisionBody.OnCollision += OnCollision;
            CollisionBody.FixedRotation = false;

            CreateJoint(entityStaticBody);
            Game1.CurrentStage.DebuggableShapes.Add(new RectangleDebugger(CollisionBody, Game1.CurrentStage.DebuggableShapes));
            Sprite.Position = CollisionBody.Position + Joint.LocalAnchorB;
        }

        protected virtual void OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            Console.WriteLine("sword collided");
        }

        public virtual void Update(GameTime gameTime)
        {
            this.Sprite.Update(gameTime);

        }

        public virtual void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            this.Sprite.DrawRotationalSprite(spriteBatch, JointCenter, BaseRotation + CollisionBody.Rotation,
                Sprite.Origin, layerDepth);

        }

        public static Tool CreateTool(GraphicsDevice graphics, ICollidable holder, Dir direction, ItemData itemData = null)
        {
           
            Texture2D texture;
            Tool tool;
            int damage;
            Rectangle sourceRectangle;
            Sprite sprite;

            texture = Game1.AllTextures.ItemSpriteSheet;
            sourceRectangle = Game1.ItemVault.GetSourceRectangle(itemData.ID); ;
            damage = itemData.Damage;

            sprite = new Sprite(graphics, texture, sourceRectangle, holder.CollisionBody.Position, (int)sourceRectangle.Width, (int)sourceRectangle.Height) { Origin = new Vector2(16, 16) }; //handle is in bottom right hand corner of sword sprites.

            switch (itemData.Type)
            {
                case ItemType.Sword:
                    tool = new Sword(holder, holder.CollisionBody.Position, sprite, damage, direction, sourceRectangle.Width, null);
                    break;
                case ItemType.Axe:
                    tool = new Axe(holder, holder.CollisionBody.Position, sprite, damage, direction, sourceRectangle.Width, null);
                    break;
                default:
                    tool = new Sword(holder, holder.CollisionBody.Position, sprite, damage, direction, sourceRectangle.Width, null);
                    break;
            }
            return tool;
        }

        public virtual void Remove()
        {
            Game1.VelcroWorld.RemoveBody(this.CollisionBody);
            this.CollisionBody = null;

        }


    }
}
