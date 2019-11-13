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
using XMLData.ItemStuff;

namespace SecretProject.Class.Universal
{
    public enum DayOfWeek
    {
        Monday = 1,
        Tuesday = 2, 
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7

    }
    public class Clock
    {
        public int GlobalTime { get;  set; } = 0;
        public Vector2 ClockPosition;

        //public TimeSpan UnpausedTime;
        public TimeSpan LocalTime;

        public int TotalHours { get; set; } = 6;
        public int TotalDays { get; set; } = 0;
        public float ColorMultiplier { get; set; } = 9f;

        public Color TimeOfDayColor { get; set; } = Color.White;

        public int DayTimeSoundEffectStart { get; set; } = 9;
        public int DayTimeSoundEffectEnd { get; set; } = 12;

        public int NightTimeSoundEffectStart { get; set; } = 12;
        public int NightTimeSoundEffectEnd { get; set; } = 14;
        public bool IsNight { get; set; }
        public float ClockSpeed { get; set; }
        TextBox ClockDisplay;

        public DayOfWeek WeekDay { get; set; }

        public event EventHandler DayChanged;

        public Clock()
        {
            ClockPosition = new Vector2(1100, 25);
           // UnpausedTime = TimeSpan.Zero;
            LocalTime = TimeSpan.Zero;
            WeekDay = DayOfWeek.Monday;
            ClockDisplay = new TextBox(Game1.AllTextures.MenuText, ClockPosition, GlobalTime.ToString() + "\n" + WeekDay.ToString(), Game1.AllTextures.UserInterfaceTileSet) { SourceRectangle = new Rectangle(432, 16, 80, 48) };

            ClockSpeed = 10;
            //this.DayChanged += Game1.World.AllTiles.HandleClockChange;


        }
        public virtual void OnDayChanged(Object sender, EventArgs e)
        {
            EventHandler handler = DayChanged;
            handler?.Invoke(this, e);
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
            
            Game1.SoundManager.PlaySoundEffectFromInt(1, roll, Game1.SoundManager.GameVolume);
        }
        public void IncrementDay()
        {
            this.TotalDays++;
            OnDayChanged(this, EventArgs.Empty);
        }
        public void Update(GameTime gameTime)
        {
            //UnpausedTime += gameTime.ElapsedGameTime;
            LocalTime += gameTime.ElapsedGameTime;

            if(LocalTime.TotalSeconds > ClockSpeed)
            {
                LocalTime = TimeSpan.Zero;
                TotalHours++;

                
                if(TotalHours > 18 && TotalHours < 23)
                {
                    ColorMultiplier-=3;
                }
                if(TotalHours > 2 && TotalHours < 18)
                {
                    PlayRandomInstance(1);
                    IsNight = false;
                }
                if(TotalHours < 2 || TotalHours > 21)
                {
                    PlayRandomInstance(2);
                    Game1.SoundManager.PlaySoundEffectFromInt(1, 12, Game1.SoundManager.GameVolume);
                    IsNight = true;
                }
                if(TotalHours < 7 && TotalHours > 2)
                {
                    
                    ColorMultiplier+=3;

                }
            }
            if(TotalHours > 23)
            {
                TotalDays++;
                if(WeekDay < DayOfWeek.Sunday)
                {
                    WeekDay++;
                }
                else
                {
                    WeekDay = DayOfWeek.Monday;
                }
                
                
                TotalHours = 0;
  
            }
            //int cleanTime = int.Parse(UnpausedTime.ToString());
            // GlobalTime += (int)gameTime.ElapsedGameTime.TotalSeconds;
            float testColor = (float)(1 + ColorMultiplier * .1);
            string displayString = "";
            if (TotalHours > 11)
            {
                displayString = (TotalHours - 12).ToString() + ":00 PM";
            }
            else if(TotalHours ==0)
            {
                displayString = "12:00 AM";
            }
            else
            {
                displayString = TotalHours.ToString() + ":00 AM";
            }
            ClockDisplay.TextToWrite = "      " + displayString + " \n Days: " + TotalDays.ToString() + "\n" + WeekDay.ToString();
            ClockDisplay.Update(gameTime, true);
            

            this.TimeOfDayColor = Color.DarkGray * (float)(1+ColorMultiplier * .1);
            

            

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ClockDisplay.Draw(spriteBatch, this.ClockPosition, this.ClockPosition, ClockDisplay.SourceRectangle,2f);
        }
    }
}
