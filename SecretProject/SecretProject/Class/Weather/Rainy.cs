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
        public ParticleEngine ParticleEngine { get; set; }
        public List<Texture2D> ParticleTextures { get; set; }
        public Color WeatherTint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        
    }
}
