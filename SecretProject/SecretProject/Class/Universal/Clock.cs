using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.UI;
using SecretProject.Class.Weather;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretProject.Class.Universal
{
    

    public class Clock
    {
        public int GlobalTime { get; set; } = 0;
        public Vector2 ClockPosition;

        //public TimeSpan UnpausedTime;
        public TimeSpan LocalTime;

        public int TotalHours { get; set; } = 5;
        public int TotalDays { get; set; } = 0;




        public int DayTimeSoundEffectStart { get; set; } = 9;
        public int DayTimeSoundEffectEnd { get; set; } = 12;

        public int NightTimeSoundEffectStart { get; set; } = 12;
        public int NightTimeSoundEffectEnd { get; set; } = 14;
        public bool IsNight { get; set; }
        public static float ClockMultiplier = 1f;
        const float BaseClockSpeed = 30f;
        public float ClockSpeed { get; set; }
        TextBox ClockDisplay;

        public DayOfWeek WeekDay { get; set; }

        public event EventHandler DayChanged;

        public Calendar Calendar { get; set; }

        public Clock()
        {
            ClockPosition = new Vector2(Game1.PresentationParameters.BackBufferWidth * .9f, Game1.PresentationParameters.BackBufferHeight * .1f);
            // UnpausedTime = TimeSpan.Zero;
            LocalTime = TimeSpan.Zero;
            this.WeekDay = DayOfWeek.Monday;
            ClockDisplay = new TextBox(Game1.AllTextures.MenuText, ClockPosition, this.GlobalTime.ToString() + "\n" + this.WeekDay.ToString(), Game1.AllTextures.UserInterfaceTileSet) { SourceRectangle = new Rectangle(432, 16, 80, 48) };

            this.ClockSpeed = 30f / ClockMultiplier;
            //this.DayChanged += Game1.World.AllTiles.HandleClockChange;
            //IsNight = true;
            this.Calendar = new Calendar();
            AdjustClockText();


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
            if (timeOfDay == 1)
            {
                roll = Game1.Utility.RGenerator.Next(this.DayTimeSoundEffectStart, this.DayTimeSoundEffectEnd);
            }
            if (timeOfDay == 2)
            {
                roll = Game1.Utility.RGenerator.Next(this.NightTimeSoundEffectStart, this.NightTimeSoundEffectEnd);
            }

            Game1.SoundManager.PlaySoundEffectFromInt(1, roll);
        }
        public void IncrementDay()
        {
            this.TotalDays++;
            if (this.WeekDay < DayOfWeek.Sunday)
            {
                this.WeekDay++;
            }
            else
            {
                this.WeekDay = DayOfWeek.Monday;
            }
            OnDayChanged(this, EventArgs.Empty);
            PickWeather();
            this.Calendar.IncrementCalendar();

            AdjustClockText();
            
        }

        public int GetTimeFromString(string time)
        {
            switch (time)
            {
                case "12AM":
                    return 0;
                case "1AM":
                    return 1;
                case "2AM":
                    return 2;
                case "3AM":
                    return 3;
                case "4AM":
                    return 4;
                case "5AM":
                    return 5;
                case "6AM":
                    return 6;
                case "7AM":
                    return 7;
                case "8AM":
                    return 8;
                case "9AM":
                    return 9;
                case "10AM":
                    return 10;
                case "11AM":
                    return 11;
                case "12PM":
                    return 12;
                case "1PM":
                    return 13;
                case "2PM":
                    return 14;
                case "3PM":
                    return 15;
                case "4PM":
                    return 16;
                case "5PM":
                    return 17;
                case "6PM":
                    return 18;
                case "7PM":
                    return 19;
                case "8PM":
                    return 20;
                case "9PM":
                    return 21;
                case "10PM":
                    return 22;
                case "11PM":
                    return 23;
                default:
                    return -1;

            }
        }

        public string GetStringFromTime()
        {
            switch (this.TotalHours)
            {
                case 0:
                    return "12AM";
                case 1:
                    return "1AM";
                case 2:
                    return "2AM";
                case 3:
                    return "3AM";
                case 4:
                    return "4AM";
                case 5:
                    return "5AM";
                case 6:
                    return "6AM";
                case 7:
                    return "7AM";
                case 8:
                    return "8AM";
                case 9:
                    return "9AM";
                case 10:
                    return "10AM";
                case 11:
                    return "11AM";
                case 12:
                    return "12PM";
                case 13:
                    return "1PM";
                case 14:
                    return "2PM";
                case 15:
                    return "3PM";
                case 16:
                    return "4PM";
                case 17:
                    return "5PM";
                case 18:
                    return "6PM";
                case 19:
                    return "7PM";
                case 20:
                    return "8PM";
                case 21:
                    return "9PM";
                case 22:
                    return "10PM";
                case 23:
                    return "11PM";
                default:
                    return "12AM";

            }
        }


        public void AdjustClockText()
        {
            string displayString = "";
            if (this.TotalHours > 11)
            {
                displayString = (this.TotalHours - 12).ToString() + ":00 PM";
            }
            else if (this.TotalHours == 0)
            {
                displayString = "12:00 AM";
            }
            else
            {
                displayString = this.TotalHours.ToString() + ":00 AM";
            }
            ClockDisplay.TextToWrite = "      " + displayString + " \n" + this.WeekDay.ToString() + 
            "\n " + this.Calendar.CurrentMonth.ToString() + ", " + this.Calendar.CurrentDay.ToString() 
            + "\n Year " + this.Calendar.CurrentYear.ToString();
        }

        public void PickWeather()
        {

            float totalSum = Game1.AllWeather.Sum(x => x.Value.ChanceToOccur);
            float selection = Game1.Utility.RFloat(0, totalSum);
            float sum = 0;
            foreach (KeyValuePair<WeatherType, IWeather> value in Game1.AllWeather)
            {
                if (selection <= (sum = sum + value.Value.ChanceToOccur))
                {
                    Game1.CurrentWeather = value.Value.WeatherType;
                    return;
                }
            }
        }


        public void Update(GameTime gameTime)
        {
            this.ClockSpeed = BaseClockSpeed / ClockMultiplier;
            //UnpausedTime += gameTime.ElapsedGameTime;
            LocalTime += gameTime.ElapsedGameTime;

            if (LocalTime.TotalSeconds > this.ClockSpeed)
            {
                LocalTime = TimeSpan.Zero;
                this.TotalHours++;



                if (this.TotalHours > 2 && this.TotalHours < 18)
                {
                    PlayRandomInstance(1);
                    this.IsNight = false;
                }
                if (this.TotalHours < 2 || this.TotalHours > 21)
                {
                    PlayRandomInstance(2);
                    Game1.SoundManager.PlaySoundEffectFromInt(1, 12);
                    this.IsNight = true;
                }

                AdjustClockText();

            }
            if (this.TotalHours > 23)
            {
                IncrementDay();


                this.TotalHours = 0;

            }

           
            ClockDisplay.Update(gameTime, true);






        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ClockDisplay.Draw(spriteBatch, true, 2f);
            //ClockDisplay.Draw(spriteBatch, this.ClockPosition, this.ClockPosition, ClockDisplay.SourceRectangle,2f);
        }
    }
}
