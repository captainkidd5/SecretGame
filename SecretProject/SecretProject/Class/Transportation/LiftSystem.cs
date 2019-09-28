using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Transportation
{
    public class LiftSystem
    {
        public List<Lift> AllLifts { get; set; }

    }
    public class Lift
    {
        public int Stage { get; set; }
        public Rectangle DestinationRectangle { get; set; }

        public void Transport(Lift LiftToTransportTo)
        {
            Game1.SwitchStage(Stage, LiftToTransportTo.Stage);
            Game1.Player.Position = new Vector2(LiftToTransportTo.DestinationRectangle.X, LiftToTransportTo.DestinationRectangle.Y);
        }
    }
}
