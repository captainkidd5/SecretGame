using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SpriteFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.NPCStuff
{
    public class Dobbin  : Character
    {

        public Dobbin(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet) : base(name, position, graphics, spriteSheet)
        {
            NPCAnimatedSprite = new Sprite[4];

            NPCAnimatedSprite[0] = new Sprite(graphics, this.Texture, 0, 0, 28, 48, 5, .15f, this.Position);
            NPCAnimatedSprite[1] = new Sprite(graphics, this.Texture, 140, 0, 28, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[2] = new Sprite(graphics, this.Texture, 308, 0, 28, 48, 6, .15f, this.Position);
            NPCAnimatedSprite[3] = new Sprite(graphics, this.Texture, 476, 0, 28, 48, 6, .15f, this.Position);

        }
    }
}
