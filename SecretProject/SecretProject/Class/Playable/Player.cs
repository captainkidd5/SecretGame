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
    [Serializable()]
    public class Player : ISerializable
    {

        //TODO: Work on gametime
        public Vector2 position;
        public Vector2 PrimaryVelocity;
        public Vector2 SecondaryVelocity;
        public Vector2 TotalVelocity;
        public bool Activate { get; set; }

        public AnimatedSprite[] animations;

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
        public AnimatedSprite PlayerMovementAnimations { get; set; }
        [XmlIgnore]
        public Texture2D Texture { get; set; }
        public int FrameNumber { get; set; }
        [XmlIgnore]
        public Collider MyCollider { get; set; }

        public bool IsPerformingAction = false;

        public AnimatedSprite CurrentAction;

        public AnimatedSprite CutGrassDown { get; set; }

        public AnimatedSprite CutGrassRight { get; set; }

        public AnimatedSprite CutGrassLeft { get; set; }

        public AnimatedSprite CutGrassUp { get; set; }


        public AnimatedSprite MiningDown { get; set; }

        public AnimatedSprite MiningRight { get; set; }

        public AnimatedSprite MiningLeft { get; set; }

        public AnimatedSprite MiningUp { get; set; }

        public AnimatedSprite ChoppingDown { get; set; }

        public AnimatedSprite ChoppingRight { get; set; }

        public AnimatedSprite ChoppingLeft { get; set; }

        public AnimatedSprite ChoppingUp { get; set; }


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
            animations = new AnimatedSprite[frameNumber];

            MyCollider = new Collider(PrimaryVelocity, Rectangle);

            Inventory = new Inventory(7) { Money = 100 };

            controls = new PlayerControls(0);



            CutGrassDown = new AnimatedSprite(graphics, Game1.AllTextures.CutGrassDown, 1, 3, 3);
            CutGrassRight = new AnimatedSprite(graphics, Game1.AllTextures.CutGrassRight, 1, 3, 3) { AdjustedLocationX = 1 };
            CutGrassLeft = new AnimatedSprite(graphics, Game1.AllTextures.CutGrassLeft, 1, 3, 3) { AdjustedLocationX = -18 };
            CutGrassUp = new AnimatedSprite(graphics, Game1.AllTextures.CutGrassUp, 1, 3, 3) { AdjustedLocationX = -1, AdjustedLocationY = -7 };

            MiningDown = new AnimatedSprite(graphics, Game1.AllTextures.MiningDown, 1, 5, 5) { AdjustedLocationX = 1, AdjustedLocationY = -16, speed = .08d };
            MiningRight = new AnimatedSprite(graphics, Game1.AllTextures.MiningRight, 1, 5, 5) { AdjustedLocationX = -6, AdjustedLocationY = -16, speed = .08d };
            MiningLeft = new AnimatedSprite(graphics, Game1.AllTextures.MiningLeft, 1, 5, 5) { AdjustedLocationX = -20, AdjustedLocationY = -16, speed = .08d };
            MiningUp = new AnimatedSprite(graphics, Game1.AllTextures.MiningUp, 1, 5, 5) { AdjustedLocationX = 1, AdjustedLocationY = -10, speed = .08d };

            ChoppingDown = new AnimatedSprite(graphics, Game1.AllTextures.ChoppingDown, 1, 5, 5) { AdjustedLocationX = 1, AdjustedLocationY = -16, speed = .08d };
            ChoppingRight = new AnimatedSprite(graphics, Game1.AllTextures.ChoppingRight, 1, 5, 5) { AdjustedLocationX = -6, AdjustedLocationY = -16, speed = .08d };
            ChoppingLeft = new AnimatedSprite(graphics, Game1.AllTextures.ChoppingLeft, 1, 5, 5) { AdjustedLocationX = -20, AdjustedLocationY = -16, speed = .08d };
            ChoppingUp = new AnimatedSprite(graphics, Game1.AllTextures.ChoppingUp, 1, 5, 5) { AdjustedLocationX = 0, AdjustedLocationY = -12, speed = .08d };

            CurrentAction = CutGrassDown;

            SetRectangleTexture(graphics, ClickRangeRectangle);

        }

        

        public void PlayAnimation(GameTime gameTime, string action)
        {
            
            switch (action)
            {
                case "CutGrassDown":
                    IsPerformingAction = true;
                    CutGrassDown.PlayOnce(gameTime);
                    CurrentAction = CutGrassDown;
                    CurrentAction.IsAnimating = true;
                    break;

                case "CutGrassRight":
                    IsPerformingAction = true;
                    CutGrassRight.PlayOnce(gameTime);
                    CurrentAction = CutGrassRight;
                    CurrentAction.IsAnimating = true;
                    break;

                case "CutGrassLeft":
                    IsPerformingAction = true;
                    CutGrassLeft.PlayOnce(gameTime);
                    CurrentAction = CutGrassLeft;
                    CurrentAction.IsAnimating = true;
                    break;

                case "CutGrassUp":
                    IsPerformingAction = true;
                    CutGrassUp.PlayOnce(gameTime);
                    CurrentAction = CutGrassUp;
                    CurrentAction.IsAnimating = true;
                    break;

                case "MiningDown":
                    IsPerformingAction = true;
                    MiningDown.PlayOnce(gameTime);
                    CurrentAction = MiningDown;
                    CurrentAction.IsAnimating = true;
                    break;

                case "MiningRight":
                    IsPerformingAction = true;
                    MiningRight.PlayOnce(gameTime);
                    CurrentAction = MiningRight;
                    CurrentAction.IsAnimating = true;
                    break;

                case "MiningLeft":
                    IsPerformingAction = true;
                    MiningLeft.PlayOnce(gameTime);
                    CurrentAction = MiningLeft;
                    CurrentAction.IsAnimating = true;
                    break;

                case "MiningUp":
                    IsPerformingAction = true;
                    MiningUp.PlayOnce(gameTime);
                    CurrentAction = MiningUp;
                    CurrentAction.IsAnimating = true;
                    break;


                case "ChoppingRight":
                    IsPerformingAction = true;
                    ChoppingRight.PlayOnce(gameTime);
                    CurrentAction = ChoppingRight;
                    CurrentAction.IsAnimating = true;
                    break;

                case "ChoppingLeft":
                    IsPerformingAction = true;
                    ChoppingLeft.PlayOnce(gameTime);
                    CurrentAction = ChoppingLeft;
                    CurrentAction.IsAnimating = true;
                    break;

                case "ChoppingUp":
                    IsPerformingAction = true;
                    ChoppingUp.PlayOnce(gameTime);
                    CurrentAction = ChoppingUp;
                    CurrentAction.IsAnimating = true;
                    break;
                case "ChoppingDown":
                    IsPerformingAction = true;
                    ChoppingDown.PlayOnce(gameTime);
                    CurrentAction = ChoppingDown;
                    CurrentAction.IsAnimating = true;
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

                
                if (controls.IsMoving && CurrentAction.IsAnimating == false)
                {
                    PlayerMovementAnimations.Update(gameTime);
                }
                else if (CurrentAction.IsAnimating == true)
                {
                    CurrentAction.PlayOnce(gameTime);

                }
                else if (CurrentAction.IsAnimating == false && controls.IsMoving == false)
                {
                    PlayerMovementAnimations.setFrame(0);
                } 


                IsMoving = false;

                if(!CurrentAction.IsAnimating)
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
                            PlayerMovementAnimations.Update(gameTime);
                            break;
                        case SecondaryDir.Left:
                            SecondaryVelocity.X = -SecondarySpeed;
                            PlayerMovementAnimations = animations[(int)Dir.Left];
                            PlayerMovementAnimations.Update(gameTime);
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

        public void Draw(SpriteBatch spriteBatch)
        {

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