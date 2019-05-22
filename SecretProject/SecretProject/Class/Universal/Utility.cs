﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Universal
{
    public class Utility
    {
        public int centerScreenX = 500;
        public int centerScreenY = 300;
        public Vector2 centerScreen;
        public Random RGenerator;

       // public static Color = new Color(100, 100, 100, 100);

        public Utility()
        {
            RGenerator = new Random();
            centerScreen = new Vector2(centerScreenX, centerScreenY);
        }

         

        public bool HasProperty(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        

        public int RNumber(int min, int max)
        {
            return RGenerator.Next(min, max - 1);
        }

    }
}
