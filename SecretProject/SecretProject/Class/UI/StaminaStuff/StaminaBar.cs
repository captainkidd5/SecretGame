using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Universal;
using System.Collections.Generic;

namespace SecretProject.Class.UI.StaminaStuff
{
    public class StaminaBar
    {
        public GraphicsDevice Graphics { get; set; }
        public int MaximumStamina { get; private set; }
        public Rectangle EnergyRectangle { get; private set; }
        public Vector2 EnergyPosition { get; set; }

        public List<EnergyPoint> EnergyPoints { get; set; }



        public StaminaBar(GraphicsDevice graphics, Vector2 energyPosition, int maximumStamina, float decayRate)
        {
            this.Graphics = graphics;
            this.EnergyPosition = energyPosition;
           this.MaximumStamina = maximumStamina;
            this.EnergyRectangle = new Rectangle(224, 320, 16, 32);
            this.EnergyPoints = new List<EnergyPoint>();
            for(int i =0; i < this.MaximumStamina; i++)
            {
                this.EnergyPoints.Add(new EnergyPoint(new Vector2(this.EnergyPosition.X + 16 * i, this.EnergyPosition.Y))); 
            }
           
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (Game1.GetCurrentStageInt() == Stages.OverWorld)
            {
                for (int i = 0; i < this.EnergyPoints.Count; i++)
                {
                    this.EnergyPoints[i].Draw(spriteBatch);
                }
            }
        }
    }
}
