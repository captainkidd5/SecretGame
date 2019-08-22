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
using SecretProject.Class.UI;

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

        public Sprite[,] animations;

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
        public Sprite[] PlayerMovementAnimations { get; set; }

        public Texture2D Texture { get; set; }
        public int FrameNumber { get; set; }

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

        public UserInterface UserInterface { get; set; }

        //1 for normal, 2 for ship
        public int GameMode { get; set; } = 1;


        [XmlIgnore]
        public Texture2D BigHitBoxRectangleTexture;
        public Texture2D LittleHitBoxRectangleTexture;

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
                return new Rectangle((int)position.X, (int)position.Y + 24, 16, 8);
            }

        }

        public Rectangle ClickRangeRectangle
        {
            get
            {
                return new Rectangle((int)position.X - 16, (int)position.Y,  48,  64);
            }
        }

        public PlayerShip PlayerShip { get; set; }

        public int Wisdom { get; set; }

        private Player()
        {

        }


        public Player(string name, Vector2 position, Texture2D texture, int numberOfFrames, int numberOfBodyParts, ContentManager content, GraphicsDevice graphics, MouseManager mouse)
        {
            this.content = content;
            Name = name;
            Position = position;
            this.Texture = texture;
            this.FrameNumber = numberOfFrames;
            animations = new Sprite[numberOfFrames, numberOfBodyParts];

            MyCollider = new Collider(PrimaryVelocity, Rectangle);

            Inventory = new Inventory(7) { Money = 500 };

            controls = new PlayerControls(0);



            CutGrassDown = new Sprite(graphics, this.Texture, 80,80,16,48,3,.15f,this.Position,0, 12);
            //order of animation frames needs to be flipped
            CutGrassRight = new Sprite(graphics, this.Texture, 176, 64, 48, 48, 3, .15f, this.Position, -1, 12);
            CutGrassLeft = new Sprite(graphics, this.Texture, 320, 64, 48, 48, 3, .15f, this.Position, -33, 12);
            CutGrassUp = new Sprite(graphics, this.Texture, 304, 128, 32, 48, 3, .15f, this.Position, -1, 0);




            MiningDown = new Sprite(graphics, this.Texture, 0,128,16,64,5,.1f, this.Position,0,-16);
            MiningRight = new Sprite(graphics, this.Texture, 144, 416, 64, 64,5, .1f, this.Position, -15, -16);
            MiningLeft = new Sprite(graphics, this.Texture, 0, 336, 64,64,5,.1f,this.Position, -31, -16);
            MiningUp = new Sprite(graphics, this.Texture, 96, 144, 16,48,5,.1f,this.Position, 0, 0);



            ChoppingDown = new Sprite(graphics, this.Texture, 0, 240, 16, 64, 5, .1f, this.Position, 0, -16);
            ChoppingRight = new Sprite(graphics, this.Texture, 160, 192, 64, 64, 5, .1f, this.Position, -15, -16);
            ChoppingLeft = new Sprite(graphics, this.Texture, 160, 256, 64, 64, 5, .1f, this.Position, -33, -16);
            ChoppingUp = new Sprite(graphics, this.Texture, 80, 256, 16, 48, 5, .1f, this.Position, 0, -0);



            CurrentAction = CutGrassDown;

            BigHitBoxRectangleTexture = SetRectangleTexture(graphics, ClickRangeRectangle);
            LittleHitBoxRectangleTexture = SetRectangleTexture(graphics, Rectangle);

            PlayerShip = new PlayerShip(graphics, Game1.AllTextures.ShipSpriteSheet);
            //PlayerShip.Texture = Game1.AllTextures.ShipSpriteSheet;
            

        }

        

        public void PlayAnimation(GameTime gameTime, int action)
        {
            
            switch (action)
            {
                case 1:
                    IsPerformingAction = true;
                    CutGrassDown.PlayOnce(gameTime,Position);
                    CurrentAction = CutGrassDown;
                    CurrentAction.IsAnimated = true;
                    break;

                case 2:
                   IsPerformingAction = true;
                    CutGrassRight.PlayOnce(gameTime,Position);
                    CurrentAction = CutGrassRight;
                    CurrentAction.IsAnimated = true;
                    break;

                case 3:
                    IsPerformingAction = true;
                    CutGrassLeft.PlayOnce(gameTime, Position);
                    CurrentAction = CutGrassLeft;
                    CurrentAction.IsAnimated = true;
                    break;

                case 4:
                    IsPerformingAction = true;
                    CutGrassUp.PlayOnce(gameTime, Position);
                    CurrentAction = CutGrassUp;
                    CurrentAction.IsAnimated = true;
                    break;

                case 5:
                    IsPerformingAction = true;
                    MiningDown.PlayOnce(gameTime, Position);
                    CurrentAction = MiningDown;
                    CurrentAction.IsAnimated = true;
                    break;

                case 6:
                    IsPerformingAction = true;
                    MiningRight.PlayOnce(gameTime, Position);
                    CurrentAction = MiningRight;
                    CurrentAction.IsAnimated = true;
                    break;

                case 7:
                    IsPerformingAction = true;
                    MiningLeft.PlayOnce(gameTime,Position);
                    CurrentAction = MiningLeft;
                    CurrentAction.IsAnimated = true;
                    break;

                case 8:
                    IsPerformingAction = true;
                    MiningUp.PlayOnce(gameTime,Position);
                    CurrentAction = MiningUp;
                    CurrentAction.IsAnimated = true;
                    break;

                case 9:
                    IsPerformingAction = true;
                    ChoppingDown.PlayOnce(gameTime,Position);
                    CurrentAction = ChoppingDown;
                    CurrentAction.IsAnimated = true;
                    break;
                case 10:
                    IsPerformingAction = true;
                    ChoppingRight.PlayOnce(gameTime,Position);
                    CurrentAction = ChoppingRight;
                    CurrentAction.IsAnimated = true;
                    break;

                case 11:
                    IsPerformingAction = true;
                    ChoppingLeft.PlayOnce(gameTime,Position);
                    CurrentAction = ChoppingLeft;
                    CurrentAction.IsAnimated = true;
                    break;

                case 12:
                    IsPerformingAction = true;
                    ChoppingUp.PlayOnce(gameTime,Position);
                    CurrentAction = ChoppingUp;
                    CurrentAction.IsAnimated = true;
                    break;





            }
        }



        


        public void Update(GameTime gameTime, List<Item> items, Dictionary<int, ObjectBody> objects, MouseManager mouse)
        {
            //this.UserInterface.Update(gameTime, Game1.OldKeyBoardState, Game1.NewKeyBoardState, this.Inventory, mouse);
            if (Activate)
            {
                if (GameMode == 1)
                {


                    KeyboardState kState = Keyboard.GetState();
                    float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    //animation set changes depending on which key is pressed, as shown in playercontrols
                    //PlayerMovementAnimations = animations[(int)controls.Direction];

                    for (int i = 0; i < animations.GetLength(1); i++)
                    {
                        PlayerMovementAnimations[i] = animations[(int)controls.Direction,i];
                    }

                    MyCollider.Rectangle = this.Rectangle;
                    MyCollider.Velocity = this.PrimaryVelocity;
                    MyCollider.DidCollideMagnet(items);



                    bool collideOccurred = MyCollider.DidCollide(objects, position); //did a collision with an object happen this loop?
                    
                    this.PrimaryVelocity = MyCollider.Velocity;

                    if (collideOccurred) //if collision occurred we don't want to take diagonal movement into account
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
                    Position += TotalVelocity;
                    PrimaryVelocity = Vector2.Zero;
                    SecondaryVelocity = Vector2.Zero;
                    TotalVelocity = Vector2.Zero;


                    if (controls.IsMoving && CurrentAction.IsAnimated == false)
                    {
                        for(int i =0; i < PlayerMovementAnimations.GetLength(0); i++)
                        {
                            PlayerMovementAnimations[i].UpdateAnimations(gameTime, this.Position);
                        }
                        
                    }
                    else if (CurrentAction.IsAnimated == true)
                    {
                        CurrentAction.PlayOnce(gameTime, Position);

                    }
                    else if (CurrentAction.IsAnimated == false && controls.IsMoving == false)
                    {
                        for (int i = 0; i < PlayerMovementAnimations.GetLength(0); i++)
                        {
                            PlayerMovementAnimations[i].SetFrame(0);
                        }
                    }


                    IsMoving = false;

                    if (!CurrentAction.IsAnimated)
                    {
                        controls.Update();
                    }


                    if (controls.IsMoving)
                    {
                        IsMoving = true;
                        switch (controls.Direction)
                        {
                            case Dir.Right:
                                PrimaryVelocity.X = Speed1;
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


                        switch (controls.SecondaryDirection)
                        {
                            case SecondaryDir.Right:
                                SecondaryVelocity.X = SecondarySpeed;
                                for (int i = 0; i < animations.GetLength(1); i++)
                                {
                                    PlayerMovementAnimations[i] = animations[(int)Dir.Right, i];
                                }

                                for (int i = 0; i < PlayerMovementAnimations.GetLength(0); i++)
                                {
                                    PlayerMovementAnimations[i].UpdateAnimations(gameTime, this.Position);
                                }
                                break;
                            case SecondaryDir.Left:
                                SecondaryVelocity.X = -SecondarySpeed;
                                for (int i = 0; i < animations.GetLength(1); i++)
                                {
                                    PlayerMovementAnimations[i] = animations[(int)Dir.Left, i];
                                }
                                //PlayerMovementAnimations = animations[(int)Dir.Left];
                                // PlayerMovementAnimations.AnimationSpeed = PlayerMovementAnimations.AnimationSpeed - this.Speed1;

                                for (int i = 0; i < PlayerMovementAnimations.GetLength(0); i++)
                                {
                                    PlayerMovementAnimations[i].UpdateAnimations(gameTime, this.Position);
                                }
                                break;
                            case SecondaryDir.Down:
                                SecondaryVelocity.Y = SecondarySpeed;
                                for (int i = 0; i < animations.GetLength(1); i++)
                                {
                                    PlayerMovementAnimations[i] = animations[(int)Dir.Down, i];
                                }
                                // PlayerMovementAnimations.AnimationSpeed = PlayerMovementAnimations.AnimationSpeed - this.Speed1;
                                //PlayerMovementAnimations = animations[(int)Dir.Down];

                                for (int i = 0; i < PlayerMovementAnimations.GetLength(0); i++)
                                {
                                    PlayerMovementAnimations[i].UpdateAnimations(gameTime, this.Position);
                                }
                                break;
                            case SecondaryDir.Up:
                                SecondaryVelocity.Y = -SecondarySpeed;
                                for (int i = 0; i < animations.GetLength(1); i++)
                                {
                                    PlayerMovementAnimations[i] = animations[(int)Dir.Up, i];
                                }

                                //PlayerMovementAnimations = animations[(int)Dir.Up];

                                for (int i = 0; i < PlayerMovementAnimations.GetLength(0); i++)
                                {
                                    PlayerMovementAnimations[i].UpdateAnimations(gameTime, this.Position);
                                }
                                break;
                            // case SecondaryDir.None:



                            default:
                                break;

                        }


                    }

                    CheckOutOfBounds(this.Position);
                }
                if (GameMode == 2)
                {
                    PlayerShip.Update(gameTime);
                    this.Position = PlayerShip.Position;
                    CheckOutOfBounds(this.Position);
                }
            }
        }

        private void CheckOutOfBounds(Vector2 position)
        {
            if (position.X < Game1.GetCurrentStage().MapRectangle.Left)
            {
                position.X = Game1.GetCurrentStage().MapRectangle.Left;
            }


            if (position.X > Game1.GetCurrentStage().MapRectangle.Right)
            {
                position.X = Game1.GetCurrentStage().MapRectangle.Right;
            }
            // if (position.Y < Game1.GetCurrentStage().MapRectangle.Top)
            // {
            //     position.Y = Game1.GetCurrentStage().MapRectangle.Top;
            //  }
            if (position.Y > Game1.GetCurrentStage().MapRectangle.Bottom)
            {
                position.Y = Game1.GetCurrentStage().MapRectangle.Bottom;
            }
        }

        //drawing relative to wrong camera it seems.
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (GameMode == 1)
            {


                if (CurrentAction.IsAnimated == false)
                {

                    for (int i = 0; i < PlayerMovementAnimations.GetLength(0); i++)
                    {
                        PlayerMovementAnimations[i].DrawAnimation(spriteBatch, this.Position, layerDepth);
                    }
                    
                }

                //????
                if (CurrentAction.IsAnimated == true)
                {
                    CurrentAction.DrawAnimation(spriteBatch, this.Position, layerDepth);
                }
            }
        }

        private Texture2D SetRectangleTexture(GraphicsDevice graphicsDevice, Rectangle rectangleToDraw)
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
            Texture2D textureToReturn;
            textureToReturn = new Texture2D(graphicsDevice, rectangleToDraw.Width, rectangleToDraw.Height);
            textureToReturn.SetData<Color>(Colors.ToArray());
            return textureToReturn;
        }
        public void DrawDebug(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(BigHitBoxRectangleTexture, new Vector2(ClickRangeRectangle.X, ClickRangeRectangle.Y), color: Color.White, layerDepth: layerDepth);
            spriteBatch.Draw(LittleHitBoxRectangleTexture, new Vector2(Rectangle.X, Rectangle.Y), color: Color.White, layerDepth: layerDepth);
        }

        public void DrawShipMode(SpriteBatch spriteBatch, float layerDepth)
        {

            PlayerShip.Draw(spriteBatch, layerDepth);
        }

        public void DrawUserInterface(SpriteBatch spriteBatch)
        {
            this.UserInterface.Draw(spriteBatch);
        }

        /*
         * if (player.position.Y > 1550 && player.position.X < 810 && player.position.X > 730)
                {
                    player.Position = new Vector2(player.position.X, 60);
                    Game1.SwitchStage(2, 5);
                    return;
                }
         * 
         * 
         */
    }
}