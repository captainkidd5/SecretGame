using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Transportation
{

    public class Lift
    {
        public int CurrentStage { get; set; }
        public Vector2 LocalPosition { get; set; }

        public Lift(int currentStage, Vector2 locationPosition)
        {
            this.CurrentStage = currentStage;
            this.LocalPosition = locationPosition;
        }

        public void Transport(Lift LiftToTransportTo)
        {
            Game1.SwitchStage(CurrentStage, LiftToTransportTo.CurrentStage);
            Game1.Player.Position = new Vector2(LiftToTransportTo.LocalPosition.X, LiftToTransportTo.LocalPosition.Y);
        }
    }
}
