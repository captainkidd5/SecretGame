using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.RouteStuff
{
    public class Route
    {
        public Route()
        {
            switch (this.EndStage)
            {
                case ("Town"):
                    this.StageToEndAt = 0;
                    break;
                case ("ElixirHouse"):
                    this.StageToEndAt = 0;
                    break;
                case ("JulianHouse"):
                    this.StageToEndAt = 0;
                    break;
                case ("World"):
                    this.StageToEndAt = 0;
                    break;
                case ("DobbinHouse"):
                    this.StageToEndAt = 0;
                    break;
                case ("PlayerHouse"):
                    this.StageToEndAt = 0;
                    break;

            }

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
