using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Universal
{
    public enum Month
    {
        Spring = 0,
        Summer = 1,
        Fall = 2,
        Winter = 3
    }
    public class Calendar
    {
        public int CurrentYear { get; private set; }
        public Month CurrentMonth { get; private set; }
        public int CurrentDay { get; private set; }
        public int DaysPerMonth { get; private set; }

        public int TotalDaysPlayed { get; private set; }

        public Calendar()
        {
            this.CurrentYear = 1;
            this.CurrentDay = 1;
            this.DaysPerMonth = 30;
            this.CurrentMonth = Month.Spring;
        }

        public void IncrementCalendar()
        {
            this.CurrentDay++;
            if(this.CurrentDay > this.DaysPerMonth)
            {
                if(CurrentMonth == Month.Winter)
                {
                    this.CurrentYear++;
                }
                this.CurrentDay = 1;
                if(CurrentMonth == Month.Winter)
                {
                    CurrentMonth = Month.Spring;
                }
                else
                {
                    this.CurrentMonth++;
                }
               
                
            }
            this.TotalDaysPlayed++;
        }
    }
}
