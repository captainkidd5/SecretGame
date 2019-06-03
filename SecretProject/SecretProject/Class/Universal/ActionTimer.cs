using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
            this.Signature = signature;
        }

        public void Update(GameTime gameTime, List<ActionTimer> actionTimers)
        {
            if(TimeUntilAction <= 0)
            {
                if(ActionComplete)
                {
                    actionTimers.Remove(this);
                }
                ActionComplete = true;
               
            }
            TimeUntilAction -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        }
    
    }
}
