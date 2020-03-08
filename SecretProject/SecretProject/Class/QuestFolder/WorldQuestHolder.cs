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
            AllWorldQuests.Add(7039, new WorldQuest(7039, "Repair clock?", new List<ItemsRequired>()
                {
                    new ItemsRequired()
                    {
                        ItemID = 680,
                        Count = 2,
                        
                    }

                }, 7037)); //should be one larger

            AllWorldQuests.Add(6255, new WorldQuest(7039, "Repair windmill?", new List<ItemsRequired>()
                {
                    new ItemsRequired()
                    {
                        ItemID = 680,
                        Count = 2,

                    }

                }, 6253));
        }

        public WorldQuest RetrieveQuest(int id)
        {
            return this.AllWorldQuests[id];
        }

        public bool CheckIfRequirementsMet(WorldQuest quest)
        {
            for(int i =0; i < quest.ItemsRequired.Count; i++)
            {
                if(!Game1.Player.Inventory.ContainsAtLeastX(quest.ItemsRequired[i].ItemID, quest.ItemsRequired[i].Count))
                {
                    return false;
                }
            }

            return true;
            
        }

        public void RemoveItemsFromPlayerInventory(WorldQuest quest)
        {
            for (int i = 0; i < quest.ItemsRequired.Count; i++)
            {
                for(int j =0; j < quest.ItemsRequired[i].Count; j++)
                {
                    Game1.Player.Inventory.RemoveItem(quest.ItemsRequired[i].ItemID);
                }
                
            }
        }


    }
}
