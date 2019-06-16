using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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

        public KeyboardState oldKeys = Keyboard.GetState();

        public TextBox(SpriteFont textFont, Vector2 position, string textToWrite, Texture2D texture)
        {
            this.textFont = textFont;
            this.position = position;
            this.TextToWrite = textToWrite;
            this.Texture = texture;
        }

        public TextBox(Vector2 position, int size)
        {
            this.position = position;

            switch (size)
            {
                case 1:
                    this.SourceRectangle = new Rectangle(96, 416, 640, 90);
                    break;
            }
        }

        public virtual void DrawWithoutString(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, position, this.SourceRectangle, Color.White, 0f, Game1.Utility.Origin, 1f,   SpriteEffects.None, Game1.Utility.StandardButtonDepth);

        }
        public virtual void Update(GameTime gameTime, Keys activationKey)
        {

            if(Game1.OldKeyBoardState.IsKeyDown(activationKey) && !Game1.NewKeyBoardState.IsKeyDown(activationKey))
            {
                IsActivated = !IsActivated;
            }
        }

        public virtual void Update(GameTime gameTime, bool stayActivated)
        {
            IsActivated = stayActivated;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if(IsActivated)
            {
                
                spriteBatch.Draw(this.Texture, new Vector2(this.position.X,this.position.Y), new Rectangle(48, 176, 128, 64), Color.White, 0f,
                    Game1.Utility.Origin, 1f,SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                spriteBatch.DrawString(textFont, TextToWrite, position, Color.Red, 0f, Game1.Utility.Origin,1f, SpriteEffects.None, Game1.Utility.StandardTextDepth);
            }
            spriteBatch.End();

        }
    }
}
