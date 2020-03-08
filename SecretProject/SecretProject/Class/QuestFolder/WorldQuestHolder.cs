using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.ItemStuff.CraftingStuff;

namespace SecretProject.Class.QuestFolder
{
    public class WorldQuestHolder
    {
        public List<WorldQuest> AllWorldQuests { get; set; }

        public WorldQuestHolder()
        {
            this.AllWorldQuests = new List<WorldQuest>()
            {
                new WorldQuest(0, "Repair clock for", new List<ItemsRequired>()
                {
                    new ItemsRequired()
                    {
                        ItemID = 280,
                        Count = 25,
                    }

                }),

            };
        }


    }
}
