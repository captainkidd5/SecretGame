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
using SecretProject.Class.Universal;
using SecretProject.Class.NPCStuff;

namespace SecretProject.Class.Playable
{
    public enum AnimationType
    {
        HandsPicking = 0,
        Chopping = 21,
        Mining = 22,
        Digging = 23,
        Swiping = 25,
    }


    public class Player : IEntity
    {

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

        public Collider MainCollider { get; set; }
        public Collider BigCollider { get; set; }

        public bool IsPerformingAction = false;

        public Sprite[,] CurrentAction;



        public Sprite[,] Mining { get; set; }
        public Sprite[,] Swiping { get; set; }
        public Line ToolLine { get; set; }

        public Sprite CurrentTool { get; set; }


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
                return new Rectangle((int)position.X - 24, (int)position.Y, 64, 64);
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
            Swiping = new Sprite[4, 5];

            MainCollider = new Collider(graphics, PrimaryVelocity, ColliderRectangle, this, ColliderType.inert);
            BigCollider = new Collider(graphics, PrimaryVelocity, ClickRangeRectangle, this, ColliderType.PlayerBigBox);
            Inventory = new Inventory(30) { Money = 10000 };

            controls = new PlayerControls(0);



            CurrentAction = Mining;

            BigHitBoxRectangleTexture = SetRectangleTexture(graphics, ClickRangeRectangle);
            LittleHitBoxRectangleTexture = SetRectangleTexture(graphics, ColliderRectangle);

            LockBounds = true;

            // this.CurrentTool = new Sprite(graphics, Game1.AllTextures.ItemSpriteSheet, Game1.ItemVault.GenerateNewItem(5, null).SourceTextureRectangle, Position, 16, 16);

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

                case AnimationType.Swiping:
                    IsPerformingAction = true;
                    CurrentAction = Swiping;
                    AnimationDirection = controls.Direction;
                    this.CurrentTool = UserInterface.BottomBar.GetCurrentEquippedToolAsItem().ItemSprite;
                    this.CurrentTool.Origin = new Vector2(CurrentTool.SourceRectangle.Width, CurrentTool.SourceRectangle.Height);
                    AdjustCurrentTool(controls.Direction, this.CurrentTool);
                    this.ToolLine = new Line(CurrentTool.Position, new Vector2(1, 1));
                    //new Vector2((float)Math.Tan(CurrentTool.Position.X), (float)Math.Tan(CurrentTool.Position.Y))
                    for (int i = 0; i < 4; i++)
                    {
                        this.Swiping[i, 0].FirstFrameY = textureColumn;
                    }
                    break;
            }


        }

        //adjusts current equipped items position based on direction player is facing
        public void AdjustCurrentTool(Dir direction, Sprite sprite)
        {
            switch (direction)
            {
                case Dir.Up:
                    sprite.Position = new Vector2(this.position.X + 12, this.position.Y + 14);
                    sprite.Rotation = 0f;
                    sprite.SpinAmount = 3f;
                    sprite.LayerDepth = .000000006f;
                    sprite.SpinSpeed = 5f;
                    break;

                case Dir.Down:
                    sprite.Position = new Vector2(this.position.X + 12, this.position.Y + 22);
                    sprite.Rotation = 5f;
                    sprite.SpinAmount = -4f;
                    sprite.LayerDepth = .00000012f;
                    sprite.SpinSpeed = 6f;
                    break;

                case Dir.Right:
                    sprite.Position = new Vector2(this.position.X + 14, this.position.Y + 18);
                    sprite.Rotation = 1f;
                    sprite.SpinAmount = 6f;
                    sprite.LayerDepth = .00000012f;
                    sprite.SpinSpeed = 6f;
                    break;

                case Dir.Left:
                    sprite.Position = new Vector2(this.position.X + 4, this.position.Y + 18);
                    sprite.Rotation = 6f;
                    sprite.SpinAmount = -6f;
                    sprite.LayerDepth = .00000012f;
                    sprite.SpinSpeed = 6f;
                    break;
                default:
                    sprite.Position = new Vector2(this.position.X + 16, this.position.Y + 16);
                    sprite.Rotation = 0f;
                    sprite.SpinAmount = 2f;
                    break;
            }
        }




        public void PlayCollectiveActions(GameTime gameTime)
        {
            for (int i = 0; i < CurrentAction.GetLength(1); i++)
            {
                PlayerActionAnimations[i] = CurrentAction[(int)AnimationDirection, i];
                PlayerActionAnimations[i].IsAnimated = true;
                PlayerActionAnimations[i].PlayOnce(gameTime, Position);
            }

        }
        public void DrawCollectiveActions(SpriteBatch spriteBatch, float layerDepth)
        {
            for (int i = 0; i < PlayerActionAnimations.Length; i++)
            {
                PlayerActionAnimations[i].DrawAnimation(spriteBatch, PlayerActionAnimations[i].destinationVector, layerDepth + PlayerActionAnimations[i].LayerDepth);
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
        public void Update(GameTime gameTime, List<Item> items, MouseManager mouse)
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

                if (mouse.IsClicked && UserInterface.BottomBar.GetCurrentEquippedToolAsItem() != null && UserInterface.BottomBar.GetCurrentEquippedToolAsItem().Type == 25)
                {
                    Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.Slash1, Game1.SoundManager.GameVolume);
                    DoPlayerAnimation(gameTime, AnimationType.Swiping);
                }


                if (controls.IsMoving && !IsPerformingAction)
                {
                    for (int i = 0; i < animations.GetLength(0); i++)
                    {
                        for (int j = 0; j < animations.GetLength(1); j++)
                        {
                            animations[i, j].UpdateAnimationPosition(this.Position);
                            animations[i, j].UpdateAnimations(gameTime, this.position);
                            
                        }
                        
                    }

                }
                else if (this.IsPerformingAction)
                {
                    PlayCollectiveActions(gameTime);
                    this.IsPerformingAction = PlayerActionAnimations[0].IsAnimated;
                    if (CurrentTool != null)
                    {
                        CurrentTool.UpdateAnimationTool(gameTime, CurrentTool.SpinAmount, CurrentTool.SpinSpeed);
                        ToolLine.Point2 = new Vector2(CurrentTool.Position.X + 20, CurrentTool.Position.Y + 20);
                        ToolLine.Rotation = CurrentTool.Rotation + (float)3.5;


                    }

                    if (this.IsPerformingAction == false)
                    {
                        this.CurrentTool = null;
                    }

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

                }
                MainCollider.Rectangle = this.ColliderRectangle;
                MainCollider.Velocity = this.PrimaryVelocity;

                BigCollider.Rectangle = this.ClickRangeRectangle;
                BigCollider.Velocity = this.PrimaryVelocity;

                List<ICollidable> returnObjects = new List<ICollidable>();
                Game1.GetCurrentStage().QuadTree.Retrieve(returnObjects, BigCollider);
                for (int i = 0; i < returnObjects.Count; i++)
                {

                    if (returnObjects[i].ColliderType == ColliderType.Item)
                    {

                        if (BigCollider.IsIntersecting(returnObjects[i]))
                        {
                            returnObjects[i].Entity.PlayerCollisionInteraction();
                        }
                    }
                    else if (returnObjects[i].ColliderType == ColliderType.grass)
                    {
                        if (MainCollider.IsIntersecting(returnObjects[i]))
                        {
                            returnObjects[i].IsUpdating = true;
                            returnObjects[i].InitialShuffDirection = this.controls.Direction;
                        }
                        #region SWORD INTERACTIONS
                        if (CurrentTool != null)
                        {
                            if (ToolLine.IntersectsRectangle(returnObjects[i].Rectangle))
                            {
                               Console.WriteLine("Intersected grass");
                             returnObjects[i].SelfDestruct();
                            }
                        }
                        #endregion
                    }
                    else if(returnObjects[i].ColliderType == ColliderType.Enemy)
                    {
                        if (CurrentTool != null)
                        {
                            if (ToolLine.IntersectsRectangle(returnObjects[i].Rectangle))
                            {
                                returnObjects[i].Entity.PlayerCollisionInteraction();
                            }
                        }
                    }
                    else
                    {
                        if (IsMoving)
                        {
                            if (returnObjects[i].Entity != this)
                            {
         
                                if (MainCollider.DidCollide(returnObjects[i], position))
                                {
                                    CollideOccured = true;
                                    //returnObjects[i].InitialShuffDirection = this.controls.Direction;
                                }

                            }


                        }

                    }

                }
                if (controls.IsMoving && !IsPerformingAction)
                {

                    if (this.CollideOccured) //if collision occurred we don't want to take diagonal movement into account
                    {
                        this.PrimaryVelocity = MainCollider.Velocity;
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
        int oldSoundFrame1 = 0;
        public int WalkSoundEffect { get; set; }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {

            if (!IsPerformingAction)
            {

                for (int i = 0; i < PlayerMovementAnimations.GetLength(0); i++)
                {
                    PlayerMovementAnimations[i].DrawAnimation(spriteBatch, PlayerMovementAnimations[i].destinationVector, PlayerMovementAnimations[i].LayerDepth + layerDepth);
                    if(IsMoving)
                    {
                        if ((PlayerMovementAnimations[i].CurrentFrame == 3 && oldSoundFrame1 != 3) || (PlayerMovementAnimations[i].CurrentFrame == 0 && oldSoundFrame1 != 0))
                        {
                            Game1.SoundManager.PlaySoundEffectFromInt(1, this.WalkSoundEffect, Game1.SoundManager.GameVolume);
                        }
                    }
                    
                    oldSoundFrame1 = PlayerMovementAnimations[i].CurrentFrame;
                }

            }

            //????
            if (IsPerformingAction)
            {
                DrawCollectiveActions(spriteBatch, layerDepth);
            }

            if (this.CurrentTool != null)
            {
                CurrentTool.DrawRotationalSprite(spriteBatch, CurrentTool.Position, CurrentTool.Rotation, CurrentTool.Origin, layerDepth + CurrentTool.LayerDepth);
                if(Game1.GetCurrentStage().ShowBorders)
                {
                    ToolLine.DrawLine(Game1.AllTextures.redPixel, spriteBatch, Color.Red, CurrentTool.Rotation + 4);
                }
                
            }

        }

        public void DoPlayerAnimation(GameTime gameTime, AnimationType animationType, float delayTimer = 0f, Item item = null)
        {
            if (Position.Y < Game1.myMouseManager.WorldMousePosition.Y - 30)
            {
                controls.Direction = Dir.Down;

            }

            else if (Game1.Player.Position.Y > Game1.myMouseManager.WorldMousePosition.Y)
            {
                controls.Direction = Dir.Up;
            }

            else if (Position.X < Game1.myMouseManager.WorldMousePosition.X)
            {
                controls.Direction = Dir.Right;
            }
            else if (Position.X > Game1.myMouseManager.WorldMousePosition.X)
            {
                controls.Direction = Dir.Left;
            }
            if (item != null)
            {
                PlayAnimation(gameTime, animationType, UserInterface.BottomBar.GetCurrentEquippedToolAsItem().AnimationColumn);
            }
            else
            {
                PlayAnimation(gameTime, animationType);
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

        public void KnockBack(Dir direction, int amount)
        {
            throw new NotImplementedException();
        }

        public void Interact()
        {
            throw new NotImplementedException();
        }

        public void PlayerCollisionInteraction()
        {

        }
    }
}