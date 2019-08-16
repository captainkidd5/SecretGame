using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.StageFolder
{
    public class Portal
    {
        public int From { get; set; }
        public int To { get; set; }
        public Rectangle PortalStart { get; set; }
        public int SafteyOffSetX { get; set; }
        public int SafteyOffSetY { get; set; }
        public bool MustBeClicked { get; set; }
        //public Rectangle PortalEnd { get; set; }

        public Portal(int from, int to,int safteyX, int safteyY, bool mustBeClicked)
        {
            this.From = from;
            this.To = to;
            this.SafteyOffSetX = safteyX;
            this.SafteyOffSetY = safteyY;
            this.MustBeClicked = mustBeClicked;
        }
    }
}
