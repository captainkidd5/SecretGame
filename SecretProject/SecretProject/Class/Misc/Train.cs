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
        public readonly Vector2 TrainStationArrivalPosition = new Vector2(240, 385); //When train is arriving at train station, stop here.
        public readonly Vector2 CastleArrivePosition = new Vector2(120, 850);

        public bool IsActive { get; set; }
        private bool IsArriving { get; set; }
        private bool IsDeparting { get; set; }
        public Vector2 Position { get; set; }
        public Body CollisionBody { get; set; }

        private Texture2D Texture { get; set; }

        private Vector2 PrimaryVelocity { get; set; }
        public Train()
        {
            this.Texture = Game1.AllTextures.Train;
            this.PrimaryVelocity = new Vector2(10, 0);
        }

        private void DepartTrain(GameTime gameTime)
        {
            this.CollisionBody.LinearVelocity = PrimaryVelocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        private void ArriveTrain(GameTime gameTime)
        {
           // if(this.Position.Y < )
            
            if(CollisionBody.Position.X < CastleArrivePosition.X)
            {
                this.CollisionBody.ApplyForce(PrimaryVelocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                PrimaryVelocity = new Vector2(10, 0);
            }
            else
            {
                PrimaryVelocity = new Vector2(0, 0);
                IsArriving = false;
            }
        }

        public void SwitchStage(Stages oldLocation, Stages newLocation)
        {
            
            
            switch (newLocation)
            {
                case Stages.TrainStation:

                    if(oldLocation == Stages.Town) //Train is already at station.
                    {

                        this.Position = TrainStationArrivalPosition;
                    }
                    else if(oldLocation == Stages.RooltapCastle) //Train is arriving at Train Station.
                    {
                        this.Position = new Vector2(-400, TrainStationArrivalPosition.Y);
                        this.IsArriving = true;

                        
                    }
                    this.IsActive = true;
                    CreateBody();

                    break;
                case Stages.RooltapCastle:
                    if(oldLocation == Stages.TrainStation) //Train is arriving at the castle.
                    {
                        IsArriving = true;
                        this.Position = new Vector2(-400, CastleArrivePosition.Y);
                    }
                    else
                    {
                        this.Position = new Vector2(120, 850); //train is already at castle.
                    }
                    this.IsActive = true;
                 
                    CreateBody();
                    break;
                default:
                    this.IsActive = false;
                    break;
            }

        }
        public void CreateBody()
        {
            this.CollisionBody = BodyFactory.CreateRectangle(Game1.VelcroWorld, this.Texture.Width, this.Texture.Height, 1f);
            CollisionBody.BodyType = BodyType.Dynamic;
            CollisionBody.Restitution = 0f;
            CollisionBody.Friction = .4f;
            CollisionBody.Mass = 10f;
            CollisionBody.Inertia = 0;
            CollisionBody.SleepingAllowed = true;
            CollisionBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Solid;
            CollisionBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Player |
                VelcroPhysics.Collision.Filtering.Category.Item |
                VelcroPhysics.Collision.Filtering.Category.Enemy;

            CollisionBody.IgnoreGravity = true;
            CollisionBody.Position = this.Position;
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {
                if(IsArriving)
                {
                    ArriveTrain(gameTime);
                }
                else if(IsDeparting)
                {
                    DepartTrain(gameTime);
                }
                this.Position = this.CollisionBody.Position;
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
