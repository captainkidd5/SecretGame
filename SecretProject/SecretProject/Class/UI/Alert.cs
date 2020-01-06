using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public enum AlertSize
    {
        Small = 0,
        Medium = 1,
        Large = 2
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

        private Button redEsc;

        public Alert(GraphicsDevice graphics,AlertSize size, Vector2 position, string text)
        {
            this.Graphics = graphics;
            LeftRectangle = new Rectangle(1024, 64, 16, 48);
            MiddleRectangle = new Rectangle(1040, 64, 16, 48);
            RightRectangle = new Rectangle(1120, 64, 16, 48);

            switch (size)
            {
                case AlertSize.Small:
                    this.NineSliceRectangle = new NineSliceRectangle(position, LeftRectangle, MiddleRectangle, RightRectangle, RectangleSize.Small);
                    break;
                case AlertSize.Medium:
                    this.NineSliceRectangle = new NineSliceRectangle(position, LeftRectangle, MiddleRectangle, RightRectangle, RectangleSize.Medium);
                    break;

                case AlertSize.Large:
                    this.NineSliceRectangle = new NineSliceRectangle(position, LeftRectangle, MiddleRectangle, RightRectangle, RectangleSize.Large);
                    break;
            }

            this.Position = position;
            this.Text = TextBuilder.ParseText(text, NineSliceRectangle.Width, 2f);

            redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), this.Graphics,
                new Vector2(this.Position.X + this.NineSliceRectangle.Width , this.Position.Y + 32), CursorType.Normal);
        }

        public void Update(GameTime gameTime, List<Alert> alerts)
        {
            redEsc.Update(Game1.myMouseManager);
            if(redEsc.isClicked)
            {
                alerts.Remove(this);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            redEsc.Draw(spriteBatch);
            this.NineSliceRectangle.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Text, new Vector2(this.Position.X + 16, this.Position.Y + 16), Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Utility.StandardButtonDepth + .06f);
        }
    }
}
