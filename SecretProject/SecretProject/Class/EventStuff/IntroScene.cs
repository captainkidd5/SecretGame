using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.NPCStuff;
using SecretProject.Class.Universal;

namespace SecretProject.Class.EventStuff
{
    class IntroScene : IEvent
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

        public Vector2 PlayerBodyPosition { get; set; }

        public IntroScene(GraphicsDevice graphics)
        {
            this.Graphics = graphics;
            this.CharactersInvolved = new List<Character>()
            {
                Game1.Dobbin
            };
            FreezePlayerControls = false;
            this.DayToTrigger = 0;
            this.StageToTrigger = (int)Stages.OverWorld;
            this.IsCompleted = false;
            this.CurrentStep = 0;
            this.TotalSteps = 11;
            StepsCompleted = new Dictionary<int, bool>(); 
            for(int i =0; i < TotalSteps; i++)
            {
                StepsCompleted.Add(i, false);
            }

            SimpleTimer = new SimpleTimer(2f);

            PlayerBodyPosition = new Vector2(128, 180);

        }
        public void Start()
        {
            if (!Game1.GetCurrentStage().CharactersPresent.Contains(Game1.Dobbin))
            {
                Game1.GetCurrentStage().CharactersPresent.Add(Game1.Dobbin);
            }

            Game1.Dobbin.CurrentStageLocation = Stages.OverWorld;
            Game1.Dobbin.IsInEvent = true;
            
            //  Game1.Dobbin.ResetPathFinding();
            this.FreezePlayerControls = true;
            this.IsActive = true;
            Game1.Player.IsDrawn = false;
            Game1.CurrentWeather = WeatherType.Rainy;
            Game1.cam.Pos = new Vector2(128, 500);
            Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.Thunder1);
            Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.Downpour);
        }
        

        public void Update(GameTime gameTime)
        {
            if (!Game1.freeze)
            {
                
              //  Game1.cam.Follow(new Vector2(Game1.Dobbin.Position.X, Game1.Dobbin.Position.Y), Game1.GetCurrentStage().MapRectangle);
               // Game1.Player.Update(gameTime, Game1.GetCurrentStage().AllItems, Game1.myMouseManager);
                Game1.Dobbin.EventUpdate(gameTime);
            }

            if (Game1.Player.UserInterface.IsTransitioning)
            {
                Game1.Player.UserInterface.BeginTransitionCycle(gameTime);
            }

            //Game1.Player.UserInterface.TextBuilder.Update(gameTime);
            Game1.Player.UserInterface.Update(gameTime, Game1.OldKeyBoardState, Game1.NewKeyBoardState, Game1.Player.Inventory, Game1.myMouseManager);
            Game1.OverWorld.AllTiles.Update(gameTime, Game1.myMouseManager);
            Game1.AllWeather[Game1.CurrentWeather].Update(gameTime, StageFolder.LocationType.Exterior);
            Game1.Player.UserInterface.CinematicMode = true;
            
           
            switch (CurrentStep)
            {
                case 0:
                    if (Game1.cam.pos.Y > 180)
                    {
                        Game1.cam.pos = new Vector2(Game1.cam.pos.X, Game1.cam.pos.Y - .5f);

                    }
                    else
                    {
                        if(SimpleTimer.Run(gameTime))
                        {
                            StepsCompleted[CurrentStep] = true;
                            CurrentStep++;
                            Game1.Dobbin.Position = new Vector2(128, 0);
                        }
                    }

                    break;

                case 1:
                    
                   // Game1.Player.UserInterface.IsTransitioning = true;
                    Game1.Dobbin.EventMoveToTile(gameTime, new Point(8, 6));
                    if (Game1.Dobbin.EventCurrentPath.Count <= 0)
                    {
                        if(!StepsCompleted[CurrentStep])
                        {
                            Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Dobbin.Name + ": " + "Hmm... Where did I leave my dibber again?", 2f, null, null);
                            StepsCompleted[CurrentStep] = true;
                        }
                        if (StepsCompleted[CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive )
                        {
                            CurrentStep ++;
                        }

                    }
                   
                    break;

                case 2:
                    Game1.Dobbin.EventMoveToTile(gameTime, new Point(5,6));
                    if (Game1.Dobbin.EventCurrentPath.Count <= 0)
                    {
                        if (!StepsCompleted[CurrentStep])
                        {
                            Game1.Dobbin.IsMoving = false;
                            StepsCompleted[CurrentStep] = true;
                        }
                        if (StepsCompleted[CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive && SimpleTimer.Run(gameTime))
                        {
                            CurrentStep++;
                        }

                    }
                    break;

                case 3:
                    Game1.Dobbin.EventMoveToTile(gameTime, new Point(10,6));
                    if (Game1.Dobbin.EventCurrentPath.Count <= 0)
                    {
   
                        if (!StepsCompleted[CurrentStep])
                        {
                            Game1.Dobbin.IsMoving = false;
                            StepsCompleted[CurrentStep] = true;
                        }
                        if (StepsCompleted[CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive && SimpleTimer.Run(gameTime))
                        {
                            CurrentStep++;
                            
                            
                        }

                    }
                    break;
                case 4:
                    Game1.Dobbin.EventMoveToTile(gameTime, new Point(8, 6));
                    if (Game1.Dobbin.EventCurrentPath.Count <= 0)
                    {
                        Game1.Dobbin.CurrentDirection = Dir.Down;
                        if (SimpleTimer.Run(gameTime))
                        {
                            

                            if (!StepsCompleted[CurrentStep])
                            {
                                Game1.Dobbin.ActivateEmoticon(EmoticonType.Exclamation);
                                Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Dobbin.Name + ": " +
                                    "Hey, are you okay!?", 2f, null, null);
                                StepsCompleted[CurrentStep] = true;
                            }
                        }
                        if (StepsCompleted[CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive)
                        {
                            CurrentStep++;
                            Game1.Dobbin.CurrentEmoticon = EmoticonType.None;
                        }

                    }
                    break;
                case 5:
                    
                    Game1.Dobbin.Speed = 1f;
                        Game1.Dobbin.EventMoveToTile(gameTime, new Point(8,10));
                        if (Game1.Dobbin.EventCurrentPath.Count <= 0)
                        {
                        if (!StepsCompleted[CurrentStep])
                        {
                            Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Dobbin.Name + ": " +
                                "You'd better come with me!", 2f, null, null);
                            StepsCompleted[CurrentStep] = true;
                        }
                        
                            if(StepsCompleted[CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive )
                            {
                                CurrentStep  ++;
                            }
                        }
                    
                    break;
                case 6:
                    Game1.Dobbin.ResetSpeed();
                    Game1.Dobbin.EventMoveToTile(gameTime, new Point(8, 1));
                    PlayerBodyPosition = Game1.Dobbin.Position;
                    if (Game1.Dobbin.EventCurrentPath.Count <= 0)
                    {
                        if (!StepsCompleted[CurrentStep])
                        {
                            Game1.Dobbin.NPCAnimatedSprite[(int)Game1.Dobbin.CurrentDirection].SetFrame(0);

                            StepsCompleted[CurrentStep] = true;
                        }
                       
                        if (StepsCompleted[CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive && SimpleTimer.Run(gameTime))
                        {
                            CurrentStep ++;
                        }
                        
                            
                    }

                    break;

                case 7:
                    if(Game1.GetCurrentStageInt() != Stages.DobbinHouseUpper)
                    {
                        Game1.OverWorld.CharactersPresent.Clear();

                        Game1.SwitchStage(Stages.OverWorld, Stages.DobbinHouseUpper);
                        Game1.GetCurrentStage().CharactersPresent.Add(Game1.Dobbin);
                        //Game1.cam.pos = new Vector2(800, 480);
                        Game1.Player.position = Game1.Dobbin.Position;
                        Game1.Dobbin.Position = new Vector2(550, 500);
                        //Game1.cam.Follow(Game1.Dobbin.Position, Game1.GetCurrentStage().MapRectangle);
                        PlayerBodyPosition = Game1.Dobbin.Position;
                        Game1.Dobbin.CurrentStageLocation = Stages.DobbinHouseUpper;
                    }
                    Game1.Dobbin.EventMoveToTile(gameTime, new Point(35, 31));
                    PlayerBodyPosition = Game1.Dobbin.Position;
                    if (Game1.Dobbin.EventCurrentPath.Count <= 0)
                    {
                        if (!StepsCompleted[CurrentStep])
                        {
                            Game1.Dobbin.NPCAnimatedSprite[(int)Game1.Dobbin.CurrentDirection].SetFrame(0);
                            Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Dobbin.Name + ": " +
                                "Here you go.", 2f, null, null);
                            StepsCompleted[CurrentStep] = true;
                        }

                        if (StepsCompleted[CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive)
                        {
                            PlayerBodyPosition = new Vector2(580, 500);
                            CurrentStep++;
                        }


                    }

                    Game1.cam.pos = new Vector2(580, 500);
                    break;
                case 8:
                    Game1.cam.pos = new Vector2(580, 500);
                    if (!StepsCompleted[CurrentStep])
                    {
                        Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Dobbin.Name + ": " +
                            "I'll be back to check up on you. You're free to stay here as long as you like. Come downstairs when you're feeling better" +
                            " and we can try to figure out what's going on.", 2f, null, null);
                        StepsCompleted[CurrentStep] = true;
                    }

                    if (StepsCompleted[CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive)
                    {

                        CurrentStep++;
                    }
                    break;
                case 9:
                    Game1.cam.pos = new Vector2(580, 500);
                    Game1.Dobbin.EventMoveToTile(gameTime, new Point(33, 40));
                    if (Game1.Dobbin.EventCurrentPath.Count <= 0)
                    {
                        if (!StepsCompleted[CurrentStep])
                        {
                            StepsCompleted[CurrentStep] = true;
                            Game1.Dobbin.NPCAnimatedSprite[(int)Game1.Dobbin.CurrentDirection].SetFrame(0);
                        }

                        if (StepsCompleted[CurrentStep] && !Game1.Player.UserInterface.TextBuilder.IsActive)
                        {
                            CurrentStep++;
                            Game1.Player.UserInterface.TransitionSpeed = .005f;
                            Game1.Player.UserInterface.TransitionTimer.TargetTime = 6f;
                            Game1.Player.UserInterface.IsTransitioning = true;
                        }


                    }
                    break;
                case 10:
                    Game1.Player.position = new Vector2(560, 500);
                    Console.WriteLine("Event has ended");
                    Game1.IsEventActive = false;
                    this.IsActive = false;
                    this.IsCompleted = true;
                    Game1.Dobbin.IsInEvent = false;
                    Game1.CurrentEvent = null;
                    Game1.Player.UserInterface.CinematicMode = false;
                    Game1.GetCurrentStage().ActivateNewRisingText(Game1.Player.Rectangle.Y, Game1.Player.Rectangle.Y - 500, Game1.GlobalClock.WeekDay.ToString(), .015f, Color.White, true, 3f);
                    
                    Game1.Player.IsDrawn = true;
                    Game1.Player.UpdateMovementAnimationsOnce();
                    break;

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: Game1.cam.getTransformation(Graphics));
            spriteBatch.Draw(Game1.AllTextures.PlayerSilouhette, PlayerBodyPosition, null,  Color.White,0f, Game1.Utility.Origin, 1f, SpriteEffects.None, 1f);
            spriteBatch.End();
        }
    }
}
