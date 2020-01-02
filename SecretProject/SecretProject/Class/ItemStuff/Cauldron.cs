using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.ParticileStuff;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;
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
        ItemStorageSlot CookedItemSlot { get; set; }
        public ItemStorageSlot CurrentHoveredSlot { get; set; }

        private Button redEsc;
        public Tile Tile { get; set; }

        public SimpleTimer CookTimer { get; set; }

        public Button CookButton { get; set; }
        public bool IsCooking { get; set; }


        public ParticleEngine ParticleEngine { get; set; }

        public Cauldron(string iD, int size, Vector2 location, GraphicsDevice graphics)
        {
            this.StorableItemType = StorableItemType.Cauldron;
            this.ID = iD;
            this.Size = size;
            this.Inventory = new Inventory(Size, 1);
            this.Location = location;
            this.GraphicsDevice = graphics;
            this.BackDropSourceRectangle = new Rectangle(512, 368, 96, 96);
            this.BackDropPosition = new Vector2(Game1.ScreenWidth / 4, Game1.ScreenHeight / 4);
            this.BackDropScale = 3f;

            this.redEsc = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(0, 0, 32, 32), GraphicsDevice,
                new Vector2(BackDropPosition.X + BackDropSourceRectangle.Width * BackDropScale - 50, BackDropPosition.Y), CursorType.Normal);

            ItemSlots = new List<ItemStorageSlot>();
            for (int i = 0; i < 3; i++)
            {
                ItemSlots.Add(new ItemStorageSlot(graphics,this.Inventory, i, new Vector2(this.BackDropPosition.X + i * 32 * BackDropScale, this.BackDropPosition.Y  + BackDropSourceRectangle.Height * BackDropScale), new Rectangle(208, 80, 32, 32), BackDropScale, true));
                
            }

            CookedItemSlot = new ItemStorageSlot(graphics, new Inventory(1), 0, new Vector2(BackDropPosition.X + BackDropSourceRectangle.Width / 2 * BackDropScale - 32 * BackDropScale, BackDropPosition.Y - 64), new Rectangle(208, 80, 32, 32), BackDropScale, true);

            this.CookButton = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(441, 496, 62, 22), graphics,
                new Vector2(BackDropPosition.X + BackDropSourceRectangle.Width /4 * BackDropScale, BackDropPosition.Y + BackDropSourceRectangle.Height * BackDropScale / 2), CursorType.Normal, 3f);

            CookTimer = new SimpleTimer(4f);
            ParticleEngine = new ParticleEngine(new List<Texture2D>() { Game1.AllTextures.SmokeParticle }, Location);
        }
        public void Activate(Tile tile)
        {
            if(!IsUpdating)
            {
                IsUpdating = true;
                Tile = tile;
                TileUtility.GetTileRectangleFromProperty(Tile, false, null, 1939);
                Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.PotLidOpen, true);
            }
            

        }
        public void Deactivate()
        {
            if (!IsCooking)
            {
                IsUpdating = false;
                // Tile.SourceRectangle = TileUtility.GetSourceRectangleWithoutTile(2139,100);
                TileUtility.GetTileRectangleFromProperty(Tile, false, null, 2139);
                Game1.SoundManager.PlaySoundEffectInstance(Game1.SoundManager.PotLidClose, true);
            }
        }

        public bool IsItemAllowedToBeStored(Item item)
        {
            if(item.Food == true)
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
            for(int i =0; i < ItemSlots.Count;i++)
            {
                if(ItemSlots[i].Inventory.currentInventory[0].SlotItems.Count <= 0)
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
            for(int i =0; i < ItemSlots.Count; i++)
            {
                meatValue += ItemSlots[i].Inventory.currentInventory[0].SlotItems[0].MeatValue;
                vegetableValue += ItemSlots[i].Inventory.currentInventory[0].SlotItems[0].VegetableValue;
                fruitValue += ItemSlots[i].Inventory.currentInventory[0].SlotItems[0].FruitValue;
            }
            Item cookedItem = Game1.ItemVault.GenerateNewItem(Game1.AllCookingRecipes.AllRecipes.Find(x => ((x.MeatValueMax > meatValue) && (x.MeatValueMin <= meatValue) &&
            (x.VegetableValueMax >= vegetableValue) && (x.VegetableValueMin <= vegetableValue) &&
            (x.FruitValueMax >= fruitValue) && (x.FruitValueMin <= fruitValue))).ItemID, null);

            if (cookedItem != null)
            {
                for (int i = 0; i < ItemSlots.Count; i++)
                {
                    Inventory.currentInventory[i].SlotItems.RemoveAt(Inventory.currentInventory[i].SlotItems.Count - 1);
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
            CookButton.Update(Game1.myMouseManager);
            CookedItemSlot.Update(gameTime);
           
            if(CookButton.isClicked)
            {
                if(IsEverySlotFilled())
                {
                    IsCooking = true;
                  
                }
            }
            if(IsCooking)
            {
                ParticleEngine.UpdateSmoke(gameTime, ParticleEngine.EmitterLocation);
                if (CookTimer.Run(gameTime))
                {
                    CookedItemSlot.Inventory.TryAddItem(DetermineMeal());
                    IsCooking = false;
                }
            }
            if (CookedItemSlot.Button.isClicked)
            {
                if (CookedItemSlot.Inventory.currentInventory[0].SlotItems.Count > 0)
                {


                    if (Game1.Player.Inventory.TryAddItem(CookedItemSlot.Inventory.currentInventory[0].SlotItems[0]))
                    {
                        CookedItemSlot.Inventory.currentInventory[0].RemoveItemFromSlot();
                    }
                }
            }
            for (int i = 0; i < ItemSlots.Count; i++)
            {
                ItemSlots[i].Update(gameTime);
                if (ItemSlots[i].Button.IsHovered)
                {

                    IsInventoryHovered = true;
                    CurrentHoveredSlot = ItemSlots[i];

                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.BackDropPosition, this.BackDropSourceRectangle,
                Color.White, 0f, Game1.Utility.Origin, BackDropScale, SpriteEffects.None, Game1.Utility.StandardButtonDepth - .01f);
            redEsc.Draw(spriteBatch);
            CookButton.Draw(spriteBatch, Game1.AllTextures.MenuText, "Cook", new Vector2(CookButton.Position.X + CookButton.BackGroundSourceRectangle.Width / 2, CookButton.Position.Y), Color.White, Game1.Utility.StandardButtonDepth + .02f, Game1.Utility.StandardButtonDepth + .03f, 2f);
            for (int i = 0; i < ItemSlots.Count; i++)
            {
                ItemSlots[i].Draw(spriteBatch);
            }
            CookedItemSlot.Draw(spriteBatch);

            if(IsCooking)
            {
                ParticleEngine.Draw(spriteBatch, true);
                spriteBatch.DrawString(Game1.AllTextures.MenuText, CookTimer.Time.ToString(), CookButton.Position, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None, Game1.Utility.StandardButtonDepth + .03f);
            }
        }
        
    }
}
