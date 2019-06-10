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
        public float RotationVelocity { get; set; } = 3f;
        public float LinearVelocity { get; set; } = 4f;

        public PlayerShip(GraphicsDevice graphics, Texture2D texture)
        {
            this.Origin = new Vector2( SourceRectangle.Width / 2, SourceRectangle.Height / 2);
            Model = new ShipModel();
            Model.AssignModel(1);
            this.Texture = texture;
            this.ShipSprite = new Sprite(graphics, this.Texture, Model.SourceRectangle, this.Position, Model.SourceRectangle.Width, Model.SourceRectangle.Height);
            
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
            }

        }
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            ShipSprite.DrawRotationalSprite(spriteBatch, Position, this.Rotation, Origin, layerDepth);
        }

        public void SwapModel(int id)
        {
            Model.AssignModel(id);
        }
    }
}
