using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

        public int DayTimeSoundEffectStart { get; set; } = 9;
        public int DayTimeSoundEffectEnd { get; set; } = 12;

        public int NightTimeSoundEffectStart { get; set; } = 12;
        public int NightTimeSoundEffectEnd { get; set; } = 14;

        TextBox ClockDisplay;

        public Clock()
        {
            ClockPosition = new Vector2(1100, 25);
           // UnpausedTime = TimeSpan.Zero;
            LocalTime = TimeSpan.Zero;
            ClockDisplay = new TextBox(Game1.AllTextures.MenuText, ClockPosition, GlobalTime.ToString(), Game1.AllTextures.UserInterfaceTileSet);



        }

        public void PlayRandomInstance(int timeOfDay) //1 = day, 2 = night
        {
            //picks a sound effect based on the switch statement in soundboard
            int roll = 0;
            if(timeOfDay == 1)
            {
                roll = Game1.Utility.RGenerator.Next(DayTimeSoundEffectStart, DayTimeSoundEffectEnd);
            }
            if(timeOfDay == 2)
            {
                roll = Game1.Utility.RGenerator.Next(NightTimeSoundEffectStart, NightTimeSoundEffectEnd);
            }
            
            Game1.SoundManager.PlaySoundEffectFromInt(false, 1, roll, 1f);
        }

        public void Update(GameTime gameTime)
        {
            //UnpausedTime += gameTime.ElapsedGameTime;
            LocalTime += gameTime.ElapsedGameTime;

            if(LocalTime.TotalSeconds > 8)
            {
                LocalTime = TimeSpan.Zero;
                TotalHours++;
                if(TotalHours > 18)
                {
                    ColorMultiplier-=3;
                }
                if(TotalHours > 5 && TotalHours < 18)
                {
                    PlayRandomInstance(1);
                }
                if(TotalHours < 2 || TotalHours > 20)
                {
                    PlayRandomInstance(2);
                }
                if(TotalHours < 7)
                {
                    
                    ColorMultiplier+=3;

                }
            }
            if(TotalHours > 23)
            {
                TotalDays++;
                TotalHours = 0;
  
            }
            //int cleanTime = int.Parse(UnpausedTime.ToString());
            // GlobalTime += (int)gameTime.ElapsedGameTime.TotalSeconds;
            float testColor = (float)(1 + ColorMultiplier * .1);
            ClockDisplay.TextToWrite = "Total Hours: " + TotalHours.ToString() + " \n Test: " + testColor.ToString();
            ClockDisplay.Update(gameTime, true);
            

            this.TimeOfDayColor = Color.DarkGray * (float)(1+ColorMultiplier * .1);
            

            

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ClockDisplay.Draw(spriteBatch);
        }
    }
}
