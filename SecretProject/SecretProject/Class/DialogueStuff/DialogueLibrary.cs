using SecretProject.Class.NPCStuff;
using System.Collections.Generic;
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

        public DialogueSkeleton RetrieveDialogue(Character character, int day, int time)
        {
            DialogueHolder holder = this.Dialogue.Find(x => x.SpeakerID == character.SpeakerID);


            DialogueSkeleton skeleton = holder.AllDialogue.Find(x => x.TimeStart <= time && x.TimeEnd >= time && x.Day == day);
            if (skeleton == null)
            {
                skeleton = holder.AllDialogue.Find(x => x.TimeStart <= time && x.TimeEnd <= x.TimeStart && x.Day == day);
            }
            if (skeleton == null)
            {
                return new DialogueSkeleton() { TextToWrite = "error, error, ERROR!" };
            }
            if (skeleton.HasOccurredAtleastOnce)
            {
                if (skeleton.Once)
                {
                    skeleton = null;
                    return skeleton;
                }
            }
            else
            {
                skeleton.HasOccurredAtleastOnce = true;
            }

            return skeleton;
        }

        public DialogueSkeleton RetrieveDialogueNoTime(int speaker, int id)
        {
            DialogueHolder holder = this.Dialogue.Find(x => x.SpeakerID == speaker);


            DialogueSkeleton skeleton = holder.AllDialogue.Find(x => x.SpeechID == id);
            if (skeleton == null)
            {
                return new DialogueSkeleton() { TextToWrite = "error, error, ERROR!" };
            }
            return skeleton;
        }

    }
}
