using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.NPCStuff.Enemies
{
    public class Boar : Enemy
    {
        public Boar(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet) : base(name, position, graphics, spriteSheet)
        {
            NPCAnimatedSprite = new Sprite[4];

            NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 48, 32, 3, .15f, this.Position);
            NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 144, 0, 48, 32, 3, .15f, this.Position);
            NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 288, 0, 48, 32, 3, .15f, this.Position);
            NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 432, 0, 48, 32, 3, .15f, this.Position);

            this.NPCRectangleXOffSet = 0;
            this.NPCRectangleYOffSet = 0;
            this.NPCRectangleHeightOffSet = 20;
            this.NPCRectangleWidthOffSet = 20;
            this.Speed = 1f;
        }
    }
}
