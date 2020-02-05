using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.CollisionDetection.ProjectileStuff;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.NPCStuff.CaptureCrateStuff;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;

namespace SecretProject.Class.Playable
{
    public enum AnimationType
    {
        HandsPicking = 0,
        Chopping = 21,
        Mining = 22,
        Digging = 23,
        Swiping = 25,
        PortalJump = 30
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
        public bool EnableControls { get; set; }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public int Health { get; set; }


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
        public Sprite[,] PortalJump { get; set; }
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

        //WHIRLPOOL
        public Vector2 MoveToPosition { get; private set; }
        public bool IsMovingTowardsPoint { get; private set; }
        public bool TransportAfterMove { get; set; }

        //TAKEDAMAGE
        public SimpleTimer DamageImmunityTimer;
        public SimpleTimer KnockBackTimer { get; set; }
        public bool IsImmuneToDamage { get; set; }
        public bool IsBeingKnockedBack { get; private set; }
        public Vector2 KnockBackVector { get; private set; }

        public Player(string name, Vector2 position, Texture2D texture, int numberOfFrames, int numberOfBodyParts, ContentManager content, GraphicsDevice graphics, MouseManager mouse)
        {
            this.content = content;
            this.Name = name;
            this.Health = 10;
            this.Position = position;
            this.Graphics = graphics;
            this.Texture = texture;
            this.FrameNumber = numberOfFrames;
            animations = new Sprite[numberOfFrames, numberOfBodyParts];
            this.Mining = new Sprite[4, 6];
            this.Swiping = new Sprite[4, 5];
            this.PickUpItem = new Sprite[4, 5];
            this.PortalJump = new Sprite[4, 5];

            this.MainCollider = new Collider(graphics, this.ColliderRectangle, this, ColliderType.inert);
            this.BigCollider = new Collider(graphics, this.ClickRangeRectangle, this, ColliderType.PlayerBigBox);
            this.Inventory = new Inventory(30) { Money = 10000 };

            this.EnableControls = true;
            controls = new PlayerControls(0);



            CurrentAction = this.Mining;

            BigHitBoxRectangleTexture = Game1.Utility.GetBorderOnlyRectangleTexture(graphics, this.ClickRangeRectangle.Width, this.ClickRangeRectangle.Height, Color.White);
            LittleHitBoxRectangleTexture = Game1.Utility.GetBorderOnlyRectangleTexture(graphics, this.ColliderRectangle.Width, this.ColliderRectangle.Height, Color.White);

            this.LockBounds = true;
            this.DamageImmunityTimer = new SimpleTimer(1.5f);
            this.KnockBackTimer = new SimpleTimer(1f);

        }



        public void PlayAnimation(AnimationType action, int textureColumn = 0)
        {

            switch (action)
            {
                case AnimationType.HandsPicking:
                    IsPerformingAction = true;
                    CurrentAction = this.PickUpItem;
                    this.AnimationDirection = controls.Direction;

                    break;

                case AnimationType.Mining:
                    IsPerformingAction = true;
                    CurrentAction = this.Mining;
                    this.AnimationDirection = controls.Direction;
                    for (int i = 0; i < 4; i++)
                    {
                        this.Mining[i, 0].FirstFrameY = textureColumn;
                    }

                    break;

                case AnimationType.Chopping:
                    IsPerformingAction = true;
                    CurrentAction = this.Mining;
                    this.AnimationDirection = controls.Direction;
                    for (int i = 0; i < 4; i++)
                    {
                        this.Mining[i, 0].FirstFrameY = textureColumn;
                    }
                    break;

                case AnimationType.Swiping:
                    IsPerformingAction = true;
                    CurrentAction = this.Swiping;
                    this.AnimationDirection = controls.Direction;
                    this.CurrentTool = this.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ItemSprite;
                    this.CurrentTool.Origin = new Vector2(this.CurrentTool.SourceRectangle.Width, this.CurrentTool.SourceRectangle.Height);
                    AdjustCurrentTool(controls.Direction, this.CurrentTool);
                    this.ToolLine = new Line(this.CurrentTool.Position, new Vector2(1, 1));
                    //new Vector2((float)Math.Tan(CurrentTool.Position.X), (float)Math.Tan(CurrentTool.Position.Y))
                    for (int i = 0; i < 4; i++)
                    {
                        this.Swiping[i, 0].FirstFrameY = textureColumn;
                    }
                    break;

                case AnimationType.PortalJump:
                    IsPerformingAction = true;
                    CurrentAction = this.PortalJump;
                    this.AnimationDirection = controls.Direction;

                    break;
            }


        }
        private bool MoveTowardsPoint(Vector2 goal,float speed, GameTime gameTime)
        {
            this.controls.IsMoving  = true;
            this.EnableControls = true;
            if (this.Position == goal) return true;

            Vector2 direction = Vector2.Normalize(goal - this.Position);

            this.PrimaryVelocity = direction * speed;

            if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - this.Position)) + 1) < 0.1f)
                this.Position = goal;

            return this.Position == goal;
        }
        public void MoveToPoint(Vector2 position)
        {
            this.MoveToPosition = position;
            this.IsMovingTowardsPoint = true;
        }
        public void MoveAwayFromPoint(Vector2 positionToMoveAwayFrom, GameTime gameTime)
        {

            this.IsMoving = true;


            Vector2 direction = Vector2.Normalize(positionToMoveAwayFrom - this.Position);

            this.PrimaryVelocity += direction * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;

        }

        //adjusts current equipped items position based on direction player is facing
        public void AdjustCurrentTool(Dir direction, Sprite sprite)
        {
            switch (direction)
            {
                case Dir.Up:
                    sprite.Position = new Vector2(position.X + 12, position.Y + 14);
                    sprite.Rotation = 0f;
                    sprite.SpinAmount = 3f;
                    sprite.LayerDepth = .000000006f;
                    sprite.SpinSpeed = 5f;
                    break;

                case Dir.Down:
                    sprite.Position = new Vector2(position.X + 12, position.Y + 22);
                    sprite.Rotation = 5f;
                    sprite.SpinAmount = -4f;
                    sprite.LayerDepth = .00000012f;
                    sprite.SpinSpeed = 6f;
                    break;

                case Dir.Right:
                    sprite.Position = new Vector2(position.X + 14, position.Y + 18);
                    sprite.Rotation = 1f;
                    sprite.SpinAmount = 6f;
                    sprite.LayerDepth = .00000012f;
                    sprite.SpinSpeed = 6f;
                    break;

                case Dir.Left:
                    sprite.Position = new Vector2(position.X + 4, position.Y + 18);
                    sprite.Rotation = 6f;
                    sprite.SpinAmount = -6f;
                    sprite.LayerDepth = .00000012f;
                    sprite.SpinSpeed = 6f;
                    break;
                default:
                    sprite.Position = new Vector2(position.X + 16, position.Y + 16);
                    sprite.Rotation = 0f;
                    sprite.SpinAmount = 2f;
                    break;
            }
        }


        public float CalculateStaminaRateOfDrain()
        {
            float staminaRateOfDrain = 0f;
            if(Game1.GetCurrentStage() == Game1.OverWorld)
            {
                staminaRateOfDrain = Math.Abs(this.Position.X) + Math.Abs(this.Position.Y);
            }

            return staminaRateOfDrain;
        }

        public void PlayCollectiveActions(GameTime gameTime)
        {
            for (int i = 0; i < CurrentAction.GetLength(1); i++)
            {
                this.PlayerActionAnimations[i] = CurrentAction[(int)this.AnimationDirection, i];
                this.PlayerActionAnimations[i].IsAnimated = true;
                this.PlayerActionAnimations[i].PlayOnce(gameTime, this.Position);
            }

        }
        public void DrawCollectiveActions(SpriteBatch spriteBatch, float layerDepth)
        {
            for (int i = 0; i < CurrentAction.GetLength(1); i++)
            {
                this.PlayerActionAnimations[i].DrawAnimation(spriteBatch, this.PlayerActionAnimations[i].destinationVector, layerDepth + this.PlayerActionAnimations[i].LayerDepth);
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

        public void UpdateAnimationPosition()
        {
            for (int i = 0; i < animations.GetLength(0); i++)
            {
                for (int j = 0; j < animations.GetLength(1); j++)
                {
                    animations[i, j].UpdateAnimationPosition(this.Position);

                }

            }
        }
        public void TestImmunity(GameTime gameTime)
        {
            if (IsImmuneToDamage)
            {
                if (DamageImmunityTimer.Run(gameTime))
                {
                    this.IsImmuneToDamage = false;
                }
            }
        }

        public bool CollideOccured { get; set; }
        public void Update(GameTime gameTime, List<Item> items, MouseManager mouse)
        {
            if (this.Activate)
            {
                PrimaryVelocity = Vector2.Zero;

                this.IsMoving = controls.IsMoving;
                TestImmunity(gameTime);
                if(IsMovingTowardsPoint)
                {
                    UpdateAnimationPosition();   
                    if(MoveTowardsPoint(this.MoveToPosition, 1f, gameTime))
                    {
                        this.IsMovingTowardsPoint = false;
                        if(TransportAfterMove)
                        {
                            Game1.Player.UserInterface.WarpGate.Transport(Game1.Player.UserInterface.WarpGate.To);
                            TransportAfterMove = false;
                        }
                        
                    }
                }
               


                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 oldPosition = this.Position;

                if(IsBeingKnockedBack)
                {
                    KnockBack(gameTime);
                }
                for (int i = 0; i < animations.GetLength(1); i++)
                {
                    this.PlayerMovementAnimations[i] = animations[(int)controls.Direction, i];
                }

                if (mouse.IsClicked && this.UserInterface.BackPack.GetCurrentEquippedToolAsItem() != null)
                {
                    Item item = this.UserInterface.BackPack.GetCurrentEquippedToolAsItem();
                    if (item.ItemType == XMLData.ItemStuff.ItemType.Sword)
                    {
                        Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.SwordSwing, true, 1f);
                        DoPlayerAnimation(AnimationType.Swiping);
                    }
                    else if(item.ItemType == XMLData.ItemStuff.ItemType.Bow)
                    {
                        if(UserInterface.BackPack.Inventory.ContainsAtLeastOne(280))
                        {
                            CheckMouseRotationFromEntity(this.Position);
                            Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.BowShoot, true, .15f);
                            Game1.GetCurrentStage().AllProjectiles.Add(new Projectile(this.Graphics, this, this.Direction, new Vector2(this.Position.X + 8, this.Position.Y + 8), MathHelper.ToRadians(Game1.myMouseManager.MouseAngleInRelationToPlayer - 90), 40f, Vector2.Zero, Game1.GetCurrentStage().AllProjectiles));
                            UserInterface.BackPack.Inventory.RemoveItem(280);
                        }
                        
                    }
                    if (item.CrateType != 0)
                    {

                        if (Game1.GetCurrentStage().StageType == StageType.Procedural)
                        {


                            CaptureCrate.Release((EnemyType)item.CrateType, this.Graphics, Game1.GetCurrentStage().AllTiles.ChunkUnderPlayer);
                            this.UserInterface.BackPack.Inventory.RemoveItem(item);
                            DoPlayerAnimation(AnimationType.HandsPicking);
                        }
                        else if (Game1.GetCurrentStage().StageType == StageType.Sanctuary)
                        {
                            SanctuaryTracker tracker = Game1.GetSanctuaryTrackFromStage(Game1.GetCurrentStageInt());
                            if (tracker.UpdateCompletionGuide(item.ID))
                            {
                                CaptureCrate.Release((EnemyType)item.CrateType, this.Graphics);
                                this.UserInterface.BackPack.Inventory.RemoveItem(item);
                                DoPlayerAnimation(AnimationType.HandsPicking);
                            }
                        }

                    }

                }


                if (this.IsMoving && !IsMovingTowardsPoint && !IsPerformingAction)
                {
                    if(Game1.GetCurrentStage() == Game1.OverWorld)
                    {
                        if(!Game1.OverWorld.CheckIfWithinStaminaSafeZone(this.Position))
                        {
                            UserInterface.StaminaBar.IsDraining = true;
                            UserInterface.StaminaBar.StaminaStatus.UpdateStaminaRectangle(true);
                        }
                        else
                        {
                            UserInterface.StaminaBar.IsDraining = false;
                            UserInterface.StaminaBar.StaminaStatus.UpdateStaminaRectangle(false);
                        }
                    }
                    else if(UserInterface.StaminaBar.IsDraining)
                    {
                        UserInterface.StaminaBar.IsDraining = false;
                        UserInterface.StaminaBar.StaminaStatus.UpdateStaminaRectangle(false);
                    }
                    for (int i = 0; i < animations.GetLength(0); i++)
                    {
                        for (int j = 0; j < animations.GetLength(1); j++)
                        {
                            animations[i, j].UpdateAnimationPosition(this.Position);
                            animations[i, j].UpdateAnimations(gameTime, position);

                        }

                    }

                }
                else if (IsPerformingAction)
                {
                    PlayCollectiveActions(gameTime);
                    IsPerformingAction = this.PlayerActionAnimations[0].IsAnimated;
                    if (this.CurrentTool != null)
                    {
                        this.CurrentTool.UpdateAnimationTool(gameTime, this.CurrentTool.SpinAmount, this.CurrentTool.SpinSpeed);
                        this.ToolLine.Point2 = new Vector2(this.CurrentTool.Position.X + 20, this.CurrentTool.Position.Y + 20);
                        this.ToolLine.Rotation = this.CurrentTool.Rotation + (float)3.5;


                    }

                    if (IsPerformingAction == false)
                    {
                        this.CurrentTool = null;
                    }

                }

                else if (!CurrentAction[0, 0].IsAnimated && !this.IsMoving)
                {
                    for (int i = 0; i < this.PlayerMovementAnimations.GetLength(0); i++)
                    {
                        this.PlayerMovementAnimations[i].SetFrame(0);
                    }

                }




                if (!CurrentAction[0, 0].IsAnimated)
                {
                    controls.Update();
                }
                if(EnableControls)
                {
                    MoveFromKeys();
                }
                

                this.MainCollider.Rectangle = this.ColliderRectangle;


                this.BigCollider.Rectangle = this.ClickRangeRectangle;


                CheckAndHandleCollisions();


                if (controls.IsMoving && !IsPerformingAction)
                {
                    UpdateStaminaDrainFlag();
                    if (controls.IsSprinting)
                    {
                        this.Position += PrimaryVelocity * 10;
                    }
                    else
                    {
                        this.Position += PrimaryVelocity;
                    }


                    if (this.LockBounds)
                    {
                        CheckOutOfBounds(this.Position);
                    }


                }

            }
        }

        private void MoveFromKeys()
        {
            if (this.IsMoving && !IsPerformingAction)
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
                            this.PlayerMovementAnimations[i] = animations[(int)Dir.Right, i];
                        }
                        break;
                    case SecondaryDir.Left:
                        PrimaryVelocity.X = -Speed1;
                        for (int i = 0; i < animations.GetLength(1); i++)
                        {
                            this.PlayerMovementAnimations[i] = animations[(int)Dir.Left, i];
                        }

                        break;
                    case SecondaryDir.Down:
                        PrimaryVelocity.Y = Speed1;
                        for (int i = 0; i < animations.GetLength(1); i++)
                        {
                            this.PlayerMovementAnimations[i] = animations[(int)Dir.Down, i];
                        }

                        break;
                    case SecondaryDir.Up:
                        PrimaryVelocity.Y = -Speed1;
                        for (int i = 0; i < animations.GetLength(1); i++)
                        {
                            this.PlayerMovementAnimations[i] = animations[(int)Dir.Up, i];
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

                Game1.GetCurrentStage().QuadTree.Retrieve(returnObjects, this.BigCollider);
            
                
            
            for (int i = 0; i < returnObjects.Count; i++)
            {

                if (returnObjects[i].ColliderType == ColliderType.Item)
                {

                    if (this.BigCollider.IsIntersecting(returnObjects[i]))
                    {
                        returnObjects[i].Entity.PlayerCollisionInteraction(0, 5, this.controls.Direction);
                    }
                }
                else if (returnObjects[i].ColliderType == ColliderType.grass)
                {
                    if (this.MainCollider.IsIntersecting(returnObjects[i]))
                    {
                        returnObjects[i].IsUpdating = true;
                        returnObjects[i].InitialShuffDirection = controls.Direction;
                        //if (Game1.EnablePlayerCollisions)
                        //{
                        //    CurrentSpeed = Speed1 / 2;
                        //}
                    }
                    #region SWORD INTERACTIONS
                    if (this.CurrentTool != null)
                    {
                        if (this.ToolLine.IntersectsRectangle(returnObjects[i].Rectangle))
                        {
                            returnObjects[i].SelfDestruct();
                        }
                    }
                    #endregion
                }
                else if (returnObjects[i].ColliderType == ColliderType.Enemy)
                {
                    if (this.CurrentTool != null)
                    {
                        if (this.ToolLine.IntersectsRectangle(returnObjects[i].Rectangle))
                        {
                            returnObjects[i].Entity.PlayerCollisionInteraction(1, 10, this.controls.Direction);
                        }
                    }
                }
                else if (returnObjects[i].ColliderType == ColliderType.TransperencyDetector) //used to make things transparent when walked behind.
                {
                    if(this.MainCollider.IsIntersecting(returnObjects[i]))
                    {
                        returnObjects[i].Entity.PlayerCollisionInteraction(0, 10, this.controls.Direction);
                    }
                }
                else
                {
                    if (this.IsMoving)
                    {
                        if (returnObjects[i].Entity != this && returnObjects[i].ColliderType != ColliderType.MouseCollider)
                        {
                            if (Game1.EnablePlayerCollisions)
                            {
                                if (this.MainCollider.HandleMove(this.Position, ref PrimaryVelocity, returnObjects[i]))
                                { this.IsBeingKnockedBack = false; }



                            }


                        }


                    }

                }

            }
        }

        public void UpdateStaminaDrainFlag()
        {
            if (Game1.GetCurrentStage() == Game1.OverWorld)
            {
                if (!Game1.OverWorld.CheckIfWithinStaminaSafeZone(this.Position))
                {
                    UserInterface.StaminaBar.IsDraining = true;
                }
                else
                {
                    UserInterface.StaminaBar.IsDraining = false;
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
            if (this.IsDrawn)
            {


                if (!IsPerformingAction)
                {

                    for (int i = 0; i < this.PlayerMovementAnimations.GetLength(0); i++)
                    {
                        this.PlayerMovementAnimations[i].DrawAnimation(spriteBatch, this.PlayerMovementAnimations[i].destinationVector, this.PlayerMovementAnimations[i].LayerDepth + layerDepth);
                        if (this.IsMoving)
                        {
                            if ((this.PlayerMovementAnimations[i].CurrentFrame == 3 && oldSoundFrame1 != 3) || (this.PlayerMovementAnimations[i].CurrentFrame == 0 && oldSoundFrame1 != 0))
                            {
                                Game1.SoundManager.PlaySoundEffectFromInt(1, this.WalkSoundEffect);
                            }
                        }

                        oldSoundFrame1 = this.PlayerMovementAnimations[i].CurrentFrame;
                    }

                }

                //????
                if (IsPerformingAction)
                {
                    DrawCollectiveActions(spriteBatch, layerDepth);
                }

                if (this.CurrentTool != null)
                {
                    this.CurrentTool.DrawRotationalSprite(spriteBatch, this.CurrentTool.Position, this.CurrentTool.Rotation, this.CurrentTool.Origin, layerDepth + this.CurrentTool.LayerDepth);
                    if (Game1.GetCurrentStage().ShowBorders)
                    {
                        this.ToolLine.DrawLine(Game1.AllTextures.redPixel, spriteBatch, Color.Red, this.CurrentTool.Rotation + 4);
                    }

                }
            }

        }

        public void CheckMouseRotationFromEntity(Vector2 positionToCheck)
        {
            Vector2 direction = positionToCheck - Game1.myMouseManager.WorldMousePosition;


            // Vector2 direction = Game1.myMouseManager.WorldMousePosition - positionToCheck;
            float angle = MathHelper.ToDegrees((float)(Math.Atan2(direction.X, direction.Y)) * -1);
            if (angle < 0)
            {
                angle = 360 - Math.Abs(angle) ;
            }


            Game1.myMouseManager.MouseAngleInRelationToPlayer = angle;
        }

        public void DoPlayerAnimation(AnimationType animationType, float delayTimer = 0f, Item item = null)
        {
            CheckMouseRotationFromEntity(this.Position);
            if (Game1.myMouseManager.MouseAngleInRelationToPlayer > 285 || Game1.myMouseManager.MouseAngleInRelationToPlayer < 45)
            {
                controls.Direction = Dir.Up;

            }

            else if( Game1.myMouseManager.MouseAngleInRelationToPlayer >= 135 && Game1.myMouseManager.MouseAngleInRelationToPlayer <= 225)
            {
                controls.Direction = Dir.Down;
            }

            else if (Game1.myMouseManager.MouseAngleInRelationToPlayer >= 45 && Game1.myMouseManager.MouseAngleInRelationToPlayer < 135)
            {
                controls.Direction = Dir.Right;
            }
            else if (Game1.myMouseManager.MouseAngleInRelationToPlayer > 225 && Game1.myMouseManager.MouseAngleInRelationToPlayer <=285)
            {
                controls.Direction = Dir.Left;
            }
            if (item != null)
            {
                PlayAnimation(animationType, this.UserInterface.BackPack.GetCurrentEquippedToolAsItem().AnimationColumn);
            }
            else
            {
                PlayAnimation(animationType);
            }
            
        }


        public void DrawDebug(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(BigHitBoxRectangleTexture, new Vector2(this.ClickRangeRectangle.X, this.ClickRangeRectangle.Y), color: Color.White, layerDepth: layerDepth);
            spriteBatch.Draw(LittleHitBoxRectangleTexture, new Vector2(this.ColliderRectangle.X, this.ColliderRectangle.Y), color: Color.White, layerDepth: layerDepth);
        }


        public void DrawUserInterface(SpriteBatch spriteBatch)
        {
            this.UserInterface.Draw(spriteBatch);
        }

        public void KnockBack(GameTime gameTime)
        {

            if(KnockBackTimer.Run(gameTime))
            {
                this.IsBeingKnockedBack = false;
                EnableControls = true;
            }
            else
            {
                MoveAwayFromPoint(this.KnockBackVector, gameTime);
                EnableControls = false;
            }
        }

        public void Interact()
        {
            throw new NotImplementedException();
        }



        public void TakeDamage(int dmgAmount)
        {
            this.Health -= (int)dmgAmount;
            if(this.Health == 0)
            {
                this.Health = 6;
            }
            this.IsImmuneToDamage = true;
            UserInterface.AllRisingText.Add(new RisingText(new Vector2(this.MainCollider.Rectangle.X +600, this.MainCollider.Rectangle.Y), 100, "-" + dmgAmount.ToString(), 50f, Color.Red, true, 3f, true));
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void PlayerCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom)
        {
            if (!this.IsImmuneToDamage)
            {

                IsBeingKnockedBack = true;
                switch (directionAttackedFrom)
                {
                    case Dir.Down:
                        this.KnockBackVector = new Vector2(this.Position.X, this.Position.Y + knockBack);
                        break;
                    case Dir.Right:
                        this.KnockBackVector = new Vector2(this.Position.X + knockBack, this.Position.Y);
                        break;
                    case Dir.Left:
                        this.KnockBackVector = new Vector2(this.Position.X - knockBack, this.Position.Y);
                        break;
                    case Dir.Up:
                        this.KnockBackVector = new Vector2(this.Position.X, this.Position.Y - knockBack);
                        break;
                    default:
                        this.KnockBackVector = new Vector2(this.Position.X, this.Position.Y - knockBack);
                        break;
                }

                TakeDamage(dmgAmount);
            }
        }

        public void MouseCollisionInteraction()
        {
           // throw new NotImplementedException();
        }
    }
}