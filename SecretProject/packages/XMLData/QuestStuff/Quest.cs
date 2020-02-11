using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.QuestStuff
{
    public class Quest
    {
        public int ID { get; set; }
        public List<int> ItemsRequired { get; set; }
        public int ItemUnlocked { get; set; }
        public string StartupSpeech { get; set; }
        public string CompletionSpeech { get; set; }
    }
}
