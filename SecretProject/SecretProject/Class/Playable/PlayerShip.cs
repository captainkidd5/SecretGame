using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.ParticileStuff;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Playable
{
    public class PlayerShip
    {
        public ParticleEngine ParticleEngine { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Rectangle DestinationRectangle { get; set; }
        public Texture2D Texture { get; set; }
        public Sprite ShipSprite { get; set; }
        public ShipModel Model { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public float Rotation { get; set; }
        public float RotationVelocity { get; set; } = 1f;
        public float LinearVelocity { get; set; } = 2f;

        public PlayerShip(GraphicsDevice graphics, Texture2D texture)
        {
            this.Origin = new Vector2( SourceRectangle.Width / 2, SourceRectangle.Height);
            Model = new ShipModel();
            Model.AssignModel(1);
            this.Texture = texture;
            this.ShipSprite = new Sprite(graphics, this.Texture, Model.SourceRectangle, this.Position, Model.SourceRectangle.Width, Model.SourceRectangle.Height);
            ParticleEngine = new ParticleEngine(new List<Texture2D>() { Game1.AllTextures.RockParticle }, ShipSprite.Position);
            ParticleEngine.Color = Color.AliceBlue;
            
        }

        public void Update(GameTime gameTime)
        {
            if(Game1.NewKeyBoardState.IsKeyDown(Keys.A))
            {
                Rotation -= MathHelper.ToRadians(RotationVelocity);
            }
            else if(Game1.NewKeyBoardState.IsKeyDown(Keys.D))
            {
                Rotation += MathHelper.ToRadians(RotationVelocity);

            }

            Vector2 direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - Rotation), -(float)Math.Sin(MathHelper.ToRadians(90)- Rotation));
            if(Game1.NewKeyBoardState.IsKeyDown(Keys.W))
            {
                Position += direction * LinearVelocity;
                ParticleEngine.EmitterLocation = new Vector2(Position.X, Position.Y - 10);
                ParticleEngine.ActivationTime = .5f;
                
            }

            if (Game1.NewKeyBoardState.IsKeyDown(Keys.S))
            {
                Position -= direction * LinearVelocity;
            }

            if(Game1.NewKeyBoardState.IsKeyDown(Keys.LeftShift))
            {
                this.LinearVelocity = 10f;
            }
            else
            {
                this.LinearVelocity = 1f;
            }
            //Position.Normalize();

            ShipSprite.UpdateShip(gameTime, Position);
            
            ParticleEngine.Update(gameTime);

        }
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            ShipSprite.DrawRotationalSprite(spriteBatch, Position, this.Rotation, Origin, layerDepth);
            ParticleEngine.Draw(spriteBatch, layerDepth);
        }

        public void SwapModel(int id)
        {
            Model.AssignModel(id);
        }
    }
}
