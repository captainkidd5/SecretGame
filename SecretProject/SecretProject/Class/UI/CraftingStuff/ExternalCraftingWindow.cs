using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.UI.ButtonStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLData.ItemStuff;
using XMLData.ItemStuff.CraftingStuff;

namespace SecretProject.Class.UI.CraftingStuff
{
    public class ExternalCraftingWindow
    {
        public bool IsActive { get; set; }
        public CraftingWindow CraftingWindow { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle BackSourceRectangle { get; set; }
        public RecipeContainer CurrentRecipe { get; set; }

        public RedEsc RedEsc { get; set; }


        public Item Item{ get; set; }
        public Button ItemToCraftButton { get; set; }
        public ExternalToolTip ToolTip { get; set; }

        public ExternalCraftingWindow(CraftingWindow craftingWindow,ItemRecipe itemRecipe, Vector2 position)
        {
            this.CraftingWindow = craftingWindow;
            this.Position = position;
            this.BackSourceRectangle = new Rectangle(432, 400, 80, 96);
            this.RedEsc = new RedEsc(Game1.Utility.CenterOnTopRightCorner(this.BackSourceRectangle, RedEsc.RedEscRectangle, this.Position, craftingWindow.Scale), craftingWindow.Graphics);
            this.Item = Game1.ItemVault.GenerateNewItem(1, null);
            this.ItemToCraftButton = new Button(Game1.AllTextures.ItemSpriteSheet, this.Item.SourceTextureRectangle, craftingWindow.Graphics,
                new Vector2(this.Position.X + this.BackSourceRectangle.Width * craftingWindow.Scale /2 - 32 , this.Position.Y + 32),
                Controls.CursorType.Normal, craftingWindow.Scale + 2f, this.Item);
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {


                this.RedEsc.Update(Game1.MouseManager);
                if (RedEsc.isClicked)
                {
                    this.IsActive = false;
                }

                this.CurrentRecipe.UpdateToolTips(gameTime);

                this.ItemToCraftButton.Update(Game1.MouseManager);
                if(this.ItemToCraftButton.IsHovered)
                {
                    Game1.Player.UserInterface.InfoBox.IsActive = true;
                    Game1.Player.UserInterface.InfoBox.DisplayTitle = true;
                    ItemData itemData = Game1.ItemVault.GetItem(Item.ID);
                    Game1.Player.UserInterface.InfoBox.FitTitleText(itemData.Name, 1f);
                    Game1.Player.UserInterface.InfoBox.FitText(itemData.Description, 1f);


                    Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(Game1.MouseManager.UIPosition.X + 32, Game1.MouseManager.Position.Y + 32);

                }

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsActive)
            {


                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackSourceRectangle,
                    Color.White, 0f, Game1.Utility.Origin, CraftingWindow.Scale, SpriteEffects.None, Utility.StandardButtonDepth);
                this.RedEsc.Draw(spriteBatch);

                this.CurrentRecipe.DrawToolTips(spriteBatch);

                this.ItemToCraftButton.Draw(spriteBatch);

            }
        }
    }
}
