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
        public DialogueHolder Dialogue { get; set; }

        public DialogueLibrary(DialogueHolder dialogue)
        {
            this.Dialogue = dialogue;
        }

        public string RetrieveDialogue(int speechID)
        {
            DialogueSkeleton skeleton = Dialogue.AllDialogue.Find(x => x.SpeechID == speechID);
            return skeleton.TextToWrite;
        }

    }
}
