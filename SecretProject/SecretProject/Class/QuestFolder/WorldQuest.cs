using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.ItemStuff.CraftingStuff;

namespace SecretProject.Class.QuestFolder
{
    public class WorldQuest
    {
        public int ID { get; set; }
        public bool Completed { get; set; }

        public string Description { get; set; }
        public string RewardDescription { get; set; }

        public int ReplacementGID { get; set; }

        public List<ItemsRequired> ItemsRequired { get; set; }


        public WorldQuest(int gid, string description, string rewardDescription, List<ItemsRequired> itemsRequired, int replacementGID)
        {
            this.ID = gid;
            this.Description = description;
            this.RewardDescription = rewardDescription;
            this.ItemsRequired = itemsRequired;
            this.ReplacementGID = replacementGID;

        }
    }
}
