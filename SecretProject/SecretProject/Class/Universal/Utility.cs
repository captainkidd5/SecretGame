using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Universal
{
    public static class Utility
    {
        public static int centerScreenX = 500;
        public static int centerScreenY = 300;

        public static Vector2 centerScreen = new Vector2(centerScreenX, centerScreenY);

        public static bool HasProperty(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

    }
}
