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
        public List<DialogueDay> AllDialogue { get; set; }
        
        

        public void RemoveAllNewLines()
        {
            foreach(DialogueDay day in AllDialogue)
            {
                foreach(DialogueSkeleton skeleton in day.DialogueSkeletons)
                {
                    skeleton.TextToWrite = skeleton.TextToWrite.Replace("\r", "").Replace("\n", "");
                }
                
            }
        }
    }
}
