using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.PathFinding.PathFinder;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
using XMLData.RouteStuff;

namespace SecretProject.Class.NPCStuff.Enemies
{
    //TODO: Write method which allows pathfinding with wandering.
    public enum CurrentBehaviour
    {
        Wander = 1,
        Chase = 2, 
        Hurt = 3
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
        public Rectangle NPCHitBoxRectangle { get { return new Rectangle((int)Position.X + NPCRectangleXOffSet, (int)Position.Y + NPCRectangleYOffSet, NPCRectangleWidthOffSet, NPCRectangleHeightOffSet); } }
        public Rectangle NPCPathFindRectangle
        {
            get
            {
                return new Rectangle(NPCAnimatedSprite[(int)CurrentDirection].DestinationRectangle.X + 16,
NPCAnimatedSprite[(int)CurrentDirection].DestinationRectangle.Y + 20, 8, 8);
            }
            set { }
        }
        public Texture2D Texture { get; set; }
        public Texture2D DebugTexture { get; set; }

        public float Speed { get; set; }
        //0 = down, 1 = left, 2 =  right, 3 = up
        public Dir CurrentDirection { get; set; }
        public bool IsMoving { get; set; }
        public Vector2 PrimaryVelocity { get; set; }
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

        public Enemy(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet)
        {
            this.Name = name;
            this.Position = position;
            this.Texture = spriteSheet;
            Collider = new Collider(graphics, this.PrimaryVelocity, this.NPCHitBoxRectangle, this, ColliderType.Enemy);

            this.DebugTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);

            NextPointRectangle = new Rectangle(0, 0, 16, 16);
            NextPointRectangleTexture = SetRectangleTexture(graphics, NextPointRectangle);
            this.DebugColor = Color.Red;
            this.CurrentPath = new List<PathFinderNode>();
            this.CurrentBehaviour = CurrentBehaviour.Wander;

            switch (name)
            {
                case "boar":
                    NPCAnimatedSprite = new Sprite[4];

                    NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 48, 32, 3, .15f, this.Position);
                    NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 144, 0, 48, 32, 3, .15f, this.Position);
                    NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 288, 0, 48, 32, 3, .15f, this.Position);
                    NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 432, 0, 48, 32, 3, .15f, this.Position);

                    this.NPCRectangleXOffSet = 15;
                    this.NPCRectangleYOffSet = 15;
                    this.NPCRectangleHeightOffSet = 4;
                    this.NPCRectangleWidthOffSet = 4;
                    this.Speed = .05f;
                    this.DebugTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
                    this.SoundID = 14;
                    this.SoundTimer = Game1.Utility.RFloat(5f, 50f);
                    this.CurrentBehaviour = CurrentBehaviour.Wander;
                    break;
                case "crab":
                    NPCAnimatedSprite = new Sprite[4];

                    NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 32, 48, 32, 1, .15f, this.Position);
                    NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 48, 32, 48, 32, 2, .15f, this.Position);
                    NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 48, 32, 48, 32, 2, .15f, this.Position);
                    NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 48, 32, 48, 32, 2, .15f, this.Position);

                    this.NPCRectangleXOffSet = 15;
                    this.NPCRectangleYOffSet = 15;
                    this.NPCRectangleHeightOffSet = 4;
                    this.NPCRectangleWidthOffSet = 4;
                    this.Speed = .05f;
                    this.DebugTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
                    this.SoundID = 14;
                    this.SoundTimer = Game1.Utility.RFloat(5f, 50f);
                    
                    break;
            }

        }

        public void Update(GameTime gameTime, MouseManager mouse, IInformationContainer container)
        {
            this.CurrentBehaviour = CurrentBehaviour.Wander;
            this.IsMoving = true;
            this.PrimaryVelocity = new Vector2(1, 1);
            Collider.Rectangle = this.NPCHitBoxRectangle;
            Collider.Velocity = this.PrimaryVelocity;
            List<ICollidable> returnObjects = new List<ICollidable>();
            Game1.GetCurrentStage().QuadTree.Retrieve(returnObjects, Collider);
            for (int i = 0; i < returnObjects.Count; i++)
            {
                //if obj collided with item in list stop it from moving boom badda bing
                if (Collider.DidCollide(returnObjects[i], Position))
                {
                    CollideOccured = true;
                    if (returnObjects[i].ColliderType == ColliderType.PlayerBigBox)
                    {
                        this.CurrentBehaviour = CurrentBehaviour.Chase;
                    }
                    else if (returnObjects[i].ColliderType == ColliderType.grass)
                    {
                        if (Collider.IsIntersecting(returnObjects[i]))
                        {
                            returnObjects[i].IsUpdating = true;
                            returnObjects[i].InitialShuffDirection = this.CurrentDirection;
                        }
                    }


                    //  IsMoving = false;
                }


            }
            for (int i = 0; i < 4; i++)
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

            if (CollideOccured)
            {
                this.PrimaryVelocity = Collider.Velocity;
            }
            if (IsMoving)
            {
                switch (CurrentBehaviour)
                {
                    case CurrentBehaviour.Wander:
                        Wander(gameTime, container);
                        break;
                    case CurrentBehaviour.Chase:
                        MoveTowardsPoint(Game1.Player.position, gameTime);
                        break;
                    case CurrentBehaviour.Hurt:
                        for (int i = 0; i < NPCAnimatedSprite.Length; i++)
                        {
                            NPCAnimatedSprite[i].Flash(gameTime, .05f, Color.Red);
                        }
                        //CurrentBehaviour = CurrentBehaviour.Wander;
                        break;
                }


            }


            if (IsMoving)
            {

                for (int i = 0; i < 4; i++)
                {
                    NPCAnimatedSprite[i].UpdateAnimations(gameTime, Position);
                }



            }
            else
            {
                this.NPCAnimatedSprite[(int)CurrentDirection].SetFrame(0);

            }
            SoundTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (SoundTimer <= 0)
            {
                Game1.SoundManager.PlaySoundEffectFromInt(1, SoundID, Game1.SoundManager.GameVolume);
                SoundTimer = Game1.Utility.RFloat(5f, 50f);
                RollDrop(gameTime, 148, this.Position, 10);
            }


        }

        public void RollDrop(GameTime gameTime, int iD, Vector2 positionToDrop, int upperChance)
        {
            int success = Game1.Utility.RNumber(1, upperChance);
            if (success == 1)
            {
                Game1.GetCurrentStage().AllItems.Add(Game1.ItemVault.GenerateNewItem(iD, positionToDrop, true));
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
            Position += direction * Speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If we moved PAST the goal, move it back to the goal
            if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - Position)) + 1) < 0.1f)
                Position = goal;

            // Return whether we've reached the goal or not
            return Position == goal;
        }
        public void MoveToTile(GameTime gameTime, IInformationContainer container)
        {

            if (CurrentPath.Count > 0)
            {
                if (MoveTowardsPoint(new Vector2(CurrentPath[CurrentPath.Count - 1].X * 16 + container.X * 16 * 16, CurrentPath[CurrentPath.Count - 1].Y * 16 + container.Y * 16 * 16), gameTime))
                {
                    CurrentPath.RemoveAt(CurrentPath.Count - 1);
                }



            }
            else if(WanderTimer <= 0)
            {
                int currentTileX = (int)(this.Position.X / 16 - (container.X * 16));
                int currentTileY = (int)(this.Position.Y / 16 - (container.Y * 16));
                int newX = Game1.Utility.RGenerator.Next(-10, 10);
                int newY = Game1.Utility.RGenerator.Next(-10, 10);
                if (currentTileX + newX < container.MapWidth - 2 && currentTileX + newX > 0 && currentTileY + newY < container.MapHeight - 2 && currentTileY + newY > 0)
                {
                    if (container.PathGrid.Weight[currentTileX + newX, currentTileY + newY] != 0)
                    {
                        Point end = new Point(currentTileX + newX, currentTileY + newY);



                        PathFinderFast finder = new PathFinderFast(container.PathGrid.Weight);


                        Point start = new Point(Math.Abs((int)this.Position.X / 16 - container.X * 16),
                         (Math.Abs((int)this.Position.Y / 16 - container.Y * 16)));

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
 
        public void Wander(GameTime gameTime, IInformationContainer container)
        {
            
            WanderTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            MoveToTile(gameTime, container);
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
            int amount = 30;
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
                    this.Position = new Vector2(this.Position.X , this.Position.Y - amount);
                    break;
                default:
                    this.Position = new Vector2(this.Position.X, this.Position.Y - amount);
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (CurrentDirection)
            {
                //double num = (NPCAnimatedSprite[0].DestinationRectangle.Bottom + NPCAnimatedSprite[0].DestinationRectangle.Height)/ 1600;
                case Dir.Down:
                    NPCAnimatedSprite[0].DrawAnimation(spriteBatch, Position, .5f + (.00001f * ((float)NPCAnimatedSprite[0].DestinationRectangle.Top + NPCAnimatedSprite[0].DestinationRectangle.Height)));
                    break;
                case Dir.Left:
                    NPCAnimatedSprite[1].DrawAnimation(spriteBatch, Position, .5f + (.00001f * ((float)NPCAnimatedSprite[1].DestinationRectangle.Top + NPCAnimatedSprite[1].DestinationRectangle.Height)));
                    break;
                case Dir.Right:
                    NPCAnimatedSprite[2].DrawAnimation(spriteBatch, Position, .5f + (.00001f * ((float)NPCAnimatedSprite[2].DestinationRectangle.Top + NPCAnimatedSprite[2].DestinationRectangle.Height)));
                    break;
                case Dir.Up:
                    NPCAnimatedSprite[3].DrawAnimation(spriteBatch, Position, .5f + (.00001f * ((float)NPCAnimatedSprite[3].DestinationRectangle.Top + NPCAnimatedSprite[3].DestinationRectangle.Height)));
                    break;
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
        public void DrawDebug(SpriteBatch spriteBatch, float layerDepth, IInformationContainer container)
        {
            spriteBatch.Draw(DebugTexture, new Vector2(this.NPCPathFindRectangle.X, this.NPCPathFindRectangle.Y), color: Color.Blue, layerDepth: layerDepth);
            spriteBatch.Draw(NextPointRectangleTexture, new Vector2(this.NextPointRectangle.X + 8, this.NextPointRectangle.Y + 8), color: Color.White, layerDepth: layerDepth);

            if(this.CurrentPath != null)
            {
                for (int i = 0; i < this.CurrentPath.Count - 1; i++)
                {
                    Game1.Utility.DrawLine(Game1.LineTexture, spriteBatch, new Vector2(CurrentPath[i].X * 16 + container.X * 16 * 16, CurrentPath[i].Y * 16 + container.Y * 16 * 16), new Vector2(CurrentPath[i + 1].X * 16 + container.X * 16 * 16, CurrentPath[i + 1].Y * 16 + container.Y * 16 * 16), this.DebugColor);
                }
            }
            
        }




    }
}
