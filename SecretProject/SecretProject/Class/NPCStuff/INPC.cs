using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
         int NPCRectangleXOffSet { get; set; }
         int NPCRectangleYOffSet { get; set; }
         int NPCRectangleWidthOffSet { get; set; }
         int NPCRectangleHeightOffSet { get; set; }
        Rectangle NPCHitBoxRectangle { get; }
        Texture2D Texture { get; set; }
        Texture2D DebugTexture { get; set; }
        float Speed { get; set; }
         bool IsMoving { get; set; }
         Vector2 PrimaryVelocity { get; set; }
         Vector2 TotalVelocity { get; set; }
         Vector2 DirectionVector { get; set; }
         Dir CurrentDirection { get; set; }
         int FrameNumber { get; set; }
         Collider Collider { get; set; }
         bool CollideOccured { get; set; }
        EmoticonType CurrentEmoticon { get; set; }

    }
}
