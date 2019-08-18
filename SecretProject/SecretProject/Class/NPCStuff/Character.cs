using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ObjectFolder;
using SecretProject.Class.SpriteFolder;
using XMLData.RouteStuff;

namespace SecretProject.Class.NPCStuff
{
    public class Character : INPC
    {
        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public Sprite[] NPCAnimatedSprite { get; set; }
        public Texture2D Texture { get; set; }
        public Texture2D DebugTexture { get; set; }
        public int NPCRectangleXOffSet { get; set; }
        public int NPCRectangleYOffSet { get; set; }
        public int NPCRectangleWidthOffSet { get; set; } = 1;
        public int NPCRectangleHeightOffSet { get; set; } = 1;
        public Rectangle NPCHitBoxRectangle { get { return new Rectangle((int)Position.X + NPCRectangleXOffSet, (int)Position.Y + NPCRectangleYOffSet, NPCRectangleWidthOffSet, NPCRectangleHeightOffSet); } }
        public Rectangle NPCDialogueRectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, NPCAnimatedSprite[CurrentDirection].SourceRectangle.Width, NPCAnimatedSprite[CurrentDirection].SourceRectangle.Height); } }


        public float Speed { get; set; } = .65f; //.65
        public Vector2 PrimaryVelocity { get; set; }
        public Vector2 TotalVelocity { get; set; }

        public Vector2 DirectionVector { get; set; }


        //0 = down, 1 = left, 2 =  right, 3 = up
        public int CurrentDirection { get; set; }


        public Collider Collider { get; set; }
        public bool CollideOccured { get; set; } = false;
        public int SpeakerID { get; set; }
        public bool IsMoving { get; set; }



        public int FrameNumber { get; set; } = 25;

        public RouteSchedule RouteSchedule { get; set; }

        public Vector2 DebugNextPoint { get; set; } = new Vector2(1, 1);
        public Texture2D NextPointTexture { get; set; }
        public Rectangle NextPointRectangle { get; set; }
        public Texture2D NextPointRectangleTexture { get; set; }
        public Texture2D HitBoxRectangleTexture { get; set; }
        public int FrameToSet { get; set; }
        public Rectangle NPCPathFindRectangle
        {
            get
            {
                return new Rectangle(NPCAnimatedSprite[CurrentDirection].DestinationRectangle.X + 8,
NPCAnimatedSprite[CurrentDirection].DestinationRectangle.Y + 36, 4, 4);
            }
            set { }
        }

        public Character(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, RouteSchedule routeSchedule)
        {
            this.Name = name;
            this.Position = position;
            this.Texture = spriteSheet;
            NPCAnimatedSprite = new Sprite[4];



            Collider = new Collider(this.PrimaryVelocity, this.NPCHitBoxRectangle);
            this.CurrentDirection = 0;

            this.RouteSchedule = routeSchedule;
            DebugTexture = SetRectangleTexture(graphics, NPCHitBoxRectangle);
            NextPointRectangle = new Rectangle(0, 0, 16, 16);
            
            // NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);
            //NextPointRectangleTexture = SetRectangleTexture(graphics, NPCPathFindRectangle);
        }

        public Character(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, int animationFrames)
        {
            this.Name = name;
            this.Position = position;
            this.Texture = spriteSheet;
            NPCAnimatedSprite = new Sprite[animationFrames];



            Collider = new Collider(this.PrimaryVelocity, this.NPCHitBoxRectangle);
            this.CurrentDirection = 0;

        }

        //for normal, moving NPCS
        public void Update(GameTime gameTime, List<ObjectBody> objects, MouseManager mouse)
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
            if (IsMoving)
            {


                UpdateDirection();
                this.PrimaryVelocity = Collider.Velocity;
            }
            else
            {
                this.NPCAnimatedSprite[CurrentDirection].SetFrame(0);
            }
            CheckSpeechInteraction(mouse, FrameToSet);

            FollowSchedule(gameTime, this.RouteSchedule);
            if (mouse.IsRightClicked)
            {
                Console.WriteLine(this.NPCHitBoxRectangle);

            }


        }

        //meant for non-moving, non-Primary NPCS
        public void UpdateBasicNPC(GameTime gameTime, MouseManager mouse)
        {
            NPCAnimatedSprite[0].Update(gameTime);
            CheckBasicNPCSpeechInteraction(mouse, FrameToSet);
        }

        public void CheckSpeechInteraction(MouseManager mouse, int frameToSet)
        {
            if (mouse.WorldMouseRectangle.Intersects(NPCDialogueRectangle))
            {
                mouse.ChangeMouseTexture(200);
                mouse.ToggleGeneralInteraction = true;
                Game1.isMyMouseVisible = false;
                if (mouse.IsClicked)
                {


                    Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, this.Name + ": " + Game1.DialogueLibrary.RetrieveDialogue(this.SpeakerID, Game1.GlobalClock.TotalHours), 2f, null, null);

                    UpdateDirectionVector(Game1.Player.position);
                    this.NPCAnimatedSprite[CurrentDirection].SetFrame(frameToSet);


                }

            }
        }


        public void CheckBasicNPCSpeechInteraction(MouseManager mouse, int frameToSet)
        {
            if (mouse.WorldMouseRectangle.Intersects(NPCDialogueRectangle))
            {
                mouse.ChangeMouseTexture(200);
                mouse.ToggleGeneralInteraction = true;
                Game1.isMyMouseVisible = false;
                if (mouse.IsClicked)
                {


                    Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, this.Name + ": " + Game1.DialogueLibrary.RetrieveDialogue(this.SpeakerID, 1), 2f, null, null);
                    this.NPCAnimatedSprite[CurrentDirection].SetFrame(frameToSet);


                }

            }
        }

        public void MoveTowardsPosition(Vector2 positionToMoveTowards, Rectangle rectangle)
        {

            Vector2 direction = Vector2.Normalize((positionToMoveTowards - new Vector2(NPCPathFindRectangle.X, NPCPathFindRectangle.Y) + new Vector2((float).0000000001, (float).0000000001)));
            //if (!(System.Single.IsNaN(direction.X) || System.Single.IsNaN(direction.Y)))
            //{
            this.DirectionVector = direction;
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
            // }




        }

        public void UpdateDirectionVector(Vector2 positionToFace)
        {
            Vector2 direction = Vector2.Normalize(positionToFace - Position);
            if (System.Single.IsNaN(direction.X) || System.Single.IsNaN(direction.Y))
            {
                throw new Exception("Not a number " + direction.X.ToString() + " or not a number " + direction.Y.ToString());
            }
            this.DirectionVector = direction;

            UpdateDirection();
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
                if (pointCounter < currentPath.Count && !this.NPCPathFindRectangle.Intersects(new Rectangle(route.EndX * 16, route.EndY * 16, 16, 16)))
                {


                    if (pathFound == false)
                    {
                        this.IsMoving = true;

                        currentPath = Game1.GetCurrentStage().AllTiles.PathGrid.Pathfind(new Point((int)this.NPCPathFindRectangle.X / 16,
                            (int)this.NPCPathFindRectangle.Y / 16), new Point(route.EndX, route.EndY));

                        pathFound = true;
                    }
                    //
                    timeBetweenJumps -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    NextPointRectangle = new Rectangle(currentPath[pointCounter].X * 16, currentPath[pointCounter].Y * 16, 6, 6);
                    if (this.NPCPathFindRectangle.Intersects(NextPointRectangle))
                    {
                        pointCounter++;
                        timeBetweenJumps = .4f;
                    }


                    if (pointCounter < currentPath.Count)
                    {

                        MoveTowardsPosition(new Vector2(NextPointRectangle.X , NextPointRectangle.Y ), new Rectangle(currentPath[pointCounter].X * 16 + 8, currentPath[pointCounter].Y * 16 + 8, 8, 8));
                        DebugNextPoint = new Vector2(route.EndX * 16, route.EndY * 16);
                    }
                    else
                    {
                        pathFound = false;
                        pointCounter = 0;
                        this.IsMoving = false;
                        this.CurrentDirection = 0;
                        //currentPath = null;

                    }
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

        public void DrawBasicNPC(SpriteBatch spriteBatch)
        {
            this.NPCAnimatedSprite[0].DrawAnimation(spriteBatch, this.Position, 1f);
            //this.NPCAnimatedSprite[0].Draw(spriteBatch, 1f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (CurrentDirection)
            {

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

        public void FollowSchedule(GameTime gameTime, RouteSchedule routeSchedule)
        {
            for (int i = 0; i < routeSchedule.Routes.Count; i++)
            {

                MoveToTile(gameTime, this.RouteSchedule.Routes[i]);

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
            spriteBatch.Draw(NextPointRectangleTexture, new Vector2(this.NPCPathFindRectangle.X, this.NPCPathFindRectangle.Y), color: Color.White, layerDepth: layerDepth);
            spriteBatch.Draw(DebugTexture, new Vector2(this.NPCHitBoxRectangle.X, this.NPCHitBoxRectangle.Y), color: Color.White, layerDepth: layerDepth);

            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                Game1.Utility.DrawLine(Game1.LineTexture, spriteBatch, new Vector2(currentPath[i].X * 16, currentPath[i].Y * 16), new Vector2(currentPath[i + 1].X * 16, currentPath[i + 1].Y * 16));
            }

            //spriteBatch.Draw(NextPointTexture, DebugNextPoint, color: Color.Blue, layerDepth: layerDepth);
        }

    }
}
