
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.Universal;

namespace SecretProject.Class.UI
{
    public class TextBox
    {
        public bool IsActivated { get; set; } = false;
        protected SpriteFont textFont;
        public Vector2 position;
        public string TextToWrite { get; set; }
        protected Texture2D Texture;
        public Rectangle SourceRectangle { get; set; }
        public Rectangle DestinationRectangle { get; set; }

        public KeyboardState oldKeys = Keyboard.GetState();
        public bool RemovesToolBar { get; set; }

        public TextBox(SpriteFont textFont, Vector2 position, string textToWrite, Texture2D texture)
        {
            this.textFont = Game1.AllTextures.MenuText;
            this.position = position;
            this.TextToWrite = textToWrite;
            Texture = texture;

        }

        public TextBox(Vector2 position, int size)
        {
            this.position = position;

            switch (size)
            {
                case 0:
                    this.SourceRectangle = new Rectangle(64, 1088, 800, 288);
                    this.DestinationRectangle = new Rectangle((int)this.position.X, (int)this.position.Y, this.SourceRectangle.Width, this.SourceRectangle.Height);
                    break;
                case 1:
                    this.SourceRectangle = new Rectangle(64, 1088, 800, 288);
                    this.DestinationRectangle = new Rectangle((int)this.position.X, (int)this.position.Y, this.SourceRectangle.Width, this.SourceRectangle.Height);
                    this.RemovesToolBar = true;
                    break;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, bool useString, float scale = 1f)
        {

            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, position, this.SourceRectangle, Color.White, 0f, Game1.Utility.Origin, scale, SpriteEffects.None,Utility.StandardButtonDepth + .05f);
            if (useString)
            {
                spriteBatch.DrawString(Game1.AllTextures.MenuText, this.TextToWrite, position, Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None,Utility.StandardButtonDepth + .06f);
            }

        }
        public virtual void Update(GameTime gameTime, Keys activationKey)
        {

            if (Game1.KeyboardManager.WasKeyPressed(activationKey))
            {
                this.IsActivated = !this.IsActivated;
            }
        }

        public virtual void Update(GameTime gameTime, bool stayActivated)
        {
            this.IsActivated = stayActivated;
        }

        //public virtual void Draw(SpriteBatch spriteBatch, Vector2 textBoxLocation, Vector2 textStartLocation, Rectangle sourceRectangle, float scale)
        //{
        //    spriteBatch.Begin();

        //    if(IsActivated)
        //    {
        //        if(RemovesToolBar)
        //        {
        //            Game1.Player.UserInterface.BottomBar.IsActive = false;
        //        }

        //        spriteBatch.Draw(this.Texture, textBoxLocation, sourceRectangle, Color.White, 0f,
        //            Game1.Utility.Origin, scale,SpriteEffects.None,Utility.StandardButtonDepth);
        //        spriteBatch.DrawString(this.textFont, this.TextToWrite, this.position, Color.White);

        //    }

        //    spriteBatch.End();

        //}
    }
}
