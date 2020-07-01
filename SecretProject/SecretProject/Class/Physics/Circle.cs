using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Physics
{
    public struct Circle
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        public Circle(Vector2 center, float radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        public bool Contains(Vector2 point)
        {
            return ((point - this.Center).Length() <= this.Radius);
        }

        public float GetDistanceBetweenCircleEdges(Circle other)
        {
            return ((float)Math.Sqrt((other.Center.X - this.Center.X) * (other.Center.X - this.Center.X) +
                (other.Center.Y - this.Center.Y) * (other.Center.Y - this.Center.Y)));
        }

        public bool IntersectsCircle(Circle other)
        {
            return ((other.Center - this.Center).Length() < (other.Radius - this.Radius));
        }

        public float Diameter()
        {
            return Radius * Radius;
        }
    }
}
