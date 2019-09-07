using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff
{
    public class Chest
    {
        public float ID { get; set; }
        public int Size { get; set; }
        public Vector2 Location { get; set; }
        public Inventory Inventory { get; set; }
        public bool IsUpdating { get; set; }
        public bool IsDrawn { get; set; }
        public bool IsRandomlyGenerated { get; set; }
        List<Button> AllButtons;
        public Chest(float iD,int size, Vector2 location, GraphicsDevice graphics, bool isRandomlyGenerated)
        {
            this.ID = ID;
            this.Size = size;
            this.Inventory = new Inventory(size);
            this.Location = location;
            this.IsUpdating = false;
            AllButtons = new List<Button>();
            for(int i =0; i < size; i++)
            {
                AllButtons.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 64, 64), graphics, new Vector2(Location.X, Location.Y)) { ItemCounter = 0, Index = size });
            }
            this.IsRandomlyGenerated = isRandomlyGenerated;
            if(isRandomlyGenerated)
            {
                FillWithLoot(size);
            }
        }
        public void Update(GameTime gameTime, MouseManager mouse)
        {
            for(int i =0; i < AllButtons.Count; i++)
            {
                AllButtons[i].Update(mouse);
                if(AllButtons[i].isClicked && this.Inventory.currentInventory[i].SlotItems.Count > 0)
                {
                    Game1.Player.Inventory.TryAddItem(this.Inventory.currentInventory[i].SlotItems[0]);
                    this.Inventory.currentInventory[i].SlotItems.Remove(this.Inventory.currentInventory[i].SlotItems[0]);
                }
            }
            if(!Game1.Player.ClickRangeRectangle.Intersects(new Rectangle((int)this.Location.X, (int)this.Location.Y,16,16)))
            {
                this.IsUpdating = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < AllButtons.Count; i++)
            {
                AllButtons[i].Draw(spriteBatch, AllButtons[i].ItemSourceRectangleToDraw, AllButtons[i].BackGroundSourceRectangle, Game1.AllTextures.MenuText, AllButtons[i].ItemCounter.ToString(), new Vector2(AllButtons[i].Position.X + 5, AllButtons[i].Position.Y + 5), Color.DarkRed, 2f);
            }
        }

        public void FillWithLoot(int size)
        {
            int slotsToFill = Game1.Utility.RGenerator.Next(0, size);
            for(int i =0; i < slotsToFill; i++)
            {
                int selection = Game1.Utility.RGenerator.Next(0, Game1.AllItems.AllItems.Count);
                this.Inventory.TryAddItem(new Item(Game1.AllItems.AllItems[selection]));
            }
            
        }
    }
}
