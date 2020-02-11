using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.RouteStuff;

namespace XMLData.DialogueStuff
{
    public class DialogueSkeleton
    {

        public int SpeechID { get; set; }
        public string Time { get; set; }
        public string TextToWrite { get; set; }


        [ContentSerializer(Optional = true)]
        public string SelectableOptions { get; set; }

        [ContentSerializer(Optional = true)]
        public string RequiredLocation { get; set; }

        [ContentSerializer(Optional = true)]
        public bool HasOccurredAtleastOnce { get; set; }

        [ContentSerializer(Optional = true)]
        public bool Once { get; set; }
    }
}
