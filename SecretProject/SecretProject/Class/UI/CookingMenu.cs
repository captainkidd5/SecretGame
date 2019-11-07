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
    public class CookingMenu : IExclusiveInterfaceComponent
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public bool IsActive { get; set; }
        public bool FreezesGame { get; set; }
        public Rectangle BackDropSourceRectangle { get; set; }
        public Vector2 BackDropPosition { get; set; }
        public float BackDropScale { get; set; }

        public Inventory Inventory { get; set; }

        private Button redEsc;

        List<CookingSlot> CookingSlots;
        public CookingMenu(GraphicsDevice graphics)
        {
            this.GraphicsDevice = graphics;
            this.BackDropSourceRectangle = new Rectangle(320, 0, 96, 80);
            this.BackDropPosition = new Vector2(Game1.ScreenWidth / 6, Game1.ScreenHeight / 6);
            this.BackDropScale = 3f;

            this.redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), GraphicsDevice,
                new Vector2(BackDropPosition.X + BackDropSourceRectangle.Width * BackDropScale - 50, BackDropPosition.Y), CursorType.Normal);

            CookingSlots = new List<CookingSlot>();
            for (int i = 0; i < 3; i++)
            {
                CookingSlots.Add(new CookingSlot(graphics,this.Inventory, i, new Vector2(this.BackDropPosition.X + i * 32 * BackDropScale, this.BackDropPosition.Y * BackDropScale), BackDropScale));
                //AllButtons.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(320, 32, 32, 32), graphics, this.BackDropPosition, CursorType.Normal, BackDropScale);
            }
        }

        public void Update(GameTime gameTime)
        {
            redEsc.Update(Game1.myMouseManager);


            if (redEsc.isClicked)
            {
                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.None;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.BackDropPosition, this.BackDropSourceRectangle,
                Color.White, 0f, Game1.Utility.Origin, BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            redEsc.Draw(spriteBatch);
        }
        public class CookingSlot
        {
            public GraphicsDevice GraphicsDevice { get; set; }
            public Inventory Inventory { get; set; }
            public int Index { get; set; }
            public int Count { get; set; }
            public Vector2 Position { get; set; }
            public float Scale { get; set; }
            public Button Button { get; set; }
            

            public CookingSlot(GraphicsDevice graphics, Inventory inventory, int index, Vector2 position, float scale)
            {
                this.GraphicsDevice = graphics;
                this.Inventory = inventory;
                this.Index = index;
                this.Count = 0;
                this.Position = position;
                this.Scale = scale;
                this.Button = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(320, 32, 32, 32),this.GraphicsDevice, this.Position, CursorType.Normal, this.Scale); 
            }

            public void Update(GameTime gameTime)
            {
                if(this.Button.isClicked)
                {
                    if (this.Inventory.currentInventory[this.Index].SlotItems.Count > 0 && this.Inventory.currentInventory[this.Index].SlotItems[0] != null)
                    {
                        if (Game1.Player.Inventory.TryAddItem(Inventory.currentInventory[this.Index].SlotItems[0]))
                        {
                            this.Inventory.currentInventory[this.Index].RemoveItemFromSlot();
                        }
                    }
                }
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                Button.Draw(spriteBatch, Button.ItemSourceRectangleToDraw, Button.BackGroundSourceRectangle, Game1.AllTextures.MenuText, this.Count.ToString(),
                    Button.Position, Color.White, this.Scale, this.Scale);
            }

        }
    }
}
