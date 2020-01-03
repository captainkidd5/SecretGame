﻿using Microsoft.Xna.Framework;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;

namespace SecretProject.Class.ItemStuff
{
    public class ItemBank
    {
        public Dictionary<int, GridItem> ExteriorGridItems { get; set; }
        public Dictionary<int, GridItem> InteriorGridItems { get; set; }

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

        public Item GenerateNewItem(int id, Vector2? location, bool isWorldItem = false)
        {
            Item newItem = new Item(Game1.AllItems.GetItemFromID(id));

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