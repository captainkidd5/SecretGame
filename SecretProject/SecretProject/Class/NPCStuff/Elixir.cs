using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.QuestFolder;
using SecretProject.Class.SpriteFolder;
using XMLData.RouteStuff;

namespace SecretProject.Class.NPCStuff
{
    public class Elixir : Character
    {


        public Elixir(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, RouteSchedule routeSchedule, QuestHandler questHandler, Texture2D characterPortraitTexture) : base(name, position, graphics, spriteSheet, routeSchedule, Stages.ElixirHouse, false, questHandler, characterPortraitTexture)
        {
            this.SpeakerID = 1;
            this.NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 48, 0, 16, 48, 6, .15f, this.Position);
            this.NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 144, 0, 16, 48, 6, .15f, this.Position);
            this.NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 240, 0, 16, 48, 6, .15f, this.Position);
            this.NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 336, 0, 16, 48, 6, .15f, this.Position);
            this.NPCRectangleXOffSet = 8;
            this.NPCRectangleYOffSet = 34;
            this.NPCRectangleHeightOffSet = 8;
            this.NPCRectangleWidthOffSet = 8;
            //NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);
            this.NextPointRectangleTexture = SetRectangleTexture(graphics, this.NPCPathFindRectangle);
            this.HitBoxTexture = SetRectangleTexture(graphics, this.NPCHitBoxRectangle);
            this.Collider = new Collider(graphics, this.NPCHitBoxRectangle, this, ColliderType.NPC);
            this.DebugColor = Color.DarkGreen;
        }


    }
}
