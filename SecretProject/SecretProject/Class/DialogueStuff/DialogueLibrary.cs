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
        List<INPC> Characters;
        public List<DialogueSkeleton> Dialogue { get; set; }

        public DialogueLibrary(List<INPC> characters, List<DialogueSkeleton> dialogue)
        {
            this.Characters = characters;
            this.Dialogue = dialogue;
        }

        public string RetrieveDialogue(INPC characterToSpeak, int textID)
        {
            
        }

        public void LoadContent(ContentManager content)
        {

        }
    }
}
