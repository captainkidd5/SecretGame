using Microsoft.Xna.Framework;
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
        public IntroduceSanctuary()
        {

            this.CharactersInvolved = new List<Character>()
            {
                Game1.Kaya
            };
            this.FreezePlayerControls = true;
            this.DayToTrigger = 0;
            this.StageToTrigger = (int)Stages.Sanctuary;
            this.IsCompleted = false;
            Game1.Kaya.CurrentStageLocation = (int)Stages.Sanctuary;
        }
        public void Update(GameTime gameTime)
        {
            Game1.Kaya.MoveToTile(gameTime, new Point(16, 16));
            //if(Game1.Kaya.Position == )
        }
    }
}
