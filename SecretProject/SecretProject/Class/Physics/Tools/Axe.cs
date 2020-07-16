using Microsoft.Xna.Framework;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Physics.Tools
{
    public class Axe : Tool
    {

        public Axe(ICollidable entityData, Vector2 entityPosition, Sprite swordSprite, int damage, Dir direction, float? toolLength, Vector2? customToolLength) : base(entityData, entityPosition, swordSprite, damage, direction, toolLength, customToolLength)
        {

        }

    }
}
