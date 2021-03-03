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
                this.CollisionBody.LinearVelocity = PrimaryVelocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                PrimaryVelocity = new Vector2(10, 0);
            }
            else
            {
                PrimaryVelocity = new Vector2(0, 0);
                IsArriving = false;
            }
        }

        public void SwitchStage(StagesEnum oldLocation, StagesEnum newLocation)
        {
            
            
            switch (newLocation)
            {
                case StagesEnum.TrainStation:

                    if(oldLocation == StagesEnum.Town) //Train is already at station.
                    {

                        this.Position = TrainStationArrivalPosition;
                    }
                    else if(oldLocation == StagesEnum.RooltapCastle) //Train is arriving at Train Station.
                    {
                        this.Position = new Vector2(-400, TrainStationArrivalPosition.Y);
                        this.IsArriving = true;

                        
                    }
                    this.IsActive = true;
                    CreateBody();

                    break;
                case StagesEnum.RooltapCastle:
                    if(oldLocation == StagesEnum.TrainStation) //Train is arriving at the castle.
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
            CollisionBody.Friction = 5f;
            CollisionBody.Mass = 15f;
            CollisionBody.Inertia = 0;
            CollisionBody.SleepingAllowed = true;
            CollisionBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Solid;
            //CollisionBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Player; 

            CollisionBody.IgnoreGravity = true;
            CollisionBody.Position = this.Position;
            Game1.CurrentStage.DebuggableShapes.Add(new RectangleDebugger(CollisionBody, Game1.CurrentStage.DebuggableShapes));
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
