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
    public class CategoryTab
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public bool IsActive { get; set; }
        public int ActivePage { get; set; }
        public List<IPage> Pages { get; set; }

        public Button Button { get; set; }

        public Vector2 PositionToDraw { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public float ButtonColorMultiplier { get; set; }
        public CategoryTab( string name, GraphicsDevice graphics, Vector2 positionToDraw, Rectangle sourceRectangle, float scale)
        {
            this.Name = name;
 
            this.PositionToDraw = positionToDraw;
            this.SourceRectangle = sourceRectangle;
            this.Button = new Button(Game1.AllTextures.UserInterfaceTileSet, SourceRectangle, graphics, PositionToDraw, CursorType.Normal, scale);
            this.ButtonColorMultiplier = 1f;
            this.ActivePage = 0;
            this.Pages = new List<IPage>();
        }

        public void Update(GameTime gameTime)
        {

            Pages[ActivePage].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch,  Rectangle backDropSourceRectangle, float backDropScale, bool drawString = true)
        {
            if(drawString)
            {
                spriteBatch.DrawString(Game1.AllTextures.MenuText, ActivePage.ToString(), PositionToDraw, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);
                spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Name, new Vector2(PositionToDraw.X + backDropSourceRectangle.Width / 8 * backDropScale, PositionToDraw.Y + 32), Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);
            }
            
            this.Pages[ActivePage].Draw(spriteBatch);


        }


    }
}
