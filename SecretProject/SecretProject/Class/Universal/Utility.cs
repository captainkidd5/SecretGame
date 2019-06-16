using Microsoft.Xna.Framework;
using SecretProject.Class.ItemStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecretProject.Class.Universal
{
    public class Utility
    {
        public int CenterScreenX { get { return Game1.ScreenWidth / 2; } }
        public int CenterScreenY { get { return Game1.ScreenHeight / 2; } }
        public Vector2 centerScreen;
        public Random RGenerator;
        public Vector2 Origin { get; set; } = new Vector2(0, 0);
        public Vector2 TextBottomThird { get; set; } = new Vector2(Game1.ScreenWidth / 4 , Game1.ScreenHeight - Game1.ScreenHeight / 4);
        public Rectangle ItemSourceNullRectangle { get; set; } = new Rectangle(320, 320, 16, 16);


        public float StandardButtonDepth { get; set; } = .7f;
        public float StandardTextDepth { get; set; } = .72f;




        // public static Color = new Color(100, 100, 100, 100);

        public Utility()
        {
            RGenerator = new Random();
            centerScreen = new Vector2(CenterScreenX, CenterScreenY);
        }

         

        public bool HasProperty(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        

        public int RNumber(int min, int max)
        {
            return RGenerator.Next(min, max - 1);
        }

        public int[] ParseSpawnsWithKey(string gidString)
        {
            //Regex keyParser = new Regex(@"d{4}\,");
            string[] gids = gidString.Split(',');
            int[] arrayToReturn = new int[gids.Length];
            for(int i=0; i<gids.Length; i++)
            {
                arrayToReturn[i] = int.Parse(gids[i]);
            }

            return arrayToReturn;
        }

        //For use with the loot property of the tilesheet. Loot objects are separated by commas. The number before the colon is the item id and the number after is the probability of being dropped.
        public List<Loot> Parselootkey(string lootstring)
        {
            string[] commaPairs = lootstring.Split(',');
            int[] Ids = new int[commaPairs.Length];
            int[] probabilitys = new int[commaPairs.Length];
            List<Loot> lootToReturn = new List<Loot>();
            for (int i = 0; i < commaPairs.Length; i++)
            {
                
                
                Ids[i] = int.Parse(commaPairs[i].Split(':')[0]);
                probabilitys[i] = int.Parse(commaPairs[i].Split(':')[1]);
                lootToReturn.Add( new Loot() { ID = Ids[i], Probability = probabilitys[i] });
            }
            return lootToReturn;
        }

        public int DetermineLootDrop(Loot loot)
        {
            int amountToReturn = 0;
            int chance = RGenerator.Next(0, 100);
            if(chance <= loot.Probability)
            {
                amountToReturn++;
            }
            return amountToReturn;
        }

    }
}
