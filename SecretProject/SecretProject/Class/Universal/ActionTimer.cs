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

        public ActionTimer(float timeUntilAction)
        {
            this.TimeUntilAction = timeUntilAction;
            Game1.AllActions.Add(this);
        }

        public void Update(GameTime gameTime)
        {
            if(TimeUntilAction == 0)
            {
                ActionComplete = true;
                Game1.AllActions.Remove(this);
            }
            TimeUntilAction -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        }
    
    }
}
