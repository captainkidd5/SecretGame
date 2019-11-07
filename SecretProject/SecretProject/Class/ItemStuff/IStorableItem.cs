using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff
{
    public enum StorableItemType
    {
        Chest = 1,
        Cauldron = 2
    }
    public interface IStorableItem
    {
        StorableItemType StorableItemType { get; set; }
        Inventory Inventory { get; set; }
        bool IsInventoryHovered { get; set; }
        bool IsUpdating { get; set; }
         int Size { get; set; }
        Vector2 Location { get; set; }

        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
