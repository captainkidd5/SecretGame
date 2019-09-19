using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public class StaminaBar
    {
        public float Stamina { get; set; }
        public float DecayRate { get; set; }
        public Color StaminaColor { get; set; }

        public Texture2D ColoredRectangle { get; set; }
        public Vector2 PositionToDraw { get; set; }





        public StaminaBar(GraphicsDevice graphics,float stamina, float decayRate)
        {
            this.StaminaColor = Color.Green;
            this.PositionToDraw = new Vector2(Game1.Utility.CenterScreenX + Game1.Utility.CenterScreenX - 64,
                Game1.Utility.CenterScreenY - Game1.Utility.CenterScreenY / 2);
            Color[] data = new Color[32*496];
            for (int i = 0; i < data.Length; ++i) data[i] = this.StaminaColor;

            

            this.ColoredRectangle = new Texture2D(graphics,32, 496);
            ColoredRectangle.SetData(data);
            this.Stamina = stamina;
            this.DecayRate = decayRate;
        }

        public void Update(GameTime gameTime)
        {
            Stamina -= (float)(gameTime.ElapsedGameTime.TotalSeconds * DecayRate);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.PositionToDraw, new Rectangle(32, 320, 32, 496), Color.White, 0f,
                    Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            spriteBatch.Draw(ColoredRectangle, PositionToDraw, null,Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
        }
    }
}
