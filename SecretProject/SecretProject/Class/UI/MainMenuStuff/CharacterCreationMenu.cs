using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.MainMenuStuff
{
    public class CharacterCreationMenu
    {
        public GraphicsDevice Graphics { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle BackGroundSourceRectangle { get; set; }
        public Rectangle CharacterPortraitWindow { get; set; }
        public TypingWindow TypingWindow { get; set; }
        public CharacterCreationMenu(GraphicsDevice graphics, Vector2 position)
        {
            this.Graphics = graphics;
            this.Position = position;
            this.BackGroundSourceRectangle = new Rectangle(832, 496, 192, 160);
            this.CharacterPortraitWindow = new Rectangle(896, 432, 64, 64);
            this.TypingWindow = new TypingWindow(graphics, position);
        }
        public void Update(GameTime gameTime)
        {
            this.TypingWindow.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackGroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, Game1.Utility.StandardTextDepth - .04f);
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet,new Vector2(this.Position.X + this.BackGroundSourceRectangle.Width /2 - this.CharacterPortraitWindow.Width,
                this.Position.Y + this.BackGroundSourceRectangle.Height/ 2 - this.CharacterPortraitWindow.Height),this.CharacterPortraitWindow, Color.White, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, Game1.Utility.StandardTextDepth);
            this.TypingWindow.Draw(spriteBatch);
        }
    }
}
