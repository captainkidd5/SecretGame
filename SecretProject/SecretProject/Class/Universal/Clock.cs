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
        public int GlobalTime { get;  set; } = 0;
        public Vector2 ClockPosition;

        //public TimeSpan UnpausedTime;
        public TimeSpan LocalTime;

        public int TotalHours { get; set; } = 9;
        public int TotalDays { get; set; } = 0;
        public float ColorMultiplier { get; set; } = 9f;

        public Color TimeOfDayColor { get; set; } = Color.White;

        TextBox ClockDisplay;

        public Clock()
        {
            ClockPosition = new Vector2(1100, 25);
           // UnpausedTime = TimeSpan.Zero;
            LocalTime = TimeSpan.Zero;
            ClockDisplay = new TextBox(Game1.AllTextures.MenuText, ClockPosition, GlobalTime.ToString(), Game1.AllTextures.UserInterfaceTileSet);

        }

        public void Update(GameTime gameTime)
        {
            //UnpausedTime += gameTime.ElapsedGameTime;
            LocalTime += gameTime.ElapsedGameTime;

            if(LocalTime.TotalSeconds > 10)
            {
                LocalTime = TimeSpan.Zero;
                TotalHours++;
                if(TotalHours > 18)
                {
                    ColorMultiplier--;
                }
                if(TotalHours < 6)
                {
                    ColorMultiplier++;
                }
            }
            if(TotalHours > 23)
            {
                TotalDays++;
                TotalHours = 0;
  
            }
            //int cleanTime = int.Parse(UnpausedTime.ToString());
            // GlobalTime += (int)gameTime.ElapsedGameTime.TotalSeconds;
            ClockDisplay.TextToWrite = "Total Hours: " + TotalHours.ToString() + " \n Total Days: " + TotalDays.ToString();
            ClockDisplay.Update(gameTime, true);

                
                this.TimeOfDayColor = Color.DarkGray * (float)(1+ColorMultiplier * .1);
            

            

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ClockDisplay.Draw(spriteBatch);
        }
    }
}
