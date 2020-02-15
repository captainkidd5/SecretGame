using Microsoft.Xna.Framework;
using SecretProject.Class.TileStuff;
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
            for (int i = 0; i < lootHolder.AllLoot.Count; i++)
            {
                LootInfo.Add(lootHolder.AllLoot[i].GID, lootHolder.AllLoot[i]);

            }
        }

        public bool UnlockLootElement(int gid, int itemIDToUnlock)
        {
            LootData data = LootInfo[gid];
            for (int i = 0; i < data.LootPieces.Count; i++)
            {
                if (data.LootPieces[i].ItemToSpawnID == itemIDToUnlock)
                {
                    data.LootPieces[i].Unlocked = true;
                    return true;
                }
            }
            return false;
        }

        public Item GetLootFromXML(int gid, Vector2 position, IInformationContainer container)
        {
            LootData data = LootInfo[gid];
            for (int i = 0; i < data.LootPieces.Count; i++)
            {
                if (data.LootPieces[i].Unlocked)
                {

                    Game1.ItemVault.GenerateNewItem(data.LootPieces[i].ItemToSpawnID, position, true, container.AllItems);



                }

            }
            return Game1.ItemVault.GenerateNewItem(data.LootPieces[0].ItemToSpawnID, null);

        }

        public Item GetLootFromTileset(int gid, Vector2 position, string lootString, IInformationContainer container)
        {
            int lootID = int.Parse(lootString);
            Game1.ItemVault.GenerateNewItem(lootID, position, true, container.AllItems);
            // container.AllItems.Add(Game1.ItemVault.GenerateNewItem(lootID, position, true));
            return Game1.ItemVault.GenerateNewItem(lootID, null);
        }
    }
}
