using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.ItemStuff.LootStuff;

namespace SecretProject.Class.ItemStuff
{
    public class LootBank
    {
        public Dictionary<int, LootData> LootInfo { get; set; }

        public LootBank(LootHolder lootHolder)
        {
            LootInfo = new Dictionary<int, LootData>();
            for(int i =0; i < lootHolder.AllLoot.Count; i++)
            {
                LootInfo.Add(lootHolder.AllLoot[i].GID, lootHolder.AllLoot[i]);

            }
        }
    }
}
