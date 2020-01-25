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
        public bool IsDepleted { get; private set; }
        public int MaximumEnergy { get; private set; }
        public int CurrentEnergy { get; private set; }

        public float ColorMultiplier { get; private set; }


        public Vector2 Position { get; private set; }

        public Rectangle EnergyRectangle { get; private set; }


        public EnergyPoint(Vector2 position)
        {
            this.MaximumEnergy = 10;
            this.CurrentEnergy = 10;
            this.ColorMultiplier = 1f;
            this.Position = position;
            this.EnergyRectangle = new Rectangle(224, 320, 16, 32);

        }

        public int DecreaseStamina(int amount)
        {
            this.CurrentEnergy -= amount;
            if(this.CurrentEnergy <= 0)
            {
                this.IsDepleted = true;
                this.ColorMultiplier = 1f;
                this.EnergyRectangle = new Rectangle(240, 320, 16, 32);
                return Math.Abs(this.CurrentEnergy);
            }
            this.ColorMultiplier = this.CurrentEnergy * .1f;
            return 0; 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.EnergyRectangle, Color.White * this.ColorMultiplier, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardTextDepth);
        }

    }
}
