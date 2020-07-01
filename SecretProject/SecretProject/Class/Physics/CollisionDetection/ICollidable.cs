using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.NPCStuff;

namespace SecretProject.Class.CollisionDetection
{
    public enum HitBoxType
    {
        Rectangle = 1,
        Circle = 2,
    }
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
        PlayerMainCollider = 10

    }
    public interface ICollidable
    {
        HitBoxType HitBoxType { get; set; }
        ColliderType ColliderType { get; set; }
        Rectangle Rectangle { get; set; }
        IEntity Entity { get; set; }

        void Update(Rectangle rectangle);
        void DrawDebug(SpriteBatch spriteBatch);
    }
}
