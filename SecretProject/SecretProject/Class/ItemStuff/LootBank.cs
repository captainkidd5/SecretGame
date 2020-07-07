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

        public Item GetandSpawnLootFromXML(int gid, Vector2 position, TileManager TileManager)
        {
            LootData data = LootInfo[gid];
            for (int i = 0; i < data.LootPieces.Count; i++)
            {
                if (data.LootPieces[i].Unlocked)
                {
                    for(int g =0; g < data.LootPieces[i].MinNumberToSpawn; g++)
                    {
                        Game1.ItemVault.GenerateNewItem(data.LootPieces[i].ItemToSpawnID, position, true, TileManager.AllItems);
                    }

                    for (int g = 0; g < data.LootPieces[i].MaxNumberToSpawn; g++) 
                    {
                        if (Game1.Utility.RNumber(0, 100) < data.LootPieces[i].ProbabilityAdditionalSpawn)
                        {
                            Game1.ItemVault.GenerateNewItem(data.LootPieces[i].ItemToSpawnID, position, true, TileManager.AllItems);
                        }
                        else
                        {
                            break;
                        }
                        
                    }


                }

            }
            return Game1.ItemVault.GenerateNewItem(data.LootPieces[0].ItemToSpawnID, null);

        }

        public Item GetLootFromTileset(int gid, Vector2 position, string lootString, TileManager TileManager)
        {
            int lootID = int.Parse(lootString);
            Game1.ItemVault.GenerateNewItem(lootID, position, true, TileManager.AllItems);
            // TileManager.AllItems.Add(Game1.ItemVault.GenerateNewItem(lootID, position, true));
            return Game1.ItemVault.GenerateNewItem(lootID, null);
        }
    }
}
