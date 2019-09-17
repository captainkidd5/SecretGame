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

        public DialogueSkeleton RetrieveDialogue(int speaker,int day, int time)
        {
            DialogueHolder holder = Dialogue.Find(x => x.SpeakerID == speaker);
            

            DialogueSkeleton skeleton = holder.AllDialogue.Find(x => x.TimeStart <= time && x.TimeEnd >= time && x.Day == day);
            if(skeleton == null)
            {
                skeleton = holder.AllDialogue.Find(x => x.TimeStart <= time && x.TimeEnd <= x.TimeStart && x.Day == day);
            }
            if(skeleton == null)
            {
                return new DialogueSkeleton() { TextToWrite = "error, error, ERROR!" };
            }
            return skeleton;
        }

    }
}
