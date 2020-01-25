﻿using SecretProject.Class.NPCStuff;
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
            //foreach (DialogueHolder holder in this.Dialogue)
            //{
            //    foreach (DialogueDay day in holder.AllDialogue)
            //    {
            //        day.DialogueSkeletons = day.DialogueSkeletons.OrderBy(Game1.GlobalClock.GetTimeFromString())
            //    }
            //}
        }

        /// <summary>
        /// Returns the dialogue skeleton which corresponds to the time. Skeleton will always default to the one which is greater or equal
        /// to the time given, but less than the next time slot. Will give the maximum time slot if less than the minimum
        /// </summary>
        /// <param name="character"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public DialogueSkeleton RetrieveDialogue(Character character, Month month, int day, string time)
        {
            DialogueHolder holder = this.Dialogue.Find(x => x.SpeakerID == character.SpeakerID);
            DialogueDay dialogueDay = holder.AllDialogue.Find(x => x.Month == month && x.Day == day);
            int skeletonIndex = dialogueDay.DialogueSkeletons.BinarySearch(new DialogueSkeleton { Time = time }, new TimeComparer());

            if (skeletonIndex < 0)
            {
                skeletonIndex = ~skeletonIndex - 1;
                if (skeletonIndex < 0)
                {
                    skeletonIndex = dialogueDay.DialogueSkeletons.Count - 1;
                }
            }



            DialogueSkeleton skeleton = dialogueDay.DialogueSkeletons[skeletonIndex];

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



    }

    public class TimeComparer : IComparer<DialogueSkeleton>
    {
        public int Compare(DialogueSkeleton x, DialogueSkeleton y)
        {
            return Game1.GlobalClock.GetTimeFromString(x.Time).CompareTo(Game1.GlobalClock.GetTimeFromString(y.Time));
        }
    }
}
