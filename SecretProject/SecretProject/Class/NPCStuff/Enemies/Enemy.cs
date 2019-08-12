using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.ObjectFolder;
using SecretProject.Class.SpriteFolder;
using XMLData.RouteStuff;

namespace SecretProject.Class.NPCStuff.Enemies
{
    //TODO: Write method which allows pathfinding with wandering.
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

        public Texture2D Texture { get; set; }
        public Texture2D DebugTexture { get; set; }

        public float Speed { get; set; }
        //0 = down, 1 = left, 2 =  right, 3 = up
        public int CurrentDirection { get; set; }
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
        public int CurrentTileX { get { return (int)(this.Position.X / 16); } }
        public int CurrentTileY { get { return (int)(this.Position.Y / 16); } }
        public float SoundTimer { get; set; }
        public float SoundMin { get; set; }
        public float SoundMax { get; set; }
        public int SoundID { get; set; }

        public Enemy(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet)
        {
            this.Name = name;
            this.Position = position;
            this.Texture = spriteSheet;
            Collider = new Collider(this.PrimaryVelocity, this.NPCHitBoxRectangle);

            this.DebugTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);

            NextPointRectangle = new Rectangle(0, 0, 16, 16);
            NextPointRectangleTexture = SetRectangleTexture(graphics, NextPointRectangle);
        }

        public virtual void Update(GameTime gameTime, List<ObjectBody> objects, MouseManager mouse)
        {
            this.PrimaryVelocity = new Vector2(1, 1);
            Collider.Rectangle = this.NPCHitBoxRectangle;
            Collider.Velocity = this.PrimaryVelocity;
            this.CollideOccured = Collider.DidCollide(objects, Position);

            switch (CurrentDirection)
            {
                case 0:
                    NPCAnimatedSprite[0].UpdateAnimations(gameTime, Position);
                    break;
                case 1:
                    NPCAnimatedSprite[1].UpdateAnimations(gameTime, Position);
                    break;
                case 2:
                    NPCAnimatedSprite[2].UpdateAnimations(gameTime, Position);
                    break;
                case 3:
                    NPCAnimatedSprite[3].UpdateAnimations(gameTime, Position);
                    break;
            }
            if (mouse.WorldMouseRectangle.Intersects(this.NPCHitBoxRectangle))
            {
                mouse.ChangeMouseTexture(200);
                mouse.ToggleGeneralInteraction = true;
                Game1.isMyMouseVisible = false;

            }
            if (IsMoving)
            {


                UpdateDirection();
                this.PrimaryVelocity = Collider.Velocity;
            }
            else
            {
                this.NPCAnimatedSprite[CurrentDirection].SetFrame(0);
            }

            SoundTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(SoundTimer <= 0)
            {
                PlaySound(SoundID);
                SoundTimer = Game1.Utility.RFloat(5f, 50f);
            }


        }

        public void RollDrop(GameTime gameTime,int iD, Vector2 positionToDrop, int upperChance)
        {
            int success = Game1.Utility.RNumber(1, upperChance);
            if(success == 1)
            {
                Game1.GetCurrentStage().AllItems.Add(Game1.ItemVault.GenerateNewItem(iD, positionToDrop, true));
            }
        }
        float timeBetweenJumps = .4f;
        int pointCounter = 0;
        bool pathFound = false;

        List<Point> currentPath = new List<Point>()
        {
            new Point(1,1)
        };

        public void MoveToTile(GameTime gameTime, Route route)
        {
            if (Game1.GlobalClock.TotalHours >= route.TimeToStart && Game1.GlobalClock.TotalHours <= route.TimeToFinish ||
                Game1.GlobalClock.TotalHours >= route.TimeToStart && route.TimeToFinish <= route.TimeToStart)
            {
                if ((!(this.Position.X == currentPath[currentPath.Count - 1].X * 16) && !(this.Position.Y == currentPath[currentPath.Count - 1].Y * 16)) ||
                    pointCounter < currentPath.Count)
                {


                    if (pathFound == false)
                    {
                        this.IsMoving = true;

                        currentPath = Game1.GetCurrentStage().AllTiles.PathGrid.Pathfind(new Point((int)this.Position.X / 16,
                            (int)this.Position.Y / 16), new Point(route.EndX, route.EndY));

                        pathFound = true;
                    }
                    timeBetweenJumps -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (timeBetweenJumps <= 0)
                    {
                        pointCounter++;
                        timeBetweenJumps = .4f;
                    }
                    if (pointCounter < currentPath.Count)
                    {
                        //this.Position = new Vector2(currentPath[counter].X * 16, currentPath[counter].Y * 16);
                        MoveTowardsPosition(new Vector2(currentPath[pointCounter].X * 16  , currentPath[pointCounter].Y * 16 ), new Rectangle(currentPath[pointCounter].X * 16 - 16, currentPath[pointCounter].Y * 16 - 16, 32, 32));
                        DebugNextPoint = new Vector2(route.EndX * 16, route.EndY * 16);
                    }
                    else
                    {
                        pathFound = false;
                        pointCounter = 0;
                        this.IsMoving = false;
                        this.CurrentDirection = 0;
                    }
                }
            }

        }
        //For use without route schedule
        public void MoveToTile(GameTime gameTime, Point point)
        {

            if ((!(this.Position.X == currentPath[currentPath.Count - 1].X * 16) && !(this.Position.Y == currentPath[currentPath.Count - 1].Y * 16)) ||
                pointCounter < currentPath.Count)
            {


                if (pathFound == false)
                {
                    this.IsMoving = true;



                    currentPath = Game1.GetCurrentStage().AllTiles.PathGrid.Pathfind(new Point((int)this.Position.X / 16,
                        (int)this.Position.Y / 16), point);
                    if (currentPath.Contains(new Point(-1, -1)))
                    {
                       
                        return;

                    }
                    pathFound = true;
                   // this.Position = new Vector2(currentPath[0].X, currentPath[0].Y);
                }
                if (!currentPath.Contains(new Point(-1, -1)))
                {
                    timeBetweenJumps -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    NextPointRectangle = new Rectangle(currentPath[pointCounter].X * 16, currentPath[pointCounter].Y * 16, 16, 16);
                    if (this.NPCHitBoxRectangle.Intersects(NextPointRectangle))
                    {
                        pointCounter++;
                        timeBetweenJumps = .4f;
                    }
                    if (pointCounter < currentPath.Count)
                    {
                        //this.Position = new Vector2(currentPath[counter].X * 16, currentPath[counter].Y * 16);
                        MoveTowardsPosition(new Vector2(NextPointRectangle.X -16, NextPointRectangle.Y- 16), new Rectangle(currentPath[pointCounter].X * 16, currentPath[pointCounter].Y * 16, 16, 16));
                        //DebugNextPoint = new Vector2(route.EndX * 16, route.EndY * 16);
                    }
                    else
                    {
                        pathFound = false;
                        pointCounter = 0;
                        this.IsMoving = false;
                        //this.CurrentDirection = 0;
                        WanderTimer = Game1.Utility.RGenerator.Next(3, 5);
                    }
                }
            }


        }


        public void MoveTowardsPosition(Vector2 positionToMoveTowards, Rectangle rectangle)
        {

            Vector2 direction = Vector2.Normalize((positionToMoveTowards - Position) + new Vector2((float).0000000001, (float).0000000001));
            //if (!(System.Single.IsNaN(direction.X) || System.Single.IsNaN(direction.Y)))
            //{
            this.DirectionVector = direction;
            if (!this.NPCHitBoxRectangle.Intersects(rectangle))
            {
                Position += (direction * Speed) * PrimaryVelocity;
                IsMoving = true;
            }
            else
            {
                this.NPCAnimatedSprite[CurrentDirection].SetFrame(0);
                IsMoving = false;
            }
        }

        private float WanderTimer = 2f;
        private Vector2 wanderPosition = new Vector2(0, 0);
        public void Wander(GameTime gameTime)
        {
            //temporary
            //MoveTowardsPosition(wanderPosition, new Rectangle((int)wanderPosition.X, (int)wanderPosition.Y, 20, 20));
            WanderTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (WanderTimer <= 0)
            {

                int newX = Game1.Utility.RGenerator.Next(-10, 10);
                int newY = Game1.Utility.RGenerator.Next(-10, 10);

                if (this.CurrentTileX + newX < 99 && this.CurrentTileX + newX > 0 && this.CurrentTileY + newY < 99 && this.CurrentTileY + newY > 0)
                {
                    int testAStar = Game1.GetCurrentStage().AllTiles.AllTiles[1][this.CurrentTileX + newX, this.CurrentTileY + newY].AStarTileValue;
                    if (Game1.GetCurrentStage().AllTiles.AllTiles[1][this.CurrentTileX + newX, this.CurrentTileY + newY].AStarTileValue == 1)
                    {
                        MoveToTile(gameTime, new Point(this.CurrentTileX + newX, this.CurrentTileY + newY));
                        //wanderPosition = new Vector2(Position.X + newX, Position.Y + newY);
                       
                    }

                }



            }
        }
        public void UpdateDirection()
        {
            if (DirectionVector.X > .5f)
            {
                CurrentDirection = 2; //right
            }
            else if (DirectionVector.X < -.5f)
            {
                CurrentDirection = 1; //left
            }
            else if (DirectionVector.Y < .5f) // up
            {
                CurrentDirection = 3;
            }

            else if (DirectionVector.Y > .5f)
            {
                CurrentDirection = 0;
            }
            else
            {
                CurrentDirection = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (CurrentDirection)
            {
                //double num = (NPCAnimatedSprite[0].DestinationRectangle.Bottom + NPCAnimatedSprite[0].DestinationRectangle.Height)/ 1600;
                case 0:
                    float num = .4f + (.0001f * ((float)NPCAnimatedSprite[0].DestinationRectangle.Y + NPCAnimatedSprite[0].DestinationRectangle.Height));
                    NPCAnimatedSprite[0].DrawAnimation(spriteBatch, Position, .4f + (.0001f * ((float)NPCAnimatedSprite[0].DestinationRectangle.Y + NPCAnimatedSprite[0].DestinationRectangle.Height)));
                    break;
                case 1:
                    NPCAnimatedSprite[1].DrawAnimation(spriteBatch, Position, .4f + (.0001f * ((float)NPCAnimatedSprite[1].DestinationRectangle.Y + NPCAnimatedSprite[1].DestinationRectangle.Height)));
                    break;
                case 2:
                    NPCAnimatedSprite[2].DrawAnimation(spriteBatch, Position, .4f + (.0001f * ((float)NPCAnimatedSprite[2].DestinationRectangle.Y + NPCAnimatedSprite[2].DestinationRectangle.Height)));
                    break;
                case 3:
                    NPCAnimatedSprite[3].DrawAnimation(spriteBatch, Position, .4f + (.0001f * ((float)NPCAnimatedSprite[3].DestinationRectangle.Y + NPCAnimatedSprite[3].DestinationRectangle.Height)));
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
        public void DrawDebug(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(DebugTexture, new Vector2(this.NPCHitBoxRectangle.X, this.NPCHitBoxRectangle.Y), color: Color.White, layerDepth: layerDepth);
            spriteBatch.Draw(NextPointRectangleTexture, new Vector2(this.NextPointRectangle.X + 8, this.NextPointRectangle.Y + 8), color: Color.White, layerDepth: layerDepth);

            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                Game1.Utility.DrawLine(Game1.LineTexture, spriteBatch, new Vector2(currentPath[i].X * 16, currentPath[i].Y * 16), new Vector2(currentPath[i + 1].X * 16, currentPath[i + 1].Y * 16));
            }
        }


         
        public void PlaySound(int soundID)
        {
            Game1.SoundManager.PlaySoundEffectFromInt(false, 1, soundID, 1f);
        }

    }
}
