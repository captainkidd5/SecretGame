using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SecretProject.Class.UI;

namespace SecretProject.Class.Universal
{
    public class Clock
    {
        public int GlobalTime { get; private set; } = 0;
        public Vector2 ClockPosition;

        public TimeSpan UnpausedTime;
        public TimeSpan LocalTime;

        public int TotalHours { get; set; } = 0;
        public int TotalDays { get; set; } = 0;

        TextBox ClockDisplay;

        public Clock()
        {
            ClockPosition = new Vector2(900, 100);
            UnpausedTime = TimeSpan.Zero;
            LocalTime = TimeSpan.Zero;
            ClockDisplay = new TextBox(Game1.AllTextures.MenuText, ClockPosition, GlobalTime.ToString(), Game1.AllTextures.ClockBackground);

        }

        public void Update(GameTime gameTime)
        {
            UnpausedTime += gameTime.ElapsedGameTime;
            LocalTime += gameTime.ElapsedGameTime;

            if(LocalTime.TotalSeconds > 10)
            {
                LocalTime = TimeSpan.Zero;
                TotalHours++;
            }
            if(TotalHours > 10)
            {
                TotalDays++;
            }
            //int cleanTime = int.Parse(UnpausedTime.ToString());
            // GlobalTime += (int)gameTime.ElapsedGameTime.TotalSeconds;
            ClockDisplay.TextToWrite = TotalHours.ToString();
            ClockDisplay.Update(gameTime, true);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ClockDisplay.Draw(spriteBatch);
        }
    }
}
