using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.QuestFolder;
using SecretProject.Class.SpriteFolder;
using XMLData.RouteStuff;

namespace SecretProject.Class.NPCStuff
{
    public class Kaya : Character
    {
        public Kaya(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, RouteSchedule routeSchedule, QuestHandler questHandler, Texture2D characterPortraitTexture) : base(name, position, graphics, spriteSheet, routeSchedule, Stages.KayaHouse, false, questHandler, characterPortraitTexture)
        {
            this.SpeakerID = 4;
            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 16, 34, 6, .15f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 112, 0, 16, 34, 7, .15f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 224, 0, 16, 34, 7, .15f, this.Position);
            this.NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 336, 0, 16, 34, 6, .15f, this.Position);
            this.NPCRectangleXOffSet = 5;
            this.NPCRectangleYOffSet = 15;
            this.NPCRectangleHeightOffSet = 12;
            this.NPCRectangleWidthOffSet = 8;
            //NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);
            this.NextPointRectangleTexture = SetRectangleTexture(graphics, this.NPCPathFindRectangle);
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.Collider = new Collider(graphics, this.NPCHitBoxRectangle, this, ColliderType.NPC);
            this.DebugColor = Color.LightBlue;
            //this.CurrentStageLocation = (int)Stages.Pass;
        }
    }
}
