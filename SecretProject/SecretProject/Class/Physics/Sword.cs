using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Dynamics.Joints;
using VelcroPhysics.Factories;

namespace SecretProject.Class.Physics
{
    public enum SwordLength
    {
        None = 0,
        Short = 5,
        Medium = 10,
        Long = 15
    }
    public class Sword : ICollidable
    {
        public Body CollisionBody { get; set; }
        public ICollidable Entity { get; set; }
        private RevoluteJoint Joint { get; set; }

        private Vector2 SwordHandle { get; set; }
        private Vector2 SwordLength { get; set; }

        public Sword(ICollidable entityData, Vector2 entityPosition, SwordLength? swordLength, Vector2? customSwordLength)
        {
            this.Entity = entityData;
            this.SwordHandle = entityPosition;
            if(swordLength != Physics.SwordLength.None)
            {
                this.SwordLength = GetSwordLength(((SwordLength)swordLength));
            }
            else
            {
                this.SwordLength = (Vector2)customSwordLength;
            }

            CreateBody();
        }
        public void CreateBody()
        {
            this.CollisionBody = BodyFactory.CreateEdge(Game1.VelcroWorld, SwordHandle, SwordLength, Entity);
            this.CollisionBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Weapon;
            this.CollisionBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Enemy;
            this.CollisionBody.IgnoreGravity = true;

            RevoluteJoint joint = JointFactory.CreateRevoluteJoint(Game1.VelcroWorld, this.Entity.CollisionBody,
                this.CollisionBody, this.Entity.CollisionBody.Position);
            joint.MotorSpeed = MathHelper.PiOver2;
            joint.MaxMotorTorque = 10;

        }

        public void Swing()
        {
            this.Joint.Enabled = true;
        }

        public Vector2 GetSwordLength(SwordLength swordLength)
        {
            return new Vector2((int)swordLength, 0);
        }
    }
}
