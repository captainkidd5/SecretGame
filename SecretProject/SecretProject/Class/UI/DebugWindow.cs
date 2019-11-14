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
using SecretProject.Class.MenuStuff;
using SecretProject.Class.PathFinding.PathFinder;
using SecretProject.Class.TileStuff;
using XMLData.ItemStuff;

namespace SecretProject.Class.UI
{
    public class DebugWindow : TextBox
    {
        public double ElapsedMS { get; set; }

        public Button DebugButton1 { get; set; }
        public DebugWindow(SpriteFont textFont, Vector2 textBoxLocation, string textToWrite, Texture2D backDrop, GraphicsDevice graphicsDevice) : base(textFont,textBoxLocation,  textToWrite,  backDrop)
        {
            ElapsedMS = 0d;
            DebugButton1 = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(this.position.X, this.position.Y - 200), CursorType.Normal);
        }

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime, Keys.F1);
            ElapsedMS = gameTime.ElapsedGameTime.TotalMilliseconds;
            DebugButton1.Update(Game1.myMouseManager);
            if ((Game1.OldKeyBoardState.IsKeyDown(Keys.G)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.G)))
            {
                Game1.GlobalClock.IncrementDay();
            }
            if (DebugButton1.isClicked)
            {


                //Game1.GlobalClock.IncrementDay();
                //Console.Clear();
                byte[,] byteList = new byte[120, 120];
                for(int i =0; i < 120; i++)
                {
                    for(int j =0; j < 120; j++)
                    {
                        byteList[i, j] = 1;
                    }
                }
                PathFinder pathfinder = new PathFinder(byteList);
                List<PathFinderNode> debugNodes = pathfinder.FindPath(new Point(65, 37), new Point(74, 82));

            }
        }
        //"\n\n TileGID " + Game1.myMouseManager.GetMouseOverTile(Game1.GetCurrentStage().AllTiles).ToString()
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (IsActivated)
            {
                //spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle((int)position.X, (int)position.Y, 256,224), new Rectangle(1024, 64, 256, 224),
                //     Game1.Utility.Origin, 0f, 3f, Color.White, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, position, new Rectangle(1024, 64, 256, 224), Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                spriteBatch.DrawString(textFont, "     Debug Window \n \n FrameRate: " + Game1.FrameRate + "\n\n MS: " + ElapsedMS + " \n \n Mouse I  "  +
                   (int)(Game1.myMouseManager.WorldMousePosition.X / 16 / (Math.Abs(Game1.World.AllTiles.ChunkUnderPlayer.X) + 1)) + " \n \n PlayerPositionX: " + Game1.Player.position.X  + " \n \n cameraY: "
                    + Game1.cam.Pos.Y + " \n \n MousePositionX: " + Game1.myMouseManager.WorldMousePosition.X + " \n \n MousePositionY: " +
                    Game1.myMouseManager.WorldMousePosition.Y  + "\n\n Kaya position y " + Game1.Kaya.Position.Y, position, Color.Red, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardTextDepth);

                //for(int i =0; i< 100; i++)
                //{
                //    for(int j =0; j < 100; j++)
                //    {
                //        //spriteBatch.DrawString(Game1.AllTextures.MenuText, Game1.GetCurrentStage().AllTiles.AllTiles[1][i, j].AStarTileValue.ToString() + "  ", new Vector2(position.X + (i * 5), position.Y + (j* 5)), Color.White);
                //        spriteBatch.DrawString(Game1.AllTextures.MenuText, Game1.GetCurrentStage().AllTiles.AllTiles[1][i, j].AStarTileValue.ToString() +"  ", new Vector2(position.X + (i * 20), position.Y - 500 + (j * 20)), Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None,1f);
                //    }
                //}
                DebugButton1.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }//+ Game1.GetCurrentStage().AllTiles.DebugTile.DestinationRectangle
}
