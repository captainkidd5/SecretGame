using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.MovieStuff.SceneStuff
{
    internal class MovieScene
    {
        public string Name { get; set; }
        private List<MovieProp> Props { get; set; }

        public SimpleTimer SimpleTimer { get; set; }

        public MovieScene(string name, List<MovieProp> movieProps, float duration)
        {
            this.Name = name;
            this.Props = movieProps;
            this.SimpleTimer = new SimpleTimer(duration);

        }

        /// <summary>
        /// returns true if is still in progress.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public bool Update(GameTime gameTime)
        {
            if(!SimpleTimer.Run(gameTime))
            {
                for (int i = 0; i < this.Props.Count; i++)
                {
                    Props[i].Update(gameTime);
                }
                return true;
            }
            return false;
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.Props.Count; i++)
            {
                Props[i].Draw(spriteBatch);
            }
        }

        public class MovieProp
        {
            private Texture2D Texture { get; set; }
            private Vector2 Position { get; set; }
            private float LayerDepth { get; set; }

            private Vector2 Velocity { get; set; }
            public MovieProp(Texture2D texture, Vector2 position, float layerDepth, Vector2 velocity)
            {
                this.Texture = texture;
                this.Position = position;
                this.LayerDepth = layerDepth;
                this.Velocity = velocity;
            }

            public void Update(GameTime gameTIme)
            {
                this.Position += Velocity * (float)gameTIme.ElapsedGameTime.TotalSeconds;
            }
            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(this.Texture, this.Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, this.LayerDepth);
            }
        }
    }
}
