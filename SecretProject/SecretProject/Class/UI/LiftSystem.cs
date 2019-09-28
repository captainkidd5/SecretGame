using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public class LiftSystem
    {
        public List<Lift> AllLifts { get; set; }

    }
    public class Lift
    {
        public int Stage { get; set; }
        public Rectangle DestinationRectangle { get; set; }
    }
}
