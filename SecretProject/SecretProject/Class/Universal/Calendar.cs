using SecretProject.Class.SavingStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.RouteStuff;

namespace SecretProject.Class.Universal
{
    
    public class Calendar : ISaveable
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

        public void Save(BinaryWriter writer)
        {
            writer.Write(this.CurrentYear);
            writer.Write((int)this.CurrentMonth);
            writer.Write(this.CurrentDay);
        }

        public void Load(BinaryReader reader)
        {
            this.CurrentYear = reader.ReadInt32();
            this.CurrentMonth = (Month)reader.ReadInt32();
            this.CurrentDay = reader.ReadInt32();
        }
    }
}
