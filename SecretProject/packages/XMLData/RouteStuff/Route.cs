﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.RouteStuff
{
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
        SanctuaryHub = 10,
        Forest = 11,
        ResearchStation = 12,
        CaveWorld = 13,
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
            //switch (this.EndStage)
            //{
            //    case ("Town"):
            //        this.StageToEndAt = 0;
            //        break;
            //    case ("ElixirHouse"):
            //        this.StageToEndAt = 1;
            //        break;
            //    case ("JulianHouse"):
            //        this.StageToEndAt = 2;
            //        break;
            //    case ("World"):
            //        this.StageToEndAt = 3;
            //        break;
            //    case ("DobbinHouse"):
            //        this.StageToEndAt = 4;
            //        break;
            //    case ("PlayerHouse"):
            //        this.StageToEndAt = 5;
            //        break;
            //    case ("GeneralStore"):
            //        this.StageToEndAt = 6;
            //        break;
            //    case ("KayaHouse"):
            //        this.StageToEndAt = 7;
            //        break;
            //    case ("Cafe"):
            //        this.StageToEndAt = 8;
            //        break;



            //}
        }
    
        public int ID { get; set; }
        public int TimeToStart { get; set; }
        public int TimeToFinish { get; set; }
        public string EndStage { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }

        [ContentSerializer(Optional = true)]
        public int StageToEndAt { get; set; }

 
    }
}
