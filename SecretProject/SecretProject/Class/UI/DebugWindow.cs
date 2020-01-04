using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;

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

        public List<Button> WeatherButtons { get; set; }
        public DebugWindow(SpriteFont textFont, Vector2 textBoxLocation, string textToWrite, Texture2D backDrop, GraphicsDevice graphicsDevice) : base(textFont, textBoxLocation, textToWrite, backDrop)
        {
            this.ElapsedMS = 0d;
            this.DebugButton1 = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphicsDevice, new Vector2(position.X, position.Y - 200), CursorType.Normal);
            this.SpeedClockUp = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(544, 656, 16, 32), graphicsDevice, new Vector2(Game1.ScreenWidth * .8f, Game1.ScreenHeight / 2), CursorType.Normal) { HitBoxScale = 2f };
            this.SlowClockDown = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(560, 656, 16, 32), graphicsDevice, new Vector2(Game1.ScreenWidth * .8f + 32, Game1.ScreenHeight / 2), CursorType.Normal) { HitBoxScale = 2f };
            this.IncrementDay = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(384, 528, 32, 16), graphicsDevice, new Vector2(Game1.ScreenWidth * .8f + 64, Game1.ScreenHeight / 2), CursorType.Normal) { HitBoxScale = 2f };

            this.WeatherNone = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(441, 496, 62, 22), graphicsDevice, new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight * .2f), CursorType.Normal, 2f);
            this.WeatherRain = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(441, 496, 62, 22), graphicsDevice, new Vector2(Game1.ScreenWidth * .6f, Game1.ScreenHeight * .2f), CursorType.Normal, 2f);
            this.WeatherSunny = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(441, 496, 62, 22), graphicsDevice, new Vector2(Game1.ScreenWidth * .7f, Game1.ScreenHeight * .2f), CursorType.Normal, 2f);
            this.WeatherButtons = new List<Button>() { this.WeatherNone, this.WeatherRain, this.WeatherSunny };

            this.SpawnAnimalPack = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(441, 496, 62, 22), graphicsDevice, new Vector2(100, Game1.ScreenHeight * .2f), CursorType.Normal, 2f);
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsActivated)
            {


                base.Update(gameTime, Keys.F1);
                this.ElapsedMS = gameTime.ElapsedGameTime.TotalMilliseconds;
                this.DebugButton1.Update(Game1.myMouseManager);
                if ((Game1.OldKeyBoardState.IsKeyDown(Keys.G)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.G)))
                {
                    Game1.GlobalClock.IncrementDay();

                }
                if (this.DebugButton1.isClicked)
                {

                    Game1.GetCurrentStage().ActivateNewRisingText(Game1.Player.Rectangle.Y, Game1.Player.Rectangle.Y - 32, "test", 25f, Color.White, true, .5f);
                    Game1.GlobalClock.IncrementDay();


                }
                this.SpawnAnimalPack.Update(Game1.myMouseManager);
                if (this.SpawnAnimalPack.isClicked)
                {
                    Game1.OverWorld.Enemies.AddRange(Game1.OverWorld.AllTiles.ChunkUnderPlayer.NPCGenerator.SpawnNpcPack(GenerationType.Dirt, Game1.Player.position));
                }
                this.SpeedClockUp.Update(Game1.myMouseManager);
                this.SlowClockDown.Update(Game1.myMouseManager);
                this.IncrementDay.Update(Game1.myMouseManager);
                if (this.IncrementDay.isClicked)
                {
                    Game1.GlobalClock.IncrementDay();
                }
                if (this.SpeedClockUp.isClicked)
                {
                    Clock.ClockMultiplier++;
                }
                if (this.SlowClockDown.isClicked)
                {
                    Clock.ClockMultiplier--;
                }

                for (int i = 0; i < this.WeatherButtons.Count; i++)
                {
                    this.WeatherButtons[i].Update(Game1.myMouseManager);
                }
                if (this.WeatherNone.isClicked)
                {
                    Game1.CurrentWeather = WeatherType.None;
                    Console.WriteLine(Game1.CurrentWeather.ToString());
                }
                if (this.WeatherSunny.isClicked)
                {
                    Game1.CurrentWeather = WeatherType.Sunny;
                    Console.WriteLine(Game1.CurrentWeather.ToString());
                }
                if (this.WeatherRain.isClicked)
                {
                    Game1.CurrentWeather = WeatherType.Rainy;
                    Console.WriteLine(Game1.CurrentWeather.ToString());
                }
            }
        }
        //"\n\n TileGID " + Game1.myMouseManager.GetMouseOverTile(Game1.GetCurrentStage().AllTiles).ToString()
        public void Draw(SpriteBatch spriteBatch)
        {


            if (this.IsActivated)
            {
                spriteBatch.Begin();
                //spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle((int)position.X, (int)position.Y, 256,224), new Rectangle(1024, 64, 256, 224),
                //     Game1.Utility.Origin, 0f, 3f, Color.White, SpriteEffects.None, Utility.StandardButtonDepth);
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, position, new Rectangle(1024, 64, 256, 224), Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, Utility.StandardButtonDepth);
                spriteBatch.DrawString(textFont, "     Debug Window \n \n FrameRate: " + Game1.FrameRate + "\n\n MS: " + this.ElapsedMS + " \n \n Mouse I  " +
                   (int)(Game1.myMouseManager.WorldMousePosition.X / 16 / (Math.Abs(Game1.OverWorld.AllTiles.ChunkUnderPlayer.X) + 1)) + " \n \n PlayerPositionX: " + Game1.Player.position.X + " \n \n cameraY: "
                    + Game1.cam.Pos.Y + " \n \n MousePositionX: " + Game1.myMouseManager.WorldMousePosition.X + " \n \n MousePositionY: " +
                    Game1.myMouseManager.WorldMousePosition.Y + "\n\n Camera Screen Rectangle " + Game1.cam.CameraScreenRectangle, position, Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardTextDepth);


                this.DebugButton1.Draw(spriteBatch);
                this.SpeedClockUp.Draw(spriteBatch);
                this.SlowClockDown.Draw(spriteBatch);
                this.IncrementDay.Draw(spriteBatch);
                for (int i = 0; i < this.WeatherButtons.Count; i++)
                {
                    this.WeatherButtons[i].Draw(spriteBatch);
                }
                this.SpawnAnimalPack.Draw(spriteBatch, Game1.AllTextures.MenuText, "SpawnAnimalPack", this.SpawnAnimalPack.Position, Color.White, Utility.StandardButtonDepth, Utility.StandardButtonDepth + .01f, 2f);
                spriteBatch.DrawString(Game1.AllTextures.MenuText, Clock.ClockMultiplier.ToString(), new Vector2(this.SpeedClockUp.Position.X, this.SpeedClockUp.Position.Y - 32), Color.White);
                spriteBatch.End();
            }
        }
    }//+ Game1.GetCurrentStage().AllTiles.DebugTile.DestinationRectangle
}
