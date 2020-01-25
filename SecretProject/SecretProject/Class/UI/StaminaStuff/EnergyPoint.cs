using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.StaminaStuff
{
    public class EnergyPoint
    {
        public int MaximumEnergy { get; private set; }
        public int CurrentEnergy { get; private set; }

        public float ColorMultiplier { get; private set; }


        public Vector2 Position { get; private set; }

        public EnergyPoint(Vector2 position)
        {
            this.MaximumEnergy = 10;
            this.CurrentEnergy = 10;
            this.ColorMultiplier = 1f;
            this.Position = position;
        }


        public void Draw(SpriteBatch spriteBatch)
        {

        }

    }
}
