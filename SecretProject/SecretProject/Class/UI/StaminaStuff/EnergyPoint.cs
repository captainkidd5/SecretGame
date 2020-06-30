using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Universal;
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
            this.MaximumEnergy = 50;
            this.CurrentEnergy = MaximumEnergy;
            this.ColorMultiplier = 1f;
            this.Position = position;
            this.EnergyRectangle = new Rectangle(224, 320, 16, 32);

        }

        public void CompletelyRefill()
        {
            this.CurrentEnergy = this.MaximumEnergy;
            this.IsDepleted = false;
        }
        public int IncreaseStamina(int amount)
        {
            this.CurrentEnergy += amount;
            Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.FoodBite, true, 1f);
            Game1.Player.UserInterface.AllRisingText.Add(new RisingText(Game1.Utility.centerScreen, Game1.Utility.CenterScreenY - 255, "+" + amount.ToString(), 400f, Color.White, false, 2f));
            if (this.CurrentEnergy > 0)
            {
                this.IsDepleted = false;
            }
            if(this.CurrentEnergy >= this.MaximumEnergy)
            {
                int amountToReturn = this.CurrentEnergy - this.MaximumEnergy;
                this.CurrentEnergy = this.MaximumEnergy;
                this.ColorMultiplier = this.CurrentEnergy * .1f;
                this.EnergyRectangle = new Rectangle(224, 320, 16, 32);
                return amountToReturn;
            }
            this.ColorMultiplier = this.CurrentEnergy * .1f;
            return 0;
        }

        public int DecreaseStamina(int amount)
        {
            this.CurrentEnergy -= amount;
            Game1.Player.UserInterface.AllRisingText.Add(new RisingText(Game1.Player.UserInterface.StaminaBar.StaminaStatus.Position, Game1.Player.UserInterface.StaminaBar.StaminaStatus.Position.Y - 100, "-", 100f, Color.White, false, 2f));
            if (this.CurrentEnergy <= 0)
            {
                this.IsDepleted = true;
                this.ColorMultiplier = 1f;
                this.EnergyRectangle = new Rectangle(240, 320, 16, 32);
                return Math.Abs(this.CurrentEnergy);
            }
            this.ColorMultiplier = this.CurrentEnergy * .1f;
            return 0; 
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (this.IsDepleted)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.EnergyRectangle, Color.White * this.ColorMultiplier, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Utility.StandardTextDepth);
            }
            else
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.EnergyRectangle, color * this.ColorMultiplier, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Utility.StandardTextDepth);
            }
            
        }

    }
}
