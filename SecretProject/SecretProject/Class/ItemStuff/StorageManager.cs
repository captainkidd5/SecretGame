using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff
{
    public class StorageManager
    {
        public Inventory Inventory { get; private set; }
        public List<ItemStorageSlot> StorageSlots { get; private set; }
        public DragSlot DragSlot { get; private set; }
        public StorageManager(Inventory inventory, List<ItemStorageSlot> storageSlots)
        {
            this.Inventory = inventory;
            this.StorageSlots = storageSlots;
        }

        public void Update(GameTime gameTime)
        {
            for(int i =0; i < this.StorageSlots.Count; i++)
            {
                this.StorageSlots[i].Update(gameTime, this.DragSlot);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.StorageSlots.Count; i++)
            {
                this.StorageSlots[i].Draw(spriteBatch);
            }
        }
    }
}
