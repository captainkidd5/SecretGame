using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.TileStuff;
using System.Collections.Generic;

namespace SecretProject.Class.ItemStuff
{
    public enum StorableItemType
    {
        Chest = 1,
        Cauldron = 2,
        Furnace = 3
    }
    public interface IStorableItemBuilding
    {
        string ID { get; set; }
        StorableItemType StorableItemType { get; set; }
        Inventory Inventory { get; set; }
        GraphicsDevice GraphicsDevice { get; set; }
        List<ItemStorageSlot> ItemSlots { get; set; }
        bool IsInventoryHovered { get; set; }
        bool IsUpdating { get; set; }
        bool FreezesGame { get; set; }
        int Size { get; set; }
        Vector2 Location { get; set; }
        Rectangle BackDropSourceRectangle { get; set; }
        Vector2 BackDropPosition { get; set; }
        float BackDropScale { get; set; }

        ItemStorageSlot CurrentHoveredSlot { get; set; }

        Tile Tile { get; set; }
        bool IsItemAllowedToBeStored(Item item);
        void Update(GameTime gameTime);
        void Activate(Tile tile);
        void Deactivate();
        void Draw(SpriteBatch spriteBatch);
    }
}
