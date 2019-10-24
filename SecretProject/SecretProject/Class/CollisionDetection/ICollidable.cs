using Microsoft.Xna.Framework;
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
        grass = 1,
        NPC = 2
    }
    public interface ICollidable
    {
        ColliderType ColliderType { get; set; }
        string LocationKey { get; set; }
        Rectangle Rectangle { get; set; }
        Dir InitialShuffDirection { get; set; }

        bool IsUpdating { get; set; }
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, float layerDepth);
        void Draw(SpriteBatch spriteBatch);
        void Shuff(GameTime gameTime, int direction);
        


    }
}
