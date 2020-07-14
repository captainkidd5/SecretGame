using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Dynamics;

namespace SecretProject.Class.Physics
{
    public interface ICollidable
    {
        Body CollisionBody { get; set; }

        void CreateBody();
    }
}
