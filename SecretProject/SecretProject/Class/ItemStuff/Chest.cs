using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        List<Button> AllButtons;
        public Chest(float iD,int size, Vector2 location, GraphicsDevice graphics)
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
                AllButtons.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 64, 64), graphics, new Vector2(500, 635)) { ItemCounter = 0, Index = 1 };)
            }
        }
        public void Update(GameTime gameTime)
        {

        }
    }
}
