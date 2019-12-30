using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.PathFinding;
using SecretProject.Class.PathFinding.PathFinder;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;
using XMLData.RouteStuff;

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
        Flee = 4
    }

  

    public class Enemy : INPC
    {


        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public Sprite[] NPCAnimatedSprite { get; set; }

        public int NPCRectangleXOffSet { get; set; }
        public int NPCRectangleYOffSet { get; set; }
        public int NPCRectangleWidthOffSet { get; set; } = 1;
        public int NPCRectangleHeightOffSet { get; set; } = 1;
        public Rectangle NPCHitBoxRectangle { get { return new Rectangle((int)Position.X - NPCRectangleXOffSet, (int)Position.Y - NPCRectangleYOffSet, NPCRectangleWidthOffSet, NPCRectangleHeightOffSet); } }
        public Rectangle NPCPathFindRectangle
        {
            get
            {
                return new Rectangle(NPCAnimatedSprite[0].DestinationRectangle.X + 16,
NPCAnimatedSprite[0].DestinationRectangle.Y + 20, 8, 8);
            }
            set { }
        }
        public Texture2D Texture { get; set; }
        public Texture2D DebugTexture { get; set; }

        public float Speed { get; set; }
        //0 = down, 1 = left, 2 =  right, 3 = up
        public Dir CurrentDirection { get; set; }
        public bool IsMoving { get; set; }
        private Vector2 primaryVelocity;
        public Vector2 PrimaryVelocity { get { return primaryVelocity; } set { primaryVelocity = value; } }
        public Vector2 TotalVelocity { get; set; }
        public Vector2 DirectionVector { get; set; }
        public bool IsUpdating { get; set; }
        public int FrameNumber { get; set; }
        public Collider Collider { get; set; }
        public bool CollideOccured { get; set; }

        public Vector2 DebugNextPoint { get; set; } = new Vector2(1, 1);

        public Texture2D NextPointTexture { get; set; }
        public Rectangle NextPointRectangle { get; set; }
        public Texture2D NextPointRectangleTexture { get; set; }
        //public int CurrentTileX { get { return (int)(this.Position.X / 16); } }
        // public int CurrentTileY { get { return (int)(this.Position.Y / 16); } }
        public float SoundTimer { get; set; }
        public float SoundMin { get; set; }
        public float SoundMax { get; set; }
        public int SoundID { get; set; }

        public Color DebugColor { get; set; }
        public List<PathFinderNode> CurrentPath { get; set; }

        public CurrentBehaviour CurrentBehaviour { get; set; }
        public CurrentBehaviour PrimaryPlayerInterationBehavior { get; set; }


        //DAMAGE RELATED THINGS
        public SimpleTimer PulseTimer { get; set; }
        public Effect CurrentEffect { get; set; }

        public int HitPoints { get; protected set; }
        public Color DamageColor { get; protected set; }

        public List<Loot> PossibleLoot { get; set; }

        //TODO
        public int CurrentChunkX { get; set; }
        public int CurrentChunkY { get; set; }
        public ObstacleGrid ObstacleGrid { get; set; }
        public float TimeInUnloadedChunk { get; set; }

        public bool IsWorldNPC { get; set; }
        public EmoticonType CurrentEmoticon { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Enemy(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, IInformationContainer container, CurrentBehaviour primaryPlayerInteractionBehavior)
        {
            this.Name = name;
            this.Position = position;
            this.Texture = spriteSheet;
            Collider = new Collider(graphics, this.NPCHitBoxRectangle, this, ColliderType.Enemy);

            this.DebugTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);

            NextPointRectangle = new Rectangle(0, 0, 16, 16);
            NextPointRectangleTexture = SetRectangleTexture(graphics, NextPointRectangle);
            this.DebugColor = Color.Red;
            this.CurrentPath = new List<PathFinderNode>();
            this.CurrentBehaviour = CurrentBehaviour.Wander;
            this.PrimaryPlayerInterationBehavior = primaryPlayerInteractionBehavior;

            this.PulseTimer = new SimpleTimer(.25f);
            TimeInUnloadedChunk = 0f;
            this.CurrentChunkX = container.X;
            this.CurrentChunkY = container.Y;
            this.ObstacleGrid = container.PathGrid;
            this.CurrentBehaviour = CurrentBehaviour.Wander;
            this.CurrentEffect = null;


            IsWorldNPC = true;
        }

        public static Enemy GetEnemyFromType(EnemyType enemyType, Vector2 position, GraphicsDevice graphics, IInformationContainer container, bool isWorldNPC = false)
        {
            switch (enemyType)
            {
                case EnemyType.Boar:
                    return new Boar("Boar", position, graphics, Game1.AllTextures.EnemySpriteSheet, container, CurrentBehaviour.Wander) { IsWorldNPC = isWorldNPC };

                case EnemyType.Crab:
                    return new Crab("Crab", position, graphics, Game1.AllTextures.EnemySpriteSheet, container, CurrentBehaviour.Wander) { IsWorldNPC = isWorldNPC };

                case EnemyType.Rabbit:
                    return new Rabbit("Rabbit", position, graphics, Game1.AllTextures.EnemySpriteSheet, container, CurrentBehaviour.Wander) { IsWorldNPC = isWorldNPC };

                case EnemyType.Butterfly:
                    return new Butterfly("Butterfly", position, graphics, Game1.AllTextures.EnemySpriteSheet, container, CurrentBehaviour.Wander) { IsWorldNPC = isWorldNPC };
                default:
                    return null;
            }

        }

        public void UpdateCurrentChunk(IInformationContainer container)
        {
            this.CurrentChunkX = container.X;
            this.CurrentChunkY = container.Y;
            this.ObstacleGrid = container.PathGrid;
        }

        public virtual void Update(GameTime gameTime, MouseManager mouse, List<Enemy> enemies = null)
        {
            this.CurrentEffect = null;
            if (this.TimeInUnloadedChunk > 100)
            {
                enemies.Remove(this);
                return;
            }
            if (this.HitPoints <= 0)
            {
                RollPeriodicDrop(this.Position);
                enemies.Remove(this);
                return;
            }
            //this.CurrentBehaviour = CurrentBehaviour.Wander;
            this.IsMoving = true;
            this.PrimaryVelocity = new Vector2(1, 1);
            Collider.Rectangle = new Rectangle((int)(this.Position.X + NPCRectangleXOffSet / 2), (int)(this.Position.Y + NPCRectangleYOffSet / 2), (int)(NPCRectangleWidthOffSet * 2), (int)(NPCRectangleHeightOffSet * 2));
            List<ICollidable> returnObjects = new List<ICollidable>();
            Game1.GetCurrentStage().QuadTree.Retrieve(returnObjects, Collider);
            for (int i = 0; i < returnObjects.Count; i++)
            {
                //if obj collided with item in list stop it from moving boom badda bing

                CollideOccured = true;
                if (returnObjects[i].ColliderType == ColliderType.PlayerBigBox)
                {
                    //  this.CurrentBehaviour = PrimaryPlayerInterationBehavior;
                }
                else if (returnObjects[i].ColliderType == ColliderType.grass)
                {
                    if (Collider.IsIntersecting(returnObjects[i]))
                    {
                        returnObjects[i].IsUpdating = true;
                        returnObjects[i].InitialShuffDirection = this.CurrentDirection;
                    }
                }
                //else
                //{
                //    Collider.HandleMove(Position, ref primaryVelocity, returnObjects[i]);
                //}


                // IsMoving = false;



            }
            for (int i = 0; i < NPCAnimatedSprite.Length; i++)
            {
                NPCAnimatedSprite[i].UpdateAnimationPosition(Position);
            }
            UpdateDirection();

            if (mouse.WorldMouseRectangle.Intersects(this.NPCHitBoxRectangle))
            {
                mouse.ChangeMouseTexture(CursorType.Normal);
                mouse.ToggleGeneralInteraction = true;
                Game1.isMyMouseVisible = false;

            }

            if (IsMoving)
            {
                switch (CurrentBehaviour)
                {
                    case CurrentBehaviour.Wander:
                        Wander(gameTime);
                        break;
                    case CurrentBehaviour.Chase:
                        MoveTowardsPoint(new Vector2(Game1.Player.MainCollider.Rectangle.X, Game1.Player.MainCollider.Rectangle.Y), gameTime);
                        break;
                    case CurrentBehaviour.Hurt:
                        this.CurrentEffect = Game1.AllTextures.Pulse;
                        if (PulseTimer.Run(gameTime))
                        {
                            this.HitPoints--;
                            CurrentBehaviour = CurrentBehaviour.Chase;
                        }

                        break;

                    case CurrentBehaviour.Flee:
                        MoveAwayFromPoint(Game1.Player.Position, gameTime);
                        break;
                }


            }


            if (IsMoving)
            {

                for (int i = 0; i < NPCAnimatedSprite.Length; i++)
                {
                    NPCAnimatedSprite[i].UpdateAnimations(gameTime, Position);
                }



            }
            else
            {
                for (int i = 0; i < NPCAnimatedSprite.Length; i++)
                {
                    this.NPCAnimatedSprite[i].SetFrame(0);
                }


            }
            if(SoundID != 0)
            {
                SoundTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (SoundTimer <= 0)
                {
                    Game1.SoundManager.PlaySoundEffectFromInt(1, SoundID);
                    SoundTimer = Game1.Utility.RFloat(45f, 100f);

                    RollPeriodicDrop(this.Position);
                }
            }
           


        }

        public void RollPeriodicDrop(Vector2 positionToDrop)
        {
            for (int i = 0; i < this.PossibleLoot.Count; i++)
            {
                if (PossibleLoot[i].DidReceive())
                {
                    Game1.GetCurrentStage().AllItems.Add(Game1.ItemVault.GenerateNewItem(PossibleLoot[i].ID, positionToDrop, true));
                }
            }

        }


        public bool MoveTowardsPoint(Vector2 goal, GameTime gameTime)
        {
            // If we're already at the goal return immediatly
            this.IsMoving = true;
            if (Position == goal) return true;

            // Find direction from current position to goal
            Vector2 direction = Vector2.Normalize(goal - Position);
            this.DirectionVector = direction;
            // Move in that direction
            Position += direction * primaryVelocity;// * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If we moved PAST the goal, move it back to the goal
            if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - Position)) + 1) < 0.1f)
                Position = goal;

            // Return whether we've reached the goal or not
            return Position == goal;
        }
        public void MoveAwayFromPoint(Vector2 positionToMoveAwayFrom, GameTime gameTime)
        {

            this.IsMoving = true;


            Vector2 direction = Vector2.Normalize(positionToMoveAwayFrom - Position);
            this.DirectionVector = new Vector2(direction.X * -1, direction.Y * -1);

            Position -= direction * Speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        }

        public void MoveToTileChunk(GameTime gameTime)
        {

            if (CurrentPath.Count > 0)
            {
                if (MoveTowardsPoint(new Vector2(CurrentPath[CurrentPath.Count - 1].X * 16 + CurrentChunkX * 16 * 16 + 8, CurrentPath[CurrentPath.Count - 1].Y * 16 + CurrentChunkY * 16 * 16 + 8), gameTime))
                {
                    CurrentPath.RemoveAt(CurrentPath.Count - 1);
                }



            }
            else if (NeedsToMoveToNewChunk)
            {
                UpdateCurrentChunk(NewChunk);
                if (MoveTowardsPoint(new Vector2(NewStartX * 16 + CurrentChunkX * 16 * 16 + 8, NewStartY * 16 + CurrentChunkY * 16 * 16 + 8), gameTime))
                {


                    // this.Position = new Vector2(NewStartX * 16 + CurrentChunkX * 16 * 16 + 8, NewStartY * 16 + CurrentChunkY * 16 * 16 + 8);
                    TryFindNewPath(OutOfChunkDestinationX, OutOfChunkDestinationY);

                    NeedsToMoveToNewChunk = false;
                }
            }
            else if (WanderTimer >= 0)
            {
                IsMoving = false;
            }
            else if (WanderTimer <= 0)
            {

                int currentTileX = (int)(this.Position.X / 16 - (CurrentChunkX * 16));
                int currentTileY = (int)(this.Position.Y / 16 - (CurrentChunkY * 16));
                int newX = Game1.Utility.RGenerator.Next(-10, 10);
                int newY = Game1.Utility.RGenerator.Next(-10, 10);
                if (currentTileX + newX < TileUtility.ChunkWidth && currentTileX + newX > 0 && currentTileY + newY < TileUtility.ChunkHeight && currentTileY + newY > 0)
                {
                    TryFindNewPath(currentTileX + newX, currentTileY + newY);

                }
                // point npc tried to go to is not in current chunk
                else
                {
                    //basically makes sure tile found is in adjacent and NOT diagonal chunk
                    if (!((currentTileX + newX > 15) && (currentTileX + newY > 15)) && !((currentTileX + newX < 0) && (currentTileY + newY < 0)))
                    {
                        Chunk chunk = TileUtility.GetChunk(CurrentChunkX * 16 + (currentTileX + newX), CurrentChunkY * 16 + (currentTileY + newY), Game1.OverWorld.AllTiles.ActiveChunks);
                        if (chunk != null)
                        {
                            int startX = currentTileX + newX;
                            int startY = currentTileY + newY;
                            NewStartX = startX;
                            NewStartY = startY;
                            OutOfChunkDestinationX = startX;
                            OutOfChunkDestinationY = startY;
                            if (startX > 15)
                            {
                                startX = 15;
                                OutOfChunkDestinationX = currentTileX + newX - 16;
                                NewStartX = 0;
                            }
                            if (startX < 0)
                            {
                                startX = 0;
                                OutOfChunkDestinationX = 16 - Math.Abs(currentTileX + newX);
                                NewStartX = 15;
                            }
                            if (startY > 15)
                            {
                                startY = 15;
                                OutOfChunkDestinationY = currentTileY + newY - 16;
                                NewStartY = 0;
                            }
                            if (startY < 0)
                            {
                                startY = 0;
                                OutOfChunkDestinationY = 16 - Math.Abs(currentTileY + newY);
                                NewStartY = 15;
                            }
                            if (chunk.PathGrid.Weight[NewStartX, NewStartY] == 1)
                            {
                                if (TryFindNewPath(startX, startY))
                                {
                                    NewChunk = chunk;
                                    NeedsToMoveToNewChunk = true;
                                }

                            }



                            //UpdateCurrentChunk(TileUtility.GetChunk(currentTileX + newX, currentTileY + newY, Game1.OverWorld.AllTiles.ActiveChunks));
                            Console.WriteLine("hi");

                        }


                    }


                }


            }
        }
        public int NewStartX { get; set; } = -1;
        public int NewStartY { get; set; } = -1;
        public int OutOfChunkDestinationX { get; set; } = -1;
        public int OutOfChunkDestinationY { get; set; } = -1;
        public bool NeedsToMoveToNewChunk { get; set; }
        public IInformationContainer NewChunk { get; set; }


        public bool TryFindNewPath(int endX, int endY)
        {
            if (ObstacleGrid.Weight[endX, endY] != 0)
            {
                Point end = new Point(endX, endY);



                PathFinderFast finder = new PathFinderFast(ObstacleGrid.Weight);


                Point start = new Point(Math.Abs((int)this.Position.X / 16 - CurrentChunkX * 16),
                 (Math.Abs((int)this.Position.Y / 16 - CurrentChunkY * 16)));
                if (start.X > 15)
                {
                    start.X = 15;
                }
                if (start.Y > 15)
                {
                    start.Y = 15;
                }
                CurrentPath = finder.FindPath(start, end);
                if (CurrentPath == null)
                {
                    CurrentPath = new List<PathFinderNode>();
                    return false;
                    throw new Exception(this.Name + " was unable to find a path between " + start + " and " + end);
                }
                WanderTimer = Game1.Utility.RGenerator.Next(3, 5);
                return true;
            }
            return false;
        }

        public void MoveToTile(GameTime gameTime)
        {

            if (CurrentPath.Count > 0)
            {
                if (MoveTowardsPoint(new Vector2(CurrentPath[CurrentPath.Count - 1].X * 16 + CurrentChunkX * 16 * 16 + 8, CurrentPath[CurrentPath.Count - 1].Y * 16 + CurrentChunkY * 16 * 16 + 8), gameTime))
                {
                    CurrentPath.RemoveAt(CurrentPath.Count - 1);
                }



            }
            else if (WanderTimer >= 0)
            {
                IsMoving = false;
            }
            else if (WanderTimer <= 0)
            {

                int currentTileX = (int)(this.Position.X / 16 - (CurrentChunkX * 16));
                int currentTileY = (int)(this.Position.Y / 16 - (CurrentChunkY * 16));
                int newX = Game1.Utility.RGenerator.Next(-10, 10);
                int newY = Game1.Utility.RGenerator.Next(-10, 10);
                if (currentTileX + newX < Game1.GetCurrentStage().MapRectangle.Width/16 - 2 && currentTileX + newX > 0 && currentTileY + newY < Game1.GetCurrentStage().MapRectangle.Width/16 - 2 && currentTileY + newY > 0)
                {
                    if (ObstacleGrid.Weight[currentTileX + newX, currentTileY + newY] != 0)
                    {
                        Point end = new Point(currentTileX + newX, currentTileY + newY);



                        PathFinderFast finder = new PathFinderFast(ObstacleGrid.Weight);


                        Point start = new Point(Math.Abs((int)this.Position.X / 16 - CurrentChunkX * 16),
                         (Math.Abs((int)this.Position.Y / 16 - CurrentChunkY * 16)));

                        CurrentPath = finder.FindPath(start, end);
                        if (CurrentPath == null)
                        {
                            CurrentPath = new List<PathFinderNode>();
                            return;
                            throw new Exception(this.Name + " was unable to find a path between " + start + " and " + end);
                        }
                        WanderTimer = Game1.Utility.RGenerator.Next(3, 5);
                    }
                }


            }
        }


        private float WanderTimer = 2f;

        public void Wander(GameTime gameTime)
        {

            WanderTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (IsWorldNPC)
            {
                MoveToTileChunk(gameTime);
            }
            else
            {
                MoveToTile(gameTime);
            }

        }
        public void UpdateDirection()
        {
            if (DirectionVector.X > .5f)
            {
                CurrentDirection = Dir.Right; //right
            }
            else if (DirectionVector.X < -.5f)
            {
                CurrentDirection = Dir.Left; //left
            }
            else if (DirectionVector.Y < .5f) // up
            {
                CurrentDirection = Dir.Up;
            }

            else if (DirectionVector.Y > .5f)
            {
                CurrentDirection = Dir.Down;
            }
            else
            {
                CurrentDirection = Dir.Down;
            }
        }

        public void PlayerCollisionInteraction()
        {
            int amount = 10;
            Game1.GetCurrentStage().ParticleEngine.ActivationTime = .25f;
            Game1.GetCurrentStage().ParticleEngine.EmitterLocation = this.Position;
            Game1.GetCurrentStage().ParticleEngine.Color = this.DamageColor;


            this.CurrentBehaviour = CurrentBehaviour.Hurt;
            switch (Game1.Player.controls.Direction)
            {
                case Dir.Down:
                    this.Position = new Vector2(this.Position.X, this.Position.Y + amount);
                    break;
                case Dir.Right:
                    this.Position = new Vector2(this.Position.X + amount, this.Position.Y);
                    break;
                case Dir.Left:
                    this.Position = new Vector2(this.Position.X - amount, this.Position.Y);
                    break;
                case Dir.Up:
                    this.Position = new Vector2(this.Position.X, this.Position.Y - amount);
                    break;
                default:
                    this.Position = new Vector2(this.Position.X, this.Position.Y - amount);
                    break;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics, ref Effect effect)
        {
            if (CurrentBehaviour == CurrentBehaviour.Hurt)
            {

                Game1.AllTextures.Pulse.Parameters["SINLOC"].SetValue((float)Math.Sin((float)PulseTimer.Time * 2 / PulseTimer.TargetTime + (float)Math.PI / 2 * ((float)(Math.PI * 3))));
                Game1.AllTextures.Pulse.Parameters["filterColor"].SetValue(Color.Red.ToVector4());
                Game1.AllTextures.Pulse.CurrentTechnique.Passes[0].Apply();



            }

            if (this.CurrentEffect != effect)
            {
                spriteBatch.End();
                effect = CurrentEffect;
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Game1.GetCurrentStage().Cam.getTransformation(graphics), effect: this.CurrentEffect, depthStencilState: DepthStencilState.Default);

            }
            NPCAnimatedSprite[(int)CurrentDirection].DrawAnimation(spriteBatch, new Vector2(Position.X - NPCRectangleXOffSet - 8, Position.Y - NPCRectangleYOffSet - 8), .5f + (Game1.Utility.ForeGroundMultiplier * ((float)NPCAnimatedSprite[(int)CurrentDirection].DestinationRectangle.Y)));


        }
        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            NPCAnimatedSprite[(int)CurrentDirection].DrawAnimation(spriteBatch, new Vector2(Position.X - NPCRectangleXOffSet - 8, Position.Y - NPCRectangleYOffSet - 8), .5f + (Game1.Utility.ForeGroundMultiplier * ((float)NPCAnimatedSprite[(int)CurrentDirection].DestinationRectangle.Y)));
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
            spriteBatch.Draw(DebugTexture, new Vector2(this.NPCPathFindRectangle.X, this.NPCPathFindRectangle.Y), color: Color.Blue, layerDepth: layerDepth);
            spriteBatch.Draw(NextPointRectangleTexture, new Vector2(this.NextPointRectangle.X + 8, this.NextPointRectangle.Y + 8), color: Color.White, layerDepth: layerDepth);

            if (this.CurrentPath != null)
            {
                for (int i = 0; i < this.CurrentPath.Count - 1; i++)
                {
                    Game1.Utility.DrawLine(Game1.LineTexture, spriteBatch, new Vector2(CurrentPath[i].X * 16 + CurrentChunkX * 16 * 16 + 8, CurrentPath[i].Y * 16 + CurrentChunkY * 16 * 16 + 8), new Vector2(CurrentPath[i + 1].X * 16 + CurrentChunkX * 16 * 16 + 8, CurrentPath[i + 1].Y * 16 + CurrentChunkY * 16 * 16 + 8), this.DebugColor);
                }
            }

        }




    }
}
