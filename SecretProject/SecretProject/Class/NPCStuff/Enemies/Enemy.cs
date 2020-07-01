using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.PathFinding;
using SecretProject.Class.PathFinding.PathFinder;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.UI;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;

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



    public class Enemy : INPC, IEntity
    {

        protected GraphicsDevice Graphics { get; set; }
        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public Sprite[] NPCAnimatedSprite { get; set; }

        protected int NPCRectangleXOffSet { get; set; }
        protected int NPCRectangleYOffSet { get; set; }
        protected int NPCRectangleWidthOffSet { get; set; } = 1;
        protected int NPCRectangleHeightOffSet { get; set; } = 1;
        public Rectangle NPCHitBoxRectangle { get { return new Rectangle((int)this.Position.X - this.NPCRectangleXOffSet, (int)this.Position.Y - this.NPCRectangleYOffSet, this.NPCRectangleWidthOffSet, this.NPCRectangleHeightOffSet); } }
        public Rectangle NPCPathFindRectangle
        {
            get
            {
                return new Rectangle(this.NPCAnimatedSprite[0].DestinationRectangle.X + 16,
this.NPCAnimatedSprite[0].DestinationRectangle.Y + 20, 8, 8);
            }
            set { }
        }
        public Texture2D Texture { get; set; }
        public Texture2D HitBoxTexture { get; set; }

        public float Speed { get; set; }
        //0 = down, 1 = left, 2 =  right, 3 = up
        public Dir CurrentDirection { get; set; }
        public bool IsMoving { get; set; }
        protected Vector2 primaryVelocity;
        public Vector2 PrimaryVelocity { get { return primaryVelocity; } set { primaryVelocity = value; } }
        public Vector2 TotalVelocity { get; set; }
        public Vector2 DirectionVector { get; set; }
        public bool IsUpdating { get; set; }
        public int FrameNumber { get; set; }
        public RectangleCollider Collider { get; set; }
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
        private List<PathFinderNode> CurrentPath { get; set; }

        public CurrentBehaviour CurrentBehaviour { get; set; }
        public CurrentBehaviour PrimaryPlayerInterationBehavior { get; set; }

        public bool DoesIntersectScreen { get; set; }


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


        private float WanderTimer = 2f;
        public Enemy(List<Enemy> pack, Vector2 position, GraphicsDevice graphics, IInformationContainer container)
        {
            this.Pack = pack;
            this.Position = position;
            this.Graphics = graphics;
            this.Texture = Game1.AllTextures.EnemySpriteSheet;
            this.Collider = new RectangleCollider(graphics, this.NPCHitBoxRectangle, this, ColliderType.Enemy);

            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);

            this.NextPointRectangle = new Rectangle(0, 0, 16, 16);
            this.NextPointRectangleTexture = SetRectangleTexture(graphics, this.NextPointRectangle);
            this.DebugColor = Color.Red;
            this.CurrentPath = new List<PathFinderNode>();
            this.CurrentBehaviour = CurrentBehaviour.Wander;
            this.PrimaryPlayerInterationBehavior = CurrentBehaviour.Chase;

            this.ObstacleGrid = container.PathGrid;
            this.CurrentBehaviour = CurrentBehaviour.Wander;

            this.DamageImmunityTimer = new SimpleTimer(.5f);
        }

        public void UpdateCurrentChunk(IInformationContainer container)
        {

            this.ObstacleGrid = container.PathGrid;
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
            this.DoesIntersectScreen = this.NPCPathFindRectangle.Intersects(cameraRectangle);

            this.IsMoving = true;
            if (DoesIntersectScreen)
            {
                if (TestDeath(enemies))
                    return;

                TestImmunity(gameTime);
                this.PrimaryVelocity = new Vector2(.5f, .5f);


                UpdateDirection();

                if (mouse.WorldMouseRectangle.Intersects(this.NPCHitBoxRectangle))
                    mouse.ChangeMouseTexture(CursorType.Normal);



                if (this.IsMoving)
                    UpdateAnimations(gameTime);
                else
                    SetToIdleAnimations();



                CheckIfSoundIsMade();
                QuadTreeInsertion();
                for (int i = 0; i < this.NPCAnimatedSprite.Length; i++)
                {
                    this.NPCAnimatedSprite[i].UpdateAnimationPosition(this.Position);
                }
            }
             
            if (this.IsMoving)
            {
                UpdateBehaviours(gameTime);
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
                    Wander(gameTime);
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
                    MoveAwayFromPoint(Game1.Player.Position, gameTime);
                    break;
            }
        }

        public virtual void QuadTreeInsertion()
        {
            this.Collider.Rectangle = this.NPCHitBoxRectangle;
            List<ICollidable> returnObjects = new List<ICollidable>();
            Game1.CurrentStage.QuadTree.Retrieve(returnObjects, this.Collider);
            for (int i = 0; i < returnObjects.Count; i++)
            {
                //if obj collided with item in list stop it from moving boom badda bing

                //this.CollideOccured = true;
                if (returnObjects[i].ColliderType == ColliderType.PlayerBigBox)
                {
                    if (this.Collider.IsIntersecting(Game1.Player.MainCollider))
                    {
                        if (!Game1.Player.IsImmuneToDamage)
                        {
                            Game1.Player.DamageCollisionInteraction(1, 200, this.CurrentDirection);
                        }

                    }
                }
                else if (returnObjects[i].ColliderType == ColliderType.grass)
                {
                    if (this.Collider.IsIntersecting(returnObjects[i]))
                    {
                       (returnObjects[i].Entity as GrassTuft).IsUpdating = true;
                        (returnObjects[i].Entity as GrassTuft).InitialShuffDirection = this.CurrentDirection;
                    }
                }
                else
                {
                    if (this.IsMoving)
                    {


                        //if (returnObjects[i].ColliderType == ColliderType.inert)
                        //{
                        //    Vector2 oldVelocity = primaryVelocity;
                        //    if (Collider.HandleMove(Position, ref primaryVelocity, returnObjects[i]))
                        //    {
                        //        //if()
                        //    }
                        //}
                    }

                }


                // IsMoving = false;



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

        public bool MoveTowardsVector(Vector2 goal, GameTime gameTime)
        {
            // If we're already at the goal return immediatly
            this.IsMoving = true;
            if (this.Position == goal) return true;

            // Find direction from current position to goal
            Vector2 direction = Vector2.Normalize(goal - this.Position);
            this.DirectionVector = direction;
            // Move in that direction
            this.Position += direction * primaryVelocity;// * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If we moved PAST the goal, move it back to the goal
            if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - this.Position)) + 1) < 0.1f)
                this.Position = goal;

            // Return whether we've reached the goal or not
            return this.Position == goal;
        }
        protected void MoveAwayFromPoint(Vector2 positionToMoveAwayFrom, GameTime gameTime)
        {

            this.IsMoving = true;


            Vector2 direction = Vector2.Normalize(positionToMoveAwayFrom - this.Position);
            this.DirectionVector = new Vector2(direction.X * -1, direction.Y * -1);

            this.Position -= direction * this.Speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        }


        protected void MoveTowardsPointAndDecrementPath(GameTime gameTime)
        {
            if (MoveTowardsVector(new Vector2(this.CurrentPath[this.CurrentPath.Count - 1].X * 16, this.CurrentPath[this.CurrentPath.Count - 1].Y * 16), gameTime))
            {
                this.CurrentPath.RemoveAt(this.CurrentPath.Count - 1);
            }
        }


        protected void MoveTowardsTarget(GameTime gameTime, int targetX, int targetY)
        {
            if (this.CurrentPath.Count > 0)
                MoveTowardsPointAndDecrementPath(gameTime);
            else
                TryFindNewPath(new Point(targetX, targetY));

        }


        public Point NewStartPoint { get; set; } = new Point(-1, -1);

        private Point GetNewWanderPoint(int currentTileX, int currentTileY)
        {
            int newX = Game1.Utility.RGenerator.Next(-10, 10) + currentTileX;
            int newY = Game1.Utility.RGenerator.Next(-10, 10) + currentTileY;

            return new Point(newX, newY);
        }



        #region PATHFINDING


        

        /// <summary>
        /// If on current path, moves towards next point. Else if allowed to wander, finds a new path.
        /// </summary>
        /// <param name="gameTime"></param>
        protected void Wander(GameTime gameTime)
        {
            WanderTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.CurrentPath.Count > 0)
            {
                if (MoveTowardsVector(new Vector2(this.CurrentPath[this.CurrentPath.Count - 1].X * 16 + 8, this.CurrentPath[this.CurrentPath.Count - 1].Y * 16 + 8), gameTime))
                    this.CurrentPath.RemoveAt(this.CurrentPath.Count - 1);

            }
            else if (WanderTimer > 0)
            {
                this.IsMoving = false;
            }
            else if (WanderTimer <= 0)
            {

                int currentTileX = Utility.GetSquareTile(this.Position.X);
                int currentTileY = Utility.GetSquareTile(this.Position.Y);
                Point newWanderPoint = GetNewWanderPoint(currentTileX, currentTileY);

                if (newWanderPoint.X < Game1.CurrentStage.MapRectangle.Width / 16 - 2 && newWanderPoint.X > 0 &&
                    newWanderPoint.Y < Game1.CurrentStage.MapRectangle.Width / 16 - 2 && newWanderPoint.Y > 0)
                {
                    if (this.ObstacleGrid.Weight[newWanderPoint.X, newWanderPoint.Y] != 0)
                        TryFindNewPath(newWanderPoint);

                }

            }
        }

        protected bool TryFindNewPath(Point endPoint)
        {
            PathFinderFast finder = new PathFinderFast(this.ObstacleGrid.Weight);


            Point start = new Point(Math.Abs((int)this.Position.X / 16),
             (Math.Abs((int)this.Position.Y / 16)));

            this.CurrentPath = finder.FindPath(start, endPoint, this.Name);
            if (this.CurrentPath == null)
            {
                this.CurrentPath = new List<PathFinderNode>();
                return false;
                throw new Exception(this.Name + " was unable to find a path between " + start + " and " + endPoint);
            }
            WanderTimer = Game1.Utility.RGenerator.Next(3, 5);
            return true;


        }
        #endregion



        public void UpdateDirection()
        {
            if (this.DirectionVector.X > .5f)
            {
                this.CurrentDirection = Dir.Right; //right
            }
            else if (this.DirectionVector.X < -.5f)
            {
                this.CurrentDirection = Dir.Left; //left
            }
            else if (this.DirectionVector.Y < .5f) // up
            {
                this.CurrentDirection = Dir.Up;
            }

            else if (this.DirectionVector.Y > .5f)
            {
                this.CurrentDirection = Dir.Down;
            }
            else
            {
                this.CurrentDirection = Dir.Down;
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
            this.CurrentPath.Clear();
            if (this.IdleSoundEffect != null)
            {
                Game1.SoundManager.PlaySoundEffect(this.IdleSoundEffect, true, 1f, .8f);
            }

            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.SwordImpact, true, .5f);
            Game1.CurrentStage.AllRisingText.Add(new RisingText(new Vector2(this.NPCHitBoxRectangle.X, this.NPCHitBoxRectangle.Y), 25, "-" + dmgAmount.ToString(), 75f, Color.LightYellow, true, 1f, false));
        }

        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics, ref Effect effect)
        {
            if (this.DoesIntersectScreen)
            {


                if (this.CurrentBehaviour == CurrentBehaviour.Hurt)
                {

                    //Game1.AllTextures.Pulse.Parameters["SINLOC"].SetValue((float)Math.Sin((float)this.PulseTimer.Time * 2 / this.PulseTimer.TargetTime + (float)Math.PI / 2 * ((float)(Math.PI * 3))));
                    //Game1.AllTextures.Pulse.Parameters["filterColor"].SetValue(Color.Red.ToVector4());
                    //Game1.AllTextures.Pulse.CurrentTechnique.Passes[0].Apply();



                }


                this.NPCAnimatedSprite[(int)this.CurrentDirection].DrawAnimation(spriteBatch, new Vector2(this.Position.X - this.NPCRectangleXOffSet - 8, this.Position.Y - this.NPCRectangleYOffSet - 8), .5f + (Utility.ForeGroundMultiplier * ((float)this.NPCAnimatedSprite[(int)this.CurrentDirection].DestinationRectangle.Y)));

            }
        }
        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            if (this.DoesIntersectScreen)
            {


                this.NPCAnimatedSprite[(int)this.CurrentDirection].DrawAnimation(spriteBatch, new Vector2(this.Position.X - this.NPCRectangleXOffSet - 8, this.Position.Y - this.NPCRectangleYOffSet - 8), .5f + (Utility.ForeGroundMultiplier * ((float)this.NPCAnimatedSprite[(int)this.CurrentDirection].DestinationRectangle.Y)));
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
            spriteBatch.Draw(this.NextPointRectangleTexture, new Vector2(this.NextPointRectangle.X + 8, this.NextPointRectangle.Y + 8), color: Color.White, layerDepth: layerDepth);

            if (this.CurrentPath != null)
            {
                for (int i = 0; i < this.CurrentPath.Count - 1; i++)
                {
                    Game1.Utility.DrawLine(Game1.LineTexture, spriteBatch, new Vector2(this.CurrentPath[i].X * 16 + 8, this.CurrentPath[i].Y * 16
                         + 8), new Vector2(this.CurrentPath[i + 1].X * 16 + 8, this.CurrentPath[i + 1].Y * 16 + 8), this.DebugColor);
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
