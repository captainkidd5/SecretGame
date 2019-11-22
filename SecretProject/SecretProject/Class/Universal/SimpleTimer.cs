using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Universal
{
    public class SimpleTimer
    {
        public float Time;
        public float TargetTime;

        public SimpleTimer(float targetTime)
        {
            this.Time = 0f;
            this.TargetTime = targetTime;
            ResetToZero();
        }


        public bool Run(GameTime gameTime)
        {
            Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            return Test();
        }
        public bool Test()
        {
            if(this.Time == TargetTime)
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
            this.Time = 0f;
        }

        
    }
}
