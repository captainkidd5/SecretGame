using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public class RisingText
    {

        public bool RisingTextActive { get; set; }
        public Vector2 RisingTextPosition { get; set; }
        public float YStart { get; set; }
        public float YEnd { get; set; }
        public string RisingTextString { get; set; }
        public float Speed { get; set; }
        public Color Color { get; set; }
        public bool Fade { get; set; }
        public float Scale { get; set; }


        public RisingText(float yStart, float yEnd, string stringToWrite, float speed, Color color, bool fade = true, float scale = .5f)
        {

            this.YStart = yStart;
            this.YEnd = yEnd;
            this.RisingTextString = stringToWrite;
            this.Speed = speed;
            this.Color = color;
            this.Fade = fade;
            this.Scale = scale;
        }

        public void Update(GameTime gameTime, List<RisingText> allRisingText)
        {

            if (UpdateRisingText(gameTime))
            {
                Console.WriteLine("rising text active!");
            }
            else
            {
                allRisingText.Remove(this);
            }

        }


        public bool UpdateRisingText(GameTime gameTime)
        {
             if (this.YStart > this.YEnd)
            {
                YStart -= (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;

                this.RisingTextPosition = new Vector2(Game1.Player.position.X, YStart);
                if(this.Fade)
                {
                    Color = this.Color * .98f;
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.RisingTextString, this.RisingTextPosition, Color, 0f, Game1.Utility.Origin, Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
        }


    }
}
