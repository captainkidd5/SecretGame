using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public class ScrollTree : IExclusiveInterfaceComponent
    {
        public bool IsActive { get; set; }
        public bool FreezesGame { get; set; }
        public List<ScrollSlot> ScrollSlots { get; set; }

        public ScrollTree(GraphicsDevice graphics)
        {
            ScrollSlots = new List<ScrollSlot>
            {
                new ScrollSlot(graphics, 1, new Vector2(500,500)),
            };
            this.IsActive = false;
            this.FreezesGame = true;

        }

        public void Update(GameTime gameTime, int wisdom)
        {
            foreach(ScrollSlot scrollSlot in ScrollSlots)
            {
                scrollSlot.Update(gameTime, wisdom);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Vector2(400, 400), new Rectangle(1104, 1120, 288, 366), Color.Wheat, 0f, Game1.Utility.Origin, 1f, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            foreach (ScrollSlot scrollSlot in ScrollSlots)
            {
                scrollSlot.Draw(spriteBatch);
            }
        }
    }

    public class ScrollSlot
    {
        public int WisdomToUnlock { get; set; }
        public bool isLocked { get; set; }
        public Item item { get; set; }
        public Button Icon { get; set; }
        public Vector2 Position { get; set; }

        public ScrollSlot(GraphicsDevice graphics,int itemID, Vector2 position)
        {
            this.Icon = new Button(Game1.AllTextures.ItemSpriteSheet, Game1.ItemVault.GenerateNewItem(itemID, position).SourceTextureRectangle, graphics, position, CursorType.Normal);
            this.isLocked = true;


        }

        public void Update(GameTime gameTime, int wisdom)
        {
            if(this.Icon.isClicked)
            {
                if(wisdom > this.WisdomToUnlock)
                {
                    this.isLocked = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(this.isLocked)
            {
                this.Icon.Draw(spriteBatch, Icon.Position, Icon.Position, new Rectangle(1328, 1472, 32, 32), new Rectangle(1280, 1472, 32, 32), Game1.AllTextures.MenuText,"Locked", this.Icon.Position, Color.White, scale: 2f); 
            }
            
        }

    }

}
