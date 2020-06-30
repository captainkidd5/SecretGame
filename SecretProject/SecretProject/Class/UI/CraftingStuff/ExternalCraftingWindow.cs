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

        public Button CraftButton { get; set; }
        public Vector2 CraftButtonTextPosition { get; set; }

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

            this.CraftButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(441,496, 62,16), craftingWindow.Graphics,
               new Vector2(this.Position.X , this.Position.Y + this.BackSourceRectangle.Height * craftingWindow.Scale),
               Controls.CursorType.Normal, craftingWindow.Scale + 1f, this.Item);
            this.CraftButtonTextPosition = Game1.Utility.CenterTextOnRectangle(Game1.AllTextures.MenuText,
                new Vector2(CraftButton.Position.X + CraftButton.BackGroundSourceRectangle.Width/2 * CraftButton.HitBoxScale, CraftButton.Position.Y + CraftButton.BackGroundSourceRectangle.Height / 2 * CraftButton.HitBoxScale), "Craft", CraftingWindow.Scale);
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


                    Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(Game1.MouseManager.UIPosition.X + 64, Game1.MouseManager.Position.Y + 64);

                }

                if(this.CurrentRecipe.CanCraft)
                {
                    this.CraftButton.Update(Game1.MouseManager);
                    if(this.CraftButton.isClicked)
                    {
                        CraftItem(this.CurrentRecipe);
                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.CraftMetal, true, .25f);
                    }
                }

            }
        }

        public void CraftItem(RecipeContainer container)
        {
            for(int i =0; i < container.ItemRecipe.AllItemsRequired.Count; i++)
            {
                for(int j =0; j < container.ItemRecipe.AllItemsRequired[i].Count; j++)
                {
                    Game1.Player.Inventory.RemoveItem(container.ItemRecipe.AllItemsRequired[i].ItemID);
                }
                
            }
            Game1.Player.Inventory.TryAddItem(Game1.ItemVault.GenerateNewItem(container.ItemRecipe.ItemToCraftID,null));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsActive)
            {


                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackSourceRectangle,
                    Color.White, 0f, Game1.Utility.Origin, CraftingWindow.Scale, SpriteEffects.None,Utility.StandardButtonDepth);
                this.RedEsc.Draw(spriteBatch);

                this.CurrentRecipe.DrawToolTips(spriteBatch);

                this.ItemToCraftButton.Draw(spriteBatch);

                this.CraftButton.Draw(spriteBatch, Game1.AllTextures.MenuText, "Craft", this.CraftButtonTextPosition, Color.White,Utility.StandardButtonDepth + .01f,Utility.StandardTextDepth, CraftingWindow.Scale);

            }
        }
    }
}
