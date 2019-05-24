using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SecretProject.Class.Controls;
using SecretProject.Class.SpriteFolder;

namespace SecretProject.Class.NPCStuff
{
    public class Character : NPC
    {
        public Character(string name, Vector2 position, GraphicsDevice graphics) : base(name, position, graphics)
        {
            
            
        }

        public override void Update(GameTime gameTime, MouseManager mouse)
        {
            NPCAnimatedSprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
           // NPCAnimatedSprite.Draw(spriteBatch, Position, .4f);
        }
    }
}
