using SecretProject.Class.SavingStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.ItemStuff.CraftingStuff;

namespace SecretProject.Class.QuestFolder
{
    public class WorldQuestHolder : ISaveable
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

            AllWorldQuests.Add(6255, new WorldQuest(6255, "Repair windmill?", new List<ItemsRequired>()
                {
                    new ItemsRequired()
                    {
                        ItemID = 680,
                        Count = 2,

                    }

                }, 6253));

            AllWorldQuests.Add(9737, new WorldQuest(9737, "Repair water wheel?", new List<ItemsRequired>()
                {
                    new ItemsRequired()
                    {
                        ItemID = 680,
                        Count = 2,

                    }

                }, 9338));

            AllWorldQuests.Add(9510, new WorldQuest(9510, "Repair fountain?", new List<ItemsRequired>()
                {
                    new ItemsRequired()
                    {
                        ItemID = 680,
                        Count = 2,

                    }

                }, 8911));
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

        public void Save(BinaryWriter writer)
        {
            writer.Write(this.AllWorldQuests.Count);
            foreach (KeyValuePair<int, WorldQuest> quest in this.AllWorldQuests)
            {
                writer.Write(quest.Key);
                writer.Write(quest.Value.Completed);
            }
            }

        public void Load(BinaryReader reader)
        {
            int questCount = reader.ReadInt32();
            for(int i = 0; i < questCount; i++)
            {
                this.AllWorldQuests[reader.ReadInt32()].Completed = reader.ReadBoolean();
            }
        }
    }
}
