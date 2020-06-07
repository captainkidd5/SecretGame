using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MenuStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.ButtonStuff
{
    public class SunsetMushroom
    {
        public GraphicsDevice Graphics { get; private set; }
        public Vector2 Position { get; private set; }
        public Button Button { get; private set; }
        public Rectangle SourceRectangle { get; private set; }
        public Vector2 InfoBoxPosition { get; private set; }

        public SunsetMushroom(GraphicsDevice graphics, Vector2 position)
        {
            this.Graphics = graphics;
            this.Position = position;

            this.SourceRectangle = new Rectangle(112, 320, 32, 32);
            this.Button = new Button(Game1.AllTextures.UserInterfaceTileSet, this.SourceRectangle, graphics, position, Controls.CursorType.Normal, 2f);
            this.InfoBoxPosition = new Vector2(this.Position.X - 64, this.Position.Y - 128);
        }
        private void Teleport()
        {
            Game1.Player.position = new Vector2(1076, 1120);

        }

        public void Update(GameTime gameTime)
        {
            this.Button.Update(Game1.MouseManager);
            if(this.Button.IsHovered)
            {
                InfoPopUp infoBox = new InfoPopUp("Press to return to the center of town", InfoBoxPosition);


                Game1.Player.UserInterface.InfoBox = infoBox;


                Game1.Player.UserInterface.InfoBox.IsActive = true;
            }
           
            if(this.Button.isClicked)
            {
                
                //if(Game1.GetCurrentStage() == Game1.OverWorld)
                //{
                //    Game1.Player.UserInterface.AddAlert(AlertType.Confirmation, Game1.Utility.centerScreen, "Telport to Town?", Teleport);
                //    //Teleport();
                //}
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            this.Button.Draw(spriteBatch);
        }
    }
}
