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

        public List<ItemsRequired> ItemsRequired { get; set; }


        public WorldQuest(int id, string description, List<ItemsRequired> itemsRequired)
        {
            this.ID = id;
            this.Description = description;
            this.ItemsRequired = itemsRequired;

        }
    }
}
