using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.ParticileStuff;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;
using System.Collections.Generic;

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
        ItemStorageSlot CookedItemSlot { get; set; }
        public ItemStorageSlot CurrentHoveredSlot { get; set; }

        private Button redEsc;
        public Tile Tile { get; set; }

        public SimpleTimer CookTimer { get; set; }

        public Button CookButton { get; set; }
        public bool IsCooking { get; set; }


        public ParticleEngine SmokeParticleEngine { get; set; }
        public ParticleEngine FireParticleEngine { get; set; }


        public Cauldron(string iD, int size, Vector2 location, GraphicsDevice graphics)
        {
            this.StorableItemType = StorableItemType.Cauldron;
            this.ID = iD;
            this.Size = size;
            this.Inventory = new Inventory(this.Size);
            this.Location = location;
            this.GraphicsDevice = graphics;
            this.BackDropSourceRectangle = new Rectangle(512, 368, 96, 96);
            this.BackDropPosition = new Vector2(Game1.ScreenWidth / 4, Game1.ScreenHeight / 4);
            this.BackDropScale = 3f;

            redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), this.GraphicsDevice,
                new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width * this.BackDropScale - 50, this.BackDropPosition.Y), CursorType.Normal);

            this.ItemSlots = new List<ItemStorageSlot>();
            for (int i = 0; i < 3; i++)
            {
                this.ItemSlots.Add(new ItemStorageSlot(graphics, this.Inventory, i, new Vector2(this.BackDropPosition.X + i * 32 * this.BackDropScale, this.BackDropPosition.Y + this.BackDropSourceRectangle.Height * this.BackDropScale), new Rectangle(208, 80, 32, 32), this.BackDropScale, true));

            }

            this.CookedItemSlot = new ItemStorageSlot(graphics, new Inventory(1), 0, new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width / 2 * this.BackDropScale - 32 * this.BackDropScale, this.BackDropPosition.Y - 64), new Rectangle(208, 80, 32, 32), this.BackDropScale, true);

            this.CookButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(441, 496, 62, 22), graphics,
                new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width / 4 * this.BackDropScale, this.BackDropPosition.Y + this.BackDropSourceRectangle.Height * this.BackDropScale / 2), CursorType.Normal, 3f);

            this.CookTimer = new SimpleTimer(4f);
            this.SmokeParticleEngine = new ParticleEngine(new List<Texture2D>() { Game1.AllTextures.SmokeParticle }, new Vector2(this.Location.X + 8, this.Location.Y));
            this.FireParticleEngine = new ParticleEngine(new List<Texture2D>() { Game1.AllTextures.Fire }, new Vector2(this.Location.X + 10, this.Location.Y + 10));
        }
        public void Activate(Tile tile)
        {
            if (!this.IsUpdating)
            {
                this.IsUpdating = true;
                this.Tile = tile;
                TileUtility.GetTileRectangleFromProperty(this.Tile, false, null, 1939);
                Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.PotLidOpen, true);
            }


        }
        public void Deactivate()
        {
            if (!this.IsCooking)
            {
                this.IsUpdating = false;
                // Tile.SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(2139,100);
                TileUtility.GetTileRectangleFromProperty(this.Tile, false, null, 2139);
                Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.PotLidClose, true);
                this.SmokeParticleEngine.ClearParticles();
                this.FireParticleEngine.ClearParticles();
            }
        }

        public bool IsItemAllowedToBeStored(Item item)
        {
            if (Game1.ItemVault.GetItem(item.ID).Food == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsEverySlotFilled()
        {
            for (int i = 0; i < this.ItemSlots.Count; i++)
            {
                if (this.ItemSlots[i].Inventory.currentInventory[0].ItemCount <= 0)
                {
                    return false;
                }

            }
            return true;
        }

        public Item DetermineMeal()
        {
            byte meatValue = 0;
            byte vegetableValue = 0;
            byte fruitValue = 0;
            for (int i = 0; i < this.ItemSlots.Count; i++)
            {
                meatValue += Game1.ItemVault.GetItem(this.ItemSlots[i].Inventory.currentInventory[0].Item.ID).MeatValue;
                vegetableValue += Game1.ItemVault.GetItem(this.ItemSlots[i].Inventory.currentInventory[0].Item.ID).VegetableValue;
                fruitValue += Game1.ItemVault.GetItem(this.ItemSlots[i].Inventory.currentInventory[0].Item.ID).FruitValue;
            }
            Item cookedItem = Game1.ItemVault.GenerateNewItem(Game1.AllCookingRecipes.AllRecipes.Find(x => ((x.MeatValueMax > meatValue) && (x.MeatValueMin <= meatValue) &&
            (x.VegetableValueMax >= vegetableValue) && (x.VegetableValueMin <= vegetableValue) &&
            (x.FruitValueMax >= fruitValue) && (x.FruitValueMin <= fruitValue))).ItemID, null);

            if (cookedItem != null)
            {
                for (int i = 0; i < this.ItemSlots.Count; i++)
                {
                    this.Inventory.currentInventory[i].RemoveItemFromSlot();
                }
            }
            return cookedItem;
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
            this.CookButton.Update(Game1.myMouseManager);
            this.CookedItemSlot.Update(gameTime);

            if (this.CookButton.isClicked)
            {
                if (IsEverySlotFilled())
                {
                    this.IsCooking = true;
                    Game1.GetCurrentStage().ParticleEngines.Add(this.SmokeParticleEngine);
                    Game1.GetCurrentStage().ParticleEngines.Add(this.FireParticleEngine);
                }
            }
            if (this.IsCooking)
            {

                this.SmokeParticleEngine.UpdateSmoke(gameTime);
                this.FireParticleEngine.UpdateFire(gameTime);
                if (this.CookTimer.Run(gameTime))
                {
                    this.CookedItemSlot.Inventory.TryAddItem(DetermineMeal());
                    this.IsCooking = false;
                    Game1.GetCurrentStage().ParticleEngines.Remove(this.SmokeParticleEngine);
                    Game1.GetCurrentStage().ParticleEngines.Remove(this.FireParticleEngine);
                    this.SmokeParticleEngine.ClearParticles();
                    this.FireParticleEngine.ClearParticles();
                }
            }
            if (this.CookedItemSlot.Button.isClicked)
            {
                if (this.CookedItemSlot.Inventory.currentInventory[0].ItemCount > 0)
                {


                    //if (Game1.Player.Inventory.TryAddItem(this.CookedItemSlot.Inventory.currentInventory[0].ItemCount))
                    //{
                    //    this.CookedItemSlot.Inventory.currentInventory[0].RemoveItemFromSlot();
                    //}
                }
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.BackDropPosition, this.BackDropSourceRectangle,
                Color.White, 0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None, Utility.StandardButtonDepth - .01f);
            redEsc.Draw(spriteBatch);
            this.CookButton.Draw(spriteBatch, Game1.AllTextures.MenuText, "Cook", new Vector2(this.CookButton.Position.X + this.CookButton.BackGroundSourceRectangle.Width / 2, this.CookButton.Position.Y), Color.White, Utility.StandardButtonDepth + .02f, Utility.StandardButtonDepth + .03f, 2f);
            for (int i = 0; i < this.ItemSlots.Count; i++)
            {
                this.ItemSlots[i].Draw(spriteBatch);
            }
            this.CookedItemSlot.Draw(spriteBatch);

            if (this.IsCooking)
            {
                spriteBatch.DrawString(Game1.AllTextures.MenuText, this.CookTimer.Time.ToString(), this.CookButton.Position, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, Utility.StandardButtonDepth + .03f);
            }
        }

    }
}
