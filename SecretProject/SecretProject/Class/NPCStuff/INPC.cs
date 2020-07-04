using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.SpriteFolder;

namespace SecretProject.Class.NPCStuff
{
    public enum EmoticonType
    {
        None = 0,
        Exclamation = 1
    }



    public interface INPC : IEntity
    {
        string Name { get; set; }
        Vector2 Position { get; set; }
        Sprite[] NPCAnimatedSprite { get; set; }
        Rectangle NPCHitBoxRectangle { get; }
        Texture2D Texture { get; set; }
        Texture2D HitBoxTexture { get; set; }
        float Speed { get; set; }
        bool IsMoving { get; set; }
        Vector2 PrimaryVelocity { get; set; }
        Vector2 TotalVelocity { get; set; }
        Vector2 DirectionVector { get; set; }
        Dir CurrentDirection { get; set; }
        CircleCollider Collider { get; set; }
        bool CollideOccured { get; set; }
        EmoticonType CurrentEmoticon { get; set; }

    }
}
