using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.Transitions
{
    /// <summary>
    /// Used to create text on the screen directly after a new day has passed.
    /// </summary>
    public class DayTransitioner
    {
        public bool IsActive { get; set; }
        private Vector2 Position;
        public string Text { get; set; }
        private float Scale;

        public DayTransitioner()
        {
            this.Position = Game1.Utility.centerScreen;
            
            this.Scale = 3f;
        }

        public void UpdateText()
        {
            this.Text = Game1.GlobalClock.WeekDay.ToString() + ", " + Game1.GlobalClock.Calendar.CurrentMonth.ToString() + " " + Game1.GlobalClock.Calendar.CurrentDay.ToString() +
                ", Year " + Game1.GlobalClock.Calendar.CurrentYear.ToString();
            this.Position = Game1.Utility.CenterTextOnScreen(Game1.AllTextures.MenuText, this.Text, this.Scale);
           
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (IsActive)
            {


                spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Text, this.Position, color, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardTextDepth);
            }
        }
    }
}
