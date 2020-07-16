using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.MovieStuff.SceneStuff
{
    internal class Movie
    {

        private List<MovieScene> MovieScenes { get; set; }
        public Movie()
        {
            this.MovieScenes = new List<MovieScene>();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.MovieScenes.Count; i++)
            {
                MovieScenes[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.MovieScenes.Count; i++)
            {
                MovieScenes[i].Draw(spriteBatch);
            }
        }
    }
}
