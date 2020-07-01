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
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.IO;
using XMLData.ItemStuff;

namespace SecretProject.Class.Playable
{
    public enum AnimationType
    {
        HandsPicking = 0,
        Walking = 1,
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

        public int MaxHealth { get; set; }
        public int Health { get; set; }


        public Dir Direction { get; set; } = Dir.Down;
        public Dir SecondDirection { get; set; } = Dir.Down;
        public Dir AnimationDirection { get; set; }
        public bool IsMoving { get; set; } = false;
        public const float Speed1 = 1f;



        public Texture2D Texture { get; set; }
        public int FrameNumber { get; set; }

        public RectangleCollider MainCollider { get; set; }
        public RectangleCollider BigCollider { get; set; }

        public bool IsPerformingAction = false;

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
                return new Rectangle((int)position.X + 4, (int)position.Y + 28, 8, 4);
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

        public Vector2 WorldSquarePosition { get; set; }



        public Wardrobe Wardrobe { get; set; }

        public Player(string name, Vector2 position, Texture2D texture, int numberOfFrames,  ContentManager content, GraphicsDevice graphics)
        {
            this.content = content;
            this.Name = name;
            this.MaxHealth = 10;
            this.Health = this.MaxHealth;
            this.Position = position;
            this.Graphics = graphics;
            this.Texture = texture;
            this.FrameNumber = numberOfFrames;


            this.MainCollider = new RectangleCollider(graphics, this.ColliderRectangle, this, ColliderType.PlayerMainCollider);
            this.BigCollider = new RectangleCollider(graphics, this.ClickRangeRectangle, this, ColliderType.PlayerBigBox);
            this.Inventory = new Inventory(30) { Money = 10000 };

            this.EnableControls = true;
            controls = new PlayerControls(0);





            BigHitBoxRectangleTexture = Game1.Utility.GetBorderOnlyRectangleTexture(graphics, this.ClickRangeRectangle.Width, this.ClickRangeRectangle.Height, Color.White);
            LittleHitBoxRectangleTexture = Game1.Utility.GetBorderOnlyRectangleTexture(graphics, this.ColliderRectangle.Width, this.ColliderRectangle.Height, Color.White);

            this.LockBounds = true;
            this.DamageImmunityTimer = new SimpleTimer(1.5f);
            this.KnockBackTimer = new SimpleTimer(1f);


            this.Wardrobe = new Wardrobe(graphics, position);
        }

        public ItemData GetCurrentEquippedToolData()
        {
            return Game1.ItemVault.GetItem(this.UserInterface.BackPack.GetCurrentEquippedTool());
        }


        public void PlayAnimation(AnimationType action, Dir direction, int textureColumn = 0)
        {

            switch (action)
            {
                case AnimationType.HandsPicking:
                    Wardrobe.CurrentAnimationSet = Wardrobe.PickUpItemSet;
                    IsPerformingAction = true;
                    break;

                case AnimationType.Mining:
                    IsPerformingAction = true;
                    Wardrobe.CurrentAnimationSet = Wardrobe.ChopSet;
                    Wardrobe.ChangeTool(UserInterface.BackPack.GetCurrentEquippedTool(), direction);

                    break;

                case AnimationType.Chopping:
                    IsPerformingAction = true;
                    Wardrobe.CurrentAnimationSet = Wardrobe.ChopSet;
                    Wardrobe.ChangeTool(UserInterface.BackPack.GetCurrentEquippedTool(), direction);
                    break;

                case AnimationType.Swiping:
                    IsPerformingAction = true;
                    Wardrobe.CurrentAnimationSet = Wardrobe.SwipeSet;
                    Wardrobe.ChangeTool(UserInterface.BackPack.GetCurrentEquippedTool(), direction);

                    break;

                case AnimationType.PortalJump:
                    IsPerformingAction = true;
                    break;
            }
            this.AnimationDirection = direction;

        }
        #region AUTOMOVEMENT
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
        #endregion
        


        public float CalculateStaminaRateOfDrain()
        {
            float staminaRateOfDrain = 0f;
            //if(Game1.CurrentStage == Game1.OverWorld)
            //{
            //    staminaRateOfDrain = Math.Abs(this.Position.X) + Math.Abs(this.Position.Y);
            //}

            return staminaRateOfDrain;
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
                controls.UpdateKeys();
                controls.Update();
                this.IsMoving = controls.IsMoving;
                this.EnableControls = true;

                TestImmunity(gameTime);
                
               
                Vector2 oldPosition = this.Position;

                if(IsBeingKnockedBack)
                {
                    KnockBack(gameTime);
                }


                if (mouse.IsClicked && this.UserInterface.BackPack.GetCurrentEquippedToolAsItem() != null)
                {
                    Item item = this.UserInterface.BackPack.GetCurrentEquippedToolAsItem();
                    if (item.ItemType == XMLData.ItemStuff.ItemType.Sword)
                    {
                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.SwordSwing, true, 1f);
                        DoPlayerAnimation(AnimationType.Swiping);
                        item.AlterDurability(1);
                        UserInterface.StaminaBar.DecreaseStamina(1);
                    }
                    else if(item.ItemType == XMLData.ItemStuff.ItemType.Bow)
                    {
                        if(UserInterface.BackPack.Inventory.ContainsAtLeastOne(280))
                        {
                            ItemData arrowData = Game1.ItemVault.GetItem(280);
                            CheckMouseRotationFromEntity(this.Position);
                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.BowShoot, true, .15f);
                            Game1.CurrentStage.AllProjectiles.Add(new Projectile(this.Graphics, this.MainCollider, this.Direction, new Vector2(this.Position.X + 8, this.Position.Y + 8), MathHelper.ToRadians(Game1.MouseManager.MouseAngleInRelationToPlayer - 90), 160f, Vector2.Zero, Game1.CurrentStage.AllProjectiles,false, arrowData.Damage));
                            UserInterface.BackPack.Inventory.RemoveItem(280);
                            item.AlterDurability(1);
                            UserInterface.StaminaBar.DecreaseStamina(1);
                        }
                        
                    }
                    if (item.CrateType != 0)
                    {

                        if (Game1.CurrentStage.StageType == StageType.Procedural)
                        {


                            CaptureCrate.Release((EnemyType)item.CrateType, this.Graphics, Game1.CurrentStage.AllTiles.ChunkUnderPlayer);
                            this.UserInterface.BackPack.Inventory.RemoveItem(item);
                            DoPlayerAnimation(AnimationType.HandsPicking);
                        }
                        else if (Game1.CurrentStage.StageType == StageType.Sanctuary)
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
                    HandleStamina();
                  

                }
                else if (IsPerformingAction)
                {
                  if(Wardrobe.PlayAnimationOnce(gameTime, Wardrobe.CurrentAnimationSet, this.Position,GetAnimationDirection()))
                    {
                        this.IsPerformingAction = false;
                        Wardrobe.CurrentAnimationSet = Wardrobe.RunSet;
                    }

                  //  IsPerformingAction = this.PlayerActionAnimations[0].IsAnimated;
                    //if (this.CurrentTool != null)
                    //{
                    //    this.CurrentTool.UpdateAnimationTool(gameTime, this.CurrentTool.SpinAmount, this.CurrentTool.SpinSpeed);
                    //    this.ToolLine.Point2 = new Vector2(this.CurrentTool.Position.X + 20, this.CurrentTool.Position.Y + 20);
                    //    this.ToolLine.Rotation = this.CurrentTool.Rotation + (float)3.5;


                    //}

                    if (IsPerformingAction == false)
                    {
                        this.CurrentTool = null;
                    }

                }

                //else if (!CurrentAction[0, 0].IsAnimated && !this.IsMoving)
                //{
                //    Wardrobe.SetZero();


                //}



                if(EnableControls)
                {
                    MoveFromKeys(gameTime);
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

        private void MoveFromKeys(GameTime gameTime)
        {
            if ( !IsPerformingAction)
            {
                switch (controls.Direction)
                {
                    case Dir.Right:
                        PrimaryVelocity.X = Speed1;
                        Wardrobe.UpdateAnimations(gameTime, this.Position, Dir.Right, this.IsMoving);
                        break;

                    case Dir.Left:
                        PrimaryVelocity.X = -Speed1;
                        Wardrobe.UpdateAnimations(gameTime, this.Position, Dir.Left, this.IsMoving);
                        break;

                    case Dir.Down:
                        PrimaryVelocity.Y = Speed1;
                        Wardrobe.UpdateAnimations(gameTime, this.Position, Dir.Down, this.IsMoving);
                        break;

                    case Dir.Up:
                        PrimaryVelocity.Y = -Speed1;
                        Wardrobe.UpdateAnimations(gameTime, this.Position, Dir.Up, this.IsMoving);
                        break;

                    default:
                        break;

                }

                switch (controls.SecondaryDirection)
                {
                    case Dir.Right:
                        PrimaryVelocity.X = Speed1;
                       // Wardrobe.UpdateMovementAnimations(gameTime, this.Position, Dir.Right,this.IsMoving);
                        break;
                    case Dir.Left:
                        PrimaryVelocity.X = -Speed1;
                       // Wardrobe.UpdateMovementAnimations(gameTime, this.Position, Dir.Left, this.IsMoving);

                        break;
                    case Dir.Down:
                        PrimaryVelocity.Y = Speed1;
                        //Wardrobe.UpdateMovementAnimations(gameTime, this.Position, Dir.Down, this.IsMoving);

                        break;
                    case Dir.Up:
                        PrimaryVelocity.Y = -Speed1;
                      //  Wardrobe.UpdateMovementAnimations(gameTime, this.Position, Dir.Up, this.IsMoving);
                        break;

                    default:
                        break;

                }
            }
        }

        private void CheckAndHandleCollisions()
        {
            List<ICollidable> returnObjects = new List<ICollidable>();

                Game1.CurrentStage.QuadTree.Retrieve(returnObjects, this.BigCollider);
            
                
            
            for (int i = 0; i < returnObjects.Count; i++)
            {

                if (returnObjects[i].ColliderType == ColliderType.Item)
                {

                    if (this.MainCollider.IsIntersecting(returnObjects[i]))
                    {
                        (returnObjects[i].Entity as Item).TryMagnetize();
                    }
                }
                else if (returnObjects[i].ColliderType == ColliderType.grass)
                {
                    if (this.MainCollider.IsIntersecting(returnObjects[i]))
                    {
                        (returnObjects[i].Entity as GrassTuft).IsUpdating = true;
                        (returnObjects[i].Entity as GrassTuft).InitialShuffDirection = controls.Direction;
                        
                        //if (Game1.EnablePlayerCollisions)
                        //{
                        //    CurrentSpeed = Speed1 / 2;
                        //}
                    }
                    #region SWORD INTERACTIONS
                    if (this.Wardrobe.ToolPiece != null)
                    {
                        if (this.Wardrobe.ToolPiece.ToolLine.IntersectsRectangle(returnObjects[i].Rectangle))
                        {
                            (returnObjects[i] as GrassTuft).SelfDestruct();
                        }
                    }
                    #endregion
                }
                else if (returnObjects[i].ColliderType == ColliderType.Enemy)
                {
                    if (this.Wardrobe.ToolPiece != null)
                    {
                        if (this.Wardrobe.ToolPiece.ToolLine.IntersectsRectangle(returnObjects[i].Rectangle))
                        {
                            returnObjects[i].Entity.DamageCollisionInteraction(Game1.ItemVault.GetItem(this.UserInterface.BackPack.GetCurrentEquippedToolAsItem().ID).Damage, 10, this.controls.Direction);
                        }
                    }
                }
                else if (returnObjects[i].ColliderType == ColliderType.TransperencyDetector) //used to make things transparent when walked behind.
                {
                    if(this.MainCollider.IsIntersecting(returnObjects[i]))
                    {
                        returnObjects[i].Entity.DamageCollisionInteraction(0, 10, this.controls.Direction);
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
            //if (Game1.CurrentStage == Game1.OverWorld)
            //{
            //    if (!Game1.OverWorld.CheckIfWithinStaminaSafeZone(this.Position))
            //    {
            //        UserInterface.StaminaBar.IsDraining = true;
            //    }
            //    else
            //    {
            //        UserInterface.StaminaBar.IsDraining = false;
            //    }
            //}
        }

        public void HandleStamina()
        {
            //if (Game1.CurrentStage == Game1.OverWorld)
            //{
            //    if (!Game1.OverWorld.CheckIfWithinStaminaSafeZone(this.Position))
            //    {
            //        UserInterface.StaminaBar.IsDraining = true;
            //        UserInterface.StaminaBar.StaminaStatus.UpdateStaminaRectangle(true);
            //    }
            //    else
            //    {
            //        UserInterface.StaminaBar.IsDraining = false;
            //        UserInterface.StaminaBar.StaminaStatus.UpdateStaminaRectangle(false);
            //    }
            //}
            //else if (UserInterface.StaminaBar.IsDraining)
            //{
            //    UserInterface.StaminaBar.IsDraining = false;
            //    UserInterface.StaminaBar.StaminaStatus.UpdateStaminaRectangle(false);
            //}
        }

        private void CheckOutOfBounds(Vector2 position)
        {
            if (position.X < Game1.CurrentStage.MapRectangle.Left)
            {
                this.Position = new Vector2(Game1.CurrentStage.MapRectangle.Left, this.position.Y);
            }


            if (position.X > Game1.CurrentStage.MapRectangle.Right)
            {
                this.Position = new Vector2(Game1.CurrentStage.MapRectangle.Right, this.position.Y);
            }
            if (position.Y < Game1.CurrentStage.MapRectangle.Top)
            {
                this.Position = new Vector2(this.position.X, Game1.CurrentStage.MapRectangle.Top);
            }
            if (position.Y > Game1.CurrentStage.MapRectangle.Bottom - 16)
            {
                this.Position = new Vector2(this.position.X, Game1.CurrentStage.MapRectangle.Bottom - 16);
            }
        }
        int oldSoundFrame1 = 0;
        public int WalkSoundEffect { get; set; }

        public bool IsDrawn { get; set; }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (this.IsDrawn)
            {

                Wardrobe.Draw(spriteBatch, layerDepth);
                if (!IsPerformingAction)
                {


                        if (this.IsMoving)
                        {
                            //if ((this.PlayerMovementAnimations[i].CurrentFrame == 3 && oldSoundFrame1 != 3) || (this.PlayerMovementAnimations[i].CurrentFrame == 0 && oldSoundFrame1 != 0))
                            //{
                            //    Game1.SoundManager.PlaySoundEffectFromInt(1, this.WalkSoundEffect);
                            //}
                        }

                       // oldSoundFrame1 = this.PlayerMovementAnimations[i].CurrentFrame;
                    

                }
            }

        }

        public void CheckMouseRotationFromEntity(Vector2 positionToCheck)
        {
            Vector2 direction = positionToCheck - Game1.MouseManager.WorldMousePosition;


            // Vector2 direction = Game1.myMouseManager.WorldMousePosition - positionToCheck;
            float angle = MathHelper.ToDegrees((float)(Math.Atan2(direction.X, direction.Y)) * -1);
            if (angle < 0)
            {
                angle = 360 - Math.Abs(angle) ;
            }


            Game1.MouseManager.MouseAngleInRelationToPlayer = angle;
        }

        public Dir GetAnimationDirection()
        {
            CheckMouseRotationFromEntity(new Vector2(this.Position.X + 8, this.Position.Y + 20));
            if (Game1.MouseManager.MouseAngleInRelationToPlayer > 285 || Game1.MouseManager.MouseAngleInRelationToPlayer < 45)
            {
                return Dir.Up;

            }

            else if (Game1.MouseManager.MouseAngleInRelationToPlayer >= 135 && Game1.MouseManager.MouseAngleInRelationToPlayer <= 225)
            {
                return Dir.Down;
            }

            else if (Game1.MouseManager.MouseAngleInRelationToPlayer >= 45 && Game1.MouseManager.MouseAngleInRelationToPlayer < 135)
            {
                return Dir.Right;
            }
            else if (Game1.MouseManager.MouseAngleInRelationToPlayer > 225 && Game1.MouseManager.MouseAngleInRelationToPlayer <= 285)
            {
                return Dir.Left;
            }
            return Dir.Up;
        }

        public void DoPlayerAnimation(AnimationType animationType, float delayTimer = 0f, Item item = null)
        {
          //  CheckMouseRotationFromEntity(this.Position);

            if (item != null)
            {
                PlayAnimation(animationType, GetAnimationDirection(),this.UserInterface.BackPack.GetCurrentEquippedToolAsItem().AnimationColumn );
            }
            else
            {
                PlayAnimation(animationType, GetAnimationDirection());
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

            //if(KnockBackTimer.Run(gameTime))
            //{
            //    this.IsBeingKnockedBack = false;
            //    EnableControls = true;
            //}
            //else
            //{
            //    MoveAwayFromPoint(this.KnockBackVector, gameTime);
            //    EnableControls = false;
            //}
        }

        public void Interact()
        {
            throw new NotImplementedException();
        }

        public void RestorePlayerToFull()
        {
            this.UserInterface.StaminaBar.RefillStamina();
            this.Health = this.MaxHealth;
        }

        public void TakeDamage(int dmgAmount)
        {
            if(!Game1.EnablePlayerInvincibility)
            {
                this.Health -= (int)dmgAmount;
                if (this.Health == 0)
                {
                    this.Health = 6;
                    Game1.SwitchStage(Game1.GetCurrentStageInt(), Stages.PlayerHouse);
                    this.Position = new Vector2(600, 600);
                    Game1.GlobalClock.IncrementDay();
                    UserInterface.BackPack.Inventory.Money -= 200;
                    UserInterface.AddAlert(AlertType.Normal, Game1.Utility.centerScreen, "You have run out of health (-200G)!");
                }
                this.IsImmuneToDamage = true;
                UserInterface.AllRisingText.Add(new RisingText(new Vector2(this.MainCollider.Rectangle.X + 600, this.MainCollider.Rectangle.Y), 100, "-" + dmgAmount.ToString(), 50f, Color.Red, true, 3f, true));
            }
            else
            {
                UserInterface.AllRisingText.Add(new RisingText(new Vector2(this.MainCollider.Rectangle.X + 600, this.MainCollider.Rectangle.Y), 100,"Invincible!", 50f, Color.Red, true, 3f, true));
            }
            
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void DamageCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom)
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

        public void Save(BinaryWriter writer)
        {
            writer.Write(this.Name);
            this.Inventory.Save(writer);
            this.Wardrobe.Save(writer);
        }
        public void Load(BinaryReader reader)
        {
            this.Name = reader.ReadString();
            this.Inventory = new Inventory();
            this.Inventory.Load(reader);
            this.Wardrobe.Load(reader);
        }
    }
}