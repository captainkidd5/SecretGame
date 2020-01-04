using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Universal;

namespace SecretProject.Class.UI
{
    public class StaminaBar
    {
        public int Stamina { get; set; }
        public float Timer { get; set; }
        public float DecayRate { get; set; }
        public Color StaminaColor { get; set; }

        public Texture2D ColoredRectangle { get; set; }
        public Vector2 PositionToDraw { get; set; }
        public int Red { get; set; }

        public Vector2 ColoredRectanglePosition { get; set; }
        public int ColoredRectangleHeight { get; set; }
        public GraphicsDevice graphics { get; set; }



        public StaminaBar(GraphicsDevice graphics, int stamina, float decayRate)
        {
            this.StaminaColor = Color.Green;
            this.PositionToDraw = new Vector2(Game1.Utility.CenterScreenX + Game1.Utility.CenterScreenX - 64,
                Game1.Utility.CenterScreenY - Game1.Utility.CenterScreenY / 2);
            Color[] data = new Color[32 * 496];
            for (int i = 0; i < data.Length; ++i) data[i] = this.StaminaColor;



            this.ColoredRectangle = new Texture2D(graphics, 32, 496);
            this.ColoredRectangle.SetData(data);
            this.Stamina = stamina;

            this.DecayRate = decayRate;
            this.Timer = decayRate;
            this.Red = 0;
            this.ColoredRectanglePosition = this.PositionToDraw;
            this.ColoredRectangleHeight = 496;
            this.graphics = graphics;
        }

        public void Update(GameTime gameTime)
        {
            if (Game1.GetCurrentStageInt() == Stages.OverWorld)
            {
                /*

                float oldStam = Stamina;
                Timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Timer <= 0)
                {
                    if (Stamina <= 0)
                    {
                        //Game1.Player.Health -= 1;
                        Stamina = 100;
                        return;
                    }
                    Stamina -= 1;
                    Red = 256 - (int)Stamina;
                    if (this.StaminaColor.R < 250)
                    {
                        this.StaminaColor = new Color(Red, StaminaColor.G, StaminaColor.B);
                    }
                    ColoredRectangleHeight -= 1;
                    ColoredRectanglePosition = new Vector2(ColoredRectanglePosition.X, ColoredRectanglePosition.Y + 1);
                    ColoredRectangle = new Texture2D(graphics, ColoredRectangle.Width, ColoredRectangleHeight + 1);
                    Color[] data = new Color[32 * ColoredRectangle.Height];
                    for (int i = 0; i < data.Length; ++i) data[i] = this.StaminaColor;
                    ColoredRectangle.SetData(data);
                    Timer = DecayRate;
                }
                if (Game1.myMouseManager.IsClicked)
                {
                    if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem() != null)
                    {


                        if (Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().StaminaRestored != 0)
                        {

                            int staminaData = Game1.Player.UserInterface.BottomBar.GetCurrentEquippedToolAsItem().StaminaRestored;
                            int staminaToRestore = staminaData + this.Stamina;
                            if (staminaData + this.Stamina > 100)
                            {
                                staminaToRestore = 100 - this.Stamina;
                            }
                            this.Stamina += staminaToRestore;
                            ColoredRectanglePosition = new Vector2(ColoredRectanglePosition.X,
                                ColoredRectanglePosition.Y - staminaToRestore);
                            ColoredRectangleHeight += staminaToRestore;
                            ColoredRectangle = new Texture2D(graphics, ColoredRectangle.Width, ColoredRectangleHeight);
                            Color[] data = new Color[32 * ColoredRectangle.Height];
                            for (int i = 0; i < data.Length; ++i) data[i] = this.StaminaColor;
                            ColoredRectangle.SetData(data);

                            Game1.Player.Inventory.RemoveItem(Game1.Player.UserInterface.BottomBar.GetCurrentEquippedTool());


                        }

                    }
                }
                if (this.Stamina > 100)
                {
                    this.Stamina = 100;
                }
                */
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Game1.GetCurrentStageInt() == Stages.OverWorld)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.PositionToDraw, new Rectangle(32, 320, 32, 496), Color.White, 0f,
                    Game1.Utility.Origin, 1f, SpriteEffects.None, Utility.StandardButtonDepth);
                spriteBatch.Draw(this.ColoredRectangle, this.ColoredRectanglePosition, null, Color.White, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Utility.StandardButtonDepth);
                spriteBatch.DrawString(Game1.AllTextures.MenuText, this.Stamina.ToString(), this.PositionToDraw, Color.Black, 0f, Game1.Utility.Origin, 3f, SpriteEffects.None, Utility.StandardButtonDepth);
            }
        }
    }
}
