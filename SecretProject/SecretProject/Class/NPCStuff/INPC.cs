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
        string Name { get; }
        Sprite[] NPCAnimatedSprite { get;  }
        Rectangle NPCHitBoxRectangle { get; }
        Texture2D Texture { get; }
        Texture2D HitBoxTexture { get; }
        float Speed { get; set; }
        bool IsMoving { get; set; }
        Vector2 PrimaryVelocity { get;  }
        Vector2 DirectionVector { get; set; }

        EmoticonType CurrentEmoticon { get; set; }

    }
}
