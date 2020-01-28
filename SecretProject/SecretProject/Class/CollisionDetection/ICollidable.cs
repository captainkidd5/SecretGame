using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;

namespace SecretProject.Class.CollisionDetection
{
    public enum ColliderType
    {
        inert = 0,
        grass = 1,
        NPC = 2,
        Enemy = 3,
        Item = 4,
        Undetectable = 5,
        PlayerBigBox = 6,
        TransperencyDetector = 7,
        Projectile = 8,
        MouseCollider = 9,

    }
    public interface ICollidable
    {
        ColliderType ColliderType { get; set; }
        string LocationKey { get; set; }
        Rectangle Rectangle { get; set; }
        Dir InitialShuffDirection { get; set; }
        IEntity Entity { get; set; }
        bool IsUpdating { get; set; }
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, float layerDepth);
        void Draw(SpriteBatch spriteBatch);
        void Shuff(GameTime gameTime, int direction);
        void SelfDestruct();




    }
}
