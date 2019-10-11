using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SecretProject.Class.Controls;
using SecretProject.Class.DialogueStuff;
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.StageFolder;
using SecretProject.Class.TileStuff;
using SecretProject.Class.Universal;

namespace SecretProject.Class.UI
{
    //TODO: Switch statements on buttons
    public enum buttonIsClicked
    {
        none = 0,
        menu = 1,
        inv = 2,
        scrollTree = 3,
    }

    public class ToolBar
    {
        //--------------------------------------
        //Textures
        public Button InGameMenu { get; set; }
        public Button OpenInventory { get; set; }
        public Button ScrollTree { get; set; }


        public Texture2D ToolBarButton { get; set; }

        public List<Button> AllNonInventoryButtons { get; set; }
        public List<Button> AllSlots { get; set; }


        public Rectangle ItemSwitchSourceRectangle { get; set; }


        public Item TempItem { get; set; }

        public Rectangle BackGroundTextureRectangle { get; set; }

        public bool MouseOverToolBar { get; set; }

        public buttonIsClicked toolBarState = buttonIsClicked.none;
        GraphicsDevice graphicsDevice;
        ContentManager content;

        public Vector2 BackGroundTexturePosition { get; set; }


        Inventory inventory;

        public Sprite DragSprite { get; set; }
        public Texture2D DragSpriteTexture { get; set; }
        public bool DragToggle { get; set; }



        public int currentSliderPosition = 1;
        public bool WasSliderUpdated = false;

        public List<ActionTimer> AllActions;
        TextBuilder TextBuilder;

        public bool IsActive { get; set; } = true;

        public bool IsAnySlotHovered { get; set; } = false;

        public ToolBar(GraphicsDevice graphicsDevice, ContentManager content)
        {
            BackGroundTexturePosition = new Vector2(320, 635);


            this.graphicsDevice = graphicsDevice;
            this.content = content;

            //--------------------------------------
            //Initialize Textur


            InGameMenu = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(80, 80, 64, 64), graphicsDevice, new Vector2(367, 635),CursorType.Normal);
            OpenInventory = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(192, 16, 32, 32), graphicsDevice, new Vector2(459, 645), CursorType.Normal);
            ScrollTree = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(192, 16, 32, 32), graphicsDevice, new Vector2(200, 645), CursorType.Normal);
            AllSlots = new List<Button>();
            
            for (int i = 0; i < 7; i++)
            {
                AllSlots.Add(new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 64, 64), graphicsDevice, new Vector2(500 + i * 65, 635), CursorType.Normal) { ItemCounter = 0, Index = i + 1 });
            }



            AllNonInventoryButtons = new List<Button>()
            {
                OpenInventory,
                InGameMenu,
                ScrollTree,
                
            };
           

            //DragSprite = new Sprite(graphicsDevice, content, ToolBarButton, new Vector2(500f, 500f), false, .5f);

            AllActions = new List<ActionTimer>();
            TextBuilder = new TextBuilder("", .01f, 5);

        }

        public void Update(GameTime gameTime, Inventory inventory, MouseManager mouse)
        {
            UpdateScrollWheel(mouse);
            TextBuilder.Update(gameTime);
            IsAnySlotHovered = false;

            if (WasSliderUpdated && inventory.currentInventory.ElementAt(currentSliderPosition - 1).SlotItems.Count > 0)
            {

                ItemSwitchSourceRectangle = GetCurrentItemTexture();
            }
            else if (WasSliderUpdated && inventory.currentInventory.ElementAt(currentSliderPosition - 1).SlotItems.Count <= 0)
            {
                ItemSwitchSourceRectangle = new Rectangle(80, 0, 1, 1);
            }

            this.inventory = inventory;

            UpdateNonInventoryButtons(mouse);

            UpdateInventoryButtons(inventory, gameTime, mouse);


            if (mouse.IsHovering(BackGroundTextureRectangle))
            {
                MouseOverToolBar = true;
            }
            else
            {
                MouseOverToolBar = false;
            }

            if (this.WasSliderUpdated && GetCurrentEquippedTool() != 666)
            {
                AllActions.Add(new ActionTimer(1, AllActions.Count - 1));
            }

            for (int i = 0; i < AllActions.Count; i++)
            {
                AllActions[i].Update(gameTime, AllActions);
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

            if (currentSliderPosition > 7)
            {
                currentSliderPosition = 7;
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

        #region NONINVENTORYBUTTONS


        public void UpdateNonInventoryButtons(MouseManager mouse)
        {
            OpenInventory.isClicked = false;
            for (int i = 0; i < AllNonInventoryButtons.Count; i++)
            {
                AllNonInventoryButtons[i].Update(mouse);
            }

            if (InGameMenu.isClicked)
            {

                Game1.Player.UserInterface.CurrentOpenInterfaceItem = ExclusiveInterfaceItem.EscMenu;
            }
            else if (OpenInventory.isClicked)
            {
                Game1.Player.UserInterface.ScrollTree.IsActive = !Game1.Player.UserInterface.ScrollTree.IsActive;
            }
            else if(ScrollTree.isClicked)
            {

            }
            
        }
        #endregion

        public int GetCurrentEquippedTool()
        {
            if (inventory.currentInventory.ElementAt(currentSliderPosition - 1).SlotItems.Count > 0)
            {
                return inventory.currentInventory.ElementAt(currentSliderPosition - 1).GetItem().ID;
            }
            else
            {
                return -50; //Placeholder
            }
        }

        public Item GetCurrentEquippedToolAsItem()
        {
            if (inventory!= null && inventory.currentInventory.ElementAt(currentSliderPosition - 1).SlotItems.Count > 0)
            {
                return inventory.currentInventory.ElementAt(currentSliderPosition - 1).GetItem();
            }
            else
            {
                return null; 
            }
        }

        public Rectangle GetCurrentItemTexture()
        {
            return inventory.currentInventory.ElementAt(currentSliderPosition - 1).GetItem().SourceTextureRectangle;
        }

        public void UpdateInventoryButtons(Inventory inventory, GameTime gameTime, MouseManager mouse)
        {

            for (int i = 0; i < 7; i++)
            {
                if (inventory.currentInventory.ElementAt(i) == null)
                {
                    AllSlots[i].ItemCounter = 0;

                }
                else
                {
                    AllSlots[i].ItemCounter = inventory.currentInventory.ElementAt(i).SlotItems.Count;
                }

                if (AllSlots[i].ItemCounter > 0)
                {
                    AllSlots[i].Texture = inventory.currentInventory.ElementAt(i).SlotItems[0].ItemSprite.AtlasTexture;
                    AllSlots[i].ItemSourceRectangleToDraw = inventory.currentInventory.ElementAt(i).SlotItems[0].SourceTextureRectangle;
                }
                else
                {
                    AllSlots[i].Texture = Game1.AllTextures.UserInterfaceTileSet;
                    AllSlots[i].ItemSourceRectangleToDraw = new Rectangle(0, 80, 32, 32);
                }

                AllSlots[i].Update(mouse);
            }
            int buttonIndex = 0;
            for (int i = 0; i < 7; i++)
            {

                if (AllSlots[i].IsHovered && AllSlots[i].ItemCounter > 0)
                {
                    IsAnySlotHovered = true;
                    buttonIndex = i;


                }

                //INTERACTIONS WITH RELEASE ITEM 
                if (AllSlots[i].wasJustReleased && AllSlots[i].ItemCounter > 0)
                {
                    Item tempItem = inventory.currentInventory[i].GetItem();
                    //FOR WHEN DROPPING STACK OF ITEMS
                    if(Game1.Player.UserInterface.IsAnyChestOpen)
                    {
                        if(Game1.GetCurrentStage().AllTiles.Chests[Game1.Player.UserInterface.OpenChestKey].IsInventoryHovered)
                        {
                            if(Game1.GetCurrentStage().AllTiles.Chests[Game1.Player.UserInterface.OpenChestKey].Inventory.TryAddItem(inventory.currentInventory[i].GetItem()))
                            {
                                inventory.currentInventory[i].RemoveItemFromSlot();
                                AllSlots[i].ItemCounter--;
                            }
                        }
                    }
                    else if (Game1.Player.controls.pressedKeys.Contains(Keys.LeftShift) && !Game1.Player.UserInterface.IsShopMenu)
                    {
                        int currentItemCount = AllSlots[i].ItemCounter;
                        for (int j = 0; j < currentItemCount; j++)
                        {
                            inventory.currentInventory[i].RemoveItemFromSlot();
                            AllSlots[i].ItemCounter--;
                            if (!tempItem.IsPlaceable)
                            {
                                Item newWorldItem = Game1.ItemVault.GenerateNewItem(tempItem.ID, new Vector2(Game1.Player.Rectangle.X, Game1.Player.Rectangle.Y), true);
                                newWorldItem.IsTossable = true;
                                Game1.GetCurrentStage().AllItems.Add(newWorldItem);
                            }
                        }
                    }
                    //SHIFT CLICK SELL TO SHOP
                    else if (Game1.Player.controls.pressedKeys.Contains(Keys.LeftShift) && Game1.Player.UserInterface.CurrentOpenInterfaceItem == ExclusiveInterfaceItem.ShopMenu)
                    {
                        for (int s = 0; s < Game1.AllShops.Find(x => x.ID == (int)Game1.Player.UserInterface.CurrentOpenShop).ShopMenu.allShopMenuItemButtons.Count; s++)
                        {
                            if (Game1.AllShops.Find(x => x.ID == (int)Game1.Player.UserInterface.CurrentOpenShop).ShopMenu.allShopMenuItemButtons[s].IsHovered)
                            {

                                int currentItemCount = AllSlots[i].ItemCounter;

                                for (int d = 0; d < currentItemCount; d++)
                                {

                                    Game1.Player.Inventory.Money += Game1.AllShops.Find(x => x.ID == (int)Game1.Player.UserInterface.CurrentOpenShop).ShopMenu.TrySellToShop(inventory.currentInventory[i].GetItem().ID, 1);
                                    inventory.currentInventory[i].RemoveItemFromSlot();
                                    AllSlots[i].ItemCounter--;
                                }
                                Game1.SoundManager.Sell1.Play();

                                break;
                            }
                        }
                    }
                    //SELL SINGLE ITEM TO SHOP
                    else if (!Game1.Player.controls.pressedKeys.Contains(Keys.LeftShift) && Game1.Player.UserInterface.CurrentOpenInterfaceItem == ExclusiveInterfaceItem.ShopMenu)
                    {
                        for (int s = 0; s < Game1.AllShops.Find(x => x.ID == (int)Game1.Player.UserInterface.CurrentOpenShop).ShopMenu.allShopMenuItemButtons.Count; s++)
                        {
                            if (Game1.AllShops.Find(x => x.ID == (int)Game1.Player.UserInterface.CurrentOpenShop).ShopMenu.allShopMenuItemButtons[s].IsHovered)
                            {
                                Game1.Player.Inventory.Money += Game1.AllShops.Find(x => x.ID == (int)Game1.Player.UserInterface.CurrentOpenShop).ShopMenu.TrySellToShop(inventory.currentInventory[i].GetItem().ID, 1);
                                inventory.currentInventory[i].RemoveItemFromSlot();
                                AllSlots[i].ItemCounter--;
                                Game1.SoundManager.Sell1.Play();
                                break;
                            }
                        }
                    }
                    else
                    {
                        inventory.currentInventory[i].RemoveItemFromSlot();
                        AllSlots[i].ItemCounter--;
                        if (tempItem.IsPlaceable == false)
                        {
                            Item newWorldItem = Game1.ItemVault.GenerateNewItem(tempItem.ID, new Vector2(Game1.Player.Rectangle.X, Game1.Player.Rectangle.Y), true);
                            newWorldItem.IsTossable = true;
                            Game1.GetCurrentStage().AllItems.Add(newWorldItem);
                        }

                    }

                    DragSprite = null;
                }

                if (AllSlots[i].isClickedAndHeld == true && AllSlots[i].ItemCounter != 0)
                {
                    Item tempItem = inventory.currentInventory[i].GetItem();
                    if (tempItem.IsPlaceable == true)
                    {

                    }

                    else
                    {
                        Sprite tempSprite = new Sprite(graphicsDevice, Game1.AllTextures.ItemSpriteSheet, Game1.Player.Inventory.currentInventory.ElementAt(i).SlotItems[0].SourceTextureRectangle,
                            mouse.WorldMousePosition, 16, 16)
                        { IsBeingDragged = true, TextureScaleX = 2f, TextureScaleY = 2f };
                        DragSprite = tempSprite;

                    }

                }

                if(AllSlots[i].isClicked)
                {
                    currentSliderPosition = i + 1;
                }
                

                if (AllSlots[i].isClickedAndHeld && AllSlots[i].ItemCounter != 0)
                {
                    if (DragSprite != null)
                    {
                        DragSprite.Update(gameTime, mouse.UIPosition);
                    }

                }

            }
            if (IsAnySlotHovered)
            {
                TextBuilder.Activate(false, TextBoxType.normal, false, inventory.currentInventory[buttonIndex].GetItem().Name, 1f,
                    new Vector2(AllSlots[buttonIndex].Position.X, AllSlots[buttonIndex].Position.Y - 32), 200f);
            }
            //else
            //{
            //    TextBuilder.Reset();
            //}

        }


       

        public void Draw(SpriteBatch spriteBatch)
        {

            TextBuilder.Draw(spriteBatch, .75f);


            for (int i = 0; i < 7; i++)
            {
                if (AllSlots[i].isClickedAndHeld && AllSlots[i].ItemCounter != 0)
                {
                    if (DragSprite != null)
                    {
                        DragSprite.Draw(spriteBatch, .72f);
                    }

                }
            }


            OpenInventory.Draw(spriteBatch, Game1.AllTextures.MenuText, "Inv", new Vector2(450, 660), Color.CornflowerBlue, .69f, .7f);
            InGameMenu.Draw(spriteBatch, Game1.AllTextures.MenuText, "Menu", new Vector2(377, 660), Color.CornflowerBlue, .69f, .7f);
            for (int i = 0; i < AllSlots.Count; i++)
            {
                AllSlots[i].Draw(spriteBatch, AllSlots[i].ItemSourceRectangleToDraw, AllSlots[i].BackGroundSourceRectangle, Game1.AllTextures.MenuText, AllSlots[i].ItemCounter.ToString(), new Vector2(AllSlots[i].Position.X + 5, AllSlots[i].Position.Y + 5), Color.DarkRed, 2f);
            }
            spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle((int)AllSlots[currentSliderPosition - 1].Position.X, (int)AllSlots[currentSliderPosition - 1].Position.Y, 68, 67), new Rectangle(80, 0, 68, 67),
                Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .71f);

        }

        //use this when we want to draw relative to another camera.
        //for drawing items above the players head when item is switched
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
