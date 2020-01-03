﻿using Microsoft.Xna.Framework;
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
        public bool IsAnySlotHovered { get; set; }
        public bool WasSlotJustReleased { get; set; }
        public Item ItemJustReleased { get; set; }
        public Sprite DragSprite { get; set; }
        public List<Button> AllSlots { get; set; }
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

        public BackPack(GraphicsDevice graphics, Inventory Inventory)
        {
            this.Graphics = graphics;
            this.Inventory = Inventory;
            this.IsActive = true;
            this.LargeBackgroundSourceRectangle = new Rectangle(208, 576, 336, 112);
            this.SmallBackgroundSourceRectangle = new Rectangle(208, 688, 336, 32);
            this.Scale = 2f;
            this.SmallPosition = new Vector2(Game1.PresentationParameters.BackBufferWidth / 4, Game1.PresentationParameters.BackBufferHeight * .9f);
            this.BigPosition = new Vector2(this.SmallPosition.X, this.SmallPosition.Y - this.LargeBackgroundSourceRectangle.Height * this.Scale + 32 * this.Scale);




            this.AllSlots = new List<Button>();
            int index = 0;
            for (int i = 0; i < 10; i++)
            {
                this.AllSlots.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 32, 32), this.Graphics, new Vector2(this.BigPosition.X + 32 * index * this.Scale + 16, this.SmallPosition.Y), CursorType.Normal) { ItemCounter = 0, Index = i + 1, HitBoxScale = 2f });
                index++;
            }
            index = 0;
            for (int i = 0; i < 10; i++)
            {
                this.AllSlots.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 32, 32), this.Graphics, new Vector2(this.BigPosition.X + 32 * index * this.Scale + 16, this.SmallPosition.Y - 32 * this.Scale - 16), CursorType.Normal) { ItemCounter = 0, Index = i + 1, HitBoxScale = 2f });
                index++;
            }
            index = 0;
            for (int i = 0; i < 10; i++)
            {
                this.AllSlots.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 32, 32), this.Graphics, new Vector2(this.BigPosition.X + 32 * index * this.Scale + 16, this.SmallPosition.Y - 64 * this.Scale - 16), CursorType.Normal) { ItemCounter = 0, Index = i + 1, HitBoxScale = 2f });
                index++;
            }
            TextBuilder = new TextBuilder("", .01f, 5);

            this.ExpandedButtonRectangle = new Rectangle(544, 656, 16, 48);
            this.RetractedButtonRectangle = new Rectangle(560, 656, 16, 48);
            ExpandButton = new Button(Game1.AllTextures.UserInterfaceTileSet, this.ExpandedButtonRectangle, graphics, new Vector2(this.SmallPosition.X + this.SmallBackgroundSourceRectangle.Width * this.Scale, this.SmallPosition.Y), CursorType.Normal, this.Scale);

            AllActions = new List<ActionTimer>();
        }



        public void Update(GameTime gameTime)
        {
            if (this.IsActive)
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
                ExpandButton.Update(Game1.myMouseManager);
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
                UpdateScrollWheel(Game1.myMouseManager);
                this.DragSprite = null;
                this.MouseIntersectsBackDrop = false;
                if (WasSliderUpdated && this.Inventory.currentInventory.ElementAt(currentSliderPosition - 1).SlotItems.Count > 0)
                {

                    this.ItemSwitchSourceRectangle = GetCurrentItemTexture();
                }
                else if (WasSliderUpdated && this.Inventory.currentInventory.ElementAt(currentSliderPosition - 1).SlotItems.Count <= 0)
                {
                    this.ItemSwitchSourceRectangle = new Rectangle(80, 0, 1, 1);
                }


                if (WasSliderUpdated && GetCurrentEquippedTool() != 666)
                {
                    //this might be broken
                    CheckGridItem();

                    AllActions.Add(new ActionTimer(1, AllActions.Count - 1));
                }

                for (int i = 0; i < AllActions.Count; i++)
                {
                    AllActions[i].Update(gameTime, AllActions);
                }

                if ((Game1.OldKeyBoardState.IsKeyDown(Keys.Q)) && (Game1.NewKeyBoardState.IsKeyUp(Keys.Q)))
                {
                    if (this.Inventory.currentInventory[currentSliderPosition - 1].SlotItems.Count > 0)
                    {
                        EjectItem();
                    }
                }

                this.MouseIntersectsBackDrop = DoesMouseIntersectBackDrop();


                TextBuilder.Update(gameTime);
                this.IsAnySlotHovered = false;


                for (int i = 0; i < this.NumberOfSlotsToUpdate; i++)
                {
                    UpdateInventorySlotTexture(this.Inventory, i);
                    this.AllSlots[i].Update(Game1.myMouseManager);
                    if (this.AllSlots[i].IsHovered)
                    {
                        this.IsAnySlotHovered = true;
                        if (this.AllSlots[i].ItemCounter > 0)
                        {
                            TextBuilder.Activate(false, TextBoxType.normal, false, this.Inventory.currentInventory[i].GetItem().Name, 1f,
                      new Vector2(this.AllSlots[i].Position.X, this.AllSlots[i].Position.Y - 32), 200f);
                            Game1.Player.UserInterface.InfoBox.IsActive = true;
                            switch (Game1.Player.UserInterface.CurrentOpenInterfaceItem)
                            {
                                case ExclusiveInterfaceItem.ShopMenu:
                                    Game1.Player.UserInterface.InfoBox.FitText(this.Inventory.currentInventory[i].GetItem().Name + ":  " + "Shop will buy for " + this.Inventory.currentInventory[i].GetItem().Price + ".", 1f);
                                    Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(this.AllSlots[i].Position.X - Game1.Player.UserInterface.InfoBox.SourceRectangle.Width + 50, this.AllSlots[i].Position.Y - 150);
                                    if (this.AllSlots[i].isClicked)
                                    {

                                        Game1.Player.UserInterface.CurrentShop.ShopMenu.TrySellToShop(this.Inventory.currentInventory[i].GetItem(), 1);
                                        this.Inventory.currentInventory[i].RemoveItemFromSlot();
                                    }
                                    break;

                                default:
                                    Game1.Player.UserInterface.InfoBox.FitText(this.Inventory.currentInventory[i].GetItem().Name + ":  " + this.Inventory.currentInventory[i].GetItem().Description, 1f);
                                    Game1.Player.UserInterface.InfoBox.WindowPosition = new Vector2(this.AllSlots[i].Position.X - Game1.Player.UserInterface.InfoBox.SourceRectangle.Width + 50, this.AllSlots[i].Position.Y - 150);
                                    break;
                            }
                            if (Game1.myMouseManager.IsClicked)
                            {
                                if (Game1.OldKeyBoardState.IsKeyDown(Keys.LeftShift))
                                {
                                    Item item = this.Inventory.currentInventory[i].GetItem();
                                    if (item != null)
                                    {

                                        if (Game1.Player.UserInterface.CurrentAccessedStorableItem != null)
                                        {
                                            if (Game1.Player.UserInterface.CurrentAccessedStorableItem.IsItemAllowedToBeStored(this.Inventory.currentInventory[i].GetItem()))
                                            {
                                                bool playAnimation = false;
                                                for (int shiftItem = this.Inventory.currentInventory[i].SlotItems.Count - 1; shiftItem >= 0; shiftItem--)
                                                {
                                                    if (Game1.Player.UserInterface.CurrentAccessedStorableItem.Inventory.TryAddItem(item))
                                                    {
                                                        playAnimation = true;
                                                        this.Inventory.currentInventory[i].SlotItems.RemoveAt(shiftItem);

                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }
                                                if (playAnimation)
                                                {
                                                    Game1.Player.DoPlayerAnimation(gameTime, AnimationType.HandsPicking, .25f);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Game1.Player.Inventory.AddToFirstEmptySlotOnly(item))
                                            {
                                                for (int shiftItem = this.Inventory.currentInventory[i].SlotItems.Count - 1; shiftItem >= 0; shiftItem--)
                                                {
                                                    if (Game1.Player.Inventory.TryAddItem(item))
                                                    {
                                                        this.Inventory.currentInventory[i].SlotItems.RemoveAt(shiftItem);

                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    if (this.AllSlots[i].isClickedAndHeld && this.AllSlots[i].ItemCounter != 0)
                    {
                        Item tempItem = this.Inventory.currentInventory[i].GetItem();

                        Sprite tempSprite = new Sprite(this.Graphics, Game1.AllTextures.ItemSpriteSheet, Game1.Player.Inventory.currentInventory.ElementAt(i).SlotItems[0].SourceTextureRectangle,
                            Game1.myMouseManager.WorldMousePosition, 16, 16)
                        { IsBeingDragged = true, TextureScaleX = 5f, TextureScaleY = 5f };
                        this.DragSprite = tempSprite;
                        this.DragSprite.Update(gameTime, new Vector2(Game1.myMouseManager.Position.X - 16, Game1.myMouseManager.Position.Y - 16));
                        Game1.myMouseManager.ChangeMouseTexture(CursorType.Normal);


                    }
                }


                for (int i = 0; i < this.NumberOfSlotsToUpdate; i++)
                {
                    //INTERACTIONS WITH RELEASE ITEM 
                    if (this.AllSlots[i].wasJustReleased && this.AllSlots[i].ItemCounter > 0)
                    {
                        if (this.MouseIntersectsBackDrop)
                        {


                            this.WasSlotJustReleased = true;

                            Item tempItem = this.Inventory.currentInventory[i].GetItem();
                            this.ItemJustReleased = tempItem;

                            if (this.IsAnySlotHovered)
                            {
                                int index = 0;
                                for (int m = 0; m < this.AllSlots.Count; m++)
                                {
                                    index = m;
                                    if (this.AllSlots[m].IsHovered)
                                    {
                                        InventorySlot currentItems = this.Inventory.currentInventory[i];
                                        this.Inventory.currentInventory[i] = this.Inventory.currentInventory[m];
                                        this.Inventory.currentInventory[m] = currentItems;

                                        return;
                                    }

                                }
                            }
                        }
                        else if (InteractWithStorageItem(gameTime, i))
                        {

                        }
                        else if (Game1.Player.UserInterface.CurrentOpenInterfaceItem == ExclusiveInterfaceItem.None)
                        {

                            Item tempItem = this.Inventory.currentInventory[i].GetItem();
                            int currentItemCount = this.AllSlots[i].ItemCounter;
                            for (int j = 0; j < currentItemCount; j++)
                            {
                                this.Inventory.currentInventory[i].RemoveItemFromSlot();
                                this.AllSlots[i].ItemCounter--;

                                Item newWorldItem = Game1.ItemVault.GenerateNewItem(tempItem.ID, new Vector2(Game1.Player.Rectangle.X, Game1.Player.Rectangle.Y), true);
                                newWorldItem.IsTossable = true;
                                Game1.GetCurrentStage().AllItems.Add(newWorldItem);

                            }


                        }
                    }

                }
            }
        }

        private bool DoesMouseIntersectBackDrop()
        {
            if (this.Expanded)
            {
                if (Game1.myMouseManager.MouseRectangle.Intersects(new Rectangle((int)this.BigPosition.X, (int)this.BigPosition.Y, (int)(this.LargeBackgroundSourceRectangle.Width * this.Scale), (int)(this.LargeBackgroundSourceRectangle.Height * this.Scale))))
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
                if (Game1.myMouseManager.MouseRectangle.Intersects(new Rectangle((int)this.SmallPosition.X, (int)this.SmallPosition.Y, (int)(this.SmallBackgroundSourceRectangle.Width * this.Scale), (int)(this.SmallBackgroundSourceRectangle.Height * this.Scale))))
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
                    if (this.AllSlots[i].IsHovered)
                    {
                        this.AllSlots[i].Draw(spriteBatch, this.AllSlots[i].ItemSourceRectangleToDraw, this.AllSlots[i].BackGroundSourceRectangle, Game1.AllTextures.MenuText, this.AllSlots[i].ItemCounter.ToString(), new Vector2(this.AllSlots[i].Position.X + 5, this.AllSlots[i].Position.Y + 5), Color.White * .5f, 2f, 2f, Game1.Utility.StandardButtonDepth);
                    }
                    else
                    {
                        this.AllSlots[i].Draw(spriteBatch, this.AllSlots[i].ItemSourceRectangleToDraw, this.AllSlots[i].BackGroundSourceRectangle, Game1.AllTextures.MenuText, this.AllSlots[i].ItemCounter.ToString(), new Vector2(this.AllSlots[i].Position.X + 5, this.AllSlots[i].Position.Y + 5), Color.White, 2f, 2f, Game1.Utility.StandardButtonDepth);
                    }

                }
                if (this.DragSprite != null)
                {


                    this.DragSprite.DrawFromUIToWorld(spriteBatch, .72f);
                }

                spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle((int)this.AllSlots[currentSliderPosition - 1].Position.X, (int)this.AllSlots[currentSliderPosition - 1].Position.Y, 68, 67), new Rectangle(80, 0, 68, 67),
                    Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .71f);
            }

        }

        public void EjectItem()
        {

            Item newWorldItem = Game1.ItemVault.GenerateNewItem(this.Inventory.currentInventory[currentSliderPosition - 1].SlotItems[0].ID, new Vector2(Game1.Player.Rectangle.X, Game1.Player.Rectangle.Y), true);
            newWorldItem.IsTossable = true;
            Game1.GetCurrentStage().AllItems.Add(newWorldItem);
            this.Inventory.currentInventory[currentSliderPosition - 1].RemoveItemFromSlot();
            this.AllSlots[currentSliderPosition - 1].ItemCounter--;

        }

        public void UpdateInventorySlotTexture(Inventory Inventory, int index)
        {
            if (Inventory.currentInventory.ElementAt(index) == null)
            {
                this.AllSlots[index].ItemCounter = 0;

            }
            else
            {
                this.AllSlots[index].ItemCounter = Inventory.currentInventory.ElementAt(index).SlotItems.Count;
            }

            if (this.AllSlots[index].ItemCounter > 0)
            {
                this.AllSlots[index].Texture = Inventory.currentInventory.ElementAt(index).SlotItems[0].ItemSprite.AtlasTexture;
                this.AllSlots[index].ItemSourceRectangleToDraw = Inventory.currentInventory.ElementAt(index).SlotItems[0].SourceTextureRectangle;
            }
            else
            {
                this.AllSlots[index].Texture = Game1.AllTextures.UserInterfaceTileSet;
                this.AllSlots[index].ItemSourceRectangleToDraw = new Rectangle(1568, 0, 32, 32);
            }
        }

        public bool InteractWithStorageItem(GameTime gameTime, int index)
        {
            if (Game1.Player.UserInterface.IsAnyStorageItemOpen)
            {
                if (Game1.Player.UserInterface.CurrentAccessedStorableItem.IsInventoryHovered)
                {
                    if (Game1.Player.UserInterface.CurrentAccessedStorableItem.IsItemAllowedToBeStored(this.Inventory.currentInventory[index].GetItem()))
                    {
                        if (Game1.Player.UserInterface.CurrentAccessedStorableItem.CurrentHoveredSlot.Inventory.TryAddItem(this.Inventory.currentInventory[index].GetItem()))
                        {
                            Game1.Player.DoPlayerAnimation(gameTime, AnimationType.HandsPicking);
                            this.Inventory.currentInventory[index].RemoveItemFromSlot();
                            this.AllSlots[index].ItemCounter--;
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
            if (this.Inventory.currentInventory.ElementAt(currentSliderPosition - 1).SlotItems.Count > 0)
            {
                return this.Inventory.currentInventory.ElementAt(currentSliderPosition - 1).GetItem().ID;
            }
            else
            {
                return -50; //Placeholder
            }
        }

        public Item GetCurrentEquippedToolAsItem()
        {
            if (this.Inventory != null && this.Inventory.currentInventory.ElementAt(currentSliderPosition - 1).SlotItems.Count > 0)
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
                if (Game1.GetCurrentStageInt() == Stages.OverWorld)
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
                else if (Game1.GetCurrentStageInt() == Stages.Forest)
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
}
