using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XMLData.DialogueStuff
{
    public class DialogueHolder
    {
        public int SpeakerID { get; set; }
        public List<DialogueSkeleton> AllDialogue { get; set; }

        public void RemoveAllNewLines()
        {
            foreach(DialogueSkeleton skeleton in AllDialogue)
            {
                skeleton.TextToWrite = skeleton.TextToWrite.Replace("\r", "").Replace("\n", "");
            }
        }
    }
}
