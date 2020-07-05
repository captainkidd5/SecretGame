using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;

namespace SecretProject.Class.EventStuff
{
    class MeetJulian : IEvent
    {
        public GraphicsDevice Graphics { get; set; }
        public List<Character> CharactersInvolved { get; set; }
        public bool FreezePlayerControls { get; set; }
        public int DayToTrigger { get; set; }
        public int StageToTrigger { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsActive { get; set; }
        public int CurrentStep { get; set; }
        public int TotalSteps { get; set; }
        Dictionary<int, bool> StepsCompleted;

        public SimpleTimer SimpleTimer { get; set; }



        public MeetJulian(GraphicsDevice graphics)
        {
            this.Graphics = graphics;
            this.CharactersInvolved = new List<Character>()
            {
                Game1.Dobbin,
                Game1.Julian
            };
            this.FreezePlayerControls = false;
            this.DayToTrigger = 0;
            this.StageToTrigger = (int)Stages.DobbinHouse;
            this.IsCompleted = false;
            this.CurrentStep = 0;
            this.TotalSteps = 11;
            StepsCompleted = new Dictionary<int, bool>();
            for (int i = 0; i < this.TotalSteps; i++)
            {
                StepsCompleted.Add(i, false);
            }

            this.SimpleTimer = new SimpleTimer(2f);


        }
        public void Start()
        {
            if (!Game1.CurrentStage.CharactersPresent.Contains(Game1.Dobbin))
            {
                Game1.CurrentStage.CharactersPresent.Add(Game1.Dobbin);
            }
            if (!Game1.CurrentStage.CharactersPresent.Contains(Game1.Julian))
            {
                Game1.CurrentStage.CharactersPresent.Add(Game1.Julian);
            }

            Game1.Dobbin.CurrentStageLocation = Stages.DobbinHouse;
            Game1.Dobbin.IsInEvent = true;
            Game1.Dobbin.DisableInteractions = false;

            Game1.Julian.CurrentStageLocation = Stages.DobbinHouse;
            Game1.Julian.IsInEvent = true;
            Game1.Julian.DisableInteractions = false;

            //  Game1.Dobbin.ResetPathFinding();
            this.IsActive = true;
            Game1.Player.IsDrawn = true;
            Game1.CurrentWeather = WeatherType.Rainy;
            Game1.EnableMusic = false;
            Game1.SoundManager.CurrentSongInstance.Stop();

            Game1.Dobbin.Position = new Vector2(610, 410);
            Game1.Dobbin.CurrentDirection = Dir.Left;
            Game1.Dobbin.IsMoving = false;
            Game1.Dobbin.ResetAnimations();

            Game1.Julian.Position = new Vector2(580, 410);
            Game1.Julian.CurrentDirection = Dir.Right;
            Game1.Julian.IsMoving = false;
            Game1.Julian.ResetAnimations();

            Game1.Player.UserInterface.LoadingScreen.BeginBlackTransition(.05f);



        }


        public void Update(GameTime gameTime)
        {

            if (!Game1.freeze)
            {


                // Game1.Dobbin.EventUpdate(gameTime);
                // Game1.Julian.EventUpdate(gameTime);
            }

            if (Game1.Player.UserInterface.LoadingScreen.IsTransitioning)
            {
                //Game1.Player.UserInterface.BlackTransition(gameTime);
            }
            Game1.Player.UserInterface.Update(gameTime,Game1.Player.Inventory);
            Game1.DobbinHouse.AllTiles.Update(gameTime, Game1.MouseManager);
            Game1.Player.UserInterface.CinematicMode = true;
            Game1.cam.pos = new Vector2(500, 430);
            Game1.Player.Direction = Dir.Right;
          //  Game1.Player.UpdateMovementAnimationsOnce();

            switch (this.CurrentStep)
            {
                case 0:



                    if (this.SimpleTimer.Run(gameTime))
                    {
                        StepsCompleted[this.CurrentStep] = true;
                        this.CurrentStep++;

                    }



                    break;

                case 1:
                    if (!StepsCompleted[this.CurrentStep])
                    {
                        //Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Dobbin.Name + ": " + "He said he doesn't remember anything..", 2f, null, null);
                        //StepsCompleted[this.CurrentStep] = true;
                    }
                    if (StepsCompleted[this.CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive)
                    {
                        this.CurrentStep++;
                    }



                    break;

                case 2:

                    if (!StepsCompleted[this.CurrentStep])
                    {
                        //Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Julian.Name + ": " + "How strange. Just lying face down in the dirt?", 2f, null, null);
                        //StepsCompleted[this.CurrentStep] = true;
                    }
                    if (StepsCompleted[this.CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive)
                    {
                        this.CurrentStep++;
                    }


                    break;

                case 3:
                    if (!StepsCompleted[this.CurrentStep])
                    {
                        //Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Dobbin.Name + ": " + "He's lucky I was there to bring him in.", 2f, null, null);
                        //StepsCompleted[this.CurrentStep] = true;
                    }
                    if (StepsCompleted[this.CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive)
                    {
                        this.CurrentStep++;
                        Game1.Dobbin.ActivateEmoticon(EmoticonType.Exclamation);
                        Game1.Julian.ActivateEmoticon(EmoticonType.Exclamation);
                    }
                    break;
                case 4:


                    Game1.Dobbin.CurrentDirection = Dir.Left;
                    Game1.Julian.CurrentDirection = Dir.Left;

                    if (!StepsCompleted[this.CurrentStep])
                    {

                        //Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Dobbin.Name + ": " +
                        //    "Oh you're up!", 2f, null, null);
                        StepsCompleted[this.CurrentStep] = true;
                    }

                    if (StepsCompleted[this.CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive)
                    {
                        this.CurrentStep++;
                        Game1.Dobbin.CurrentEmoticon = EmoticonType.None;
                        Game1.Julian.CurrentEmoticon = EmoticonType.None;
                        Game1.Dobbin.IsMoving = true;
                        Game1.Julian.IsMoving = true;
                    }


                    break;
                case 5:

                    Game1.Dobbin.Speed = 1f;
                    Game1.Dobbin.EventMoveToTile(gameTime, new Point(34, 25));
                    Game1.Julian.EventMoveToTile(gameTime, new Point(34, 27));
                    if (Game1.Dobbin.EventCurrentPath.Count <= 0 && Game1.Julian.EventCurrentPath.Count <= 0)
                    {
                        if (!StepsCompleted[this.CurrentStep])
                        {
                            Game1.Dobbin.ResetAnimations();
                            Game1.Julian.ResetAnimations();
                            //Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Dobbin.Name + ": " +
                            //    "This is Julian, he runs the workshop down the street.", 2f, null, null);
                            //StepsCompleted[this.CurrentStep] = true;
                        }
                    }

                    if (StepsCompleted[this.CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive)
                    {
                        this.CurrentStep++;
                    }
                    break;

                case 6:

                    if (!StepsCompleted[this.CurrentStep])
                    {
                        //Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Julian.Name + ": " +
                        //    "You must be very confused.. ", 2f, null, null);

                        StepsCompleted[this.CurrentStep] = true;
                    }

                    if (StepsCompleted[this.CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive && this.SimpleTimer.Run(gameTime))
                    {
                        this.CurrentStep++;
                    }




                    break;

                case 7:
                    if (!StepsCompleted[this.CurrentStep])
                    {
                        //Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Julian.Name + ": " +
                        //    "You're in [TOWNNAME], many of us in town are interested with what lies beyond the portal where you were found.", 2f, null, null);

                        StepsCompleted[this.CurrentStep] = true;
                    }

                    if (StepsCompleted[this.CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive && this.SimpleTimer.Run(gameTime))
                    {
                        this.CurrentStep++;
                    }

                    break;
                case 8:
                    if (!StepsCompleted[this.CurrentStep])
                    {
                       // Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Julian.Name + ": " +
                         //   "Many people in town would be happy to help gear you up to explore the wilderness if you gather resources for them.", 2f, null, null);

                        StepsCompleted[this.CurrentStep] = true;
                    }

                    if (StepsCompleted[this.CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive && this.SimpleTimer.Run(gameTime))
                    {
                        this.CurrentStep++;
                    }
                    break;
                case 9:
                    if (!StepsCompleted[this.CurrentStep])
                    {
                        //Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Julian.Name + ": " +
                        //    "Anyway, feel free to stop by my workshop any time. I'm just across the town square to the East.", 2f, null, null);

                        StepsCompleted[this.CurrentStep] = true;
                    }

                    if (StepsCompleted[this.CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive && this.SimpleTimer.Run(gameTime))
                    {
                        this.CurrentStep++;
                    }
                    break;
                case 10:

                    Console.WriteLine("Event has ended");
                    Game1.IsEventActive = false;
                    this.IsActive = false;
                    this.IsCompleted = true;
                    Game1.Dobbin.IsInEvent = false;
                    Game1.Julian.IsInEvent = false;
                    Game1.CurrentEvent = null;
                    Game1.Player.UserInterface.CinematicMode = false;

                    Game1.Player.IsDrawn = true;
                  //  Game1.Player.UpdateMovementAnimationsOnce();
                    break;

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: Game1.cam.getTransformation(this.Graphics));

            spriteBatch.End();
        }
    }
}
