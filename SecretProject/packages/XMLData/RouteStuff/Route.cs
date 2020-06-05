﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.RouteStuff
{
    public enum Month
    {
        Spring = 0,
        Summer = 1,
        Fall = 2,
        Winter = 3
    }
    public enum WeekDay
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7

    }
    public enum Stages
    {
        Town = 0,
        ElixirHouse = 1,
        JulianHouse = 2,
        OverWorld = 3,
        DobbinHouse = 4,
        PlayerHouse = 5,
        GeneralStore = 6,
        KayaHouse = 7,
        Cafe = 8,

        DobbinHouseUpper = 9,
        MarcusHouse = 10,

        LightHouse = 11,
        UnderWorld = 12,
        CasparHouse = 13,
        MountainTop = 14,
        GisaardRanch = 15,
        MainMenu = 50,
        Exit = 55,


    }
    public class Route
    {
        public Route()
        {
            

        }

        public void ProcessStageToEndAt()
        {
            this.StageToEndAt = (int)Enum.Parse(typeof(Stages), this.EndStage);

        }
        
        public int ID { get; set; }
        public Month Month { get; set; }
        public WeekDay WeekDay { get; set; }
        public string Time { get; set; }
        public string EndStage { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }

        [ContentSerializer(Optional = true)]
        public int StageToEndAt { get; set; }

 
    }
}
