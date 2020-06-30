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
    /// <summary>
    /// UI Button which, when pressed, will teleport the player back to a set position in town.
    /// </summary>
    public class SunsetMushroom
    {
        private readonly Vector2 teleportPosition = new Vector2(875, 880);
        private GraphicsDevice Graphics;
        private Vector2 Position;
        private Button Button;
        private Rectangle SourceRectangle;
        private Vector2 InfoBoxPosition;

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
            Game1.SwitchStage(Game1.GetCurrentStageInt(), Stages.Town);
            Game1.Player.position = teleportPosition;

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

                //if(Game1.CurrentStage == Game1.OverWorld)
                //{
                Game1.Player.UserInterface.AddAlert(AlertType.Confirmation, Game1.Utility.centerScreen, "Telport to Town?", Teleport);
                //Teleport();
                //}
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            this.Button.Draw(spriteBatch);
        }
    }
}
