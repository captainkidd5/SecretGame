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

namespace SecretProject.Class.NPCStuff.Enemies
{
    public class Enemy : INPC
    {
        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public Sprite[] NPCAnimatedSprite { get; set; }

        public int NPCRectangleXOffSet { get; set; }
        public int NPCRectangleYOffSet { get; set; }
        public int NPCRectangleWidthOffSet { get; set; }
        public int NPCRectangleHeightOffSet { get; set; }
        public Rectangle NPCRectangle { get { return new Rectangle((int)Position.X + NPCRectangleXOffSet, (int)Position.Y + NPCRectangleYOffSet, NPCRectangleWidthOffSet, NPCRectangleHeightOffSet); } }

        public Texture2D Texture { get; set; }
        public float Speed { get; set; }
        //0 = down, 1 = left, 2 =  right, 3 = up
        public int CurrentDirection { get; set; }
        public bool IsMoving { get; set; }
        public Vector2 PrimaryVelocity { get; set; }
        public Vector2 TotalVelocity { get; set; }
        public Vector2 DirectionVector { get; set; }
        public bool IsUpdating { get; set; }
        public int FrameNumber { get; set; }
        public Collider Collider { get; set; }
        public bool CollideOccured { get; set; }
        

        public Enemy(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet)
        {
            this.Name = name;
            this.Position = position;
            this.Texture = spriteSheet;
            Collider = new Collider(this.PrimaryVelocity, this.NPCRectangle);
        }

        public virtual void Update(GameTime gameTime, List<ObjectBody> objects, MouseManager mouse)
        {
            this.PrimaryVelocity = new Vector2(1, 1);
            Collider.Rectangle = this.NPCRectangle;
            Collider.Velocity = this.PrimaryVelocity;
            this.CollideOccured = Collider.DidCollide(objects, Position);

            switch (CurrentDirection)
            {
                case 0:
                    NPCAnimatedSprite[0].UpdateAnimations(gameTime, Position);
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
            if (mouse.WorldMouseRectangle.Intersects(this.NPCRectangle))
            {
                mouse.ChangeMouseTexture(200);
                mouse.ToggleGeneralInteraction = true;
                Game1.isMyMouseVisible = false;

            }
            if (IsMoving)
            {


                UpdateDirection();
                this.PrimaryVelocity = Collider.Velocity;
            }
            else
            {
                this.NPCAnimatedSprite[CurrentDirection].SetFrame(0);
            }
            


        }


        public void MoveTowardsPosition(Vector2 positionToMoveTowards, Rectangle rectangle)
        {

            Vector2 direction = Vector2.Normalize((positionToMoveTowards - Position) + new Vector2((float).0000000001, (float).0000000001));
            //if (!(System.Single.IsNaN(direction.X) || System.Single.IsNaN(direction.Y)))
            //{
            this.DirectionVector = direction;
            if (!this.NPCRectangle.Intersects(rectangle))
            {
                Position += (direction * Speed) * PrimaryVelocity;
                IsMoving = true;
            }
            else
            {
                this.NPCAnimatedSprite[CurrentDirection].SetFrame(0);
                IsMoving = false;
            }
        }

        private float WanderTimer = 2f;
        private Vector2 wanderPosition = new Vector2(0, 0);
        public void Wander(GameTime gameTime)
        {
            //temporary
            MoveTowardsPosition(wanderPosition, new Rectangle(0, 0, 1, 1));
            WanderTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(WanderTimer<=0)
            {
                int newX = Game1.Utility.RGenerator.Next(-200, 200);
                int newY = Game1.Utility.RGenerator.Next(-200, 200);
                wanderPosition = new Vector2(Position.X + newX, Position.Y + newY);
                WanderTimer = Game1.Utility.RGenerator.Next(0, 5);
            }
        }
        public void UpdateDirection()
        {
            if (DirectionVector.X > .5f)
            {
                CurrentDirection = 2; //right
            }
            else if (DirectionVector.X < -.5f)
            {
                CurrentDirection = 1; //left
            }
            else if (DirectionVector.Y < .5f) // up
            {
                CurrentDirection = 3;
            }

            else if (DirectionVector.Y > .5f)
            {
                CurrentDirection = 0;
            }
            else
            {
                CurrentDirection = 0;
            }
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
