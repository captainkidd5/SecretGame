using Microsoft.Xna.Framework;

namespace SecretProject.Class.Universal
{
    public class SimpleTimer
    {
        public float Time;
        public float TargetTime;

        public SimpleTimer(float targetTime)
        {
            Time = 0f;
            TargetTime = targetTime;
            ResetToZero();
        }


        public bool Run(GameTime gameTime, float multiplier = 1f)
        {
            Time += (float)gameTime.ElapsedGameTime.TotalSeconds * multiplier;
            return Test();
        }
        public bool Test()
        {
            if (Time >= TargetTime)
            {
                ResetToZero();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ResetToZero()
        {
            Time = 0f;
        }


    }
}
