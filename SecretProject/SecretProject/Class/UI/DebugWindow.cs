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
using SecretProject.Class.NPCStuff;
using SecretProject.Class.PathFinding.PathFinder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;
using XMLData.ItemStuff;

namespace SecretProject.Class.UI
{
    public class DebugWindow : TextBox
    {
        public double ElapsedMS { get; set; }

        public Button DebugButton1 { get; set; }
        public Button SpeedClockUp { get; set; }
        public Button SlowClockDown { get; set; }

        public Button IncrementDay { get; set; }
        

        public Button WeatherNone { get; set; }
        public Button WeatherRain { get; set; }
        public Button WeatherSunny { get; set; }

        public Button SpawnAnimalPack { get; set; }

        public List<Button> WeatherButtons{ get; set; }
        public DebugWindow(SpriteFont textFont, Vector2 textBoxLocation, string textToWrite, Texture2D backDrop, GraphicsDevice graphicsDevice) : base(textFont,textBoxLocation,  textToWrite,  backDrop)
        {
            ElapsedMS = 0d;
            DebugButton1 = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(this.position.X, this.position.Y - 200), CursorType.Normal);
            SpeedClockUp = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(544, 656, 16, 32), graphicsDevice, new Vector2(Game1.ScreenWidth * .8f, Game1.ScreenHeight / 2), CursorType.Normal) { HitBoxScale = 2f };
            SlowClockDown = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(560, 656, 16, 32), graphicsDevice, new Vector2(Game1.ScreenWidth * .8f + 32, Game1.ScreenHeight / 2), CursorType.Normal) { HitBoxScale = 2f };
            IncrementDay = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(384, 528, 32, 16), graphicsDevice, new Vector2(Game1.ScreenWidth * .8f + 64, Game1.ScreenHeight / 2), CursorType.Normal) { HitBoxScale = 2f };

            WeatherNone = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(441, 496, 62, 22), graphicsDevice, new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight * .2f), CursorType.Normal, 2f);
            WeatherRain = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(441, 496, 62, 22), graphicsDevice, new Vector2(Game1.ScreenWidth *.6f, Game1.ScreenHeight * .2f), CursorType.Normal, 2f);
            WeatherSunny = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(441, 496, 62, 22), graphicsDevice, new Vector2(Game1.ScreenWidth * .7f, Game1.ScreenHeight * .2f), CursorType.Normal, 2f);
            WeatherButtons = new List<Button>() { WeatherNone, WeatherRain, WeatherSunny };

            SpawnAnimalPack = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(441, 496, 62, 22), graphicsDevice, new Vector2(100, Game1.ScreenHeight * .2f), CursorType.Normal, 2f);
        }

        public void Update(GameTime gameTime)
        {
            if (IsActivated)
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

                    Game1.GetCurrentStage().ActivateNewRisingText(Game1.Player.Rectangle.Y, Game1.Player.Rectangle.Y - 32, "test", 25f, Color.White, true, .5f);
                    Game1.GlobalClock.IncrementDay();


                }
                SpawnAnimalPack.Update(Game1.myMouseManager);
                if(SpawnAnimalPack.isClicked)
                {
                    Game1.OverWorld.Enemies.AddRange(Game1.OverWorld.AllTiles.ChunkUnderPlayer.NPCGenerator.SpawnNpcPack(GenerationType.Dirt, Game1.Player.position));
                }
                SpeedClockUp.Update(Game1.myMouseManager);
                SlowClockDown.Update(Game1.myMouseManager);
                IncrementDay.Update(Game1.myMouseManager);
                if (IncrementDay.isClicked)
                {
                    Game1.GlobalClock.IncrementDay();
                }
                if (SpeedClockUp.isClicked)
                {
                    Clock.ClockMultiplier++;
                }
                if (SlowClockDown.isClicked)
                {
                    Clock.ClockMultiplier--;
                }

                for (int i = 0; i < WeatherButtons.Count; i++)
                {
                    WeatherButtons[i].Update(Game1.myMouseManager);
                }
                if (WeatherNone.isClicked)
                {
                    Game1.CurrentWeather = WeatherType.None;
                    Console.WriteLine(Game1.CurrentWeather.ToString());
                }
                if (WeatherSunny.isClicked)
                {
                    Game1.CurrentWeather = WeatherType.Sunny;
                    Console.WriteLine(Game1.CurrentWeather.ToString());
                }
                if (WeatherRain.isClicked)
                {
                    Game1.CurrentWeather = WeatherType.Rainy;
                    Console.WriteLine(Game1.CurrentWeather.ToString());
                }
            }
        }
        //"\n\n TileGID " + Game1.myMouseManager.GetMouseOverTile(Game1.GetCurrentStage().AllTiles).ToString()
        public void Draw(SpriteBatch spriteBatch)
        {
            

            if (IsActivated)
            {
                spriteBatch.Begin();
                //spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle((int)position.X, (int)position.Y, 256,224), new Rectangle(1024, 64, 256, 224),
                //     Game1.Utility.Origin, 0f, 3f, Color.White, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, position, new Rectangle(1024, 64, 256, 224), Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                spriteBatch.DrawString(textFont, "     Debug Window \n \n FrameRate: " + Game1.FrameRate + "\n\n MS: " + ElapsedMS + " \n \n Mouse I  "  +
                   (int)(Game1.myMouseManager.WorldMousePosition.X / 16 / (Math.Abs(Game1.OverWorld.AllTiles.ChunkUnderPlayer.X) + 1)) + " \n \n PlayerPositionX: " + Game1.Player.position.X  + " \n \n cameraY: "
                    + Game1.cam.Pos.Y + " \n \n MousePositionX: " + Game1.myMouseManager.WorldMousePosition.X + " \n \n MousePositionY: " +
                    Game1.myMouseManager.WorldMousePosition.Y + "\n\n Camera Screen Rectangle " + Game1.cam.CameraScreenRectangle, position, Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardTextDepth);


                DebugButton1.Draw(spriteBatch);
                SpeedClockUp.Draw(spriteBatch);
                SlowClockDown.Draw(spriteBatch);
                IncrementDay.Draw(spriteBatch);
                for(int i =0; i < WeatherButtons.Count; i++)
                {
                    WeatherButtons[i].Draw(spriteBatch);
                }
                SpawnAnimalPack.Draw(spriteBatch, Game1.AllTextures.MenuText, "SpawnAnimalPack", SpawnAnimalPack.Position, Color.White, Game1.Utility.StandardButtonDepth, Game1.Utility.StandardButtonDepth + .01f, 2f);
                spriteBatch.DrawString(Game1.AllTextures.MenuText, Clock.ClockMultiplier.ToString(), new Vector2(SpeedClockUp.Position.X, SpeedClockUp.Position.Y - 32), Color.White);
            spriteBatch.End();
            }
        }
    }//+ Game1.GetCurrentStage().AllTiles.DebugTile.DestinationRectangle
}
