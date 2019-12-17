using Microsoft.Xna.Framework;
using SecretProject.Class.NPCStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.EventStuff
{
    public interface IEvent
    {
         List<Character> CharactersInvolved { get; set; }
         bool FreezePlayerControls { get; set; }
         int DayToTrigger { get; set; }
         int StageToTrigger { get; set; }
         bool IsCompleted { get; set; }
         bool IsActive { get; set; }
        int CurrentStep { get; set; }
        int TotalSteps { get; set; }
        void Start();
        void Update(GameTime gameTime);
    }
}
