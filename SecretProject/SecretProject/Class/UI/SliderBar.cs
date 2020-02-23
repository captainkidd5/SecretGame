using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.Universal;
using System;

namespace SecretProject.Class.UI
{
    public class SliderBar
    {
        public Button SliderButton { get; set; }
        public Rectangle SliderBackground { get; set; }
        public Vector2 SliderBackgroundPosition { get; set; }
        public int MaxSliderX { get; set; }
        public int MinSliderX { get; set; }
        public float Scale { get; set; }
        public int DisplayValue { get; set; }
        public SliderBar(GraphicsDevice graphics, Vector2 position, float scale)
        {
            this.Scale = scale;
            this.SliderBackground = new Rectangle(48, 160, 112, 16);
            this.SliderBackgroundPosition = position;
            this.MinSliderX = (int)position.X;
            this.MaxSliderX = (int)position.X + 100 * (int)this.Scale;
            this.SliderButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(64, 144, 16, 16), graphics, new Vector2(this.MaxSliderX, position.Y), Controls.CursorType.Normal, scale);

            this.DisplayValue = 100;
        }

        public float Update(float valueToAffect)
        {
            this.SliderButton.Update(Game1.MouseManager);
            if (this.SliderButton.isClickedAndHeld)
            {
                if (Game1.MouseManager.UIPosition.X > Game1.MouseManager.OldMouseInterfacePosition.X)
                {
                    this.SliderButton.Position = new Vector2(this.SliderButton.Position.X + Game1.MouseManager.UIPosition.X - Game1.MouseManager.OldMouseInterfacePosition.X, this.SliderButton.Position.Y);
                }
                else if (Game1.MouseManager.UIPosition.X < Game1.MouseManager.OldMouseInterfacePosition.X)
                {
                    this.SliderButton.Position = new Vector2(this.SliderButton.Position.X - Math.Abs(Game1.MouseManager.OldMouseInterfacePosition.X - Game1.MouseManager.UIPosition.X), this.SliderButton.Position.Y);
                }
            }

            if (this.SliderButton.Position.X > this.MaxSliderX)
            {
                this.SliderButton.Position.X = this.MaxSliderX;
            }
            if (this.SliderButton.Position.X < this.MinSliderX)
            {
                this.SliderButton.Position.X = this.MinSliderX;
            }

            this.DisplayValue = (int)(((int)this.SliderButton.Position.X - this.MinSliderX) / this.Scale);
            float floatDisplayValue = (float)this.DisplayValue;
            return floatDisplayValue / 100;
        }

        public void Draw(SpriteBatch spriteBatch, string textToIdentifyNumber)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.SliderBackgroundPosition, this.SliderBackground, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Utility.StandardButtonDepth + .01f);
            this.SliderButton.DrawNormal(spriteBatch, this.SliderButton.Position, this.SliderButton.BackGroundSourceRectangle, this.SliderButton.Color, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Utility.StandardButtonDepth + .02f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, textToIdentifyNumber + this.DisplayValue.ToString(), new Vector2(this.SliderBackgroundPosition.X, this.SliderBackgroundPosition.Y - 100), Color.Black, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Utility.StandardButtonDepth + .01f);
        }
    }
}
