using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;
using System.Collections.Generic;
using System.Linq;
using XMLData.DialogueStuff;
using XMLData.RouteStuff;

namespace SecretProject.Class.DialogueStuff
{
    public class DialogueManager : Component
    {
        public List<DialogueHolder> Dialogue { get; set; }

        public DialogueHolder ElixirDialogue { get; private set; }
        public DialogueHolder DobbinDialogue { get; private set; }
        public DialogueHolder SnawDialogue { get; private set; }
        public DialogueHolder KayaDialogue { get; private set; }
        public DialogueHolder JulianDialogue { get; private set; }
        public DialogueHolder SarahDialogue { get; private set; }
        public DialogueHolder BusinessSnailDialogue { get; private set; }
        public DialogueHolder MippinDialogue { get; private set; }
        public DialogueHolder NedDialogue { get; private set; }
        public DialogueHolder TealDialogue { get; private set; }
        public DialogueHolder MarcusDialogue { get; private set; }
        public DialogueHolder CasparDialogue { get; private set; }
        public DialogueManager(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
        }


        public override void Load()
        {
            ElixirDialogue = this.content.Load<DialogueHolder>("Dialogue/ElixirDialogue");
            DobbinDialogue = this.content.Load<DialogueHolder>("Dialogue/DobbinDialogue");
            SnawDialogue = this.content.Load<DialogueHolder>("Dialogue/SnawDialogue");
            KayaDialogue = this.content.Load<DialogueHolder>("Dialogue/KayaDialogue");
            JulianDialogue = this.content.Load<DialogueHolder>("Dialogue/JulianDialogue");
            SarahDialogue = this.content.Load<DialogueHolder>("Dialogue/SarahDialogue");
            BusinessSnailDialogue = this.content.Load<DialogueHolder>("Dialogue/BusinessSnailDialogue");
            MippinDialogue = this.content.Load<DialogueHolder>("Dialogue/MippinDialogue");
            NedDialogue = this.content.Load<DialogueHolder>("Dialogue/NedDialogue");
            TealDialogue = this.content.Load<DialogueHolder>("Dialogue/TealDialogue");
            MarcusDialogue = this.content.Load<DialogueHolder>("Dialogue/MarcusDialogue");
            CasparDialogue = this.content.Load<DialogueHolder>("Dialogue/CasparDialogue");




            Dialogue = new List<DialogueHolder>() { ElixirDialogue, DobbinDialogue, SnawDialogue, KayaDialogue,
                JulianDialogue, SarahDialogue,
                BusinessSnailDialogue, MippinDialogue, NedDialogue, TealDialogue, MarcusDialogue,CasparDialogue };
            foreach (DialogueHolder holder in Dialogue)
            {
                holder.RemoveAllNewLines();
            }


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
            if(dialogueDay == null)
            {
                return new DialogueSkeleton() { TextToWrite = "Dialogue hasn't been created for me at this time!" };
            }
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
                return new DialogueSkeleton() { TextToWrite = "Dialogue hasn't been created for me at this time!" };
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



        public override void Unload()
        {
            throw new System.NotImplementedException();
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
