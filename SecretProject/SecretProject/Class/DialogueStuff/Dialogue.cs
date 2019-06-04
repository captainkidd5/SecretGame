using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.DialogueStuff
{
    public class Dialogue
    {
        //who will be speaking?
        public string Publisher { get; set; }
        
        public bool ConditionMet { get; set; }
        public string TextToWrite { get; set; }
    }
}
