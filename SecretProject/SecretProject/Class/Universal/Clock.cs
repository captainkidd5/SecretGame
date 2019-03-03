using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SecretProject.Class.UI;

namespace SecretProject.Class.Universal
{
    public class Clock
    {
        public int GlobalTime { get; private set; } = 0;
        public Vector2 ClockPosition;

        TextBox ClockDisplay;

        public Clock()
        {
            ClockPosition = new Vector2(900, 100);
            ClockDisplay = new TextBox(Game1.AllTextures.MenuText, ClockPosition, GlobalTime.ToString(), Game1.AllTextures.ClockBackground);

        }

        public void Update(GameTime gameTime)
        {
            GlobalTime += (int)(Math.Ceiling(gameTime.ElapsedGameTime.TotalSeconds));
            ClockDisplay.TextToWrite = GlobalTime.ToString();
            ClockDisplay.Update(gameTime, true);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ClockDisplay.Draw(spriteBatch);
        }
    }
}
