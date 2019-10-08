using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
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
        public GraphicsDevice Graphics { get; set; }
        public List<LiftButton> LiftButtons { get; set; }
        public Vector2 Position { get; set; }
        public string CurrentLift { get; set; }

        public LiftWindow(GraphicsDevice graphics)
        {
            this.Graphics = graphics;
            LiftButtons = new List<LiftButton>();
            this.Position = new Vector2(50, 50);
        }

        public void Update(GameTime gameTime)
        {
            for(int i =0; i < LiftButtons.Count; i++)
            {
                LiftButtons[i].Update(Game1.myMouseManager, CurrentLift);

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, Position, new Rectangle(80, 400, 1024, 672), Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            for(int i =0; i < LiftButtons.Count; i++)
            {
                LiftButtons[i].Draw(spriteBatch);
            }
        }

        public void AddLiftKeyButton(string liftKey,string flavorText)
        {
            int count = LiftButtons.Count;
            this.LiftButtons.Add(new LiftButton(this.Graphics, new Vector2(this.Position.X + 200 * count, this.Position.Y), liftKey,flavorText));
        }

    }

    public class LiftButton
    {
        public Vector2 Position { get; set; }
        public Button Button { get; set; }
        public string LiftKey { get; set; }
        public string FlavorText { get; set; }
        public LiftButton(GraphicsDevice graphics,Vector2 position, string liftKey,string flavorText)
        {
            this.Button = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(48, 176, 128, 64), graphics, position);
            this.Position = position;
            this.LiftKey = liftKey;
            this.FlavorText = flavorText;
        }
        public void Update(MouseManager mouse, string currentLift)
        {
            Button.Update(mouse);
            if(Button.isClicked)
            {
                Game1.Lifts[currentLift].Transport(Game1.Lifts[LiftKey]);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
                Button.Draw(spriteBatch, Game1.AllTextures.MenuText, FlavorText + "WarpGate ", Position, Color.White, .69f, .75f);
        }
    }
}
