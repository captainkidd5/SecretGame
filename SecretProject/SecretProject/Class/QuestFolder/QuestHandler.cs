using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.QuestStuff;

namespace SecretProject.Class.QuestFolder
{
    public class QuestHandler
    {
        public QuestHolder QuestHolder { get; set; }

        public QuestHandler(QuestHolder questHolder)
        {
            this.QuestHolder = questHolder;
        }
    }
}
