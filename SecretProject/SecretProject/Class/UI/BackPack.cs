using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.Playable;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using XMLData.ItemStuff;

namespace SecretProject.Class.UI
{
    public class BackPack : IExclusiveInterfaceComponent
    {
        public bool IsActive { get; set; }
        public bool FreezesGame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public GraphicsDevice Graphics { get; set; }
        public Inventory Inventory { get; set; }
        public int Capacity { get; set; }
        public Vector2 BigPosition { get; set; }
        public Vector2 SmallPosition { get; set; }
        public Rectangle LargeBackgroundSourceRectangle { get; set; }
        public Rectangle SmallBackgroundSourceRectangle { get; set; }
        public float Scale { get; set; }
        public bool WasSlotJustReleased { get; set; }
        public Item ItemJustReleased { get; set; }
        public Sprite DragSprite { get; set; }
        public List<Button> AllItemButtons { get; set; }
        Button ExpandButton;
        public Rectangle ExpandedButtonRectangle { get; set; }
        public Rectangle RetractedButtonRectangle { get; set; }

        TextBuilder TextBuilder;

        public bool MouseIntersectsBackDrop { get; set; }

        public bool Expanded { get; set; }
        public int NumberOfSlotsToUpdate { get; set; }

        //MAIN TOOLBAR STUFF
        public Rectangle ItemSwitchSourceRectangle { get; set; }
        public Item TempItem { get; set; }
        public int currentSliderPosition = 1;
        public bool WasSliderUpdated = false;

        public List<ActionTimer> AllActions;

        public Item CurrentEquippedItem { get; set; }

        public DragSlot DraggedSlot { get; private set; }
        public bool IsDragSlotActive { get; private set; }


        public Rectangle InventoryAreaRectangle { get; set; }

        public Button ButtonHoveredLastFrame { get; private set; }

        public BackPack(GraphicsDevice graphics, Inventory Inventory)
        {
            this.Graphics = graphics;
            this.Inventory = Inventory;
            this.IsActive = true;
            this.LargeBackgroundSourceRectangle = new Rectangle(208, 576, 336, 112);
            this.SmallBackgroundSourceRectangle = new Rectangle(208, 688, 336, 32);
            this.Scale = 2f;
            float centeredX = Game1.Utility.CenterRectangleOnScreen(SmallBackgroundSourceRectangle, this.Scale).X;
            this.SmallPosition = new Vector2(centeredX, Game1.PresentationParameters.BackBufferHeight * .9f);
            this.BigPosition = new Vector2(centeredX, this.SmallPosition.Y - this.LargeBackgroundSourceRectangle.Height * this.Scale + 32 * this.Scale);




            this.AllItemButtons = new List<Button>();
            int index = 0;
            for (int i = 0; i < 10; i++)
            {
                this.AllItemButtons.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 32, 32), this.Graphics, new Vector2(this.BigPosition.X + 32 * index * this.Scale + 16, this.SmallPosition.Y), CursorType.Normal) { ItemCounter = 0, Index = i + 1, HitBoxScale = 2f });
                index++;
            }
            index = 0;
            for (int i = 0; i < 10; i++)
            {
                this.AllItemButtons.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 32, 32), this.Graphics, new Vector2(this.BigPosition.X + 32 * index * this.Scale + 16, this.SmallPosition.Y - 32 * this.Scale - 16), CursorType.Normal) { ItemCounter = 0, Index = i + 1, HitBoxScale = 2f });
                index++;
            }
            index = 0;
            for (int i = 0; i < 10; i++)
            {
                this.AllItemButtons.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 32, 32), this.Graphics, new Vector2(this.BigPosition.X + 32 * index * this.Scale + 16, this.SmallPosition.Y - 64 * this.Scale - 16), CursorType.Normal) { ItemCounter = 0, Index = i + 1, HitBoxScale = 2f });
                index++;
            }
            TextBuilder = new TextBuilder("", .01f, 5);

            this.ExpandedButtonRectangle = new Rectangle(544, 656, 16, 48);
            this.RetractedButtonRectangle = new Rectangle(560, 656, 16, 48);
            ExpandButton = new Button(Game1.AllTextures.UserInterfaceTileSet, this.ExpandedButtonRectangle, graphics, new Vector2(this.SmallPosition.X + this.SmallBackgroundSourceRectangle.Width * this.Scale, this.SmallPosition.Y), CursorType.Normal, this.Scale);

            AllActions = new List<ActionTimer>();

            this.DraggedSlot = new DragSlot();
        }


        /// <summary>
        /// If held item has a food value, try to eat it. If eaten, remove from inventory and restore stamina based on item food value.
        /// </summary>
        public void HandleFoodItem()
        {
            Item item = GetCurrentEquippedToolAsItem();
            if (item != null)
            {
                if (Game1.ItemVault.GetItem(item.ID).StaminaRestoreAmount > 0)
                {
                    Game1.Player.UserInterface.StaminaBar.IncreaseStamina(Game1.ItemVault.GetItem(item.ID).StaminaRestoreAmount);
                    this.Inventory.RemoveItem(item.ID);
                }
            }
        }

        private void HandleInventoryExpansion()
        {
            if (this.Expanded)
            {
                this.NumberOfSlotsToUpdate = 30;
                ExpandButton.BackGroundSourceRectangle = this.RetractedButtonRectangle;

            }
            else
            {
                this.NumberOfSlotsToUpdate = 10;
                ExpandButton.BackGroundSourceRectangle = this.ExpandedButtonRectangle;

            }
            ExpandButton.Update(Game1.MouseManager);
            if (ExpandButton.isClicked)
            {
                this.Expanded = !this.Expanded;
                if (this.Expanded)
                {
                    Game1.SoundManager.PlayOpenUI();
                }
                else
                {
                    Game1.SoundManager.PlayCloseUI();
                }
            }
        }

        private void UpdateItemSwitchSourceRectangle()
        {
            if (WasSliderUpdated && this.Inventory.currentInventory.ElementAt(currentSliderPosition - 1).ItemCount > 0)
            {

                this.ItemSwitchSourceRectangle = GetCurrentItemTexture();
            }
            else if (WasSliderUpdated && this.Inventory.currentInventory.ElementAt(currentSliderPosition - 1).ItemCount <= 0)
            {
                this.ItemSwitchSourceRectangle = new Rectangle(80, 0, 1, 1);
            }
        }

        public void ItemInfoInteraction(Button button, InventorySlot slot, ItemData itemData)
        {
            TextBuilder.Activate(false, TextBoxType.normal, false, itemData.Name, 1f,
                      new Vector2(button.Position.X, button.Position.Y - 32), 200f);

            Vector2 infoBoxPosition = new Vector2(button.Position.X, button.Position.Y - 150);

            InfoPopUp infoBox;
           
            switch (Game1.Player.UserInterface.CurrentOpenInterfaceItem)
            {
                case ExclusiveInterfaceItem.ShopMenu:
                     infoBox = new InfoPopUp(this.Graphics, itemData, infoBoxPosition, true);
 
                    if (button.isRightClicked)
                    {
                        int numberToSell = 1;
                        if (Game1.KeyboardManager.OldKeyBoardState.IsKeyDown(Keys.LeftShift))
                        {
                            numberToSell = slot.ItemCount;
                        }
                        for (int numSell = 0; numSell < numberToSell; numSell++)
                        {
                            Game1.Player.UserInterface.CurrentShop.ShopMenu.TrySellToShop(slot.GetItem(), 1);
                            slot.RemoveItemFromSlot();
                        }

                    }
                    break;

                default:
                     infoBox = new InfoPopUp(this.Graphics, itemData, infoBoxPosition);
                    break;
            }
            Game1.Player.UserInterface.InfoBox = infoBox;
        }

        public void Update(GameTime gameTime)
        {
            this.Inventory = Game1.Player.Inventory;
            this.Inventory.HasChangedSinceLastFrame = false;
            if (this.Expanded)
            {
                this.InventoryAreaRectangle = new Rectangle((int)this.BigPosition.X, (int)this.BigPosition.Y, (int)(this.LargeBackgroundSourceRectangle.Width * this.Scale),
                    (int)(this.LargeBackgroundSourceRectangle.Height * this.Scale));
            }
            else
            {
                this.InventoryAreaRectangle = new Rectangle((int)this.SmallPosition.X, (int)this.SmallPosition.Y, (int)(this.SmallBackgroundSourceRectangle.Width * this.Scale),
                    (int)(this.SmallBackgroundSourceRectangle.Height * this.Scale));
            }

            if (this.IsActive)
            {
                UpdateScrollWheel(Game1.MouseManager);
                this.CurrentEquippedItem = GetCurrentEquippedToolAsItem();
                this.MouseIntersectsBackDrop = DoesMouseIntersectBackDrop();
                HandleInventoryExpansion();

                if (Game1.MouseManager.IsRightClicked)
                {
                    if (this.IsDragSlotActive)
                    {
                        this.IsDragSlotActive = false;
                    }
                }

                if (this.DragSprite != null)
                {
                    this.DragSprite.Position = Game1.MouseManager.UIPosition;
                }

                this.MouseIntersectsBackDrop = false;

                UpdateItemSwitchSourceRectangle();

                if (WasSliderUpdated && this.CurrentEquippedItem != null && this.CurrentEquippedItem.ID != 666)
                {
                    CheckGridItem();
                    AllActions.Add(new ActionTimer(1, AllActions.Count - 1));
                }
                else if (this.CurrentEquippedItem == null)
                {
                    Game1.GetCurrentStage().AllTiles.GridItem = null;
                }

                for (int i = 0; i < AllActions.Count; i++)
                {
                    AllActions[i].Update(gameTime, AllActions);
                }

                if (Game1.KeyboardManager.WasKeyPressed(Keys.Q))
                {
                    if (this.Inventory.currentInventory[currentSliderPosition - 1].ItemCount > 0)
                    {
                        EjectItem();
                    }
                }
                TextBuilder.Update(gameTime);

                for (int i = 0; i < NumberOfSlotsToUpdate; i++)
                {
                    Button itemButton = AllItemButtons[i];
                    itemButton.Update(Game1.MouseManager);

                    if (itemButton.IsHovered)
                    {
                        InventorySlot newInventorySlot = Inventory.currentInventory[i];
                        if (newInventorySlot.ItemCount > 0)
                        {
                            Game1.Player.UserInterface.InfoBox.IsActive = true;
                        }
                        
                        if (itemButton != this.ButtonHoveredLastFrame)
                        {
                            if(newInventorySlot.ItemCount > 0)
                            {
                                
                                ItemInfoInteraction(itemButton, newInventorySlot, newInventorySlot.GetItemData());
                            this.ButtonHoveredLastFrame = itemButton;
                            }
                            
                        }

                        
                        if (itemButton.isClicked)
                        {

                            HandleDragSlot(i);

                        }
                        else if (Game1.MouseManager.IsRightClicked)
                        {

                            if (newInventorySlot.ItemCount > 0)
                            {
                                if (Game1.Player.UserInterface.CurrentOpenInterfaceItem == ExclusiveInterfaceItem.ShopMenu)
                                {
                                    Item item = newInventorySlot.GetItem();

                                    if (Game1.KeyboardManager.OldKeyBoardState.IsKeyDown(Keys.LeftShift))
                                    {
                                        int slotCounter = newInventorySlot.ItemCount;
                                        for (int p = 0; p < slotCounter; p++)
                                        {
                                            Game1.Player.UserInterface.CurrentShop.ShopMenu.TrySellToShop(item, 1);
                                            newInventorySlot.RemoveItemFromSlot();
                                        }
                                    }
                                    else
                                    {
                                        Game1.Player.UserInterface.CurrentShop.ShopMenu.TrySellToShop(item, 1);
                                        newInventorySlot.RemoveItemFromSlot();
                                    }


                                }
                                else
                                {
                                    HandleFoodItem();
                                }

                            }

                        }
                    }

                    UpdateInventorySlotTexture(this.Inventory, i);
                }

                HandleDragOutOfInventory();
            }
        }

        /// <summary>
        /// If item is dragged outside of inventory bounds, drop it into the world.
        /// </summary>
        private void HandleDragOutOfInventory()
        {
            if (Game1.MouseManager.IsClicked && this.IsDragSlotActive)
            {
                if (!Game1.MouseManager.MouseRectangle.Intersects(this.InventoryAreaRectangle))
                {
                    Item tempItem = DraggedSlot.InventorySlot.GetItem();
                    int currentItemCount = DraggedSlot.InventorySlot.ItemCount;

                    if (InteractWithStorageItem(DraggedSlot.InventorySlot))
                    {

                    }
                    else
                    {
                        for (int j = 0; j < currentItemCount; j++)
                        {
                            this.Inventory.currentInventory[DraggedSlot.Index].RemoveItemFromSlot();
                            Item newWorldItem = Game1.ItemVault.GenerateNewItem(tempItem.ID, new Vector2(Game1.Player.Rectangle.X, Game1.Player.Rectangle.Y), true, Game1.GetCurrentStage().AllTiles.GetItems(Game1.Player.position));
                            newWorldItem.IsTossable = true;

                        }


                    }

                    this.DragSprite = null;
                    this.IsDragSlotActive = false;
                }
            }
        }

        /// <summary>
        /// Determines what happens when an item is clicked, and whether or not it is placed down into another inventory slot. Will change the drag sprite texture if needed.
        /// </summary>
        /// <param name="slotIndex"></param>
        private void HandleDragSlot(int slotIndex)
        {
            if (Inventory.currentInventory[slotIndex].ItemCount > 0) //some item already occupies the slot we are trying to interact with.
            {
                if (IsDragSlotActive && DraggedSlot.InventorySlot.ItemCount > 0)
                {
                    InventorySlot newSlot = new InventorySlot(Inventory.currentInventory[slotIndex].Item)
                    {
                        ItemCount = Inventory.currentInventory[slotIndex].ItemCount
                    };


                    InventorySlot newNewSlot = new InventorySlot(DraggedSlot.InventorySlot.Item)
                    {
                        ItemCount = DraggedSlot.InventorySlot.ItemCount
                    };
                    this.DraggedSlot.InventorySlot = newSlot;

                    Inventory.currentInventory[this.DraggedSlot.Index] = this.DraggedSlot.InventorySlot;
                    this.DraggedSlot.Index = slotIndex;
                    Inventory.currentInventory[slotIndex] = newNewSlot;

                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.UIClick, true, .25f, 0f);
                    this.IsDragSlotActive = false;

                }
                else
                {

                    this.DraggedSlot.InventorySlot = Inventory.currentInventory[slotIndex];
                    this.DraggedSlot.Index = slotIndex;
                    this.IsDragSlotActive = true;
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.UIClick, true, .25f, 0f);
                    Game1.isMyMouseVisible = false;
                }


            }
            else
            {
                if (IsDragSlotActive) //if clicking on empty slot and we are dragging an item.
                {

                    Inventory.currentInventory[slotIndex] = new InventorySlot(DraggedSlot.InventorySlot.Item)
                    {
                        ItemCount = DraggedSlot.InventorySlot.ItemCount
                    };

                    this.DraggedSlot.InventorySlot.Clear();
                    Game1.SoundManager.PlaySoundEffect(Game1.SoundManager.UIClick, true, .25f, 0f);
                    this.IsDragSlotActive = false;
                }

            }
            if (this.IsDragSlotActive)
            {
                this.DragSprite = new Sprite(this.Graphics, Game1.AllTextures.ItemSpriteSheet, this.DraggedSlot.InventorySlot.GetItem().SourceTextureRectangle, Game1.MouseManager.UIPosition, 16, 16) { TextureScaleX = 5f, TextureScaleY = 5f };
            }
            else
            {
                this.DragSprite = null;
            }
        }


        public bool InteractWithStorageItem(InventorySlot slot)
        {
            IStorableItemBuilding storageItem = Game1.Player.UserInterface.CurrentAccessedStorableItem;
            if(storageItem != null)
            {
                if (storageItem.IsUpdating)
                {
                    if (storageItem.IsInventoryHovered)
                    {

                        for (int i = 0; i < storageItem.ItemSlots.Count - 1; i++)
                        {
                            if (storageItem.ItemSlots[i].Button.IsHovered)
                            {
                                int count = slot.ItemCount;
                                for (int j = 0; j < count; j++)
                                {
                                    if (storageItem.IsItemAllowedToBeStored(slot.GetItem()))
                                    {
                                        if (Game1.Player.UserInterface.CurrentAccessedStorableItem.CurrentHoveredSlot.Inventory.currentInventory[i].AddItemToSlot(slot.GetItem()))
                                        {
                                            slot.RemoveItemFromSlot();
                                        }
                                        else if (Game1.Player.UserInterface.CurrentAccessedStorableItem.CurrentHoveredSlot.Inventory.TryAddItem(slot.GetItem()))
                                        {
                                            slot.RemoveItemFromSlot();

                                        }
                                    }
                                }
                                break;
                            }
                        }


                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
           
        }
        private void HandleStorageItem(int i, Item item)
        {
            if (Game1.Player.UserInterface.CurrentAccessedStorableItem.IsItemAllowedToBeStored(this.Inventory.currentInventory[i].GetItem()))
            {
                bool playAnimation = false;
                for (int shiftItem = this.Inventory.currentInventory[i].ItemCount - 1; shiftItem >= 0; shiftItem--)
                {
                    if (Game1.Player.UserInterface.CurrentAccessedStorableItem.DepositItem(item))
                    {
                        playAnimation = true;
                        this.Inventory.currentInventory[i].RemoveItemFromSlot();

                    }
                    else
                    {
                        break;
                    }
                }
                if (playAnimation)
                {
                    Game1.Player.DoPlayerAnimation(AnimationType.HandsPicking, .25f);
                }
            }
        }

        private bool DoesMouseIntersectBackDrop()
        {
            if (this.Expanded)
            {
                if (Game1.MouseManager.MouseRectangle.Intersects(new Rectangle((int)this.BigPosition.X, (int)this.BigPosition.Y, (int)(this.LargeBackgroundSourceRectangle.Width * this.Scale), (int)(this.LargeBackgroundSourceRectangle.Height * this.Scale))))
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (Game1.MouseManager.MouseRectangle.Intersects(new Rectangle((int)this.SmallPosition.X, (int)this.SmallPosition.Y, (int)(this.SmallBackgroundSourceRectangle.Width * this.Scale), (int)(this.SmallBackgroundSourceRectangle.Height * this.Scale))))
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsActive)
            {

                ExpandButton.Draw(spriteBatch);

                TextBuilder.Draw(spriteBatch, .75f);
                if (this.Expanded)
                {
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.BigPosition, this.LargeBackgroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth - .1f);
                }
                else
                {
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, this.SmallPosition, this.SmallBackgroundSourceRectangle, Color.White, 0f, Game1.Utility.Origin, this.Scale, SpriteEffects.None, Game1.Utility.StandardButtonDepth - .1f);
                }

                for (int i = 0; i < this.NumberOfSlotsToUpdate; i++)
                {
                    float colorMultiplier = 1f;
                    if (this.AllItemButtons[i].IsHovered)
                    {
                        colorMultiplier = .5f;


                    }
                    this.AllItemButtons[i].Draw(spriteBatch, this.AllItemButtons[i].ItemSourceRectangleToDraw, this.AllItemButtons[i].BackGroundSourceRectangle, Game1.AllTextures.MenuText, this.AllItemButtons[i].ItemCounter.ToString(), new Vector2(this.AllItemButtons[i].Position.X + 5, this.AllItemButtons[i].Position.Y + 5), Color.White * colorMultiplier, 2f, 2f, Game1.Utility.StandardButtonDepth);
                    Item item = this.Inventory.currentInventory[i].GetItem();
                    if (item != null)
                    {
                        if (item.Durability > 0)
                        {
                            spriteBatch.Draw(Game1.AllTextures.redPixel, new Rectangle((int)AllItemButtons[i].Position.X + 4, (int)AllItemButtons[i].Position.Y + (int)AllItemButtons[i].HitBoxRectangle.Height - 8, (int)(item.DurabilityLineWidth * 50), 8), null,
    Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .9f);
                        }

                    }


                }
                if (this.IsDragSlotActive && this.DragSprite != null)
                {


                    this.DragSprite.Draw(spriteBatch, .85f);
                }

                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle((int)this.AllItemButtons[currentSliderPosition - 1].Position.X, (int)this.AllItemButtons[currentSliderPosition - 1].Position.Y, 68, 67), new Rectangle(80, 0, 68, 67),
                    Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .71f);
            }

        }

        private void EjectItem()
        {

            Item oldItem = this.Inventory.currentInventory[currentSliderPosition - 1].GetItem();

            Item newWorldItem = Game1.ItemVault.GenerateNewItem(this.Inventory.currentInventory[currentSliderPosition - 1].GetItem().ID, new Vector2(Game1.Player.MainCollider.Rectangle.X, Game1.Player.MainCollider.Rectangle.Y), true, Game1.GetCurrentStage().AllTiles.GetItems(Game1.Player.position));
            newWorldItem.IsTossable = true;
            newWorldItem.Durability = oldItem.Durability;

            this.Inventory.currentInventory[currentSliderPosition - 1].RemoveItemFromSlot();
            this.AllItemButtons[currentSliderPosition - 1].ItemCounter--;

        }

        public void UpdateInventorySlotTexture(Inventory Inventory, int index)
        {
            if (Inventory.currentInventory.ElementAt(index) == null)
            {
                this.AllItemButtons[index].ItemCounter = 0;

            }
            else
            {
                this.AllItemButtons[index].ItemCounter = Inventory.currentInventory.ElementAt(index).ItemCount;
            }

            if (this.AllItemButtons[index].ItemCounter > 0)
            {
                this.AllItemButtons[index].Texture = Inventory.currentInventory.ElementAt(index).GetItem().ItemSprite.AtlasTexture;
                this.AllItemButtons[index].ItemSourceRectangleToDraw = Inventory.currentInventory.ElementAt(index).GetItem().SourceTextureRectangle;
            }
            else
            {
                this.AllItemButtons[index].Texture = Game1.AllTextures.UserInterfaceTileSet;
                this.AllItemButtons[index].ItemSourceRectangleToDraw = new Rectangle(1568, 0, 32, 32);
            }
        }



        #region SCROLLWHEEL
        private void UpdateScrollWheel(MouseManager mouse)
        {
            WasSliderUpdated = false;
            int oldSliderPosition = currentSliderPosition;

            if ((Game1.Player.controls.pressedKeys != null))
            {
                if (Game1.Player.controls.pressedKeys.Contains(Keys.D1))
                {
                    currentSliderPosition = 1;
                }

                if (Game1.Player.controls.pressedKeys.Contains(Keys.D2))
                {
                    currentSliderPosition = 2;
                }
                if (Game1.Player.controls.pressedKeys.Contains(Keys.D3))
                {
                    currentSliderPosition = 3;
                }
                if (Game1.Player.controls.pressedKeys.Contains(Keys.D4))
                {
                    currentSliderPosition = 4;
                }
                if (Game1.Player.controls.pressedKeys.Contains(Keys.D5))
                {
                    currentSliderPosition = 5;
                }
                if (Game1.Player.controls.pressedKeys.Contains(Keys.D6))
                {
                    currentSliderPosition = 6;
                }
                if (Game1.Player.controls.pressedKeys.Contains(Keys.D7))
                {
                    currentSliderPosition = 7;
                }
            }

            if (mouse.HasScrollWheelValueDecreased)
            {
                currentSliderPosition += 1;
            }
            else if (mouse.HasScrollWheelValueIncreased)
            {
                currentSliderPosition -= 1;
            }

            if (currentSliderPosition > 10)
            {
                currentSliderPosition = 10;
            }
            if (currentSliderPosition < 1)
            {
                currentSliderPosition = 1;
            }

            if (oldSliderPosition != currentSliderPosition)
            {
                WasSliderUpdated = true;
            }

        }
        #endregion
        public int GetCurrentEquippedTool()
        {
            if (this.Inventory.currentInventory.ElementAt(currentSliderPosition - 1).GetItem() != null)
            {
                return this.Inventory.currentInventory.ElementAt(currentSliderPosition - 1).GetItem().ID;
            }

            else
            {
                return -1; //Placeholder
            }
        }

        public Item GetCurrentEquippedToolAsItem()
        {
            if (this.Inventory != null && this.Inventory.currentInventory.ElementAt(currentSliderPosition - 1).ItemCount > 0)
            {
                return this.Inventory.currentInventory.ElementAt(currentSliderPosition - 1).GetItem();
            }
            else
            {
                return null;
            }
        }

        public void CheckGridItem()
        {
            if (Game1.ItemVault.ExteriorGridItems != null && Game1.ItemVault.InteriorGridItems != null)
            {
                if (Game1.GetCurrentStageInt() == Stages.OverWorld || Game1.GetCurrentStageInt() == Stages.UnderWorld)
                {
                    if (Game1.ItemVault.ExteriorGridItems.ContainsKey(GetCurrentEquippedTool()))
                    {
                        Game1.GetCurrentStage().AllTiles.GridItem = Game1.ItemVault.ExteriorGridItems[GetCurrentEquippedTool()];


                    }
                    else
                    {
                        Game1.GetCurrentStage().AllTiles.GridItem = null;
                    }
                }
                else if (Game1.GetCurrentStageInt() == Stages.PlayerHouse)
                {
                    if (Game1.ItemVault.InteriorGridItems.ContainsKey(GetCurrentEquippedTool()))
                    {
                        Game1.GetCurrentStage().AllTiles.GridItem = Game1.ItemVault.InteriorGridItems[GetCurrentEquippedTool()];


                    }
                    else
                    {
                        Game1.GetCurrentStage().AllTiles.GridItem = null;
                    }
                }
            }
            else
            {
                Game1.GetCurrentStage().AllTiles.GridItem = null;
            }
        }

        public Rectangle GetCurrentItemTexture()
        {
            return this.Inventory.currentInventory.ElementAt(currentSliderPosition - 1).GetItem().SourceTextureRectangle;
        }

        public void DrawToStageMatrix(SpriteBatch spriteBatch)
        {
            //if action still exists and isn't complete we'll still draw it. 
            if (AllActions.Count > 0 && !AllActions[AllActions.Count - 1].ActionComplete)
            {
                spriteBatch.Draw(Game1.AllTextures.ItemSpriteSheet, sourceRectangle: this.ItemSwitchSourceRectangle, destinationRectangle: new Rectangle((int)Game1.Player.position.X + 3,
                    (int)Game1.Player.position.Y - 15, 16, 16), color: Color.White, layerDepth: 1, scale: new Vector2(1f, 1f));
            }


        }

    }

    public class DragSlot
    {
        public int Index { get; set; }
        public InventorySlot InventorySlot { get; set; }
    }
}
