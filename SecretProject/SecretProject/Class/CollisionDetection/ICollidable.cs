﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.CollisionDetection
{
    public enum ColliderType
    {
        inert = 0,
        grass = 1
    }
    public interface ICollidable
    {
        ColliderType ColliderType { get; set; }
        Rectangle Rectangle { get; set; }

        void Update(GameTime gameTime, Dir direction);
        void Draw(SpriteBatch spriteBatch, float layerDepth);

        void Shuff(GameTime gameTime, int direction);
        


    }
}
