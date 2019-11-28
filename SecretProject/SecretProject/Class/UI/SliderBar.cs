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
            SliderBackground = new Rectangle(48, 160, 100, 16);
            SliderBackgroundPosition = position;
            SliderButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(64,144,16,16), graphics, position, Controls.CursorType.Normal, scale);
            Scale = scale;
        }

        public void Update()
        {
            SliderButton.Update(Game1.myMouseManager);
            if(SliderButton.isClickedAndHeld)
            {

            }

            if(SliderButton.Position.X > MaxSliderX)
            {
                SliderButton.Position.X = MaxSliderX;
            }
            if(SliderButton.Position.X < MinSliderX)
            {
                SliderButton.Position.X = MinSliderX;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, SliderBackgroundPosition, SliderBackground, Color.White, 0f, Game1.Utility.Origin, Scale,   SpriteEffects.None, Game1.Utility.StandardButtonDepth + .01f);
            SliderButton.Draw(spriteBatch, Game1.AllTextures.MenuText, this.DisplayValue.ToString(), SliderBackgroundPosition, Color.Black, Game1.Utility.StandardButtonDepth + .01f, Game1.Utility.StandardButtonDepth + .01f);
        }
    }
}
