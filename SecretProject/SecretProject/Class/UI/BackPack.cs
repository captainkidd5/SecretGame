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
            this.Position = new Vector2(Game1.PresentationParameters.BackBufferWidth / 3, Game1.PresentationParameters.BackBufferHeight / 4);
            this.BackGroundSourceRectangle = new Rectangle(208, 560, 288, 176);
            this.Scale = 2f;

            AllSlots = new List<Button>();

            for (int i = 0; i < Inventory.Capacity; i++)
            {
                AllSlots.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 64, 64), Graphics, new Vector2(Game1.PresentationParameters.BackBufferWidth * .35f + i * 65, Game1.PresentationParameters.BackBufferHeight * .9f), CursorType.Normal) { ItemCounter = 0, Index = i + 1 });
            }

        }

        public void Activate()
        {
            this.IsActive = true;
            for (int i = 0; i < AllSlots.Count; i++)
            {
                AllSlots[i].Position = new Vector2(BackGroundSourceRectangle.X * Scale + 32 * i * Scale, BackGroundSourceRectangle.Y - 364);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {
                for (int i = 0; i < AllSlots.Count; i++)
                {
                    AllSlots[i].Update(Game1.myMouseManager);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(this.IsActive)
            {
                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.Position, this.BackGroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
                for (int i = 0; i < AllSlots.Count; i++)
                {
                    AllSlots[i].Draw(spriteBatch, AllSlots[i].ItemSourceRectangleToDraw, AllSlots[i].BackGroundSourceRectangle, Game1.AllTextures.MenuText, AllSlots[i].ItemCounter.ToString(), new Vector2(BackGroundSourceRectangle.X * Scale + 32 * i, BackGroundSourceRectangle.Y - 364), Color.White, 2f, 2f, Game1.Utility.StandardButtonDepth);
                }
            }
            
        }


    }
}
