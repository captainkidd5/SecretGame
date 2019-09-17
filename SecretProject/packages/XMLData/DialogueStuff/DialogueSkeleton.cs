using Microsoft.Xna.Framework.Content;
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
        public int Day { get; set; }
        public int TimeStart { get; set; }
        public int TimeEnd { get; set; }
        public string TextToWrite { get; set; }

        [ContentSerializer(Optional = true)]
        public string SelectableOptions { get; set; }

    }
}
