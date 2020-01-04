using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLData.SanctuaryStuff
{

    public class SanctuaryRequirement
    {
        public int Tab { get; set; }
        public int ItemID { get; set; }
        public int GIDRequired { get; set; }
        public int NumberRequired { get; set; }

        public string Description { get; set; }

        [ContentSerializer(Optional = true)]
        public int GoldAwardAmount { get; set; }

        public List<SanctuaryReward> Rewards { get; set; }

        //Source Rectangle
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        [ContentSerializer(Optional = true)]
        public Rectangle Rectangle { get
            { return new Rectangle(X, Y, Width, Height); }  }

        public SanctuaryRequirement()
        {

        }

    }
}
