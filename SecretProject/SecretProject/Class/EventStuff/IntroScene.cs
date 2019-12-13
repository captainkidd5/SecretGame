using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.NPCStuff;

namespace SecretProject.Class.EventStuff
{
    class IntroScene : IEvent
    {
        public List<Character> CharactersInvolved { get; set; }
        public bool FreezePlayerControls { get; set; }
        public int DayToTrigger { get; set; }
        public int StageToTrigger { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsActive { get; set; }
        public int CurrentStep { get; set; }

        public IntroScene()
        {
            this.CharactersInvolved = new List<Character>()
            {
                Game1.Dobbin
            };
            FreezePlayerControls = false;
            this.DayToTrigger = 0;
            this.StageToTrigger = (int)Stages.World;
            this.IsCompleted = false;
            this.CurrentStep = 0;

        }
        public void Start()
        {
            if (!Game1.GetCurrentStage().CharactersPresent.Contains(Game1.Dobbin))
            {
                Game1.GetCurrentStage().CharactersPresent.Add(Game1.Dobbin);
            }

            Game1.Dobbin.CurrentStageLocation = Stages.World;
            Game1.Dobbin.IsInEvent = true;
            Game1.Dobbin.Position = new Vector2(128, 128);
            //  Game1.Dobbin.ResetPathFinding();
            this.FreezePlayerControls = true;
            this.IsActive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!Game1.freeze)
            {
                Game1.cam.Follow(new Vector2(Game1.Player.Position.X + 8, Game1.Player.Position.Y + 16), Game1.GetCurrentStage().MapRectangle);
                Game1.Player.Update(gameTime, Game1.GetCurrentStage().AllItems, Game1.myMouseManager);
                Game1.Dobbin.EventUpdate(gameTime);
            }

            if (Game1.Player.UserInterface.IsTransitioning)
            {
                Game1.Player.UserInterface.BeginTransitionCycle(gameTime);
            }

            Game1.Player.UserInterface.TextBuilder.Update(gameTime);
            switch (CurrentStep)
            {
                case 0:
                    Game1.Dobbin.EventMoveToTile(gameTime, new Point(8, 8));
                    if (Game1.Dobbin.EventCurrentPath.Count <= 0)
                    {
                        Game1.freeze = true;
                       // Game1.Dobbin.UpdateDirectionVector(Game1.Player.position);
                        Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Dobbin.Name + ": " + "Hmm... Where did I leave my seeds again?", 2f, null, null);
                        CurrentStep = 1;
                    }
                    break;
                case 1:
                    if (!Game1.Player.UserInterface.TextBuilder.IsActive)
                    {


                        Game1.Dobbin.EventMoveToTile(gameTime, new Point(1,1));
                        if (Game1.Dobbin.EventCurrentPath.Count <= 0)
                        {
                          //  Game1.Dobbin.UpdateDirectionVector(new Vector2(Game1.Dobbin.Position.X, Game1.Dobbin.Position.Y - 10));
                            Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Dobbin.Name + ": " +
                                "Hey, are you okay!?", 2f, null, null);
                            CurrentStep = 2;
                        }
                    }
                    break;
                case 2:
                    Game1.Dobbin.EventMoveToTile(gameTime, new Point(5, 5));
                    if (Game1.Dobbin.EventCurrentPath.Count <= 0)
                    {
                        Game1.Dobbin.UpdateDirectionVector(new Vector2(Game1.Player.Position.X, Game1.Player.Position.Y));
                        Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Dobbin.Name + ": " +
                            "Here, you better come with me", 2f, null, null);
                        CurrentStep = 3;
                    }

                    break;
                case 3:
                    Console.WriteLine("Event has ended");
                    Game1.IsEventActive = false;
                    this.IsActive = false;
                    this.IsCompleted = true;
                    Game1.Dobbin.IsInEvent = false;
                    break;

            }
        }
    }
}
