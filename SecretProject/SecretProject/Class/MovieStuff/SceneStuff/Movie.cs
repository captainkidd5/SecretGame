using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
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

        protected List<MovieScene> MovieScenes { get; set; }
        public ContentManager ContentManager { get; set; }
        private int SceneIndex { get; set; }

        public MovieName MovieName { get; set; }
        public Movie(MovieName movieName)
        {
            this.MovieName = movieName;
        }

        protected virtual void LoadContent()
        {

        }

        public void InsertMovie(IServiceProvider serviceProvider)
        {
            LoadContentManager(serviceProvider);
            LoadContent();
        }

        public void EjectMovie()
        {
            this.ContentManager.Unload();
        }

        protected void LoadContentManager(IServiceProvider serviceProvider)
        {
            this.ContentManager = new ContentManager(serviceProvider);
            ContentManager.RootDirectory = "Content";
        }

        protected virtual Texture2D LoadTexture(string path)
        {
            return ContentManager.Load<Texture2D>(path);
        }

        protected virtual SoundEffect LoadSound(string path)
        {
            return ContentManager.Load<SoundEffect>(path);
        }


        /// <summary>
        /// returns true if movie is finished.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public virtual bool Update(GameTime gameTime)
        {
            if (!MovieScenes[SceneIndex].Update(gameTime))
            {
                if (SceneIndex >= MovieScenes.Count - 1)
                {
                    EjectMovie(); //movie is finished, unload stuff.
                    return true;
                }
                SceneIndex++;
            }
            return false;

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            MovieScenes[SceneIndex].Draw(spriteBatch);
        }
    }
}
