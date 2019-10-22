using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.CollisionDetection
{
    public interface ICollidable
    {
        Rectangle Rectangle { get; set; }

        void Draw(SpriteBatch spriteBatch, float layerDepth);
    }
}
