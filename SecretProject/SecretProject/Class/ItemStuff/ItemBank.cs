using Microsoft.Xna.Framework;
using SecretProject.Class.TileStuff;
using SecretProject.Class.TileStuff.SpawnStuff;
using System;
using System.Collections.Generic;
using XMLData.ItemStuff;

namespace SecretProject.Class.ItemStuff
{
    public class ItemBank
    {
        public Dictionary<int, GridItem> ExteriorGridItems { get; set; }
        public Dictionary<int, GridItem> InteriorGridItems { get; set; }
        private Dictionary<int,ItemData> ItemDictionary { get; set; }

        private Dictionary<int,Rectangle> SourceRectangles { get; set; }

        public ItemBank()
        {


        }

        public Rectangle GetSourceRectangle(int id)
        {
            return SourceRectangles[id];
        }
        
        public void Load()
        {
            ItemDictionary = new Dictionary<int, ItemData>();
            SourceRectangles = new Dictionary<int, Rectangle>();
            for (int i = 0; i < Game1.AllItems.AllItems.Count; i++)
            {
                int id = Game1.AllItems.AllItems[i].ID;
                ItemDictionary.Add(id, Game1.AllItems.AllItems[i]);
                this.SourceRectangles.Add(id, Game1.AllTextures.GetItemTexture(id, 40));
            }
        }

        public void LoadExteriorContent(TileManager exteriorTileManager)
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

        public void LoadInteriorContent(TileManager interiorTileManager)
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

        public ItemData GetData(int itemID)
        {
            if(ItemDictionary.ContainsKey(itemID))
            {
                return ItemDictionary[itemID];
            }
            return null;

            
        }

        public void AlterDurability(ItemData item, int amountToSubtract)
        {
            item.Durability -= amountToSubtract;
            if (item.Durability <= 0)
            {
                Game1.Player.Inventory.RemoveItem(item.ID);
                Game1.SoundManager.ToolBreak.Play();
            }
            
        }

        public ItemData GetItemDataCopy(int itemID)
        {
            if (ItemDictionary.ContainsKey(itemID))
            {
                ItemData referenceData = ItemDictionary[itemID];
                ItemData newData = new ItemData();
                newData.Name = referenceData.Name;
                newData.ID = referenceData.ID;
                newData.Durability = referenceData.Durability;
                return newData;

            }
            return null;
        }

        public GenerationType GetPlantableTileType(int id)
        {
            string generationType = GetData(id).GrowsOn;
            return (GenerationType)Enum.Parse(typeof (GenerationType), generationType);
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