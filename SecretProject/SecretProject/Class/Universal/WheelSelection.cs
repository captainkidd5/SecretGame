using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Universal
{
    public static class WheelSelection
    {
        public static IWeightable GetSelection(List<IWeightable> myList)
        {
            int poolSize = 0;
            for (int i = 0; i < myList.Count; i++)
            {
                poolSize += myList[i].Chance;
            }

            // Get a random integer from 0 to PoolSize.
            int randomNumber = Game1.Utility.RGenerator.Next(0, poolSize) + 1;

            // Detect the item, which corresponds to current random number.
            int accumulatedProbability = 0;
            for (int i = 0; i < myList.Count; i++)
            {
                accumulatedProbability += myList[i].Chance;
                if (randomNumber <= accumulatedProbability)
                    return myList[i];
            }
            return null;
        }
    }
}
