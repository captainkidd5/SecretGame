using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.CollisionDetection.ProjectileStuff;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.LightStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.NPCStuff.CaptureCrateStuff;
using SecretProject.Class.NPCStuff.Enemies;
using SecretProject.Class.Physics;
using SecretProject.Class.Physics.Tools;
using SecretProject.Class.SavingStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.IO;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;
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


    public class Player : IEntity, ILightBlockable, ICollidable
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

        public const float BaseSpeed = 5f;
        public float CurrentSpeed { get; set; }



        public Texture2D Texture { get; set; }
        public int FrameNumber { get; set; }

        public bool IsPerformingAction = false;

        public Tool CurrentEquippedTool { get; set; }
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


        //TAKEDAMAGE
        public SimpleTimer DamageImmunityTimer;
        public SimpleTimer KnockBackTimer { get; set; }
        public bool IsImmuneToDamage { get; set; }
        public bool IsBeingKnockedBack { get; private set; }
        public Vector2 KnockBackVector { get; private set; }

        public Vector2 WorldSquarePosition { get; set; }



        public Wardrobe Wardrobe { get; set; }

        //PENUMBRA
        public List<Light> PenumbraLights { get; set; }
        public Hull Hull { get; set; }

        //VELCROPHYSICS
        public Body CollisionBody { get; set; }

        public Body LargeProximitySensor { get; set; }


        public Player(string name, Vector2 position, Texture2D texture, int numberOfFrames, ContentManager content, GraphicsDevice graphics)
        {
            this.content = content;
            this.Name = name;
            this.MaxHealth = 10;
            this.Health = this.MaxHealth;
            this.Position = position;
            this.Graphics = graphics;
            this.Texture = texture;
            this.FrameNumber = numberOfFrames;

            this.Inventory = new Inventory(30) { Money = 10000 };

            this.EnableControls = true;
            controls = new PlayerControls(0);



            BigHitBoxRectangleTexture = Game1.Utility.GetBorderOnlyRectangleTexture(graphics, this.ClickRangeRectangle.Width, this.ClickRangeRectangle.Height, Color.White);
            LittleHitBoxRectangleTexture = Game1.Utility.GetBorderOnlyRectangleTexture(graphics, this.ColliderRectangle.Width, this.ColliderRectangle.Height, Color.White);

            this.LockBounds = true;
            this.DamageImmunityTimer = new SimpleTimer(1.5f);
            this.KnockBackTimer = new SimpleTimer(1f);


            this.Wardrobe = new Wardrobe(graphics, position);
            this.Hull = Hull.CreateRectangle(this.Position, new Vector2(6, 6));

            this.CurrentSpeed = BaseSpeed;

        }

        public void CreateBody()
        {
            this.CollisionBody = BodyFactory.CreateCircle(Game1.VelcroWorld, 6, 0f, this.position);
            CollisionBody.BodyType = BodyType.Dynamic;
            CollisionBody.Restitution = 0f;
            CollisionBody.Friction = .4f;
            CollisionBody.Mass = 1f;
            CollisionBody.Inertia = 0;
            CollisionBody.SleepingAllowed = true;
            CollisionBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Player;
            CollisionBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Solid |
                VelcroPhysics.Collision.Filtering.Category.Item |
                VelcroPhysics.Collision.Filtering.Category.TransparencySensor |
                VelcroPhysics.Collision.Filtering.Category.Enemy;

            CollisionBody.IgnoreGravity = true;
            CollisionBody.OnCollision += OnCollision;
            CollisionBody.OnSeparation += OnSeparation;


            this.LargeProximitySensor = BodyFactory.CreateRectangle(Game1.VelcroWorld, 32, 32, 1f);
            LargeProximitySensor.Position = this.Position;
            LargeProximitySensor.BodyType = BodyType.Dynamic;
             LargeProximitySensor.IsSensor = true;
            CollisionBody.Enabled = true;
            LargeProximitySensor.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Player;
            LargeProximitySensor.CollidesWith = VelcroPhysics.Collision.Filtering.Category.ProximitySensor;

            LargeProximitySensor.IgnoreGravity = true;


        }

        public void SetPosition(Vector2 position)
        {
            CollisionBody.SetTransform(position, 0f);
            this.Position = CollisionBody.Position;
            this.LargeProximitySensor.Position = this.Position;
        }

        public void LoadPenumbra(Stage stage)
        {

            stage.Penumbra.Hulls.Add(Hull);
            Hull.Enabled = true;
            //this.PenumbraLights = new List<Light>()
            //{
            //    new PointLight()
            //    {
            //        Position = position,
            //        Scale = new Vector2(64),
            //        ShadowType = ShadowType.Occluded,
            //        Color = Color.FloralWhite,

            //    },
            // };
            //for (int i = 0; i < this.PenumbraLights.Count; i++)
            //{
            //   // penumbra.Lights.Add(this.PenumbraLights[i]);
            //};
        }
        private void OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)

        {

            if (fixtureB.CollisionCategories == VelcroPhysics.Collision.Filtering.Category.TransparencySensor)
            {

                (fixtureB.Body.UserData as Tile).ColorMultiplier = .25f;
            }
            else if(fixtureB.CollisionCategories == VelcroPhysics.Collision.Filtering.Category.ProximitySensor)
            {
                Console.WriteLine("hi");
            }
        }

        private void OnSeparation(Fixture fixtureA, Fixture fixtureB, Contact contact)

        {
            if (fixtureB.CollisionCategories == VelcroPhysics.Collision.Filtering.Category.TransparencySensor)
            {
                Console.WriteLine("hi");
                (fixtureB.Body.UserData as Tile).ColorMultiplier = 1f;
            }
        }
        private void UpdatePenumbraLights()
        {
            //for (int i = 0; i < this.PenumbraLights.Count; i++)
            //{
            //   // PenumbraLights[i].Position = new Vector2(this.MainCollider.Rectangle.X, this.MainCollider.Rectangle.Y);
            //}
            UpdateHullPosition();
        }

        public void UpdateHullPosition()
        {
            this.Hull.Position = new Vector2(this.Position.X + 8, this.Position.Y + 32);
        }

        public ItemData GetCurrentEquippedToolData()
        {
            return Game1.ItemVault.GetData(this.UserInterface.BackPack.GetCurrentEquippedTool());
        }

        public int GetCurrentToolID()
        {
            ItemData data = Game1.ItemVault.GetData(this.UserInterface.BackPack.GetCurrentEquippedTool());
            if (data != null)
            {
                return data.ID;
            }
            else
            {
                return -1;
            }
        }

        public void ReplaceCurrentItem(int newID)
        {
            UserInterface.BackPack.ReplaceCurrentItem(newID);
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
                    this.CurrentEquippedTool = Tool.CreateTool(this.Graphics, this, direction, GetCurrentEquippedToolData());
                    //Wardrobe.ChangeTool(UserInterface.BackPack.GetCurrentEquippedTool(), direction);

                    break;

                case AnimationType.Chopping:
                    IsPerformingAction = true;
                    Wardrobe.CurrentAnimationSet = Wardrobe.ChopSet;
                    //Wardrobe.ChangeTool(UserInterface.BackPack.GetCurrentEquippedTool(), direction);
                    this.CurrentEquippedTool = Tool.CreateTool(this.Graphics, this, direction, GetCurrentEquippedToolData());
                    break;

                case AnimationType.Swiping:
                    IsPerformingAction = true;
                    Wardrobe.CurrentAnimationSet = Wardrobe.SwipeSet;
                    this.CurrentEquippedTool = Tool.CreateTool(this.Graphics, this, direction, GetCurrentEquippedToolData());
                    break;

                case AnimationType.PortalJump:
                    IsPerformingAction = true;
                    break;
            }
            this.AnimationDirection = direction;

        }
        #region AUTOMOVEMENT
        private bool MoveTowardsPoint(Vector2 goal, float speed, GameTime gameTime)
        {
            this.controls.IsMoving = true;
            this.EnableControls = true;
            if (this.Position == goal) return true;

            Vector2 direction = Vector2.Normalize(goal - this.Position);

            this.PrimaryVelocity = direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - this.Position)) + 1) < 0.1f)
                this.Position = goal;

            return this.Position == goal;
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
        public Vector2 PlayerCamPos { get; set; }
        public bool CollideOccured { get; set; }
        public void Update(GameTime gameTime)
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

                if (IsBeingKnockedBack)
                {
                    KnockBack(gameTime);
                }


                if (Game1.MouseManager.IsClicked && this.UserInterface.BackPack.GetCurrentEquippedToolAsItem() != null)
                {
                    ItemData item = this.UserInterface.BackPack.GetCurrentEquippedToolAsItem();
                    if (item.Type == XMLData.ItemStuff.ItemType.Sword)
                    {

                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.SwordSwing, true, 1f);
                        DoPlayerAnimation(AnimationType.Swiping);
                        Game1.ItemVault.AlterDurability(item, 1);
                        UserInterface.StaminaBar.DecreaseStamina(1);
                    }
                    else if (item.Type == XMLData.ItemStuff.ItemType.Bow)
                    {
                        if (UserInterface.BackPack.Inventory.ContainsAtLeastOne(280))
                        {
                            ItemData arrowData = Game1.ItemVault.GetData(280);
                            CheckMouseRotationFromEntity(this.Position);
                            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.BowShoot, true, .15f);
                            //Game1.CurrentStage.AllProjectiles.Add(new Projectile(this.Graphics, this.MainCollider, this.Direction, new Vector2(this.Position.X + 8, this.Position.Y + 8),
                            //    MathHelper.ToRadians(Game1.MouseManager.MouseAngleInRelationToPlayer - 90), 160f, Vector2.Zero, Game1.CurrentStage.AllProjectiles, false, arrowData.Damage));
                            UserInterface.BackPack.Inventory.RemoveItem(280);
                            Game1.ItemVault.AlterDurability(item, 1);
                            UserInterface.StaminaBar.DecreaseStamina(1);
                        }

                    }
                    if (item.CrateType != 0)
                    {



                    }

                }


                if (this.IsMoving && !IsPerformingAction)
                {
                    HandleStamina();


                }
                else if (IsPerformingAction)
                {
                    if (Wardrobe.PlayAnimationOnce(gameTime, Wardrobe.CurrentAnimationSet, this.Position, GetAnimationDirection()))
                    {
                        this.IsPerformingAction = false;
                        Wardrobe.CurrentAnimationSet = Wardrobe.RunSet;
                        if(CurrentEquippedTool != null)
                        {
                            this.CurrentEquippedTool.Remove();
                            this.CurrentEquippedTool = null;
                        }
                      
                    }

                    if (this.CurrentEquippedTool != null)
                    {
                        CurrentEquippedTool.Update(gameTime);
      
                    }

                }

                //else if (!CurrentAction[0, 0].IsAnimated && !this.IsMoving)
                //{
                //    Wardrobe.SetZero();


                //}
                int sprintMultiplier = 1;
                if (controls.IsMoving && !IsPerformingAction)
                {
                    UpdateStaminaDrainFlag();
                    if (controls.IsSprinting)
                    {
                        sprintMultiplier =250;
                    }

                    if (this.LockBounds)
                    {
                        CheckOutOfBounds(this.Position);
                    }
                }

                Dir movementDir = Dir.None;
                if (EnableControls)
                {
                    movementDir = MoveFromKeys(gameTime);
                }

                if (movementDir != Dir.None)
                {
                    CollisionBody.LinearVelocity = PrimaryVelocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds * sprintMultiplier;
                    CollisionBody.Inertia = 0f;

                    //CollisionBody.LinearVelocity = PrimaryVelocity;
                }
                else
                {
                    CollisionBody.Inertia = 0f;
                    CollisionBody.LinearVelocity = Vector2.Zero;
                    CollisionBody.AngularVelocity = 0f;
                }
                this.LargeProximitySensor.Position = CollisionBody.Position;
                this.LargeProximitySensor.LinearVelocity = CollisionBody.LinearVelocity;
                LargeProximitySensor.Inertia = CollisionBody.Inertia;
                LargeProximitySensor.AngularVelocity = CollisionBody.AngularVelocity;
                Position = new Vector2(CollisionBody.Position.X - 8, CollisionBody.Position.Y - 32);
                //Wardrobe.SetZero();
                PlayerCamPos = new Vector2((int)this.Position.X, (int)this.Position.Y);

                if (!IsPerformingAction)
                    Wardrobe.UpdateAnimations(gameTime, Position, movementDir, this.IsMoving);


                UpdatePenumbraLights();

            }
        }

        private Dir MoveFromKeys(GameTime gameTime)
        {
            Dir movementDir = Dir.None;
            if (!IsPerformingAction)
            {

                switch (controls.Direction)
                {
                    case Dir.Right:
                        PrimaryVelocity.X = CurrentSpeed;
                        movementDir = Dir.Right;
                        break;

                    case Dir.Left:
                        PrimaryVelocity.X = -CurrentSpeed;
                        movementDir = Dir.Left;
                        break;

                    case Dir.Down:
                        PrimaryVelocity.Y = CurrentSpeed;
                        movementDir = Dir.Down;
                        break;

                    case Dir.Up:
                        PrimaryVelocity.Y = -CurrentSpeed;
                        movementDir = Dir.Up;
                        break;

                    default:
                        break;

                }

                switch (controls.SecondaryDirection) //if a second key is being pressed we'll use that for the animation direction instead of the first one pressed.
                {
                    case Dir.Right:
                        PrimaryVelocity.X = CurrentSpeed;
                        movementDir = Dir.Right;
                        break;
                    case Dir.Left:
                        PrimaryVelocity.X = -CurrentSpeed;
                        movementDir = Dir.Left;

                        break;
                    case Dir.Down:
                        PrimaryVelocity.Y = CurrentSpeed;
                        movementDir = Dir.Down;

                        break;
                    case Dir.Up:
                        PrimaryVelocity.Y = -CurrentSpeed;
                        movementDir = Dir.Up;
                        break;

                    default:
                        break;

                }

            }
            return movementDir;
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
            //if (position.X < Game1.CurrentStage.MapRectangle.Left)
            //{
            //    this.Position = new Vector2(Game1.CurrentStage.MapRectangle.Left, this.position.Y);
            //}


            //if (position.X > Game1.CurrentStage.MapRectangle.Right)
            //{
            //    this.Position = new Vector2(Game1.CurrentStage.MapRectangle.Right, this.position.Y);
            //}
            //if (position.Y < Game1.CurrentStage.MapRectangle.Top)
            //{
            //    this.Position = new Vector2(this.position.X, Game1.CurrentStage.MapRectangle.Top);
            //}
            //if (position.Y > Game1.CurrentStage.MapRectangle.Bottom - 16)
            //{
            //    this.Position = new Vector2(this.position.X, Game1.CurrentStage.MapRectangle.Bottom - 16);
            //}
        }

        public int WalkSoundEffect { get; set; }

        public bool IsDrawn { get; set; }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (this.IsDrawn)
            {

                Wardrobe.Draw(spriteBatch, layerDepth);
                if(CurrentEquippedTool != null)
                {
                    CurrentEquippedTool.Draw(spriteBatch, layerDepth);
                }
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
                angle = 360 - Math.Abs(angle);
            }


            Game1.MouseManager.MouseAngleInRelationToPlayer = angle;
        }

        public Dir GetAnimationDirection()
        {
            CheckMouseRotationFromEntity(new Vector2(this.Position.X + 8, this.Position.Y + 16));
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

        public void DoPlayerAnimation(AnimationType animationType, float delayTimer = 0f, ItemData item = null)
        {

            if (item != null)
            {
                PlayAnimation(animationType, GetAnimationDirection(), this.UserInterface.BackPack.GetCurrentEquippedToolAsItem().AnimationColumn);
            }
            else
            {
                PlayAnimation(animationType, GetAnimationDirection());
            }

        }


        public void DrawDebug(SpriteBatch spriteBatch, float layerDepth)
        {
         // REDO  spriteBatch.Draw(BigHitBoxRectangleTexture, new Vector2(this.ClickRangeRectangle.X, this.ClickRangeRectangle.Y), color: Color.White, layerDepth: layerDepth);
            //REDO spriteBatch.Draw(LittleHitBoxRectangleTexture, new Vector2(this.ColliderRectangle.X, this.ColliderRectangle.Y), color: Color.White, layerDepth: layerDepth);
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


        public void RestorePlayerToFull()
        {
            this.UserInterface.StaminaBar.RefillStamina();
            this.Health = this.MaxHealth;
        }

        public void TakeDamage(int dmgAmount)
        {
            if (!Game1.Flags.EnablePlayerInvincibility)
            {
                this.Health -= (int)dmgAmount;
                if (this.Health == 0)
                {
                    this.Health = 6;
                    //Game1.SwitchStage(Game1.PlayerHouse);
                    this.Position = new Vector2(600, 600);
                    Game1.GlobalClock.IncrementDay();
                    UserInterface.BackPack.Inventory.Money -= 200;
                    UserInterface.AddAlert(AlertType.Normal, Game1.Utility.centerScreen, "You have run out of health (-200G)!");
                }
                this.IsImmuneToDamage = true;
                //  UserInterface.AllRisingText.Add(new RisingText(new Vector2(this.MainCollider.Rectangle.X + 600, this.MainCollider.Rectangle.Y), 100, "-" + dmgAmount.ToString(), 50f, Color.Red, true, 3f, true));
            }
            else
            {
                //   UserInterface.AllRisingText.Add(new RisingText(new Vector2(this.MainCollider.Rectangle.X + 600, this.MainCollider.Rectangle.Y), 100, "Invincible!", 50f, Color.Red, true, 3f, true));
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