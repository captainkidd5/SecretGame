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
                spriteBatch.DrawString(textFont, "     Debug Window \n \n FrameRate: " + Game1.FrameRate  + " \n \n PlayerLocationX: " + Game1.Player.Position.X  +
                    " \n \n PlayerWorldPositionY: " + Game1.Player.Position.Y + "\n \n Cam PositionX " + Game1.cam.Pos.X  + " \n \n ScreenWidth/2 " + Game1.ScreenWidth + " \n \n MouseWorldPositionY: "
                    + Game1.myMouseManager.WorldMousePosition.Y + " \n \n MouseSquarePositionX: " + Game1.myMouseManager.MouseSquareCoordinateX + " \n \n MouseSquarePositionY: " +
                    Game1.myMouseManager.MouseSquareCoordinateY, textBoxLocation, Color.Red);
            }
            spriteBatch.End();
        }
    }
}
