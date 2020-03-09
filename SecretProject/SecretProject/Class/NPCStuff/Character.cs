﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.PathFinding;
using SecretProject.Class.PathFinding.PathFinder;
using SecretProject.Class.QuestFolder;
using SecretProject.Class.RouteStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using XMLData.DialogueStuff;
using XMLData.QuestStuff;
using XMLData.RouteStuff;

namespace SecretProject.Class.NPCStuff
{
    public class Character : INPC
    {
        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public Sprite[] NPCAnimatedSprite { get; set; }
        public Texture2D Texture { get; set; }
        public Texture2D HitBoxTexture { get; set; }
        public int NPCRectangleXOffSet { get; set; }
        public int NPCRectangleYOffSet { get; set; }
        public int NPCRectangleWidthOffSet { get; set; } = 1;
        public int NPCRectangleHeightOffSet { get; set; } = 1;
        public Rectangle NPCHitBoxRectangle { get { return new Rectangle((int)this.Position.X + this.NPCRectangleXOffSet, (int)this.Position.Y + this.NPCRectangleYOffSet, this.NPCRectangleWidthOffSet, this.NPCRectangleHeightOffSet); } }
        public Rectangle NPCDialogueRectangle { get { return new Rectangle((int)this.Position.X , (int)this.Position.Y - 32, this.NPCAnimatedSprite[(int)this.CurrentDirection].SourceRectangle.Width, this.NPCAnimatedSprite[(int)this.CurrentDirection].SourceRectangle.Height); } }

        public float BaseSpeed { get; private set; } = .65f;
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
                return new Rectangle(this.NPCAnimatedSprite[(int)this.CurrentDirection].DestinationRectangle.X,
this.NPCAnimatedSprite[(int)this.CurrentDirection].DestinationRectangle.Y + this.NPCAnimatedSprite[(int)this.CurrentDirection].DestinationRectangle.Height, 4, 4);
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


        public EmoticonType CurrentEmoticon { get; set; }

        public Route CurrentRoute { get; set; }

        public Stages HomeStage { get; set; }
        public Vector2 HomePosition { get; set; }

        public QuestHandler QuestHandler { get; set; }
        public bool HasActiveQuest { get; set; }
        public bool HasBeenSpokenToAtLeastOnce { get; set; }

        public bool IsBeingSpokenTo { get; set; }

        public Character(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, RouteSchedule routeSchedule, Stages currentStageLocation, bool isBasicNPC, QuestHandler questHandler, Texture2D characterPortraitTexture = null)
        {
            this.HomeStage = currentStageLocation;


            this.Name = name;
            this.Position = new Vector2(position.X * 16, position.Y * 16);
            this.HomePosition = this.Position;
            this.Texture = spriteSheet;
            this.NPCAnimatedSprite = new Sprite[4];
            this.IsInEvent = false;



            this.Collider = new Collider(graphics, this.NPCHitBoxRectangle, this);
            this.CurrentDirection = 0;

            this.RouteSchedule = routeSchedule;
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.NextPointRectangle = new Rectangle(0, 0, 16, 16);
            this.CurrentStageLocation = currentStageLocation;
            this.IsBasicNPC = isBasicNPC;

            // NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);
            //NextPointRectangleTexture = SetRectangleTexture(graphics, NPCPathFindRectangle);
            this.DebugColor = Color.White;
            this.PortalTraverser = new GraphTraverser(Game1.PortalGraph);

            if (characterPortraitTexture != null)
            {
                this.CharacterPortraitTexture = characterPortraitTexture;
            }

            this.CharacterPortraitSourceRectangle = new Rectangle(0, 0, 96, 96);
            this.CurrentPath = new List<PathFinderNode>();
            Game1.GlobalClock.DayChanged += OnDayIncreased;
            Game1.GlobalClock.HourChanged += OnHourIncreased;

            this.QuestHandler = questHandler;
        }

        public Character(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, int animationFrames, Texture2D characterPortraitTexture = null, QuestHandler questHandler = null)
        {
            this.Name = name;
            this.Position = new Vector2(position.X * 16, position.Y * 16);
            this.Texture = spriteSheet;
            this.NPCAnimatedSprite = new Sprite[animationFrames];



            this.Collider = new Collider(graphics, this.NPCHitBoxRectangle, this, ColliderType.NPC);
            this.CurrentDirection = 0;
            if (characterPortraitTexture != null)
            {
                this.CharacterPortraitTexture = characterPortraitTexture;
            }
            this.CharacterPortraitSourceRectangle = new Rectangle(0, 0, 96, 96);
            this.QuestHandler = questHandler;
        }

        public void ResetAnimations()
        {
            for (int i = 0; i < NPCAnimatedSprite.Length; i++)
            {
                if (this.NPCAnimatedSprite[i].CurrentFrame != 0)
                {
                    this.NPCAnimatedSprite[i].SetFrame(0);
                }

            }
            this.CurrentDirection = Dir.Down;

        }

        public void ResetSpeed()
        {
            this.Speed = this.BaseSpeed;
        }

        //for normal, moving NPCS
        #region UPDATE METHODS
        public virtual void Update(GameTime gameTime, MouseManager mouse)
        {
            this.IsMoving = false;
            this.CollideOccured = false;
            if(IsBeingSpokenTo)
            {
                return;
            }
            this.Speed = this.BaseSpeed * Clock.ClockMultiplier;
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
            this.Collider.Rectangle = this.NPCHitBoxRectangle;

            List<ICollidable> returnObjects = new List<ICollidable>();


            for (int i = 0; i < 4; i++)
            {
                this.NPCAnimatedSprite[i].UpdateAnimationPosition(this.Position);
            }



            UpdateDirection();




            if (!this.IsInEvent)
            {
                FollowSchedule(gameTime, this.RouteSchedule);
            }

            if (!this.DisableInteractions)
            {
                CheckSpeechInteraction(mouse, this.FrameToSet);
            }

            if (this.IsMoving)
            {
                for (int i = 0; i < 4; i++)
                {
                    this.NPCAnimatedSprite[i].UpdateAnimations(gameTime, this.Position);
                }
            }
            else
            {
                ResetAnimations();
            }

        }

        public void EventUpdate(GameTime gameTime)
        {
            this.PrimaryVelocity = new Vector2(1, 1);
            UpdateDirection();


            if (this.IsMoving)
            {
                for (int i = 0; i < NPCAnimatedSprite.Length; i++)
                {
                    this.NPCAnimatedSprite[i].UpdateAnimations(gameTime, this.Position);
                }
            }
            else
            {

                ResetAnimations();
            }

        }

 

        //meant for non-moving, non-Primary NPCS
        public void UpdateBasicNPC(GameTime gameTime, MouseManager mouse)
        {
            this.NPCAnimatedSprite[0].Update(gameTime);
            for (int i = 0; i < this.NPCAnimatedSprite.Length; i++)
            {
                this.NPCAnimatedSprite[i].UpdateAnimationPosition(this.Position);
            }
            if (Game1.GetCurrentStageInt() == this.CurrentStageLocation)
            {
                this.DisableInteractions = false;
            }
            else
            {
                this.DisableInteractions = true;
            }
            if (!this.DisableInteractions)
            {
                CheckBasicNPCSpeechInteraction(mouse, this.FrameToSet);
            }

        }
        #endregion
        #region SPEECH

        public bool LoadNewQuest()
        {
            if(this.QuestHandler != null)
            {
                Quest newQuest = QuestHandler.FetchCurrentQuest();
                if (newQuest != null && !QuestHandler.ActiveQuest.Completed)
                {
                    this.HasActiveQuest = true;
                    return true;
                }
            }
            else
            {
                Console.WriteLine(Name + " does not have a propery quest handler!");
            }
            
            return false;

        }

        public void CheckSpeechInteraction(MouseManager mouse, int frameToSet)
        {
            if (Game1.Player.BigCollider.Rectangle.Intersects(this.NPCDialogueRectangle))
                {

                if (mouse.WorldMouseRectangle.Intersects(this.NPCDialogueRectangle))
                {
                    mouse.ChangeMouseTexture(CursorType.Chat);
                    mouse.ToggleGeneralInteraction = true;
                    Game1.isMyMouseVisible = false;
                    if (mouse.IsClicked)
                    {
                        Game1.freeze = true;
                        DialogueSkeleton skeleton;
                        string textToWrite;

                        if (!this.HasActiveQuest && HasBeenSpokenToAtLeastOnce)
                        {
                            if (LoadNewQuest())
                            {
                                Game1.Player.UserInterface.QuestLog.AddNewQuest(QuestHandler);
                                skeleton = QuestHandler.ActiveQuest.MidQuestSkeleton;
                                textToWrite = QuestHandler.ActiveQuest.StartupSpeech;
                                FinishUpDialogue(frameToSet, textToWrite, skeleton);
                                return;
                            }


                        }


                        skeleton = Game1.DialogueLibrary.RetrieveDialogue(this, Game1.GlobalClock.Calendar.CurrentMonth, Game1.GlobalClock.Calendar.CurrentDay, Game1.GlobalClock.GetStringFromTime());
                        if (HasBeenSpokenToAtLeastOnce)
                        {
                            if (!skeleton.HasQuestOptionBeenAdded)
                            {
                                skeleton.TextToWrite += "`";
                                skeleton.SelectableOptions += "Talk about Quest. ~LoadQuest, Exit. ~ExitDialogue";
                                skeleton.HasQuestOptionBeenAdded = true;
                            }

                        }
                        HasBeenSpokenToAtLeastOnce = true;
                        textToWrite = skeleton.TextToWrite;
                        if (HasActiveQuest)
                        {
                            if (QuestHandler.CheckActiveQuestState())
                            {
                                for (int i = 0; i < QuestHandler.ActiveQuest.AllRequiredItems.Count; i++)
                                {
                                    Game1.Player.UserInterface.BackPack.Inventory.RemoveItem(QuestHandler.ActiveQuest.AllRequiredItems[i]);
                                    textToWrite = QuestHandler.ActiveQuest.CompletionSpeech;
                                    HasActiveQuest = false;
                                    QuestHandler.ActiveQuest.Completed = true;
                                }
                                QuestHandler.PerformReward();


                            }
                        }

                        FinishUpDialogue(frameToSet, textToWrite, skeleton);

                    }

                }
            }
        }

        public void FinishUpDialogue(int frameToSet, string textToWrite, DialogueSkeleton skeleton)
        {
            Game1.Player.UserInterface.TextBuilder.ActivateCharacter(this, TextBoxType.dialogue, true, this.Name + ": " + textToWrite, 2f);


            Game1.Player.UserInterface.TextBuilder.Skeleton = skeleton;


            UpdateDirectionVector(Game1.Player.position);
            this.NPCAnimatedSprite[(int)this.CurrentDirection].SetFrame(frameToSet);
            this.IsBeingSpokenTo = true;
        }


        public void CheckBasicNPCSpeechInteraction(MouseManager mouse, int frameToSet)
        {
            if (mouse.WorldMouseRectangle.Intersects(this.NPCDialogueRectangle))
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
                    DialogueSkeleton skeleton = Game1.DialogueLibrary.RetrieveDialogue(this, Game1.GlobalClock.Calendar.CurrentMonth, Game1.GlobalClock.Calendar.CurrentDay, Game1.GlobalClock.GetStringFromTime());
                    Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, this.Name + ": " + skeleton.TextToWrite, 2f, null, null);
                    if (skeleton.SelectableOptions != null)
                    {
                        Game1.Player.UserInterface.TextBuilder.Skeleton = skeleton;
                    }

                    this.NPCAnimatedSprite[(int)this.CurrentDirection].SetFrame(frameToSet);


                }

            }
        }

        public void OnDayIncreased(object sender, EventArgs e)
        {
            //if (this.HasActiveResearch)
            //{
            //    if (this.CurrentResearch.ContinueResearch())
            //    {
            //        this.HasActiveResearch = false;
            //    }

            //}
        }

        public void OnHourIncreased(object sender, EventArgs e)
        {

            this.CurrentRoute = RouteLibrary.RetrieveRoute(this.RouteSchedule, Game1.GlobalClock.Calendar.CurrentMonth, Game1.GlobalClock.Calendar.CurrentDay, Game1.GlobalClock.GetStringFromTime());

        }
        #endregion

        #region PLAYER DIRECTION
        public void UpdateDirectionVector(Vector2 positionToFace)
        {
            Vector2 direction = Vector2.Normalize(positionToFace - this.Position);
            if (System.Single.IsNaN(direction.X) || System.Single.IsNaN(direction.Y))
            {
                throw new Exception("Not a number " + direction.X.ToString() + " or not a number " + direction.Y.ToString());
            }
            this.DirectionVector = direction;

            UpdateDirection();
        }

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

        #endregion

        #region SCHEDULE AND PATHFINDING
        public void FollowSchedule(GameTime gameTime, RouteSchedule routeSchedule)
        {
            if (CurrentRoute == null)
            {

            }
            else
            {
                this.IsMoving = true;
                MoveToTileRoute(gameTime, this.CurrentRoute);

            }

        }


        int nodeToEndAt;
        public Point FindIntermediateStages(int stageFrom, int stageTo)
        {
            nodeToEndAt = 100;
            if (this.PortalTraverser.Graph.HasEdge(stageFrom, stageTo))
            {
                //+32 Y offset to end at bottom of tile!
                nodeToEndAt = stageTo;
                Portal portal = Game1.GetStageFromInt(this.CurrentStageLocation).AllPortals.Find(x => x.To == stageTo);
                return new Point((portal.PortalStart.X + portal.SafteyOffSetX) / 16, (portal.PortalStart.Y) / 16);
            }
            else
            {
                int node = this.PortalTraverser.GetNextNodeInPath(stageFrom, stageTo);
                nodeToEndAt = node;
                Portal portal = Game1.GetStageFromInt(this.CurrentStageLocation).AllPortals.Find(x => x.To == node);
                return new Point((portal.PortalStart.X) / 16,
                            (portal.PortalStart.Y) / 16);
                //throw new Exception(Game1.GetStageFromInt(stageFrom).StageName + " and " + Game1.GetStageFromInt(stageTo).StageName + " are not directly connected!");
            }
        }

        //Note: if NPCs are moving faster than they should be its probably because two schedule times are overlapping.

        private bool MoveTowardsPoint(Vector2 goal, GameTime gameTime)
        {
            // If we're already at the goal return immediatly
            if (this.Position == goal) return true;

            // Find direction from current position to goal
            Vector2 direction = Vector2.Normalize(goal - this.Position);
            this.DirectionVector = direction;
            // Move in that direction
            this.Position += direction * this.Speed;

            // If we moved PAST the goal, move it back to the goal
            if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - this.Position)) + 1) < 0.1f)
                this.Position = goal;

            // Return whether we've reached the goal or not
            return this.Position == goal;
        }
        //public bool TestRoute(Route route)
        //{
        //    if (route.Month == Game1.GlobalClock.Calendar.CurrentMonth && route.WeekDay == (WeekDay)Game1.GlobalClock.Calendar.CurrentDay)
        //    {
        //        if (Game1.GlobalClock.TotalHours >= route.TimeToStart && Game1.GlobalClock.TotalHours <= route.TimeToFinish ||
        //            Game1.GlobalClock.TotalHours >= route.TimeToStart && route.TimeToFinish <= route.TimeToStart)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        public void MoveToTileRoute(GameTime gameTime, Route route)
        {

            if (this.CurrentPath.Count > 0)
            {
                if (MoveTowardsPoint(new Vector2(this.CurrentPath[this.CurrentPath.Count - 1].X * 16, this.CurrentPath[this.CurrentPath.Count - 1].Y * 16), gameTime))
                {
                    this.CurrentPath.RemoveAt(this.CurrentPath.Count - 1);
                }

                if (this.CurrentPath.Count == 0)
                {
                    if (route.StageToEndAt != (int)this.CurrentStageLocation)
                    {
                        if (nodeToEndAt != (int)this.CurrentStageLocation)
                        {
                            Portal portalTo = Game1.GetStageFromInt((Stages)nodeToEndAt).AllPortals.Find(x => x.To == (int)this.CurrentStageLocation);
                            if (portalTo != null)
                            {
                                this.Position = new Vector2(portalTo.PortalStart.X + 16,
                            portalTo.PortalStart.Y + 32);
                                Game1.GetStageFromInt(this.CurrentStageLocation).CharactersPresent.Remove(this);
                                this.CurrentStageLocation = (Stages)nodeToEndAt;
                                Game1.GetStageFromInt(this.CurrentStageLocation).CharactersPresent.Add(this);
                            }

                        }

                    }
                }

            }
            else if (this.Position != new Vector2(route.EndX * 16, route.EndY * 16))
            {
                PathFinderFast finder = new PathFinderFast(Game1.GetStageFromInt(this.CurrentStageLocation).AllTiles.PathGrid.Weight);
                finder.SearchLimit = 10000;
                if (route.StageToEndAt == (int)this.CurrentStageLocation)
                {
                    Point start = new Point((int)this.NPCPathFindRectangle.X / 16,
                     ((int)this.NPCPathFindRectangle.Y - this.NPCAnimatedSprite[(int)this.CurrentDirection].DestinationRectangle.Height) / 16);
                    Point end = new Point(route.EndX, route.EndY);
                    this.CurrentPath = finder.FindPath(start, end, this.Name);
                    if (this.CurrentPath == null)
                    {
                        throw new Exception(this.Name + " was unable to find a path between " + start + " and " + end);
                    }
                }
                else
                {
                    Point start = new Point((int)this.NPCPathFindRectangle.X / 16,
                     ((int)this.NPCPathFindRectangle.Y - this.NPCAnimatedSprite[(int)this.CurrentDirection].DestinationRectangle.Height) / 16);
                    Point end = FindIntermediateStages((int)this.CurrentStageLocation, route.StageToEndAt);

                    this.CurrentPath = finder.FindPath(start, end, this.Name);
                    if (this.CurrentPath == null)
                    {
                        throw new Exception(this.Name + " was unable to find a path between " + start + " and " + end);
                    }
                }
            }
            else
            {
                this.IsMoving = false;
                this.CurrentDirection = 0;
                this.CurrentRoute = null;
            }

        }

        #region COMBAT
        public void DamageCollisionInteraction(int dmgAmount, int knockBack, Dir directionAttackedFrom)
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
        public List<PathFinderNode> EventCurrentPath { get; set; } = new List<PathFinderNode>();


        public void EventMoveToTile(GameTime gameTime, Point endPoint)
        {
            if (this.EventCurrentPath.Count > 0)
            {
                if (MoveTowardsPoint(new Vector2(this.EventCurrentPath[this.EventCurrentPath.Count - 1].X * 16, this.EventCurrentPath[this.EventCurrentPath.Count - 1].Y * 16), gameTime))
                {
                    this.IsMoving = true;
                    this.EventCurrentPath.RemoveAt(this.EventCurrentPath.Count - 1);
                }



            }
            else if (this.Position != new Vector2(endPoint.X * 16, endPoint.Y * 16))
            {
                PathFinderFast finder = null;
                if (Game1.GetCurrentStageInt() == Stages.OverWorld)
                {
                    finder = new PathFinderFast(Game1.GetStageFromInt(this.CurrentStageLocation).AllTiles.ChunkUnderPlayer.PathGrid.Weight);
                }
                else
                {
                    finder = new PathFinderFast(Game1.GetStageFromInt(this.CurrentStageLocation).AllTiles.PathGrid.Weight);
                }


                Point start = new Point((int)(this.Position.X / 16),
                 (int)(this.Position.Y / 16));
                Point end = new Point(endPoint.X, endPoint.Y);
                this.EventCurrentPath = finder.FindPath(start, end, this.Name);
                if (this.EventCurrentPath == null)
                {
                    throw new Exception(this.Name + " was unable to find a path between " + start + " and " + end);
                }


            }
            else
            {
                this.IsMoving = false;
                this.NPCAnimatedSprite[(int)this.CurrentDirection].SetFrame(0);
                // this.CurrentDirection = 0;
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
            if (!this.DisableInteractions)
            {
                this.NPCAnimatedSprite[0].DrawAnimation(spriteBatch, this.Position, 1f);
            }

            //this.NPCAnimatedSprite[0].Draw(spriteBatch, 1f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!this.DisableInteractions)
            {
                if (this.IsBasicNPC)
                {
                    DrawBasicNPC(spriteBatch);
                    return;
                }
                float num = .5f + (Game1.Utility.ForeGroundMultiplier * ((float)this.NPCAnimatedSprite[0].DestinationRectangle.Y + this.NPCAnimatedSprite[0].DestinationRectangle.Height - this.NPCRectangleYOffSet));
                switch (this.CurrentDirection)
                {

                    case 0:

                        this.NPCAnimatedSprite[0].DrawAnimation(spriteBatch, new Vector2(this.Position.X, this.Position.Y - this.NPCRectangleYOffSet), num);
                        break;
                    case Dir.Left:
                        this.NPCAnimatedSprite[1].DrawAnimation(spriteBatch, new Vector2(this.Position.X, this.Position.Y - this.NPCRectangleYOffSet), num);
                        break;
                    case Dir.Right:
                        this.NPCAnimatedSprite[2].DrawAnimation(spriteBatch, new Vector2(this.Position.X, this.Position.Y - this.NPCRectangleYOffSet), num);
                        break;
                    case Dir.Up:
                        this.NPCAnimatedSprite[3].DrawAnimation(spriteBatch, new Vector2(this.Position.X, this.Position.Y - this.NPCRectangleYOffSet), num);
                        break;
                }
            }

            if (this.CurrentEmoticon != EmoticonType.None)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(this.Position.X, this.Position.Y - 24), new Rectangle(0, 48, 16, 32), Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, 1f);
            }

        }
        #endregion


        public void ActivateEmoticon(EmoticonType emoticonType)
        {
            this.CurrentEmoticon = emoticonType;
            Game1.SoundManager.PlayEmoticonSound(emoticonType);
        }

        public void ResetEndOfDay()
        {
            this.Position = this.HomePosition;
            for (int i = 0; i < this.NPCAnimatedSprite.Length; i++)
            {
                this.NPCAnimatedSprite[i].UpdateAnimationPosition(this.Position);
            }
            this.CurrentStageLocation = this.HomeStage;
            

        }


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
            if (this.NextPointRectangleTexture != null && this.HitBoxTexture != null)
            {
                //spriteBatch.Draw(NextPointRectangleTexture, new Vector2(this.NPCPathFindRectangle.X, this.NPCPathFindRectangle.Y), color: Color.White, layerDepth: layerDepth);
                spriteBatch.Draw(this.HitBoxTexture, new Vector2(this.NPCHitBoxRectangle.X, this.NPCHitBoxRectangle.Y), color: this.DebugColor, layerDepth: 1f);

                for (int i = 0; i < this.CurrentPath.Count - 1; i++)
                {
                    Game1.Utility.DrawLine(Game1.LineTexture, spriteBatch, new Vector2(this.CurrentPath[i].X * 16 + 8, this.CurrentPath[i].Y * 16 + 8), new Vector2(this.CurrentPath[i + 1].X * 16 + 8, this.CurrentPath[i + 1].Y * 16 + 8), this.DebugColor);
                }
            }


            //spriteBatch.Draw(NextPointTexture, DebugNextPoint, color: Color.Blue, layerDepth: layerDepth);
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void MouseCollisionInteraction()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}

