using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.NPCStuff
{
    public class ResearchAssignment
    {
        public int ID { get; set; }
        public int DaysUntilCompletion { get; set; }
        public bool Complete { get; set; }

        public ResearchAssignment(int iD, int daysUntilCompletion)
        {
            this.ID = iD;
            this.DaysUntilCompletion = daysUntilCompletion;
        }

        public bool ContinueResearch()
        {
            DaysUntilCompletion--;
            if(DaysUntilCompletion <= 0)
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
