using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI.AlertStuff
{
    public class ConfirmationAlert : Alert
    {
        public Button Yes { get; set; }
        public Button No { get; set; }
        private Action actionToExecute;
        public ConfirmationAlert(Action action, GraphicsDevice graphics, AlertSize size, Vector2 position, string text) : base (graphics, size, position, text)
        {
            this.actionToExecute = action;
            this.Yes = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(736, 32, 32, 32), graphics, new Vector2(position.X + 100, position.Y + 40), Controls.CursorType.Chat, 2f);
            this.No = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(736, 32, 32, 32), graphics, new Vector2(position.X, position.Y + 40), Controls.CursorType.Chat, 2f);
        }

        public override void Update(GameTime gameTime, List<Alert> alerts)
        {
            base.Update(gameTime, alerts);
            Yes.Update(Game1.myMouseManager);
            No.Update(Game1.myMouseManager);

            if(Yes.isClicked)
            {
                actionToExecute.Invoke();
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Yes.Draw(spriteBatch, Game1.AllTextures.MenuText, "Yes", Yes.Position, Color.White, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth, 2f);
            No.Draw(spriteBatch, Game1.AllTextures.MenuText, "No", No.Position, Color.White, Utility.StandardButtonDepth, Game1.Utility.StandardTextDepth, 2f);
        }
    
    }
}
