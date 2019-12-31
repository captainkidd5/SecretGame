﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.TileStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.ItemStuff
{
    public class Cauldron : IStorableItemBuilding
    {
        public StorableItemType StorableItemType { get; set; }
        public string ID { get; set; }
        public int Size { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public bool IsActive { get; set; }
        public bool FreezesGame { get; set; }
        public Rectangle BackDropSourceRectangle { get; set; }
        public Vector2 BackDropPosition { get; set; }
        public float BackDropScale { get; set; }
        public bool IsInventoryHovered { get; set; }
        public bool IsUpdating { get; set; }

        public Inventory Inventory { get; set; }
        public Vector2 Location { get; set; }
        public List<ItemStorageSlot> ItemSlots { get; set; }
        public ItemStorageSlot CurrentHoveredSlot { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private Button redEsc;
        public Tile Tile { get; set; }


        public Cauldron(string iD, int size, Vector2 location, GraphicsDevice graphics)
        {
            this.StorableItemType = StorableItemType.Cauldron;
            this.ID = iD;
            this.Size = size;
            this.Inventory = new Inventory(Size);
            this.Location = location;
            this.GraphicsDevice = graphics;
            this.BackDropSourceRectangle = new Rectangle(320, 0, 96, 80);
            this.BackDropPosition = new Vector2(Game1.ScreenWidth / 6, Game1.ScreenHeight / 6);
            this.BackDropScale = 3f;

            this.redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), GraphicsDevice,
                new Vector2(BackDropPosition.X + BackDropSourceRectangle.Width * BackDropScale - 50, BackDropPosition.Y), CursorType.Normal);

            ItemSlots = new List<ItemStorageSlot>();
            for (int i = 0; i < 3; i++)
            {
                ItemSlots.Add(new ItemStorageSlot(graphics,this.Inventory, i, new Vector2(this.BackDropPosition.X + i * 32 * BackDropScale, this.BackDropPosition.Y * BackDropScale), new Rectangle(208, 80, 32, 32), BackDropScale, true));
                
            }
        }
        public void Activate(Tile tile)
        {
            IsUpdating = true;
            Tile = tile;
            TileUtility.GetTileRectangleFromProperty(Tile, false, null, 1939);

        }
        public void Deactivate()
        {
            IsUpdating = false;
           // Tile.SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(2139,100);
            TileUtility.GetTileRectangleFromProperty(Tile,false, null, 2139);

        }
        public void Update(GameTime gameTime)
        {
            redEsc.Update(Game1.myMouseManager);


            if (redEsc.isClicked)
            {
                Deactivate();

            }

            if (!Game1.Player.ClickRangeRectangle.Intersects(new Rectangle((int)this.Location.X, (int)this.Location.Y, 16, 16)))
            {
                Deactivate();
            }


            for(int i =0; i < ItemSlots.Count; i++)
            {
                ItemSlots[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.BackDropPosition, this.BackDropSourceRectangle,
                Color.White, 0f, Game1.Utility.Origin, BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth);
            redEsc.Draw(spriteBatch);

            for (int i = 0; i < ItemSlots.Count; i++)
            {
                ItemSlots[i].Draw(spriteBatch);
            }
        }
        
    }
}
