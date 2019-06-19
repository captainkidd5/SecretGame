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

namespace SecretProject.Class.NPCStuff
{
    public class Elixir : Character
    {


        public Elixir(string name, Vector2 position, GraphicsDevice graphics, Texture2D spriteSheet):base(name, position, graphics, spriteSheet)
        {
            this.SpeakerID = 1;

        }

        
    }
}
