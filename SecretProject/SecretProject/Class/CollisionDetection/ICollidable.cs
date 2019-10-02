using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.CollisionDetection
{
    public interface ICollidable
    {
        Rectangle DestinationRectangle { get; set; }
    }
}
