namespace SecretProject.Class.NPCStuff
{
    public class ResearchAssignment
    {
        public int ID { get; set; }
        public int DaysUntilCompletion { get; set; }
        public bool Complete { get; set; }
        public bool Claimed { get; set; }

        public ResearchAssignment(int iD, int daysUntilCompletion)
        {
            this.ID = iD;
            this.DaysUntilCompletion = daysUntilCompletion;
        }

        public bool ContinueResearch()
        {
            this.DaysUntilCompletion--;
            if (this.DaysUntilCompletion <= 0)
            {
                this.Complete = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
