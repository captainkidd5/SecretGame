using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MenuStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Scale = scale;
            SliderBackground = new Rectangle(48, 160, 112, 16);
            SliderBackgroundPosition = position;
            MinSliderX = (int)position.X;
            MaxSliderX = (int)position.X + 100 * (int)Scale;
            SliderButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(64,144,16,16), graphics, new Vector2(MaxSliderX, position.Y), Controls.CursorType.Normal, scale);
            
            DisplayValue = 100;
        }

        public float Update( float valueToAffect)
        {
            SliderButton.Update(Game1.myMouseManager);
            if(SliderButton.isClickedAndHeld)
            {
                if(Game1.myMouseManager.UIPosition.X > Game1.myMouseManager.OldMouseInterfacePosition.X)
                {
                    SliderButton.Position = new Vector2(SliderButton.Position.X + Game1.myMouseManager.UIPosition.X - Game1.myMouseManager.OldMouseInterfacePosition.X, SliderButton.Position.Y);
                }
                else if(Game1.myMouseManager.UIPosition.X < Game1.myMouseManager.OldMouseInterfacePosition.X)
                {
                    SliderButton.Position = new Vector2(SliderButton.Position.X - Math.Abs(Game1.myMouseManager.OldMouseInterfacePosition.X- Game1.myMouseManager.UIPosition.X), SliderButton.Position.Y);
                }
            }

            if (SliderButton.Position.X > MaxSliderX)
            {
                SliderButton.Position.X = MaxSliderX;
            }
            if (SliderButton.Position.X < MinSliderX)
            {
                SliderButton.Position.X = MinSliderX;
            }

            DisplayValue = (int)(((int)SliderButton.Position.X - MinSliderX)/Scale);
            float floatDisplayValue = (float)DisplayValue;
            return floatDisplayValue / 100;
        }

        public void Draw(SpriteBatch spriteBatch, string textToIdentifyNumber)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, SliderBackgroundPosition, SliderBackground, Color.White, 0f, Game1.Utility.Origin, Scale,   SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);
            SliderButton.DrawNormal(spriteBatch, SliderButton.Position, SliderButton.BackGroundSourceRectangle, SliderButton.Color, 0f, Game1.Utility.Origin, Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .02f);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, textToIdentifyNumber + DisplayValue.ToString(), new Vector2(SliderBackgroundPosition.X, SliderBackgroundPosition.Y - 100), Color.Black, 0f, Game1.Utility.Origin, Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);
        }
    }
}
