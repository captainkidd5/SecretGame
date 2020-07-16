using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.MovieStuff.SceneStuff
{
    internal class MovieScene
    {

        private List<MovieProp> Props { get; set; }

        public MovieScene()
        {
            this.Props = new List<MovieProp>();
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        class ParallaxMovieBackground
        {
            private Texture2D Texture { get; set; }
            private Vector2 Position { get; set; }
            private float LayerDepth { get; set; }
            public ParallaxMovieBackground(Texture2D texture, Vector2 position, float layerDepth)
            {
                this.Texture = texture;
                this.Position = position;
                this.LayerDepth = layerDepth;
            }

            public void Update(GameTime gameTIme)
            {

            }
            public void Draw(SpriteBatch spriteBatch)
            {

            }
        }
    }
}
