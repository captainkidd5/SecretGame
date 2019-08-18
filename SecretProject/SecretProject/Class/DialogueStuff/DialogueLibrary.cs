using Microsoft.Xna.Framework.Content;
using SecretProject.Class.NPCStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.DialogueStuff;

namespace SecretProject.Class.DialogueStuff
{
    public class DialogueLibrary
    {
        public List<DialogueHolder> Dialogue { get; set; }

        public DialogueLibrary(List<DialogueHolder> dialogue)
        {
            this.Dialogue = dialogue;
        }

        public string RetrieveDialogue(int speaker, int time)
        {
            DialogueHolder holder = Dialogue.Find(x => x.SpeakerID == speaker);
            // DialogueSkeleton skeleton = holder.AllDialogue.Find((x => x.SpeechID == speechID));
            //Game1.GlobalClock.TotalHours >= route.TimeToStart && Game1.GlobalClock.TotalHours <= route.TimeToFinish ||
            //     Game1.GlobalClock.TotalHours >= route.TimeToStart && route.TimeToFinish <= route.TimeToStart)
            

            DialogueSkeleton skeleton = holder.AllDialogue.Find(x => x.TimeStart <= time && x.TimeEnd >= time);
            if(skeleton == null)
            {
                skeleton = holder.AllDialogue.Find(x => x.TimeStart <= time && x.TimeEnd <= x.TimeStart);
            }
            if(skeleton == null)
            {
                return "my speech doesn't exist at this time!";
            }
            return skeleton.TextToWrite;
        }

    }
}
