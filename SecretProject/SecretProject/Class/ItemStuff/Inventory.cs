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
        string name;
        int iD;
        int itemCount;
        Sprite itemSprite;

        ContentManager content;

        MouseManager mouse;

        List<IItem> currentInventory;

        GraphicsDevice graphicsDevice;

        public Inventory(ContentManager content, GraphicsDevice graphicsDevice, MouseManager mouse)
        {
            this.content = content;
            currentInventory = new List<IItem>();
            itemCount = 0;
            this.mouse = mouse;
            this.graphicsDevice = graphicsDevice;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void AddItemToInventory(IItem item)
        {
            currentInventory.Add(item);
            

        }


        
        public void RemoveItemFromInventory(IItem item)
        {
            currentInventory.Remove(item);
        }

        public void DropItemFromInventory(IItem item)
        {
            if(currentInventory.Contains(item))
            {
                currentInventory.Remove(item);
                Iliad.allSprites.Add(new Sprite(graphicsDevice, content, mouse.WorldMousePosition, true, item));
            }

            
        }
        
    }
}
