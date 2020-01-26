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

        public Vector2 SimpleTimerDebugPosition { get; set; } = new Vector2(32, 200);
        public Vector2 LastEnergyLeft { get; set; } = new Vector2(32, 264);

        public Color StaminaEnergyColor { get; set; }

        public StaminaStatus StaminaStatus { get; set; }

        public StaminaBar(GraphicsDevice graphics, Vector2 energyPosition, int maximumStamina)
        {
            this.Graphics = graphics;
            this.EnergyPosition = energyPosition;
            this.MaximumStamina = maximumStamina;
            this.CurrentStamina = this.MaximumStamina;
            this.EnergyRectangle = new Rectangle(224, 320, 16, 32);
            this.EnergyPoints = new List<EnergyPoint>();
            this.StaminaDrainRate = 1f;
            this.StaminaDrainLimit = 10f;
            this.StaminaDrainTimer = new SimpleTimer(this.StaminaDrainLimit);


            for (int i = 0; i < this.MaximumStamina; i++)
            {
                this.EnergyPoints.Add(new EnergyPoint(new Vector2(this.EnergyPosition.X + 16 * i, this.EnergyPosition.Y)));
            }

            this.StaminaEnergyColor = Color.White;
            StaminaStatus = new StaminaStatus(new Vector2(this.EnergyPosition.X - 32, this.EnergyPosition.Y));
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsDraining)
            {
                if (StaminaDrainTimer.Run(gameTime, StaminaDrainRate))
                {
                    DecreaseStamina(1);

                }

            }
        }

        public float FetchStaminaDrainRate()
        {
            return Game1.Player.CalculateStaminaRateOfDrain();
        }

        public void IncreaseStamina(int amount)
        {
            int spillOverStamina = this.EnergyPoints[this.CurrentStamina - 1].IncreaseStamina(amount);
            if (spillOverStamina > 0)
            {
                if (this.CurrentStamina < this.MaximumStamina)
                {
                    this.CurrentStamina++;
                    this.EnergyPoints[this.CurrentStamina - 1].IncreaseStamina(spillOverStamina);
                    CheckStaminaEnergyColor();
                }

            }
        }

        public void CheckStaminaEnergyColor()
        {
            if (CurrentStamina > 4)
            {
                this.StaminaEnergyColor = Color.Green;
            }
            else if (CurrentStamina > 2 && CurrentStamina < 5)
            {
                this.StaminaEnergyColor = Color.Orange;
            }
            else
            {
                this.StaminaEnergyColor = Color.Red;
            }
        }

        public void DecreaseStamina(int amount)
        {
            //removing will give error, but do it to enable death mechanic
            if (CurrentStamina > 0)
            {


                int spillOverStamina = this.EnergyPoints[this.CurrentStamina - 1].DecreaseStamina(amount);
                if (spillOverStamina > 0)
                {
                    this.CurrentStamina--;
                    if (this.CurrentStamina <= 0)
                    {
                        this.IsStaminaDepleted = true;

                        return;
                    }

                    this.EnergyPoints[this.CurrentStamina - 1].DecreaseStamina(spillOverStamina);
                    CheckStaminaEnergyColor();

                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            this.StaminaStatus.Draw(spriteBatch);
            for (int i = 0; i < this.EnergyPoints.Count; i++)
            {
                this.EnergyPoints[i].Draw(spriteBatch,this.StaminaEnergyColor);
            }
            //if (CurrentStamina > 0)
            //{
            //    spriteBatch.DrawString(Game1.AllTextures.MenuText, "Stamina drain timer " + this.StaminaDrainTimer.Time.ToString(), SimpleTimerDebugPosition, Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, 1f);
            //    spriteBatch.DrawString(Game1.AllTextures.MenuText, "Energy left " + this.EnergyPoints[CurrentStamina - 1].CurrentEnergy.ToString(), LastEnergyLeft, Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, 1f);

            //}


        }
    }
}
