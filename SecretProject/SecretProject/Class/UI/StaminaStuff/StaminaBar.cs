using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Universal;
using System.Collections.Generic;

namespace SecretProject.Class.UI.StaminaStuff
{
    public class StaminaBar
    {
        public bool IsDraining { get; set; }
        public GraphicsDevice Graphics { get; set; }
        public int MaximumStamina { get; private set; }
        public int CurrentStamina { get; private set; }
        public bool IsStaminaDepleted { get; private set; }
        public Rectangle EnergyRectangle { get; private set; }
        public Vector2 EnergyPosition { get; set; }
        

        public List<EnergyPoint> EnergyPoints { get; set; }

        public float StaminaDrainLimit { get; set; }
        public float StaminaDrainRate { get; set; }
        public SimpleTimer StaminaDrainTimer { get; set; }
       

        public StaminaBar(GraphicsDevice graphics, Vector2 energyPosition, int maximumStamina)
        {
            this.Graphics = graphics;
            this.EnergyPosition = energyPosition;
           this.MaximumStamina = maximumStamina;
            this.CurrentStamina = this.MaximumStamina;
            this.EnergyRectangle = new Rectangle(224, 320, 16, 32);
            this.EnergyPoints = new List<EnergyPoint>();
            this.StaminaDrainRate = 1f;
            this.StaminaDrainLimit = 100f;
            this.StaminaDrainTimer = new SimpleTimer(this.StaminaDrainLimit);
            

            for(int i =0; i < this.MaximumStamina; i++)
            {
                this.EnergyPoints.Add(new EnergyPoint(new Vector2(this.EnergyPosition.X + 16 * i, this.EnergyPosition.Y))); 
            }
           
        }

        public void Update(GameTime gameTime)
        {
            if(this.IsDraining)
            {
                if(StaminaDrainTimer.Run(gameTime, StaminaDrainRate))
                {
                    DecreaseStamina(1);
                    
                }

            }
        }

        public float FetchStaminaDrainRate()
        {
            return Game1.Player.CalculateStaminaRateOfDrain();
        }

        public void DecreaseStamina(int amount)
        {
            int spillOverStamina = this.EnergyPoints[this.CurrentStamina - 1].DecreaseStamina(amount);
            if (spillOverStamina > 0)
            {
                this.CurrentStamina--;
                if(this.CurrentStamina <= 0)
                {
                    this.IsStaminaDepleted = true;

                    return;
                }

                    this.EnergyPoints[this.CurrentStamina -1].DecreaseStamina(spillOverStamina);
                
            }
         }


        public void Draw(SpriteBatch spriteBatch)
        {

                for (int i = 0; i < this.EnergyPoints.Count; i++)
                {
                    this.EnergyPoints[i].Draw(spriteBatch);
                }
            
        }
    }
}
