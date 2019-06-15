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

        public string RetrieveDialogue(int speaker, int speechID)
        {
            DialogueHolder holder = Dialogue.Find(x => x.SpeakerID == speaker);
            DialogueSkeleton skeleton = holder.AllDialogue.Find((x => x.SpeechID == speechID));
            return skeleton.TextToWrite;
        }

    }
}
