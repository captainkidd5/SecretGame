using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.SpriteFolder;
using XMLData.RouteStuff;

namespace SecretProject.Class.NPCStuff
{
    public class Julian : Character
    {
        public Julian(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, RouteSchedule routeSchedule, Texture2D characterPortraitTexture) : base(name, position, graphics, spriteSheet, routeSchedule, Stages.JulianHouse, false, characterPortraitTexture)
        {
            this.SpeakerID = 5;
            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 16, 34, 6, .15f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 96, 0, 16, 34, 7, .15f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 208, 0, 16, 34, 7, .15f, this.Position);
            this.NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 320, 0, 16, 34, 6, .15f, this.Position);
            this.NPCRectangleXOffSet = 7;
            this.NPCRectangleYOffSet = 30;
            this.NPCRectangleHeightOffSet = 2;
            this.NPCRectangleWidthOffSet = 2;
            //NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);
            this.NextPointRectangleTexture = SetRectangleTexture(graphics, this.NPCPathFindRectangle);
            this.DebugTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.Collider = new Collider(graphics, this.NPCHitBoxRectangle, this, ColliderType.NPC);
            this.DebugColor = Color.LightBlue;
            //this.CurrentStageLocation = (int)Stages.Pass;
            Game1.GlobalClock.DayChanged += OnDayIncreased;
        }
    }
}
