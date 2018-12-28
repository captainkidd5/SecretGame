using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff.Items;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.Stage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff
{
   public class Inventory

    {

        MouseManager mouse;

        List<IItem> currentInventory;


        public int ID { get; set; }
        public string Name { get; set; }
        public int ItemCount { get; set; }
        public Sprite ItemSprite { get; set; }

        GraphicsDevice graphics;
        ContentManager content;

        public Inventory(GraphicsDevice graphics, ContentManager content, MouseManager mouse)
        {
            currentInventory = new List<IItem>();
            ItemCount = 0;
            this.mouse = mouse;
            this.graphics = graphics;
            this.content = content;
        }

        public void Update(GameTime gameTime)
        {

        }


        public void AddItemToInventory(IItem item)
        {
            if(ItemCount <= 5)
            {
                if (item.IsDropped)
                {

                }
                ItemCount++;
                currentInventory.Add(item);
                
            }
            
            

        }


        
        public void RemoveItemFromInventory(IItem item, Vector2 position)
        {
            if(ItemCount > 0)
            {
                item.Drop(graphics, content, position);
                currentInventory.Remove(item);
                ItemCount--;
            }
            
            
        }



        
    }
}
