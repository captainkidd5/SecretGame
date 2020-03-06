using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.UI.ButtonStuff;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ExternalCraftingWindow(CraftingWindow craftingMenu, Vector2 position)
        {
            this.CraftingWindow = craftingMenu;
            this.Position = position;
            this.BackSourceRectangle = new Rectangle(432, 400, 80, 96);
            this.RedEsc = new RedEsc(Game1.Utility.CenterOnTopRightCorner(this.BackSourceRectangle, RedEsc.RedEscRectangle, this.Position, craftingMenu.Scale), craftingMenu.Graphics);
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
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsActive)
            {


                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackSourceRectangle,
                    Color.White, 0f, Game1.Utility.Origin, CraftingWindow.Scale, SpriteEffects.None, Utility.StandardButtonDepth);
                this.RedEsc.Draw(spriteBatch);
            }
        }
    }
}
