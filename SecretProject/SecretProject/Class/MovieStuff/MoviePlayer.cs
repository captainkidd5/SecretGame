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
    public enum MovieName
    {
        None = 0,
        Intro = 1
    }
    public class MoviePlayer
    {
        public bool IsActive { get; set; }
        private List<Movie> Movies { get; set; }
        private Movie CurrentMovie { get; set; }
        public MoviePlayer()
        {
            Movies = new List<Movie>();
            Movies.Add(new IntroMovie(MovieName.Intro));
        }

        public void ChangeMovie(MovieName movieName, IServiceProvider serviceProvider)
        {
            this.CurrentMovie = Movies.Find(x => x.MovieName == movieName);
            this.CurrentMovie.InsertMovie(serviceProvider);
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentMovie.Update(gameTime))
                this.IsActive = false;

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsActive)
                CurrentMovie.Draw(spriteBatch);
        }
    }


}
