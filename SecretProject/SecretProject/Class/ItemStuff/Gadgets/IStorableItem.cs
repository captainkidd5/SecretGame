using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.TileStuff;
using System.Collections.Generic;
using XMLData.ItemStuff;

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

        TileManager TileManager { get; set; }

        int Layer { get; set; }
        int X { get; set; }
        int Y { get; set; }
        bool IsItemAllowedToBeStored(ItemData item);

        bool IsAnimationOpen { get; set; }
        void Update(GameTime gameTime);
        void Activate(TileManager TileManager, int layer, int x, int y);
        void Deactivate();
        void Draw(SpriteBatch spriteBatch);

        bool DepositItem(ItemData referenceItem);
    }
}
