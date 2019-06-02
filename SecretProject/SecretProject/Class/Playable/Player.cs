using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;


using SecretProject.Class.SpriteFolder;
using SecretProject.Class.ObjectFolder;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.ItemStuff;
using Microsoft.Xna.Framework.Content;
using SecretProject.Class.Controls;

namespace SecretProject.Class.Playable
{

    public class Player
    {

        //TODO: Work on gametime
        public Vector2 position;
        public Vector2 PrimaryVelocity;
        public Vector2 SecondaryVelocity;
        public Vector2 TotalVelocity;
        public bool Activate { get; set; }

        public Sprite[] animations;

        public string Name { get; set; }
        public Inventory Inventory { get; set; }
        [XmlIgnore]
        ContentManager content;

        [XmlIgnore]
        public PlayerControls controls;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public void SetX(float newX)
        {
            position.X = newX;
        }

        public void SetY(float newY)
        {
            position.Y = newY;
        }

        public int Health { get { return Health1; } set { value = Health1; } }

        public int Health1 { get; set; } = 3;
        public Dir Direction { get; set; } = Dir.Down;
        public SecondaryDir SecondDirection { get; set; } = SecondaryDir.Down;
        public bool IsMoving { get; set; } = false;
        public float Speed1 { get; set; } = 2f;
        public float SecondarySpeed { get; set; } = 1f;
        public Sprite PlayerMovementAnimations { get; set; }
        [XmlIgnore]
        public Texture2D Texture { get; set; }
        public int FrameNumber { get; set; }
        [XmlIgnore]
        public Collider MyCollider { get; set; }

        public bool IsPerformingAction = false;

        public Sprite CurrentAction;



        public Sprite CutGrassDown { get; set; }
        public Sprite CutGrassRight { get; set; }
        public Sprite CutGrassLeft { get; set; }
        public Sprite CutGrassUp { get; set; }

        public Sprite MiningDown { get; set;}
        public Sprite MiningRight { get; set; }
        public Sprite MiningLeft { get; set; }
        public Sprite MiningUp { get; set; }

        public Sprite ChoppingDown { get; set; }
        public Sprite ChoppingRight { get; set; }
        public Sprite ChoppingLeft { get; set; }
        public Sprite ChoppingUp { get; set; }







        [XmlIgnore]
        public Texture2D BigHitBoxRectangleTexture;

        public Rectangle ChunkDetector {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y , (int)position.X + 200, (int)position.Y + 200);
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X + 2, (int)position.Y + 20, ((int)Texture.Width / FrameNumber) -2, (int)Texture.Height - 25);
            }

        }

        public Rectangle ClickRangeRectangle
        {
            get
            {
                return new Rectangle((int)position.X - 15, (int)position.Y - 15, ((int)Texture.Width / FrameNumber) + 30, (int)Texture.Height + 30);
            }
        }

        private Player()
        {

        }


        public Player(string name, Vector2 position, Texture2D texture, int frameNumber, ContentManager content, GraphicsDevice graphics, MouseManager mouse)
        {
            this.content = content;
            Name = name;
            Position = position;
            this.Texture = texture;
            this.FrameNumber = frameNumber;
            animations = new Sprite[frameNumber];

            MyCollider = new Collider(PrimaryVelocity, Rectangle);

            Inventory = new Inventory(7) { Money = 100 };

            controls = new PlayerControls(0);



            CutGrassDown = new Sprite(graphics, Game1.AllTextures.PlayerSpriteSheet,80,80,16,48,3,.15f,this.Position);
            //order of animation frames needs to be flipped
            CutGrassRight = new Sprite(graphics, Game1.AllTextures.PlayerSpriteSheet, 176, 64, 48, 48, 3, .15f, this.Position, offSetX: -1);
            CutGrassLeft = new Sprite(graphics, Game1.AllTextures.PlayerSpriteSheet, 320, 64, 48, 48, 3, .15f, this.Position, -33);
            CutGrassUp = new Sprite(graphics, Game1.AllTextures.PlayerSpriteSheet, 304, 128, 32, 48, 3, .15f, this.Position, -1, -12);




            MiningDown = new Sprite(graphics, Game1.AllTextures.PlayerSpriteSheet,0,128,16,64,5,.1f, this.Position,0,-28);
            MiningRight = new Sprite(graphics, Game1.AllTextures.PlayerSpriteSheet, 144, 416, 64, 64,5, .15f, this.Position, -15, -28);
            MiningLeft = new Sprite(graphics, Game1.AllTextures.PlayerSpriteSheet, 0, 336, 64,64,5,.15f,this.Position, -31, -28);
            MiningUp = new Sprite(graphics, Game1.AllTextures.PlayerSpriteSheet, 96, 144, 16,48,5,.15f,this.Position, 0, -12);



            ChoppingDown = new Sprite(graphics, Game1.AllTextures.PlayerSpriteSheet, 0, 240, 16, 64, 5, .1f, this.Position, 0, -28);
            ChoppingRight = new Sprite(graphics, Game1.AllTextures.PlayerSpriteSheet, 160, 192, 64, 64, 5, .15f, this.Position, -15, -28);
            ChoppingLeft = new Sprite(graphics, Game1.AllTextures.PlayerSpriteSheet, 160, 256, 64, 64, 5, .15f, this.Position, -33, -28);
            ChoppingUp = new Sprite(graphics, Game1.AllTextures.PlayerSpriteSheet, 80, 256, 16, 48, 5, .1f, this.Position, 0, -12);



            CurrentAction = CutGrassDown;

            SetRectangleTexture(graphics, ClickRangeRectangle);

        }

        

        public void PlayAnimation(GameTime gameTime, string action)
        {
            
            switch (action)
            {
                case "CutGrassDown":
                    IsPerformingAction = true;
                    CutGrassDown.PlayOnce(gameTime,Position);
                    CurrentAction = CutGrassDown;
                    CurrentAction.IsAnimated = true;
                    break;

                case "CutGrassRight":
                   IsPerformingAction = true;
                    CutGrassRight.PlayOnce(gameTime,Position);
                    CurrentAction = CutGrassRight;
                    CurrentAction.IsAnimated = true;
                    break;

                case "CutGrassLeft":
                    IsPerformingAction = true;
                    CutGrassLeft.PlayOnce(gameTime, Position);
                    CurrentAction = CutGrassLeft;
                    CurrentAction.IsAnimated = true;
                    break;

                case "CutGrassUp":
                    IsPerformingAction = true;
                    CutGrassUp.PlayOnce(gameTime, Position);
                    CurrentAction = CutGrassUp;
                    CurrentAction.IsAnimated = true;
                    break;

                case "MiningDown":
                    IsPerformingAction = true;
                    MiningDown.PlayOnce(gameTime, Position);
                    CurrentAction = MiningDown;
                    CurrentAction.IsAnimated = true;
                    break;

                case "MiningRight":
                    IsPerformingAction = true;
                    MiningRight.PlayOnce(gameTime, Position);
                    CurrentAction = MiningRight;
                    CurrentAction.IsAnimated = true;
                    break;

                case "MiningLeft":
                    IsPerformingAction = true;
                    MiningLeft.PlayOnce(gameTime,Position);
                    CurrentAction = MiningLeft;
                    CurrentAction.IsAnimated = true;
                    break;

                case "MiningUp":
                    IsPerformingAction = true;
                    MiningUp.PlayOnce(gameTime,Position);
                    CurrentAction = MiningUp;
                    CurrentAction.IsAnimated = true;
                    break;

                case "ChoppingDown":
                    IsPerformingAction = true;
                    ChoppingDown.PlayOnce(gameTime,Position);
                    CurrentAction = ChoppingDown;
                    CurrentAction.IsAnimated = true;
                    break;
                case "ChoppingRight":
                    IsPerformingAction = true;
                    ChoppingRight.PlayOnce(gameTime,Position);
                    CurrentAction = ChoppingRight;
                    CurrentAction.IsAnimated = true;
                    break;

                case "ChoppingLeft":
                    IsPerformingAction = true;
                    ChoppingLeft.PlayOnce(gameTime,Position);
                    CurrentAction = ChoppingLeft;
                    CurrentAction.IsAnimated = true;
                    break;

                case "ChoppingUp":
                    IsPerformingAction = true;
                    ChoppingUp.PlayOnce(gameTime,Position);
                    CurrentAction = ChoppingUp;
                    CurrentAction.IsAnimated = true;
                    break;





            }
        }



        private void SetRectangleTexture(GraphicsDevice graphicsDevice, Rectangle rectangleToDraw)
        {
            var Colors = new List<Color>();
            for (int y = 0; y < rectangleToDraw.Height; y++)
            {
                for (int x = 0; x < rectangleToDraw.Width; x++)
                {
                    if (x == 0 || //left side
                        y == 0 || //top side
                        x == rectangleToDraw.Width - 1 || //right side
                        y == rectangleToDraw.Height - 1) //bottom side
                    {
                        Colors.Add(new Color(255, 255, 255, 255));
                    }
                    else
                    {
                        Colors.Add(new Color(0, 0, 0, 0));

                    }

                }
            }
            BigHitBoxRectangleTexture = new Texture2D(graphicsDevice, rectangleToDraw.Width, rectangleToDraw.Height);
            BigHitBoxRectangleTexture.SetData<Color>(Colors.ToArray());
        }


        public void Update(GameTime gameTime, List<Item> items, List<ObjectBody> objects)
        {
            if (Activate)
            {
                KeyboardState kState = Keyboard.GetState();
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

                //animation set changes depending on which key is pressed, as shown in playercontrols
                PlayerMovementAnimations = animations[(int)controls.Direction];

                MyCollider.Rectangle = this.Rectangle;
                MyCollider.Velocity = this.PrimaryVelocity;
                MyCollider.DidCollideMagnet(items);

                

                bool collideOccurred = MyCollider.DidCollide(objects); //did a collision with an object happen this loop?
                this.PrimaryVelocity = MyCollider.Velocity;

                if(collideOccurred) //if collision occurred we don't want to take diagonal movement into account
                {
                    TotalVelocity = PrimaryVelocity;
                }
                else
                {
                    TotalVelocity = PrimaryVelocity + SecondaryVelocity;
                }
                

                if (controls.IsSprinting)
                {
                    TotalVelocity = TotalVelocity * 5f;
                }
                else
                {

                }

                Position += TotalVelocity;
                PrimaryVelocity = Vector2.Zero;
                SecondaryVelocity = Vector2.Zero;
                TotalVelocity = Vector2.Zero;

                
                if (controls.IsMoving && CurrentAction.IsAnimated == false)
                {
                    PlayerMovementAnimations.UpdateAnimations(gameTime, this.Position);
                }
                else if (CurrentAction.IsAnimated == true)
                {
                    CurrentAction.PlayOnce(gameTime,Position);

                }
                else if (CurrentAction.IsAnimated == false && controls.IsMoving == false)
                {
                    PlayerMovementAnimations.SetFrame(0);
                } 


                IsMoving = false;

                if(!CurrentAction.IsAnimated)
                {
                    controls.Update();
                }
                

                if (controls.IsMoving)
                {
                    IsMoving = true;
                    switch (controls.Direction)
                    {
                        case Dir.Right:
                            PrimaryVelocity.X = Speed1 ;
                            break;

                        case Dir.Left:
                            PrimaryVelocity.X = -Speed1;
                            break;

                        case Dir.Down:
                            PrimaryVelocity.Y = Speed1;
                            break;

                        case Dir.Up:
                            PrimaryVelocity.Y = -Speed1;
                            break;

                        default:
                            break;

                    }


                    switch(controls.SecondaryDirection)
                    {
                        case SecondaryDir.Right:
                            SecondaryVelocity.X = SecondarySpeed;
                            PlayerMovementAnimations = animations[(int)Dir.Right];
                            PlayerMovementAnimations.UpdateAnimations(gameTime,this.Position);
                            break;
                        case SecondaryDir.Left:
                            SecondaryVelocity.X = -SecondarySpeed;
                            PlayerMovementAnimations = animations[(int)Dir.Left];
                            PlayerMovementAnimations.UpdateAnimations(gameTime, this.Position);
                            break;
                        case SecondaryDir.Down:
                            SecondaryVelocity.Y = SecondarySpeed;
                            //PlayerMovementAnimations = animations[(int)Dir.Down];
                            //PlayerMovementAnimations.Update(gameTime);
                            break;
                        case SecondaryDir.Up:
                            SecondaryVelocity.Y = -SecondarySpeed;
                            //PlayerMovementAnimations = animations[(int)Dir.Up];
                            //PlayerMovementAnimations.Update(gameTime);
                            break;
                       // case SecondaryDir.None:

                        

                        default:
                            break;
                        
                    }


                }

                if(position.X < Game1.GetCurrentStage().MapRectangle.Left)
                {
                    position.X = Game1.GetCurrentStage().MapRectangle.Left;
                }


                if (position.X > Game1.GetCurrentStage().MapRectangle.Right)
                {
                    position.X = Game1.GetCurrentStage().MapRectangle.Right;
                }
                if (position.Y < Game1.GetCurrentStage().MapRectangle.Top)
                {
                    position.Y = Game1.GetCurrentStage().MapRectangle.Top;
                }
                if (position.Y > Game1.GetCurrentStage().MapRectangle.Bottom)
                {
                    position.Y = Game1.GetCurrentStage().MapRectangle.Bottom;
                }
            }
        }

        //drawing relative to wrong camera it seems.
        public void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentAction.IsAnimated == false)
            {
               PlayerMovementAnimations.DrawAnimation(spriteBatch, (float).4);
            }

            //????
            if (CurrentAction.IsAnimated == true)
            {
                CurrentAction.DrawAnimation(spriteBatch, (float).4);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Position", Position);
            info.AddValue("Health", Health);
        }

        public Player(SerializationInfo info, StreamingContext context)
        {
            Position = (Vector2)info.GetValue("Position", typeof(Vector2));
            Health = (int)info.GetValue("Health", typeof(int));
        }
    }
}