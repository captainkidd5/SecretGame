using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MovieStuff.SceneStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.MovieStuff
{
    public class MoviePlayer
    {
        private List<Movie> Movies { get; set; }
        public MoviePlayer()
        {
            Movies = new List<Movie>();
        }

        public void Update(GameTime gameTime)
        {
            for(int i =0; i < this.Movies.Count;i++)
            {
                Movies[i].Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.Movies.Count; i++)
            {
                Movies[i].Draw(spriteBatch);
            }
        }
    }

    
}
