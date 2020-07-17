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
        public IServiceProvider IServiceProvider { get; set; }
        public bool IsActive { get; set; }
        private List<Movie> Movies { get; set; }
        private Movie CurrentMovie { get; set; }

        public bool Paused { get; set; }
        public MoviePlayer(IServiceProvider serviceProvider)
        {
            Movies = new List<Movie>();
            Movies.Add(new IntroMovie(MovieName.Intro));
            this.IServiceProvider = serviceProvider;
        }

        /// <summary>
        /// TODO, need user interface involved to work.
        /// </summary>
        private void EndMovieEarly()
        {
            CurrentMovie.EjectMovie();
            this.IsActive = false;
            Game1.Player.UserInterface.CinematicMode = false;
        }
        private void Unpause()
        {
            Game1.mainMenu.ReturnToDefaultState();
            this.Paused = false;
            Game1.Player.UserInterface.CinematicMode = true;
        }

        public void ChangeMovie(MovieName movieName)
        {
            this.CurrentMovie = Movies.Find(x => x.MovieName == movieName);
            this.CurrentMovie.InsertMovie(this.IServiceProvider);
            this.Paused = false;
            Game1.Player.UserInterface.CinematicMode = true;
        }

        public void Update(GameTime gameTime)
        {
            Game1.Player.UserInterface.Update(gameTime, Game1.Player.Inventory);

            if(Game1.KeyboardManager.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Paused = true;
                Action action = new Action(EndMovieEarly);
                Game1.Player.UserInterface.AddAlert(UI.AlertType.Confirmation, Game1.Utility.centerScreen,
                    "Skip Cutscene?", action, new Action(Unpause));
            }
            if(!Paused)
            {
                if (CurrentMovie.Update(gameTime))
                    this.IsActive = false;
            }
           

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            if (this.IsActive)
            {
                CurrentMovie.Draw(spriteBatch);
                Game1.Player.UserInterface.Draw(spriteBatch);
            }
                
        }
    }


}
