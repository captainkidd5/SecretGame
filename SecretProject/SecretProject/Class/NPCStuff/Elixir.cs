using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.CollisionDetection;
using SecretProject.Class.Controls;
using SecretProject.Class.ObjectFolder;
using SecretProject.Class.SpriteFolder;
using XMLData.RouteStuff;

namespace SecretProject.Class.NPCStuff
{
    public class Elixir : Character
    {


        public Elixir(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet,RouteSchedule routeSchedule):base(name, position, graphics, spriteSheet, routeSchedule)
        {
            this.SpeakerID = 1;
            NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 48, 0, 16, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 144, 0, 16, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 240, 0, 16, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 336, 0, 16, 48, 6, .15f, this.Position);
            this.NPCRectangleXOffSet = 8;
            this.NPCRectangleYOffSet = 34;
            this.NPCRectangleHeightOffSet = 8;
            this.NPCRectangleWidthOffSet = 8;
            //NPCPathFindRectangle = new Rectangle(0, 0, 1, 1);
            NextPointRectangleTexture = SetRectangleTexture(graphics, NPCPathFindRectangle);
            DebugTexture = SetRectangleTexture(graphics, NPCHitBoxRectangle);
            Collider = new Collider(this.PrimaryVelocity, this.NPCHitBoxRectangle);
        }

        
    }
}
