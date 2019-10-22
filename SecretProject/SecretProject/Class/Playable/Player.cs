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
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.ItemStuff;
using Microsoft.Xna.Framework.Content;
using SecretProject.Class.Controls;
using SecretProject.Class.UI;

namespace SecretProject.Class.Playable
{
    public enum AnimationType
    {
        HandsPicking = 0,
        Chopping = 21,
        Mining = 22,
    }


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

        ContentManager content;


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

        public int Health { get; set; }
        public int Stamina { get; set; }


        public Dir Direction { get; set; } = Dir.Down;
        public SecondaryDir SecondDirection { get; set; } = SecondaryDir.Down;
        public Dir AnimationDirection { get; set; }
        public bool IsMoving { get; set; } = false;
        public float Speed1 { get; set; } = 1f;
        public float SecondarySpeed { get; set; } = 1f;
        public Sprite[] PlayerMovementAnimations { get; set; }
        public Sprite[] PlayerActionAnimations { get; set; }

        public Texture2D Texture { get; set; }
        public int FrameNumber { get; set; }

        public Collider MyCollider { get; set; }

        public bool IsPerformingAction = false;

        public Sprite[,] CurrentAction;



        public Sprite[,] Mining { get; set; }


        public UserInterface UserInterface { get; set; }

        public Texture2D BigHitBoxRectangleTexture;
        public Texture2D LittleHitBoxRectangleTexture;
        public bool LockBounds { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, 16, 32);
            }

        }

        public Rectangle ColliderRectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y + 28, 16, 4);
            }

        }

        public Rectangle ClickRangeRectangle
        {
            get
            {
                return new Rectangle((int)position.X - 16, (int)position.Y, 48, 64);
            }
        }



        public Player(string name, Vector2 position, Texture2D texture, int numberOfFrames, int numberOfBodyParts, ContentManager content, GraphicsDevice graphics, MouseManager mouse)
        {
            this.content = content;
            Name = name;
            this.Health = 3;
            this.Stamina = 100;
            Position = position;
            this.Texture = texture;
            this.FrameNumber = numberOfFrames;
            animations = new Sprite[numberOfFrames, numberOfBodyParts];
            Mining = new Sprite[4, 6];

            MyCollider = new Collider(graphics,PrimaryVelocity, ColliderRectangle, 1);

            Inventory = new Inventory(7) { Money = 10000 };

            controls = new PlayerControls(0);



            CurrentAction = Mining;

            BigHitBoxRectangleTexture = SetRectangleTexture(graphics, ClickRangeRectangle);
            LittleHitBoxRectangleTexture = SetRectangleTexture(graphics, ColliderRectangle);

            LockBounds = true;

        }



        public void PlayAnimation(GameTime gameTime, AnimationType action, int textureColumn = 0)
        {

            switch (action)
            {
                case AnimationType.Mining:
                    IsPerformingAction = true;
                    CurrentAction = Mining;
                    AnimationDirection = controls.Direction;
                    for (int i = 0; i < 4; i++)
                    {
                        this.Mining[i, 0].FirstFrameY = textureColumn;
                    }

                    break;

                case AnimationType.Chopping:
                    IsPerformingAction = true;
                    CurrentAction = Mining;
                    AnimationDirection = controls.Direction;
                    for (int i = 0; i < 4; i++)
                    {
                        this.Mining[i, 0].FirstFrameY = textureColumn;
                    }
                    break;
            }


        }

        public void PlayCollectiveActions(GameTime gameTime)
        {
            for (int i = 0; i < 6; i++)
            {
                PlayerActionAnimations[i] = CurrentAction[(int)AnimationDirection, i];
                PlayerActionAnimations[i].IsAnimated = true;
                PlayerActionAnimations[i].PlayOnce(gameTime, Position);
            }
            // CurrentAction0, 0].IsAnimated = true;
            //for (int i = 0; i < CurrentAction.GetLength(0); i++)
            //{
            //    for (int j = 0; j < CurrentAction.GetLength(1); j++)
            //    {
            //        CurrentAction[i, j].IsAnimated = true;
            //        CurrentAction[i, j].PlayOnce(gameTime, Position);

            //    }
            //}
            //IsPerformingAction = false;
        }
        public void DrawCollectiveActions(SpriteBatch spriteBatch, float layerDepth)
        {
            for (int i = 0; i < PlayerActionAnimations.Length; i++)
            {
                PlayerActionAnimations[i].DrawAnimation(spriteBatch, Position,layerDepth +  PlayerActionAnimations[i].LayerDepth);
            }

        }

        public void UpdateMovementAnimationsOnce(GameTime gameTime)
        {
            for (int i = 0; i < animations.GetLength(0); i++)
            {
                for (int j = 0; j < animations.GetLength(1); j++)
                {
                    animations[i, j].UpdateAnimations(gameTime, this.position);
                }
            }
        }
        public bool CollideOccured { get; set; }
        public void Update(GameTime gameTime, List<Item> items, Dictionary<string, Collider> objects, MouseManager mouse)
        {
            if (Activate)
            {
               // this.IsPerformingAction = PlayerActionAnimations[0].IsAnimated;
               

                KeyboardState kState = Keyboard.GetState();
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 oldPosition = this.Position;


                for (int i = 0; i < animations.GetLength(1); i++)
                {
                    PlayerMovementAnimations[i] = animations[(int)controls.Direction, i];
                }



                if (controls.IsMoving && !IsPerformingAction)
                {
                    for (int i = 0; i < animations.GetLength(0); i++)
                    {
                        for (int j = 0; j < animations.GetLength(1); j++)
                        {
                            animations[i, j].UpdateAnimations(gameTime, this.position);
                        }
                    }

                }
                else if (this.IsPerformingAction == true)
                {
                    PlayCollectiveActions(gameTime);
                    this.IsPerformingAction = PlayerActionAnimations[0].IsAnimated;

                }

                else if (!CurrentAction[0, 0].IsAnimated && !controls.IsMoving)
                {
                    for (int i = 0; i < PlayerMovementAnimations.GetLength(0); i++)
                    {
                        PlayerMovementAnimations[i].SetFrame(0);
                    }

                }


                IsMoving = false;

                if (!CurrentAction[0, 0].IsAnimated)
                {
                    controls.Update();
                }


                if (controls.IsMoving && !IsPerformingAction)
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
                            break;
                        case SecondaryDir.Left:
                            SecondaryVelocity.X = -SecondarySpeed;
                            for (int i = 0; i < animations.GetLength(1); i++)
                            {
                                PlayerMovementAnimations[i] = animations[(int)Dir.Left, i];
                            }

                            break;
                        case SecondaryDir.Down:
                            SecondaryVelocity.Y = SecondarySpeed;
                            for (int i = 0; i < animations.GetLength(1); i++)
                            {
                                PlayerMovementAnimations[i] = animations[(int)Dir.Down, i];
                            }


                            break;
                        case SecondaryDir.Up:
                            SecondaryVelocity.Y = -SecondarySpeed;
                            for (int i = 0; i < animations.GetLength(1); i++)
                            {
                                PlayerMovementAnimations[i] = animations[(int)Dir.Up, i];
                            }


                            break;

                        default:
                            break;

                    }


                    MyCollider.Rectangle = this.ColliderRectangle;
                    MyCollider.Velocity = this.PrimaryVelocity;
                    MyCollider.DidCollideMagnet(items);
                    // if(this.PrimaryVelocity.X > )

                    // MyCollider.DidCollide(objects, position); //did a collision with an object happen this loop?
                    List<Collider> returnObjects = new List<Collider>();
                    Game1.GetCurrentStage().QuadTree.Retrieve(returnObjects, MyCollider);
                    for (int i = 0; i < returnObjects.Count; i++)
                    {
                        //if obj collided with item in list stop it from moving boom badda bing

                        if (MyCollider.DidCollide(returnObjects[i], position))
                        {
                            //Console.WriteLine("collide occurred");
                            //player.MyCollider.Velocity = player.PrimaryVelocity;
                            CollideOccured = true;
                        }

                    }
                    this.PrimaryVelocity = MyCollider.Velocity;

                    if (this.CollideOccured) //if collision occurred we don't want to take diagonal movement into account
                    {
                        TotalVelocity = PrimaryVelocity;
                        // SecondaryVelocity = Vector2.Zero;
                    }
                    else
                    {
                        TotalVelocity = PrimaryVelocity + SecondaryVelocity;
                    }


                    if (controls.IsSprinting)
                    {
                        TotalVelocity = TotalVelocity * 15f;
                    }
                    Position += TotalVelocity;
                    PrimaryVelocity = Vector2.Zero;
                    SecondaryVelocity = Vector2.Zero;
                    TotalVelocity = Vector2.Zero;
                    if (LockBounds)
                    {
                        CheckOutOfBounds(this.Position);
                    }
                    

                }

            }
        }

        private void CheckOutOfBounds(Vector2 position)
        {
            if (position.X < Game1.GetCurrentStage().MapRectangle.Left)
            {
                this.Position = new Vector2(Game1.GetCurrentStage().MapRectangle.Left, this.position.Y);
            }


            if (position.X > Game1.GetCurrentStage().MapRectangle.Right)
            {
                this.Position = new Vector2(Game1.GetCurrentStage().MapRectangle.Right, this.position.Y);
            }
            if (position.Y < Game1.GetCurrentStage().MapRectangle.Top)
            {
                this.Position = new Vector2(this.position.X, Game1.GetCurrentStage().MapRectangle.Top);
            }
            if (position.Y > Game1.GetCurrentStage().MapRectangle.Bottom - 16)
            {
                this.Position = new Vector2(this.position.X, Game1.GetCurrentStage().MapRectangle.Bottom - 16);
            }
        }


        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {



            if (!IsPerformingAction)
            {

                for (int i = 0; i < PlayerMovementAnimations.GetLength(0); i++)
                {
                    PlayerMovementAnimations[i].DrawAnimation(spriteBatch, this.Position, PlayerMovementAnimations[i].LayerDepth + layerDepth);
                }

            }

            //????
            if (IsPerformingAction)
            {
                DrawCollectiveActions(spriteBatch, layerDepth);
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
            spriteBatch.Draw(LittleHitBoxRectangleTexture, new Vector2(ColliderRectangle.X, ColliderRectangle.Y), color: Color.White, layerDepth: layerDepth);
        }


        public void DrawUserInterface(SpriteBatch spriteBatch)
        {
            this.UserInterface.Draw(spriteBatch);
        }

    }
}