using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.ItemStuff;

namespace SecretProject.Class.UI.CraftingStuff
{
    public class ExternalToolTip
    {
        public CraftingWindow CraftingWindow { get; set; }
        public Item Item { get; set; }
        public Vector2 Position { get; set; }
        public int CurrentCount { get; set; }
        public int CountRequired { get; set; }

        public Button Button { get; set; }

        public ExternalToolTip(CraftingWindow craftingWindow, int itemID, Vector2 position)
        {
            this.CraftingWindow = craftingWindow;
            this.Item = Game1.ItemVault.GenerateNewItem(itemID, null);
            this.Position = position;
            this.Button = new Button(Game1.AllTextures.ItemSpriteSheet, this.Item.SourceTextureRectangle, craftingWindow.Graphics,
                position, Controls.CursorType.Normal, craftingWindow.Scale, this.Item );
        }

        public void Update(GameTime gameTime)
        {
            Button.Update(Game1.MouseManager);
            if(Button.IsHovered)
            {
                Game1.Player.UserInterface.InfoBox.IsActive = true;
                Game1.Player.UserInterface.InfoBox.DisplayTitle = true;
                ItemData itemData = Game1.ItemVault.GetItem(Item.ID);
                Game1.Player.UserInterface.InfoBox.FitTitleText(itemData.Name, 1f);
                Game1.Player.UserInterface.InfoBox.FitText(itemData.Description, 1f);


                Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(Game1.MouseManager.UIPosition.X + 32, Game1.MouseManager.Position.Y + 32);

            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Button.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.AllTextures.MenuText, this.CurrentCount.ToString() + "/" + this.CountRequired.ToString(),
                new Vector2(this.Button.Position.X, this.Button.Position.Y + 32), Color.White, 0f, Game1.Utility.Origin, CraftingWindow.Scale - 1f, SpriteEffects.None, Utility.StandardButtonDepth + .02f);
        }

    }
}
