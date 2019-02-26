﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using SecretProject.Class.SpriteFolder;

namespace SecretProject.Class.NPCStuff
{
    internal class Joe : NPC
    {
        public Joe(string name, Vector2 position, GraphicsDevice graphics) : base(name, position, graphics)
        {
            NPCAnimatedSprite = new AnimatedSprite(graphics, Game1.AllTextures.joeDown, 1, 4, 4);
            
        }

        public override void Update(GameTime gameTime)
        {
            NPCAnimatedSprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            NPCAnimatedSprite.Draw(spriteBatch, position, .4f);
        }
    }
}