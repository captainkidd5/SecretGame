using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.RouteStuff;

namespace SecretProject.Class.NPCStuff
{
    public class Dobbin  : Character
    {

        public Dobbin(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet, RouteSchedule routeSchedule) : base(name, position, graphics, spriteSheet, routeSchedule)
        {
            NPCAnimatedSprite = new Sprite[4];

            NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 28, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 167, 0, 28, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 335, 0, 28, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 503, 0, 28, 48, 6, .15f, this.Position);
            this.NPCRectangleXOffSet = 0;
            this.NPCRectangleYOffSet = 0;
            this.NPCRectangleHeightOffSet = 20;
            this.NPCRectangleWidthOffSet = 20;
            this.SpeakerID = 2;
            DebugTexture = SetRectangleTexture(graphics, NPCRectangle);
        }
    }
}
