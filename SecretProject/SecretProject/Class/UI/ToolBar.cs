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
using SecretProject.Class.ItemStuff;
using SecretProject.Class.MenuStuff;
using SecretProject.Class.SpriteFolder;
using SecretProject.Class.Stage;
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

    class ToolBar
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

        public Texture2D InvSlot1Texture { get; set; }
        public Texture2D InvSlot2Texture { get; set; }
        public Texture2D InvSlot3Texture { get; set; }
        public Texture2D InvSlot4Texture { get; set; }
        public Texture2D InvSlot5Texture { get; set; }
        public Texture2D InvSlot6Texture { get; set; }
        public Texture2D InvSlot7Texture { get; set; }

        public Texture2D ItemSwitchTexture;

        public Item TempItem { get; set; }

        public Rectangle BackGroundTextureRectangle { get; set; }

        public bool MouseOverToolBar { get; set; }

        public buttonIsClicked toolBarState = buttonIsClicked.none;
        Game1 game;
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

        public ToolBar(Game1 game, GraphicsDevice graphicsDevice, ContentManager content )
        {
            BackGroundTexturePosition = new Vector2(320, 635);



            this.game = game;
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
            InGameMenu = new Button(ToolBarButton, graphicsDevice, new Vector2(367, 635));
            OpenInventory = new Button(ToolBarButton, graphicsDevice, new Vector2(433, 635));
            InvSlot1 = new Button(ToolBarButton, graphicsDevice, new Vector2(500, 635)) { ItemCounter = 0, Index = 1 };
            InvSlot2 = new Button(ToolBarButton, graphicsDevice, new Vector2(565, 635)) { ItemCounter = 0, Index = 2 };
            InvSlot3 = new Button(ToolBarButton, graphicsDevice, new Vector2(630, 635)) { ItemCounter = 0, Index = 3 };
            InvSlot4 = new Button(ToolBarButton, graphicsDevice, new Vector2(695, 635)) { ItemCounter = 0, Index = 4 };
            InvSlot5 = new Button(ToolBarButton, graphicsDevice, new Vector2(765, 635)) { ItemCounter = 0, Index = 5 };
            InvSlot6 = new Button(ToolBarButton, graphicsDevice, new Vector2(830, 635)) { ItemCounter = 0, Index = 6 };
            InvSlot7 = new Button(ToolBarButton, graphicsDevice, new Vector2(895, 635)) { ItemCounter = 0, Index = 7 };

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
        }

        public void Update(GameTime gameTime, Inventory inventory, MouseManager mouse)
        {
            UpdateScrollWheel(mouse);

            if(WasSliderUpdated && inventory.currentInventory.ElementAt(currentSliderPosition - 1).SlotItems.Count > 0)
            {
                ItemSwitchTexture = GetCurrentItemTexture();
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

            if(this.WasSliderUpdated && GetCurrentEquippedTool() != 666)
            {
                AllActions.Add(new ActionTimer(1, AllActions.Count - 1));
            }

            for(int i = 0; i < AllActions.Count; i++)
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

            if(oldSliderPosition != currentSliderPosition)
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

                UserInterface.IsEscMenu = !UserInterface.IsEscMenu;
            }
            else if (OpenInventory.isClicked)
            {

            }
            OpenInventory.isClicked = false;
        }
        #endregion

        public int GetCurrentEquippedTool()
        {
            if(inventory.currentInventory.ElementAt(currentSliderPosition - 1).SlotItems.Count > 0)
            {
                return inventory.currentInventory.ElementAt(currentSliderPosition - 1).GetItem().ID;
            }
            else
            {
                return 666; //Placeholder, kinda funny I guess
            }
        }

        public Texture2D GetCurrentItemTexture()
        {
                return inventory.currentInventory.ElementAt(currentSliderPosition - 1).GetItem().Texture;
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
                }
                else
                {
                    AllSlots[i].Texture = ToolBarButton;
                }

                AllSlots[i].Update(mouse);
            }

            for (int i = 0; i < 7; i++)
            {

                if (AllSlots[i].wasJustReleased == true && AllSlots[i].ItemCounter > 0)
                {
                    Item tempItem = inventory.currentInventory[i].GetItem();
                    inventory.currentInventory[i].RemoveItemFromSlot();
                    AllSlots[i].ItemCounter--;
                    if (tempItem.IsPlaceable == false)
                    {
                        Game1.GetCurrentStage().allItems.Add(Game1.ItemVault.GenerateNewItem(tempItem.ID, mouse.WorldMousePosition, true));
                    }

                    if (tempItem.IsPlaceable == true)
                    {
                       // Iliad.allItems.Add(new WorldItem(tempItem.Name, graphicsDevice, content, mouse.WorldMousePosition));

                        DragoToggleBuildingDropped = true;
                    }

                    //tempItem.ItemSprite.Color = Color.White;
                    DragSprite = null;
                }

                if (AllSlots[i].isClickedAndHeld == true && AllSlots[i].ItemCounter != 0)
                {
                    Item tempItem = inventory.currentInventory[i].GetItem();
                    if (tempItem.IsPlaceable == true)
                    {
                        DragToggleBuilding = true;
                        this.TempItem = tempItem;
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // tempItem.ItemSprite.Color = Color.White * .5f;
                        //Draw building tiles of placeable object
                      //  int j = 0;
                       // int k = 0;

                       // for (j = 0; j < tempItem.Building.TotalTiles.GetLength(0); j++)
                       // {
                        //    for (k = 0; k < tempItem.Building.TotalTiles.GetLength(1); k++)
                         //   {
                         //       Iliad.PlacementTiles.ReplaceTileTemporary(Iliad.PlacementTiles.CurrentIndexX + k, Iliad.PlacementTiles.CurrentIndexY + j, tempItem.Building.TotalTiles[j, k], .5f, j, k);
                           // }
                      //  }

                    }

                    else
                    {
                        Sprite tempSprite = new Sprite(graphicsDevice, content, Game1.Player.Inventory.currentInventory.ElementAt(i).SlotItems[0].Texture, mouse.WorldMousePosition, false, .5f) { IsBeingDragged = true, ScaleX = 3f, ScaleY = 3f };
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
        }

        public void MiniDrawTiles(int[,] GIDArray, SpriteBatch spriteBatch, MouseManager mouse)
        {
            for (int i = 0; i < GIDArray.GetLength(0); i++)
            {
                for(int j = 0; j < GIDArray.GetLength(1); j++)
                {
                    Tile tempTile = new Tile(mouse.MouseSquareCoordinateX + j , mouse.MouseSquareCoordinateY  + i, GIDArray[i, j], 100, 100, 100, 100, 0);
                    spriteBatch.Draw(Game1.GetCurrentStage().TileSet, tempTile.DestinationRectangle, tempTile.SourceRectangle, Color.White * .5f, (float)0, new Vector2(0, 0), SpriteEffects.None, 1);
                }
                
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

            //--------------------------------------
            //Draw Background
            spriteBatch.Draw(Background, BackGroundTexturePosition, layerDepth: .4f);

            for(int i = 0; i < 7; i++)
            {
                if (AllSlots[i].isClickedAndHeld && AllSlots[i].ItemCounter != 0)
                {
                    if(DragSprite != null)
                    {
                        DragSprite.Draw(spriteBatch, .5f);
                    }
                    
                }
            }

            if(InvSlot1.isClickedAndHeld && InvSlot1.ItemCounter != 0)
            {
                if(DragSprite != null)
                {
                    DragSprite.Draw(spriteBatch, .5f);
                }
                
            }

            //--------------------------------------
            //Draw Buttons
            OpenInventory.Draw(spriteBatch, Font, "Inv", new Vector2(450, 660), Color.CornflowerBlue);
            InGameMenu.Draw(spriteBatch, Font, "Menu", new Vector2(377, 660), Color.CornflowerBlue);
            InvSlot1.Draw(spriteBatch, Font, InvSlot1.ItemCounter.ToString(), new Vector2(543, 670), Color.DarkRed);
            InvSlot2.Draw(spriteBatch, Font, InvSlot2.ItemCounter.ToString(), new Vector2(600, 670), Color.DarkRed);
            InvSlot3.Draw(spriteBatch, Font, InvSlot3.ItemCounter.ToString(), new Vector2(670, 670), Color.DarkRed);
            InvSlot4.Draw(spriteBatch, Font, InvSlot4.ItemCounter.ToString(), new Vector2(730, 670), Color.DarkRed);
            InvSlot5.Draw(spriteBatch, Font, InvSlot5.ItemCounter.ToString(), new Vector2(810, 670), Color.DarkRed);
            InvSlot6.Draw(spriteBatch, Font, InvSlot6.ItemCounter.ToString(), new Vector2(870, 670), Color.DarkRed);
            InvSlot7.Draw(spriteBatch, Font, InvSlot7.ItemCounter.ToString(), new Vector2(940, 670), Color.DarkRed);

            switch(currentSliderPosition)
            {
                case 1:
                    spriteBatch.Draw(Game1.AllTextures.ToolBarButtonSelector, InvSlot1.Position, Color.White);
                    break;

                case 2:
                    spriteBatch.Draw(Game1.AllTextures.ToolBarButtonSelector, InvSlot2.Position, Color.White);
                    break;

                case 3:
                    spriteBatch.Draw(Game1.AllTextures.ToolBarButtonSelector, InvSlot3.Position, Color.White);
                    break;

                case 4:
                    spriteBatch.Draw(Game1.AllTextures.ToolBarButtonSelector, InvSlot4.Position, Color.White);
                    break;

                case 5:
                    spriteBatch.Draw(Game1.AllTextures.ToolBarButtonSelector, InvSlot5.Position, Color.White);
                    break;

                case 6:
                    spriteBatch.Draw(Game1.AllTextures.ToolBarButtonSelector, InvSlot6.Position, Color.White);
                    break;

                case 7:
                    spriteBatch.Draw(Game1.AllTextures.ToolBarButtonSelector, InvSlot7.Position, Color.White);
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
                spriteBatch.Draw(ItemSwitchTexture, new Vector2(Game1.GetCurrentStage().Player.position.X + 3,
                    Game1.GetCurrentStage().Player.position.Y - 15), color: Color.White, layerDepth: 1, scale: new Vector2(.5f, .5f));
            }

        }

        public void DrawDraggableItems(SpriteBatch spriteBatch, TileManager buildingsTiles, TileManager foreGroundTiles, MouseManager mouse)
        {
           
            if (DragToggleBuilding)
            {
                MiniDrawTiles(TempItem.Building.TotalTiles, spriteBatch, mouse);

            }
            if (DragoToggleBuildingDropped == true)
            {
                for (int i = 0; i < TempItem.Building.BuildingID.Length; i++)
                {

                    Tile TempTile;
                    TempTile = new Tile(mouse.MouseSquareCoordinateX + i, mouse.MouseSquareCoordinateY + 1, TempItem.Building.BuildingID[i], 100, 100, 100, 100, 0);
                    buildingsTiles.Tiles[mouse.MouseSquareCoordinateX + i + 1, mouse.MouseSquareCoordinateY] = TempTile;
                    buildingsTiles.AddObjectToBuildingTile(TempTile, mouse.MouseSquareCoordinateX + i + 1, mouse.MouseSquareCoordinateY);


                }
                for (int j = 0; j < TempItem.Building.ForeGroundID.Length; j++)
                {
                    Tile TempTile;
                    TempTile = new Tile(mouse.MouseSquareCoordinateX + j, mouse.MouseSquareCoordinateY, TempItem.Building.ForeGroundID[j], 100, 100, 100, 100, 0);
                    foreGroundTiles.Tiles[mouse.MouseSquareCoordinateX + j + 1, mouse.MouseSquareCoordinateY] = TempTile;
                }
            }
           
        }
    }
}
