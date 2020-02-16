using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.DayTransitionStuff
{
    public class DayTransitioner
    {
        public Vector2 Position { get; set; }
        public string Text { get; set; }
        public float Scale { get; set; }

        public DayTransitioner()
        {
            this.Position = Game1.Utility.centerScreen;
            
            this.Scale = 3f;
        }

        public void UpdateText()
        {
            this.Text = Game1.GlobalClock.GetStringFromTime();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Text, this.Position, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardTextDepth);
        }
    }
}
