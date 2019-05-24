using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.SpriteFolder;

namespace SecretProject.Class.NPCStuff
{
    public class Elixir : Character
    {
        public Elixir(string name, Vector2 position, GraphicsDevice graphics) : base(name, position, graphics)
        {
            NPCAnimatedSprite = new AnimatedSprite(graphics, Game1.AllTextures.Elixer, 1, 1, 1);
            this.Name = "Elixir";
            this.Position = position;

        }

        public override void Update(GameTime gameTime, MouseManager mouse)
        {
            NPCAnimatedSprite.Update(gameTime);
            if(mouse.IsHovering(this.NPCRectangle) && mouse.IsClicked)
            {
                Game1.userInterface.IsShopMenu = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            NPCAnimatedSprite.Draw(spriteBatch, Position, .4f);
        }
    }
}
