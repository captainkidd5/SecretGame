using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.CraftingStuff
{
    public class ExternalToolTip
    {
        public Vector2 Position { get; set; }
        public int CurrentCount { get; set; }
        public int CountRequired { get; set; }

        public ExternalToolTip(Vector2 position)
        {
            this.Position = position;
        }

    }
}
