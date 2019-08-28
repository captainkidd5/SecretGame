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
        public double ElapsedMS { get; set; }
        public DebugWindow(SpriteFont textFont, Vector2 textBoxLocation, string textToWrite, Texture2D backDrop) : base(textFont,textBoxLocation,  textToWrite,  backDrop)
        {
            ElapsedMS = 0d;
        }

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime, Keys.F1);
            ElapsedMS = gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (IsActivated)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle((int)position.X, (int)position.Y, 256,224), new Rectangle(272, 128, 256, 224),
                    Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                spriteBatch.DrawString(textFont, "     Debug Window \n \n FrameRate: " + Game1.FrameRate + "\n\n MS: " + ElapsedMS + " \n \n MouseOverTile: "  +
                    " \n \n PlayerPositionX: " + Game1.Player.position.X  + " \n \n MouseWorldRectangle: "
                    + Game1.myMouseManager.WorldMouseRectangle + " \n \n MouseSquarePositionX: " + Game1.myMouseManager.MouseSquareCoordinateX + " \n \n mouseworldrectangle: " +
                    Game1.myMouseManager.MouseSquareCoordinateY, position, Color.Red, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardTextDepth);

                //for(int i =0; i< 100; i++)
                //{
                //    for(int j =0; j < 100; j++)
                //    {
                //        //spriteBatch.DrawString(Game1.AllTextures.MenuText, Game1.GetCurrentStage().AllTiles.AllTiles[1][i, j].AStarTileValue.ToString() + "  ", new Vector2(position.X + (i * 5), position.Y + (j* 5)), Color.White);
                //        spriteBatch.DrawString(Game1.AllTextures.MenuText, Game1.GetCurrentStage().AllTiles.AllTiles[1][i, j].AStarTileValue.ToString() +"  ", new Vector2(position.X + (i * 20), position.Y - 500 + (j * 20)), Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None,1f);
                //    }
                //}
                
            }
            spriteBatch.End();
        }
    }//+ Game1.GetCurrentStage().AllTiles.DebugTile.DestinationRectangle
}
