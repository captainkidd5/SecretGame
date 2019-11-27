

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

using SecretProject.Class.PathFinding;
using SecretProject.Class.PathFinding.PathFinder;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using XMLData.DialogueStuff;
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
        public Rectangle NPCDialogueRectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, NPCAnimatedSprite[(int)CurrentDirection].SourceRectangle.Width, NPCAnimatedSprite[(int)CurrentDirection].SourceRectangle.Height); } }


        public float Speed { get; set; } = .65f; //.65
        public Vector2 PrimaryVelocity { get; set; }
        public Vector2 TotalVelocity { get; set; }

        public Vector2 DirectionVector { get; set; }


        //0 = down, 1 = left, 2 =  right, 3 = up
        public Dir CurrentDirection { get; set; }


        public Collider Collider { get; set; }
        public bool CollideOccured { get; set; } = false;
        public int SpeakerID { get; set; }
        public bool IsMoving { get; set; }
        public int CurrentRoutePosition { get; set; }



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
                return new Rectangle(NPCAnimatedSprite[(int)CurrentDirection].DestinationRectangle.X,
NPCAnimatedSprite[(int)CurrentDirection].DestinationRectangle.Y + NPCAnimatedSprite[(int)CurrentDirection].DestinationRectangle.Height, 4, 4);
            }
            set { }
        }

        public Stages CurrentStageLocation { get; set; }
        // refers to whether or not the npc is on the current stage. If not they still update but the player has no knowledge of it.
        public bool DisableInteractions { get; set; }
        public bool IsBasicNPC { get; set; }
        public bool IsInEvent { get; set; }

        public GraphTraverser PortalTraverser { get; set; }



        public Color DebugColor { get; set; }

        public Texture2D CharacterPortraitTexture { get; set; }
        public Rectangle CharacterPortraitSourceRectangle { get; set; }

        public List<PathFinderNode> CurrentPath { get; set; }

        public ResearchAssignment CurrentResearch { get; set; }
        public bool HasActiveResearch { get; set; }

        public Character(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, RouteSchedule routeSchedule, Stages currentStageLocation, bool isBasicNPC, Texture2D characterPortraitTexture = null)
        {
            this.Name = name;
            this.Position = position;
            this.Texture = spriteSheet;
            NPCAnimatedSprite = new Sprite[4];
            this.IsInEvent = false;



            Collider = new Collider(graphics, this.PrimaryVelocity, this.NPCHitBoxRectangle, this);
            this.CurrentDirection = 0;

            this.RouteSchedule = routeSchedule;
            DebugTexture = SetRectangleTexture(graphics, NPCHitBoxRectangle);
            NextPointRectangle = new Rectangle(0, 0, 16, 16);
            this.CurrentStageLocation = currentStageLocation;
            this.IsBasicNPC = isBasicNPC;

            // NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);
            //NextPointRectangleTexture = SetRectangleTexture(graphics, NPCPathFindRectangle);
            DebugColor = Color.White;
            this.PortalTraverser = new GraphTraverser(Game1.PortalGraph);

            if (characterPortraitTexture != null)
            {
                this.CharacterPortraitTexture = characterPortraitTexture;
            }

            this.CharacterPortraitSourceRectangle = new Rectangle(0, 0, 128, 128);
            this.CurrentPath = new List<PathFinderNode>();
            Game1.GlobalClock.DayChanged += this.OnDayIncreased;
        }

        public Character(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, int animationFrames)
        {
            this.Name = name;
            this.Position = position;
            this.Texture = spriteSheet;
            NPCAnimatedSprite = new Sprite[animationFrames];



            Collider = new Collider(graphics, this.PrimaryVelocity, this.NPCHitBoxRectangle, this, ColliderType.NPC);
            this.CurrentDirection = 0;

        }

        //for normal, moving NPCS
        #region UPDATE METHODS
        public virtual void Update(GameTime gameTime, MouseManager mouse)
        {
            this.IsMoving = true;
            CollideOccured = false;
            if (this.IsBasicNPC)
            {
                UpdateBasicNPC(gameTime, mouse);
                return;
            }
            if (Game1.GetCurrentStageInt() == this.CurrentStageLocation)
            {
                this.DisableInteractions = false;
            }
            else
            {
                this.DisableInteractions = true;
            }
            this.PrimaryVelocity = new Vector2(1, 1);
            Collider.Rectangle = this.NPCHitBoxRectangle;
            Collider.Velocity = this.PrimaryVelocity;
            List<ICollidable> returnObjects = new List<ICollidable>();
            //Game1.GetStageFromInt(CurrentStageLocation).QuadTree.Retrieve(returnObjects, Collider);
            //for (int i = 0; i < returnObjects.Count; i++)
            //{
            //    //if obj collided with item in list stop it from moving boom badda bing

            //    if (returnObjects[i].ColliderType != ColliderType.NPC && returnObjects[i].ColliderType != ColliderType.Undetectable && Collider.DidCollide(returnObjects[i], Position))
            //    {
            //        this.NPCAnimatedSprite[CurrentDirection].SetFrame(0);
            //        CollideOccured = true;
            //        IsMoving = false;
            //    }

            //}
            //this.CollideOccured = Collider.DidCollide(Game1.GetStageFromInt(CurrentStageLocation).AllTiles.Objects, Position);



            for (int i = 0; i < 4; i++)
            {
                NPCAnimatedSprite[i].UpdateAnimationPosition(Position);
            }

            UpdateDirection();
            if (CollideOccured)
            {
                this.PrimaryVelocity = Collider.Velocity;
            }



            if (!this.IsInEvent)
            {
                FollowSchedule(gameTime, this.RouteSchedule);
            }

            if (!DisableInteractions)
            {
                CheckSpeechInteraction(mouse, FrameToSet);
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

        }

        public void EventUpdate(GameTime gameTime)
        {
            this.PrimaryVelocity = new Vector2(1, 1);
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
                this.NPCAnimatedSprite[(int)CurrentDirection].SetFrame(0);
            }
        }

        //meant for non-moving, non-Primary NPCS
        public void UpdateBasicNPC(GameTime gameTime, MouseManager mouse)
        {
            NPCAnimatedSprite[0].Update(gameTime);
            for (int i = 0; i < NPCAnimatedSprite.Length; i++)
            {
                NPCAnimatedSprite[i].UpdateAnimationPosition(Position);
            }
            if (Game1.GetCurrentStageInt() == this.CurrentStageLocation)
            {
                this.DisableInteractions = false;
            }
            else
            {
                this.DisableInteractions = true;
            }
            if (!DisableInteractions)
            {
                CheckBasicNPCSpeechInteraction(mouse, FrameToSet);
            }

        }
        #endregion
        #region SPEECH
        public void CheckSpeechInteraction(MouseManager mouse, int frameToSet)
        {
            if (mouse.WorldMouseRectangle.Intersects(NPCDialogueRectangle))
            {
                mouse.ChangeMouseTexture(CursorType.Chat);
                mouse.ToggleGeneralInteraction = true;
                Game1.isMyMouseVisible = false;
                if (mouse.IsClicked)
                {
                    if (this.CharacterPortraitTexture != null)
                    {
                        Game1.Player.UserInterface.TextBuilder.SpeakerTexture = this.CharacterPortraitTexture;
                        Game1.Player.UserInterface.TextBuilder.SpeakerPortraitSourceRectangle = this.CharacterPortraitSourceRectangle;
                    }
                    DialogueSkeleton skeleton;
                    if (CurrentResearch != null && CurrentResearch.Complete && !CurrentResearch.Claimed)
                    {
                        string itemStringToWrite = "error!";
                        skeleton = Game1.DialogueLibrary.RetrieveDialogueNoTime(this.SpeakerID, 100);
                        for (int i = 0; i < Game1.Player.UserInterface.CraftingMenu.Tabs.Length; i++)
                        {
                            for (int j = 0; j < Game1.Player.UserInterface.CraftingMenu.Tabs[i].Pages.Count; j++)
                            {
                                for (int z = 0; z < Game1.Player.UserInterface.CraftingMenu.Tabs[i].Pages[j].ToolTips.Count; z++)
                                {
                                    if (Game1.Player.UserInterface.CraftingMenu.Tabs[i].Pages[j].ToolTips[z].Item.ID == CurrentResearch.ID)
                                    {
                                        Game1.Player.UserInterface.CraftingMenu.Tabs[i].Pages[j].ToolTips[z].Locked = false;
                                        itemStringToWrite = Game1.Player.UserInterface.CraftingMenu.Tabs[i].Pages[j].ToolTips[z].Item.Name + " Unlocked!";
                                    }
                                }
                            }
                        }
                        CurrentResearch.Claimed = true;
                         Game1.GetCurrentStage().ActivateNewRisingText(Game1.Player.Rectangle.Y, Game1.Player.Rectangle.Y - 16, itemStringToWrite, 10f, Color.White);
                    }
                    else
                    {
                        skeleton = Game1.DialogueLibrary.RetrieveDialogue(this, Game1.GlobalClock.TotalDays, Game1.GlobalClock.TotalHours);
                    }
                     
                    if(skeleton != null)
                    {
                        Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, this.Name + ": " + skeleton.TextToWrite, 2f, null, null);
                        Game1.Player.UserInterface.TextBuilder.SpeakerName = this.Name;
                        Game1.Player.UserInterface.TextBuilder.SpeakerID = this.SpeakerID;
                        if (skeleton.SelectableOptions != null)
                        {
                            Game1.Player.UserInterface.TextBuilder.Skeleton = skeleton;
                        }

                        UpdateDirectionVector(Game1.Player.position);
                        this.NPCAnimatedSprite[(int)CurrentDirection].SetFrame(frameToSet);
                    }
                    


                }

            }
        }


        public void CheckBasicNPCSpeechInteraction(MouseManager mouse, int frameToSet)
        {
            if (mouse.WorldMouseRectangle.Intersects(NPCDialogueRectangle))
            {
                mouse.ChangeMouseTexture(CursorType.Chat);
                mouse.ToggleGeneralInteraction = true;
                Game1.isMyMouseVisible = false;
                if (mouse.IsClicked)
                {

                    DialogueSkeleton skeleton = Game1.DialogueLibrary.RetrieveDialogue(this, Game1.GlobalClock.TotalDays, Game1.GlobalClock.TotalHours);
                    Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, this.Name + ": " + skeleton.TextToWrite, 2f, null, null);
                    if (skeleton.SelectableOptions != null)
                    {
                        Game1.Player.UserInterface.TextBuilder.Skeleton = skeleton;
                    }

                    this.NPCAnimatedSprite[(int)CurrentDirection].SetFrame(frameToSet);


                }

            }
        }

        public void OnDayIncreased(object sender, EventArgs e)
        {
            if(this.HasActiveResearch)
            {
                if(CurrentResearch.ContinueResearch())
                {
                    this.HasActiveResearch = false;
                }

            }
        }
        #endregion

        #region PLAYER DIRECTION
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
        #endregion

        #region SCHEDULE AND PATHFINDING
        public void FollowSchedule(GameTime gameTime, RouteSchedule routeSchedule)
        {
            if (!this.CollideOccured)
            {


                for (int i = 0; i < routeSchedule.Routes.Count; i++)
                {

                    MoveToTileRoute(gameTime, this.RouteSchedule.Routes[i]);

                }
            }

        }


        int nodeToEndAt;
        public Point FindIntermediateStages(int stageFrom, int stageTo)
        {
            nodeToEndAt = 100;
            if (PortalTraverser.Graph.HasEdge(stageFrom, stageTo))
            {
                //+32 Y offset to end at bottom of tile!
                nodeToEndAt = stageTo;
                Portal portal = Game1.GetStageFromInt(CurrentStageLocation).AllPortals.Find(x => x.To == stageTo);
                return new Point((portal.PortalStart.X + portal.SafteyOffSetX )/ 16 , (portal.PortalStart.Y + portal.SafteyOffSetY) / 16 );
            }
            else
            {
                int node = PortalTraverser.GetNextNodeInPath(stageFrom, stageTo);
                nodeToEndAt = node;
                Portal portal = Game1.GetStageFromInt(CurrentStageLocation).AllPortals.Find(x => x.To == node);
                return new Point((portal.PortalStart.X + portal.SafteyOffSetX) / 16,
                            (portal.PortalStart.Y + portal.SafteyOffSetY) / 16);
                //throw new Exception(Game1.GetStageFromInt(stageFrom).StageName + " and " + Game1.GetStageFromInt(stageTo).StageName + " are not directly connected!");
            }
        }

        //Note: if NPCs are moving faster than they should be its probably because two schedule times are overlapping.

        private bool MoveTowardsPoint(Vector2 goal, GameTime gameTime)
        {
            // If we're already at the goal return immediatly
            if (Position == goal) return true;

            // Find direction from current position to goal
            Vector2 direction = Vector2.Normalize(goal - Position);
            this.DirectionVector = direction;
            // Move in that direction
            Position += direction * Speed;

            // If we moved PAST the goal, move it back to the goal
            if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - Position)) + 1) < 0.1f)
                Position = goal;

            // Return whether we've reached the goal or not
            return Position == goal;
        }
        public void MoveToTileRoute(GameTime gameTime, Route route)
        {
            if (Game1.GlobalClock.TotalHours >= route.TimeToStart && Game1.GlobalClock.TotalHours <= route.TimeToFinish ||
                Game1.GlobalClock.TotalHours >= route.TimeToStart && route.TimeToFinish <= route.TimeToStart)
            {

                if (CurrentPath.Count > 0)
                {
                    if (MoveTowardsPoint(new Vector2(CurrentPath[CurrentPath.Count - 1].X * 16, CurrentPath[CurrentPath.Count - 1].Y * 16), gameTime))
                    {
                        CurrentPath.RemoveAt(CurrentPath.Count - 1);
                    }

                    if (CurrentPath.Count == 0)
                    {
                        if (route.StageToEndAt != (int)CurrentStageLocation)
                        {
                            if (nodeToEndAt != (int)CurrentStageLocation)
                            {
                                this.Position = new Vector2(Game1.GetStageFromInt((Stages)nodeToEndAt).AllPortals.Find(x => x.To == (int)CurrentStageLocation).PortalStart.X + 16,
                                Game1.GetStageFromInt((Stages)nodeToEndAt).AllPortals.Find(x => x.To == (int)CurrentStageLocation).PortalStart.Y + 32);
                                Game1.GetStageFromInt(CurrentStageLocation).CharactersPresent.Remove(this);
                                CurrentStageLocation = (Stages)nodeToEndAt;
                                Game1.GetStageFromInt(CurrentStageLocation).CharactersPresent.Add(this);
                            }

                        }
                    }

                }
                else if (this.Position != new Vector2(route.EndX * 16, route.EndY * 16))
                {
                    PathFinderFast finder = new PathFinderFast(Game1.GetStageFromInt(CurrentStageLocation).AllTiles.PathGrid.Weight);
                    if (route.StageToEndAt == (int)CurrentStageLocation)
                    {
                        Point start = new Point((int)this.NPCPathFindRectangle.X / 16,
                         ((int)this.NPCPathFindRectangle.Y - NPCAnimatedSprite[(int)CurrentDirection].DestinationRectangle.Height) / 16);
                        Point end = new Point(route.EndX, route.EndY);
                        CurrentPath = finder.FindPath(start, end);
                        if (CurrentPath == null)
                        {
                            throw new Exception(this.Name + " was unable to find a path between " + start + " and " + end);
                        }
                    }
                    else
                    {
                        Point start = new Point((int)this.NPCPathFindRectangle.X / 16,
                         ((int)this.NPCPathFindRectangle.Y - NPCAnimatedSprite[(int)CurrentDirection].DestinationRectangle.Height) / 16);
                        Point end = FindIntermediateStages((int)CurrentStageLocation, route.StageToEndAt);

                        CurrentPath = finder.FindPath(start, end);
                        if (CurrentPath == null)
                        {
                            throw new Exception(this.Name + " was unable to find a path between " + start + " and " + end);
                        }
                    }
                }
                else
                {
                    this.IsMoving = false;
                    this.CurrentDirection = 0;
                }
            }
        }

        #region COMBAT
        public void PlayerCollisionInteraction()
        {
            int amount = 5;
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
            }
        }
        #endregion
        //forEvents
        public void EventMoveToTile(GameTime gameTime, Point endPoint)
        {
            if (CurrentPath.Count > 0)
            {
                if (MoveTowardsPoint(new Vector2(CurrentPath[CurrentPath.Count - 1].X * 16, CurrentPath[CurrentPath.Count - 1].Y * 16), gameTime))
                {
                    CurrentPath.RemoveAt(CurrentPath.Count - 1);
                }
                else if (this.Position != new Vector2(CurrentPath[0].X * 16, CurrentPath[0].Y * 16))
                {
                    PathFinderFast finder = new PathFinderFast(Game1.GetStageFromInt(CurrentStageLocation).AllTiles.PathGrid.Weight);


                    Point start = new Point((int)this.NPCPathFindRectangle.X / 16,
                     ((int)this.NPCPathFindRectangle.Y - NPCAnimatedSprite[(int)CurrentDirection].DestinationRectangle.Height) / 16);
                    Point end = endPoint;
                    CurrentPath = finder.FindPath(start, end);
                    if (CurrentPath == null)
                    {
                        throw new Exception(this.Name + " was unable to find a path between " + start + " and " + end);
                    }

                }
            }
            else
            {
                this.IsMoving = false;
                this.CurrentDirection = 0;
            }

        }

        public bool IsAtTile(Point point)
        {
            if (this.NPCPathFindRectangle.Intersects(new Rectangle(point.X * 16, point.Y * 16, 16, 16)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion

        #region DRAW METHODS
        public void DrawBasicNPC(SpriteBatch spriteBatch)
        {
            if (!DisableInteractions)
            {
                this.NPCAnimatedSprite[0].DrawAnimation(spriteBatch, this.Position, 1f);
            }

            //this.NPCAnimatedSprite[0].Draw(spriteBatch, 1f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!DisableInteractions)
            {
                if (this.IsBasicNPC)
                {
                    DrawBasicNPC(spriteBatch);
                    return;
                }
                switch (CurrentDirection)
                {

                    case 0:
                        float num = .5f + (.0000001f * ((float)NPCAnimatedSprite[0].DestinationRectangle.Y + NPCAnimatedSprite[0].DestinationRectangle.Height));
                        NPCAnimatedSprite[0].DrawAnimation(spriteBatch, Position, .5f + (.0000001f * ((float)NPCAnimatedSprite[0].DestinationRectangle.Y + NPCAnimatedSprite[0].DestinationRectangle.Height)));
                        break;
                    case Dir.Left:
                        NPCAnimatedSprite[1].DrawAnimation(spriteBatch, Position, .5f + (.0000001f * ((float)NPCAnimatedSprite[1].DestinationRectangle.Y + NPCAnimatedSprite[1].DestinationRectangle.Height)));
                        break;
                    case Dir.Right:
                        NPCAnimatedSprite[2].DrawAnimation(spriteBatch, Position, .5f + (.0000001f * ((float)NPCAnimatedSprite[2].DestinationRectangle.Y + NPCAnimatedSprite[2].DestinationRectangle.Height)));
                        break;
                    case Dir.Up:
                        NPCAnimatedSprite[3].DrawAnimation(spriteBatch, Position, .5f + (.0000001f * ((float)NPCAnimatedSprite[3].DestinationRectangle.Y + NPCAnimatedSprite[3].DestinationRectangle.Height)));
                        break;
                }
            }

        }
        #endregion


        #region DEBUG
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
            if (NextPointRectangleTexture != null && DebugTexture != null)
            {
                //spriteBatch.Draw(NextPointRectangleTexture, new Vector2(this.NPCPathFindRectangle.X, this.NPCPathFindRectangle.Y), color: Color.White, layerDepth: layerDepth);
                spriteBatch.Draw(DebugTexture, new Vector2(this.NPCHitBoxRectangle.X, this.NPCHitBoxRectangle.Y), color: DebugColor, layerDepth: 1f);

                for (int i = 0; i < this.CurrentPath.Count - 1; i++)
                {
                    Game1.Utility.DrawLine(Game1.LineTexture, spriteBatch, new Vector2(CurrentPath[i].X * 16 + 8, CurrentPath[i].Y * 16 + 8), new Vector2(CurrentPath[i + 1].X * 16 + 8, CurrentPath[i + 1].Y * 16 + 8), this.DebugColor);
                }
            }


            //spriteBatch.Draw(NextPointTexture, DebugNextPoint, color: Color.Blue, layerDepth: layerDepth);
        }
        #endregion

    }
}

