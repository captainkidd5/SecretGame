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

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime, Keys.F1);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (IsActivated)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle((int)position.X, (int)position.Y, 256,224), new Rectangle(272, 128, 256, 224),
                    Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                spriteBatch.DrawString(textFont, "     Debug Window \n \n FrameRate: " + Game1.FrameRate  + " \n \n MouseOverTile: "  +
                    " \n \n elixerposition: " + Game1.RoyalDock.ElixerNPC.Position + "\n \n Cam PositionX " + Game1.cam.Pos.X  + " \n \n ScreenWidth/2 " + Game1.ScreenWidth + " \n \n MouseWorldPositionY: "
                    + Game1.myMouseManager.WorldMousePosition.Y + " \n \n MouseSquarePositionX: " + Game1.myMouseManager.MouseSquareCoordinateX + " \n \n MouseSquarePositionY: " +
                    Game1.myMouseManager.MouseSquareCoordinateY, position, Color.Red, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardTextDepth);
            }
            spriteBatch.End();
        }
    }//+ Game1.GetCurrentStage().AllTiles.DebugTile.DestinationRectangle
}
