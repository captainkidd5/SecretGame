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
            ////////////////////////////////
            ///
            #region SCENE 0
            List<MovieProp> scene0Props = new List<MovieProp>();
            Texture2D backgroundTexture0 = LoadTexture("slide0");
            MovieProp background0 = new MovieProp(backgroundTexture0,new Vector2(0, 50), .1f, new Vector2(0, -25), 1f);

            scene0Props.Add(background0);

            List<MovieSound> movieSounds0 = new List<MovieSound>()
            {
                //airRaid,
            };


            string string00 = "The world was once peaceful.";
            string string01 = "Plants and animals lived in harmony with all who roamed the land.";
            string string02 = " All nations shared one thing in common - a deep connection with nature. ";
            string string03 = "The realm itself was a symbol of unity, and all who cared for it were met with kindness and prosperity. ";


            List<string> scene0Strings = new List<string>()
            {
                string00,
                string01,
               // string02,
               // string03
            };
            MovieText movieText0 = new MovieText(scene0Strings);

            MovieScenes.Add(new MovieScene("Peace Scene", scene0Props, movieSounds0,movieText0, 10));
            #endregion
            //////////////////////////////////
            ///
            #region SCENE 1

            

            List<MovieProp> scene1Props = new List<MovieProp>();
            Texture2D backgroundtexture1 = LoadTexture("fierybackground");
            MovieProp background1 = new MovieProp(backgroundtexture1, Vector2.Zero, .1f, Vector2.Zero,1f);
            MovieProp flames = new MovieProp(LoadTexture("flames"), new Vector2(200, 200), .2f, new Vector2(-10,-10), 1f);
            Texture2D airplaneTexture = LoadTexture("airplane");
            MovieProp airPlane = new MovieProp(airplaneTexture, new Vector2(1200, 350), .4f, new Vector2(-50, 0),1f);
            MovieProp airPlane1 = new MovieProp(airplaneTexture, new Vector2(1400,100), .35f, new Vector2(-25, 0), .5f);
            MovieProp airPlane2 = new MovieProp(airplaneTexture, new Vector2(400, 100), .32f, new Vector2(-15, 0), .25f);
            MovieProp airPlane3 = new MovieProp(airplaneTexture, new Vector2(800, 50), .3f, new Vector2(-12, 0), .12f);
            MovieProp airPlane4 = new MovieProp(airplaneTexture, new Vector2(500, 300), .3f, new Vector2(-15, 0), .4f);
            MovieProp airPlane5 = new MovieProp(airplaneTexture, new Vector2(1100, 100), .3f, new Vector2(-8, 0), .05f);
            scene1Props.Add(background1);
            scene1Props.Add(flames);
            scene1Props.Add(airPlane5);
            scene1Props.Add(airPlane4);
            scene1Props.Add(airPlane3);
            scene1Props.Add(airPlane2);
            scene1Props.Add(airPlane1);
            scene1Props.Add(airPlane);

            MovieSound airRaid = new MovieSound(LoadSound("airRaid"), new List<int>() { 0 });
            List<MovieSound> movieSounds1 = new List<MovieSound>()
            {
                airRaid,
            };

            MovieScenes.Add(new MovieScene("bombing scene", scene1Props, movieSounds1, movieText0, 20f));

            #endregion
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
