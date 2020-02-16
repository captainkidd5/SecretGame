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

        public void PerformReward()
        {
            string[] options = ActiveQuest.UnlockData.Split(',');
           
          

            switch (options[0])
            {
                case "unlockCraftingRecipe":
                    Game1.Player.UserInterface.CraftingMenu.UnlockRecipe(int.Parse(options[1]));
                    ;
                    break;

                case "unlockWorldLoot":
                    int gidToUnlock = int.Parse(options[1]);
                    int lootIDToUnlock = int.Parse(options[2]);
                    Game1.LootBank.UnlockLootElement(gidToUnlock, lootIDToUnlock);
                    break;
            }

            Game1.Player.UserInterface.AddAlert(UI.AlertType.Normal, UI.AlertSize.XXL, new Microsoft.Xna.Framework.Vector2(Game1.Utility.ClockPosition.X - 64, Game1.Utility.ClockPosition.Y + 64), ActiveQuest.UnlockDescription);
        }
    }
}
