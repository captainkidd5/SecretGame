using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ParticileStuff;
using SecretProject.Class.StageFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.Weather
{
    public interface IWeather
    {
        WeatherType WeatherType { get; set; }
        ParticleEngine ParticleEngine { get; set; }
        List<Texture2D> ParticleTextures { get; set; }
        Color WeatherTint { get; set; }
        float ChanceToOccur { get; set; }
        void Update(GameTime gameTime, LocationType locationType);
        void Draw(SpriteBatch spriteBatch, LocationType locationType);
    }
}
