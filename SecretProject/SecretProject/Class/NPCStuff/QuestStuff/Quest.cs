using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.NPCStuff.QuestStuff
{
    public enum QuestType
    {
        Gather = 1,

    }
    public class Quest
    {
        public int ID { get; set; }
        public bool Completed { get; set; }
        public List<int> ItemsRequired;

        public Quest()
        {

        }

    }
}
