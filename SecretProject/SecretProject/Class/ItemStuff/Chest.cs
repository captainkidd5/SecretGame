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
            this.IsDrawn = false;
            AllButtons = new List<Button>();
            for(int i =0; i < size; i++)
            {
                AllButtons.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 64, 64), graphics, new Vector2(Location.X, Location.Y)) { ItemCounter = 0, Index = size });
            }
            this.IsRandomlyGenerated = isRandomlyGenerated;
        }
        public void Update(GameTime gameTime, MouseManager mouse)
        {
            for(int i =0; i < AllButtons.Count; i++)
            {
                AllButtons[i].Update(mouse);
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
