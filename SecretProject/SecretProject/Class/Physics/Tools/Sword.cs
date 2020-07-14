using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private Vector2 SwordHandle { get; set; }
        private Vector2 SwordLength { get; set; }

        public int Damage { get; set; }

        public Sword(ICollidable entityData, Vector2 entityPosition, Sprite swordSprite, int damage, float? swordLength, Vector2? customSwordLength)
        {
            this.Entity = entityData;
            this.SwordHandle = entityPosition;
            if (swordLength != 0)
            {
                this.SwordLength = new Vector2((float)swordLength, 0);
            }
            else
            {
                this.SwordLength = (Vector2)customSwordLength;
            }
            this.Damage = damage;
            CreateBody();

            this.Sprite = swordSprite;
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
            joint.Enabled = true;
            this.Joint = joint;

        }

        public void Update(GameTime gameTime)
        {
            this.Sprite.Update(gameTime);
            this.Sprite.Rotation = Joint.JointAngle;
            this.Sprite.Origin = this.CollisionBody.Position;
            
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            this.Sprite.Draw(spriteBatch, layerDepth);
        }

        public static Sword CreateSword(GraphicsDevice graphics, ICollidable holder, ItemData itemData = null)
        {
            Texture2D texture;
            Sword sword;
            int damage;
            Rectangle sourceRectangle;
            Sprite swordSprite;
            // if (itemData != null)
            //  {
            texture = Game1.AllTextures.ItemSpriteSheet;
            sourceRectangle = Game1.ItemVault.GenerateNewItem(itemData.ID, null).SourceTextureRectangle;
            damage = itemData.Damage;

            //  }
            swordSprite = new Sprite(graphics, texture, sourceRectangle, holder.CollisionBody.Position, (int)sourceRectangle.Width, (int)sourceRectangle.Height);
            sword = new Sword(holder, holder.CollisionBody.Position, swordSprite, damage, sourceRectangle.Width, null);

            return sword;
        }
    }
}
