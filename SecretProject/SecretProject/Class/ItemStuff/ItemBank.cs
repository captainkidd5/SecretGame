using Microsoft.Xna.Framework;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using XMLData.ItemStuff;

namespace SecretProject.Class.ItemStuff
{
    public class ItemBank
    {
        public Dictionary<int, GridItem> ExteriorGridItems { get; set; }
        public Dictionary<int, GridItem> InteriorGridItems { get; set; }
        public Dictionary<int,ItemData> ItemDictionary { get; set; }

        public ItemBank()
        {


        }

        public void LoadExteriorContent(ITileManager exteriorTileManager)
        {
            this.ExteriorGridItems = new Dictionary<int, GridItem>();
            for (int i = 0; i < Game1.AllItems.AllItems.Count; i++)
            {
                if (Game1.AllItems.AllItems[i].PlaceID != 0)
                {
                    if (Game1.AllItems.AllItems[i].PlaceID > 0)
                    {
                        this.ExteriorGridItems.Add(Game1.AllItems.AllItems[i].ID, new GridItem(exteriorTileManager, Game1.AllItems.AllItems[i].PlaceID));
                    }

                }
            }
        }

        public void LoadInteriorContent(ITileManager interiorTileManager)
        {
            this.InteriorGridItems = new Dictionary<int, GridItem>();
            for (int i = 0; i < Game1.AllItems.AllItems.Count; i++)
            {
                if (Game1.AllItems.AllItems[i].PlaceID != 0)
                {
                    if (Game1.AllItems.AllItems[i].PlaceID < 0)
                    {
                        this.InteriorGridItems.Add(Game1.AllItems.AllItems[i].ID, new GridItem(interiorTileManager, Math.Abs(Game1.AllItems.AllItems[i].PlaceID)));
                    }

                }
            }
        }

        public ItemData GetItem(int itemID)
        {
            return ItemDictionary[itemID];
        }

        public Item GenerateNewItem(int id, Vector2? location, bool isWorldItem = false, List<Item> allItems = null)
        {
            Item newItem = new Item(ItemDictionary[id], allItems);

            if (!(location == null))
            {
                newItem.WorldPosition = (Vector2)location;

            }
            if (isWorldItem)
            {
                newItem.IsWorldItem = true;
                newItem.IsDropped = true;
            }
            else
            {
                newItem.IsWorldItem = false;
            }

            newItem.Load();
            return newItem;
        }
    }

}