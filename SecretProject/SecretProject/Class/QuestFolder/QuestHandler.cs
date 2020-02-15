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
        public Quest ActiveQuest;

        public QuestHandler(QuestHolder questHolder)
        {
            this.QuestHolder = questHolder;
        }

        public Quest FetchCurrentQuest()
        {
            for(int i =0; i < QuestHolder.AllQuests.Count; i++)
            {
                if(!QuestHolder.AllQuests[i].Completed)
                {
                    this.ActiveQuest = QuestHolder.AllQuests[i];
                    return QuestHolder.AllQuests[i];
                }
            }
            return null;
        }

        public bool CheckActiveQuestState()
        {
            for(int i =0; i < this.ActiveQuest.ItemsRequired.Count; i++)
            {
                if(Game1.Player.UserInterface.BackPack.Inventory.ContainsAtLeastOne(this.ActiveQuest.ItemsRequired[i]))
                {

                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public void PerformReward(string rewardString)
        {
            string[] options = rewardString.Split(',');
           
          

            switch (options[0])
            {
                case "unlockCraftingRecipe":
                    Game1.Player.UserInterface.CraftingMenu.UnlockRecipe(int.Parse(options[0]));
                    break;

                case "unlockWorldLoot":
                    break;
            }
        }
    }
}
