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

        /// <summary>
        /// Returns true if time has reached target time.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
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
