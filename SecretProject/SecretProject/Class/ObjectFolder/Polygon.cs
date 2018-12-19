using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace SecretProject.Class.ObjectFolder
{
    //WIP
    public class Polygon
    {

        public List<Vector2> vertices;

        public Polygon(TmxObject tileObject)
        {
            vertices = new List<Vector2>();

            foreach(TmxObjectPoint point in tileObject.Points)
            {
                vertices.Add(new Vector2((float)point.X, (float)point.Y));
            }
            
        }
    }

    public class Line
    {
        Vector2 start;
        Vector2 end;

        public Line(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
        }
    }
}
