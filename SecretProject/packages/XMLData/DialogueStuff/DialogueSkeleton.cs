using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.DialogueStuff
{
    public class DialogueSkeleton
    {
        //who will be speaking?
        public int SpeechID { get; set; }
        public bool ConditionMet { get; set; }
        public string TextToWrite { get; set; }
    }
}
