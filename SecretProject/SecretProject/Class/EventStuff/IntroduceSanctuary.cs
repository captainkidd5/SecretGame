using Microsoft.Xna.Framework;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.NPCStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.EventStuff
{
    public class IntroduceSanctuary : IEvent
    {
        public List<Character> CharactersInvolved { get; set; }
        public bool FreezePlayerControls { get; set; }
        public int DayToTrigger { get; set; }
        public int StageToTrigger { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsActive { get; set; }
        public IntroduceSanctuary()
        {

            this.CharactersInvolved = new List<Character>()
            {
                Game1.Kaya
            };
            FreezePlayerControls = false;
            this.DayToTrigger = 0;
            this.StageToTrigger = (int)Stages.Sanctuary;
            this.IsCompleted = false;
            
        }
        public void Start()
        {
            Game1.GetCurrentStage().CharactersPresent.Add(Game1.Kaya);
            Game1.Kaya.CurrentStageLocation = (int)Stages.Sanctuary;
            Game1.Kaya.IsInEvent = true;
            Game1.Kaya.Position = new Vector2(560, 32);
            Game1.Kaya.pathFound = false;
            Game1.Kaya.pointCounter = 0;
            this.FreezePlayerControls = true;
            this.IsActive = true;
        }
        public void Update(GameTime gameTime)
        {
            Game1.cam.Follow(new Vector2(Game1.Player.Position.X + 8, Game1.Player.Position.Y + 16), Game1.GetCurrentStage().MapRectangle);
            Game1.Player.Update(gameTime, Game1.GetCurrentStage().AllItems, Game1.GetCurrentStage().AllObjects, Game1.myMouseManager);
            Game1.Kaya.EventUpdate(gameTime);
            Game1.Kaya.EventMoveToTile(gameTime, new Point(32, 16));
            if(Game1.Kaya.IsAtTile(new Point(32, 16)))
            {
                Game1.Player.UserInterface.TextBuilder.Activate(true, TextBoxType.dialogue, true, Game1.Kaya.Name + ": " + "Welcome to the sanctuary!", 2f, null, null);
                Console.WriteLine("Event has ended");
                Game1.IsEventActive = false;
                this.IsActive = false;
                this.IsCompleted = true;
            }
            //if(Game1.Kaya.Position == )
        }
    }
}
