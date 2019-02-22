using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SecretProject.Class.UI
{
    public class DebugWindow : TextBox
    {
        public DebugWindow(SpriteFont textFont, Vector2 textBoxLocation, string textToWrite, Texture2D backDrop) : base(textFont,textBoxLocation,  textToWrite,  backDrop)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (IsActivated)
            {
                spriteBatch.Draw(backDrop, textBoxLocation, Color.White);
                spriteBatch.DrawString(textFont, "     Debug Window \n \n FrameRate: " + Game1.FrameRate + " \n \n PlayerLocationX: " + Game1.Player.Position.X  +
                    " \n \n PlayerWorldPositionY: " + Game1.Player.Position.Y + "\n \n MouseScreenPositionX: " + Game1.myMouseManager.Position.X + " \n \n MouseScreenPositionY: " +
                    Game1.myMouseManager.Position.Y + " \n \n MouseWorldPositionX: " + Game1.myMouseManager.WorldMousePosition.X + " \n \n MouseWorldPositionY: "
                    + Game1.myMouseManager.WorldMousePosition.Y, textBoxLocation, Color.Red);
            }
            spriteBatch.End();
        }
    }
}
