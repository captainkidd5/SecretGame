using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ParticileStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Weather
{
    public class Rainy : IWeather
    {
        public WeatherType WeatherType { get; set; }
        public ParticleEngine ParticleEngine { get; set; }
        public List<Texture2D> ParticleTextures { get; set; }
        public Color WeatherTint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Texture2D RainyMask;
        public Vector2 RainyMaskPosition;

        public float ChanceToOccur { get; set; }

        public Rainy(GraphicsDevice graphics)
        {
            WeatherType = WeatherType.Rainy;
            ParticleTextures = new List<Texture2D>()
           {
               Game1.AllTextures.RainDrop,
           };

            ParticleEngine = new ParticleEngine(ParticleTextures, new Vector2(Game1.Player.Position.X - Game1.ScreenWidth, Game1.Player.Position.Y - 100)) { AddNewParticleTimer = .0001f } ;
            RainyMask = Game1.Utility.GetColoredRectangle(graphics, Game1.PresentationParameters.BackBufferWidth, Game1.PresentationParameters.BackBufferHeight, Color.Teal);
            this.ChanceToOccur = .7f;
        }


        public void Update(GameTime gameTime)
        {
            ParticleEngine.UpdateWeather(gameTime);
            RainyMaskPosition = new Vector2(Game1.cam.Pos.X - Game1.ScreenWidth/2, Game1.cam.Pos.Y - Game1.ScreenHeight/2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ParticleEngine.Draw(spriteBatch);
            spriteBatch.Draw(this.RainyMask, RainyMaskPosition, null, Color.White * .5f, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, 1f); ;
        }

        
    }
}
