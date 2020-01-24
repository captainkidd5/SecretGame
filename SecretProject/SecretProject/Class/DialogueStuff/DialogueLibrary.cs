using SecretProject.Class.NPCStuff;
using System.Collections.Generic;
using System.Linq;
using XMLData.DialogueStuff;
using XMLData.RouteStuff;

namespace SecretProject.Class.DialogueStuff
{
    public class DialogueLibrary
    {
        public List<DialogueHolder> Dialogue { get; set; }

        public DialogueLibrary(List<DialogueHolder> dialogue)
        {
            this.Dialogue = dialogue;

            
            
        }

        public void SortSkeletonsByTime()
        {
            foreach (DialogueHolder holder in this.Dialogue)
            {
                holder.AllDialogue = holder.AllDialogue.OrderBy(o => o.)
                }
        }

        public DialogueSkeleton RetrieveDialogue(Character character, Month month, int day, int time)
        {
            DialogueHolder holder = this.Dialogue.Find(x => x.SpeakerID == character.SpeakerID);
            DialogueDay dialogueDay = holder.AllDialogue.Find(x => x.Month == month && x.Day == day);
            DialogueSkeleton skeleton = dialogueDay.DialogueSkeletons.Find(x => x.Month == month &&
            x.Day == day && Game1.GlobalClock.GetTimeFromString(x.Time) == time);

            //if (skeleton == null)
            //{
            //    skeleton = holder.AllDialogue.Find(x => x.TimeStart <= time && x.TimeEnd <= x.TimeStart && x.Day == day);
            //}
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

        #region DATA STRUCTURE UTILITY
        //public DialogueSkeleton GetIndexClosestToButGreaterThanValue(DialogueHolder holder, Month month, int time)
        //{

        //}
        #endregion

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
