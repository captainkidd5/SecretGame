using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;

using SecretProject.Class.SpriteFolder;
using SecretProject.Class.TileStuff;
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
        public Rectangle NPCPathFindRectangle
        {
            get
            {
                return new Rectangle(NPCAnimatedSprite[CurrentDirection].DestinationRectangle.X + 16,
NPCAnimatedSprite[CurrentDirection].DestinationRectangle.Y + 20, 8, 8);
            }
            set { }
        }
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
        //public int CurrentTileX { get { return (int)(this.Position.X / 16); } }
       // public int CurrentTileY { get { return (int)(this.Position.Y / 16); } }
        public float SoundTimer { get; set; }
        public float SoundMin { get; set; }
        public float SoundMax { get; set; }
        public int SoundID { get; set; }

        public Color DebugColor { get; set; }

        public Enemy(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet)
        {
            this.Name = name;
            this.Position = position;
            this.Texture = spriteSheet;
            Collider = new Collider(this.PrimaryVelocity, this.NPCHitBoxRectangle);

            this.DebugTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);

            NextPointRectangle = new Rectangle(0, 0, 16, 16);
            NextPointRectangleTexture = SetRectangleTexture(graphics, NextPointRectangle);
            this.DebugColor = Color.Red;
        }

        public virtual void Update(GameTime gameTime, Dictionary<string,ObjectBody> objects, MouseManager mouse, IInformationContainer container)
        {
            this.PrimaryVelocity = new Vector2(1, 1);
            Collider.Rectangle = this.NPCHitBoxRectangle;
            Collider.Velocity = this.PrimaryVelocity;
           // this.CollideOccured = Collider.DidCollide(objects, Position);

            for (int i = 0; i < 4; i++)
            {
                NPCAnimatedSprite[i].UpdateAnimations(gameTime, Position);
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

        
        //For use without route schedule
        public void MoveToTile(GameTime gameTime, Point point,IInformationContainer container)
        {

            if (pointCounter < currentPath.Count && !this.NPCPathFindRectangle.Intersects(new Rectangle(point.X * 16 * + container.X * 16, point.Y * 16 + container.Y * 16, 16, 16)))
            { 
                if (pathFound == false)
                {
                    this.IsMoving = true;



                    currentPath = container.PathGrid.Pathfind(new Point(Math.Abs((int)this.NPCPathFindRectangle.X) / 16,
                        Math.Abs((int)this.NPCPathFindRectangle.Y) / 16), point,this.Name);
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
                    NextPointRectangle = new Rectangle(currentPath[pointCounter].X * 16 + container.X * 16, currentPath[pointCounter].Y * 16 + container.Y * 16, 16, 16);
                    if (this.NPCPathFindRectangle.Intersects(NextPointRectangle))
                    {
                        pointCounter++;
                        timeBetweenJumps = .4f;
                    }
                    if (pointCounter < currentPath.Count)
                    {
                        //this.Position = new Vector2(currentPath[counter].X * 16, currentPath[counter].Y * 16);
                        MoveTowardsPosition(new Vector2(NextPointRectangle.X , NextPointRectangle.Y), new Rectangle(currentPath[pointCounter].X * 16 + container.X * 16 + 8, currentPath[pointCounter].Y * 16 + + container.Y * 16 + 8, 4, 4),container);
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


        public void MoveTowardsPosition(Vector2 positionToMoveTowards, Rectangle rectangle, IInformationContainer container)
        {

            Vector2 direction = Vector2.Normalize((positionToMoveTowards - new Vector2(NPCPathFindRectangle.X, NPCPathFindRectangle.Y) + new Vector2((float).0000000001, (float).0000000001)));
            //if (!(System.Single.IsNaN(direction.X) || System.Single.IsNaN(direction.Y)))
            //{
            
            if (!this.NPCPathFindRectangle.Intersects(rectangle))
            {
                Position += (direction * Speed) * PrimaryVelocity;
                IsMoving = true;
            }
            else
            {
                this.NPCAnimatedSprite[CurrentDirection].SetFrame(0);
                IsMoving = false;
            }
            this.DirectionVector = direction;
        }

        private float WanderTimer = 2f;
        private Vector2 wanderPosition = new Vector2(0, 0);
        public void Wander(GameTime gameTime, IInformationContainer container)
        {
            int currentTileX = Math.Abs((int)(this.Position.X / 16 - (container.X * 16)));
            int currentTileY = Math.Abs((int)(this.Position.Y / 16 - (container.Y * 16)));
            //temporary
            //MoveTowardsPosition(wanderPosition, new Rectangle((int)wanderPosition.X, (int)wanderPosition.Y, 20, 20));
            WanderTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (WanderTimer <= 0)
            {

                int newX = Game1.Utility.RGenerator.Next(-10, 10);
                int newY = Game1.Utility.RGenerator.Next(-10, 10);

                if (currentTileX + newX < container.MapWidth - 2 && currentTileX + newX > 0 && currentTileY + newY < container.MapHeight - 2 && currentTileY + newY > 0)
                {

                    if (container.PathGrid.Weight[currentTileX + newX, currentTileY + newY] != 0)
                    {
                        MoveToTile(gameTime, new Point(currentTileX   + newX, currentTileY+ newY), container);
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
                    NPCAnimatedSprite[0].DrawAnimation(spriteBatch, Position, .5f + (.00001f * ((float)NPCAnimatedSprite[0].DestinationRectangle.Top + NPCAnimatedSprite[0].DestinationRectangle.Height)));
                    break;
                case 1:
                    NPCAnimatedSprite[1].DrawAnimation(spriteBatch, Position, .5f + (.00001f * ((float)NPCAnimatedSprite[1].DestinationRectangle.Top + NPCAnimatedSprite[1].DestinationRectangle.Height)));
                    break;
                case 2:
                    NPCAnimatedSprite[2].DrawAnimation(spriteBatch, Position, .5f + (.00001f * ((float)NPCAnimatedSprite[2].DestinationRectangle.Top + NPCAnimatedSprite[2].DestinationRectangle.Height)));
                    break;
                case 3:
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
        public void DrawDebug(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(DebugTexture, new Vector2(this.NPCPathFindRectangle.X, this.NPCPathFindRectangle.Y), color: Color.Blue, layerDepth: layerDepth);
            spriteBatch.Draw(NextPointRectangleTexture, new Vector2(this.NextPointRectangle.X + 8, this.NextPointRectangle.Y + 8), color: Color.White, layerDepth: layerDepth);

            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                Game1.Utility.DrawLine(Game1.LineTexture, spriteBatch, new Vector2(currentPath[i].X * 16, currentPath[i].Y * 16), new Vector2(currentPath[i + 1].X * 16, currentPath[i + 1].Y * 16),this.DebugColor);
            }
        }


         
        public void PlaySound(int soundID)
        {
            Game1.SoundManager.PlaySoundEffectFromInt(false, 1, soundID, 1f);
        }

    }
}
