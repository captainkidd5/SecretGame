using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MenuStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public class LiftWindow
    {
        public List<Button> LiftButtons { get; set; }

        public LiftWindow()
        {
            LiftButtons = new List<Button>();
        }

        public void Update(GameTime gameTime)
        {
            for(int i =0; i < LiftButtons.Count; i++)
            {
                LiftButtons[i].Update(Game1.myMouseManager);
                if()
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

    }

    public class LiftButton
    {
        public Vector2 Position { get; set; }
        public Button Button { get; set; }
        public LiftButton(Vector2 position)
        {

        }
    }
}
