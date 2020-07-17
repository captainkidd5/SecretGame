using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private List<MovieSound> SoundEffects { get; set; }
        private MovieText Text { get; set; }

        public SimpleTimer SimpleTimer { get; set; }

        public MovieScene(string name, List<MovieProp> movieProps, List<MovieSound> soundEffects, MovieText movieText,float duration)
        {
            this.Name = name;
            this.Props = movieProps;
            this.SoundEffects = soundEffects;
            this.SimpleTimer = new SimpleTimer(duration);
            this.Text = movieText;
        }

        /// <summary>
        /// returns true if is still in progress.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public bool Update(GameTime gameTime)
        {
            if (!SimpleTimer.Run(gameTime))
            {
                for (int i = 0; i < this.Props.Count; i++)
                {
                    Props[i].Update(gameTime);
                }
                for(int i =0; i < this.SoundEffects.Count; i++)
                {
                    SoundEffects[i].Update(SimpleTimer);
                }
                Text.Update(gameTime);
                return true;
            }
            return false;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);

            for (int i = 0; i < this.Props.Count; i++)
            {
                Props[i].Draw(spriteBatch);
            }
            Text.Draw(spriteBatch);
            spriteBatch.End();
        }

        public class MovieProp
        {
            private Texture2D Texture { get; set; }
            private Vector2 Position { get; set; }
            private float LayerDepth { get; set; }
            private float Scale { get; set; }

            private Vector2 Velocity { get; set; }
            public MovieProp(Texture2D texture, Vector2 position, float layerDepth, Vector2 velocity, float scale)
            {
                this.Texture = texture;
                this.Position = position;
                this.LayerDepth = layerDepth;
                this.Velocity = velocity;
                this.Scale = scale;
            }

            public void Update(GameTime gameTIme)
            {
                this.Position += Velocity * (float)gameTIme.ElapsedGameTime.TotalSeconds;
            }
            public void Draw(SpriteBatch spriteBatch)
            {
                
                spriteBatch.Draw(this.Texture, this.Position, null, Color.White, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, this.LayerDepth);
                
            }
        }

        public class MovieSound
        {
            private SoundEffect SoundEffect { get; set; }
            private List<int> TimeStamps { get; set; }
            public MovieSound(SoundEffect soundEffect, List<int> timeStamps)
            {
                this.SoundEffect = soundEffect;
                this.TimeStamps = timeStamps;
            }

            /// <summary>
            /// Play sound every time the timer is greater than the time stamp. 
            /// </summary>
            /// <param name="simpleTimer"></param>
            public void Update(SimpleTimer simpleTimer)
            {
                for(int i = 0; i < this.TimeStamps.Count; i++)
                {
                    if(TimeStamps[i] < simpleTimer.Time)
                    {
                        Game1.SoundManager.PlaySoundEffect(this.SoundEffect);
                        TimeStamps.RemoveAt(i);
                    }
                }
            }
        }

        public class MovieText
        {
            private List<string> Lines { get; set; }
            private List<Vector2> LinePositions{ get; set; }
            private float TextScale { get; set; } = 3f;

            public MovieText(List<string> lines)
            {
                this.Lines = lines;
                // this.LinePositions = linePositions;
                LinePositions = new List<Vector2>();

                Vector2 basePosition = new Vector2(80, Game1.ScreenHeight - 320);
                for (int i = 0; i < this.Lines.Count; i++)
                {
                    LinePositions.Add(new Vector2(basePosition.X, basePosition.Y + 32 * i * TextScale));
                }
            }

            public void Update(GameTime gameTime)
            {
                for(int i =0; i < this.Lines.Count; i++)
                {
                    
                }
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                for (int i = 0; i < this.Lines.Count; i++)
                {
                    spriteBatch.DrawString(Game1.AllTextures.MenuText, Lines[i],
                        LinePositions[i], Color.White, 0f, Vector2.Zero, TextScale, SpriteEffects.None, Utility.StandardTextDepth);
                }
            }
        }
    }
}
