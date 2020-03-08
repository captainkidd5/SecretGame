using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.UI.ButtonStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public enum AlertType
    {
        Normal = 0,
        Confirmation = 1,
    }
    public enum AlertSize
    {
        Tiny = 1,
        Small = 2,
        Medium = 5,
        Large = 8,
        XL = 15,
        XXL = 20
    }
    public class Alert
    {
        public GraphicsDevice Graphics { get; set; }
        public NineSliceRectangle NineSliceRectangle { get; set; }
        public Vector2 Position { get; set; }
        public string Text { get; set; }

        public Rectangle LeftRectangle { get; set; }
        public Rectangle MiddleRectangle { get; set; }
        public Rectangle RightRectangle { get; set; }

        protected RedEsc redEsc;

     

        public Alert( GraphicsDevice graphics,AlertSize size, Vector2 position, string text)
        {

            this.Graphics = graphics;
            LeftRectangle = new Rectangle(1024, 64, 16, 48);
            MiddleRectangle = new Rectangle(1040, 64, 16, 48);
            RightRectangle = new Rectangle(1120, 64, 16, 48);


                    this.NineSliceRectangle = new NineSliceRectangle(position, LeftRectangle, MiddleRectangle, RightRectangle, (RectangleSize)size);


            this.Position = position;
            this.Text = TextBuilder.ParseText(text, NineSliceRectangle.Width, 1f);

            redEsc = new RedEsc(new Vector2(this.Position.X + this.NineSliceRectangle.Width - 32, this.Position.Y + 32),this.Graphics);
        }

        public virtual void Update(GameTime gameTime, List<Alert> alerts)
        {
            Game1.freeze = true;
            redEsc.Update(Game1.MouseManager);
            if(redEsc.isClicked)
            {
                Game1.freeze = false;
                alerts.Remove(this);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            redEsc.Draw(spriteBatch);
            this.NineSliceRectangle.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Text, new Vector2(this.Position.X + 16, this.Position.Y + 16), Color.White, 0f, Game1.Utility.Origin,1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .06f);
        }
    }
}
