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
        float timeUntilAction;
        float timer = 0;

        public ActionToPerform Action { get; set; }

        public delegate void ActionToPerform();

        public ActionTimer(float timeUntilAction, ActionToPerform methodToExecute)
        {
            Action = methodToExecute;
            timer = timeUntilAction;
        }

        public void Update(GameTime gameTime)
        {
            timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(timer <= 0)
            {
                Action();
                Game1.GetCurrentStage().AllActions.Remove(this);
            }
        }
    }
}
