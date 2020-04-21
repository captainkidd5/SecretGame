using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.Universal;
using XMLData.ItemStuff;

namespace SecretProject.Class.UI
{
    public enum LineLimits
    {
        ItemDescriptions = 200,

    }

    public class InfoPopUp
    {
        public Vector2 WindowPosition { get; set; }
        public string StringToWrite { get; set; }

        public bool IsActive { get; set; }

        public bool TextFitted { get; set; }

        public Color Color { get; set; }

        public string TitleString { get; set; }
        public bool DisplayTitle { get; set; }

        public NineSliceRectangle FittedRectangle { get; set; }
        public float LineLimit { get; set; }

        public float TextScale { get; private set; }

        public InfoPopUp(string stringToWrite, Vector2 windowPosition)
        {

            this.StringToWrite = stringToWrite;
            this.WindowPosition = windowPosition;
            this.TitleString = string.Empty;
            this.TextFitted = false;
            this.Color = Color.White;

            this.TextScale = 1f;

            this.FittedRectangle = new NineSliceRectangle(this.WindowPosition, this.StringToWrite, this.TextScale);
        }

        public InfoPopUp(ItemData itemData, Vector2 windowPosition)
        {

            this.StringToWrite = itemData.Name + "\n" + itemData.Description;
            this.WindowPosition = windowPosition;
            this.TitleString = string.Empty;
            this.TextFitted = false;
            this.Color = Color.White;

            this.TextScale = 1f;

            this.FittedRectangle = new NineSliceRectangle(this.WindowPosition, this.StringToWrite,this.TextScale);
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {

            }
        }

        public string FitText(string text, float scale)
        {
            this.StringToWrite = TextBuilder.ParseText(text, this.FittedRectangle.Width * 2.5f, 1.5f);
            return StringToWrite;
        }

        public string FitTitleText(string text, float scale)
        {
         //   this.TitleString = TextBuilder.ParseText(text, this.TitleSourceRectangle.Width * 2.5f, 1.5f);
            return StringToWrite;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsActive)
            {
                this.FittedRectangle.Draw(spriteBatch);
              //  spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.WindowPosition, this.SourceRectangle, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .05f);
                spriteBatch.DrawString(Game1.AllTextures.MenuText, this.StringToWrite, new Vector2(this.WindowPosition.X + 16, this.WindowPosition.Y + 16), this.Color, 0f, Game1.Utility.Origin,this.TextScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .06f);
                if(this.DisplayTitle)
                {
                  //  spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(this.WindowPosition.X + 24, this.WindowPosition.Y - 46), this.TitleSourceRectangle, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .05f);
                    spriteBatch.DrawString(Game1.AllTextures.MenuText, this.TitleString, new Vector2(this.WindowPosition.X + 34, this.WindowPosition.Y - 24), this.Color, 0f, Game1.Utility.Origin, this.TextScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .06f);
                }
            }

        }
    }
}
