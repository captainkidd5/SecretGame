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
        private Action positiveAction;
        private Action negativeAction;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="positiveAction">action which will execute when player clicks yes. Cannot be null.</param>
        /// <param name="negativeAction">action which will execute when player clicks no. Leave null to just close the alert with no further action.</param>
        /// <param name="graphics"></param>
        /// <param name="size"></param>
        /// <param name="position"></param>
        /// <param name="text"></param>
        public ConfirmationAlert(Action positiveAction, Action negativeAction, GraphicsDevice graphics, AlertSize size, Vector2 position, string text) : base (graphics, size, position, text)
        {
            this.positiveAction = positiveAction;
            this.negativeAction = negativeAction;
            this.Yes = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(736, 32, 32, 32), graphics, new Vector2(position.X + 120, position.Y + 40), Controls.CursorType.Normal, 3f);
            this.No = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(736, 32, 32, 32), graphics, new Vector2(position.X, position.Y + 40), Controls.CursorType.Normal, 3f);
        }

        public override void Update(GameTime gameTime, List<Alert> alerts)
        {
            redEsc.Update(Game1.myMouseManager);
            if (redEsc.isClicked)
            {
                if (negativeAction != null)
                {
                    negativeAction.Invoke();
                }
                alerts.Remove(this);
                return;
            }
            Yes.Update(Game1.myMouseManager);
            No.Update(Game1.myMouseManager);

            if(Yes.isClicked)
            {
                alerts.Remove(this);
                positiveAction.Invoke();
            }
            else if(No.isClicked)
            {
                Game1.freeze = false;
                alerts.Remove(this);
                if (negativeAction != null)
                {
                    negativeAction.Invoke();
                }
                
                
                return;
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Yes.Draw(spriteBatch, Game1.AllTextures.MenuText, "Yes", Yes.Position, Color.White, Utility.StandardButtonDepth + .03f, Game1.Utility.StandardTextDepth + .05f, 2f);
            No.Draw(spriteBatch, Game1.AllTextures.MenuText, "No", No.Position, Color.White, Utility.StandardButtonDepth + .03f, Game1.Utility.StandardTextDepth + .05f, 2f);
        }
    
    }
}
