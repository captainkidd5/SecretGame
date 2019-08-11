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
    }

    public class ToolBar
    {
        //--------------------------------------
        //Textures
        public Texture2D Background { get; set; }
        public Button InGameMenu { get; set; }
        public Button OpenInventory { get; set; }
        public Button InvSlot1 { get; set; }
        public Button InvSlot2 { get; set; }
        public Button InvSlot3 { get; set; }
        public Button InvSlot4 { get; set; }
        public Button InvSlot5 { get; set; }
        public Button InvSlot6 { get; set; }
        public Button InvSlot7 { get; set; }


        public Texture2D ToolBarButton { get; set; }
        public SpriteFont Font { get; set; }
        public List<Button> AllNonInventoryButtons { get; set; }
        public List<Button> AllSlots { get; set; }


        public Rectangle ItemSwitchSourceRectangle { get; set; }

        public Texture2D ItemSwitchTexture;

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

        public bool DragToggleBuilding { get; set; } = false;
        public bool DragoToggleBuildingDropped { get; set; } = false;

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
            //initialize SpriteFonts
            Font = content.Load<SpriteFont>("SpriteFont/MenuText");

            //--------------------------------------
            //Initialize Textures
            this.ToolBarButton = content.Load<Texture2D>("Button/ToolBarButton");
            this.Background = content.Load<Texture2D>("Button/ToolBar");

            this.BackGroundTextureRectangle = new Rectangle((int)BackGroundTexturePosition.X, (int)BackGroundTexturePosition.Y, Background.Width, Background.Height);

            //


            //--------------------------------------
            //Initialize Buttons
            InGameMenu = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(80, 80, 64, 64), graphicsDevice, new Vector2(367, 635));
            OpenInventory = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(192, 16, 32, 32), graphicsDevice, new Vector2(459, 645));
            InvSlot1 = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 32, 32), graphicsDevice, new Vector2(500, 635)) { ItemCounter = 0, Index = 1, BackGroundSourceRectangle = new Rectangle(208, 80, 32, 32) };
            InvSlot2 = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 32, 32), graphicsDevice, new Vector2(565, 635)) { ItemCounter = 0, Index = 2, BackGroundSourceRectangle = new Rectangle(208, 80, 32, 32) };
            InvSlot3 = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 32, 32), graphicsDevice, new Vector2(630, 635)) { ItemCounter = 0, Index = 3, BackGroundSourceRectangle = new Rectangle(208, 80, 32, 32) };
            InvSlot4 = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 32, 32), graphicsDevice, new Vector2(695, 635)) { ItemCounter = 0, Index = 4, BackGroundSourceRectangle = new Rectangle(208, 80, 32, 32) };
            InvSlot5 = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 32, 32), graphicsDevice, new Vector2(765, 635)) { ItemCounter = 0, Index = 5, BackGroundSourceRectangle = new Rectangle(208, 80, 32, 32) };
            InvSlot6 = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 32, 32), graphicsDevice, new Vector2(830, 635)) { ItemCounter = 0, Index = 6, BackGroundSourceRectangle = new Rectangle(208, 80, 32, 32) };
            InvSlot7 = new Button(Game1.AllTextures.UserInterfaceTileSet, new Rectangle(208, 80, 32, 32), graphicsDevice, new Vector2(895, 635)) { ItemCounter = 0, Index = 7, BackGroundSourceRectangle = new Rectangle(208, 80, 32, 32) };

            //--------------------------------------
            //Button List Stuff
            AllNonInventoryButtons = new List<Button>()
            {
                OpenInventory,
                InGameMenu
            };
            AllSlots = new List<Button>()
            {
                InvSlot1,
                InvSlot2,
                InvSlot3,
                InvSlot4,
                InvSlot5,
                InvSlot6,
                InvSlot7

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


            //--------------------------------------
            //Switch GameStages on click



            if (mouse.IsHovering(BackGroundTextureRectangle))
            {
                MouseOverToolBar = true;
            }
            if (!mouse.IsHovering(BackGroundTextureRectangle))
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
            for (int i = 0; i < AllNonInventoryButtons.Count; i++)
            {
                AllNonInventoryButtons[i].Update(mouse);
            }

            if (InGameMenu.isClicked)
            {

                Game1.Player.UserInterface.IsEscMenu = !Game1.Player.UserInterface.IsEscMenu;
            }
            else if (OpenInventory.isClicked)
            {

            }
            OpenInventory.isClicked = false;
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
                return -50; //Placeholder, kinda funny I guess
            }
        }

        public Item GetCurrentEquippedToolAsItem()
        {
            if (inventory.currentInventory.ElementAt(currentSliderPosition - 1).SlotItems.Count > 0)
            {
                return inventory.currentInventory.ElementAt(currentSliderPosition - 1).GetItem();
            }
            else
            {
                return null; //Placeholder, kinda funny I guess
            }
        }

        public Rectangle GetCurrentItemTexture()
        {
            return inventory.currentInventory.ElementAt(currentSliderPosition - 1).GetItem().SourceTextureRectangle;
        }

        public void UpdateInventoryButtons(Inventory inventory, GameTime gameTime, MouseManager mouse)
        {

            DragToggleBuilding = false;
            DragoToggleBuildingDropped = false;
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

                if (AllSlots[i].ItemCounter != 0)
                {
                    AllSlots[i].Texture = inventory.currentInventory.ElementAt(i).SlotItems[0].Texture;
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

                //else
                //{
                //    TextBuilder.StringToWrite = "";
                //    TextBuilder.IsActive = false;
                //}
                if (AllSlots[i].wasJustReleased == true && AllSlots[i].ItemCounter > 0)
                {
                    Item tempItem = inventory.currentInventory[i].GetItem();
                    //FOR WHEN DROPPING STACK OF ITEMS
                    if (Game1.Player.controls.pressedKeys.Contains(Keys.LeftShift) && !Game1.Player.UserInterface.IsShopMenu)
                    {
                        int currentItemCount = AllSlots[i].ItemCounter;
                        for (int j = 0; j < currentItemCount; j++)
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
                    }
                    //SHIFT CLICK SELL TO SHOP
                    else if (Game1.Player.controls.pressedKeys.Contains(Keys.LeftShift) && Game1.Player.UserInterface.IsShopMenu)
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

                                break;
                            }
                        }
                    }
                    //SELL SINGLE ITEM TO SHOP
                    else if (!Game1.Player.controls.pressedKeys.Contains(Keys.LeftShift) && Game1.Player.UserInterface.IsShopMenu)
                    {
                        for (int s = 0; s < Game1.AllShops.Find(x => x.ID == (int)Game1.Player.UserInterface.CurrentOpenShop).ShopMenu.allShopMenuItemButtons.Count; s++)
                        {
                            if (Game1.AllShops.Find(x => x.ID == (int)Game1.Player.UserInterface.CurrentOpenShop).ShopMenu.allShopMenuItemButtons[s].IsHovered)
                            {
                                Game1.Player.Inventory.Money += Game1.AllShops.Find(x => x.ID == (int)Game1.Player.UserInterface.CurrentOpenShop).ShopMenu.TrySellToShop(inventory.currentInventory[i].GetItem().ID, 1);
                                inventory.currentInventory[i].RemoveItemFromSlot();
                                AllSlots[i].ItemCounter--;
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

                        if (tempItem.IsPlaceable == true)
                        {

                            DragoToggleBuildingDropped = true;
                        }
                    }

                    DragSprite = null;
                }

                if (AllSlots[i].isClickedAndHeld == true && AllSlots[i].ItemCounter != 0)
                {
                    Item tempItem = inventory.currentInventory[i].GetItem();
                    if (tempItem.IsPlaceable == true)
                    {
                        //DragToggleBuilding = true;
                        //this.TempItem = tempItem;
                        //int j = 0;
                        //int k = 0;

                        //for (j = 0; j < tempItem.Building.TotalTiles.GetLength(0); j++)
                        //{
                        //    for (k = 0; k < tempItem.Building.TotalTiles.GetLength(1); k++)
                        //    {
                        //        Game1.GetCurrentStage().AllTiles.ReplaceTileTemporary(4, Game1.GetCurrentStage().AllTiles.CurrentIndexX + k, Game1.GetCurrentStage().AllTiles.CurrentIndexY + j, tempItem.Building.TotalTiles[j, k], .5f, j, k);
                        //    }
                        //}

                    }

                    else
                    {
                        Sprite tempSprite = new Sprite(graphicsDevice, Game1.AllTextures.ItemSpriteSheet, Game1.Player.Inventory.currentInventory.ElementAt(i).SlotItems[0].SourceTextureRectangle,
                            mouse.WorldMousePosition, 16, 16)
                        { IsBeingDragged = true, TextureScaleX = 2f, TextureScaleY = 2f };
                        DragSprite = tempSprite;

                    }

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
            else
            {
                TextBuilder.Reset();
            }

        }

        public void MiniDrawTiles(int[,] GIDArray, SpriteBatch spriteBatch, MouseManager mouse)
        {
            for (int i = 0; i < GIDArray.GetLength(0); i++)
            {
                for (int j = 0; j < GIDArray.GetLength(1); j++)
                {
                    Tile tempTile = new Tile(mouse.MouseSquareCoordinateX + j, mouse.MouseSquareCoordinateY + i, GIDArray[i, j], 100, 100, 100, 100);
                    spriteBatch.Draw(Game1.GetCurrentStage().TileSet, tempTile.DestinationRectangle, tempTile.SourceRectangle, Color.White * .5f, (float)0, new Vector2(0, 0), SpriteEffects.None, 1);
                }

            }
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


            OpenInventory.Draw(spriteBatch, Font, "Inv", new Vector2(450, 660), Color.CornflowerBlue, .69f, .7f);
            InGameMenu.Draw(spriteBatch, Font, "Menu", new Vector2(377, 660), Color.CornflowerBlue, .69f, .7f);
            for (int i = 0; i < AllSlots.Count; i++)
            {
                AllSlots[i].Draw(spriteBatch, AllSlots[i].ItemSourceRectangleToDraw, AllSlots[i].BackGroundSourceRectangle, Font, AllSlots[i].ItemCounter.ToString(), new Vector2(AllSlots[i].Position.X + 5, AllSlots[i].Position.Y + 5), Color.DarkRed, 2f);
            }

            switch (currentSliderPosition)
            {
                case 1:
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle((int)InvSlot1.Position.X, (int)InvSlot1.Position.Y, 68, 67),
                        new Rectangle(80, 0, 68, 67), Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .71f);
                    break;

                case 2:
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle((int)InvSlot2.Position.X, (int)InvSlot2.Position.Y, 68, 67),
                        new Rectangle(80, 0, 68, 67), Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .71f);
                    break;

                case 3:
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle((int)InvSlot3.Position.X, (int)InvSlot3.Position.Y, 68, 67),
                         new Rectangle(80, 0, 68, 67), Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .71f);
                    break;

                case 4:
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle((int)InvSlot4.Position.X, (int)InvSlot4.Position.Y, 68, 67),
                        new Rectangle(80, 0, 68, 67), Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .71f);
                    break;

                case 5:
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle((int)InvSlot5.Position.X, (int)InvSlot5.Position.Y, 68, 67),
                        new Rectangle(80, 0, 68, 67), Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .71f);
                    break;

                case 6:
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle((int)InvSlot6.Position.X, (int)InvSlot6.Position.Y, 68, 67),
                        new Rectangle(80, 0, 68, 67), Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .71f);
                    break;

                case 7:
                    spriteBatch.Draw(Game1.AllTextures.UserInterfaceTileSet, new Rectangle((int)InvSlot7.Position.X, (int)InvSlot7.Position.Y, 68, 67),
                                            new Rectangle(80, 0, 68, 67), Color.White, 0f, Game1.Utility.Origin, SpriteEffects.None, .71f);
                    break;
            }



            // if(DragToggleBuilding)
            // {
            //    MiniDrawTiles(TempItem.Building.TotalTiles, spriteBatch);
            // }

            // spriteBatch.End();
        }

        //use this when we want to draw relative to another camera.
        public void DrawToStageMatrix(SpriteBatch spriteBatch)
        {
            //if action still exists and isn't complete we'll still draw it. 
            if (AllActions.Count > 0 && !AllActions[AllActions.Count - 1].ActionComplete)
            {
                spriteBatch.Draw(Game1.AllTextures.ItemSpriteSheet, sourceRectangle: this.ItemSwitchSourceRectangle, destinationRectangle: new Rectangle((int)Game1.Player.position.X + 3,
                    (int)Game1.Player.position.Y - 15, 16, 16), color: Color.White, layerDepth: 1, scale: new Vector2(1f, 1f));
            }

        }

        public void DrawDraggableItems(SpriteBatch spriteBatch, TileManager buildingsTiles, TileManager foreGroundTiles, MouseManager mouse)
        {

            //if (DragToggleBuilding)
            //{
            //    MiniDrawTiles(TempItem.Building.TotalTiles, spriteBatch, mouse);

            //}
            //if (DragoToggleBuildingDropped == true)
            //{
            //    for (int i = 0; i < TempItem.Building.BuildingID.Length; i++)
            //    {

            //        Tile TempTile;
            //        TempTile = new Tile(mouse.MouseSquareCoordinateX + i, mouse.MouseSquareCoordinateY + 1, TempItem.Building.BuildingID[i], 100, 100, 100, 100);
            //        buildingsTiles.Tiles[mouse.MouseSquareCoordinateX + i + 1, mouse.MouseSquareCoordinateY] = TempTile;
            //        buildingsTiles.AddObjectToBuildingTile(TempTile, mouse.MouseSquareCoordinateX + i + 1, mouse.MouseSquareCoordinateY);


            //    }
            //    for (int j = 0; j < TempItem.Building.ForeGroundID.Length; j++)
            //    {
            //        Tile TempTile;
            //        TempTile = new Tile(mouse.MouseSquareCoordinateX + j, mouse.MouseSquareCoordinateY, TempItem.Building.ForeGroundID[j], 100, 100, 100, 100);
            //        foreGroundTiles.Tiles[mouse.MouseSquareCoordinateX + j + 1, mouse.MouseSquareCoordinateY] = TempTile;
            //    }
            //}

        }
    }
}
