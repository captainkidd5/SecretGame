using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    public class BackPack : IExclusiveInterfaceComponent
    {
        public bool IsActive { get; set; }
        public bool FreezesGame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public GraphicsDevice Graphics { get; set; }
        public Inventory Inventory { get; set; }
        public int Capacity { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle BackGroundSourceRectangle { get; set; }
        public float Scale { get; set; }

        public List<Button> AllSlots { get; set; }

        public BackPack(GraphicsDevice graphics, Inventory inventory)
        {
            this.Graphics = graphics;
            this.Inventory = inventory;
            this.IsActive = false;
            this.Position = new Vector2(Game1.PresentationParameters.BackBufferWidth / 2, Game1.PresentationParameters.BackBufferHeight / 2);
            this.BackGroundSourceRectangle = new Rectangle(208, 560, 288, 176);
            this.Scale = 2f;

            for (int i = 0; i < Inventory.Capacity; i++)
            {
             //   AllSlots.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 64, 64), Graphics, new Vector2(Game1.PresentationParameters.BackBufferWidth * .35f + i * 65, Game1.PresentationParameters.BackBufferHeight * .9f), CursorType.Normal) { ItemCounter = 0, Index = i + 1 });
            }

        }

        public void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {
                //if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Tab)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Tab)))
                //{
                //    this.IsActive = false;
                //}
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(this.IsActive)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackGroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);

            }
            
        }
    }
}
