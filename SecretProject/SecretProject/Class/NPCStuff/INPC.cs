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
    interface INPC
    {
        string Name { get; set; }
        Vector2 Position { get; set; }
        AnimatedSprite NPCAnimatedSprite { get; set; }
        Rectangle NPCRectangle { get; }
        Texture2D Texture { get; set; }

    }
}
