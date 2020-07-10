using Microsoft.Xna.Framework;

namespace SecretProject.Class.Universal
{
    public static class SecretPhysics
    {
        public static Vector2 Gravity = new Vector2(0, -9.8f);
        public static Vector2 Eject(GameTime gameTime, Vector2 startingPosition, Vector2 velocity)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocity += Gravity * time;
            startingPosition += velocity * time;
            return startingPosition;
        }

        public static Rectangle CreatePinBox(Vector2 entityPosition, int width, int height)
        {
            return new Rectangle((int)entityPosition.X, (int)entityPosition.Y, width, height);
        }

        public static Vector2 GetCenterOfPinBox(Rectangle pinBox)
        {
            return new Vector2(pinBox.X + pinBox.Width / 2, pinBox.Y + pinBox.Height / 2);
        }
    }
}
