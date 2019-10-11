using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public class HealthBar
    {

        public HealthBar()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, int health)
        {
            for(int i =0; i < health; i++)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(Game1.Player.UserInterface.BottomBar.AllSlots[i].Position.X,
                    Game1.Player.UserInterface.BottomBar.AllSlots[i].Position.Y - 48), new Rectangle(240, 256, 32, 32), Color.White, 0f,
                    Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            }
        }
    }
}
