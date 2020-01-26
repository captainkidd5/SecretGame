using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;

using SecretProject.Class.SpriteFolder;
using XMLData.RouteStuff;

namespace SecretProject.Class.NPCStuff
{
    public class Dobbin : Character
    {

        public Dobbin(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, RouteSchedule routeSchedule, Texture2D characterPortraitTexture) : base(name, position, graphics, spriteSheet, routeSchedule, Stages.DobbinHouse, false, characterPortraitTexture)
        {
            this.NPCAnimatedSprite = new Sprite[4];

            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 28, 48, 6, .15f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 167, 0, 28, 48, 6, .15f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 335, 0, 28, 48, 6, .15f, this.Position);
            this.NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 503, 0, 28, 48, 6, .15f, this.Position);
            this.NPCRectangleXOffSet = 15;
            this.NPCRectangleYOffSet = 30;
            this.NPCRectangleHeightOffSet = 2;
            this.NPCRectangleWidthOffSet = 2;
            this.SpeakerID = 2;
            // NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);
            this.NextPointRectangleTexture = SetRectangleTexture(graphics, this.NPCPathFindRectangle);
            this.DebugTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            //DebugTexture = SetRectangleTexture(graphics, )
            this.Collider = new Collider(graphics, this.NPCHitBoxRectangle, this, ColliderType.NPC);
            this.DebugColor = Color.HotPink;
        }


    }
}
