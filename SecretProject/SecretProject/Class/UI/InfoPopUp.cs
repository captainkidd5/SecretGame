using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public class InfoPopUp
    {
        public Vector2 WindowPosition { get; set; }
        public string StringToWrite { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public bool IsActive { get; set; }

        public bool TextFitted { get; set; }

        public InfoPopUp(string stringToWrite)
        {
            this.SourceRectangle = new Rectangle(1024, 64, 112, 48);
            this.StringToWrite = stringToWrite;
            this.TextFitted = false;
        }

        public void Update(GameTime gameTime)
        {
            if(this.IsActive)
            {

            }
        }

        public void FitText(string text,float scale)
        {
            this.StringToWrite = Game1.Player.UserInterface.TextBuilder.ParseText(text, this.SourceRectangle.Width * 2.5f, 1.5f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(this.IsActive)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, WindowPosition, this.SourceRectangle, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, .9f);
                spriteBatch.DrawString(Game1.AllTextures.MenuText, this.StringToWrite, new Vector2(this.WindowPosition.X + 16, this.WindowPosition.Y + 16), Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, .91f);
            }
            
        }
    }
}
