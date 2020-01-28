//using SecretProject.Class.NPCStuff;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using XMLData.RouteStuff;

//namespace SecretProject.Class.RouteStuff
//{
//    public class RouteLibrary
//    {
//        public List<RouteSchedule> RouteSchedules { get; set; }
//        public RouteLibrary(List<RouteSchedule> routeSchedules)
//        {
//            this.RouteSchedules = routeSchedules;



//        }

//        /// <summary>
//        /// Returns the dialogue skeleton which corresponds to the time. Skeleton will always default to the one which is greater or equal
//        /// to the time given, but less than the next time slot. Will give the maximum time slot if less than the minimum
//        /// </summary>
//        /// <param name="character"></param>
//        /// <param name="month"></param>
//        /// <param name="day"></param>
//        /// <param name="time"></param>
//        /// <returns></returns>
//        public Route RetrieveRoute(Character character, Month month, int day, string time)
//        {
//            DialogueHolder holder = this.Dialogue.Find(x => x.SpeakerID == character.SpeakerID);
//            DialogueDay dialogueDay = holder.AllDialogue.Find(x => x.Month == month && x.Day == day);
//            int skeletonIndex = dialogueDay.DialogueSkeletons.BinarySearch(new DialogueSkeleton { Time = time }, new TimeComparer());

//            if (skeletonIndex < 0)
//            {
//                skeletonIndex = ~skeletonIndex - 1;
//                if (skeletonIndex < 0)
//                {
//                    skeletonIndex = dialogueDay.DialogueSkeletons.Count - 1;
//                }
//            }



//            DialogueSkeleton skeleton = dialogueDay.DialogueSkeletons[skeletonIndex];

//            if (skeleton == null)
//            {
//                return new DialogueSkeleton() { TextToWrite = "error, error, ERROR!" };
//            }
//            if (skeleton.HasOccurredAtleastOnce)
//            {
//                if (skeleton.Once)
//                {
//                    skeleton = null;
//                    return skeleton;
//                }
//            }
//            else
//            {
//                skeleton.HasOccurredAtleastOnce = true;
//            }

//            return skeleton;
//        }



//    }

//    public class TimeComparer : IComparer<DialogueSkeleton>
//    {
//        public int Compare(DialogueSkeleton x, DialogueSkeleton y)
//        {
//            return Game1.GlobalClock.GetTimeFromString(x.Time).CompareTo(Game1.GlobalClock.GetTimeFromString(y.Time));
//        }
//    }
//}
//}
