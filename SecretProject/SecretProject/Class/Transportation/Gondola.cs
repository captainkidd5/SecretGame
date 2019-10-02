using Microsoft.Xna.Framework;
using SecretProject.Class.CollisionDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Transportation
{
    public class Gondola
    {
        public int ID { get; set; }
        public Vector2 StartingPosition { get; set; }
        public Vector2 EndingPosition { get; set; }
        public Vector2 Position { get; set; }

        public Rectangle SourceRectangle { get; set; }
        public ObjectBody Object { get; set; }

        public Gondola()
        {

        }
    }
}
