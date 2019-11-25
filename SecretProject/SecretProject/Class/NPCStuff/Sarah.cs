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
    public class Sarah : Character
    {
        public Sarah(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, RouteSchedule routeSchedule, Texture2D characterPortraitTexture) : base(name, position, graphics, spriteSheet, routeSchedule, Stages.Cafe, false, characterPortraitTexture)
        {
            this.SpeakerID = 6;
            NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 16, 32, 6, .15f, this.Position);
            NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 96, 0, 16, 32, 7, .15f, this.Position);
            NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 96, 0, 16, 32, 7, .15f, this.Position) { Flip = true };
            NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 108, 0, 16, 34, 6, .15f, this.Position);
            this.NPCRectangleXOffSet = 7;
            this.NPCRectangleYOffSet = 30;
            this.NPCRectangleHeightOffSet = 2;
            this.NPCRectangleWidthOffSet = 2;
            //NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);
            NextPointRectangleTexture = SetRectangleTexture(graphics, NPCPathFindRectangle);
            DebugTexture = SetRectangleTexture(graphics, NPCHitBoxRectangle);
            Collider = new Collider(graphics, this.PrimaryVelocity, this.NPCHitBoxRectangle, this, ColliderType.NPC);
            this.DebugColor = Color.Orange;
            //this.CurrentStageLocation = (int)Stages.Pass;
            Game1.GlobalClock.DayChanged += this.OnDayIncreased;
        }
    }
}
