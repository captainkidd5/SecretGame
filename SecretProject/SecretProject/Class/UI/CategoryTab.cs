using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.UI.ButtonStuff;
using SecretProject.Class.Universal;
using System.Collections.Generic;

namespace SecretProject.Class.UI
{
    /// <summary>
    /// For use with ESC menu
    /// </summary>
    public class CategoryTab
    {
        public string Name { get; private set; }
        public int Index { get; private set; }
        public bool IsActive { get; set; }
        public int ActivePage { get; set; }
        public List<IPage> Pages { get; set; }

        public Button Button { get; set; }

        public Vector2 PositionToDraw { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public float ButtonColorMultiplier { get; set; }
        public CategoryTab(string name, GraphicsDevice graphics, Vector2 positionToDraw, Rectangle sourceRectangle, float scale)
        {
            this.Name = name;

            this.PositionToDraw = positionToDraw;
            this.SourceRectangle = sourceRectangle;
            this.Button = new Button(Game1.AllTextures.UserInterfaceTileSet, this.SourceRectangle, graphics, this.PositionToDraw, CursorType.Normal, scale);
            this.ButtonColorMultiplier = 1f;
            this.ActivePage = 0;
            this.Pages = new List<IPage>();
        }

        public void Update(GameTime gameTime)
        {

            this.Pages[this.ActivePage].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle backDropSourceRectangle, float backDropScale, bool drawString = true)
        {
            if (drawString)
            {
                spriteBatch.DrawString(Game1.AllTextures.MenuText, this.ActivePage.ToString(), this.PositionToDraw, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None,Utility.StandardButtonDepth + .01f);
                spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Name, new Vector2(this.PositionToDraw.X + backDropSourceRectangle.Width / 8 * backDropScale, this.PositionToDraw.Y + 32), Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None,Utility.StandardButtonDepth + .01f);
            }

            this.Pages[this.ActivePage].Draw(spriteBatch);


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Pages[this.ActivePage].Draw(spriteBatch);
        }

    }
}
