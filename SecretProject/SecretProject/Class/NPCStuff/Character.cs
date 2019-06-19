using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.ObjectFolder;
using SecretProject.Class.SpriteFolder;

namespace SecretProject.Class.NPCStuff
{
    public class Character : INPC
    {
        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public Sprite[] NPCAnimatedSprite { get; set; }
        public Texture2D Texture { get; set; }

        public Rectangle NPCRectangle { get { return new Rectangle((int)Position.X, (int)Position.Y + 16, 16, 32); } }

        public float Speed { get; set; } = .29f;
        public Vector2 PrimaryVelocity { get; set; }
        public Vector2 TotalVelocity { get; set; }

        public Vector2 DirectionVector { get; set; }


        //0 = down, 1 = left, 2 =  right, 3 = up
        public int CurrentDirection { get; set; }

        public bool IsUpdating { get; set; } = false;

        public Collider Collider { get; set; }
        public bool CollideOccured { get; set; } = false;
        public int SpeakerID { get; set; }
        public bool IsMoving { get; set; }



        public int FrameNumber { get; set; } = 25;

        public Character(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet)
        {
            this.Name = name;
            this.Position = position;
            this.Texture = spriteSheet;
            NPCAnimatedSprite = new Sprite[4];

            NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 48, 0, 16, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 144, 0, 16, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 240, 0, 16, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 336, 0, 16, 48, 6, .15f, this.Position);

            Collider = new Collider(this.PrimaryVelocity, this.NPCRectangle);
            this.CurrentDirection = 0;

        }

        public void Update(GameTime gameTime, List<ObjectBody> objects, MouseManager mouse)
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
                if (mouse.IsRightClicked)
                {


                    Game1.Player.UserInterface.TextBuilder.IsActive = true;
                    Game1.Player.UserInterface.TextBuilder.UseTextBox = true;
                    Game1.Player.UserInterface.TextBuilder.FreezeStage = true;
                    Game1.Player.UserInterface.TextBuilder.StringToWrite = Game1.DialogueLibrary.RetrieveDialogue(this.SpeakerID, 1);
                    UpdateDirectionVector(Game1.Player.position);


                }

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
            //this.Speed = PrimaryVelocity
        }

        public void MoveTowardsPosition(Vector2 positionToMoveTowards, Rectangle rectangle)
        {
            Vector2 direction = Vector2.Normalize(positionToMoveTowards - Position);
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

        public void UpdateDirectionVector(Vector2 positionToFace)
        {
            Vector2 direction = Vector2.Normalize(positionToFace - Position);
            this.DirectionVector = direction;

            UpdateDirection();
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
        }
        float timeBetweenJumps = 1f;
        int counter = 0;
        bool pathFound = false;
        List<Point> currentPath;
        public void MoveToTile(GameTime gameTime, List<Point> path)
        {
            
            if(pathFound == false)
            {
                currentPath = path;
                pathFound = true;
            }
            timeBetweenJumps -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(timeBetweenJumps <= 0)
            {
                counter++;
                timeBetweenJumps = 1f;
            }
            if(counter < currentPath.Count)
            {
                //this.Position = new Vector2(currentPath[counter].X * 16, currentPath[counter].Y * 16);
                MoveTowardsPosition(new Vector2(currentPath[counter].X * 16, currentPath[counter].Y * 16), new Rectangle(0, 0, 0, 0));
            }
            else
            {
                CurrentDirection = (int)Dir.Down;
                IsMoving = false;
                
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

        public void TextBoxResponse()
        {

        }
    }
}
