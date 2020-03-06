using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
    public class CraftingWindow : IExclusiveInterfaceComponent
    {
        public GraphicsDevice Graphics { get; set; }
        public CraftingGuide CraftingGuide { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle BackSourceRectangle { get; set; }
        public float Scale { get; set; }

        public RedEsc RedEsc { get; set; }

        public List<CraftingCategoryTab> CategoryTabs { get; set; }
        public CraftingCategory CurrentOpenTab { get; set; }


        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FreezesGame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CraftingWindow(ContentManager content, GraphicsDevice graphics)
        {
            this.Graphics = graphics;
            this.Scale = 3f;
            this.BackSourceRectangle = new Rectangle(304, 365, 112, 164);
            this.Position = Game1.Utility.CenterRectangleOnScreen(this.BackSourceRectangle, this.Scale);
            this.RedEsc = new RedEsc(this.Position, graphics);
            this.CraftingGuide = this.CraftingGuide = content.Load<CraftingGuide>("Item/Crafting/CraftingGuide");

            this.CategoryTabs = new List<CraftingCategoryTab>()
            {
                new CraftingCategoryTab(this, this.Position),

            };
           // for(int i =0; i < CraftingGuide.)
        }

        public void Update(GameTime gameTime)
        {
            this.RedEsc.Update(Game1.MouseManager);
            if(RedEsc.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.RedEsc.Draw(spriteBatch);
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackSourceRectangle,
                Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Utility.StandardButtonDepth);
        }
    }
}
