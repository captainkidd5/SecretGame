using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SecretProject.Class.Controls;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.TileStuff;
using SecretProject.Class.UI.ButtonStuff;
using SecretProject.Class.Universal;
using System.Collections.Generic;
using XMLData.ItemStuff;

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

        public ItemStorageSlot SmeltSlot { get; set; }

        public ItemStorageSlot CurrentHoveredSlot { get; set; }

        public Vector2 TimerStringLocation { get; set; }
        public Tile Tile { get; set; }

        private RedEsc redEsc;

        public Vector2 OrangeFlamePosition { get; set; }
        public Vector2 OrangeFlameCurrentPosition { get; set; }
        public Rectangle OrangeFlameSourceRectangle { get; set; }


        public SoundEffectInstance FireSound { get; set; }


        public float TimeActivated { get; set; }
        public float CookTimeRequired { get; set; }


        public bool IsCooking { get; set; }
        public TileManager TileManager {get;set;}
        public int Layer {get;set;}
        public int X {get;set;}
        public int Y {get;set;}
        public bool IsAnimationOpen { get; set; }

        public Furnace(string iD, int size, Vector2 location, GraphicsDevice graphics)
        {
            this.StorableItemType = StorableItemType.Furnace;
            this.ID = iD;
            this.Size = 1;
            this.Inventory = new Inventory(1);
            this.Location = location;
            this.GraphicsDevice = graphics;
            this.BackDropSourceRectangle = new Rectangle(224, 128, 48, 96);
            this.BackDropPosition = new Vector2(Game1.ScreenWidth / 6, Game1.ScreenHeight / 6);
            this.BackDropScale = 3f;

            //this.ItemSlots = new List<ItemStorageSlot>();
            //for (int i = 0; i < 1; i++)
            //{
            //    this.ItemSlots.Add(new ItemStorageSlot(graphics, this.Inventory, i, new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width / 2, this.BackDropPosition.Y + this.BackDropSourceRectangle.Height + 32 * this.BackDropScale), new Rectangle(208, 80, 32, 32), this.BackDropScale, true));

            //}
            //this.SmeltSlot = new ItemStorageSlot(graphics, new Inventory(1,1), 0, new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width / 2, this.BackDropPosition.Y), new Rectangle(208, 80, 32, 32), this.BackDropScale, true);
            //this.TimerStringLocation = new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width, this.BackDropPosition.Y + this.BackDropSourceRectangle.Height);

            redEsc = new RedEsc(
               new Vector2(this.BackDropPosition.X + this.BackDropSourceRectangle.Width * this.BackDropScale, this.BackDropPosition.Y), graphics);

            this.OrangeFlamePosition = new Vector2(this.BackDropPosition.X + 32, this.BackDropPosition.Y + this.BackDropSourceRectangle.Height * BackDropScale - 40 * BackDropScale);
            this.OrangeFlameCurrentPosition = this.OrangeFlamePosition;
            this.OrangeFlameSourceRectangle = new Rectangle(272, 160, 32, 32);
            this.FireSound = Game1.SoundManager.FurnaceFire.CreateInstance();

            this.CookTimeRequired = 5f;
        }
        public void Activate(TileManager TileManager, int layer, int x, int y)
        {
            if (!this.IsAnimationOpen)
            {
                Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.ChestOpen, true, 1f, .6f);
                this.IsAnimationOpen = true;

                this.TileManager = TileManager;
                this.Layer = layer;
                this.X = x;
                this.Y = y;
                this.IsUpdating = true;
                this.Tile = TileManager.AllTiles[layer][x, y];
                TileManager.AnimationFrames.Remove(Tile.TileKey);
                TileUtility.Animate(Dir.Right, layer, x, y, TileManager, false);
            }
        }

        public void StartCook()
        {
            this.TimeActivated = Game1.GlobalClock.SecondsPassedToday;
            this.IsCooking = true;
        }


        public void Deactivate()
        {
            if (this.IsAnimationOpen)
            {
                this.IsAnimationOpen = false;

                this.IsUpdating = false;
                TileUtility.Animate(Dir.Left, this.Layer, this.X, this.Y, this.TileManager, false);
            }
        }

        public bool DepositItem(Item item)
        {
            //if(Game1.ItemVault.GetItem(item.ID).SmeltedItem != 0)
            //{
            //    if(this.SmeltSlot.Inventory.IsPossibleToAddItem(item))
            //    {
            //        return this.SmeltSlot.Inventory.TryAddItem(item);
            //    }
            //}
            //else if(Game1.ItemVault.GetItem(item.ID).FuelValue > 0)
            //{
            //    if (this.ItemSlots[0].Inventory.IsPossibleToAddItem(item))
            //    {
            //        return this.ItemSlots[0].Inventory.TryAddItem(item);
            //    }
            //}
            return false;
        }
        public bool IsItemAllowedToBeStored(Item item)
        {
           

            //if(this.ItemSlots[0].Inventory.IsPossibleToAddItem(item) || this.SmeltSlot.Inventory.IsPossibleToAddItem(item))
            //{
            //    return true;
            //}

            return false;
        }
        public void Update(GameTime gameTime)
        {
            this.IsInventoryHovered = false;
            float flameYOffSet = 10 * (Game1.GlobalClock.SecondsPassedToday - this.TimeActivated);
            if(this.TimeActivated <= 0) //otherwise yoffset will increase forever
            {
                flameYOffSet = 0;
            }
            this.OrangeFlameCurrentPosition = new Vector2(this.OrangeFlamePosition.X, this.OrangeFlamePosition.Y -  flameYOffSet );
           
            if (!Game1.Player.ClickRangeRectangle.Intersects(new Rectangle((int)this.Location.X, (int)this.Location.Y, 16, 16)))
            {
                Deactivate();
            }


            //for (int i = 0; i < this.ItemSlots.Count; i++)
            //{
            //    this.ItemSlots[i].Update(gameTime);
            //    if (this.ItemSlots[i].Button.IsHovered)
            //    {

            //        this.IsInventoryHovered = true;
            //        this.CurrentHoveredSlot = this.ItemSlots[i];

            //    }
            //}

            //this.SmeltSlot.Update(gameTime);
            //if (this.SmeltSlot.Button.IsHovered)
            //{
            //    this.CurrentHoveredSlot = this.SmeltSlot;
            //    this.IsInventoryHovered = true;

            //}
            //InventorySlot smeltSlot = this.SmeltSlot.Inventory.currentInventory[0];


            //if(!this.IsCooking && this.ItemSlots[0].Inventory.currentInventory[0].ItemCount > 0 &&  smeltSlot.ItemCount > 0 && smeltSlot.GetItemData().SmeltedItem > 0) //if change was made to smelted item
            //{

            //        StartCook();
                
            //}
            if(this.IsCooking)
            {
                if (FireSound.State == SoundState.Stopped)
                {
                    FireSound.Play();
                }
            }
            //if(this.IsCooking && Game1.GlobalClock.SecondsPassedToday - this.TimeActivated > this.CookTimeRequired)
            //{
            //    int newID = smeltSlot.GetItemData().SmeltedItem;
            //    smeltSlot.RemoveItemFromSlot();
            //    smeltSlot.AddItemToSlot(Game1.ItemVault.GenerateNewItem(newID, null));
            //    this.ItemSlots[0].Inventory.currentInventory[0].RemoveItemFromSlot();
            //    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.FurnaceDone, true, .25f);

            //    this.TimeActivated = 0;
            //    this.IsCooking = false;
            //    this.FireSound.Stop();
            //}
            

            redEsc.Update(Game1.MouseManager);


            if (redEsc.isClicked)
            {
                    Deactivate();
                
                this.IsUpdating = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.BackDropPosition, this.BackDropSourceRectangle,
                Color.White, 0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None,Utility.StandardButtonDepth - .02f);
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.OrangeFlameCurrentPosition, this.OrangeFlameSourceRectangle,
                Color.White, 0f, Game1.Utility.Origin, this.BackDropScale, SpriteEffects.None,Utility.StandardButtonDepth - .03f);

            for (int i = 0; i < this.ItemSlots.Count; i++)
            {
                this.ItemSlots[i].Draw(spriteBatch);
            }

            this.SmeltSlot.Draw(spriteBatch);

            // spriteBatch.DrawString(Game1.AllTextures.MenuText, this.SimpleTimer.Time.ToString(), this.TimerStringLocation, Color.White, 0f, Game1.Utility.Origin, 2f, SpriteEffects.None,Utility.StandardButtonDepth + .01f);

            redEsc.Draw(spriteBatch);
        }

        public bool IsItemAllowedToBeStored(ItemData item)
        {
            throw new System.NotImplementedException();
        }

        public bool DepositItem(ItemData referenceItem)
        {
            throw new System.NotImplementedException();
        }
    }
}
