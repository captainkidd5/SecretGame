using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Dynamics.Joints;
using VelcroPhysics.Factories;
using XMLData.ItemStuff;

namespace SecretProject.Class.Physics.Tools
{

    public class Sword : Tool
    {

        public Sword(ICollidable entityData, Vector2 entityPosition, Sprite swordSprite, int damage, Dir direction, float? toolLength, Vector2? customToolLength) : base( entityData,  entityPosition,  swordSprite,  damage,  direction, toolLength,  customToolLength)
        {

        }

    }
}
