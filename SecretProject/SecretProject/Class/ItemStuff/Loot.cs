using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SecretProject.Class.ItemStuff
{
    public class Loot
    {

        public int ID { get; set; }
        public int Probability { get; set; }

        public Loot(int iD, int probability)
        {
            this.ID = iD;
            this.Probability = probability;
        }
        public Loot()
        {

        }

        public bool DidReceive(int increasedLikelihood = 0)
        {
            if (Game1.Utility.RGenerator.Next(0, 101 - increasedLikelihood) < this.Probability)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Item GetDrop(List<Loot> possibleLoot, Rectangle tileDestinationRectangle)
        {
            Item firstItemToReturn = null;
            if (possibleLoot != null)
            {
                firstItemToReturn = Game1.ItemVault.GenerateNewItem(possibleLoot[0].ID, null);

                for (int l = 0; l < possibleLoot.Count; l++)
                {
                    int lootCount = Loot.DetermineLootDrop(possibleLoot[l]);
                    for (int d = 0; d < lootCount; d++)
                    {
                        Item item = Game1.ItemVault.GenerateNewItem(possibleLoot[l].ID, new Vector2(tileDestinationRectangle.X, tileDestinationRectangle.Y), true);
                        StageManager.CurrentStage.AllTiles.AllItems.Add(item);
                    }
                }
            }
            return firstItemToReturn;
        }

        //For use with the loot property of the tilesheet. Loot objects are separated by commas. The number before the colon is the item id and the number after is the probability of being dropped.
        public static List<Loot> Parselootkey(string lootstring)
        {
            string[] commaPairs = lootstring.Split(',');
            int[] Ids = new int[commaPairs.Length];
            int[] probabilitys = new int[commaPairs.Length];
            int[] numberToSpawn = new int[commaPairs.Length];
            List<Loot> lootToReturn = new List<Loot>();
            for (int i = 0; i < commaPairs.Length; i++)
            {


                Ids[i] = int.Parse(commaPairs[i].Split(':')[0]);
                probabilitys[i] = int.Parse(commaPairs[i].Split(':')[1]);
                for (int j = 0; j < int.Parse(commaPairs[i].Split(':')[2]); j++)
                {
                    lootToReturn.Add(new Loot() { ID = Ids[i], Probability = probabilitys[i] });
                }

            }
            return lootToReturn;
        }

        public static int DetermineLootDrop(Loot loot)
        {
            int amountToReturn = 0;
            int chance = Game1.Utility.RGenerator.Next(0, 100);
            if (chance <= loot.Probability)
            {
                amountToReturn++;
            }
            return amountToReturn;
        }
    }
}
