
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.PathFinding;
using SecretProject.Class.PathFinding.PathFinder;
using SecretProject.Class.Physics;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace SecretProject.Class.NPCStuff.Enemies
{
    public enum EnemyType
    {
        Boar = 1,
        Crab = 2,
        Rabbit = 3,
        Butterfly = 4
    }

    public enum CurrentBehaviour
    {
        Wander = 1,
        Chase = 2,
        Hurt = 3,
        Flee = 4,
    }



    public class Enemy : INPC, IEntity, ICollidable
    {

        protected GraphicsDevice Graphics { get; set; }
        public string Name { get; set; }
        public Vector2 Position;
        public Sprite[] NPCAnimatedSprite { get; set; }

        public Texture2D Texture { get; set; }
        public Texture2D HitBoxTexture { get; set; }

        public float Speed { get; set; }

        public Dir CurrentDirection;
        public bool IsMoving { get; set; }
        protected Vector2 primaryVelocity;
        protected Vector2 oldPrimaryVelocity;
        public Vector2 TotalVelocity { get; set; }
        public Vector2 DirectionVector { get; set; }
        public bool IsUpdating { get; set; }

        public bool CollideOccured { get; set; }

        protected Vector2 DebugNextPoint { get; set; } = new Vector2(1, 1);

        protected Texture2D NextPointTexture { get; set; }
        protected Rectangle NextPointRectangle { get; set; }
        protected Texture2D NextPointRectangleTexture { get; set; }
        //public int CurrentTileX { get { return (int)(this.Position.X / 16); } }
        // public int CurrentTileY { get { return (int)(this.Position.Y / 16); } }
        protected float SoundTimer { get; set; }
        protected float SoundMin { get; set; }
        protected float SoundMax { get; set; }

        private Color DebugColor { get; set; }

        public CurrentBehaviour CurrentBehaviour { get; set; }
        public CurrentBehaviour PrimaryPlayerInterationBehavior { get; set; }

        public bool DoesIntersectScreen { get; set; }

        public Rectangle NPCHitBoxRectangle { get; set; }
        //DAMAGE RELATED THINGS


        public int HitPoints { get; protected set; }
        public Color DamageColor { get; protected set; }
        public SimpleTimer DamageImmunityTimer { get; private set; }
        public bool IsImmuneToDamage { get; set; }

        public List<Loot> PossibleLoot { get; set; }



        public ObstacleGrid ObstacleGrid { get; set; }

        public EmoticonType CurrentEmoticon { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool MakesPeriodicSound { get; set; }
        public SoundEffect IdleSoundEffect { get; set; }
        public float SoundLowerBound { get; set; }
        public float SoundUpperBound { get; set; }

        public List<Enemy> Pack { get; set; }
        public bool HasPackAggression { get; set; }

        private Navigator Navigator { get; set; }

        private bool HasReachedNextPoint { get; set; }

        public Body Body { get; set; }
        public Body CollisionBody { get; set; }

        public Enemy(List<Enemy> pack, Vector2 position, GraphicsDevice graphics, TileManager TileManager)
        {
            this.Pack = pack;
            this.Position = position;
            this.Graphics = graphics;
            this.Texture = Game1.AllTextures.EnemySpriteSheet;


            this.NextPointRectangle = new Rectangle(0, 0, 16, 16);
            this.NextPointRectangleTexture = SetRectangleTexture(graphics, this.NextPointRectangle);
            this.DebugColor = Color.Red;
           
            this.CurrentBehaviour = CurrentBehaviour.Wander;
            this.PrimaryPlayerInterationBehavior = CurrentBehaviour.Chase;

            this.ObstacleGrid = TileManager.PathGrid;
            this.Navigator = new Navigator("blank", ObstacleGrid.Weight);
            this.CurrentBehaviour = CurrentBehaviour.Wander;

            this.DamageImmunityTimer = new SimpleTimer(.5f);
            LoadTextures(graphics);

   
        }

        protected virtual void LoadTextures(GraphicsDevice graphics)
        {
            this.NPCHitBoxRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.NPCAnimatedSprite[0].Width, this.NPCAnimatedSprite[0].Height);
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
        }

        public virtual void CreateBody()
        {
            this.CollisionBody = BodyFactory.CreateCircle(Game1.VelcroWorld, 6, 0f, this.Position);
            CollisionBody.BodyType = BodyType.Dynamic;
            CollisionBody.Restitution = 0f;
            CollisionBody.Friction = .4f;
            CollisionBody.Mass = 1f;
            CollisionBody.Inertia = 0;
            CollisionBody.SleepingAllowed = true;
            CollisionBody.CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Enemy;
            CollisionBody.CollidesWith = VelcroPhysics.Collision.Filtering.Category.Solid | VelcroPhysics.Collision.Filtering.Category.Player | VelcroPhysics.Collision.Filtering.Category.TransparencySensor;

            CollisionBody.IgnoreGravity = true;
            //CollisionBody.OnCollision += OnCollision;
        }



        /// <summary>
        /// If enemy hitpoints are 0 or less, drop items and remove from game.
        /// </summary>
        /// <param name="enemies"></param>
        /// <returns></returns>
        protected virtual bool TestDeath(List<Enemy> enemies)
        {
            if (this.HitPoints <= 0)
            {
                RollPeriodicDrop(this.Position);
                enemies.Remove(this);
                return true;
            }
            return false;
        }


        public virtual void Update(GameTime gameTime, MouseManager mouse, Rectangle cameraRectangle, List<Enemy> enemies = null)
        {
            this.DoesIntersectScreen = this.NPCHitBoxRectangle.Intersects(cameraRectangle);
            Navigator.HasReachedNextPoint = false;
            this.IsMoving = true;
            oldPrimaryVelocity = primaryVelocity;
            if (DoesIntersectScreen)
            {
                if (TestDeath(enemies))
                    return;

                TestImmunity(gameTime);
                primaryVelocity = new Vector2(.5f, .5f);


                if (mouse.WorldMouseRectangle.Intersects(this.NPCHitBoxRectangle))
                    mouse.ChangeMouseTexture(CursorType.Normal);



                if (this.IsMoving)
                    UpdateAnimations(gameTime);
                else
                    SetToIdleAnimations();



                CheckIfSoundIsMade();
                
                for (int i = 0; i < this.NPCAnimatedSprite.Length; i++)
                {
                    this.NPCAnimatedSprite[i].UpdateAnimationPosition(this.Position);
                }
            }
             
            if (this.IsMoving)
            {
                UpdateBehaviours(gameTime);

                this.Position += this.primaryVelocity * DirectionVector;
                //if(Navigator.HasReachedNextPoint)
                //    Navigator.CurrentPath.RemoveAt(Navigator.CurrentPath.Count - 1);


            }
            else
            {
                SetToIdleAnimations();
            }
        }


        protected virtual void CheckIfSoundIsMade()
        {
            if (this.MakesPeriodicSound)
            {
                if (this.IdleSoundEffect != null)
                {
                    if ((Game1.GlobalClock.SecondsPassedToday - this.SoundTimer) > this.SoundUpperBound)
                    {
                        Game1.SoundManager.PlaySoundEffect(this.IdleSoundEffect, true, 1f);
                        this.SoundTimer = Game1.GlobalClock.SecondsPassedToday;
                    }
                }
            }
        }

        private void SetToIdleAnimations()
        {
            for (int i = 0; i < this.NPCAnimatedSprite.Length; i++)
            {
                this.NPCAnimatedSprite[i].SetFrame(0);
            }
        }

        private void UpdateAnimations(GameTime gameTime)
        {
            for (int i = 0; i < this.NPCAnimatedSprite.Length; i++)
            {
                this.NPCAnimatedSprite[i].UpdateAnimations(gameTime, this.Position);
            }
        }

        /// <summary>
        /// Changes based on the four states of the enemy: Wander, chase, hurt, or flee.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void UpdateBehaviours(GameTime gameTime)
        {
            switch (this.CurrentBehaviour)
            {
                case CurrentBehaviour.Wander:
                    Navigator.Wander(gameTime,ref this.Position, ref CurrentDirection);
                    this.DirectionVector = Navigator.DirectionVector;
                    this.NPCHitBoxRectangle = new Rectangle((int)this.Position.X - this.NPCAnimatedSprite[(int)CurrentDirection].Width / 2,
                        (int)this.Position.Y - this.NPCAnimatedSprite[(int)CurrentDirection].Height / 2,
                        this.NPCAnimatedSprite[(int)CurrentDirection].Width, this.NPCAnimatedSprite[(int)CurrentDirection].Height);
                    break;
                case CurrentBehaviour.Chase:
                    //   MoveTowardsPoint(new Vector2(Game1.Player.MainCollider.Rectangle.X, Game1.Player.MainCollider.Rectangle.Y), gameTime);
                    //int currentTileX = (int)(this.Position.X / 16 - (this.CurrentChunkX * 16));
                    //int currentTileY = (int)(this.Position.Y / 16 - (this.CurrentChunkY * 16));
                    //int newTargetX = (int)Game1.Player.WorldSquarePosition.X;
                    //int newTargetY = (int)Game1.Player.WorldSquarePosition.Y;

                    //MoveTowardsTarget(gameTime, newTargetX, newTargetY);
                    break;
                case CurrentBehaviour.Hurt:
                    this.CurrentBehaviour = CurrentBehaviour.Chase;


                    break;

                case CurrentBehaviour.Flee:
                    Navigator.MoveAwayFromPoint(Game1.Player.Position,this.Speed, ref this.Position, gameTime);
                    break;
            }
        }


        public void RollPeriodicDrop(Vector2 positionToDrop)
        {
            for (int i = 0; i < this.PossibleLoot.Count; i++)
            {
                if (this.PossibleLoot[i].DidReceive())
                {
                    Game1.ItemVault.GenerateNewItem(this.PossibleLoot[i].ID, positionToDrop, true, Game1.CurrentStage.AllTiles.GetItems(positionToDrop));
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




        public virtual void DamageCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom)
        {
            if (!this.IsImmuneToDamage)
            {



                Game1.CurrentStage.ParticleEngine.ActivationTime = .25f;
                Game1.CurrentStage.ParticleEngine.EmitterLocation = this.Position;
                Game1.CurrentStage.ParticleEngine.Color = this.DamageColor;


                this.CurrentBehaviour = CurrentBehaviour.Hurt;
                switch (directionAttackedFrom)
                {
                    case Dir.Down:
                        this.Position = new Vector2(this.Position.X, this.Position.Y + knockBack);
                        break;
                    case Dir.Right:
                        this.Position = new Vector2(this.Position.X + knockBack, this.Position.Y);
                        break;
                    case Dir.Left:
                        this.Position = new Vector2(this.Position.X - knockBack, this.Position.Y);
                        break;
                    case Dir.Up:
                        this.Position = new Vector2(this.Position.X, this.Position.Y - knockBack);
                        break;
                    default:
                        this.Position = new Vector2(this.Position.X, this.Position.Y - knockBack);
                        break;
                }

                TakeDamage(dmgAmount);
            }
            else
            {

            }
        }

        public virtual void TakeDamage(int dmgAmount)
        {
            this.HitPoints -= dmgAmount;

            this.IsImmuneToDamage = true;
            if (this.HasPackAggression)
            {


                if (this.Pack != null)
                {
                    for (int i = 0; i < this.Pack.Count; i++)
                    {
                        Pack[i].CurrentBehaviour = CurrentBehaviour.Chase;
                    }
                }
            }
            Navigator.CurrentPath.Clear();
            if (this.IdleSoundEffect != null)
            {
                Game1.SoundManager.PlaySoundEffect(this.IdleSoundEffect, true, 1f, .8f);
            }

            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.SwordImpact, true, .5f);
            Game1.CurrentStage.AllRisingText.Add(new RisingText(new Vector2(this.NPCHitBoxRectangle.X, this.NPCHitBoxRectangle.Y), 25, "-" + dmgAmount.ToString(), 75f, Color.LightYellow, true, 1f, false));
        }

        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            if (this.DoesIntersectScreen)
            {
                Rectangle sourceRectangle = this.NPCAnimatedSprite[(int)this.CurrentDirection].SourceRectangle;

                this.NPCAnimatedSprite[(int)this.CurrentDirection].DrawAnimation(spriteBatch, new Vector2(this.Position.X - sourceRectangle.Width/3.5f, this.Position.Y - sourceRectangle.Height/1.5f),
                    .5f + (Position.Y + sourceRectangle.Height) * Utility.ForeGroundMultiplier);
            }
        }
        public Texture2D SetRectangleTexture(GraphicsDevice graphicsDevice, Rectangle rectangleToDraw)
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
                        Colors.Add(new Color(255, 0, 0, 255));
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
            spriteBatch.Draw(this.HitBoxTexture, new Vector2(this.NPCHitBoxRectangle.X, this.NPCHitBoxRectangle.Y), color: Color.Green, layerDepth: 1f);
            //spriteBatch.Draw(this.NextPointRectangleTexture, new Vector2(this.NextPointRectangle.X + 8, this.NextPointRectangle.Y + 8), color: Color.White, layerDepth: layerDepth);

            if (Navigator.CurrentPath != null)
            {
                for (int i = 0; i < Navigator.CurrentPath.Count - 1; i++)
                {
                    Game1.Utility.DrawLine(Game1.LineTexture, spriteBatch, new Vector2(Navigator.CurrentPath[i].X * 16 + 8, Navigator.CurrentPath[i].Y * 16
                         + 8), new Vector2(Navigator.CurrentPath[i + 1].X * 16 + 8, Navigator.CurrentPath[i + 1].Y * 16 + 8), this.DebugColor);
                }
            }

        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void MouseCollisionInteraction()
        {
            throw new NotImplementedException();
        }

        
    }
}
