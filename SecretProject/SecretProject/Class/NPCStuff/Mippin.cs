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
    public class Mippin : Character
    {
        public Mippin(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, RouteSchedule routeSchedule, Texture2D characterPortraitTexture) : base(name, position, graphics, spriteSheet, routeSchedule, Stages.JulianHouse, false, characterPortraitTexture)
        {
            this.SpeakerID = 8;
            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 16, 32, 6, .06f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 96, 0, 16, 32, 7, .06f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 96, 0, 16, 32, 7, .06f, this.Position) { Flip = true };
            this.NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 208, 0, 16, 32, 6, .06f, this.Position);
            this.NPCRectangleXOffSet = 7;
            this.NPCRectangleYOffSet = 30;
            this.NPCRectangleHeightOffSet = 2;
            this.NPCRectangleWidthOffSet = 2;
            //NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);
            this.NextPointRectangleTexture = SetRectangleTexture(graphics, this.NPCPathFindRectangle);
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.Collider = new Collider(graphics, this.NPCHitBoxRectangle, this, ColliderType.NPC);
            this.DebugColor = Color.LightBlue;
            //this.CurrentStageLocation = (int)Stages.Pass;
            Game1.GlobalClock.DayChanged += OnDayIncreased;
        }
    }
}

