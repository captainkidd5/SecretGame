using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public IntroMovie(MovieName movieName ) : base(movieName)
        {
            

        }

        protected override void LoadContent()
        {
            this.MovieScenes = new List<MovieScene>();


            List<MovieProp> scene1Props = new List<MovieProp>();
            Texture2D backgroundTexture = LoadTexture("fierybackground");
            MovieProp background = new MovieProp(backgroundTexture, Vector2.Zero, .1f, Vector2.Zero,1f);
            MovieProp flames = new MovieProp(LoadTexture("flames"), new Vector2(200, 200), .2f, new Vector2(-10,-10), 1f);
            Texture2D airplaneTexture = LoadTexture("airplane");
            MovieProp airPlane = new MovieProp(airplaneTexture, new Vector2(800, 350), .4f, new Vector2(-50, 0),1f);
            MovieProp airPlane1 = new MovieProp(airplaneTexture, new Vector2(1000,400), .35f, new Vector2(-25, 0), .5f);
            MovieProp airPlane2 = new MovieProp(airplaneTexture, new Vector2(400, 100), .32f, new Vector2(-10, 0), .15f);
            MovieProp airPlane3 = new MovieProp(airplaneTexture, new Vector2(600, 50), .3f, new Vector2(-5, 0), .1f);
            scene1Props.Add(background);
            scene1Props.Add(flames);
            scene1Props.Add(airPlane3);
            scene1Props.Add(airPlane2);
            scene1Props.Add(airPlane1);
            scene1Props.Add(airPlane);

            MovieScenes.Add(new MovieScene("bombing scene", scene1Props, 18f));
        }

        protected override Texture2D LoadTexture(string path)
        {
            return ContentManager.Load<Texture2D>("MovieStuff/IntroMovie/" + path);

        }

        protected override SoundEffect LoadSound(string path)
        {
            return ContentManager.Load<SoundEffect>("MovieStuff/IntroMovie/" + path);
        }
    }
}
