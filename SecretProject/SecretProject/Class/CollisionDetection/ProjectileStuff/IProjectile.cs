using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.CollisionDetection.ProjectileStuff
{
    public enum ProjectileType
    {
        Arrow = 1
    }
    public interface IProjectile
    {
        ProjectileType ProjectileType { get; set; }
        float Speed { get; set; }
        int Damage { get; set; }
        Rectangle SourceRectangle { get; set; }
    }
}
