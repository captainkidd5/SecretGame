﻿using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.DialogueStuff;

namespace XMLData.QuestStuff
{
    public class Quest
    {
        public string QuestName { get; set; }
        public int QuestID { get; set; }

        [ContentSerializer(Optional = true)]
        public bool Completed { get; set; }

        public List<int> ItemsRequired { get; set; }

        [ContentSerializer(Optional = true)]
        public string UnlockData { get; set; }
        public string StartupSpeech { get; set; }
        public DialogueSkeleton MidQuestSkeleton { get; set; }
        public string CompletionSpeech { get; set; }
    }
}
