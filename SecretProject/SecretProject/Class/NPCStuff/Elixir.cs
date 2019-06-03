using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.ObjectFolder;
using SecretProject.Class.SpriteFolder;

namespace SecretProject.Class.NPCStuff
{
    public class Elixir : INPC
    {
        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public Sprite[] NPCAnimatedSprite { get; set; }
        public Texture2D Texture { get; set; } = Game1.AllTextures.ElixirSpriteSheet;

        public Rectangle NPCRectangle { get { return new Rectangle((int)Position.X, (int)Position.Y + 16, 16, 32); } }

        public float Speed { get; set; } = 1f;
        public Vector2 PrimaryVelocity { get; set; }
        public Vector2 TotalVelocity { get; set; }

        public Vector2 DirectionVector { get; set; }


        //0 = down, 1 = left, 2 =  right, 3 = up
        public int CurrentDirection { get; set; } = 3;

        public bool IsUpdating { get; set; } = false;

        public Collider Collider { get; set; }
        public bool CollideOccured { get; set; } = false;



        public int FrameNumber { get; set; } = 25;

        public Elixir(string name, Vector2 position, GraphicsDevice graphics)
        {
            this.Name = "Elixir";
            this.Position = position;
            NPCAnimatedSprite = new Sprite[4];

            NPCAnimatedSprite[0] = new Sprite(graphics, Game1.AllTextures.ElixirSpriteSheet, 48, 0, 16,48,6, .15f, this.Position);
            NPCAnimatedSprite[1] = new Sprite(graphics, Game1.AllTextures.ElixirSpriteSheet, 144, 0, 16, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[2] = new Sprite(graphics, Game1.AllTextures.ElixirSpriteSheet, 240, 0, 16, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[3] = new Sprite(graphics, Game1.AllTextures.ElixirSpriteSheet, 336, 0, 16, 48, 6, .15f, this.Position);

            Collider = new Collider(this.PrimaryVelocity, this.NPCRectangle);

        }

        public void Update(GameTime gameTime, List<ObjectBody> objects, MouseManager mouse)
        {
            this.PrimaryVelocity = new Vector2(1, 1);
            Collider.Rectangle = this.NPCRectangle;
            Collider.Velocity = this.PrimaryVelocity;
            this.CollideOccured = Collider.DidCollide(objects);

            switch (CurrentDirection)
            {
                case 0:
                    NPCAnimatedSprite[0].UpdateAnimations(gameTime,Position);
                    break;
                case 1:
                    NPCAnimatedSprite[1].UpdateAnimations(gameTime, Position);
                    break;
                case 2:
                    NPCAnimatedSprite[2].UpdateAnimations(gameTime, Position);
                    break;
                case 3:
                    NPCAnimatedSprite[3].UpdateAnimations(gameTime, Position);
                    break;
            }
            if(mouse.WorldMouseRectangle.Intersects(this.NPCRectangle))
             {
                if(mouse.IsRightClicked)
                {
                    Game1.userInterface.IsShopMenu = true;
                }
                  
              }

            if(DirectionVector.X > .5f)
            {
                CurrentDirection = 2; //right
            }
            else if (DirectionVector.X < -.5f)
            {
                CurrentDirection = 1; //left
            }
            else if (DirectionVector.Y <.5f) // up
            {
                CurrentDirection = 3;
            }
            
            else if (DirectionVector.Y > .5f)
            {
                CurrentDirection = 0;
            }
            this.PrimaryVelocity = Collider.Velocity;
            //this.Speed = PrimaryVelocity
        }

        public void MoveTowardsPosition(Vector2 positionToMoveTowards)
        {
            Vector2 direction = Vector2.Normalize(positionToMoveTowards - Position);
            this.DirectionVector = direction;

            Position += (direction * Speed) * PrimaryVelocity;
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (CurrentDirection)
            {
                case 0:
                    NPCAnimatedSprite[0].DrawAnimation(spriteBatch, Position, .4f);
                    break;
                case 1:
                    NPCAnimatedSprite[1].DrawAnimation(spriteBatch, Position, .4f);
                    break;
                case 2:
                    NPCAnimatedSprite[2].DrawAnimation(spriteBatch, Position, .4f);
                    break;
                case 3:
                    NPCAnimatedSprite[3].DrawAnimation(spriteBatch, Position, .4f);
                    break;
            }
        }
    }
}
