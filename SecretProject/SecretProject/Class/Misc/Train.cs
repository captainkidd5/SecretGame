using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Physics;
using SecretProject.Class.StageFolder;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace SecretProject.Class.Misc
{
    public class Train : ICollidable
    {
        public bool IsActive { get; set; }
        public Vector2 Position { get; set; }
        public Body CollisionBody { get; set; }

        private Texture2D Texture { get; set; }
        public Train()
        {
            this.Texture = Game1.AllTextures.Train;
        }

        public void SwitchStage(Stages location)
        {
           switch(location)
            {
                case Stages.TrainStation:
                    this.IsActive = true;
                    this.Position = new Vector2(240, 450);
                    CreateBody();
                    break;
                case Stages.RooltapCastle:
                    this.IsActive = true;
                    this.Position = new Vector2(120, 910);
                    CreateBody();
                    break;
            }
            this.IsActive = false;
        }
        public void CreateBody()
        {
            this.CollisionBody = BodyFactory.CreateRectangle(Game1.VelcroWorld, this.Texture.Width, this.Texture.Height,1f);
            CollisionBody.BodyType = BodyType.Dynamic;
            CollisionBody.Restitution = 0f;
            CollisionBody.Friction = .4f;
            CollisionBody.Mass = 1f;
            CollisionBody.Inertia = 0;
            CollisionBody.SleepingAllowed = true;
            CollisionBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Solid;
            CollisionBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Player |
                VelcroPhysics.Collision.Filtering.Category.Item |
                VelcroPhysics.Collision.Filtering.Category.Enemy;

            CollisionBody.IgnoreGravity = true;
        }

        public void Update(GameTime gameTime)
        {
            if(this.IsActive)
            {

            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsActive)
            {
                spriteBatch.Draw(this.Texture, this.Position, null, Color.White, 0f, Vector2.Zero,
                    1f, SpriteEffects.None, .5f + (Position.Y + Texture.Height) * Utility.ForeGroundMultiplier);
            }
        }
    }
}
