using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public Texture2D DebugTexture { get; set; }

        public static Dictionary<int, Texture2D> CircleSizeTextures = new Dictionary<int, Texture2D>();

        public Circle(Vector2 center, float radius, bool getTexture = true)
        {
            this.Center = center;
            this.Radius = radius;
            DebugTexture = null;
            if(getTexture)
            {
                DebugTexture = RetrieveDebugTexture((int)Radius);
            }
            
           
        }

        /// <summary>
        /// If dictionary already has a texture of desired radius, just use that one. Otherwise create a new one and 
        /// add it to the dictionary.
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        private static Texture2D RetrieveDebugTexture(int radius)
        {
            Texture2D debugTexture;
            if (!CircleSizeTextures.ContainsKey(radius))
            {
                debugTexture = Game1.Utility.GetColoredCircle(radius, Color.Red);
                CircleSizeTextures.Add(radius, debugTexture);
            }
            else
            {
                debugTexture = CircleSizeTextures[radius];
            }
            return debugTexture;
        }

        public bool ContainsPoint(Vector2 point)
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
            float length = (other.Center - this.Center).Length();
            return ((other.Center - this.Center).Length() < (other.Radius + this.Radius) /2);
        }

        //public Vector2 GetTangent(Circle other)
        //{
        //    Vector2 tangent = other.Center - this.Center;
        //    tangent = new Vector2(tangent.Y, tangent.X * -1);
        //    return tangent;
        //}

        public Vector2 GetTangentAlternative(Circle other)
        {
            return new Vector2(-(other.Center.X - this.Center.X),
                other.Center.Y - this.Center.Y);
        }

        public float Diameter()
        {
            return Radius * Radius;
        }

        
    }
}
