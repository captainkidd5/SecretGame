using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Universal
{
    public static class Physics
    {
        public static Vector2 Gravity = new Vector2(0, -9.8f);
        public static Vector2 Eject(GameTime gameTime, Vector2 startingPosition, Vector2 velocity)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocity += Gravity * time;
            startingPosition += velocity * time;
            return startingPosition;
        }
    }
}
