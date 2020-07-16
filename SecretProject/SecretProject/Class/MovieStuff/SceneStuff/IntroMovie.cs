using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SecretProject.Class.MovieStuff.SceneStuff.MovieScene;

namespace SecretProject.Class.MovieStuff.SceneStuff
{
    internal class IntroMovie : Movie
    {
        public IntroMovie( ) : base()
        {
            

        }

        protected override void LoadContent()
        {
            this.MovieScenes = new List<MovieScene>();


            List<MovieProp> scene1Props = new List<MovieProp>();
            Texture2D backgroundTexture = LoadTexture("fierybackground");
            MovieProp background = new MovieProp(backgroundTexture, Vector2.Zero, .1f, Vector2.Zero);
            MovieProp flames = new MovieProp(LoadTexture("flames"), Vector2.Zero, .2f, Vector2.Zero);
            MovieProp airPlane = new MovieProp(LoadTexture("airplane"), Vector2.Zero, .2f, Vector2.Zero);
            scene1Props.Add(background);
        }

        protected override Texture2D LoadTexture(string path)
        {
            return ContentManager.Load<Texture2D>("MovieStuff/IntroMovie/" + path);

        }
    }
}
