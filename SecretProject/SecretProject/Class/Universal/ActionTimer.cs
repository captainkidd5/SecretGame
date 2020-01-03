using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SecretProject.Class.Universal
{
    public class ActionTimer
    {
        public float TimeUntilAction { get; set; }
        public bool ActionComplete { get; set; } = false;

        public int Signature;

        private ActionTimer()
        {

        }
        //signature should be all actions count - 1
        public ActionTimer(float timeUntilAction, int signature)
        {
            this.TimeUntilAction = timeUntilAction;
            Game1.AllActions.Add(this);
            Signature = signature;
        }

        public void Update(GameTime gameTime, List<ActionTimer> actionTimers)
        {
            if (this.TimeUntilAction <= 0)
            {
                if (this.ActionComplete)
                {
                    actionTimers.Remove(this);
                }
                this.ActionComplete = true;

            }
            this.TimeUntilAction -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        }

    }
}
