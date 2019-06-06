using Microsoft.Xna.Framework;
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
        public Rectangle ItemSourceNullRectangle { get; set; } = new Rectangle(320, 320, 16, 16);

        


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

    }
}
