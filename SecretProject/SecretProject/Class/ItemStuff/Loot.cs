using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if(Game1.Utility.RGenerator.Next(0 , 101 - increasedLikelihood) < this.Probability)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
