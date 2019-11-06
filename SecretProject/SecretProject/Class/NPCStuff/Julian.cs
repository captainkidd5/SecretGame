using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.RouteStuff;

namespace SecretProject.Class.NPCStuff
{
    public class Julian : Character
    {
        public Julian(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, RouteSchedule routeSchedule, Texture2D characterPortraitTexture) : base(name, position, graphics, spriteSheet, routeSchedule, 0, false, characterPortraitTexture)
        {
            this.SpeakerID = 5;
            NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 16, 34, 6, .15f, this.Position);
            NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 96, 0, 16, 34, 7, .15f, this.Position);
            NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 208, 0, 16, 34, 7, .15f, this.Position);
            NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 320, 0, 16, 34, 6, .15f, this.Position);
            this.NPCRectangleXOffSet = 5;
            this.NPCRectangleYOffSet = 15;
            this.NPCRectangleHeightOffSet = 12;
            this.NPCRectangleWidthOffSet = 8;
            //NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);
            NextPointRectangleTexture = SetRectangleTexture(graphics, NPCPathFindRectangle);
            DebugTexture = SetRectangleTexture(graphics, NPCHitBoxRectangle);
            Collider = new Collider(graphics,this.PrimaryVelocity, this.NPCHitBoxRectangle, this,ColliderType.NPC);
            this.DebugColor = Color.LightBlue;
            //this.CurrentStageLocation = (int)Stages.Pass;
        }
    }
}
