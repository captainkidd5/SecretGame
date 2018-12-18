using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace SecretProject.Class.ObjectFolder
{
    public class Polygon
    {
        public List<TmxObjectPoint> points;
        public Polygon(List<TmxObject> tmxObjects)
        {
            foreach(TmxObject obj in tmxObjects)
            {
                obj.Points
            }
        }
    }
}
