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
        public Dictionary<int,WorldQuest> AllWorldQuests { get; set; }

        public WorldQuestHolder()
        {
            this.AllWorldQuests = new Dictionary<int, WorldQuest>();
            AllWorldQuests.Add(7039, new WorldQuest(7039, "Repair clock for", new List<ItemsRequired>()
                {
                    new ItemsRequired()
                    {
                        ItemID = 280,
                        Count = 25,
                    }

                }));
        }

        public WorldQuest RetrieveQuest(int id)
        {
            return this.AllWorldQuests[id];
        }


    }
}
