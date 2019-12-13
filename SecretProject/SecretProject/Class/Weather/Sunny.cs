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
    public class Sunny : IWeather
    {
        public WeatherType WeatherType { get; set; }
        public ParticleEngine ParticleEngine { get; set; }
        public List<Texture2D> ParticleTextures { get; set; }
        public Color WeatherTint { get; set; }
        public float ChanceToOccur { get; set; }

        public Sunny()
        {
            this.WeatherType = WeatherType.Sunny;
            this.ChanceToOccur = .3f;
        }
        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }


    }
}
