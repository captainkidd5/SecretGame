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
using SecretProject.Class.NPCStuff.CaptureCrateStuff;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.StageFolder;

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

        public int Health { get; set; }
        public int Stamina { get; set; }


        public Dir Direction { get; set; } = Dir.Down;
        public SecondaryDir SecondDirection { get; set; } = SecondaryDir.Down;
        public Dir AnimationDirection { get; set; }
        public bool IsMoving { get; set; } = false;
        public const float Speed1 = 1f;

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
        public Sprite[,] PickUpItem { get; set; }
        public Line ToolLine { get; set; }

        public Sprite CurrentTool { get; set; }


        public UserInterface UserInterface { get; set; }

        public Texture2D BigHitBoxRectangleTexture;
        public Texture2D LittleHitBoxRectangleTexture;
        public bool LockBounds { get; set; }

        public GraphicsDevice Graphics { get; set; }
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
            this.Graphics = graphics;
            this.Texture = texture;
            this.FrameNumber = numberOfFrames;
            animations = new Sprite[numberOfFrames, numberOfBodyParts];
            Mining = new Sprite[4, 6];
            Swiping = new Sprite[4, 5];
            PickUpItem = new Sprite[4, 5];

            MainCollider = new Collider(graphics, ColliderRectangle, this, ColliderType.inert);
            BigCollider = new Collider(graphics, ClickRangeRectangle, this, ColliderType.PlayerBigBox);
            Inventory = new Inventory(30) { Money = 10000 };

            controls = new PlayerControls(0);



            CurrentAction = Mining;

            BigHitBoxRectangleTexture = Game1.Utility.GetBorderOnlyRectangleTexture(graphics, ClickRangeRectangle.Width, ClickRangeRectangle.Height, Color.White);
            LittleHitBoxRectangleTexture = Game1.Utility.GetBorderOnlyRectangleTexture(graphics, ColliderRectangle.Width, ColliderRectangle.Height, Color.White);

            LockBounds = true;

        }



        public void PlayAnimation(GameTime gameTime, AnimationType action, int textureColumn = 0)
        {

            switch (action)
            {
                case AnimationType.HandsPicking:
                    IsPerformingAction = true;
                    CurrentAction = PickUpItem;
                    AnimationDirection = controls.Direction;
                    //for (int i = 0; i < 4; i++)
                    //{
                    //    this.PickUpItem[i, 0].FirstFrameY = textureColumn;
                    //}
                    break;

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
                    this.CurrentTool = UserInterface.BackPack.GetCurrentEquippedToolAsItem().ItemSprite;
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
            for (int i = 0; i < CurrentAction.GetLength(1); i++)
            {
                PlayerActionAnimations[i].DrawAnimation(spriteBatch, PlayerActionAnimations[i].destinationVector, layerDepth + PlayerActionAnimations[i].LayerDepth);
            }

        }

        public void UpdateMovementAnimationsOnce()
        {
            for (int i = 0; i < animations.GetLength(0); i++)
            {
                for (int j = 0; j < animations.GetLength(1); j++)
                {
                    animations[i, j].SetFrame(0);
                    animations[i, j].UpdateAnimationPosition(this.Position);
                }
            }
        }
        public bool CollideOccured { get; set; }
        public void Update(GameTime gameTime, List<Item> items, MouseManager mouse)
        {
            if (Activate)
            {

                PrimaryVelocity = Vector2.Zero;

                IsMoving = controls.IsMoving;
                KeyboardState kState = Keyboard.GetState();
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 oldPosition = this.Position;


                for (int i = 0; i < animations.GetLength(1); i++)
                {
                    PlayerMovementAnimations[i] = animations[(int)controls.Direction, i];
                }

                if (mouse.IsClicked && UserInterface.BackPack.GetCurrentEquippedToolAsItem() != null)
                {
                    Item item = UserInterface.BackPack.GetCurrentEquippedToolAsItem();
                    if (item.ItemType == XMLData.ItemStuff.ItemType.Sword)
                    {
                        Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.Slash1);
                        DoPlayerAnimation(gameTime, AnimationType.Swiping);
                    }
                    if (item.CrateType != 0)
                    {
                        if (Game1.GetCurrentStage().StageType == StageType.Procedural)
                        {


                            CaptureCrate.Release((EnemyType)item.CrateType, Graphics, Game1.GetCurrentStage().AllTiles.ChunkUnderPlayer);
                            UserInterface.BackPack.Inventory.RemoveItem(item);
                        }
                        else if (Game1.GetCurrentStage().StageType == StageType.Sanctuary)
                        {
                            SanctuaryTracker tracker = Game1.GetSanctuaryTrackFromStage(Game1.GetCurrentStageInt());
                            if (tracker.UpdateCompletionGuide(item.ID))
                            {
                                CaptureCrate.Release((EnemyType)item.CrateType, Graphics);
                                UserInterface.BackPack.Inventory.RemoveItem(item);
                            }
                        }

                    }

                }


                if (IsMoving && !IsPerformingAction)
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

                else if (!CurrentAction[0, 0].IsAnimated && !IsMoving)
                {
                    for (int i = 0; i < PlayerMovementAnimations.GetLength(0); i++)
                    {
                        PlayerMovementAnimations[i].SetFrame(0);
                    }

                }




                if (!CurrentAction[0, 0].IsAnimated)
                {
                    controls.Update();
                }

                MoveFromKeys();

                MainCollider.Rectangle = this.ColliderRectangle;


                BigCollider.Rectangle = this.ClickRangeRectangle;


                CheckAndHandleCollisions();


                if (controls.IsMoving && !IsPerformingAction)
                {
                    if (controls.IsSprinting)
                    {
                        Position += PrimaryVelocity * 10;
                    }
                    else
                    {
                        Position += PrimaryVelocity;
                    }


                    if (LockBounds)
                    {
                        CheckOutOfBounds(this.Position);
                    }


                }

            }
        }

        private void MoveFromKeys()
        {
            if (IsMoving && !IsPerformingAction)
            {
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
                        PrimaryVelocity.X = Speed1;
                        for (int i = 0; i < animations.GetLength(1); i++)
                        {
                            PlayerMovementAnimations[i] = animations[(int)Dir.Right, i];
                        }
                        break;
                    case SecondaryDir.Left:
                        PrimaryVelocity.X = -Speed1;
                        for (int i = 0; i < animations.GetLength(1); i++)
                        {
                            PlayerMovementAnimations[i] = animations[(int)Dir.Left, i];
                        }

                        break;
                    case SecondaryDir.Down:
                        PrimaryVelocity.Y = Speed1;
                        for (int i = 0; i < animations.GetLength(1); i++)
                        {
                            PlayerMovementAnimations[i] = animations[(int)Dir.Down, i];
                        }

                        break;
                    case SecondaryDir.Up:
                        PrimaryVelocity.Y = -Speed1;
                        for (int i = 0; i < animations.GetLength(1); i++)
                        {
                            PlayerMovementAnimations[i] = animations[(int)Dir.Up, i];
                        }
                        break;

                    default:
                        break;

                }
            }
        }

        private void CheckAndHandleCollisions()
        {
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
                        //if (Game1.EnablePlayerCollisions)
                        //{
                        //    CurrentSpeed = Speed1 / 2;
                        //}
                    }
                    #region SWORD INTERACTIONS
                    if (CurrentTool != null)
                    {
                        if (ToolLine.IntersectsRectangle(returnObjects[i].Rectangle))
                        {
                            returnObjects[i].SelfDestruct();
                        }
                    }
                    #endregion
                }
                else if (returnObjects[i].ColliderType == ColliderType.Enemy)
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
                            if (Game1.EnablePlayerCollisions)
                            {
                                MainCollider.HandleMove(Position, ref PrimaryVelocity, returnObjects[i]);

                            }


                        }


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

        public bool IsDrawn { get; set; }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (IsDrawn)
            {


                if (!IsPerformingAction)
                {

                    for (int i = 0; i < PlayerMovementAnimations.GetLength(0); i++)
                    {
                        PlayerMovementAnimations[i].DrawAnimation(spriteBatch, PlayerMovementAnimations[i].destinationVector, PlayerMovementAnimations[i].LayerDepth + layerDepth);
                        if (IsMoving)
                        {
                            if ((PlayerMovementAnimations[i].CurrentFrame == 3 && oldSoundFrame1 != 3) || (PlayerMovementAnimations[i].CurrentFrame == 0 && oldSoundFrame1 != 0))
                            {
                                Game1.SoundManager.PlaySoundEffectFromInt(1, this.WalkSoundEffect);
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
                    if (Game1.GetCurrentStage().ShowBorders)
                    {
                        ToolLine.DrawLine(Game1.AllTextures.redPixel, spriteBatch, Color.Red, CurrentTool.Rotation + 4);
                    }

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
                PlayAnimation(gameTime, animationType, UserInterface.BackPack.GetCurrentEquippedToolAsItem().AnimationColumn);
            }
            else
            {
                PlayAnimation(gameTime, animationType);
            }

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