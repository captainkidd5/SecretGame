﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;
using System.Collections.Generic;

namespace SecretProject.Class.ItemStuff
{
    public class Furnace : IStorableItemBuilding
    {
        public string ID { get; set; }
        public StorableItemType StorableItemType { get; set; }
        public Inventory Inventory { get; set; }
        public List<ItemStorageSlot> ItemSlots { get; set; }
        public bool IsInventoryHovered { get; set; }
        public bool IsUpdating { get; set; }
        public bool FreezesGame { get; set; }
        public int Size { get; set; }
        public Vector2 Location { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public Rectangle BackDropSourceRectangle { get; set; }
        public Vector2 BackDropPosition { get; set; }
        public float BackDropScale { get; set; }

        public SimpleTimer SimpleTimer { get; set; }
        public ItemStorageSlot SmeltSlot { get; set; }

        public ItemStorageSlot CurrentHoveredSlot { get; set; }

        public Vector2 TimerStringLocation { get; set; }
        public Tile Tile { get; set; }

        private Button redEsc;

        public Vector2 OrangeFlamePosition { get; set; }
        public Vector2 OrangeFlameCurrentPosition { get; set; }
        public Rectangle OrangeFlameSourceRectangle { get; set; }


        public SoundEffectInstance FireSound { get; set; }
        public Furnace(string iD, int size, Vector2 location, GraphicsDevice graphics)
        {
            this.StorableItemType = StorableItemType.Cauldron;
            this.ID = iD;
            this.Size = size;
            this.Inventory = new Inventory(this.Size);
            this.Location = location;
            this.GraphicsDevice = graphics;
            this.BackDropSourceRectangle = new Rectangle(224, 128, 48, 96);
            this.BackDropPosition = new Vector2(Game1.ScreenWidth / 6, Game1.ScreenHeight / 6);
            this.BackDropScale = 3f;

            //this.redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), GraphicsDevice,
            //    new Vector2(BackDropPosition.X + BackDropSourceRectangle.Width * BackDropScale - 50, BackDropPosition.Y), CursorType.Normal);

            this.ItemSlots = new List<ItemStorageSlot>();
            for (int i = 0; i < 1; i++)
            {
                this.ItemSlots.Add(new ItemStorageSlot(graphics, this.Inventory, i, new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width / 2, this.BackDropPosition.Y + this.BackDropSourceRectangle.Height + 32 * this.BackDropScale), new Rectangle(208, 80, 32, 32), this.BackDropScale, true));

            }
            this.SimpleTimer = new SimpleTimer(5f);
            this.SmeltSlot = new ItemStorageSlot(graphics, new Inventory(1), 0, new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width / 2, this.BackDropPosition.Y), new Rectangle(208, 80, 32, 32), this.BackDropScale, true);
            this.TimerStringLocation = new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width, this.BackDropPosition.Y + this.BackDropSourceRectangle.Height);

            redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), graphics,
               new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width * this.BackDropScale, this.BackDropPosition.Y), CursorType.Normal);

            this.OrangeFlamePosition = new Vector2(this.BackDropPosition.X + 32, this.BackDropPosition.Y + this.BackDropSourceRectangle.Height * BackDropScale - 40 * BackDropScale);
            this.OrangeFlameCurrentPosition = this.OrangeFlamePosition;
            this.OrangeFlameSourceRectangle = new Rectangle(272, 160, 32, 32);
            this.FireSound = Game1.SoundManager.FurnaceFire.CreateInstance();
        }
        public void Activate(Tile tile)
        {
            this.IsUpdating = true;
            this.Tile = tile;
            //Tile.SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(1752, 100);

        }



        public void Deactivate()
        {

        }

        public bool IsItemAllowedToBeStored(Item item)
        {
            return true;
        }
        public void Update(GameTime gameTime)
        {
            this.IsInventoryHovered = false;
            this.OrangeFlameCurrentPosition = new Vector2(this.OrangeFlamePosition.X, this.OrangeFlamePosition.Y - 10 * this.SimpleTimer.Time);
            if (!Game1.Player.ClickRangeRectangle.Intersects(new Rectangle((int)this.Location.X, (int)this.Location.Y, 16, 16)))
            {
                this.IsUpdating = false;
            }


            for (int i = 0; i < this.ItemSlots.Count; i++)
            {
                this.ItemSlots[i].Update(gameTime);
                if (this.ItemSlots[i].Button.IsHovered)
                {

                    this.IsInventoryHovered = true;
                    this.CurrentHoveredSlot = this.ItemSlots[i];

                }
            }

            this.SmeltSlot.Update(gameTime);
            if (this.SmeltSlot.Button.IsHovered)
            {
                this.CurrentHoveredSlot = this.SmeltSlot;
                this.IsInventoryHovered = true;

            }
            
            if (this.SmeltSlot.Inventory.currentInventory[0].ItemCount > 0 && this.ItemSlots[0].Inventory.currentInventory[0].ItemCount > 0)
            {
                if (this.SmeltSlot.Inventory.currentInventory[0].GetItemData().SmeltedItem != 0 && this.ItemSlots[0].Inventory.currentInventory[0].GetItemData().FuelValue > 0)
                {
                    if(FireSound.State == SoundState.Stopped)
                    {
                        FireSound.Play();
                    }
                    if (this.SimpleTimer.Run(gameTime))
                    {
                        int newID = this.SmeltSlot.Inventory.currentInventory[0].GetItemData().SmeltedItem;
                        this.SmeltSlot.Inventory.currentInventory[0].RemoveItemFromSlot();
                        this.SmeltSlot.Inventory.currentInventory[0].AddItemToSlot(Game1.ItemVault.GenerateNewItem(newID, null));
                        this.ItemSlots[0].Inventory.currentInventory[0].RemoveItemFromSlot();
                        Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.FurnaceDone, true, .25f);
                    }
                }
            }
            else

            {
                this.SimpleTimer.Time = 0;
                this.FireSound.Stop();
            }

            redEsc.Update(Game1.MouseManager);


            if (redEsc.isClicked)
            {
                this.IsUpdating = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.BackDropPosition, this.BackDropSourceRectangle,
                Color.White, 0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Utility.StandardButtonDepth - .02f);
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.OrangeFlameCurrentPosition, this.OrangeFlameSourceRectangle,
                Color.White, 0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Utility.StandardButtonDepth - .03f);

            for (int i = 0; i < this.ItemSlots.Count; i++)
            {
                this.ItemSlots[i].Draw(spriteBatch);
            }

            this.SmeltSlot.Draw(spriteBatch);

           // spriteBatch.DrawString(Game1.AllTextures.MenuText, this.SimpleTimer.Time.ToString(), this.TimerStringLocation, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, Utility.StandardButtonDepth + .01f);

            redEsc.Draw(spriteBatch);
        }

    }
}