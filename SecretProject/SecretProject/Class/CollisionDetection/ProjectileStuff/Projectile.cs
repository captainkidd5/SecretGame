using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;

namespace SecretProject.Class.CollisionDetection.ProjectileStuff
{

    public class Projectile : ICollidable
    {
        public ColliderType ColliderType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string LocationKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Rectangle Rectangle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Dir InitialShuffDirection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEntity Entity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsUpdating { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void SelfDestruct()
        {
            throw new NotImplementedException();
        }

        public void Shuff(GameTime gameTime, int direction)
        {
            throw new NotImplementedException();
        }

        
    }
}
