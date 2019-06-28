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
    public interface INPC
    {
        string Name { get; set; }
        Vector2 Position { get; set; }
        Sprite[] NPCAnimatedSprite { get; set; }
         int NPCRectangleXOffSet { get; set; }
         int NPCRectangleYOffSet { get; set; }
         int NPCRectangleWidthOffSet { get; set; }
         int NPCRectangleHeightOffSet { get; set; }
        Rectangle NPCRectangle { get; }
        Texture2D Texture { get; set; }
        float Speed { get; set; }
         bool IsMoving { get; set; }
         Vector2 PrimaryVelocity { get; set; }
         Vector2 TotalVelocity { get; set; }
         Vector2 DirectionVector { get; set; }
         int CurrentDirection { get; set; }
         bool IsUpdating { get; set; }
         int FrameNumber { get; set; }
         Collider Collider { get; set; }
         bool CollideOccured { get; set; }


    }
}
