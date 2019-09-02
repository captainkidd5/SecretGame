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
    public class Kaya : Character
    {
        public Kaya(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, RouteSchedule routeSchedule) : base(name, position, graphics, spriteSheet, routeSchedule, 4, false)
        {
            this.SpeakerID = 4;
            NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 16, 34, 6, .15f, this.Position);
            NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 112, 0, 16, 34, 7, .15f, this.Position);
            NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 224, 0, 16, 34, 7, .15f, this.Position);
            NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 336, 0, 16, 34, 6, .15f, this.Position);
            this.NPCRectangleXOffSet = 5;
            this.NPCRectangleYOffSet = 15;
            this.NPCRectangleHeightOffSet = 12;
            this.NPCRectangleWidthOffSet = 8;
            //NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);
            NextPointRectangleTexture = SetRectangleTexture(graphics, NPCPathFindRectangle);
            DebugTexture = SetRectangleTexture(graphics, NPCHitBoxRectangle);
            Collider = new Collider(this.PrimaryVelocity, this.NPCHitBoxRectangle);
            this.DebugColor = Color.LightBlue;
            //this.CurrentStageLocation = (int)Stages.Pass;
        }
    }
}
