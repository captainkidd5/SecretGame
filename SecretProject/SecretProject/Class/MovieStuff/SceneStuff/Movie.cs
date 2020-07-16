using Microsoft.Xna.Framework;
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
        public Movie()
        {

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


        public virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.MovieScenes.Count; i++)
            {
                MovieScenes[i].Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.MovieScenes.Count; i++)
            {
                MovieScenes[i].Draw(spriteBatch);
            }
        }
    }
}
