using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.SpriteFolder;

namespace SecretProject.Class.NPCStuff.Enemies
{
    class Enemy : INPC
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Vector2 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Sprite[] NPCAnimatedSprite { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Rectangle NPCRectangle => throw new NotImplementedException();

        public Texture2D Texture { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Speed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
